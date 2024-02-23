
namespace SPPDogApiWrapper.Service;
using DataAccess.Models;

public interface IDogService
{
    Task<DogModel> SelectDog(string dogBreed);
    Task<DogModel> CheckForKindOfBreed(string dogBreed);
    Task<DogModel> GetDogImageFromApi(string dogBreed, string url);
    Task InsertDog(DogModel dog);

}