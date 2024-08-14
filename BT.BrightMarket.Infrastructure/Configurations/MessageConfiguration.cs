using BT.BrightMarket.Domain.Models.Conversations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.BrightMarket.Infrastructure.Configurations
{

    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            //builder
            //    .HasOne(m => m.Sender)
            //    .WithOne() // No navigation property in User class for Message
            //    .HasForeignKey<Message>(m => m.SenderId)
            //    .OnDelete(DeleteBehavior.SetNull);

        }
    }


    
}
