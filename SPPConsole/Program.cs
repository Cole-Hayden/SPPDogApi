using SPPConsole;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

ServiceCollection services = new();
services.AddSingleton<IStartApplication, StartApplication>();

StartApplication app = new StartApplication();

bool tryAgain = true;
ApiHelper.InitializeClient();
while (tryAgain == true)
{
    Console.WriteLine("\nPlease enter a dog breed to fetch a image.");
    string? dogBreed = Console.ReadLine();
    await app.SelectBreed(dogBreed);
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

/*
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
}*/