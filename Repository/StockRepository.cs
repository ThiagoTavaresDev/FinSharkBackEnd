using FinSharkProjeto.Data;
using FinSharkProjeto.Dtos.Stock;
using FinSharkProjeto.Interfaces;
using FinSharkProjeto.Mappers;
using FinSharkProjeto.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FinSharkProjeto.Repository;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;
    
    public StockRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stock>> GetAllAsync()
    {
        return  await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        var stockModel = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);

        if (stockModel == null)
        {
            return null;
        }
        
        return stockModel;
        
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async  Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateDto)
    {
        var stockModel = await _context.Stocks.FindAsync(id);
        
        if (stockModel == null)
        {
            return null;            
        }
        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap = updateDto.MarketCap;

        await _context.SaveChangesAsync();
        
        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FindAsync(id);

        if (stockModel == null)
        {
            return null;
        }
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public Task<bool> StockExists(int id)
    {
        return _context.Stocks.AnyAsync(i => i.Id == id);
    }
}