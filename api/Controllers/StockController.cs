using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context; // field to hold the database context, which will be injected via the constructor
        private readonly IStockRepository _stockRepo; // field to hold the stock repository, which will be injected via the constructor
        
        public StockController(ApplicationDBContext context, IStockRepository stockRepo) // constructor that takes in the database context and assigns it to the field
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        [Authorize] // require authentication to access this endpoint
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) // enforce data validation on the query parameters
                return BadRequest(ModelState);
                
            var stocks = await _stockRepo.GetAllAsync(query); // get all stocks from the repository
             
            var stockDTOs = stocks.Select(s => s.ToStockDTO()); // map to DTOs

            return Ok(stockDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetByIdAsync(id); // get stock by id from the repository

            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }

        [HttpPost] 
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO createStockDTO) //From Body automatically deserialize JSON body into CreateStockRequestDTO object
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = createStockDTO.ToStockFromCreateDTO(); // map from createStockDTO to model
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO()); // return 201 with location header pointing to new resource
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepo.UpdateAsync(id, updateDTO); // update stock by id using the repository

            if(stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepo.DeleteAsync(id); // delete stock by id using the repository
            
            if(stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}