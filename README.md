# SPPDogApi


You can run these projects via the dotnet run command.  Start the SPPDogApiWrapper project to get the api up and running and then you can start the SPPConsole app to query the database and api.

In order to test with your database, go to the appsettings.json and look for 

"ConnectionStrings": {
    "Default": "Server=localhost; Database=master; User Id=SA; Password=Password1"
  }

^Update the above with your own db credentials

Also the console app is looking at http://localhost:5039 by default, you can modify that for your own testing in the Constants.cs class in the SPPConsole project

This is the sql script you need for the tables and stored procedures that the app uses

create table Dog (ID int PRIMARY KEY IDENTITY, Breed varchar(80), Image varchar(300));

CREATE PROCEDURE Dog_Get
@DogBreed varchar(80)
AS 
    Select Image from Dog where Breed = @DogBreed
GO

CREATE PROCEDURE Dog_Insert
@Breed varchar(80),
@Image varchar(300)
AS
    Insert into Dog (Breed, Image)
    VALUES (@Breed, @Image)
GO



Prior to beginning I estimated this project would take about 5 hours or so as I wanted to make sure everything was done properly.

So for this project I'm using dapper as its lightweight and then also using stored procedures to select from the database.  I made the sqldataaccess class generic so if you were to want to extend the application with different animals you could reuse that functionality.  Piggybacking off of that, depending on if the new cat  used the same GetDog and InsertDog functionalities, I would just make an Animal interface and have both the cat and dog implement that interface.  You could then use different stored procedures for the cat but re use the sqldataaccess functions.  You could also have an AnimalClient and just pass the animal object into that client depending on what kind of animal it is. 

The program is straightforward, if there is data in the database then you will pull that image and be asked if you’d like to start the app again or not.  If there isn’t an image, it will convert the text to lowercase and then if there is a space detected in the text it will attempt to hit the api that is responsible for sub breeds.  This is done because sub breed dogs will always have a space in their breed name while master breed names will never have any spaces.  If the api pulls data back, the program will then insert the dog breed and image into the database.

I don’t believe the api is production ready as it hasn't been tested on an external server and there also hasn’t been any security analysis or performance testing done on the project.  I also believe that you should log the log statements to the database as well as have more testing before moving to prod.
