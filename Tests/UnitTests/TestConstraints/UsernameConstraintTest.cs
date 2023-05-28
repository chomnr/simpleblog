using Application.Common;

namespace Tests.UnitTests.TestConstraints;

public class UsernameConstraintTest
{
    [SetUp]
    public void SetUp(){}
    
    [TestCase("Zeljko")]
    [TestCase("JohnDoe")]
    [TestCase("HenryHhhh")]
    [TestCase("SloppyJoes")]
    [TestCase("UnderScoreDude_")]
    public void ShouldReturnTrueBecauseUsernameIsValid(string username)
    {
        Assert.That(Constraints.IsValidUsername(username).Succeeded);
    }
    
    [TestCase("A")]
    [TestCase("BB")]
    public void ShouldReturnFalseBecauseUsernameTooShort(string username)
    {
        Assert.That(!Constraints.IsValidUsername(username).Succeeded);
    }
    
    [TestCase("@@@")]
    [TestCase("!!!!#$")]
    [TestCase("$$$$$$")]
    [TestCase("(((((((*")]
    [TestCase(")))))))))")]
    [TestCase("&&&&&&&&&&&&")]
    [TestCase("He nry")]
    public void ShouldReturnFalseBecauseUsernameHasIllegalCharacters(string username)
    {
        Assert.That(!Constraints.IsValidUsername(username).Succeeded);
    }
    
    [TestCase("ddddddddddddddddd")]
    [TestCase("dddddddddddddddd_")]
    public void ShouldReturnFalseBecauseUsernameHasTooLong(string username)
    {
        Assert.That(!Constraints.IsValidUsername(username).Succeeded);
    }
}