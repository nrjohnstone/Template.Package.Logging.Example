using Microsoft.Extensions.Logging;

namespace ExampleLib;

public class AwesomeLibraryFunction
{
    public void DoSomethingAndLogIt()
    {
        Log.Logger.LogInformation("Doing something !!");
    }

    public void DoSomethingWithTemplateValues(string value1, string value2)
    {
        Log.Logger.LogInformation("Doing something with value1 {Value1} and {Value2} !!", value1, value2);
    }
    
    public void DoSomethingWithScopeValues()
    {
        using (Log.Logger.BeginScope(new List<KeyValuePair<string, object>>
               {
                   new ("TransactionId", Guid.Parse("67B3BA3C-7387-4686-B9A4-A132791DFC52")),
                   new ("MeaningOfLife", 42)
               }))
        {
            Log.Logger.LogInformation("Doing something with scope values !!");
        }
 
        Log.Logger.LogInformation("Doing something without scope values !!");
    }
}