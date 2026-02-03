using TreeWeb.Abstract;
using TreeWeb.Models.DTO;

namespace TreeWeb.Endpoints
{
    public static class  DirectoryEndpoints 
    {
        public static void AddDirectoryEndpoints(this WebApplication app)
        {
            var directoryGroup = app.MapGroup("/api/directories").RequireAuthorization(p => p.RequireRole("User","Admin"));
            
            directoryGroup.MapGet("/export", GetDirectories);
            directoryGroup.MapGet("/{id}", GetDirectory);
            
            directoryGroup.MapPost("", AddDirectory).RequireAuthorization(p => p.RequireRole("Admin"));
            directoryGroup.MapPut("/{id}", UpdateDirectory);
            directoryGroup.MapDelete("/{id}", DeleteDirectory).RequireAuthorization(p => p.RequireRole("Admin"));           
        }

        private static async Task<IResult> GetDirectories(IDirectoryService dirService)
        {
            return TypedResults.Ok(await dirService.GetDirectoriesAsync());
        }

        private static async Task<IResult> AddDirectory(DirectoryDTO dto, IDirectoryService dirService)
        {          
            var (result, error) = await dirService.AddDirectoryAsync(dto);
            if (error != null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Created($"/api/directories/{result.Name}", result);
        }

        private static async Task<IResult> GetDirectory(long id, IDirectoryService dirService)
        {
            var res =  await dirService.GetDirectoryAsync(id);
            if(res != null)
            {
                return TypedResults.Ok(res);
            }
            return TypedResults.NotFound();
        }

        private static async Task<IResult> UpdateDirectory(long id, DirectoryDTO dto, IDirectoryService dirService)
        {
            var (result, error) = await dirService.UpdateDirectoryAsync(id, dto);
            if (error != null) {
                return TypedResults.BadRequest(error);
            }
            if(result == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }

        private static async Task<IResult> DeleteDirectory(long id, IDirectoryService dirService)
        {
            var result = await dirService.DeleteDirectoryAsync(id);
            return result ? TypedResults.Ok() : TypedResults.NotFound();
        }
    }
}
