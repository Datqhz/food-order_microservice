using CustomerService.Data.Requests;
using CustomerService.Features.Queries.UserInfoQueries.SearchMerchantsByName;
using FluentAssertions;

namespace CustomerService.Test.Features.Queries.SearchMerchantByName;

public class SearchMerchantByNameValidatorTests
{
    private SearchMerchantsByNameValidator _validator;
    private SearchMerchantsByNameRequest _validRequest;

    [SetUp]
    public void Setup()
    {
        _validator = new SearchMerchantsByNameValidator();
        _validRequest = new SearchMerchantsByNameRequest
        {
            Keyword = "aaaa"
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidKeywordsTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Keyword is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Keyword is required");
    }

    #endregion

    #region Setup Tests

    [Test]
    public void Validate_ShouldReturnValid_WhenGivenValidRequest()
    {
        var query = new SearchMerchantsByNameQuery()
        {
            Payload = _validRequest
        };
        
        var act = _validator.Validate(query);

        act.IsValid.Should().BeTrue();
    }
    
    [Test, TestCaseSource(nameof(InvalidKeywordsTestCases))]
    public void Validate_ShouldReturnInvalid_WhenGivenInvalidKeyword(string invalidKeyword)
    {
        var query = new SearchMerchantsByNameQuery()
        {
            Payload = new SearchMerchantsByNameRequest
            {
                Keyword = invalidKeyword
            }
        };
        
        var act = _validator.Validate(query);

        act.IsValid.Should().BeFalse();
    }

    #endregion
}
