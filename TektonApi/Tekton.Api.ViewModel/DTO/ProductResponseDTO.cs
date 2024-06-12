namespace Tekton.Api.ViewModel.DTO
{
    public class ProductResponseDTO
    {
        public long ProductId { get; set; }

        public string Name { get; set; } = null!;

        public bool Status { get; set; }

        public string StatusName { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal Discount { get; set; }
        
        public decimal FinalPrice { get; set; }
    }
}
