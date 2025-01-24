using FinSharkProjeto.Dtos.Stock;
using FinSharkProjeto.Model;

namespace FinSharkProjeto.Mappers;

public static class StockMappers
{
    public static StockDTO ToStockDTO(this Stock stockModel)
    {
        return new StockDTO
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList()
        };
    }

    public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockDto)
    {
        return new Stock
        {
            Symbol = stockDto.Symbol,
            CompanyName = stockDto.CompanyName,
            Purchase = stockDto.Purchase,
            LastDiv = stockDto.LastDiv,
            Industry = stockDto.Industry,
            MarketCap = stockDto.MarketCap
        };
    }
}