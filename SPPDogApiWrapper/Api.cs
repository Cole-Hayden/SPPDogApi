namespace SPPDogApiWrapper;
using System.Net.Http.Json;
using DataAccess.Models;
using DataAccess.Data;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/GetCachedDogImage/{dogBreed}", GetCachedDogImage);
        app.MapGet("/GetDogImageFromApi/{dogBreed}", GetDogImageFromApi);
        app.MapGet("/GetSubBreedListFromApi/{masterBreed}/{subBreed}", GetSubBreedListFromApi);
        app.MapPost("/InsertDogBreedAndImageIntoDb", InsertDogBreedAndImageIntoDb);
        ApiHelper.InitializeClient();
    }
    private static async Task<IResult> GetCachedDogImage(string dogBreed, IDogData data)
    {
        try
        {
            var result = await data.GetDog(dogBreed);
            if (result == null) return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    private static async Task<IResult> GetDogImageFromApi(string dogBreed)
    {
        var url = $"{Constants.DOG_API_URL_START}{dogBreed}{Constants.DOG_API_URL_END}";
        try
        {
            var response = await ApiHelper.ApiClient.GetAsync(url);
            RandomImageResponse res = await response.Content.ReadFromJsonAsync<RandomImageResponse>();
            return Results.Ok(res);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    private static async Task<IResult> GetSubBreedListFromApi(string masterBreed, string subBreed)
    {
        var url = $"{Constants.DOG_API_URL_START}{masterBreed}/{subBreed}{Constants.DOG_API_URL_END}";
        try
        {
            var response = await ApiHelper.ApiClient.GetAsync(url);
            RandomImageResponse res = await response.Content.ReadFromJsonAsync<RandomImageResponse>();
            return Results.Ok(res);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    private static async Task<IResult> InsertDogBreedAndImageIntoDb(DogModel dog, IDogData data)
    {
        try
        {
            await data.InsertDog(dog);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}