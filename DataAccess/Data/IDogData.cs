using DataAccess.Models;

namespace DataAccess.Data;

public interface IDogData
{
    Task<DogModel?> GetDog(string dogBreed);
    Task InsertDog(DogModel dog);

}