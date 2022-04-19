using evnto.chat.ui.Models;
using System.Collections.ObjectModel;

namespace evnto.chat.ui.Forms
{
    public partial class FormChat : Form
    {
        #region Fields

        EvntoHttpClient _httpClient;
        EvntoWSClient _wSClient;

        #endregion

        #region Properties

        public ObservableCollection<UserModel> Users { get; set; }

        public ObservableCollection<ChatModel> Chats { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; }

        #endregion

        #region Ctor

        public FormChat(EvntoHttpClient httpClient, EvntoWSClient wSClient)
        {
            _httpClient = httpClient;
            _wSClient = wSClient;
            this.FormClosed += FormChat_FormClosed;
            InitializeComponent();
            dataGridViewMessages.AutoGenerateColumns = false;
        }

        #endregion

        #region Methods

        private async Task LoadUsers()
        {
            Users = await _httpClient.GetOnlineUsersAsync();
            listBoxUsers.DataSource = Users;
        }

        private async Task LoadChats()
        {
            Chats = await _httpClient.GetActiveChatsAsync();

            foreach (ChatModel chat in Chats)
            {
                chat.ChatInfo = chat.State.ToString() + " chat by " + (chat.InitiatorUserId == _httpClient.Session.UserId ? "Me" : chat.InitiatorUser.FullName);
                chat.ChatInfo += " with " + (chat.InitiatorUserId == _httpClient.Session.UserId ? chat.RecipientUser.FullName : chat.InitiatorUser.FullName);
                chat.ChatInfo += " started at " + chat.Created;
            }

            listBoxChats.DataSource = Chats;
        }

        private async Task LoadMessages(int chatId)
        {
            Messages = await _httpClient.GetMessagesAsync(chatId);
            dataGridViewMessages.DataSource = Messages;
        }

        #endregion

        #region Event handlers

        #region Form events

        private async void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            await _wSClient.DisconnectAsync();
            Application.Exit();
        }

        private async void FormChat_Load(object sender, EventArgs e)
        {
            await _wSClient.ConnectAsync();

            await LoadUsers();
            await LoadChats();            
        }

        #endregion

        private async void toolStripButtonRefreshChats_Click(object sender, EventArgs e)
        {
            await LoadChats();
        }

        private async void toolStripButtonRefreshUsers_Click(object sender, EventArgs e)
        {
            await LoadUsers();
        }

        private async void toolStripButtonStartChat_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedValue == null) return;

            int ruid = (listBoxUsers.SelectedValue as UserModel).UserId;

            await _httpClient.StartChatAsync(ruid);
        }

        private void listBoxUsers_SelectedValueChanged(object sender, EventArgs e)
        {
            toolStripButtonStartChat.Enabled = listBoxUsers.DataSource != null && listBoxUsers.SelectedValue != null;
        }

        private async void toolStripButtonAcceptChat_Click(object sender, EventArgs e)
        {
            if (listBoxChats.DataSource == null && listBoxChats.SelectedValue == null) return;

            ChatModel chat = listBoxChats.SelectedValue as ChatModel;
            await _httpClient.SetChatStateAsync(chat.ChatId, ChatState.Accepted);
        }

        private async void toolStripButtonRejectChat_Click(object sender, EventArgs e)
        {
            if (listBoxChats.DataSource == null && listBoxChats.SelectedValue == null) return;

            ChatModel chat = listBoxChats.SelectedValue as ChatModel;
            await _httpClient.SetChatStateAsync(chat.ChatId, ChatState.Rejected);
        }

        private async void listBoxChats_SelectedValueChanged(object sender, EventArgs e)
        {
            bool flag = listBoxChats.DataSource != null && listBoxChats.SelectedValue != null;

            if (flag)
            {
                ChatModel chat = (listBoxChats.SelectedValue as ChatModel);

                toolStripButtonCloseChat.Enabled = flag && chat.State == ChatState.Accepted;

                toolStripButtonAcceptChat.Enabled = toolStripButtonRejectChat.Enabled = 
                    flag && chat.State == ChatState.Initiated && chat.RecipientUserId == _httpClient.Session.UserId;

                buttonSend.Enabled = flag && chat.State == ChatState.Accepted;

                if (chat.State == ChatState.Accepted)
                    await LoadMessages(chat.ChatId);
                else
                {
                    Messages = null;
                    dataGridViewMessages.DataSource = null;
                }
            }
        }

        private async void toolStripButtonCloseChat_Click(object sender, EventArgs e)
        {
            if (listBoxChats.DataSource == null && listBoxChats.SelectedValue == null) return;

            ChatModel chat = listBoxChats.SelectedValue as ChatModel;
            await _httpClient.SetChatStateAsync(chat.ChatId, ChatState.Closed);
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMessage.Text)) return;
            
            if (listBoxChats.DataSource == null && listBoxChats.SelectedValue == null) return;

            ChatModel chat = listBoxChats.SelectedValue as ChatModel;

            if (chat.State != ChatState.Accepted) return;

            await _httpClient.SendMessageAsync(chat.ChatId, textBoxMessage.Text);
            textBoxMessage.Clear();
        }

        #endregion
    }
}
