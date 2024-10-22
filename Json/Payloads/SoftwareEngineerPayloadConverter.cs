using System;
using System.Text.Json;
using TestJsonCustomConverter.Models;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter.Json.Payloads;

public class SoftwareEngineerPayloadConverter : BasePayloadConverter
{
  public override Person InsertPayloadToPerson(ref Utf8JsonReader reader, Person person)
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
    return new Person()
    {
      Payload = payload,
      Name = person.Name,
      Occupation = person.Occupation
    };
  }

  public override Person InsertPayloadToPerson(JsonElement rawPayload, Person person)
  {

    if (rawPayload.ValueKind != JsonValueKind.Object)
    {
      throw new Exception("Invalid JSON object");
    }

    SoftwareEngineerPayload payload = new SoftwareEngineerPayload();

    foreach (JsonProperty property in rawPayload.EnumerateObject())
    {
      switch (property.Name)
      {
        case "age":
          payload.Age = property.Value.GetInt32();
          break;
        default:
          throw new JsonException($"Invalid property found: {property.Name}");
      }
    }

    return new Person(person, payload);
  }
}
