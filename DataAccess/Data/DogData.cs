using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data;

public class DogData : IDogData
{
    private readonly ISqlDataAccess _db;
    public DogData(ISqlDataAccess db)
    {
        _db = db;
    }
    public async Task<DogModel?> GetDog(string dogBreed)
    {
        var results = await _db.LoadData<DogModel, dynamic>("dbo.Dog_Get", new { dogBreed = dogBreed });
        return results.FirstOrDefault();
    }
    public Task InsertDog(DogModel dog) =>
        _db.SaveData("dbo.Dog_Insert", new {dog.Breed, dog.Image});

}