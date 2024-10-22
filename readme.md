# Custom JsonSerializer Converter Practice

## Introduction

This is a practice project for me to learn how can I create a custom converter to handle the where a property in a class can contain different structure.

For example, in `data.json`:

```json
[
  {
    "name": "James",
    "occupation": "Software Engineer",
    "payload": {
      "age": 28
    }
  },
  {
    "name": "Lisa",
    "occupation": "Quantity Surveyor",
    "payload": {
      "hobby": "Play pickle ball"
    }
  }
]
```

## Challenges

To use [JsonSerializer](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to#serialization-behavior) for serializing / deserializing a JSON content, we need to have a corresponding POCO class

However, in above example json, the content of payload property can be challenging, and default converter of JsonSerializer isn't capable to map something dynamic like this.

Therefore, a custom converter will be an ideal solution to overcome the challenge like this. The idea is that it use a factory method to map `payload` to the corresponding type. You can find the implementation of this solution in the following class:

- `CustomJsonConverter` / `CustomJsonConverterJsonDocument`, these are the custom converter for JsonSerializer
- `PayloadConverterFactory`, contains factory method `GetConverterByPerson()` which take a `Person` as method input, and return a `BasePayloadConverter` according to person's occupation
- `QuantitySurveyorPayloadConverter`, responsible to convert payload to `QuantitySurveyorPayload`
- `SoftwareEngineerPayloadConverter`, responsible to convert payload to `SoftwareEngineerPayload`

You might notice that I built 2 custom converter:

1. `CustomJsonConverter`
2. `CustomJsonConverterJsonDocument`

`CustomJsonConverter` is my first custom converter. I used the reader object (`Utf8JsonReader`) to read from JSON. However, after I completed this convert, I realized that this implementation is not the best implementation, because the code is hard to read, which can lead to higher complexity

Therefore I built the second converter: `CustomJsonConverterJsonDocument`. This implementation use `JsonDocument.GetValue()`, which take a `Utf8JsonReader` as method input. This allow me to use type like `JsonDocument` or `JsonElement`, and the code readability is vastly improved.
