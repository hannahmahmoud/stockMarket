using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;

namespace backend.Service
{
    public class CommentService
    {
        
        private readonly ApplicationDbContext dbContext;

        public CommentService ( ApplicationDbContext dbContext )
        {
            this.dbContext=dbContext;
            
        }


        public async Task<List<Comments>?> getAllComments()
        {
            var comments= await dbContext.Comments.ToListAsync();
            if (comments.Count ==0)
            return null;
            return comments;
        }

        public async Task<Comments?> getCommentById( int id )
        {
            var comments = await dbContext.Comments.FindAsync(id);
             if (comments ==null)
             return null;
             return comments;
        }

        public async Task<Comments?> createComment(Comments comment)
        {
            var existingCommment= await dbContext.Comments.FirstOrDefaultAsync(c =>
               c.title== comment.title && c.content== comment.content ); 
            if (existingCommment!= null)
            return null ;
            var comments= await dbContext.Comments.AddAsync(comment);
             dbContext.SaveChanges();
             return comment;



        }


        public async Task<Comments?>deleteComment (int id)
        {
            var comment = await dbContext.Comments.FindAsync(id);
             if (comment== null)
             return null;
              dbContext.Comments.Remove(comment);
              dbContext.SaveChanges();
              return comment; 
            
        }

        public  async Task<Comments?> updateComment (int id, Comments UpdatedComment)
        {
            var comment =  await dbContext.Stock.FindAsync( id);
            if (comment==null)
            return null;
            UpdatedComment.id = id;
            dbContext.Entry(comment).CurrentValues.SetValues(UpdatedComment);
            await dbContext.SaveChangesAsync();
             return UpdatedComment;


        }




    }
}