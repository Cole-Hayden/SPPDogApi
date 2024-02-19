using System.Linq.Expressions;
using DataAccess.Models;
using SPPConsole;

public sealed class StartApplication : IStartApplication
{
    IDogClient _client;
    public StartApplication(IDogClient client)
    {
        _client = client;
    }
    public async Task Start(string? dogBreed)
    {
        DogModel newDog = null;
        if (await _client.IsDogCached(dogBreed) == false)
        {
            newDog = await _client.CheckForKindOfBreedAndSelectDog(dogBreed);
            if (newDog != null)
            {
                await _client.InsertDogImageAndBreed(newDog);
            }
        }
    }
}
