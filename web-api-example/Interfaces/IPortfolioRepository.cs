using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api_examlpe.Models;

namespace web_api_examlpe.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
    }
}