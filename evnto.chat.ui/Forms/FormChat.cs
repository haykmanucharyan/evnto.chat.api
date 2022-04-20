using evnto.chat.ui.Models;
using System.Collections.ObjectModel;

namespace evnto.chat.ui.Forms
{
    public partial class FormChat : Form
    {
        #region Fields

        EvntoHttpClient _httpClient;
        EvntoWSClient _wSClient;
        CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        BindingSource _bindingSourceUsers = new BindingSource() { DataSource = new List<UserModel>() };
        BindingSource _bindingSourceChats = new BindingSource() { DataSource = new List<ChatModel>() };
        BindingSource _bindingSourceMessages = new BindingSource() { DataSource = new List<MessageModel>() };

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
            _bindingSourceUsers.DataSource = await _httpClient.GetOnlineUsersAsync();
            listBoxUsers.DataSource = _bindingSourceUsers;            
        }

        private async Task LoadChats()
        {
            List<ChatModel> chats = await _httpClient.GetActiveChatsAsync();

            foreach (ChatModel chat in chats)
                SetChatInfo(chat);

            _bindingSourceChats.DataSource = chats;
            listBoxChats.DataSource = _bindingSourceChats;
        }

        private void SetChatInfo(ChatModel chat)
        {
            chat.ChatInfo = chat.State.ToString() + " chat by " + (chat.InitiatorUserId == _httpClient.Session.UserId ? "Me" : chat.InitiatorUser.FullName);
            chat.ChatInfo += " with " + (chat.InitiatorUserId == _httpClient.Session.UserId ? chat.RecipientUser.FullName : chat.InitiatorUser.FullName);
            chat.ChatInfo += " started at " + chat.Created;
        }

        private async Task LoadMessages(int chatId)
        {
            _bindingSourceMessages.DataSource = await _httpClient.GetMessagesAsync(chatId);
            dataGridViewMessages.DataSource = _bindingSourceMessages;
        }

        #endregion

        #region Event handlers

        #region Form events

        private async void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            await _httpClient.SignOutAsync();
            _wSClient.MessageArrived -= _wSClient_MessageArrived;
            CancellationTokenSource.Cancel();
            await _wSClient.DisconnectAsync();
            Application.Exit();
        }

        private async void FormChat_Load(object sender, EventArgs e)
        {
            await LoadUsers();
            await LoadChats();

            _wSClient.MessageArrived += _wSClient_MessageArrived;
            await _wSClient.RecieveAsync(CancellationTokenSource.Token).ConfigureAwait(false);
        }

        private void _wSClient_MessageArrived(RmqMessage message)
        {
            switch (message.MessageType)
            {
                case RmqMessageType.SignIn:
                    int userId = int.Parse(message.PayLoad[nameof(UserModel.UserId)]);

                    if (userId != _httpClient.Session.UserId)
                    {
                        UserModel user = new UserModel();
                        user.UserId = userId;
                        user.UserName = message.PayLoad[nameof(UserModel.UserName)];
                        user.FullName = message.PayLoad[nameof(UserModel.FullName)];

                        List<UserModel> ulist1 = _bindingSourceUsers.DataSource as List<UserModel>;

                        if(!ulist1.Any(u => u.UserId == userId))
                            ulist1.Add(user);

                        _bindingSourceUsers.ResetBindings(false);
                    }

                    break;

                case RmqMessageType.SignOut:
                    userId = int.Parse(message.PayLoad[nameof(UserModel.UserId)]);

                    List<UserModel> ulist2 = _bindingSourceUsers.DataSource as List<UserModel>;

                    UserModel um = ulist2.Where(u => u.UserId == userId).FirstOrDefault();

                    if (um != null)
                    {
                        ulist2.Remove(um);
                        _bindingSourceUsers.ResetBindings(false);
                    }

                    break;

                case RmqMessageType.ChatCreated:
                    int chatId = int.Parse(message.PayLoad[nameof(ChatModel.ChatId)]);

                    List<ChatModel> chats1 = _bindingSourceChats.DataSource as List<ChatModel>;
                    if (!chats1.Any(c => c.ChatId == chatId))
                    {
                        ChatModel chat = new ChatModel();
                        chat.ChatId = chatId;
                        chat.Created = DateTimeOffset.MinValue.AddTicks(long.Parse(message.PayLoad[nameof(ChatModel.Created)]));
                        chat.State = ChatState.Initiated;
                        chat.InitiatorUserId = int.Parse(message.PayLoad[nameof(ChatModel.InitiatorUserId)]);
                        chat.RecipientUserId = int.Parse(message.PayLoad[nameof(ChatModel.RecipientUserId)]);
                        chat.RecipientUser = new UserModel()
                        {
                            UserId = chat.RecipientUserId,
                            UserName = message.PayLoad[$"{nameof(ChatModel.RecipientUser)}.{nameof(UserModel.UserName)}"],
                            FullName = message.PayLoad[$"{nameof(ChatModel.RecipientUser)}.{nameof(UserModel.FullName)}"],
                        };

                        chat.InitiatorUser = new UserModel()
                        {
                            UserId = chat.InitiatorUserId,
                            UserName = message.PayLoad[$"{nameof(ChatModel.InitiatorUser)}.{nameof(UserModel.UserName)}"],
                            FullName = message.PayLoad[$"{nameof(ChatModel.InitiatorUser)}.{nameof(UserModel.FullName)}"],
                        };

                        SetChatInfo(chat);
                        chats1.Add(chat);
                        _bindingSourceChats.ResetBindings(false);
                    }

                    break;

                case RmqMessageType.ChatStateChanged:
                    chatId = int.Parse(message.PayLoad[nameof(ChatModel.ChatId)]);
                    List<ChatModel> chats2 = _bindingSourceChats.DataSource as List<ChatModel>;
                    ChatModel ch = chats2.FirstOrDefault(c => c.ChatId == chatId);

                    if (ch != null)
                    {
                        ch.State = (ChatState)byte.Parse(message.PayLoad[nameof(ChatModel.State)]);
                        SetChatInfo(ch);
                        _bindingSourceChats.ResetBindings(false);
                    }
                    break;

                case RmqMessageType.Message:
                    if (listBoxChats.DataSource == null || listBoxChats.SelectedValue == null)
                        return;

                    chatId = int.Parse(message.PayLoad[nameof(ChatModel.ChatId)]);
                    if ((listBoxChats.SelectedValue as ChatModel).ChatId != chatId)
                        return;

                    MessageModel mm = new MessageModel();
                    mm.ChatId = chatId;
                    mm.MessageId = long.Parse(message.PayLoad[nameof(MessageModel.MessageId)]);
                    mm.Created = DateTimeOffset.MinValue.AddTicks(long.Parse(message.PayLoad[nameof(MessageModel.Created)]));
                    mm.AuthorUserId = int.Parse(message.PayLoad[nameof(MessageModel.AuthorUserId)]);
                    mm.Text = message.PayLoad[nameof(MessageModel.Text)];

                    mm.AuthorUser = new UserModel()
                    {
                        UserId = mm.AuthorUserId,
                        UserName = message.PayLoad[$"{nameof(MessageModel.AuthorUser)}.{nameof(UserModel.UserName)}"],
                        FullName = message.PayLoad[$"{nameof(MessageModel.AuthorUser)}.{nameof(UserModel.FullName)}"],
                    };

                    List<MessageModel> mms = _bindingSourceMessages.DataSource as List<MessageModel>;

                    if (!mms.Any(m => m.MessageId == mm.MessageId))
                    {
                        mms.Add(mm);
                        _bindingSourceMessages.ResetBindings(false);
                    }

                    break;
            }
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
                    _bindingSourceMessages.DataSource = new List<MessageModel>();
                    _bindingSourceMessages.ResetBindings(false);
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
