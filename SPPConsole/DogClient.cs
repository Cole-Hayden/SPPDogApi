using DataAccess.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace SPPConsole;

public sealed class DogClient : IDogClient
{
    private string url;
    ILogger<DogClient> _logger;
    public DogClient(ILogger<DogClient> logger)
    {
        _logger = logger;
    }
    public async Task<bool> IsDogCached(string dogBreed)
    {
        string url = $"{Constants.LOCAL_HOST_NAME}/GetCachedDogImage/{dogBreed}";
        try
        {
            HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync($"{url}");
            if (response.IsSuccessStatusCode)
            {
                DogModel dog = await response.Content.ReadFromJsonAsync<DogModel>();
                Console.WriteLine("\nUsing cached dog image.\n");
                Console.WriteLine(dog.Image + "\n");
            }
            else
            {
                _logger.LogWarning($"Warning, could not connect to {url} STATUS_CODE: {response.StatusCode} REASON PHRASE: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error connecting to {url} Exception {e.Message}");
            throw;
        }
        return true;
    }
    public async Task<DogModel> CheckForKindOfBreedAndSelectDog(string dogBreed)
    {
        DogModel newDog;
        dogBreed = dogBreed.ToLower();
        if (dogBreed.Contains(' '))
        {
            //master breed will always be first and sub breed will always be last in input
            string[] breeds = dogBreed.Split(" ");
            url = $"{Constants.LOCAL_HOST_NAME}/GetSubBreedListFromApi/{breeds[0]}/{breeds[1]}";
        }
        else
        {
            url = $"{Constants.LOCAL_HOST_NAME}/GetDogImageFromApi/{dogBreed}";
        }
        try
        {
            newDog = await SelectDog(url, dogBreed);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error connecting to {url} Exception {e.Message}");
            throw;
        }
        return newDog;
    }
    public async Task<DogModel> SelectDog(string url, string dogBreed)
    {
        DogModel newDog;
        HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            RandomImageResponse result = await response.Content.ReadFromJsonAsync<RandomImageResponse>();

            Console.WriteLine("\n" + result.Message + "\n");

            newDog = CreateDog(result.Message, dogBreed);
        }
        else
        {
            //Log errors to console
            _logger.LogError($"Error connecting to {url} STATUS_CODE: {response.StatusCode} REASON PHRASE: {response.ReasonPhrase}");
            return null;
        }
        return newDog;
    }
    public DogModel CreateDog(string message, string dogBreed)
    {
        DogModel newDog;
        if (message == Constants.BREED_NOT_FOUND)
        {
            return null;
        }
        else
        {
            newDog = new DogModel(message, dogBreed);
        }
        return newDog;
    }
    public async Task InsertDogImageAndBreed(DogModel newDog)
    {
        string url = $"{Constants.LOCAL_HOST_NAME}/InsertDogBreedAndImageIntoDb";
        try
        {
            HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync($"{url}", newDog);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Image = {newDog.Image} and Breed = {newDog.Breed} inserted successfully.");
            }
            else
            {
                _logger.LogError($"Unsucessfully inserted dog image Image = {newDog.Image} and Breed = {newDog.Breed}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error connecting to {url} Exception {e.Message}");
            throw;
        }
    }
}