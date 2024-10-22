using System;
using TestJsonCustomConverter.Models;

namespace TestJsonCustomConverter.Json.Payloads;

public class PayloadConverterFactory
{
  public static BasePayloadConverter GetConverterByPerson(Person person)
  {
    switch (person.Occupation)
    {
      case "Software Engineer":
        return new SoftwareEngineerPayloadConverter();
      case "Quantity Surveyor":
        return new QuantitySurveyorPayloadConverter();
      default:
        throw new InvalidOperationException("Unable to find converter");
    }
  }
}
