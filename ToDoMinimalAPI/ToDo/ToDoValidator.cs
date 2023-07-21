using FluentValidation;

namespace ToDoMinimalAPI.ToDo
{
    public class ToDoValidator : AbstractValidator<ToDo>
    {
        public ToDoValidator()
        {
            RuleFor(t => t.Value)
                .NotEmpty()
                .MinimumLength(5)
                .WithMessage("Value of a todo must be atleast 5 characters.");
        }
    }
}
