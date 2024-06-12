namespace Tekton.Api.ViewModel.DTO
{
    public class ProductRequestUpdateDTO
    {
        public long ProductId { get; set; }

        public string Name { get; set; } = null!;

        public bool Status { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
