using ChneseSaleApi.models;

namespace sale.repositories
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> getAllPurchases();
    }
}
