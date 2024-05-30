using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockProductsRepository
{
    public static IProductsRepository GetProductsRepository(Guid categoryId, Guid subCategoryId)
    {
        var products = ProductsFixture.GetListofProducts(categoryId, subCategoryId);
        var mockRepo = Substitute.For<IProductsRepository>();

        mockRepo.AddAsync(Arg.Do<Product>(p => products.Add(p))).Returns(Task.CompletedTask);

        mockRepo.GetAllAsync().Returns(Task.FromResult(products));

        mockRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(x => products.FirstOrDefault(product => product.Id == x.Arg<Guid>()));

        mockRepo.RemoveProductAsync(Arg.Do<Product>(p => products.Remove(p)));

        mockRepo.UpdateAsync(Arg.Any<Product>()).Returns(Task.CompletedTask).AndDoes(x =>
        {
            var product = x.Arg<Product>();

            products.Remove(product);

            var newProduct = ProductsFixture.CreateProduct(product);

            products.Add(newProduct);
        });


        mockRepo.GetProductsByCategoryIdAsync(Arg.Any<Guid>())
            .Returns(x => products.Where(product => product.CategoryId == x.Arg<Guid>()).ToList());

        mockRepo.GetProductsBySubCategoryIdAsync(Arg.Any<Guid>())
            .Returns(x => products.Where(product => product.SubCategoryId == x.Arg<Guid>()).ToList());

        mockRepo.GetFeaturedProductAsync()
            .Returns(x => products.Where(product => product.IsFeatured).ToList());
        return mockRepo;
    }
}
