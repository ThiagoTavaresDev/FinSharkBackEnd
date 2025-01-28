using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinSharkProjeto.Data;
using FinSharkProjeto.Dtos.Comment;
using FinSharkProjeto.Interfaces;
using FinSharkProjeto.Mappers;
using FinSharkProjeto.Model;

namespace FinSharkProjeto.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        
        public CommentController(AppDbContext context, ICommentRepository commentRepository, IStockRepository stockRepo)
        {
            _context = context;
            _commentRepo = commentRepository;
            _stockRepo = stockRepo;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _commentRepo.GetAllCommentsAsync();
            
            var commentsDto = comments.Select(x => x.ToCommentDTO());
            
            return Ok(commentsDto);
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDTO());
        }

        // PUT: api/Comment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, UpdateCommentRequestDTO updateDTO)
        {
            var comment = await _commentRepo.UpdateAsync(id,updateDTO.ToCommentFromUpdate());
            
            if (id != comment.Id)
            {
                return BadRequest();
            }
            else if(comment == null)
            {
                return NotFound("Comentario nao encontrado");
            }
            return Ok(comment.ToCommentDTO());
            
        }

        // POST: api/Comment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(int stockId, CreateCommentDTO commentDto)
        {
            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock não existe");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction("GetComment", new { id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepo.DeleteAsync(id);
            
            if (comment == null)
            {
                return NotFound("Nao existe comentario para deletar");
            }
            return Ok(comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
