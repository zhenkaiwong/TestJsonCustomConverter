using System.Text.Json;
using System.Text.Json.Serialization;
using TestJsonCustomConverter.Json.Payloads;
using TestJsonCustomConverter.Models;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter.Json.Converters;

public class CustomJsonConverter : JsonConverter<List<Person>>
{
  protected Person? ReadPersonFromReader(ref Utf8JsonReader reader)
  {
    // meaning this is not a new person object in our array
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      return null;
    }

    int newPersonObjectDepth = reader.CurrentDepth;
    var person = new Person();

    // we should loop the reader and break when we encounter EndObject with same depth
    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == newPersonObjectDepth)
      {
        break;
      }

      if (reader.TokenType == JsonTokenType.PropertyName)
      {
        switch (reader.GetString())
        {
          case "name":
            reader.Read();
            person.Name = reader.GetString();
            break;
          case "occupation":
            reader.Read();
            person.Occupation = reader.GetString();
            break;
          case "payload":
            if (string.IsNullOrEmpty(person.Occupation))
            {
              Console.WriteLine("Reader is now at payload, but current user doesn't have occupation, which is required by reader to determine how to parse payload");
              break;
            }

            reader.Read();

            var payloadConverter = PayloadConverterFactory.GetConverterByPerson(person);
            person = payloadConverter.InsertPayloadToPerson(ref reader, person);

            break;
          default:
            Console.WriteLine($"No handler for property \"{reader.GetString()}\"");
            break;
        }
      }


    }

    return person;
  }
  public override List<Person>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var people = new List<Person>();

    int startDepth = reader.CurrentDepth;
    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndArray && reader.CurrentDepth == startDepth)
      {
        break;
      }

      Person? personFromReader = ReadPersonFromReader(ref reader);
      if (personFromReader is null)
      {
        Console.WriteLine("Fail to read person from reader");
        continue;
      }

      people.Add(personFromReader);
    }
    return people;
  }

  public override void Write(Utf8JsonWriter writer, List<Person> value, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }
}
