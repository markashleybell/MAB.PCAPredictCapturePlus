using RestSharp;

namespace MAB.PCAPredictCapturePlus
{
    /// <summary>
    /// Query the PCA Predict Capture Plus address search API.
    /// </summary>
    public class CapturePlusClient
    {
        private const string _BASE_URL = "https://services.postcodeanywhere.co.uk/CapturePlus/Interactive";
        private const string _FIND_URL = "Find/v{0}/json3.ws";
        private const string _RETRIEVE_URL = "Retrieve/v{0}/json3.ws";

        private string _apiVersion;
        private string _key;
        private RestClient _client;
        private string _defaultFindCountry;
        private string _defaultLanguage;
    
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="apiVersion">The Capture Plus API version (e.g. "2.10")</param>
        /// <param name="key">A Capture Plus Service Key (obtained from https://account.pcapredict.com).</param>
        /// <param name="defaultFindCountry">The ISO code for the default country to search (e.g. "GB").</param>
        /// <param name="defaultLanguage">The language identifier code for the default search result language (e.g. "EN").</param>
        public CapturePlusClient(string apiVersion, string key, string defaultFindCountry, string defaultLanguage)
        {
            _apiVersion = apiVersion;
            _key = key;
            _client = new RestClient(_BASE_URL);
            _defaultFindCountry = defaultFindCountry;
            _defaultLanguage = defaultLanguage;
        }

        #region Find overloads

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="searchTerm">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string searchTerm)
        {
            return Find(searchTerm, SearchFor.Everything);
        }

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="searchTerm">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="searchFor">A <see cref="SearchFor"/> value specifying which properties of address records to search against.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string searchTerm, SearchFor searchFor)
        {
            return Find(searchTerm, searchFor, _defaultFindCountry, _defaultLanguage);
        }

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="searchTerm">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="searchFor">A <see cref="SearchFor"/> value specifying which properties of address records to search against.</param>
        /// <param name="country">The ISO code for the country to search (e.g. "GB").</param>
        /// <param name="languagePreference">The language identifier code for the search result language (e.g. "EN").</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string searchTerm, SearchFor searchFor, string country, string languagePreference)
        {
            return Find(searchTerm, searchFor, country, languagePreference, null, null);
        }

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="searchTerm">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="searchFor">A <see cref="SearchFor"/> value specifying which properties of address records to search against.</param>
        /// <param name="country">The ISO code for the country to search (e.g. "GB").</param>
        /// <param name="languagePreference">The language identifier code for the search result language (e.g. "EN").</param>
        /// <param name="maxSuggestions">The maximum number of autocomplete suggestions to return.</param>
        /// <param name="maxResults">The maximum number of retrievable address results to return.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string searchTerm, SearchFor searchFor, string country, string languagePreference, int? maxSuggestions, int? maxResults)
        {
            return Find(searchTerm, null, searchFor, country, languagePreference, maxSuggestions, maxResults);
        }

        #endregion Find overloads

        /// <summary>
        /// Return a list of address records matching the supplied search term.
        /// </summary>
        /// <param name="searchTerm">A search term representing part of an address (e.g. "WR5", "Postcode Anywhere", "Basin Road").</param>
        /// <param name="lastId">An ID from a previous Find operation.</param>
        /// <param name="searchFor">A <see cref="SearchFor"/> value specifying which properties of address records to search against.</param>
        /// <param name="country">The ISO code for the country to search (e.g. "GB").</param>
        /// <param name="languagePreference">The language identifier code for the search result language (e.g. "EN").</param>
        /// <param name="maxSuggestions">The maximum number of autocomplete suggestions to return.</param>
        /// <param name="maxResults">The maximum number of retrievable address results to return.</param>
        /// <returns>A <see cref="CapturePlusFindResult"/> containing a list of results or error information.</returns>
        public CapturePlusFindResult Find(string searchTerm, string lastId, SearchFor searchFor, string country, string languagePreference, int? maxSuggestions, int? maxResults)
        {
            var url = string.Format(_FIND_URL, _apiVersion);

            var req = new RestRequest(url, Method.GET);
            req.AddParameter("Key", _key);
            req.AddParameter("SearchTerm", searchTerm);
            // TODO: I don't yet understand how the LastId parameter is used... 
            // The docs say 'If the LastId is provided, the SearchTerm searches within the results from the LastId', 
            // but the IDs are unique, so you would only ever be searching within a single result?
            req.AddParameter("LastId", lastId ?? ""); 
            req.AddParameter("SearchFor", searchFor.ToString());
            req.AddParameter("Country", country);
            req.AddParameter("LanguagePreference", languagePreference);
            req.AddParameter("MaxSuggestions", maxSuggestions.HasValue ? maxSuggestions.Value.ToString() : "");
            req.AddParameter("MaxResults", maxResults.HasValue ? maxResults.Value.ToString() : "");

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
