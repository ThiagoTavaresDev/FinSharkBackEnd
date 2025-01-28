using System.ComponentModel.DataAnnotations;

namespace FinSharkProjeto.Dtos.Comment;

public class CreateCommentDTO
{       [Required]
        [MinLength(5, ErrorMessage = "O título deve ter 5 caracteres")]
        [MaxLength(280, ErrorMessage = "O título não pode ter mais de 280 caracteres")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "O conteúdo deve ter 5 caracteres")]
        [MaxLength(280, ErrorMessage = "O conteúdo não pode ter mais de 280 caracteres")]
        public string Content { get; set; } = string.Empty;
}