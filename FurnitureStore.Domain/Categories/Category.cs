using ErrorOr;
using FurnitureStore.Domain.Products;
using FurnitureStore.Domain.SubCategories;
using Throw;

namespace FurnitureStore.Domain.Categories;

public class Category
{
    #region Private Fields
    private readonly List<SubCategory> _subCategories = new();
    private readonly List<Product> _products = new();
    #endregion Private Fields

    #region Constructors
    public Category(string name, Guid? id = null)
    {
        this.Name = name;
        this.Id = id ?? Guid.NewGuid();
    }

    private Category(){ }
    #endregion Constructors

    #region Public Properties
    public Guid Id { get;}
    public string Name { get; private set; } = null!;
    public IReadOnlyCollection<SubCategory>? SubCategories => _subCategories.AsReadOnly();
    public IReadOnlyCollection<Product>? Products => _products.AsReadOnly();
    #endregion Public Properties

    #region Public Methods
    public void UpdateCategory(string name)
    {
        Name = name;
    }

    public ErrorOr<Success> AddSubCategory(SubCategory subCategory)
    {
        if(_subCategories.Any(sc => sc.Name == subCategory.Name))
        {
            return CategoryErrors.CannotHaveDuplicateSubCategories;
        }

        _subCategories.Add(subCategory);

        return Result.Success;
    }

    public void RemoveSubCategory(SubCategory subCategory)
    {
        //_subCategories
        //    .Throw(() => new ArgumentException($"Sub-category, {subCategory.Name}, does not exist in category."))
        //    .IfNotContains(subCategory);

        var exists = _subCategories.Any(sc => sc.Id == subCategory.Id);

        if (!exists)
        {
            throw new ArgumentException($"Sub-category, {subCategory.Name}, does not exist in category.");
        }

        _subCategories.Remove(subCategory);
    }

    public bool HasSubCategory(Guid subCategoryId)
    {
        return _subCategories.Any(sc => sc.Id == subCategoryId);
    }
    #endregion
}
