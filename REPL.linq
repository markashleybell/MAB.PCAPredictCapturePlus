<Query Kind="Program">
  <Reference Relative="MAB.PCAPredictCapturePlus\bin\Debug\MAB.PCAPredictCapturePlus.dll">E:\Src\MAB.PCAPredictCapturePlus\MAB.PCAPredictCapturePlus\bin\Debug\MAB.PCAPredictCapturePlus.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Namespace>MAB.PCAPredictCapturePlus</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>System.Net</Namespace>
  <AppConfig>
    <Path Relative="REPL.config">E:\Src\MAB.PCAPredictCapturePlus\REPL.config</Path>
  </AppConfig>
</Query>

void Main()
{
    var key = ConfigurationManager.AppSettings["PCAPredictKey"];

    var client = new PCAPredictCapturePlusClient("2.10", key, "GBR", "EN");
    
    var searchResults = client.Find("WR5 3DA");
    
    searchResults.Dump();
    
    var address = client.Retrieve("GBR|52509479");
    
    address.Dump();

    address.Select(a => new {
        Company = a.Company,
        BuildingName = a.BuildingName,
        Street = a.Street,
        Line1 = a.Line1,
        Line2 = a.Line2,
        Line3 = a.Line3,
        Line4 = a.Line4,
        Line5 = a.Line5,
        City = a.City,
        County = a.Province,
        Postcode = a.PostalCode
    }).First().Dump();
}