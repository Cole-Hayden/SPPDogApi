
namespace DataAccess.Models;

public class DogModel
{
    public DogModel()
    {

    }
    public DogModel(string Image, string Breed)
    {
        this.Image = Image;
        this.Breed = Breed;
    }
    public string Image { get; set; }
    public string Breed { get; set; }
}