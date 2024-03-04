
using Microsoft.EntityFrameworkCore;
using SMAK_AJWTAuthNetCore_Core.Entities;
using SMAK_AJWTAuthNetCore_Core.Interfaces;
using SMAK_AJWTAuthNetCore_Infra.Data;
using SMAK_AJWTAuthNetCore_Infra.Repositories;
using SMAK_AJWTAuthNetCore_Services.Services;

namespace SMAK_AJWTAuthNetCore_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddControllers();

            //Add the Custom services in IOC container
            builder.Services.AddScoped<IUsersRepository<RegisterRequestModel>, UsersRepository>();
            builder.Services.AddScoped<IUserService<RegisterRequestModel>, UserService>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();
            builder.Services.AddScoped<ITokenService,TokenService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
