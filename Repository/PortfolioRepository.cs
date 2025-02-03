using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinSharkBackEnd.Interfaces;
using FinSharkBackEnd.Model;
using FinSharkProjeto.Data;
using FinSharkProjeto.Model;
using Microsoft.EntityFrameworkCore;

namespace FinSharkBackEnd.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());

            if(portfolio == null){
                return null;
            }
            _context.Portfolios.Remove(portfolio);

            await _context.SaveChangesAsync();

            return portfolio;

        }

        public async Task<List<Stock>> GetUserPorfolio(AppUser user)
        {
           return await _context.Portfolios.Where(p => p.AppUserId == user.Id).Select(stock => new Stock{
            Id = stock.StockId,
            Symbol = stock.Stock.Symbol,
            CompanyName = stock.Stock.CompanyName,
            Purchase = stock.Stock.Purchase,
            LastDiv = stock.Stock.LastDiv,
            Industry = stock.Stock.Industry,
            MarketCap = stock.Stock.MarketCap
           }).ToListAsync();
        }
    }
}