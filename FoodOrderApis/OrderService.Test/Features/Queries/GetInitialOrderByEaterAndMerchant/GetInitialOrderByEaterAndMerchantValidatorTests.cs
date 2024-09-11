using FluentAssertions;
using OrderService.Data.Requests;
using OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;

namespace OrderService.Test.Features.Queries.GetInitialOrderByEaterAndMerchant;

[TestFixture]
public class GetInitialOrderByEaterAndMerchantValidatorTests
{
    private readonly GetInitialOrderByEaterAndMerchantValidator _validator;
    private GetInitialOrderByEaterAndMerchantInput _validRequest;

    public GetInitialOrderByEaterAndMerchantValidatorTests()
    {
        _validator = new GetInitialOrderByEaterAndMerchantValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = "aaaaa",
            MerchantId = "bbbbb",
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidEaterIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("EaterId cannot be null");
        yield return new TestCaseData(string.Empty)
            .SetName("EaterId cannot be empty");
    }
    
    private static IEnumerable<TestCaseData> InvalidMerchantIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("EaterId cannot be null");
        yield return new TestCaseData(string.Empty)
            .SetName("EaterId cannot be empty");
    }

    #endregion

    #region Setup Tests

    [Test]
    public void Validate_ShouldBeValid()
    {
        // arrange
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = _validRequest
        };
        
        // act
        var actual = _validator.Validate(query);
        
        // Assert
        actual.IsValid.Should().BeTrue();
    }
    
    [Test, TestCaseSource(nameof(InvalidEaterIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidEaterId(string invalidEaterId)
    {
        // arrange
        var request = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = invalidEaterId,
            MerchantId = _validRequest.MerchantId,
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = request 
        };
        
        // act
        var actual = _validator.Validate(query);
        
        // Assert
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidMerchantIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidMerchantId(string invalidMerchantId)
    {
        // arrange
        var request = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = _validRequest.EaterId,
            MerchantId = invalidMerchantId,
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = request 
        };
        
        // act
        var actual = _validator.Validate(query);
        
        // Assert
        actual.IsValid.Should().BeFalse();
    }

    #endregion
}
