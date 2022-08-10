﻿using FluentValidation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Validation
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(course => course.Name).NotNull().NotEmpty();
            RuleFor(course => course.Description).NotNull().NotEmpty();
            RuleFor(course => course.Owner).NotNull();
            RuleFor(course => course.AccessLevel).Must(access => access >= 0);
            RuleFor(course => course.Materials).NotNull();
            RuleFor(course => course.Subscribers).NotNull();
            RuleFor(course => course.Skills).NotNull();
        }
    }
}
