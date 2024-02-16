using DataAccess.DbAccess;
using SPPDogApiWrapper;
using DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IDogData, DogData>();

var app = builder.Build();

app.UseHttpsRedirection();
app.ConfigureApi();

app.Run();