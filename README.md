# ASP.NET Core/React URL Shortener
A basic implementation of an URL shortener web application using ASP.NET Core MVC and Entity Framework Core.

## Goal

Implement an URL shortener application .

## Algorithm


## Implementation

ShortUrl.API is a ASP.NET Core that hosts 3 methods:
* Get method to get already shortened URLs
* Get method for redirect towards to full urls
* Post method for generating short Url

ShortUrl.Frontend
	React Web app to generate short urls

## Usage

First, you have to type `dotnet restore` in order to retrieve the dependancies of the project.


The projet is using SQLite as DB backed. The data file is named `shorturls.db` by default.

In order to init the DB schema, you have to rune the command `dotnet ef database update`.

Then, simply type `dotnet run` on your command prompt and then browse to http://localhost:5000.
