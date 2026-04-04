using FluentValidation;

using JobTrackPro.Application.Jobs.Commands.CreateJob;

namespace JobTrackPro.Application.Features.Jobs.Commands.CreateJob;

public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name cannot be empty.")
            .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters.");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Position cannot be empty.")
            .MaximumLength(200).WithMessage("Position cannot exceed 200 characters.");

        RuleFor(x => x.JobUrl)
     .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
     .When(x => !string.IsNullOrEmpty(x.JobUrl))
     .WithMessage("Please enter a valid URL for the job posting.");
        RuleFor(x => x.CompanyUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.CompanyUrl))
            .WithMessage("Please enter a valid URL for the company website.");

        RuleFor(x => x.InterviewDate)
            .GreaterThan(DateTime.UtcNow.AddDays(-1))
            .When(x => x.InterviewDate.HasValue)
            .WithMessage("Interview date cannot be in the past.");
    }
}