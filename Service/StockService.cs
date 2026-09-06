using backend.Models;
using backend.Repository;

namespace backend.Service
{
    public class StockService
    {
        private readonly StockRepo repository;

        public StockService(StockRepo repository)
        {
            this.repository = repository;
        }

        public async Task<List<Stock>?> getAllStocks()
        {
            var stocks = await repository.GetAll();

            if (stocks.Count == 0)
                return null;

            return stocks;
        }

  
        public async Task<Stock?> createStock(Stock stock)
        {
            
            var foundStock = await repository.GetStockBySymbolAndCompanyName(
                stock.symbol,
                stock.companyName);

            if (foundStock != null)
                return null;

            return await repository.Create(stock);
        }

        
        public async Task<Stock?> deleteStock(int id)
        {
            var stock = await repository.getStockByID(id);

            if (stock == null)
                return null;

            await repository.Delete(stock);

            return stock;
        }

        public async Task<Stock?> getStockByID(int id)
        {
            var stock = await repository.getStockByID(id);

            if (stock == null)
                return null;

            return stock;
        }

     
        public async Task<Stock?> updateStock(
            int id,
            Stock updatedStock)
        {
            var stock = await repository.getStockByID(id);

            if (stock == null)
                return null;

            await repository.updateStock(
                id,
                stock,
                updatedStock);

            return updatedStock;
        }

      
        public async Task<List<Stock>?> stockQuery(
            string? companyName,
            string? symbol,
            decimal? purchase,
            decimal? lastDiv,
            string? industry,
            long? matketCap,
            int pageNumber,
            int pageSize)
        {
            return await repository.stockQuery(
                companyName,
                symbol,
                purchase,
                lastDiv,
                industry,
                matketCap,
                pageNumber,
                pageSize);
        }
    }
}