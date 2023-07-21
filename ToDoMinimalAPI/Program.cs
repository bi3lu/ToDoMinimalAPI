using Microsoft.AspNetCore.Mvc;
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

            app.MapGet("/todos", (IToDoService service) => service.GetAll());
            app.MapGet("/todos/{id}", (IToDoService service, Guid id) => service.GetById(id));
            app.MapPost("/todos", (IToDoService service, ToDo.ToDo toDo) => service.Create(toDo));
            app.MapPut("/todos/{id}", (IToDoService service, Guid id, ToDo.ToDo toDo) => service.Update(toDo));
            app.MapDelete("/todos/{id}", (IToDoService service, Guid id) => service.Delete(id));

            app.Run();
        }
    }
}