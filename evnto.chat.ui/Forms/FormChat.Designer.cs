namespace evnto.chat.ui.Forms
{
    partial class FormChat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChat));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRefreshUsers = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefreshChats = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStartChat = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAcceptChat = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRejectChat = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCloseChat = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxUsers = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxChats = new System.Windows.Forms.ListBox();
            this.dataGridViewMessages = new System.Windows.Forms.DataGridView();
            this.ColumnCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonSend = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessages)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefreshUsers,
            this.toolStripButtonRefreshChats,
            this.toolStripSeparator1,
            this.toolStripButtonStartChat,
            this.toolStripSeparator2,
            this.toolStripButtonAcceptChat,
            this.toolStripButtonRejectChat,
            this.toolStripButtonCloseChat});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonRefreshUsers
            // 
            this.toolStripButtonRefreshUsers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRefreshUsers.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefreshUsers.Image")));
            this.toolStripButtonRefreshUsers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefreshUsers.Name = "toolStripButtonRefreshUsers";
            this.toolStripButtonRefreshUsers.Size = new System.Drawing.Size(80, 22);
            this.toolStripButtonRefreshUsers.Text = "Refresh users";
            this.toolStripButtonRefreshUsers.Click += new System.EventHandler(this.toolStripButtonRefreshUsers_Click);
            // 
            // toolStripButtonRefreshChats
            // 
            this.toolStripButtonRefreshChats.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRefreshChats.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefreshChats.Image")));
            this.toolStripButtonRefreshChats.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefreshChats.Name = "toolStripButtonRefreshChats";
            this.toolStripButtonRefreshChats.Size = new System.Drawing.Size(81, 22);
            this.toolStripButtonRefreshChats.Text = "Refresh chats";
            this.toolStripButtonRefreshChats.Click += new System.EventHandler(this.toolStripButtonRefreshChats_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonStartChat
            // 
            this.toolStripButtonStartChat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStartChat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStartChat.Image")));
            this.toolStripButtonStartChat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStartChat.Name = "toolStripButtonStartChat";
            this.toolStripButtonStartChat.Size = new System.Drawing.Size(61, 22);
            this.toolStripButtonStartChat.Text = "Start chat";
            this.toolStripButtonStartChat.Click += new System.EventHandler(this.toolStripButtonStartChat_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAcceptChat
            // 
            this.toolStripButtonAcceptChat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAcceptChat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAcceptChat.Image")));
            this.toolStripButtonAcceptChat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAcceptChat.Name = "toolStripButtonAcceptChat";
            this.toolStripButtonAcceptChat.Size = new System.Drawing.Size(74, 22);
            this.toolStripButtonAcceptChat.Text = "Accept chat";
            this.toolStripButtonAcceptChat.Click += new System.EventHandler(this.toolStripButtonAcceptChat_Click);
            // 
            // toolStripButtonRejectChat
            // 
            this.toolStripButtonRejectChat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRejectChat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRejectChat.Image")));
            this.toolStripButtonRejectChat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRejectChat.Name = "toolStripButtonRejectChat";
            this.toolStripButtonRejectChat.Size = new System.Drawing.Size(69, 22);
            this.toolStripButtonRejectChat.Text = "Reject chat";
            this.toolStripButtonRejectChat.Click += new System.EventHandler(this.toolStripButtonRejectChat_Click);
            // 
            // toolStripButtonCloseChat
            // 
            this.toolStripButtonCloseChat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCloseChat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCloseChat.Image")));
            this.toolStripButtonCloseChat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCloseChat.Name = "toolStripButtonCloseChat";
            this.toolStripButtonCloseChat.Size = new System.Drawing.Size(66, 22);
            this.toolStripButtonCloseChat.Text = "Close chat";
            this.toolStripButtonCloseChat.Click += new System.EventHandler(this.toolStripButtonCloseChat_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxUsers);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 425);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Online users";
            // 
            // listBoxUsers
            // 
            this.listBoxUsers.DisplayMember = "UserInfo";
            this.listBoxUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUsers.FormattingEnabled = true;
            this.listBoxUsers.ItemHeight = 15;
            this.listBoxUsers.Location = new System.Drawing.Point(3, 19);
            this.listBoxUsers.Name = "listBoxUsers";
            this.listBoxUsers.Size = new System.Drawing.Size(253, 403);
            this.listBoxUsers.TabIndex = 0;
            this.listBoxUsers.SelectedValueChanged += new System.EventHandler(this.listBoxUsers_SelectedValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(259, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewMessages);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(541, 425);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxChats);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 425);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Active chats";
            // 
            // listBoxChats
            // 
            this.listBoxChats.DisplayMember = "ChatInfo";
            this.listBoxChats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxChats.FormattingEnabled = true;
            this.listBoxChats.ItemHeight = 15;
            this.listBoxChats.Location = new System.Drawing.Point(3, 19);
            this.listBoxChats.Name = "listBoxChats";
            this.listBoxChats.Size = new System.Drawing.Size(214, 403);
            this.listBoxChats.TabIndex = 1;
            this.listBoxChats.SelectedValueChanged += new System.EventHandler(this.listBoxChats_SelectedValueChanged);
            // 
            // dataGridViewMessages
            // 
            this.dataGridViewMessages.AllowUserToAddRows = false;
            this.dataGridViewMessages.AllowUserToDeleteRows = false;
            this.dataGridViewMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCreated,
            this.ColumnFrom,
            this.ColumnText});
            this.dataGridViewMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMessages.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMessages.Name = "dataGridViewMessages";
            this.dataGridViewMessages.ReadOnly = true;
            this.dataGridViewMessages.RowTemplate.Height = 25;
            this.dataGridViewMessages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewMessages.Size = new System.Drawing.Size(317, 287);
            this.dataGridViewMessages.TabIndex = 1;
            // 
            // ColumnCreated
            // 
            this.ColumnCreated.DataPropertyName = "Created";
            this.ColumnCreated.HeaderText = "Timestamp";
            this.ColumnCreated.Name = "ColumnCreated";
            this.ColumnCreated.ReadOnly = true;
            // 
            // ColumnFrom
            // 
            this.ColumnFrom.DataPropertyName = "AuthorUserInfo";
            this.ColumnFrom.HeaderText = "From";
            this.ColumnFrom.Name = "ColumnFrom";
            this.ColumnFrom.ReadOnly = true;
            // 
            // ColumnText
            // 
            this.ColumnText.DataPropertyName = "Text";
            this.ColumnText.HeaderText = "Message";
            this.ColumnText.Name = "ColumnText";
            this.ColumnText.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxMessage);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 138);
            this.panel1.TabIndex = 0;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.Location = new System.Drawing.Point(0, 0);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(229, 138);
            this.textBoxMessage.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonSend);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(229, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(88, 138);
            this.panel2.TabIndex = 0;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(7, 40);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 63);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send>";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Evnto Chat";
            this.Load += new System.EventHandler(this.FormChat_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessages)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonRefreshUsers;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButtonRefreshChats;
        private GroupBox groupBox1;
        private ListBox listBoxUsers;
        private SplitContainer splitContainer1;
        private GroupBox groupBox2;
        private ListBox listBoxChats;
        private ToolStripButton toolStripButtonStartChat;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButtonAcceptChat;
        private ToolStripButton toolStripButtonRejectChat;
        private ToolStripButton toolStripButtonCloseChat;
        private DataGridView dataGridViewMessages;
        private Panel panel1;
        private TextBox textBoxMessage;
        private Panel panel2;
        private Button buttonSend;
        private DataGridViewTextBoxColumn ColumnCreated;
        private DataGridViewTextBoxColumn ColumnFrom;
        private DataGridViewTextBoxColumn ColumnText;
    }
}