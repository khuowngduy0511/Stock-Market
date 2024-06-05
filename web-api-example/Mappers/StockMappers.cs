using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_api_examlpe.Dtos.Stock;
using web_api_examlpe.Models;

namespace web_api_examlpe.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel) {
            return new StockDto{
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }

        public static Stock ToStockFromCreateDTO (this CreateStockRequestDto stockDto) {
            return new Stock{
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }

    }
}