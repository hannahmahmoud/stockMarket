using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class StockService
    {
        private readonly ApplicationDbContext context;

        public StockService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Stock>?> getAllStocks()
        {
            try
            {
                var stocks = await context.Stock.ToListAsync();

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
                var foundStock = await context.Stock.FindAsync(stock.id);

                if (foundStock != null)
                    return null;

                await context.Stock.AddAsync(stock);
                await context.SaveChangesAsync();

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
                var stock = await context.Stock.FindAsync(id);

                if (stock == null)
                    return null;

                context.Stock.Remove(stock);
                await context.SaveChangesAsync();

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
                var stock = await context.Stock.FindAsync(id);

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
                var stock = await context.Stock.FindAsync(id);

                if (stock == null)
                    return null;

                UpdatedStock.id = id;

                context.Entry(stock).CurrentValues.SetValues(UpdatedStock);

                await context.SaveChangesAsync();

                return UpdatedStock;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating stock: " + ex.Message);
                return null;
            }
        }

        public async Task<List<Stock>?> stockQuery(
            String? companyName,
            String? symbol,
            decimal? purchase,
            decimal? lastDiv,
            String? industry,
            long? matketCap,
            int pageNumber,
            int pageSize)
        {
            try
            {
                IQueryable<Stock> Query = context.Stock;

                if (companyName != null)
                    Query = Query.Where(s => s.companyName.Contains(companyName));

                if (symbol != null)
                    Query = Query.Where(s => s.symbol == symbol);

                if (purchase.HasValue)
                    Query = Query.Where(s => s.purchase >= purchase);

                if (lastDiv.HasValue)
                    Query = Query.Where(s => s.lastDiv == lastDiv);

                if (industry != null)
                    Query = Query.Where(s => s.industry == industry);

                if (matketCap.HasValue)
                    Query = Query.Where(s => s.matketCap == matketCap);

                Query = Query.OrderBy(s => s.companyName);

                if (pageNumber < 1)
                    pageNumber = 1;

                if (pageSize < 1)
                    pageSize = 10;

                if (pageSize > 100)
                    pageSize = 100;

                Console.WriteLine("page number: " + pageNumber);
                Console.WriteLine("page size: " + pageSize);

                Query = Query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                return await Query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching stocks: " + ex.Message);
                return null;
            }
        }
    }
}