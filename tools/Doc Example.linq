<Query Kind="Program">
  <Reference Relative="..\MAB.PCAPredictCapturePlus\bin\Debug\MAB.PCAPredictCapturePlus.dll">C:\Src\MAB.PCAPredictCapturePlus\MAB.PCAPredictCapturePlus\bin\Debug\MAB.PCAPredictCapturePlus.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Namespace>MAB.PCAPredictCapturePlus</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>System.Net</Namespace>
  <AppConfig>
    <Path Relative="REPL.config">C:\Src\MAB.PCAPredictCapturePlus\tools\REPL.config</Path>
  </AppConfig>
</Query>

void Main()
{
    // Store your key somewhere safe!
    var key = ConfigurationManager.AppSettings["PCAPredictCapturePlusKey"];
    
    // Create a client, passing in your desired options
    var client = new CapturePlusClient(
        apiVersion: "1.00", 
        key: key, 
        defaultCountries: "GB", 
        defaultLanguage: "EN"
    );
    
    // Search for a postcode
    var postcodeFindResults = client.Find("WR5 3DA");
    
    // Iterate over the result list (there may be multiple results
    // if the search query was only a partial postcode e.g. "WR5")
    foreach (var result in postcodeFindResults.Items)
        Console.WriteLine($"{result.Id}: {result.Text}");
	
    // Use a postcode 'container' ID from a postcode search result 
    // to retrieve a list of addresses within that postcode
    var addressFindResults = client.Find(text: null, container: "GB|RM|ENG|3DA-WR5");
    
	// Iterate over the address result list
    foreach (var result in addressFindResults.Items)
        Console.WriteLine($"{result.Id}: {result.Text}");
    
    // Use an address ID from an address search result to
    // retrieve the full address details for that address
    var retrieveResults = client.Retrieve("GB|RM|A|52509479");

    // There should only ever be one result for a Retrieve query, but 
    // the Capture Plus API returns an array, so that's what we expose...
    var a = retrieveResults.Items.Single();
    
    Console.WriteLine($"{a.Id}: {a.Company}, {a.Line1}, {a.Line2}, {a.City}, {a.PostalCode}");
    
    
    var badClient = new CapturePlusClient(
        apiVersion: "1.00", 
        key: "BAD_API_KEY", 
        defaultCountries: "GB", 
        defaultLanguage: "EN"
    );
    
    var badPostcodeFindResults = badClient.Find("WR5 3DA");
    
    // Errors populate the Error property of the result type  
    // with error code, description, cause and resolution
    var e = badPostcodeFindResults.Error;
    
    Console.WriteLine($"Error {e.Error} ({e.Description}): {e.Cause} {e.Resolution}");
}