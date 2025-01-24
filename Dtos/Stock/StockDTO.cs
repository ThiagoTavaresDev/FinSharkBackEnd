using System.ComponentModel.DataAnnotations.Schema;
using FinSharkProjeto.Dtos.Comment;
using FinSharkProjeto.Model;

namespace FinSharkProjeto.Dtos.Stock;

public class StockDTO
{
  
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        
        public List<CommentDTO> Comments { get; set; }
}