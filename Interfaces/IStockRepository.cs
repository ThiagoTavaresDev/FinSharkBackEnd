using FinSharkBackEnd.Helpers;
using FinSharkProjeto.Dtos.Stock;
using FinSharkProjeto.Model;

namespace FinSharkProjeto.Interfaces;

public interface IStockRepository
{
     Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?>  GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockModelDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);

    Task<Stock?> GetBySymbolAsync(string symbol);
   

}