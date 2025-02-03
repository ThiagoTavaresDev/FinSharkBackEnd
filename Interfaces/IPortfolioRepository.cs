using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinSharkBackEnd.Model;
using FinSharkProjeto.Model;

namespace FinSharkBackEnd.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPorfolio(AppUser user);

        Task<Portfolio> CreateAsync(Portfolio portfolio);

        Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);

    }
}