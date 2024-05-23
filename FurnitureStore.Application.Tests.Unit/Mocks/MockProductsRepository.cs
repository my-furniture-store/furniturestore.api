using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockProductsRepository
{
    public static IProductsRepository GetProductsRepository(Guid categoryId, Guid subCategoryId)
    {
        var mockRepo = Substitute.For<IProductsRepository>();

        mockRepo.AddAsync(Arg.Do<Product>(ProductsFixture.GetListofProducts(categoryId, subCategoryId).Add));

        return mockRepo;
    }
}
