using backend.Models;

namespace backend.Repository
{
    public interface ICommentRepo
    {
        Task<List<Comments>> GetAll();

        Task<Comments?> GetCommentById(int id);

        Task<Comments?> GetCommentByTitleAndContent(
            string title,
            string content);

        Task<Comments> Create(Comments comment);

        Task<Comments> Delete(Comments comment);

        Task<Comments> Update(
            Comments comment,
            Comments updatedComment);
    }
}