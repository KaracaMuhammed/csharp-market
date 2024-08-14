using BT.BrightMarket.Domain.Models.Conversations;
using BT.BrightMarket.Domain.Models.Products;
using System.Text.Json.Serialization;

namespace BT.BrightMarket.Domain.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        [JsonIgnore] public List<Product> Products { get; set; }
        [JsonIgnore] public List<Conversation> Conversations { get; set; }

        //public List<Conversation> Conversations { get; set; }
    }
}
