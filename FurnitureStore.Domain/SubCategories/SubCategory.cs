using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Domain.SubCategories;

public class SubCategory
{
    #region Private Fields
    private readonly List<Product> _products = new(); 
    #endregion Private Fields

    #region Constructors
    public SubCategory(string name, Guid categoryId,Guid? id = null)
    {
        Name = name;
        CategoryId = categoryId;
        Id = id ?? Guid.NewGuid();
    }

    private SubCategory() { }
    #endregion Constructors

    #region Properties
    public Guid Id { get;private set; }
    public string Name { get; private set; } = null!;
    public Guid CategoryId { get; }
    public Category Category { get; } = null!;

    public IReadOnlyCollection<Product>? Products => _products.AsReadOnly();
    #endregion Properties

    #region Public Methods
    public void UpdateSubCategory(string name)
    {
        this.Name = name; 
    }
    #endregion Public Methods
}
