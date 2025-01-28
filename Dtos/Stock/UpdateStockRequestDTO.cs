using System.ComponentModel.DataAnnotations;

namespace FinSharkProjeto.Dtos.Stock;

public class UpdateStockRequestDTO
{
      
        [Required]
        [MaxLength(10, ErrorMessage = "O símbolo não pode ter mais de 10 caracteres")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "O nome da empresa não pode ter mais de 10 caracteres")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000, ErrorMessage = "O valor de compra deve estar entre 1 e 1.000.000.000")] 
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100, ErrorMessage = "O último dividendo deve estar entre 0,001 e 100")] 
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "A indústria não pode ter mais de 10 caracteres")]
        public string Industry { get; set; } = string.Empty;
        [Range(1, 5000000000, ErrorMessage = "A capitalização de mercado deve estar entre 1 e 5.000.000.000")] 
        public long MarketCap { get; set; }
}
