using evnto.chat.ui.Models;
using Newtonsoft.Json;
using System.Text;

namespace evnto.chat.ui
{
    internal class EvntoHttpClient
    {
        string _url;
        UserSessionModel _session = null;

        public EvntoHttpClient(string baseUrl)
        {
            _url = baseUrl;

            if(!_url.EndsWith("/"))
                _url = _url + "/";
        }

        private async Task<string> HandleResult(HttpResponseMessage response)
        {
            string body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(body);

            return body;
        }

        private async Task Put<T>(string controller, T requestObject)
        { 
            HttpClient client = new HttpClient();

            if (_session!= null && !string.IsNullOrEmpty(_session.Token))
                client.DefaultRequestHeaders.Add("Token", _session.Token);

            string requestJson = JsonConvert.SerializeObject(requestObject);

            HttpResponseMessage response = await client.PutAsync($"{_url}{controller}", new StringContent(requestJson, Encoding.UTF8, "application/json"));
            await HandleResult(response);
        }

        private async Task<T2> Post<T1, T2>(string controller, T1 requestObject)
        {
            HttpClient client = new HttpClient();

            if (_session != null && !string.IsNullOrEmpty(_session.Token))
                client.DefaultRequestHeaders.Add("Token", _session.Token);

            string requestJson = JsonConvert.SerializeObject(requestObject);

            HttpResponseMessage response = await client.PostAsync($"{_url}{controller}", new StringContent(requestJson, Encoding.UTF8, "application/json"));
            string json = await HandleResult(response);

            return JsonConvert.DeserializeObject<T2>(json);
        }

        public async Task<bool> SignIn(string userName, string password)
        {
            SignInModel sm = new SignInModel();
            sm.UserName = userName;
            sm.Password = password;

            _session = await Post<SignInModel, UserSessionModel>("user", sm);
            return true;
        }

        public async Task SignUp(string userNmae, string fullName, string password)
        {
            SignUpModel sm = new SignUpModel();
            sm.FullName = fullName;
            sm.UserName = userNmae;
            sm.Password = password;

            await Put("user", sm);
        }
    }
}
