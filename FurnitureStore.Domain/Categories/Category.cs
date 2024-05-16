namespace FurnitureStore.Domain.Categories;

public class Category
{
    #region Constructors

    //Parameterless constructor of EF Core
    private Category(){ }
    public Category(string name, Guid? id = null)
    {
        this.Name = name;
        this.Id = id ?? Guid.NewGuid();
    }
    #endregion Constructors

    #region Public Properties
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    #endregion Public Properties

    #region Public Methods
    public void UpdateCategory(string name)
    {
        Name = name;
    }
    #endregion
}
