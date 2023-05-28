using Application.Common;

namespace Tests.UnitTests.TestConstraints;

public class RealNameConstraintTest
{
    [SetUp]
    public void SetUp(){}
    
    [TestCase("Henry")]
    [TestCase("Ford")]
    [TestCase("John")]
    [TestCase("Doe")]
    [TestCase("Jones")]
    [TestCase("Myers")]
    public void ShouldReturnTrueBecauseRealNameValid(string realname)
    {
        Assert.That(Constraints.IsValidRealName(realname).Succeeded);
    }
    
    [TestCase("M yers")]
    [TestCase("Usain_")]
    public void ShouldReturnFalseBecauseRealNameNotValid(string realname)
    {
        Assert.That(!Constraints.IsValidRealName(realname).Succeeded);
    }
}