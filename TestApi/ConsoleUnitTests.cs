using AutoFixture;
using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Moq;
using SPPConsole;

namespace TestApi;

public class ConsoleUnitTests
{
    Mock<IDogClient> _client;
    Mock<ILogger<DogClient>> _logger;
    public ConsoleUnitTests()
    {
        _client = new();
        _logger = new();
    }

    [Fact]
    public async Task IfDogIsCached_DontCheckForKindOfBreedAndSelectDog()
    {
        StartApplication app = new StartApplication(_client.Object);
        _client.Setup(x => x.IsDogCached(It.IsAny<string>())).ReturnsAsync(true);

        await app.Start(It.IsAny<string>());

        _client.Verify(x => x.CheckForKindOfBreedAndSelectDog(It.IsAny<string>()), Times.Never());
    }
    [Fact]
    public async Task IfDogIsNOTCached_CheckForKindOfBreedAndSelectDog()
    {
        StartApplication app = new StartApplication(_client.Object);
        _client.Setup(x => x.IsDogCached(It.IsAny<string>())).ReturnsAsync(false);

        await app.Start(It.IsAny<string>());

        _client.Verify(x => x.CheckForKindOfBreedAndSelectDog(It.IsAny<string>()), Times.Once());
    }
    [Fact]
    public async Task IfCheckForKindOfBreedAndSelectDog_ReturnsTrue_InsertDog()
    {
        var fixture = new Fixture();
        var dog = fixture.Create<DogModel>();

        StartApplication app = new StartApplication(_client.Object);
        _client.Setup(x => x.IsDogCached(It.IsAny<string>())).ReturnsAsync(false);
        _client.Setup(x => x.CheckForKindOfBreedAndSelectDog(It.IsAny<string>())).ReturnsAsync(dog);

        await app.Start(It.IsAny<string>());

        _client.Verify(x => x.InsertDogImageAndBreed(It.IsAny<DogModel>()), Times.Once());
    }
    [Fact]
    public void IfDog_EqualsBREEDNOTFOUND_ReturnNull()
    {
        DogClient client = new DogClient(_logger.Object);
        var result = client.CreateDog(SPPConsole.Constants.BREED_NOT_FOUND, "test");

        Assert.Null(result);
    }
}