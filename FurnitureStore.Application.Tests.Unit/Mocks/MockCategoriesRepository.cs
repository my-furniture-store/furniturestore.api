using FurnitureStore.Domain.Categories;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockCategoriesRepository
{
    public static ICategoriesRepository GetCategoriesRepository()
    {
        var mockRepo = Substitute.For<ICategoriesRepository>();

        mockRepo.GetAllCategoriesAsync().Returns(CategoriesFixture.GetTestCategories());
        mockRepo.AddCategoryAsync(Arg.Do<Category>(CategoriesFixture.GetTestCategories().Add));
        mockRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(x => CategoriesFixture.GetTestCategories().FirstOrDefault(c => c.Id == x.Arg<Guid>()));
        mockRepo.ExistsAsync(Arg.Any<Guid>()).Returns(x => CategoriesFixture.GetTestCategories().Any(c => c.Id == x.Arg<Guid>()));
        mockRepo.RemoveCategoryAsync(Arg.Do<Category>(x => CategoriesFixture.GetTestCategories().Remove(x)));
        return mockRepo;
    }
}
