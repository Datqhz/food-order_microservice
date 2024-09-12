using FluentAssertions;
using OrderService.Data.Requests;
using OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

namespace OrderService.Test.Features.Queries.GetAllOrderByUserId;

[TestFixture]
public class GetAllOrderByUserIdValidatorTests
{
    
    private readonly GetAllOrderByUserIdValidator _validator;
    private GetAllOrderByUserIdRequest _validRequest;

    public GetAllOrderByUserIdValidatorTests()
    {
        _validator = new GetAllOrderByUserIdValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new GetAllOrderByUserIdRequest
        {
            EaterId = "aaaa"
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidUserIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("UserId is required");
        yield return new TestCaseData(string.Empty)
            .SetName("UserId is required");
    }
    
    #endregion

    #region Setup Tests

    [Test]
    public void Validate_ShouldBeValid()
    {
        var query = new GetAllOrderByUserIdQuery
        {
            Payload = _validRequest
        };
        
        var actual = _validator.Validate(query);
        
        actual.IsValid.Should().BeTrue();
    }
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidEaterId(string invalidUserId)
    {

        var request = new GetAllOrderByUserIdRequest
        {
            EaterId = invalidUserId
        };
        var query = new GetAllOrderByUserIdQuery
        {
            Payload = request
        };
        
        var actual = _validator.Validate(query);
        
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidMerchantId(string invalidUserId)
    {

        var request = new GetAllOrderByUserIdRequest
        {
            EaterId = invalidUserId
        };
        var query = new GetAllOrderByUserIdQuery
        {
            Payload = request
        };
        
        var actual = _validator.Validate(query);
        
        actual.IsValid.Should().BeFalse();
    }

    #endregion
}
