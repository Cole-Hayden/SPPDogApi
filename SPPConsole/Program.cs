using SPPConsole;
using DataAccess.Models;

bool tryAgain = true;
ApiHelper.InitializeClient();

QuestionPrompts question = new QuestionPrompts();

while (tryAgain == true)
{
    Console.WriteLine("\nPlease enter a dog breed to fetch a image.");
    string? dogBreed = Console.ReadLine();

    if (await question.IsDogCached(dogBreed) == false)
    {
        DogModel newDog = await question.IsDogImageInExternalApi(dogBreed);
        if (newDog != null)
        {
            await question.InsertDogImageAndBreed(newDog);
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