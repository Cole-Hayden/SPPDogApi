using System.Net;
using DataAccess.Data;
using DataAccess.Models;

namespace SPPDogApiWrapper.Service;

public class DogService : IDogService
{
    
    IDogData _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DogService> _logger;
   public DogService(IDogData db, IHttpClientFactory httpClientFactory, ILogger<DogService> logger)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    public async Task<DogModel> SelectDog(string dogBreed)
    {
        DogModel? dog;
        try
        {
            dog = await _db.GetDog(dogBreed);
            if (dog != null)
            {
                return dog;
            }
            else
            {
                dog = await CheckForKindOfBreed(dogBreed);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in SelectAndInsertDog using DOGBREED = {dogBreed} EXCEPTION = {e.Message}");
            throw;
        }
        return dog;
    }
    public async Task<DogModel> CheckForKindOfBreed(string dogBreed)
    {
        string url;
        DogModel dog;
        dogBreed = dogBreed.ToLower();
        if (dogBreed.Contains(' '))
        {
            //master breed will always be first and sub breed will always be last in input
            string[] breeds = dogBreed.Split(" ");
            url = $"{breeds[1]}/{breeds[0]}{Constants.DOG_API_URL_END}";
        }
        else
        {
            url = $"{dogBreed}{Constants.DOG_API_URL_END}";
        }
        dog = await GetDogImageFromApi(dogBreed, url);
        return dog;
    }
    public async Task<DogModel> GetDogImageFromApi(string dogBreed, string url)
    {
        DogModel newDog;
        try
        {
            var _client = _httpClientFactory.CreateClient("DogApi");
            var response = await _client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                RandomImageResponse? apiResult = await response.Content.ReadFromJsonAsync<RandomImageResponse>();
                newDog = new DogModel(apiResult.Message, dogBreed);
                await InsertDog(newDog);
            }
            else
            {
                return null;
            }
            return newDog;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in GetDogImageFromApi using DOGBREED = {dogBreed} and url = {url} EXCEPTION = {e.Message}");
            throw;
        }
    }
    public async Task InsertDog(DogModel dog)
    {
        try
        { 
            await _db.InsertDog(dog);
        }
        catch(Exception e)
        {
            _logger.LogError($"Error in InsertDog DOG = {dog} EXCEPTION = {e.Message}");
            throw;
        }
    }
}