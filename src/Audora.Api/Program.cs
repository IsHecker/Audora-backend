using Audora.Application;
using Audora.Infrastructure;

namespace Audora.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services
                .AddPresentation()
                .AddApplication()
                .AddInfrastructure(builder.Configuration);
        }
        
        var app = builder.Build();
        {
            app.UseHttpsRedirection();
            app.MapControllers();
        }
        
        app.Run();
    }
}