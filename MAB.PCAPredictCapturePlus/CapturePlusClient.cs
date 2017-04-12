using RestSharp;

namespace MAB.PCAPredictCapturePlus
{
    /// <summary>
    /// Query the PCA Predict Capture Plus address search API.
    /// </summary>
    public class CapturePlusClient
    {
        private const string _BASE_URL = "https://services.postcodeanywhere.co.uk/Capture/Interactive";
        private const string _FIND_URL = "Find/v{0}/json3.ws";
        private const string _RETRIEVE_URL = "Retrieve/v{0}/json3.ws";

        private string _apiVersion;
        private string _key;
        private RestClient _client;
        private string _defaultCountries;
        private string _defaultLanguage;
    
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="apiVersion">The Capture Plus API version (e.g. "2.10")</param>
        /// <param name="key">A Capture Plus Service Key (obtained from https://account.pcapredict.com).</param>
        /// <param name="defaultFindCountry">The ISO code for the default country to search (e.g. "GB").</param>
        /// <param name="defaultLanguage">The language identifier code for the default search result language (e.g. "EN").</param>
        public CapturePlusClient(string apiVersion, string key, string defaultCountries, string defaultLanguage)
        {
            _apiVersion = apiVersion;
            _key = key;
            _client = new RestClient(_BASE_URL);
            _defaultCountries = defaultCountries;
            _defaultLanguage = defaultLanguage;
        }

        #region Find overloads

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="text">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="limit">The maximum number of retrievable address results to return.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string text, int? limit = null)
        {
            return Find(text, null, _defaultCountries, _defaultLanguage, limit);
        }

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="text">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="countries">A comma-separated list of ISO codes for the countries to search (e.g. "GB").</param>
        /// <param name="language">The language identifier code for the preferred search result language (e.g. "EN").</param>
        /// <param name="limit">The maximum number of retrievable address results to return.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        private CapturePlusFindResult Find(string text, string countries, string language, int? limit = null)
        {
            return Find(text, null, countries, language, limit);
        }

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="text">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="container">An ID from a previous Find query representing a container (postcode, country, locality etc.) result (e.g. "GB|RM|ENG|3DA-WR5").</param>
        /// <param name="limit">The maximum number of retrievable address results to return.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string text, string container, int? limit = null)
        {
            return Find(text, container, _defaultCountries, _defaultLanguage, limit);
        }

        #endregion Find overloads

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="text">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="container">An ID from a previous Find query representing a container (postcode, country, locality etc.) result (e.g. "GB|RM|ENG|3DA-WR5").</param>
        /// <param name="countries">A comma-separated list of ISO codes for the countries to search (e.g. "GB").</param>
        /// <param name="language">The language identifier code for the preferred search result language (e.g. "EN").</param>
        /// <param name="limit">The maximum number of retrievable address results to return.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string text, string container, string countries, string language, int? limit = null)
        {
            var url = string.Format(_FIND_URL, _apiVersion);

            // TODO: Passing in text when container is specified has no effect... separate method or overloads?

            var req = new RestRequest(url, Method.GET);
            req.AddParameter("Key", _key);
            req.AddParameter("Text", text);
            req.AddParameter("Container", !string.IsNullOrWhiteSpace(container) ? container : "");
            req.AddParameter("Origin", "");
            req.AddParameter("Countries", !string.IsNullOrWhiteSpace(countries) ? countries : "");
            req.AddParameter("Language", !string.IsNullOrWhiteSpace(language) ? language : "");
            req.AddParameter("Limit", limit.HasValue ? limit.Value.ToString() : "");

            var resp = _client.Execute<CapturePlusFindResultList>(req);        
            var result = resp.Data.Items;

            if(result.IsError())
                return new CapturePlusFindResult(result.MapError());
            else
                return new CapturePlusFindResult(result.MapResults());
        }

        /// <summary>
        /// Return full address information for the address with the supplied identifier.
        /// </summary>
        /// <param name="id">A unique address identifier (e.g. "GBR|52509479").</param>
        /// <returns>A <see cref="CapturePlusRetrieveResult"/> containing a list of results or error information.</returns>
        public CapturePlusRetrieveResult Retrieve(string id)
        {
            var url = string.Format(_RETRIEVE_URL, _apiVersion);

            var req = new RestRequest(url, Method.GET);
            req.AddParameter("Key", _key);
            req.AddParameter("Id", id);
        
            var resp = _client.Execute<CapturePlusRetrieveResultList>(req);
            var result = resp.Data.Items;

            if(result.IsError())
                return new CapturePlusRetrieveResult(result.MapError());
            else
                return new CapturePlusRetrieveResult(result.MapResults());
        }
    }
}
