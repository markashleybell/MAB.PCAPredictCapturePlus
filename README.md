# MAB.PCAPredictCapturePlus
A .NET client for the PCA Predict Capture Plus address search API.

This is a basic wrapper around the [Capture Plus web services][1], which maps the JSON responses to .NET classes. 

It *doesn't* include a client-side JavaScript implementation of the Capture Plus UI, as I figured the main reason people would use this library would be to implement a custom UI; however, there is a [basic implementation][2]  included in the test harness project which you are welcome to use as a starting point.

Please feel free to submit issues or pull requests as neccessary!

[1]: http://www.pcapredict.com/Support/WebService/ServicesList/CapturePlus
[2]: https://github.com/markashleybell/MAB.PCAPredictCapturePlus/blob/master/MAB.PCAPredictCapturePlus.TestHarness/Scripts/main.js
