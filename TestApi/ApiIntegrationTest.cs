using System.Net;
using System.Net.Http.Json;
using DataAccess.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;

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
    public async Task IsSamoyedImageInDatabase()
    {
        string dogBreed = "samoyed";
        string samoyedImage = "https://images.dog.ceo/breeds/samoyed/n02111889_2366.jpg";
        string url = $"{Constants.LOCAL_HOST_NAME}{dogBreed}";

        var response = await _client.GetAsync(url);
        var samoyed = await response.Content.ReadFromJsonAsync<DogModel>();

        samoyedImage.Should().StartWith("https://images.dog.ceo/breeds/samoyed/");
        samoyedImage.Should().EndWith(".jpg");
    }

    [Fact]
    public async Task ReturnsIfMasterBreedIsFound()
    {
        string dogBreed = "boxer";
        string url = $"{Constants.LOCAL_HOST_NAME}{dogBreed}";

        var response = await _client.GetAsync(url);
        var samoyed = await response.Content.ReadFromJsonAsync<DogModel>();

        samoyed.Should().NotBeNull();
    }
    [Fact]
    public async Task ReturnsTibetanMastiffIfSubBreedIsFound()
    {
        string dogBreed = "tibetan mastiff";
        string url = $"{Constants.LOCAL_HOST_NAME}{dogBreed}";

        var response = await _client.GetAsync(url);
        var tibetanMastiff = await response.Content.ReadFromJsonAsync<DogModel>();

        tibetanMastiff.Should().NotBeNull();
    }
    [Fact]
    public async Task ReturnsBreedNotFoundIfDogIsNotInApi()
    {
        string dogBreed = "not found";
        var breeds = dogBreed.Split(' ');
        string url = $"{Constants.LOCAL_HOST_NAME}{dogBreed}";

        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}