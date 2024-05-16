namespace FurnitureStore.Application.Common.Interfaces;

public interface IUnitofWork
{
    Task CommitChangesAsync();
}
