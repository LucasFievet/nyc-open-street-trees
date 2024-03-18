# StreetTree API Setup Guide

This guide provides instructions on how to set up the StreetTree API on your local environment. Follow these steps to download the required data, generate the SQLite database, and run the API service.

## Prerequisites

Before you begin, ensure you have the following installed:
- .NET 7.0 SDK or later
- SQLite3

## Step 1: Download the CSV Data File

The StreetTree API requires a CSV file containing tree data to populate the database. Perform the following steps to download this file:

1. Visit the [NYC Open Data portal](https://data.cityofnewyork.us/Environment/2015-Street-Tree-Census-Tree-Data/pi5s-9p35) (or the relevant data source for the most recent tree census data).
2. Look for the option to download the data in CSV format.
3. Save the CSV file to your project directory, ideally in a folder named `Data` for easy access.

## Step 2: Generate the SQLite Database

After downloading the CSV file, you'll need to generate the SQLite database that the API will use to retrieve data.

1. Place the CSV file in the root directory of your project or a specific folder you'll reference in the code.
2. Navigate to your project directory in the terminal or command prompt.
3. Run the loader module to create the SQLite database and populate it with data from the CSV file. Ensure the path to the CSV file in the loader script matches the location where you saved the CSV file.
   ```bash
   dotnet run --project StreetTreeLoader
   ```

## Step 3: Copy the SQLite Database to the API Project
After generating the SQLite database, you need to ensure it's accessible to the API service.

Copy the generated SQLite database file (street-trees.db) to a directory within your API project, such as Data.
Update the connection string in the API project to reflect the path to the database file.

## Step 4: Run the API Service
Start the API service in HTTP mode via Visual Studio or the command line. Once the service is running, you can access the Swagger UI by navigating to http://localhost:5283/swagger/index.html in your web browser.

## Unit Tests
Use the Visual Studio test explorer to run the unit tests
