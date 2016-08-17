using MAB.PCAPredict;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAB.PCAPredict.TestHarness.Controllers
{
    public class HomeController : Controller
    {
        private PCAPredictClient _client = new PCAPredictClient(
            apiVersion: "2.10",
            key: ConfigurationManager.AppSettings["PCAPredictKey"],
            defaultFindCountry: "GBR",
            defaultLanguage: "EN"
        );

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find(string term)
        {
            return Json(_client.Find(term));
        }

        public ActionResult Retrieve(string id)
        {
            var result = _client.Retrieve(id);

            var model = result.Select(a => new {
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
            }).First();

            return Json(model);
        }
    }
}