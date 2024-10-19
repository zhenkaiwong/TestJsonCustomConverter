using System.Text.Json;
using System.Text.Json.Serialization;
using TestJsonCustomConverter.Models;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter.Json.Converters;

public class CustomJsonConverter : JsonConverter<List<Person>>
{
  protected SoftwareEngineerPayload ReadSoftwareEngineerPayloadFromReader(ref Utf8JsonReader reader)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException("Invalid start object");
    }

    var payload = new SoftwareEngineerPayload();
    var startDepth = reader.CurrentDepth;
    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
      {
        break;
      }

      if (reader.TokenType == JsonTokenType.PropertyName)
      {
        var propertyName = reader.GetString();
        switch (propertyName)
        {
          case "age":
            reader.Read();
            payload.Age = reader.GetInt32();
            break;
          default:
            throw new JsonException($"Invalid property found: {propertyName}");
        }
      }
    }
    return payload;
  }

  protected QuantitySurveyorPayload ReadQuantitySurveyorPayloadFromReader(ref Utf8JsonReader reader)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException("Invalid start object");
    }

    var payload = new QuantitySurveyorPayload();
    var startDepth = reader.CurrentDepth;
    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
      {
        break;
      }

      if (reader.TokenType == JsonTokenType.PropertyName)
      {
        var propertyName = reader.GetString();
        switch (propertyName)
        {
          case "hobby":
            reader.Read();
            payload.Hobby = reader.GetString();
            break;
          default:
            throw new JsonException($"Invalid property found: {propertyName}");
        }
      }
    }
    return payload;
  }

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
            switch (person.Occupation)
            {
              case "Software Engineer":
                person.Payload = ReadSoftwareEngineerPayloadFromReader(ref reader);
                break;
              case "Quantity Surveyor":
                person.Payload = ReadQuantitySurveyorPayloadFromReader(ref reader);
                break;
              default:
                throw new InvalidOperationException("");
            }
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
