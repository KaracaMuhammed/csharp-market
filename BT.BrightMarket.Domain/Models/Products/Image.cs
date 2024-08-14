
using System.Text.Json.Serialization;

namespace BT.BrightMarket.Domain.Models.Products
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        [JsonIgnore] public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
