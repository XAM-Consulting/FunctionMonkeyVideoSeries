using System;
using FluentValidation;
using ServerlessBlog.Commands;

namespace ServerlessBlog.Application.Validators
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(cmd => cmd.Body).NotNull();
            RuleFor(cmd => cmd.Title).NotNull();
            RuleFor(cmd => cmd.UserId).NotNull();
        }
    }
}
