using System.Collections.Generic;

namespace BT.BrightMarket.WebAPI.Hubs
{
    public class ConnectedClientsService
    {
        public class UserSessionInfo
        {
            public string MessageSessionId { get; set; }
            public string NotificationSessionId { get; set; }
        }

        private static Dictionary<int, UserSessionInfo> connectedClients = new Dictionary<int, UserSessionInfo>();

        private static ConnectedClientsService instance;

        public ConnectedClientsService() { }

        public static ConnectedClientsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConnectedClientsService();
                }
                return instance;
            }
        }

        public void AddMessageSessionId(int userId, string messageSessionId)
        {
            if (connectedClients.ContainsKey(userId))
            {
                connectedClients[userId].MessageSessionId = messageSessionId;
            }
            else
            {
                connectedClients[userId] = new UserSessionInfo
                {
                    MessageSessionId = messageSessionId
                };
            }
        }

        public void AddNotificationSessionId(int userId, string notificationSessionId)
        {
            if (connectedClients.ContainsKey(userId))
            {
                connectedClients[userId].NotificationSessionId = notificationSessionId;
            }
            else
            {
                connectedClients[userId] = new UserSessionInfo
                {
                    NotificationSessionId = notificationSessionId
                };
            }
        }

        public void RemoveClientByUserId(int userId)
        {
            if (connectedClients.ContainsKey(userId))
            {
                connectedClients.Remove(userId);
            }
        }

        public void RemoveMessageSessionId(string messageSessionId)
        {
            foreach (var entry in connectedClients)
            {
                if (entry.Value != null && entry.Value.MessageSessionId == messageSessionId)
                {
                    entry.Value.MessageSessionId = null;
                }
            }
        }

        public void RemoveClientByNotificationSessionId(string notificationSessionId)
        {
            var userId = GetUserIdByNotificationSessionId(notificationSessionId);
            if (userId != null)
            {
                connectedClients.Remove((int)userId);
            }
        }

        public string GetMessageSessionId(int userId)
        {
            return connectedClients.ContainsKey(userId) ? connectedClients[userId].MessageSessionId : null;
        }

        public string GetNotificationSessionId(int userId)
        {
            return connectedClients.ContainsKey(userId) ? connectedClients[userId].NotificationSessionId : null;
        }

        public int? GetUserIdByMessageSessionId(string messageSessionId)
        {
            foreach (var entry in connectedClients)
            {
                if (entry.Value != null && entry.Value.MessageSessionId == messageSessionId)
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public int? GetUserIdByNotificationSessionId(string notificationSessionId)
        {
            foreach (var entry in connectedClients)
            {
                if (entry.Value != null && entry.Value.NotificationSessionId == notificationSessionId)
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public IEnumerable<KeyValuePair<int, UserSessionInfo>> GetAllClients()
        {
            return connectedClients;
        }

    }
}
