namespace FurnitureStore.Domain.Categories;

public class Category
{
    #region Constructors
    public Category(string name, Guid? id = null)
    {
        this.Name = name;
        this.Id = id ?? Guid.NewGuid();
    }
    #endregion Constructors

    #region Public Properties
    public Guid Id { get; private set; }
    public string Name { get; }
    #endregion Public Properties
}
