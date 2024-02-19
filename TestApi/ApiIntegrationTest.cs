using System.Net.Http.Json;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestApi;

public class ApiIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    HttpClient _client;
    public ApiIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task IsDbHealthy()
    {
        string dogBreed = "test";
        var _client = _factory.CreateClient();
        string url = $"{SPPConsole.Constants.LOCAL_HOST_NAME}/GetCachedDogImage/{dogBreed}";
        var response = await _client.GetAsync(url);

        Assert.True(response.IsSuccessStatusCode);
    }
    [Fact]
    public async Task IsSamoyedImageInDatabase()
    {
        string dogBreed = "samoyed";
        string samoyedImage = "https://images.dog.ceo/breeds/samoyed/n02111889_2366.jpg";
        string url = $"{SPPConsole.Constants.LOCAL_HOST_NAME}/GetCachedDogImage/{dogBreed}";

        var response = await _client.GetAsync(url);
        var samoyed = await response.Content.ReadFromJsonAsync<DataAccess.Models.DogModel>();

        Assert.Equal(samoyedImage, samoyed.Image);
    }

    [Fact]
    public async Task ReturnsSamoyedIfMasterBreedIsFound()
    {
        string dogBreed = "samoyed";
        string url = $"{SPPConsole.Constants.LOCAL_HOST_NAME}/GetDogImageFromApi/{dogBreed}";

        var response = await _client.GetAsync(url);
        var samoyed = await response.Content.ReadFromJsonAsync<RandomImageResponse>();

        Assert.NotNull(samoyed.Message);
    }

    [Fact]
    public async Task ReturnsTibetanMastiffIfSubBreedIsFound()
    {
        string dogBreed = "tibetan mastiff";
        var breeds = dogBreed.Split(' ');
        string url = $"{SPPConsole.Constants.LOCAL_HOST_NAME}/GetSubBreedListFromApi/{breeds[1]}/{breeds[0]}";

        var response = await _client.GetAsync(url);
        var tibetanMastiff = await response.Content.ReadFromJsonAsync<RandomImageResponse>();

        Assert.NotNull(tibetanMastiff.Message);
    }

    [Fact]
    public async Task ReturnsBreedNotFoundIfDogIsNotInApi()
    {
        string dogBreed = "not found";
        var breeds = dogBreed.Split(' ');
        string url = $"{SPPConsole.Constants.LOCAL_HOST_NAME}/GetSubBreedListFromApi/{breeds[1]}/{breeds[0]}";

        var response = await _client.GetAsync(url);
        var notFoundMessage = await response.Content.ReadFromJsonAsync<RandomImageResponse>();

        Assert.Equal(SPPConsole.Constants.BREED_NOT_FOUND, notFoundMessage.Message);
    }
}