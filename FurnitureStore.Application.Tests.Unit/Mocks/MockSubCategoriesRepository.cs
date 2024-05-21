using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockSubCategoriesRepository
{
    public static ISubCategoriesRepository GetSubCategoriesRepository(Guid categoryId)
    {
        var mockRepo = Substitute.For<ISubCategoriesRepository>();

        mockRepo.ListByCategoryIdAsync(Arg.Any<Guid>())
            .Returns(x => SubCategoriesFixture.GetTestSubCategories(categoryId).Where(c =>c.CategoryId == x.Arg<Guid>()).ToList());
        mockRepo.GetByIdAsync(Arg.Any<Guid>())
            .Returns(x=>SubCategoriesFixture.GetTestSubCategories(categoryId).FirstOrDefault(c => c.Id == x.Arg<Guid>()));
        mockRepo.AddSubCategoryAsync(Arg.Do<SubCategory>(SubCategoriesFixture.GetTestSubCategories(categoryId).Add));
        mockRepo.RemoveSubCategoryAsync(Arg.Do<SubCategory>(x => SubCategoriesFixture.GetTestSubCategories(categoryId).Remove(x)));
        mockRepo.UpdateAsync(Arg.Any<SubCategory>()).Returns(Task.CompletedTask).AndDoes(x =>
        {
            var updatedSubCategory = x.Arg<SubCategory>();
            var existingSubCategory = SubCategoriesFixture.GetTestSubCategories(categoryId).FirstOrDefault(c => c.Id == updatedSubCategory.Id);
            if(existingSubCategory != null)
            {
                existingSubCategory.UpdateSubCategory(updatedSubCategory.Name);
            }
        });


        return mockRepo;
    }
}
