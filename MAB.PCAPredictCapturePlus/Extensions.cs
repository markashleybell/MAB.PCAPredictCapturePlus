using System.Collections.Generic;
using System.Linq;

namespace MAB.PCAPredictCapturePlus
{
    internal static class Extensions
    {
        internal static bool IsError(this IEnumerable<CapturePlusFindItemRaw> results)
        {
            return results.Count() == 1 && results.First().Error.HasValue;
        }

        internal static bool IsError(this IEnumerable<CapturePlusRetrieveItemRaw> results)
        {
            return results.Count() == 1 && results.First().Error.HasValue;
        }

        internal static CapturePlusError MapError(this IEnumerable<CapturePlusFindItemRaw> results)
        {
            return results.Select(r => new CapturePlusError {
                Error = r.Error.Value,
                Description = r.Description,
                Cause = r.Cause,
                Resolution = r.Resolution
            }).First();
        }

        internal static CapturePlusError MapError(this IEnumerable<CapturePlusRetrieveItemRaw> results)
        {
            return results.Select(r => new CapturePlusError {
                Error = r.Error.Value,
                Description = r.Description,
                Cause = r.Cause,
                Resolution = r.Resolution
            }).First();
        }

        internal static IEnumerable<CapturePlusFindItem> MapResults(this IEnumerable<CapturePlusFindItemRaw> results)
        {
            return results.Select(r => {
                var type = CapturePlusFindItemType.Unknown;
                switch(r.Type)
                {
                    case "Address":
                        type = CapturePlusFindItemType.Address;
                        break;
                    case "Postcode":
                        type = CapturePlusFindItemType.Postcode;
                        break;
                    case "Locality":
                        type = CapturePlusFindItemType.Locality;
                        break;
                    default:
                        type = CapturePlusFindItemType.Unknown;
                        break;
                }
                return new CapturePlusFindItem {
                    Id = r.Id,
                    Type = type,
                    Text = r.Text,
                    Highlight = r.Highlight,
                    Description = r.Description
                };
            });
        }

        internal static IEnumerable<CapturePlusRetrieveItem> MapResults(this IEnumerable<CapturePlusRetrieveItemRaw> results)
        {
            return results.Select(r => new CapturePlusRetrieveItem {
                Id = r.Id,
                DomesticId = r.DomesticId,
                Language = r.Language,
                LanguageAlternatives = r.LanguageAlternatives,
                Department = r.Department,
                Company = r.Company,
                SubBuilding = r.SubBuilding,
                BuildingNumber = r.BuildingNumber,
                BuildingName = r.BuildingName,
                SecondaryStreet = r.SecondaryStreet,
                Street = r.Street,
                Block = r.Block,
                Neighbourhood = r.Neighbourhood,
                District = r.District,
                City = r.City,
                Line1 = r.Line1,
                Line2 = r.Line2,
                Line3 = r.Line3,
                Line4 = r.Line4,
                Line5 = r.Line5,
                AdminAreaName = r.AdminAreaName,
                AdminAreaCode = r.AdminAreaCode,
                Province = r.Province,
                ProvinceName = r.ProvinceName,
                ProvinceCode = r.ProvinceCode,
                PostalCode = r.PostalCode,
                CountryName = r.CountryName,
                CountryIso2 = r.CountryIso2,
                CountryIso3 = r.CountryIso3,
                CountryIsoNumber = r.CountryIsoNumber,
                SortingNumber1 = r.SortingNumber1,
                SortingNumber2 = r.SortingNumber2,
                Barcode = r.Barcode,
                POBoxNumber = r.POBoxNumber,
                Label = r.Label,
                Type = r.Type,
                DataLevel = r.DataLevel
            });
        }
    }
}
