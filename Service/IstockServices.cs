using backend.Models;

namespace backend.Service
{
    public interface IstockServices
    {
     Task<Stock?> createStock (Stock stock);

     Task<List<Stock>?> getAllStocks();

    Task <Stock?> deleteStock (int id);

     Task<Stock?> getStockByID (int id );

      Task<Stock?> updateStock (int id, Stock UpdatedStock);



         
    }
}