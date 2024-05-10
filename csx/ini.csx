#r "nuget: Microsoft.Extensions.Configuration, 8.0.0"
#r "nuget: Microsoft.Extensions.Configuration.Ini, 8.0.0"

using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddIniFile("./env.ini");

var configuration = builder.Build();

var server = configuration["Settings:Server"];
var database = configuration["Settings:Database"];

Console.WriteLine($"Server: {server}");
Console.WriteLine($"Database: {database}");
