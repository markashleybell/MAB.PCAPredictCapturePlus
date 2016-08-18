using RestSharp;

namespace MAB.PCAPredictCapturePlus
{
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
    
        public CapturePlusClient(string apiVersion, string key, string defaultFindCountry, string defaultLanguage)
        {
            _apiVersion = apiVersion;
            _key = key;
            _client = new RestClient(_BASE_URL);
            _defaultFindCountry = defaultFindCountry;
            _defaultLanguage = defaultLanguage;
        }

        #region Find overloads

        public CapturePlusFindResult Find(string term)
        {
            return Find(term, SearchFor.Everything);
        }

        public CapturePlusFindResult Find(string term, SearchFor searchFor)
        {
            return Find(term, searchFor, _defaultFindCountry, _defaultLanguage);
        }

        public CapturePlusFindResult Find(string term, SearchFor searchFor, string country, string languagePreference)
        {
            return Find(term, searchFor, country, languagePreference, null, null);
        }

        public CapturePlusFindResult Find(string term, SearchFor searchFor, string country, string languagePreference, int? maxSuggestions, int? maxResults)
        {
            return Find(term, null, searchFor, country, languagePreference, maxSuggestions, maxResults);
        }

        #endregion Find overloads

        public CapturePlusFindResult Find(string term, string lastId, SearchFor searchFor, string country, string languagePreference, int? maxSuggestions, int? maxResults)
        {
            var url = string.Format(_FIND_URL, _apiVersion);

            var req = new RestRequest(url, Method.GET);
            req.AddParameter("Key", _key);
            req.AddParameter("SearchTerm", term);
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
