﻿using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public void TestSetUp()
    {
         ResetState();
    }
}