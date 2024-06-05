using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web_api_examlpe.Models;

namespace web_api_examlpe.Controller
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        [HttpGet("{StockId}")]
        public IActionResult GetbyID([FromRoute] int StockId) {

            var commentsForStockID = new List<Comment>()
            {
                new Comment { id = 123, Title = $"{StockId} is the best option", Content = "Sample Content", CreatedOn = DateTime.Now, StockId = StockId },
                 new Comment { id = 123, Title = $"I've already bought {StockId} and never regret", Content = "Sample Content 2", CreatedOn = DateTime.Now, StockId = StockId }
            };

            var response = new
            {
                code = 200,
                data = commentsForStockID,
                message = $"Comments retrieved successfully for StockId: {StockId}"
            };


            return StatusCode(200, response);;
        }

    }   
}