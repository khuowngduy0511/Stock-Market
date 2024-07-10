using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api_examlpe.Data;
using web_api_examlpe.Dtos.Stock;
using web_api_examlpe.Helpers;
using web_api_examlpe.Interfaces;
using web_api_examlpe.Mappers;
using web_api_examlpe.Models;
using web_api_examlpe.Repository;



namespace web_api_examlpe.Controller
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){

            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var stocks = await _stockRepo.GetAllAsync(query);
            
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetbyID([FromRoute] int id) {

            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null) {

                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto) {
                        
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetbyID), new { id = stockModel.Id}, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}") ]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) 
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

            if (stockModel == null) {

                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var stockModel = await _stockRepo.DeleteAsync(id); 

            if (stockModel == null) {

                return NotFound();
            }

            return NoContent();
        }
    }   
}