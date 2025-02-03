using FinSharkBackEnd.Helpers;
using FinSharkProjeto.Model;

namespace FinSharkProjeto.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllCommentsAsync(CommentQueryObject queryObject);
    
    Task<Comment?> GetCommentByIdAsync(int id);
    
    Task<Comment> CreateAsync(Comment commentModel);
    
    Task<Comment?> UpdateAsync(int id, Comment commentModel);
    
    Task<Comment?> DeleteAsync(int id);
    
}