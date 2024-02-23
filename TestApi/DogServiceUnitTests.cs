using System.Net;
using System.Net.Http.Json;
using DataAccess.Data;
using DataAccess.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using SPPDogApiWrapper.Service;
using RichardSzalay.MockHttp;

namespace TestApi;

public class DogServiceUnitTests
{
    private readonly IDogService _sut;
    private readonly IDogData _dogDataRepository = Substitute.For<IDogData>();
    private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
    private readonly MockHttpMessageHandler _handlerMock = new MockHttpMessageHandler();
    private readonly ILogger<DogService> _logger = Substitute.For<ILogger<DogService>>();
    
    public DogServiceUnitTests()
    {
        _sut = new DogService(_dogDataRepository, _httpClientFactory, _logger);
    }
    [Fact]
    public async Task SelectDog_ShouldReturnADog_WhenDogIsInDb()
    {
        string dogBreed = "Samoyed";
        DogModel dog = new DogModel("test", "Samoyed");
        _dogDataRepository.GetDog(dogBreed).Returns(dog);

        DogModel result = await _sut.SelectDog(dogBreed);

        result.Should().BeEquivalentTo(dog);
    }
    [Fact]
    public async Task SelectDog_ShouldGetADogFromApi_WhenDogIsNotInDb()
    {
        DogModel dog = new DogModel("test", "test");
        string dogBreed = "test";
        _handlerMock.When("*")
            .Respond(HttpStatusCode.OK, JsonContent.Create(new
            {
                message = "test"
            }));
        _httpClientFactory.CreateClient("DogApi").Returns(new HttpClient(_handlerMock)
        {
            BaseAddress = new Uri("https://test.com")
        });
        _dogDataRepository.GetDog(dogBreed).ReturnsNull();
        
        DogModel resultDog = await _sut.SelectDog(dogBreed);
        
        dog.Should().BeEquivalentTo(resultDog);
    }
    [Fact]
    public async Task SelectDog_ShouldReturnException_WhenGetDogMethodFails()
    {
        string dogBreed = "tibetan mastiff";
        _dogDataRepository.GetDog(dogBreed).ThrowsAsyncForAnyArgs(new Exception());
        
        var result = async () => await _sut.SelectDog(dogBreed);
        await result.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task CheckForKindOfBreed_ShouldReturnMasterBreed_WhenDogBreedHasNoSpace()
    {
        DogModel dog = new DogModel("test", "samoyed");
        string dogBreed = "samoyed";
        _handlerMock.When("*").Respond(HttpStatusCode.OK, JsonContent.Create(new
            {
                message = "test"
            }));
        _httpClientFactory.CreateClient("DogApi").Returns(new HttpClient(_handlerMock)
        {
            BaseAddress = new Uri("https://test.com")
        });
        
        DogModel result = await _sut.CheckForKindOfBreed(dogBreed);

        result.Should().BeEquivalentTo(dog);
    }

    [Fact]
    public async Task GetDogImageFromApi_ShouldReturnNull_WhenTheresNotASuccessRequest()
    {
        DogModel dog = new DogModel("test", "SAMOYED");
        string dogBreed = "SAMOYED";
        string url = "test";
        _handlerMock.When("*").Respond(HttpStatusCode.Forbidden, JsonContent.Create(new
        {
            message = "test"
        }));
        _httpClientFactory.CreateClient("DogApi").Returns(new HttpClient(_handlerMock)
        {
            BaseAddress = new Uri("https://test.com")
        });
        
        DogModel result = await _sut.GetDogImageFromApi(dogBreed, url);
        result.Should().BeNull();
    }
    [Fact]
    public async Task GetDogImageFromApi_ShouldReturnADog_WhenTheresASuccessRequest()
    {
        DogModel dog = new DogModel("test", "SAMOYED");
        string dogBreed = "SAMOYED";
        string url = "test";
        _handlerMock.When("*").Respond(HttpStatusCode.OK, JsonContent.Create(new
        {
            message = "test"
        }));
        _httpClientFactory.CreateClient("DogApi").Returns(new HttpClient(_handlerMock)
        {
            BaseAddress = new Uri("https://test.com")
        });
        
        DogModel result = await _sut.GetDogImageFromApi(dogBreed, url);
        result.Should().BeEquivalentTo(dog);
    }
    [Fact]
    public async Task InsertDog_ShouldHaveAnException_WhenTheresAnErrorCallingDatabase()
    {
        string message = "This is a dog image";
        string dogBreed = "cardigan corgi";
        DogModel newDog = new DogModel(message, dogBreed);
        _dogDataRepository.InsertDog(newDog).ThrowsAsyncForAnyArgs(new Exception());
        
        var result = async () => await _sut.InsertDog(newDog);
        await result.Should().ThrowAsync<Exception>();
    }
}