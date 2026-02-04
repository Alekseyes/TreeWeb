using TreeWeb.Endpoints;
using TreeWeb.Extensions;
using TreeWeb.Middleware;

namespace TreeWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.         
            builder.AddServices();
     
            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Minimal API endopoints
            app.AddAuthEndpoints(); // Для аутентификации
            app.AddDirectoryEndpoints(); // Для работы с каталогами
            
            app.Run();
        }
    }
}
