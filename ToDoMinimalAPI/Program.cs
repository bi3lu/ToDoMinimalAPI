using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoMinimalAPI.ToDo;

namespace ToDoMinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IToDoService, ToDoService>();
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(ToDoValidator));
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["JwtIssuer"],
                        ValidAudience = builder.Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
                    };
                });

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.RegisterEndpoints();

            // For simplicty this part give us a JWT token.
            app.MapGet("/token", () =>
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "user-id"),
                    new Claim(ClaimTypes.Name, "Test Name"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var token = new JwtSecurityToken
                (
                    issuer: builder.Configuration["JwtIssuer"],
                    audience: builder.Configuration["JwtIssuer"],
                    claims: claims,
                    expires: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])), SecurityAlgorithms.HmacSha256)
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                return jwtToken;
            });

            app.MapGet("/hello", (ClaimsPrincipal user) =>
            {
                var userName = user.Identity.Name;

                return $"Hello {userName}";
            });

            app.Run();
        }
    }
}