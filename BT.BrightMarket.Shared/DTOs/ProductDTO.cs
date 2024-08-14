using BT.BrightMarket.Shared.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace BT.BrightMarket.Shared.DTOs
{
    public class ProductDTO
    {
        public class PostProductObject
        {
            [Required(ErrorMessage = "De productnaam mag niet leeg zijn.")]
            [StringLength(40, ErrorMessage = "De productnaam mag niet meer dan 40 tekens bevatten.")]
            public string Name { get; set; }

            [StringLength(450, ErrorMessage = "Beschrijving mag niet meer dan 450 tekens bevatten.")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Prijs is verplicht.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Prijs moet een hogere waarde zijn dan 0 met maximaal twee decimalen.")]
            public double Price { get; set; }

            [Required(ErrorMessage = "De productstatus mag niet leeg zijn.")]
            public Status Status { get; set; }

            [Required(ErrorMessage = "De categorie mag niet leeg zijn.")]
            [Range(0, int.MaxValue, ErrorMessage = "De categorie moet worden geselecteerd.")]
            public int CategoryId { get; set; }

            [EnumDataType(typeof(Duration), ErrorMessage = "Gelieve een geldige weergavetijd te selecteren.")]
            public Duration DisplayDuration { get; set; }

            [Required(ErrorMessage = "Minstens één afbeelding is vereist.")]
            public List<ImageDTO> Images { get; set; }

            [ItemTypeValidation]
            [EnumDataType(typeof(ItemType), ErrorMessage = "Gelieve een geldige aanbieding soort te selecteren.")]
            public ItemType ItemType { get; set; }

        }

        public class PostUpdateObject
        {
            [Required(ErrorMessage = "De productnaam mag niet leeg zijn.")]
            [StringLength(40, ErrorMessage = "De productnaam mag niet meer dan 40 tekens bevatten.")]
            public string Name { get; set; }

            [StringLength(450, ErrorMessage = "Beschrijving mag niet meer dan 450 tekens bevatten.")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Prijs is verplicht.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Prijs moet een hogere waarde zijn dan 0 met maximaal twee decimalen.")]
            public double Price { get; set; }

            [Required(ErrorMessage = "De productstatus mag niet leeg zijn.")]
            public Status Status { get; set; }

            [Required(ErrorMessage = "De categorie mag niet leeg zijn.")]
            [Range(0, int.MaxValue, ErrorMessage = "De categorie moet worden geselecteerd.")]
            public int CategoryId { get; set; }

            [EnumDataType(typeof(Duration), ErrorMessage = "Gelieve een geldige weergavetijd te selecteren.")]
            public Duration DisplayDuration { get; set; }

            [Required(ErrorMessage = "Minstens één afbeelding is vereist.")]
            public List<ImageDTO> Images { get; set; }

        }

    }

    public class ItemTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ItemType itemType && itemType == ItemType.Undefined)
            {
                return new ValidationResult("Gelieve een aanbieding soort te selecteren.");
            }

            return ValidationResult.Success;
        }
    }

}
