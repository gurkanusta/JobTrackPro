using FluentValidation;

namespace JobTrackPro.Application.Jobs.Commands.UpdateJob;

public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Job id is required.");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters.");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Position is required.")
            .MaximumLength(100).WithMessage("Position cannot exceed 100 characters.");

        RuleFor(x => x.ApplicationDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Application date cannot be in the future.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}