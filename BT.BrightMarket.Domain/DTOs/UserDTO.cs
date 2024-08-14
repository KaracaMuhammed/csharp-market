using BT.BrightMarket.Domain.Models.Users;

namespace BT.BrightMarket.Domain.DTOs
{
    public class UserDTO
    {
        public class PostUserObject
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public Role Role { get; set; }
            public int RegionId { get; set; }
        }
    }
}
 