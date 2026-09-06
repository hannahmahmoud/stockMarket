using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class CommentRepo : ICommentRepo
    {
        private readonly ApplicationDbContext dbContext;

        public CommentRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Comments>> GetAll()
        {
            return await dbContext.Comments.ToListAsync();
        }

        public async Task<Comments?> GetCommentById(int id)
        {
            return await dbContext.Comments.FindAsync(id);
        }

        public async Task<Comments?> GetCommentByTitleAndContent(
            string title,
            string content)
        {
            return await dbContext.Comments.FirstOrDefaultAsync(c =>
                c.title == title &&
                c.content == content);
        }

        public async Task<Comments> Create(Comments comment)
        {
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Comments> Delete(Comments comment)
        {
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Comments> Update(
            Comments comment,
            Comments updatedComment)
        {
            updatedComment.id = comment.id;

            dbContext.Entry(comment)
                .CurrentValues
                .SetValues(updatedComment);

            await dbContext.SaveChangesAsync();

            return updatedComment;
        }
    }
}