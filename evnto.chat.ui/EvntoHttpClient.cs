using evnto.chat.ui.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;

namespace evnto.chat.ui
{
    public class EvntoHttpClient
    {
        #region Properties

        string _url;

        #endregion

        #region Properties

        public UserSessionModel Session { get; private set; }

        #endregion

        #region Ctor

        public EvntoHttpClient(string baseUrl)
        {
            _url = baseUrl;

            if(!_url.EndsWith("/"))
                _url = _url + "/";
        }

        #endregion

        #region Private methods

        private async Task<string> HandleResult(HttpResponseMessage response)
        {
            string body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(body);

            return body;
        }

        private async Task PutAsync<T>(string controller, T requestObject)
        { 
            HttpClient client = new HttpClient();

            if (Session!= null && !string.IsNullOrEmpty(Session.Token))
                client.DefaultRequestHeaders.Add("Token", Session.Token);

            string requestJson = JsonConvert.SerializeObject(requestObject);

            HttpResponseMessage response = await client.PutAsync($"{_url}{controller}", new StringContent(requestJson, Encoding.UTF8, "application/json"));
            await HandleResult(response);
        }

        private async Task<T> GetAsync<T>(string controller, Dictionary<string, string> parameters)
        {
            HttpClient client = new HttpClient();

            if (Session != null && !string.IsNullOrEmpty(Session.Token))
                client.DefaultRequestHeaders.Add("Token", Session.Token);

            UriBuilder uriBuilder = new UriBuilder($"{_url}{controller}");
            if (parameters != null && parameters.Count > 0)
            {
                uriBuilder.Query = "";
                foreach (KeyValuePair<string, string> kvp in parameters)
                    uriBuilder.Query += $"{kvp.Key}={kvp.Value}&";

                uriBuilder.Query = uriBuilder.Query.TrimEnd('&');
            }

            HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri);
            string json = await HandleResult(response);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task<T2> PostAsync<T1, T2>(string controller, T1 requestObject)
        {
            HttpClient client = new HttpClient();

            if (Session != null && !string.IsNullOrEmpty(Session.Token))
                client.DefaultRequestHeaders.Add("Token", Session.Token);

            string requestJson = JsonConvert.SerializeObject(requestObject);

            HttpResponseMessage response = await client.PostAsync($"{_url}{controller}", new StringContent(requestJson, Encoding.UTF8, "application/json"));
            string json = await HandleResult(response);

            return JsonConvert.DeserializeObject<T2>(json);
        }

        #endregion

        #region Public methods

        public async Task<bool> SignInAsync(string userName, string password)
        {
            SignModel sm = new SignModel();
            sm.UserName = userName;
            sm.Password = password;

            Session = await PostAsync<SignModel, UserSessionModel>("user", sm);
            return true;
        }

        public async Task SignOutAsync()
        {
            SignModel sm = new SignModel();
            sm.Userid = Session.UserId;

            await PostAsync<SignModel, UserSessionModel>("user", sm);
        }

        public async Task SignUpAsync(string userNmae, string fullName, string password)
        {
            SignUpModel sm = new SignUpModel();
            sm.FullName = fullName;
            sm.UserName = userNmae;
            sm.Password = password;

            await PutAsync("user", sm);
        }

        public async Task<List<UserModel>> GetOnlineUsersAsync()
        {
            return await GetAsync<List<UserModel>>("user", null);
        }

        public async Task<List<ChatModel>> GetActiveChatsAsync()
        {
            return await GetAsync<List<ChatModel>>("chat", null);
        }

        public async Task StartChatAsync(int recipientUserId)
        {
            ChatModel chat = new ChatModel();
            chat.RecipientUserId = recipientUserId;
            chat.InitiatorUserId = Session.UserId;

            await PutAsync("chat", chat);
        }

        public async Task SetChatStateAsync(int chatId, ChatState state)
        {
            ChatModel chat = new ChatModel();
            chat.ChatId = chatId;
            chat.State = state;

            await PostAsync<ChatModel, string>("chat", chat);
        }

        public async Task<List<MessageModel>> GetMessagesAsync(int chatId)
        {
            return await GetAsync<List<MessageModel>>("message", new Dictionary<string, string>() { { "chatId", chatId.ToString() } });
        }

        public async Task SendMessageAsync(int chatId, string text)
        {
            MessageModel mm = new MessageModel();
            mm.AuthorUserId = Session.UserId;
            mm.ChatId = chatId;
            mm.Text = text;

            await PutAsync("message", mm);
        }

        #endregion
    }
}
