namespace Tekton.Api.ViewModel.DTO
{
    public class ProductRequestInsertDTO
    {
        public string Name { get; set; } = null!;

        public int Stock { get; set; }

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
