using backend.Models;
using backend.Repository;

namespace backend.Service
{
    public class CommentService
    {
        private readonly ICommentRepo repository;

        public CommentService(ICommentRepo repository)
        {
            this.repository = repository;
        }

        
        public async Task<List<Comments>?> getAllComments()
        {
            var comments = await repository.GetAll();

            if (comments.Count == 0)
                return null;

            return comments;
        }

   
        public async Task<Comments?> getCommentById(int id)
        {
            var comment = await repository.GetCommentById(id);

            if (comment == null)
                return null;

            return comment;
        }

      
        public async Task<Comments?> createComment(Comments comment)
        {
            var existingComment =
                await repository.GetCommentByTitleAndContent(
                    comment.title,
                    comment.content);

            if (existingComment != null)
                return null;

            return await repository.Create(comment);
        }

        public async Task<Comments?> deleteComment(int id)
        {
            var comment = await repository.GetCommentById(id);

            if (comment == null)
                return null;

            return await repository.Delete(comment);
        }

      
        public async Task<Comments?> updateComment(
            int id,
            Comments updatedComment)
        {
            var comment = await repository.GetCommentById(id);

            if (comment == null)
                return null;

            return await repository.Update(
                comment,
                updatedComment);
        }
    }
}