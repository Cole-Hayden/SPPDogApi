using SPPDogApiWrapper.Service;

namespace SPPDogApiWrapper;
using System.Net.Http.Json;
using DataAccess.Models;
using DataAccess.Data;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/GetDogImage/{dogBreed}", GetDogImage);
    }
    private static async Task<IResult> GetDogImage(string dogBreed, IDogService data)
    {
        try
        {
            var result = await data.SelectDog(dogBreed);
            if (result == null) return Results.NotFound();
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}