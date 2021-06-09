using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Seeder {
    public class HttpProxy {
        private static string AccessToken;
        public HttpProxy() {

        }
        public HttpProxy(string accessToken) {
            AccessToken = accessToken;
        }
        #region Http Methods
        public async Task<string> PostDataAsync(string url, string jsonData) {
            string strReturn = null;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            if (!string.IsNullOrWhiteSpace(AccessToken)) {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            if (!string.IsNullOrWhiteSpace(jsonData)) {
                requestMessage.Content = new StringContent(jsonData);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            using (var httpClient = new HttpClient()) {
                HttpResponseMessage result = await httpClient.SendAsync(requestMessage);
                strReturn = await result.Content.ReadAsStringAsync();
            }
            return strReturn;
        }
        public async Task<string> PutDataAsync(string url, string jsonData) {
            string strReturn = null;
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            if (!string.IsNullOrWhiteSpace(AccessToken)) {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            if (!string.IsNullOrWhiteSpace(jsonData)) {
                requestMessage.Content = new StringContent(jsonData);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            using (var httpClient = new HttpClient()) {
                var result = await httpClient.SendAsync(requestMessage);
                strReturn = await result.Content.ReadAsStringAsync();
            }
            return strReturn;
        }
        public async Task<string> GetDataAsync(string url, string jsonData) {
            string strReturn = null;
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrWhiteSpace(AccessToken)) {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            if (!string.IsNullOrWhiteSpace(jsonData)) {
                requestMessage.Content = new StringContent(jsonData);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            using (var httpClient = new HttpClient()) {
                var result = await httpClient.SendAsync(requestMessage);
                strReturn = await result.Content.ReadAsStringAsync();
            }
            return strReturn;
        }
        #endregion
    }
}
