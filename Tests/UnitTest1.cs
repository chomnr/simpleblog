using Tests.IntegrationTests;

namespace Tests;

using static Testing;
public class Tests
{
    [SetUp]
    public async Task Setup()
    {
        await ResetState();
    }
    
    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}