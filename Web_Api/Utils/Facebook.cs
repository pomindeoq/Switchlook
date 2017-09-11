using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Utils
{
    public class Facebook
    {
        public static async Task<FacebookDataModel> GetUserLoginData(string accessToken)
        {
            FacebookDataModel facebookLoginModel = new FacebookDataModel();

            HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v2.10/") };
            var response = await httpClient.GetAsync($"me?access_token={accessToken}&fields=id,name,email,first_name,last_name");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                facebookLoginModel = JsonConvert.DeserializeObject<FacebookDataModel>(result);
            }
            

            return facebookLoginModel;
        }
    }

}
