namespace BT.BrightMarket.Shared.Models.Products
{
    public class Image
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
