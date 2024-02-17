using SPPConsole;
using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();


ServiceProvider serviceProvider = new ServiceCollection()
    .AddLogging((loggingBuilder) => loggingBuilder
        .SetMinimumLevel(LogLevel.Error)
        .AddConsole()
        ).AddSingleton<IDogClient, DogClient>()
    .BuildServiceProvider();

IDogClient client = serviceProvider.GetService<IDogClient>();

bool tryAgain = true;
ApiHelper.InitializeClient();

while (tryAgain == true)
{
    DogModel newDog = null;
    Console.WriteLine("\nPlease enter a dog breed to fetch a image.");
    string? dogBreed = Console.ReadLine();

    if (await client.IsDogCached(dogBreed) == false)
    {
        newDog = await client.CheckForKindOfBreedAndSelectDog(dogBreed);
        if (newDog != null)
        {
            await client.InsertDogImageAndBreed(newDog);
        }
    }
    do
    {
        Console.WriteLine(@"Would you like to try again?  Please type in Y or N");
        ConsoleKey keyInfo = Console.ReadKey().Key;
        if (keyInfo == ConsoleKey.N)
        {
            Console.WriteLine("\nThank you for using the application.  Goodbye!");
            return;
        }
        else if (keyInfo == ConsoleKey.Y)
        {
            break;
        }
    }
    while (tryAgain == true);
}