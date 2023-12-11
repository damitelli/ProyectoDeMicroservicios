namespace Application.Common.Interfaces;

public interface IItemService
{
    Task<IReadOnlyCollection<Item>> GetAllItemsAsync();
    Task<Item> GetItemByIdAsync(Guid id);
    Task CreateItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task<Unit> DeleteItemAsync(Guid id);
}