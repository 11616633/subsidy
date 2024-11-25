using Nancy.Json;
using SubsidyReconciliation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Services
{
    public class APIConsumtionService
    {
        string bassAddress = AppConfigurationHelper.BaseAddress.ToString();
        HttpClient _httpClient = new HttpClient();
        //string bassAddress = @"http://192.168.1.52:8090/";
        public Models.ResponseResult VerifyLogin(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(bassAddress);
                //var user = new UserDetail();

                var user = new UserViewModel
                {
                    username = userName,
                    password = password,
                    applicationcode = "POSSettlement System"
                };


                var responseTask = client.PostAsync("api/central/login",
                    new StringContent(new JavaScriptSerializer().Serialize(user),
                    Encoding.UTF8, "application/json")).Result;


                var responseResult = responseTask.Content.ReadAsStringAsync().Result;
                var newobj = new JavaScriptSerializer().Deserialize<Models.ResponseResult>(responseResult);

                if (responseTask.IsSuccessStatusCode)
                {
                    return newobj;
                }
                else
                {
                    return null;
                }
            }

        }
    }
}
