using DataAccess.Models;

namespace SPPConsole;

public interface IDogClient
{

    Task<bool> IsDogCached(string dogBreed);
    Task<DogModel> CheckForKindOfBreedAndSelectDog(string dogBreed);
    Task InsertDogImageAndBreed(DogModel newDog);
    Task<DogModel> SelectDog(string url, string dogBreed);
    DogModel CreateDog(string message, string dogBreed);

}