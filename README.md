# MAB.PCAPredictCapturePlus

A .NET client library for the [PCA Predict Capture Plus][1] address search API.

This is a basic wrapper around the [Capture Plus web services][2], which maps the JSON responses to .NET classes. 

It *doesn't* include a client-side JavaScript implementation of the Capture Plus UI, as I figured the main reason people might use this library would be to implement a custom UI; however, there is a [basic implementation][3] included in the test harness project which you are welcome to use as a starting point.

Please feel free to submit issues or pull requests as neccessary!

## Disclaimer

**This is not an official library.** I am not in any way affiliated with Postcode Anywhere Ltd or the PCA Predict service (other than being a user of the service).

## Installation

Installation of the [MAB.PCAPredictCapturePlus NuGet package][4] is recommended, as this will always be kept up to date with the latest stable version. The easiest way is just to search for `MAB.PCAPredictCapturePlus` from within the Visual Studio Package Explorer.

## Example code

There are now three stages to retrieving an address:

### Perform a postcode search

```cs
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
```

#### Output:

    GB|RM|ENG|3DA-WR5: WR5 3DA

### Perform an address search

```cs
// Use a postcode 'container' ID from a postcode search result  
// to retrieve a list of addresses within that postcode
var addressFindResults = client.Find(text: null, container: "GB|RM|ENG|3DA-WR5");

// Iterate over the address result list
foreach (var result in addressFindResults.Items)
    Console.WriteLine($"{result.Id}: {result.Text}");
```

#### Output:

    GB|RM|A|53327590: Lock Keepers Cottage, Basin Road
    GB|RM|A|26772356: Lock View, Basin Road
    GB|RM|A|52509479: P C A Predict, Waterside, Basin Road
    GB|RM|A|52509479|1361760: P C A Predict, Waterside, Basin Road
    
### Retrieve address details

```cs
// Use an address ID from an address search result to
// retrieve the full address details for that address
var retrieveResults = client.Retrieve("GB|RM|A|52509479");

// There should only ever be one result for a Retrieve query, but 
// the Capture Plus API returns an array, so that's what we expose...
var a = retrieveResults.Items.Single();

Console.WriteLine($"{a.Id}: {a.Company}, {a.Line1}, {a.Line2}, {a.City}, {a.PostalCode}");
```

#### Output:
    
    GB|RM|A|52509479: P C A Predict, Waterside, Basin Road, Worcester, WR5 3DA

### Error handling

```cs
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

Console.WriteLine($"Error {e.Error} ({e.Description}): {e.Cause}");
Console.WriteLine($"{e.Resolution}");
```

#### Output:

    Error 2 (Unknown key): The key you are using to access the service was not found.
    Please check that the key is correct. It should be in the form AA11-AA11-AA11-AA11.

[1]: https://www.pcapredict.com/en-gb/address-capture-software/
[2]: https://www.pcapredict.com/support/webservice/serviceslist/capture
[3]: https://github.com/markashleybell/MAB.PCAPredictCapturePlus/blob/master/TestHarness/Scripts/main.js
[4]: https://www.nuget.org/packages/MAB.PCAPredictCapturePlus
