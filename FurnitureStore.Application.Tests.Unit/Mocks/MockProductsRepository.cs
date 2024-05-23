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

        return mockRepo;
    }
}
