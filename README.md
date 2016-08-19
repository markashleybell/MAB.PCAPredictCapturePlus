# MAB.PCAPredictCapturePlus
A .NET client for the PCA Predict Capture Plus address search API.

This is a basic wrapper around the [Capture Plus web services][1], which maps the JSON responses to .NET classes. 

It *doesn't* include a client-side JavaScript implementation of the Capture Plus UI, as I figured the main reason people would use this library would be to implement a custom UI; however, there is a [basic implementation][2]  included in the test harness project which you are welcome to use as a starting point.

Please feel free to submit issues or pull requests as neccessary!

## Example code

### Get address suggestions

    // Store your key somewhere safe!
    var key = ConfigurationManager.AppSettings["PCAPredictCapturePlusKey"];

    var client = new CapturePlusClient("2.10", key, "GB", "en");
    
    // Search for a postcode
    var findResults = client.Find("WR5 3DA");
    
    // If there was an error...
    if (findResults.Error != null)
    {
        // The error object contains full error info (error code, description, cause and resolution)
        var e = findResults.Error;
        var output = string.Format("{0} ({1}): {2} {3}", e.Description, e.Error, e.Cause, e.Resolution);
        Console.WriteLine(output);
    }
    else
    {
        // Iterate over the result list
        foreach (var r in findResults.Items)
        {
            var output = string.Format("{0}: {1}", r.Id, r.Text);
            Console.WriteLine(output);
        }
    }

Output:

    GBR|53327590: WR5 3DA, Lock Keepers Cottage, Basin Road, Worcester 
    GBR|26772356: WR5 3DA, Lock View, Basin Road, Worcester 
    GBR|52509479_1361760: WR5 3DA, P C A Predict, Postcode Anywhere (Europe) Ltd, Waterside, Basin Road, Worcester 
    GBR|52509479: WR5 3DA, Postcode Anywhere (Europe) Ltd, Waterside, Basin Road, Worcester 
    
### Retrieve address details
    
    // Use one of the returned IDs to retrieve full details for an address
    var retrieveResults = client.Retrieve("GBR|52509479");

    if (retrieveResults.Error != null)
    {
        var e = retrieveResults.Error;
        var output = string.Format("{0} ({1}): {2} {3}", e.Description, e.Error, e.Cause, e.Resolution);
        Console.WriteLine(output);
    }
    else
    {
        // There should only ever be one result for a Retrieve query, but 
        // the Capture Plus API returns an array, so that's what we expose...
        foreach (var r in retrieveResults.Items)
        {
            var output = string.Format("{0}: {1}, {2}, {3}, {4} {5}", r.Id, r.Company, r.Line1, r.Line2, r.City, r.PostalCode);
            Console.WriteLine(output);
        }
    }

Output:
    
    GB|RM|A|52509479: Postcode Anywhere (Europe) Ltd, Waterside, Basin Road, Worcester WR5 3DA

[1]: http://www.pcapredict.com/Support/WebService/ServicesList/CapturePlus
[2]: https://github.com/markashleybell/MAB.PCAPredictCapturePlus/blob/master/MAB.PCAPredictCapturePlus.TestHarness/Scripts/main.js
