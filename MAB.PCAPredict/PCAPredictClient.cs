using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAB.PCAPredict
{
    public class PCAPredictClient
    {
        private string _apiVersion;
        private string _key;
        private RestClient _client;
        private string _defaultFindCountry;
        private string _defaultLanguage;
    
        public PCAPredictClient(string apiVersion, string key, string defaultFindCountry, string defaultLanguage)
        {
            _apiVersion = apiVersion;
            _key = key;
            _client = new RestClient("https://services.postcodeanywhere.co.uk/CapturePlus/Interactive");
            _defaultFindCountry = defaultFindCountry;
            _defaultLanguage = defaultLanguage;
        }
    
        public List<PCAPredictFindResult> Find(string term)
        {
            return Find(term, SearchFor.Everything);
        }

        public List<PCAPredictFindResult> Find(string term, SearchFor searchFor)
        {
            return Find(term, searchFor, _defaultFindCountry, _defaultLanguage);
        }

        public List<PCAPredictFindResult> Find(string term, SearchFor searchFor, string country, string languagePreference)
        {
            return Find(term, null, searchFor, country, languagePreference, null, null);
        }

        public List<PCAPredictFindResult> Find(string term, SearchFor searchFor, string country, string languagePreference, int? maxSuggestions, int? maxResults)
        {
            return Find(term, null, searchFor, country, languagePreference, maxSuggestions, maxResults);
        }

        public List<PCAPredictFindResult> Find(string term, string lastId, SearchFor searchFor, string country, string languagePreference, int? maxSuggestions, int? maxResults)
        {
            var url = string.Format("Find/v{0}/json3.ws", _apiVersion);

            var req = new RestRequest(url, Method.GET);
            req.AddParameter("Key", _key);
            req.AddParameter("SearchTerm", term);
            req.AddParameter("LastId", lastId ?? ""); // I don't yet understand how this is used... why would you want to search within a single result?
            req.AddParameter("SearchFor", searchFor.ToString());
            req.AddParameter("Country", country);
            req.AddParameter("LanguagePreference", languagePreference);
            req.AddParameter("MaxSuggestions", maxSuggestions.HasValue ? maxSuggestions.Value.ToString() : "");
            req.AddParameter("MaxResults", maxResults.HasValue ? maxResults.Value.ToString() : "");

            var resp = _client.Execute<PCAPredictFindResultList>(req);
        
            return resp.Data.Items;
        }

        public List<PCAPredictRetrieveResult> Retrieve(string id)
        {
            var url = string.Format("Retrieve/v{0}/json3.ws", _apiVersion);

            var req = new RestRequest(url, Method.GET);
            req.AddParameter("Key", _key);
            req.AddParameter("Id", id);
        
            var resp = _client.Execute<PCAPredictRetrieveResultList>(req);

            return resp.Data.Items;
        }
    }
}
