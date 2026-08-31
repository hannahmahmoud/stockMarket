using backend.Models;
using backend.Service;

namespace backend.Endpoints
{
    public static class CommentEndpoints
    {
        public static void MapCommentEndpoints(this WebApplication app)
        {
            app.MapGet("/api/v1/comments", async (CommentService commentService) =>
            {
                var comments = await commentService.getAllComments();

                if (comments == null)
                    return Results.NotFound();

                return Results.Ok(comments);
            });

            app.MapGet("/api/v1/comments/{id}", async (
                int id,
                CommentService commentService) =>
            {
                var comment = await commentService.getCommentById(id);

                if (comment == null)
                    return Results.NotFound();

                return Results.Ok(comment);
            });

            app.MapPost("/api/v1/comments", async (
                Comments comment,
                CommentService commentService) =>
            {
                var createdComment =
                    await commentService.createComment(comment);

                if (createdComment == null)
                {
                    return Results.BadRequest(new
                    {
                        status = "failed",
                        msg = "comment already exists"
                    });
                }

                return Results.Created(
                    $"/api/v1/comments/{createdComment.id}",
                    createdComment
                );
            });

            app.MapDelete("/api/v1/comments/{id}", async (
                int id,
                CommentService commentService) =>
            {
                var deletedComment =
                    await commentService.deleteComment(id);

                if (deletedComment == null)
                    return Results.NotFound();

                return Results.NoContent();
            });

            app.MapPatch("/api/v1/comments/{id}", async (
                int id,
                Comments comment,
                CommentService commentService) =>
            {
                var updatedComment =
                    await commentService.updateComment(id, comment);

                if (updatedComment == null)
                    return Results.NotFound();

                return Results.Ok(updatedComment);
            });
        }
    }
}