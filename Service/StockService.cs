using backend.Data;
using backend.Models;
using backend.Repository;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var stocks = await repository.GetAll();

                if (stocks.Count == 0)
                    return null;

                return stocks;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting stocks: " + ex.Message);
                return null;
            }
        }

        public async Task<Stock?> createStock(Stock stock)
        {
            try
            {
                var foundStock = await repository.getStockByID(stock.id);

                if (foundStock != null)
                    return null;

                var createStock= await repository.Create(stock);



                return stock;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating stock: " + ex.Message);
                return null;
            }
        }

        public async Task<Stock?> deleteStock(int id)
        {
            try
            {
                var stock = await repository.getStockByID(id);

                if (stock == null)
                    return null;

               await  repository.Delete(stock);

                return stock;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting stock: " + ex.Message);
                return null;
            }
        }

        public async Task<Stock?> getStockByID(int id)
        {
            try
            {
                var stock = await repository.getStockByID(id);

                if (stock == null)
                    return null;

                return stock;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting stock: " + ex.Message);
                return null;
            }
        }

        public async Task<Stock?> updateStock(int id, Stock UpdatedStock)
        {
            try
            {
                var stock = await repository.getStockByID(id);

                if (stock == null)
                    return null;

                await repository.updateStock(id, stock , UpdatedStock);
                return UpdatedStock;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating stock: " + ex.Message);
                return null;
            }
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
    try
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
    catch (Exception ex)
    {
        Console.WriteLine(
            "Error searching stocks: " + ex.Message);

        return null;
    }
}
    }
}