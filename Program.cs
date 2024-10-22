using System.Text.Json;
using TestJsonCustomConverter.Json.Converters;
using TestJsonCustomConverter.Models;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter;

class Program
{
    static void Main(string[] args)
    {
        string jsonContent = File.ReadAllText("data.json");
        var serializeOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        // serializeOptions.Converters.Add(new CustomJsonConverter());
        serializeOptions.Converters.Add(new CustomJsonConverterJsonDocument());

        List<Person>? people = JsonSerializer.Deserialize<List<Person>>(jsonContent, serializeOptions);


        if (people is null)
        {
            Console.WriteLine("Unable to deserialize JSON content to people");
            return;
        }

        if (people.Count() == 0)
        {
            Console.WriteLine("No people");
            return;
        }

        foreach (Person person in people)
        {
            Console.WriteLine(person);
        }
    }
}
