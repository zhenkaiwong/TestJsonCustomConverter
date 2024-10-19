using System.Text;
using TestJsonCustomConverter.Models.Payloads;

namespace TestJsonCustomConverter.Models;

public class Person
{

  public string? Name { get; set; }
  public string? Occupation { get; set; }
  public IPersonPayload? Payload { get; set; }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"Name: {Name}, Occupation: {Occupation}, ");
    switch (Payload)
    {
      case SoftwareEngineerPayload softwareEngineerPayload:
        stringBuilder.Append($"Age: {softwareEngineerPayload.Age} yo");
        break;
      case QuantitySurveyorPayload quantitySurveyorPayload:
        stringBuilder.Append($"Hobby: {quantitySurveyorPayload.Hobby}");
        break;
      default:
        stringBuilder.Append("This person don't have payload");
        break;
    }
    return stringBuilder.ToString();
  }
}
