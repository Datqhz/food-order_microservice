using AutoFixture;

namespace OrderService.Test.Extensions;

internal static class FixtureExtensions
{
    public static Fixture OmitOnRecursionBehavior(this Fixture fixture)
    {
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
