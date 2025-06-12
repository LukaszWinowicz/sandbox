namespace ProxyPattern
{
    public class ProductsController
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Product Get(int id)
        {
            var product = productRepository.Get(id);

            return product;
        }
    }


}
