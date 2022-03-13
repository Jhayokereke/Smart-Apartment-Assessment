using FluentValidation;
using SmartApartment.Application.Dtos;

namespace SmartApartment.Application.Features.Query
{
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(s => s.SearchPhrase).NotEmpty().WithMessage("A word or phrase is required for the search");
            RuleFor(s => s.Limit).GreaterThanOrEqualTo(25).WithMessage("Minimum page size of 25");
            RuleFor(s => s.Pagenumber).GreaterThan(0).WithMessage("Page cannot be less than 1");
        }
    }
}
