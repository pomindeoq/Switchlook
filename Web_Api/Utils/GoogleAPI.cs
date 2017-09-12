using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Utils
{
    public class GoogleAPI
    {
        private static HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://www.googleapis.com/oauth2/v3/") };
        public static async Task<GoogleDataModel> GetUserLoginData(string accessToken)
        {
            GoogleDataModel facebookLoginModel = new GoogleDataModel();
            var response = await _httpClient.GetAsync($"tokeninfo?id_token={accessToken}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                facebookLoginModel = JsonConvert.DeserializeObject<GoogleDataModel>(result);
            }
            return facebookLoginModel;
        }
    }
}
