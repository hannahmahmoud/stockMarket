using backend.Models;

namespace backend.Repository
{
    public interface IstockRepo
    {
         public   Task<List<Stock>> GetAll();
         public  Task<Stock> Create(Stock stock);

         public  Task<Stock> Delete(Stock stock);

        public  Task<Stock?> getStockByID(int id);


         public  Task<Stock> updateStock(int id, Stock UpdatedStock, Stock oldStock);

         Task<Stock?> GetStockBySymbolAndCompanyName(string symbol, string companyName);


         Task<List<Stock>> stockQuery(
            string? companyName,
            string? symbol,
            decimal? purchase,
            decimal? lastDiv,
            string? industry,
            long? matketCap,
            int pageNumber,
            int pageSize);

    }
}