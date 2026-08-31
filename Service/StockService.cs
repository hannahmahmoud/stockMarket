using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{   
    public class StockService
    {
        private readonly ApplicationDbContext context;
        private int num;
        public StockService(ApplicationDbContext context )
        {
            this.context=context;
            //this.num = num; 
            
        }

    
        public async Task<List<Stock>?> getAllStocks()
        {
            
            var stocks=  await context.Stock.ToListAsync();
            if (stocks.Count ==0)
             return null;
             return stocks;
        }

        public async Task<Stock?> createStock (Stock stock)
        {
            var foundStock=  await context.Stock.FindAsync(stock.id);
             if (foundStock!= null)
             return null; 
            await context.Stock.AddAsync(stock);
             await context.SaveChangesAsync();
            return stock;
        }

        public async Task <Stock?> deleteStock (int id)
        {
            var stock =await  context.Stock.FindAsync(id);
             if (stock==null)
             return null;

             context.Stock.Remove(stock);
             await context.SaveChangesAsync();
              return stock;

        }

        public async  Task<Stock?> getStockByID (int id )
        {
            var stock = await context.Stock.FindAsync(id);
            if (stock==null)
            return null;
             return stock;
        }


        public  async Task<Stock?> updateStock (int id, Stock UpdatedStock)
        {
            var stock =  await context.Stock.FindAsync( id);
            if (stock==null)
            return null;
            UpdatedStock.id = id;
            context.Entry(stock).CurrentValues.SetValues(UpdatedStock);
            await context.SaveChangesAsync();
             return UpdatedStock;


        }


    }
}