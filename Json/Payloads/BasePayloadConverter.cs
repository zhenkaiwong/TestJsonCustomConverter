using System;
using System.Text.Json;
using TestJsonCustomConverter.Models;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter.Json.Payloads;

public abstract class BasePayloadConverter
{
  public abstract Person InsertPayloadToPerson(ref Utf8JsonReader reader, Person person);
  public abstract Person InsertPayloadToPerson(JsonElement rawPayload, Person person);
}
