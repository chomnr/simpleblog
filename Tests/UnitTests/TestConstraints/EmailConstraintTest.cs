using Application.Common;
using Moq;

namespace Tests.UnitTests.TestConstraints;

public class EmailConstraintTest
{
    [SetUp]
    public void SetUp(){}

    [TestCase("test@example.com")]
    [TestCase("john.doe@test.co.uk")]
    [TestCase("user123@test.net")]
    [TestCase("johnhoe@outlook.com")]
    public void ShouldReturnTrueBecauseEmailIsValid(string email)
    {
        Assert.That(Constraints.IsValidEmail(email));
    }
    
    [TestCase("dddddd@example")]
    [TestCase("dsdsdsdssdd@example.=")]
    [TestCase("test/@example")]
    [TestCase("test/@.com")]
    [TestCase("tes@.com")]
    [TestCase("tes.d@example")]
    [TestCase("tesg @example.com")]
    public void ShouldReturnFalseBecauseEmailIsNotValid(string email)
    {
        Assert.That(!Constraints.IsValidEmail(email));
    }
}