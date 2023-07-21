namespace ToDoMinimalAPI.ToDo
{
    public static class ToDoRequests
    {
        public static WebApplication RegisterEndpoints(this WebApplication app)
        {
            app.MapGet("/todos", ToDoRequests.GetAll);
            app.MapGet("/todos/{id}", ToDoRequests.GetById);
            app.MapPost("/todos", ToDoRequests.Create);
            app.MapPut("/todos/{id}", ToDoRequests.Update);
            app.MapDelete("/todos/{id}", ToDoRequests.Delete);

            return app;
        }

        public static IResult GetAll(IToDoService service)
        {
            var todos = service.GetAll();
            return Results.Ok(todos);
        }

        public static IResult GetById(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);

            if (todo == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(todo);
        }

        public static IResult Create(IToDoService service, ToDo toDo)
        {
            service.Create(toDo);

            return Results.Created($"/todos/{toDo.Id}", toDo);
        }

        public static IResult Update(IToDoService service, Guid id, ToDo toDo)
        {
            var todo = service.GetById(id);

            if (todo == null)
            {
                return Results.NotFound();
            }

            service.Update(todo);

            return Results.NoContent();
        }

        public static IResult Delete(IToDoService service, Guid id)
        {
            var todo = service.GetById(id);

            if (todo == null)
            {
                return Results.NotFound();
            }

            service.Delete(id);

            return Results.NoContent();
        }
    }
}
