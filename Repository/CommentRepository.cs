using FinSharkProjeto.Data;
using FinSharkProjeto.Interfaces;
using FinSharkProjeto.Model;
using Microsoft.EntityFrameworkCore;

namespace FinSharkProjeto.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;
    
    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
       var comment =  await _context.Comments.FindAsync(id);

       if (comment == null)
       {
           return null;
       }
       
       return comment;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        
        await _context.SaveChangesAsync();
        
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return null;
        }
        
        comment.Title = commentModel.Title;
        comment.Content = commentModel.Content;
        
        await _context.SaveChangesAsync();
        
        return comment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
        {
            return null;    
        }
        _context.Comments.Remove(comment);
        
        _context.SaveChanges();
        
        return comment;
        
    }
}