using System.Net.Http.Json;
using DataAccess.Models;
using Serilog;
using SPPConsole;

public sealed class StartApplication : IStartApplication
{
    public async Task SelectBreed(string? dogBreed)
    {
        try
        {
            string url = $"{Constants.LOCAL_HOST_NAME}/GetDogImage/{dogBreed}";
            HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync($"{url}");
            if (response.IsSuccessStatusCode)
            {
                DogModel? dog = await response.Content.ReadFromJsonAsync<DogModel>();
                Console.WriteLine($"\n{dog.Image}\n");
            }
            else
            {
                Console.WriteLine("Dog was not found in the database or api.\n");
                Log.Warning($"Warning, could not connect to {url} STATUS_CODE: {response.StatusCode} REASON PHRASE: {response.ReasonPhrase}");
            }
        }
        catch (Exception e)
        {
            Log.Error($"ERROR SelectBreed DOG BREED = {dogBreed} EXCEPTION = {e.Message}");
            throw;
        }
    }
}
