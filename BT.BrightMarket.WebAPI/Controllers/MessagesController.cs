using BT.BrightMarket.Application.CQRS.Messages;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.Domain.Models.Conversations;
using BT.BrightMarket.WebAPI.Hubs;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{
    public class MessagesController : APIv1Controller
    {
        private readonly IMediator mediator;
        private readonly IHubContext<ChatHub> chatHubContext;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly ConnectedClientsService connectedClientsService;
        public MessagesController(IMediator mediator, IHubContext<ChatHub> chatHubContext, IHubContext<NotificationHub> notificationHubContext, ConnectedClientsService connectedClientsService)
        {
            this.mediator = mediator;
            this.chatHubContext = chatHubContext;
            this.notificationHubContext = notificationHubContext;
            this.connectedClientsService = connectedClientsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMessage([FromBody] MessageDTO message)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim?.Value, out int authenticatedUserId))
            {
                Message processedMessage = await mediator.Send(new AddMessageCommand
                {
                    Text = message.Text,
                    ConversationId = message.ConversationId,
                    AuthenticatedUserId = authenticatedUserId
                });

                // Send message to the seller if active
                string sellerMessageSessionId = connectedClientsService.GetMessageSessionId(processedMessage.Conversation.Product.UserId);
                if (sellerMessageSessionId != null)
                    await chatHubContext.Clients.Client(sellerMessageSessionId).SendAsync("message", processedMessage);

                // Send message to the buyer if active
                string buyerMessageSessionId = connectedClientsService.GetMessageSessionId(processedMessage.Conversation.BuyerId);
                if (buyerMessageSessionId != null)
                    await chatHubContext.Clients.Client(buyerMessageSessionId).SendAsync("message", processedMessage);

                // Send notification to the buyer if active
                int recipientId = processedMessage.Conversation.Product.UserId == authenticatedUserId ? processedMessage.Conversation.BuyerId : processedMessage.Conversation.Product.UserId;

                string recipientNotificationSessionId = connectedClientsService.GetNotificationSessionId(recipientId);
                if (recipientNotificationSessionId != null)
                    await notificationHubContext.Clients.Client(recipientNotificationSessionId).SendAsync("notification", processedMessage);

                return Created("", processedMessage);
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpGet("{conversationId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByConversationId(int conversationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 15)
        {

            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                var messages = await mediator.Send(new GetPersonalMessagesByConversationIdQuery { ConversationId = conversationId, PageNumber = pageNumber, PageSize = pageSize, AuthenticatedUserId = authenticatedUserId });
                return Ok(messages);
            }
            else
            {
                return Unauthorized();
            }

        }

    }
}
