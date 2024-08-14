using BT.BrightMarket.Shared.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace BT.BrightMarket.Shared.DTOs
{
    public class UserDTO
    {
        public class PostUserObject
        {

            [Required(ErrorMessage = "E-mail is verplicht.")]
            [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Naam is verplicht.")]
            [MaxLength(50, ErrorMessage = "Naam moet minder dan of gelijk aan 50 tekens zijn.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "De rol mag niet leeg zijn.")]
            public Role Role { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "Regio moet worden geselecteerd.")]
            public int RegionId { get; set; }
        }
    }
}
 