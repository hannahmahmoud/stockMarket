using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class CommentService
    {
        private readonly ApplicationDbContext dbContext;

        public CommentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Comments>?> getAllComments()
        {
            try
            {
                var comments = await dbContext.Comments.ToListAsync();

                if (comments.Count == 0)
                    return null;

                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting all comments: " + ex.Message);
                return null;
            }
        }

        public async Task<Comments?> getCommentById(int id)
        {
            try
            {
                var comment = await dbContext.Comments.FindAsync(id);

                if (comment == null)
                    return null;

                return comment;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting comment: " + ex.Message);
                return null;
            }
        }

        public async Task<Comments?> createComment(Comments comment)
        {
            try
            {
                var existingComment = await dbContext.Comments.FirstOrDefaultAsync(c =>
                    c.title == comment.title &&
                    c.content == comment.content);

                if (existingComment != null)
                    return null;

                await dbContext.Comments.AddAsync(comment);

                await dbContext.SaveChangesAsync();

                return comment;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating comment: " + ex.Message);
                return null;
            }
        }

        public async Task<Comments?> deleteComment(int id)
        {
            try
            {
                var comment = await dbContext.Comments.FindAsync(id);

                if (comment == null)
                    return null;

                dbContext.Comments.Remove(comment);

                await dbContext.SaveChangesAsync();

                return comment;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting comment: " + ex.Message);
                return null;
            }
        }

        public async Task<Comments?> updateComment(
            int id,
            Comments UpdatedComment)
        {
            try
            {
                var comment = await dbContext.Comments.FindAsync(id);

                if (comment == null)
                    return null;

                UpdatedComment.id = id;

                dbContext.Entry(comment)
                    .CurrentValues
                    .SetValues(UpdatedComment);

                await dbContext.SaveChangesAsync();

                return UpdatedComment;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating comment: " + ex.Message);
                return null;
            }
        }
    }
}