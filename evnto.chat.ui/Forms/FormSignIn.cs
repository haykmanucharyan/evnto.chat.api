namespace evnto.chat.ui.Forms
{
    public partial class FormSignIn : Form
    {
        public FormSignIn()
        {
            InitializeComponent();
        }

        private async void buttonSignIn_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(textBoxUrl.Text))
            {
                errorProvider1.SetError(textBoxUrl, "Empty URl");
                return;
            }

            if (string.IsNullOrEmpty(textBoxUserName.Text))
            {
                errorProvider1.SetError(textBoxUserName, "Empty username");
                return;
            }

            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                errorProvider1.SetError(textBoxPassword, "Empty password");
                return;
            }

            EvntoHttpClient httpClient = new EvntoHttpClient(textBoxUrl.Text);
            if (await httpClient.SignInAsync(textBoxUserName.Text, textBoxPassword.Text))
            {
                FormChat frm = new FormChat(httpClient);
                frm.Show();
                textBoxPassword.Text = string.Empty;
                Hide();
            }
        }

        private async void buttonSigUp_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(textBoxUrl.Text))
            {
                errorProvider1.SetError(textBoxUrl, "Empty URl");
                return;
            }

            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                errorProvider1.SetError(textBoxFullName, "Empty full name");
                return;
            }

            if (string.IsNullOrEmpty(textBoxSignupUsername.Text))
            {
                errorProvider1.SetError(textBoxSignupUsername, "Empty username");
                return;
            }

            if (string.IsNullOrEmpty(textBoxSignupPassword.Text))
            {
                errorProvider1.SetError(textBoxSignupPassword, "Empty password");
                return;
            }

            if (string.IsNullOrEmpty(textBoxRepeatPassword.Text))
            {
                errorProvider1.SetError(textBoxRepeatPassword, "Empty password repetition");
                return;
            }

            if (textBoxSignupPassword.Text != textBoxRepeatPassword.Text)
            {
                errorProvider1.SetError(textBoxSignupPassword, "Passwords don't match.");
                return;
            }

            EvntoHttpClient httpClient = new EvntoHttpClient(textBoxUrl.Text);

            await httpClient.SignUpAsync(textBoxSignupUsername.Text, textBoxFullName.Text, textBoxSignupPassword.Text);
            MessageBox.Show("Done");
        }
    }
}