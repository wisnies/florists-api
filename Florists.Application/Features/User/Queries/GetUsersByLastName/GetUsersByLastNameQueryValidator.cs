﻿using Florists.Core.Common.Messages;
using FluentValidation;

namespace Florists.Application.Features.User.Queries.GetUsersByLastName
{
  public class GetUsersByLastNameQueryValidator : AbstractValidator<GetUsersByLastNameQuery>
  {
    public GetUsersByLastNameQueryValidator()
    {
      RuleFor(x => x.LastName)
        .MaximumLength(32)
        .WithMessage(Messages.User.LastNameMaxLengthIs + 32);

      RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage(Messages.Pagination.PageNumber);

      RuleFor(x => x.PerPage)
       .GreaterThan(0)
       .WithMessage(Messages.Pagination.PerPage);
    }
  }
}
