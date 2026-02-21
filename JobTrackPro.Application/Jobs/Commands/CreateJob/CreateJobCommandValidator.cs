using FluentValidation;

using JobTrackPro.Application.Jobs.Commands.CreateJob;

public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(100);

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Position is required");

        RuleFor(x => x.ApplicationDate)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}