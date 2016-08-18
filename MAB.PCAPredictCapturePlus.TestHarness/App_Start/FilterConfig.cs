using System.Web.Mvc;

namespace MAB.PCAPredictCapturePlus.TestHarness
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
