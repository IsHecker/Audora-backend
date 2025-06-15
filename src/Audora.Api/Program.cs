using System.IdentityModel.Tokens.Jwt;
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
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddPresentation(builder.Configuration);
        }

        var app = builder.Build();
        {
            app.UseHttpsRedirection();
            app.UseCors("AllowNullOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }

        app.Run();
    }
}