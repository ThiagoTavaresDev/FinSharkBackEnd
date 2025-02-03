using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSharkBackEnd.Model;
using Microsoft.AspNetCore.Identity;
using FinSharkProjeto.Interfaces;
using Microsoft.AspNetCore.Authorization;
using FinSharkBackEnd.Extension;
using FinSharkBackEnd.Interfaces;

namespace FinSharkBackEnd.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepository;

        private readonly IFMPService _fmpService;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepository, IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepository = portfolioRepository;
            _fmpService = fmpService;
        }

        // GET: api/Portfolio
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Portfolio>>> GetPortfolios()
        {
            var username = User.GetUsername();
            var appUser  = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPorfolio(appUser);

            return Ok(userPortfolio);

        }
        // POST: api/Portfolio
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Portfolio>> CreatePortfolio(string symbol)
        {
         var username = User.GetUsername();
         var appUser  = await _userManager.FindByNameAsync(username);
         var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock does not exists");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

        if(stock == null) return BadRequest("");

        var userPortfolio = await _portfolioRepository.GetUserPorfolio(appUser);

        if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())){
        return BadRequest("");
        }
      

        var portfolio = new Portfolio{
            StockId = stock.Id,
            AppUserId = appUser.Id
        };

        await _portfolioRepository.CreateAsync(portfolio);

        if(portfolio == null){
            return NotFound("");
        } 
        else{
            return Created();
        }
        }
        // DELETE: api/Portfolio/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser  = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepository.GetUserPorfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1){
                await _portfolioRepository.DeletePortfolio(appUser,symbol);
            }
            else{
                return BadRequest();
            }
            return Ok();
        }
    }
} 