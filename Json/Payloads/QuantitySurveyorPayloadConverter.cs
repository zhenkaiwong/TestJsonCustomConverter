using System;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using TestJsonCustomConverter.Models;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter.Json.Payloads;

public class QuantitySurveyorPayloadConverter : BasePayloadConverter
{

  public override Person InsertPayloadToPerson(ref Utf8JsonReader reader, Person person)
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

    return new Person(person, payload);
  }

  public override Person InsertPayloadToPerson(JsonElement rawPayload, Person person)
  {
    if (rawPayload.ValueKind != JsonValueKind.Object)
    {
      throw new JsonException("Invalid object");
    }

    var payload = new QuantitySurveyorPayload();

    foreach (JsonProperty property in rawPayload.EnumerateObject())
    {
      switch (property.Name)
      {
        case "hobby":
          payload.Hobby = property.Value.GetString();
          break;
        default:
          throw new JsonException($"Invalid property found: {property.Name}");
      }
    }

    return new Person(person, payload);
  }
}
