using System.Text.Json;
using System.Text.Json.Serialization;
using TestJsonCustomConverter.Json.Payloads;
using TestJsonCustomConverter.Models;

namespace TestJsonCustomConverter.Json.Converters;

public class CustomJsonConverterJsonDocument : JsonConverter<List<Person>>
{
  public override List<Person>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var persons = new List<Person>();
    using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
    {
      JsonElement root = jsonDocument.RootElement;
      if (root.ValueKind != JsonValueKind.Array)
      {
        return persons;
      }
      foreach (JsonElement item in root.EnumerateArray())
      {
        string? name = item.GetProperty("name").GetString();
        string? occupation = item.GetProperty("occupation").GetString();

        if (name is null || occupation is null)
        {
          continue;
        }

        Person person = new Person()
        {
          Name = name,
          Occupation = occupation,
        };

        BasePayloadConverter converter = PayloadConverterFactory.GetConverterByPerson(person);
        JsonElement rawPayload = item.GetProperty("payload");
        person = converter.InsertPayloadToPerson(rawPayload, person);
        persons.Add(person);
      }

    }
    return persons;
  }

  public override void Write(Utf8JsonWriter writer, List<Person> value, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }
}
