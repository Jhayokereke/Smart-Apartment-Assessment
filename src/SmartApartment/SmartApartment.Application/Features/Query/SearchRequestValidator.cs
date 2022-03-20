using FluentValidation;
using SmartApartment.Application.Dtos;

namespace SmartApartment.Application.Features.Query
{
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(s => s.SearchPhrase).NotEmpty().WithMessage("A word or phrase is required for the search");
            RuleFor(s => s.Markets[0]).NotEmpty().WithMessage("At least one market should be selected");
            RuleFor(s => s.Limit).GreaterThanOrEqualTo(10).WithMessage("Minimum page size of 10");
            RuleFor(s => s.Pagenumber).GreaterThan(0).WithMessage("Page cannot be less than 1");
        }
    }
}
