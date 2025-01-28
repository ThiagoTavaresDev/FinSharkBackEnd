using System.ComponentModel.DataAnnotations;

namespace FinSharkProjeto.Dtos.Comment;

public class UpdateCommentRequestDTO
{
        [Required]
        [MinLength(5, ErrorMessage = "O título deve ter no mínimo 5 caracteres")]
        [MaxLength(280, ErrorMessage = "O título não pode exceder 280 caracteres")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "O conteúdo deve ter no mínimo 5 caracteres")]
        [MaxLength(280, ErrorMessage = "O conteúdo não pode exceder 280 caracteres")]
        public string Content { get; set; } = string.Empty;
}