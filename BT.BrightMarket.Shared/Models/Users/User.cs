namespace BT.BrightMarket.Shared.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        //public List<Product> Products { get; set; }

        //public List<Conversation> Conversations { get; set; }
    }
}
