using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;

namespace backend.Repository
{
    public class StockRepo:IstockRepo
    {

        private readonly ApplicationDbContext context;

        public  StockRepo(ApplicationDbContext context)
        {
            this.context=context;
        }

        public  async Task<List<Stock>> GetAll()
        {
            return await context.Stock.ToListAsync();
        }


        public  async  Task<Stock> Create(Stock stock)
        {  
            await context.Stock.AddAsync(stock);
            await context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> getStockByID(int id)
        {
            return await context.Stock.FindAsync(id);
        }


        public async Task<Stock> updateStock(int id, Stock UpdatedStock, Stock oldStock)

        {
            UpdatedStock.id = id;

                context.Entry(oldStock).CurrentValues.SetValues(UpdatedStock);

                await context.SaveChangesAsync();

                return UpdatedStock;
        }

        public async Task<Stock> Delete(Stock stock )
        {
                context.Stock.Remove(stock);
                await context.SaveChangesAsync();
                return stock; 
        }

        public async Task<Stock?> GetStockBySymbolAndCompanyName(
            string symbol,
            string companyName)
        {
             return await context.Stock
            .FirstOrDefaultAsync(s =>
            s.symbol == symbol &&
            s.companyName == companyName);
        }


        public async Task<List<Stock>> stockQuery(
    string? companyName,
    string? symbol,
    decimal? purchase,
    decimal? lastDiv,
    string? industry,
    long? matketCap,
    int pageNumber,
    int pageSize)
{
    IQueryable<Stock> Query = context.Stock;

    if (companyName != null)
        Query = Query.Where(s =>
            s.companyName.Contains(companyName));

    if (symbol != null)
        Query = Query.Where(s =>
            s.symbol == symbol);

    if (purchase.HasValue)
        Query = Query.Where(s =>
            s.purchase >= purchase);

    if (lastDiv.HasValue)
        Query = Query.Where(s =>
            s.lastDiv == lastDiv);

    if (industry != null)
        Query = Query.Where(s =>
            s.industry == industry);

    if (matketCap.HasValue)
        Query = Query.Where(s =>
            s.matketCap == matketCap);

    Query = Query.OrderBy(s => s.companyName);

    if (pageNumber < 1)
        pageNumber = 1;

    if (pageSize < 1)
        pageSize = 10;

    if (pageSize > 100)
        pageSize = 100;

    Query = Query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize);

    return await Query.ToListAsync();
}



        
    }
}