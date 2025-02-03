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
using Microsoft.AspNetCore.Identity;
using FinSharkBackEnd.Model;
using FinSharkBackEnd.Extension;
using FinSharkBackEnd.Interfaces;
using FinSharkBackEnd.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace FinSharkProjeto.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        
        private readonly IFMPService _fmpService;

        public CommentController(AppDbContext context, ICommentRepository commentRepository, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _context = context;
            _commentRepo = commentRepository;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        // GET: api/Comment
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments([FromQuery] CommentQueryObject queryObject)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var comments = await _commentRepo.GetAllCommentsAsync(queryObject);
            
            var commentsDto = comments.Select(x => x.ToCommentDTO());
            
            return Ok(commentsDto);
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
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
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

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
        [Route("{symbol:alpha}")]
        public async Task<ActionResult<Comment>> PostComment([FromRoute] string symbol, CreateCommentDTO commentDto)
        {
             if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null){
                stock = await  _fmpService.FindStockBySymbolAsync(symbol);
                if(stock == null){
                    return BadRequest();
                }
                else{
                    await _stockRepo.CreateAsync(stock);
                }
            }
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync( username);

            var commentModel = commentDto.ToCommentFromCreate(stock.Id);
            commentModel.AppUserId = appUser.Id;
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
