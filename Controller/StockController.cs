using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSharkProjeto.Data;
using FinSharkProjeto.Dtos.Stock;
using FinSharkProjeto.Interfaces;
using FinSharkProjeto.Mappers;
using FinSharkProjeto.Model;
using FinSharkBackEnd.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace FinSharkProjeto.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStockRepository _stockRepo;
        
        public StockController(AppDbContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        // GET: api/Stock
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks([FromQuery] QueryObject query)
        {
              if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stocks = await _stockRepo.GetAllAsync(query);

            var stocksMapped = stocks.Select(s => s.ToStockDTO()).ToList();
            
            return Ok(stocksMapped);
        }

        // GET: api/Stock/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStock([FromRoute] int id)
        {   
              if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }

        // PUT: api/Stock/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, UpdateStockRequestDTO updateDto)
        {
              if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
            
            if (id != stockModel.Id)
            {
                return BadRequest();
            }

            try
            {
                return Ok(stockModel.ToStockDTO());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Stock
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Stock>> PostStock(CreateStockRequestDTO stock)
        {
              if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stockModel = stock.ToStockFromCreateDTO();
            
            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction("GetStock", new { id = stockModel.Id }, stockModel);
        }

        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stockModel = await _stockRepo.DeleteAsync(id);
            
            if (stockModel == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}
