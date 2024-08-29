﻿using AutoFixture;

namespace AuthServer.Test.Extensions;

public static class FixtureExtensions
{
    public static Fixture OmitOnRecursionBehavior(this Fixture fixture)
    {
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
