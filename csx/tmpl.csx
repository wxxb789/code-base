#load "reference.csx"

using Newtonsoft.Json;

var json = @"{ 'Name': 'John Doe', 'Age': 30 }";
var person = JsonConvert.DeserializeObject<dynamic>(json);

Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
Console.WriteLine(233);