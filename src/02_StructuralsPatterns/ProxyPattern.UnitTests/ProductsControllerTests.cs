using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProxyPattern.UnitTests
{

    [TestClass]
    public class ProductsControllerTests
    {
        private ProductsController productsController;

        [TestInitialize]
        public void Init()
        {
            productsController =  new ProductsController(
                new LoggerProductRepository( 
                                new CacheProductRepository(
                                    new DbProductRepository())));
        }

        [TestMethod]
        public void Get_FirstCall_ShouldCacheHitEqualZero()
        {
            // Arrange
            int productId = 1;

            // Act
            var request = productsController.Get(productId);

            // Assert
            Assert.AreEqual(0, request.CacheHit);
            Assert.AreEqual(1, request.Id);
            Assert.AreEqual("Product 1", request.Name);
        }

        [TestMethod]
        public void Get_TwiceCalls_ShouldCacheHitEqualOne()
        {
            // Arrange
            int productId = 1;
            productsController.Get(productId);

            // Act
            var request = productsController.Get(productId);

            // Assert
            Assert.IsNotNull(request);
            Assert.AreEqual(1, request.CacheHit);
            Assert.AreEqual(1, request.Id);
            Assert.AreEqual("Product 1", request.Name);
        }

        [TestMethod]
        public void Get_ManyCalls_ShouldCacheHitAboveZero()
        {
            // Arrange
            int productId = 1;
            productsController.Get(productId);
            productsController.Get(productId);
            productsController.Get(productId);

            // Act
            var request = productsController.Get(productId);

            // Assert
            Assert.IsNotNull(request);
            Assert.AreEqual(3, request.CacheHit);
            Assert.AreEqual(1, request.Id);
            Assert.AreEqual("Product 1", request.Name);
        }

        [TestMethod]
        public void Get_NotFound_ShouldReturnsEmpty()
        {
            // Arrange
            int productId = 999;

            // Act
            productsController.Get(productId);

            var request = productsController.Get(productId);

            // Assert
            Assert.IsNull(request);
        }
    }
}
