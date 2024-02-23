using DataAccess.DbAccess;
using SPPDogApiWrapper;
using DataAccess.Data;
using SPPDogApiWrapper.Service;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IDogData, DogData>();
builder.Services.AddSingleton<IDogService, DogService>();


builder.Services.AddHttpClient("DogApi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://dog.ceo/api/breed/");
});

/*ServiceProvider serviceProvider = new ServiceCollection()
    .AddLogging((loggingBuilder) => loggingBuilder
        .SetMinimumLevel(LogLevel.Error)
        .AddConsole()
    ).AddSingleton<IDogService, DogService>()
    .BuildServiceProvider();*/


var app = builder.Build();

app.UseHttpsRedirection();
app.ConfigureApi();

app.Run();

//Need this to reference when testing in apiIntegrationTest
public partial class Program { }