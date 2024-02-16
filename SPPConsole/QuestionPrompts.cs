using DataAccess.Models;
using System.Net.Http.Json;

namespace SPPConsole;

public class QuestionPrompts
{
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
                //Log statement if you can't find a image in the database
                //Console.WriteLine($"Error connecting to {url} STATUS_CODE: {response.StatusCode} REASON PHRASE: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error connecting to {url} Exception {e.Message}");
            throw;
        }
        return true;
    }

    public async Task<DogModel> IsDogImageInExternalApi(string dogBreed)
    {
        DogModel newDog;
        string getUrl = $"{Constants.LOCAL_HOST_NAME}/GetDogImageFromApi/{dogBreed}";
        try
        {
            HttpResponseMessage getResponse = await ApiHelper.ApiClient.GetAsync(getUrl);
            if (getResponse.IsSuccessStatusCode)
            {
                RandomImageResponse result = await getResponse.Content.ReadFromJsonAsync<RandomImageResponse>();

                Console.WriteLine("\n" + result.Message + "\n");

                if (result.Message == Constants.BREED_NOT_FOUND)
                {
                    return null;
                }
                else
                {
                    newDog = new DogModel(result.Message, dogBreed);
                }
            }
            else
            {
                //Log errors to console
                //Console.WriteLine($"Error connecting to {getUrl} STATUS_CODE: {getResponse.StatusCode} REASON PHRASE: {getResponse.ReasonPhrase}");
                Console.WriteLine("\nImproper request. Please try again\n");
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error connecting to {getUrl} Exception {e.Message}");
            throw;
        }
        return newDog;
    }
    public async Task InsertDogImageAndBreed(DogModel newDog)
    {
        string insertUrl = $"{Constants.LOCAL_HOST_NAME}/InsertDogBreedAndImageIntoDb";
        try
        {
            HttpResponseMessage insertResponse = await ApiHelper.ApiClient.PostAsJsonAsync($"{insertUrl}", newDog);
            //Console.WriteLine("Successfully inserted")
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error connecting to {insertUrl} Exception {e.Message}");
            throw;
        }
    }
}