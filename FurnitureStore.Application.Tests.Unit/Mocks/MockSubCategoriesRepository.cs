using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockSubCategoriesRepository
{
    public static ISubCategoriesRepository GetSubCategoriesRepository(Guid categoryId)
    {
        var mockRepo = Substitute.For<ISubCategoriesRepository>();

        mockRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(x=>SubCategoriesFixture.GetTestSubCategories(categoryId).FirstOrDefault(c => c.Id == c.Id));
        mockRepo.AddSubCategoryAsync(Arg.Do<SubCategory>(SubCategoriesFixture.GetTestSubCategories(categoryId).Add));
        mockRepo.RemoveSubCategoryAsync(Arg.Do<SubCategory>(x => SubCategoriesFixture.GetTestSubCategories(categoryId).Remove(x)));

        return mockRepo;
    }
}
