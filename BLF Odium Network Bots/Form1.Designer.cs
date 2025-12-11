namespace BLF_Odium_Network_Bots
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            listBoxBots = new ListBox();
            labelBotList = new Label();
            btnAddBot = new Button();
            btnRemoveBot = new Button();
            groupBoxStatus = new GroupBox();
            labelStatus = new Label();
            labelStatusValue = new Label();
            labelPing = new Label();
            labelPingValue = new Label();
            labelRoom = new Label();
            labelRoomValue = new Label();
            labelPlayers = new Label();
            labelPlayersValue = new Label();
            labelMaster = new Label();
            labelMasterValue = new Label();
            groupBoxConfig = new GroupBox();
            labelBotName = new Label();
            textBoxBotName = new TextBox();
            labelClanTag = new Label();
            textBoxClanTag = new TextBox();
            labelTeam = new Label();
            comboBoxTeam = new ComboBox();
            labelRank = new Label();
            numericRank = new NumericUpDown();
            groupBoxRoomActions = new GroupBox();
            button1 = new Button();
            btnJoinRandom = new Button();
            btnJoinRandomAll = new Button();
            btnCreateRoom = new Button();
            btnLeaveRoom = new Button();
            labelRoomName = new Label();
            textBoxRoomName = new TextBox();
            btnJoinByName = new Button();
            btnJoinByNameAll = new Button();
            labelMap = new Label();
            comboBoxMap = new ComboBox();
            labelGameMode = new Label();
            comboBoxGameMode = new ComboBox();
            labelMaxPlayers = new Label();
            numericMaxPlayers = new NumericUpDown();
            labelMaxPing = new Label();
            numericMaxPing = new NumericUpDown();
            groupBoxBotActions = new GroupBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            btnSpawn = new Button();
            btnStartMovement = new Button();
            btnStopMovement = new Button();
            groupBoxPlayerList = new GroupBox();
            listBoxPlayers = new ListBox();
            btnRefreshPlayers = new Button();
            checkBoxOrbitPlayer = new CheckBox();
            checkBoxFollowPlayer = new CheckBox();
            btnOrbitAllBots = new Button();
            btnFollowAllBots = new Button();
            groupBoxAnnouncements = new GroupBox();
            labelAnnouncementText = new Label();
            textBoxAnnouncement = new TextBox();
            labelDuration = new Label();
            numericDuration = new NumericUpDown();
            btnSendAnnouncement = new Button();
            groupBoxEventLog = new GroupBox();
            listBoxEventLog = new ListBox();
            btnClearLog = new Button();
            checkBoxAutoScroll = new CheckBox();
            groupBoxConsoleLog = new GroupBox();
            textBoxConsoleLog = new TextBox();
            btnClearConsole = new Button();
            groupBoxStatus.SuspendLayout();
            groupBoxConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericRank).BeginInit();
            groupBoxRoomActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericMaxPlayers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMaxPing).BeginInit();
            groupBoxBotActions.SuspendLayout();
            groupBoxPlayerList.SuspendLayout();
            groupBoxAnnouncements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericDuration).BeginInit();
            groupBoxEventLog.SuspendLayout();
            groupBoxConsoleLog.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxBots
            // 
            listBoxBots.BackColor = Color.FromArgb(40, 40, 43);
            listBoxBots.BorderStyle = BorderStyle.FixedSingle;
            listBoxBots.ForeColor = Color.FromArgb(220, 220, 220);
            listBoxBots.FormattingEnabled = true;
            listBoxBots.ItemHeight = 15;
            listBoxBots.Location = new Point(12, 27);
            listBoxBots.Name = "listBoxBots";
            listBoxBots.Size = new Size(200, 347);
            listBoxBots.TabIndex = 1;
            listBoxBots.SelectedIndexChanged += listBoxBots_SelectedIndexChanged;
            // 
            // labelBotList
            // 
            labelBotList.AutoSize = true;
            labelBotList.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelBotList.ForeColor = Color.FromArgb(220, 220, 220);
            labelBotList.Location = new Point(12, 9);
            labelBotList.Name = "labelBotList";
            labelBotList.Size = new Size(85, 15);
            labelBotList.TabIndex = 0;
            labelBotList.Text = "Bot Instances:";
            // 
            // btnAddBot
            // 
            btnAddBot.BackColor = Color.FromArgb(255, 68, 68);
            btnAddBot.FlatAppearance.BorderSize = 0;
            btnAddBot.FlatStyle = FlatStyle.Flat;
            btnAddBot.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddBot.ForeColor = Color.White;
            btnAddBot.Location = new Point(12, 380);
            btnAddBot.Name = "btnAddBot";
            btnAddBot.Size = new Size(95, 30);
            btnAddBot.TabIndex = 2;
            btnAddBot.Text = "Add Bot";
            btnAddBot.UseVisualStyleBackColor = false;
            btnAddBot.Click += btnAddBot_Click;
            // 
            // btnRemoveBot
            // 
            btnRemoveBot.BackColor = Color.FromArgb(60, 60, 63);
            btnRemoveBot.FlatAppearance.BorderSize = 0;
            btnRemoveBot.FlatStyle = FlatStyle.Flat;
            btnRemoveBot.Font = new Font("Segoe UI", 9F);
            btnRemoveBot.ForeColor = Color.FromArgb(220, 220, 220);
            btnRemoveBot.Location = new Point(113, 380);
            btnRemoveBot.Name = "btnRemoveBot";
            btnRemoveBot.Size = new Size(99, 30);
            btnRemoveBot.TabIndex = 3;
            btnRemoveBot.Text = "Remove";
            btnRemoveBot.UseVisualStyleBackColor = false;
            btnRemoveBot.Click += btnRemoveBot_Click;
            // 
            // groupBoxStatus
            // 
            groupBoxStatus.Controls.Add(labelStatus);
            groupBoxStatus.Controls.Add(labelStatusValue);
            groupBoxStatus.Controls.Add(labelPing);
            groupBoxStatus.Controls.Add(labelPingValue);
            groupBoxStatus.Controls.Add(labelRoom);
            groupBoxStatus.Controls.Add(labelRoomValue);
            groupBoxStatus.Controls.Add(labelPlayers);
            groupBoxStatus.Controls.Add(labelPlayersValue);
            groupBoxStatus.Controls.Add(labelMaster);
            groupBoxStatus.Controls.Add(labelMasterValue);
            groupBoxStatus.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxStatus.Location = new Point(220, 9);
            groupBoxStatus.Name = "groupBoxStatus";
            groupBoxStatus.Size = new Size(1072, 70);
            groupBoxStatus.TabIndex = 4;
            groupBoxStatus.TabStop = false;
            groupBoxStatus.Text = "Connection Status";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.ForeColor = Color.FromArgb(150, 150, 150);
            labelStatus.Location = new Point(15, 25);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(42, 15);
            labelStatus.TabIndex = 0;
            labelStatus.Text = "Status:";
            // 
            // labelStatusValue
            // 
            labelStatusValue.AutoSize = true;
            labelStatusValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelStatusValue.ForeColor = Color.FromArgb(100, 255, 100);
            labelStatusValue.Location = new Point(15, 40);
            labelStatusValue.Name = "labelStatusValue";
            labelStatusValue.Size = new Size(29, 15);
            labelStatusValue.TabIndex = 1;
            labelStatusValue.Text = "N/A";
            // 
            // labelPing
            // 
            labelPing.AutoSize = true;
            labelPing.ForeColor = Color.FromArgb(150, 150, 150);
            labelPing.Location = new Point(200, 25);
            labelPing.Name = "labelPing";
            labelPing.Size = new Size(34, 15);
            labelPing.TabIndex = 2;
            labelPing.Text = "Ping:";
            // 
            // labelPingValue
            // 
            labelPingValue.AutoSize = true;
            labelPingValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPingValue.ForeColor = Color.FromArgb(220, 220, 220);
            labelPingValue.Location = new Point(200, 40);
            labelPingValue.Name = "labelPingValue";
            labelPingValue.Size = new Size(29, 15);
            labelPingValue.TabIndex = 3;
            labelPingValue.Text = "N/A";
            // 
            // labelRoom
            // 
            labelRoom.AutoSize = true;
            labelRoom.ForeColor = Color.FromArgb(150, 150, 150);
            labelRoom.Location = new Point(380, 25);
            labelRoom.Name = "labelRoom";
            labelRoom.Size = new Size(42, 15);
            labelRoom.TabIndex = 4;
            labelRoom.Text = "Room:";
            // 
            // labelRoomValue
            // 
            labelRoomValue.AutoSize = true;
            labelRoomValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelRoomValue.ForeColor = Color.FromArgb(220, 220, 220);
            labelRoomValue.Location = new Point(380, 40);
            labelRoomValue.Name = "labelRoomValue";
            labelRoomValue.Size = new Size(29, 15);
            labelRoomValue.TabIndex = 5;
            labelRoomValue.Text = "N/A";
            // 
            // labelPlayers
            // 
            labelPlayers.AutoSize = true;
            labelPlayers.ForeColor = Color.FromArgb(150, 150, 150);
            labelPlayers.Location = new Point(660, 25);
            labelPlayers.Name = "labelPlayers";
            labelPlayers.Size = new Size(47, 15);
            labelPlayers.TabIndex = 6;
            labelPlayers.Text = "Players:";
            // 
            // labelPlayersValue
            // 
            labelPlayersValue.AutoSize = true;
            labelPlayersValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPlayersValue.ForeColor = Color.FromArgb(220, 220, 220);
            labelPlayersValue.Location = new Point(660, 40);
            labelPlayersValue.Name = "labelPlayersValue";
            labelPlayersValue.Size = new Size(29, 15);
            labelPlayersValue.TabIndex = 7;
            labelPlayersValue.Text = "N/A";
            // 
            // labelMaster
            // 
            labelMaster.AutoSize = true;
            labelMaster.ForeColor = Color.FromArgb(150, 150, 150);
            labelMaster.Location = new Point(920, 25);
            labelMaster.Name = "labelMaster";
            labelMaster.Size = new Size(60, 15);
            labelMaster.TabIndex = 8;
            labelMaster.Text = "Master ID:";
            // 
            // labelMasterValue
            // 
            labelMasterValue.AutoSize = true;
            labelMasterValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelMasterValue.ForeColor = Color.FromArgb(220, 220, 220);
            labelMasterValue.Location = new Point(920, 40);
            labelMasterValue.Name = "labelMasterValue";
            labelMasterValue.Size = new Size(29, 15);
            labelMasterValue.TabIndex = 9;
            labelMasterValue.Text = "N/A";
            // 
            // groupBoxConfig
            // 
            groupBoxConfig.Controls.Add(labelBotName);
            groupBoxConfig.Controls.Add(textBoxBotName);
            groupBoxConfig.Controls.Add(labelClanTag);
            groupBoxConfig.Controls.Add(textBoxClanTag);
            groupBoxConfig.Controls.Add(labelTeam);
            groupBoxConfig.Controls.Add(comboBoxTeam);
            groupBoxConfig.Controls.Add(labelRank);
            groupBoxConfig.Controls.Add(numericRank);
            groupBoxConfig.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxConfig.Location = new Point(220, 85);
            groupBoxConfig.Name = "groupBoxConfig";
            groupBoxConfig.Size = new Size(260, 212);
            groupBoxConfig.TabIndex = 5;
            groupBoxConfig.TabStop = false;
            groupBoxConfig.Text = "Bot Configuration";
            // 
            // labelBotName
            // 
            labelBotName.AutoSize = true;
            labelBotName.ForeColor = Color.FromArgb(180, 180, 180);
            labelBotName.Location = new Point(15, 25);
            labelBotName.Name = "labelBotName";
            labelBotName.Size = new Size(63, 15);
            labelBotName.TabIndex = 0;
            labelBotName.Text = "Bot Name:";
            // 
            // textBoxBotName
            // 
            textBoxBotName.BackColor = Color.FromArgb(40, 40, 43);
            textBoxBotName.BorderStyle = BorderStyle.FixedSingle;
            textBoxBotName.ForeColor = Color.FromArgb(220, 220, 220);
            textBoxBotName.Location = new Point(15, 43);
            textBoxBotName.Name = "textBoxBotName";
            textBoxBotName.Size = new Size(230, 23);
            textBoxBotName.TabIndex = 1;
            textBoxBotName.Text = "PC-Bot_1234";
            // 
            // labelClanTag
            // 
            labelClanTag.AutoSize = true;
            labelClanTag.ForeColor = Color.FromArgb(180, 180, 180);
            labelClanTag.Location = new Point(15, 75);
            labelClanTag.Name = "labelClanTag";
            labelClanTag.Size = new Size(56, 15);
            labelClanTag.TabIndex = 2;
            labelClanTag.Text = "Clan Tag:";
            // 
            // textBoxClanTag
            // 
            textBoxClanTag.BackColor = Color.FromArgb(40, 40, 43);
            textBoxClanTag.BorderStyle = BorderStyle.FixedSingle;
            textBoxClanTag.ForeColor = Color.FromArgb(220, 220, 220);
            textBoxClanTag.Location = new Point(15, 93);
            textBoxClanTag.Name = "textBoxClanTag";
            textBoxClanTag.Size = new Size(230, 23);
            textBoxClanTag.TabIndex = 3;
            textBoxClanTag.Text = "[BOT]";
            // 
            // labelTeam
            // 
            labelTeam.AutoSize = true;
            labelTeam.ForeColor = Color.FromArgb(180, 180, 180);
            labelTeam.Location = new Point(13, 150);
            labelTeam.Name = "labelTeam";
            labelTeam.Size = new Size(39, 15);
            labelTeam.TabIndex = 4;
            labelTeam.Text = "Team:";
            // 
            // comboBoxTeam
            // 
            comboBoxTeam.BackColor = Color.FromArgb(40, 40, 43);
            comboBoxTeam.FlatStyle = FlatStyle.Flat;
            comboBoxTeam.ForeColor = Color.FromArgb(220, 220, 220);
            comboBoxTeam.FormattingEnabled = true;
            comboBoxTeam.Items.AddRange(new object[] { "Team 1", "Team 2" });
            comboBoxTeam.Location = new Point(15, 168);
            comboBoxTeam.Name = "comboBoxTeam";
            comboBoxTeam.Size = new Size(100, 23);
            comboBoxTeam.TabIndex = 5;
            // 
            // labelRank
            // 
            labelRank.AutoSize = true;
            labelRank.ForeColor = Color.FromArgb(180, 180, 180);
            labelRank.Location = new Point(137, 150);
            labelRank.Name = "labelRank";
            labelRank.Size = new Size(36, 15);
            labelRank.TabIndex = 6;
            labelRank.Text = "Rank:";
            // 
            // numericRank
            // 
            numericRank.BackColor = Color.FromArgb(40, 40, 43);
            numericRank.BorderStyle = BorderStyle.FixedSingle;
            numericRank.ForeColor = Color.FromArgb(220, 220, 220);
            numericRank.Location = new Point(140, 168);
            numericRank.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numericRank.Name = "numericRank";
            numericRank.Size = new Size(105, 23);
            numericRank.TabIndex = 7;
            numericRank.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // groupBoxRoomActions
            // 
            groupBoxRoomActions.Controls.Add(button1);
            groupBoxRoomActions.Controls.Add(btnJoinRandom);
            groupBoxRoomActions.Controls.Add(btnJoinRandomAll);
            groupBoxRoomActions.Controls.Add(btnCreateRoom);
            groupBoxRoomActions.Controls.Add(btnLeaveRoom);
            groupBoxRoomActions.Controls.Add(labelRoomName);
            groupBoxRoomActions.Controls.Add(textBoxRoomName);
            groupBoxRoomActions.Controls.Add(btnJoinByName);
            groupBoxRoomActions.Controls.Add(btnJoinByNameAll);
            groupBoxRoomActions.Controls.Add(labelMap);
            groupBoxRoomActions.Controls.Add(comboBoxMap);
            groupBoxRoomActions.Controls.Add(labelGameMode);
            groupBoxRoomActions.Controls.Add(comboBoxGameMode);
            groupBoxRoomActions.Controls.Add(labelMaxPlayers);
            groupBoxRoomActions.Controls.Add(numericMaxPlayers);
            groupBoxRoomActions.Controls.Add(labelMaxPing);
            groupBoxRoomActions.Controls.Add(numericMaxPing);
            groupBoxRoomActions.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxRoomActions.Location = new Point(486, 85);
            groupBoxRoomActions.Name = "groupBoxRoomActions";
            groupBoxRoomActions.Size = new Size(540, 266);
            groupBoxRoomActions.TabIndex = 6;
            groupBoxRoomActions.TabStop = false;
            groupBoxRoomActions.Text = "Room Actions";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(60, 60, 63);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.FromArgb(220, 220, 220);
            button1.Location = new Point(403, 215);
            button1.Name = "button1";
            button1.Size = new Size(122, 35);
            button1.TabIndex = 6;
            button1.Text = "Leave (All)";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btnJoinRandom
            // 
            btnJoinRandom.BackColor = Color.FromArgb(60, 60, 63);
            btnJoinRandom.FlatAppearance.BorderSize = 0;
            btnJoinRandom.FlatStyle = FlatStyle.Flat;
            btnJoinRandom.ForeColor = Color.FromArgb(220, 220, 220);
            btnJoinRandom.Location = new Point(15, 25);
            btnJoinRandom.Name = "btnJoinRandom";
            btnJoinRandom.Size = new Size(244, 30);
            btnJoinRandom.TabIndex = 0;
            btnJoinRandom.Text = "Join Random Room";
            btnJoinRandom.UseVisualStyleBackColor = false;
            btnJoinRandom.Click += btnJoinRandom_Click;
            // 
            // btnJoinRandomAll
            // 
            btnJoinRandomAll.BackColor = Color.FromArgb(255, 140, 0);
            btnJoinRandomAll.FlatAppearance.BorderSize = 0;
            btnJoinRandomAll.FlatStyle = FlatStyle.Flat;
            btnJoinRandomAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnJoinRandomAll.ForeColor = Color.White;
            btnJoinRandomAll.Location = new Point(270, 25);
            btnJoinRandomAll.Name = "btnJoinRandomAll";
            btnJoinRandomAll.Size = new Size(255, 30);
            btnJoinRandomAll.TabIndex = 1;
            btnJoinRandomAll.Text = "Join Random (All)";
            btnJoinRandomAll.UseVisualStyleBackColor = false;
            btnJoinRandomAll.Click += btnJoinRandomAll_Click;
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.BackColor = Color.FromArgb(255, 68, 68);
            btnCreateRoom.FlatAppearance.BorderSize = 0;
            btnCreateRoom.FlatStyle = FlatStyle.Flat;
            btnCreateRoom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCreateRoom.ForeColor = Color.White;
            btnCreateRoom.Location = new Point(15, 215);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(244, 35);
            btnCreateRoom.TabIndex = 14;
            btnCreateRoom.Text = "Create Room";
            btnCreateRoom.UseVisualStyleBackColor = false;
            btnCreateRoom.Click += btnCreateRoom_Click;
            // 
            // btnLeaveRoom
            // 
            btnLeaveRoom.BackColor = Color.FromArgb(60, 60, 63);
            btnLeaveRoom.FlatAppearance.BorderSize = 0;
            btnLeaveRoom.FlatStyle = FlatStyle.Flat;
            btnLeaveRoom.ForeColor = Color.FromArgb(220, 220, 220);
            btnLeaveRoom.Location = new Point(270, 215);
            btnLeaveRoom.Name = "btnLeaveRoom";
            btnLeaveRoom.Size = new Size(122, 35);
            btnLeaveRoom.TabIndex = 15;
            btnLeaveRoom.Text = "Leave Room";
            btnLeaveRoom.UseVisualStyleBackColor = false;
            btnLeaveRoom.Click += btnLeaveRoom_Click;
            // 
            // labelRoomName
            // 
            labelRoomName.AutoSize = true;
            labelRoomName.ForeColor = Color.FromArgb(180, 180, 180);
            labelRoomName.Location = new Point(15, 65);
            labelRoomName.Name = "labelRoomName";
            labelRoomName.Size = new Size(77, 15);
            labelRoomName.TabIndex = 2;
            labelRoomName.Text = "Room Name:";
            // 
            // textBoxRoomName
            // 
            textBoxRoomName.BackColor = Color.FromArgb(40, 40, 43);
            textBoxRoomName.BorderStyle = BorderStyle.FixedSingle;
            textBoxRoomName.ForeColor = Color.FromArgb(220, 220, 220);
            textBoxRoomName.Location = new Point(15, 83);
            textBoxRoomName.Name = "textBoxRoomName";
            textBoxRoomName.PlaceholderText = "Enter room name...";
            textBoxRoomName.Size = new Size(319, 23);
            textBoxRoomName.TabIndex = 3;
            // 
            // btnJoinByName
            // 
            btnJoinByName.BackColor = Color.FromArgb(60, 60, 63);
            btnJoinByName.FlatAppearance.BorderSize = 0;
            btnJoinByName.FlatStyle = FlatStyle.Flat;
            btnJoinByName.ForeColor = Color.FromArgb(220, 220, 220);
            btnJoinByName.Location = new Point(340, 83);
            btnJoinByName.Name = "btnJoinByName";
            btnJoinByName.Size = new Size(79, 23);
            btnJoinByName.TabIndex = 4;
            btnJoinByName.Text = "Join";
            btnJoinByName.UseVisualStyleBackColor = false;
            btnJoinByName.Click += btnJoinByName_Click;
            // 
            // btnJoinByNameAll
            // 
            btnJoinByNameAll.BackColor = Color.FromArgb(255, 140, 0);
            btnJoinByNameAll.FlatAppearance.BorderSize = 0;
            btnJoinByNameAll.FlatStyle = FlatStyle.Flat;
            btnJoinByNameAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnJoinByNameAll.ForeColor = Color.White;
            btnJoinByNameAll.Location = new Point(425, 83);
            btnJoinByNameAll.Name = "btnJoinByNameAll";
            btnJoinByNameAll.Size = new Size(100, 23);
            btnJoinByNameAll.TabIndex = 5;
            btnJoinByNameAll.Text = "Join (All)";
            btnJoinByNameAll.UseVisualStyleBackColor = false;
            btnJoinByNameAll.Click += btnJoinByNameAll_Click;
            // 
            // labelMap
            // 
            labelMap.AutoSize = true;
            labelMap.ForeColor = Color.FromArgb(180, 180, 180);
            labelMap.Location = new Point(15, 109);
            labelMap.Name = "labelMap";
            labelMap.Size = new Size(34, 15);
            labelMap.TabIndex = 6;
            labelMap.Text = "Map:";
            // 
            // comboBoxMap
            // 
            comboBoxMap.BackColor = Color.FromArgb(40, 40, 43);
            comboBoxMap.FlatStyle = FlatStyle.Flat;
            comboBoxMap.ForeColor = Color.FromArgb(220, 220, 220);
            comboBoxMap.FormattingEnabled = true;
            comboBoxMap.Location = new Point(15, 127);
            comboBoxMap.Name = "comboBoxMap";
            comboBoxMap.Size = new Size(160, 23);
            comboBoxMap.TabIndex = 7;
            // 
            // labelGameMode
            // 
            labelGameMode.AutoSize = true;
            labelGameMode.ForeColor = Color.FromArgb(180, 180, 180);
            labelGameMode.Location = new Point(190, 109);
            labelGameMode.Name = "labelGameMode";
            labelGameMode.Size = new Size(75, 15);
            labelGameMode.TabIndex = 8;
            labelGameMode.Text = "Game Mode:";
            // 
            // comboBoxGameMode
            // 
            comboBoxGameMode.BackColor = Color.FromArgb(40, 40, 43);
            comboBoxGameMode.FlatStyle = FlatStyle.Flat;
            comboBoxGameMode.ForeColor = Color.FromArgb(220, 220, 220);
            comboBoxGameMode.FormattingEnabled = true;
            comboBoxGameMode.Location = new Point(190, 127);
            comboBoxGameMode.Name = "comboBoxGameMode";
            comboBoxGameMode.Size = new Size(160, 23);
            comboBoxGameMode.TabIndex = 9;
            // 
            // labelMaxPlayers
            // 
            labelMaxPlayers.AutoSize = true;
            labelMaxPlayers.ForeColor = Color.FromArgb(180, 180, 180);
            labelMaxPlayers.Location = new Point(365, 109);
            labelMaxPlayers.Name = "labelMaxPlayers";
            labelMaxPlayers.Size = new Size(72, 15);
            labelMaxPlayers.TabIndex = 10;
            labelMaxPlayers.Text = "Max Players:";
            // 
            // numericMaxPlayers
            // 
            numericMaxPlayers.BackColor = Color.FromArgb(40, 40, 43);
            numericMaxPlayers.BorderStyle = BorderStyle.FixedSingle;
            numericMaxPlayers.ForeColor = Color.FromArgb(220, 220, 220);
            numericMaxPlayers.Location = new Point(365, 127);
            numericMaxPlayers.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            numericMaxPlayers.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numericMaxPlayers.Name = "numericMaxPlayers";
            numericMaxPlayers.Size = new Size(160, 23);
            numericMaxPlayers.TabIndex = 11;
            numericMaxPlayers.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // labelMaxPing
            // 
            labelMaxPing.AutoSize = true;
            labelMaxPing.ForeColor = Color.FromArgb(180, 180, 180);
            labelMaxPing.Location = new Point(15, 162);
            labelMaxPing.Name = "labelMaxPing";
            labelMaxPing.Size = new Size(59, 15);
            labelMaxPing.TabIndex = 12;
            labelMaxPing.Text = "Max Ping:";
            // 
            // numericMaxPing
            // 
            numericMaxPing.BackColor = Color.FromArgb(40, 40, 43);
            numericMaxPing.BorderStyle = BorderStyle.FixedSingle;
            numericMaxPing.ForeColor = Color.FromArgb(220, 220, 220);
            numericMaxPing.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numericMaxPing.Location = new Point(15, 180);
            numericMaxPing.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numericMaxPing.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numericMaxPing.Name = "numericMaxPing";
            numericMaxPing.Size = new Size(160, 23);
            numericMaxPing.TabIndex = 13;
            numericMaxPing.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // groupBoxBotActions
            // 
            groupBoxBotActions.Controls.Add(btnConnect);
            groupBoxBotActions.Controls.Add(btnDisconnect);
            groupBoxBotActions.Controls.Add(btnSpawn);
            groupBoxBotActions.Controls.Add(btnStartMovement);
            groupBoxBotActions.Controls.Add(btnStopMovement);
            groupBoxBotActions.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxBotActions.Location = new Point(486, 357);
            groupBoxBotActions.Name = "groupBoxBotActions";
            groupBoxBotActions.Size = new Size(540, 117);
            groupBoxBotActions.TabIndex = 7;
            groupBoxBotActions.TabStop = false;
            groupBoxBotActions.Text = "Bot Actions";
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(255, 68, 68);
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(15, 25);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(160, 35);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.BackColor = Color.FromArgb(60, 60, 63);
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.ForeColor = Color.FromArgb(220, 220, 220);
            btnDisconnect.Location = new Point(181, 25);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(178, 35);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnSpawn
            // 
            btnSpawn.BackColor = Color.FromArgb(60, 60, 63);
            btnSpawn.FlatAppearance.BorderSize = 0;
            btnSpawn.FlatStyle = FlatStyle.Flat;
            btnSpawn.ForeColor = Color.FromArgb(220, 220, 220);
            btnSpawn.Location = new Point(365, 25);
            btnSpawn.Name = "btnSpawn";
            btnSpawn.Size = new Size(160, 35);
            btnSpawn.TabIndex = 2;
            btnSpawn.Text = "Spawn Bot";
            btnSpawn.UseVisualStyleBackColor = false;
            btnSpawn.Click += btnSpawn_Click;
            // 
            // btnStartMovement
            // 
            btnStartMovement.BackColor = Color.FromArgb(60, 60, 63);
            btnStartMovement.FlatAppearance.BorderSize = 0;
            btnStartMovement.FlatStyle = FlatStyle.Flat;
            btnStartMovement.ForeColor = Color.FromArgb(220, 220, 220);
            btnStartMovement.Location = new Point(276, 66);
            btnStartMovement.Name = "btnStartMovement";
            btnStartMovement.Size = new Size(122, 35);
            btnStartMovement.TabIndex = 4;
            btnStartMovement.Text = "Start Movement";
            btnStartMovement.UseVisualStyleBackColor = false;
            btnStartMovement.Click += btnStartMovement_Click;
            // 
            // btnStopMovement
            // 
            btnStopMovement.BackColor = Color.FromArgb(60, 60, 63);
            btnStopMovement.FlatAppearance.BorderSize = 0;
            btnStopMovement.FlatStyle = FlatStyle.Flat;
            btnStopMovement.ForeColor = Color.FromArgb(220, 220, 220);
            btnStopMovement.Location = new Point(404, 66);
            btnStopMovement.Name = "btnStopMovement";
            btnStopMovement.Size = new Size(121, 35);
            btnStopMovement.TabIndex = 5;
            btnStopMovement.Text = "Stop Movement";
            btnStopMovement.UseVisualStyleBackColor = false;
            btnStopMovement.Click += btnStopMovement_Click;
            // 
            // groupBoxPlayerList
            // 
            groupBoxPlayerList.Controls.Add(listBoxPlayers);
            groupBoxPlayerList.Controls.Add(btnRefreshPlayers);
            groupBoxPlayerList.Controls.Add(checkBoxOrbitPlayer);
            groupBoxPlayerList.Controls.Add(checkBoxFollowPlayer);
            groupBoxPlayerList.Controls.Add(btnOrbitAllBots);
            groupBoxPlayerList.Controls.Add(btnFollowAllBots);
            groupBoxPlayerList.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxPlayerList.Location = new Point(1032, 85);
            groupBoxPlayerList.Name = "groupBoxPlayerList";
            groupBoxPlayerList.Size = new Size(260, 389);
            groupBoxPlayerList.TabIndex = 8;
            groupBoxPlayerList.TabStop = false;
            groupBoxPlayerList.Text = "Player List & Controls";
            // 
            // listBoxPlayers
            // 
            listBoxPlayers.BackColor = Color.FromArgb(40, 40, 43);
            listBoxPlayers.BorderStyle = BorderStyle.FixedSingle;
            listBoxPlayers.ForeColor = Color.FromArgb(220, 220, 220);
            listBoxPlayers.FormattingEnabled = true;
            listBoxPlayers.ItemHeight = 15;
            listBoxPlayers.Location = new Point(15, 25);
            listBoxPlayers.Name = "listBoxPlayers";
            listBoxPlayers.Size = new Size(230, 167);
            listBoxPlayers.TabIndex = 0;
            // 
            // btnRefreshPlayers
            // 
            btnRefreshPlayers.BackColor = Color.FromArgb(60, 60, 63);
            btnRefreshPlayers.FlatAppearance.BorderSize = 0;
            btnRefreshPlayers.FlatStyle = FlatStyle.Flat;
            btnRefreshPlayers.ForeColor = Color.FromArgb(220, 220, 220);
            btnRefreshPlayers.Location = new Point(15, 198);
            btnRefreshPlayers.Name = "btnRefreshPlayers";
            btnRefreshPlayers.Size = new Size(230, 26);
            btnRefreshPlayers.TabIndex = 1;
            btnRefreshPlayers.Text = "Refresh Players";
            btnRefreshPlayers.UseVisualStyleBackColor = false;
            btnRefreshPlayers.Click += btnRefreshPlayers_Click;
            // 
            // checkBoxOrbitPlayer
            // 
            checkBoxOrbitPlayer.AutoSize = true;
            checkBoxOrbitPlayer.ForeColor = Color.FromArgb(180, 180, 180);
            checkBoxOrbitPlayer.Location = new Point(15, 235);
            checkBoxOrbitPlayer.Name = "checkBoxOrbitPlayer";
            checkBoxOrbitPlayer.Size = new Size(132, 19);
            checkBoxOrbitPlayer.TabIndex = 2;
            checkBoxOrbitPlayer.Text = "Orbit Selected Actor";
            checkBoxOrbitPlayer.UseVisualStyleBackColor = true;
            checkBoxOrbitPlayer.CheckedChanged += checkBoxOrbitPlayer_CheckedChanged;
            // 
            // checkBoxFollowPlayer
            // 
            checkBoxFollowPlayer.AutoSize = true;
            checkBoxFollowPlayer.ForeColor = Color.FromArgb(180, 180, 180);
            checkBoxFollowPlayer.Location = new Point(15, 260);
            checkBoxFollowPlayer.Name = "checkBoxFollowPlayer";
            checkBoxFollowPlayer.Size = new Size(140, 19);
            checkBoxFollowPlayer.TabIndex = 3;
            checkBoxFollowPlayer.Text = "Follow Selected Actor";
            checkBoxFollowPlayer.UseVisualStyleBackColor = true;
            checkBoxFollowPlayer.CheckedChanged += checkBoxFollowPlayer_CheckedChanged;
            // 
            // btnOrbitAllBots
            // 
            btnOrbitAllBots.BackColor = Color.FromArgb(255, 140, 0);
            btnOrbitAllBots.FlatAppearance.BorderSize = 0;
            btnOrbitAllBots.FlatStyle = FlatStyle.Flat;
            btnOrbitAllBots.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnOrbitAllBots.ForeColor = Color.White;
            btnOrbitAllBots.Location = new Point(15, 290);
            btnOrbitAllBots.Name = "btnOrbitAllBots";
            btnOrbitAllBots.Size = new Size(230, 38);
            btnOrbitAllBots.TabIndex = 4;
            btnOrbitAllBots.Text = "Orbit Actor (All)";
            btnOrbitAllBots.UseVisualStyleBackColor = false;
            btnOrbitAllBots.Click += btnOrbitAllBots_Click;
            // 
            // btnFollowAllBots
            // 
            btnFollowAllBots.BackColor = Color.FromArgb(100, 149, 237);
            btnFollowAllBots.FlatAppearance.BorderSize = 0;
            btnFollowAllBots.FlatStyle = FlatStyle.Flat;
            btnFollowAllBots.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFollowAllBots.ForeColor = Color.White;
            btnFollowAllBots.Location = new Point(15, 334);
            btnFollowAllBots.Name = "btnFollowAllBots";
            btnFollowAllBots.Size = new Size(230, 38);
            btnFollowAllBots.TabIndex = 5;
            btnFollowAllBots.Text = "Follow Actor (All)";
            btnFollowAllBots.UseVisualStyleBackColor = false;
            btnFollowAllBots.Click += btnFollowAllBots_Click;
            // 
            // groupBoxAnnouncements
            // 
            groupBoxAnnouncements.Controls.Add(labelAnnouncementText);
            groupBoxAnnouncements.Controls.Add(textBoxAnnouncement);
            groupBoxAnnouncements.Controls.Add(labelDuration);
            groupBoxAnnouncements.Controls.Add(numericDuration);
            groupBoxAnnouncements.Controls.Add(btnSendAnnouncement);
            groupBoxAnnouncements.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxAnnouncements.Location = new Point(220, 303);
            groupBoxAnnouncements.Name = "groupBoxAnnouncements";
            groupBoxAnnouncements.Size = new Size(260, 171);
            groupBoxAnnouncements.TabIndex = 9;
            groupBoxAnnouncements.TabStop = false;
            groupBoxAnnouncements.Text = "Announcements";
            // 
            // labelAnnouncementText
            // 
            labelAnnouncementText.AutoSize = true;
            labelAnnouncementText.ForeColor = Color.FromArgb(180, 180, 180);
            labelAnnouncementText.Location = new Point(15, 25);
            labelAnnouncementText.Name = "labelAnnouncementText";
            labelAnnouncementText.Size = new Size(31, 15);
            labelAnnouncementText.TabIndex = 0;
            labelAnnouncementText.Text = "Text:";
            // 
            // textBoxAnnouncement
            // 
            textBoxAnnouncement.BackColor = Color.FromArgb(40, 40, 43);
            textBoxAnnouncement.BorderStyle = BorderStyle.FixedSingle;
            textBoxAnnouncement.ForeColor = Color.FromArgb(220, 220, 220);
            textBoxAnnouncement.Location = new Point(15, 43);
            textBoxAnnouncement.Multiline = true;
            textBoxAnnouncement.Name = "textBoxAnnouncement";
            textBoxAnnouncement.PlaceholderText = "Enter announcement text...";
            textBoxAnnouncement.Size = new Size(230, 60);
            textBoxAnnouncement.TabIndex = 1;
            // 
            // labelDuration
            // 
            labelDuration.AutoSize = true;
            labelDuration.ForeColor = Color.FromArgb(180, 180, 180);
            labelDuration.Location = new Point(15, 110);
            labelDuration.Name = "labelDuration";
            labelDuration.Size = new Size(84, 15);
            labelDuration.TabIndex = 2;
            labelDuration.Text = "Duration (sec):";
            // 
            // numericDuration
            // 
            numericDuration.BackColor = Color.FromArgb(40, 40, 43);
            numericDuration.BorderStyle = BorderStyle.FixedSingle;
            numericDuration.DecimalPlaces = 1;
            numericDuration.ForeColor = Color.FromArgb(220, 220, 220);
            numericDuration.Location = new Point(120, 108);
            numericDuration.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numericDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericDuration.Name = "numericDuration";
            numericDuration.Size = new Size(125, 23);
            numericDuration.TabIndex = 3;
            numericDuration.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // btnSendAnnouncement
            // 
            btnSendAnnouncement.BackColor = Color.FromArgb(255, 68, 68);
            btnSendAnnouncement.FlatAppearance.BorderSize = 0;
            btnSendAnnouncement.FlatStyle = FlatStyle.Flat;
            btnSendAnnouncement.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSendAnnouncement.ForeColor = Color.White;
            btnSendAnnouncement.Location = new Point(15, 137);
            btnSendAnnouncement.Name = "btnSendAnnouncement";
            btnSendAnnouncement.Size = new Size(230, 22);
            btnSendAnnouncement.TabIndex = 4;
            btnSendAnnouncement.Text = "Send";
            btnSendAnnouncement.UseVisualStyleBackColor = false;
            btnSendAnnouncement.Click += btnSendAnnouncement_Click;
            // 
            // groupBoxEventLog
            // 
            groupBoxEventLog.Controls.Add(listBoxEventLog);
            groupBoxEventLog.Controls.Add(btnClearLog);
            groupBoxEventLog.Controls.Add(checkBoxAutoScroll);
            groupBoxEventLog.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxEventLog.Location = new Point(12, 480);
            groupBoxEventLog.Name = "groupBoxEventLog";
            groupBoxEventLog.Size = new Size(540, 235);
            groupBoxEventLog.TabIndex = 10;
            groupBoxEventLog.TabStop = false;
            groupBoxEventLog.Text = "Event Log";
            // 
            // listBoxEventLog
            // 
            listBoxEventLog.BackColor = Color.FromArgb(40, 40, 43);
            listBoxEventLog.BorderStyle = BorderStyle.FixedSingle;
            listBoxEventLog.Font = new Font("Consolas", 8.25F);
            listBoxEventLog.ForeColor = Color.FromArgb(100, 255, 100);
            listBoxEventLog.FormattingEnabled = true;
            listBoxEventLog.ItemHeight = 13;
            listBoxEventLog.Location = new Point(15, 25);
            listBoxEventLog.Name = "listBoxEventLog";
            listBoxEventLog.Size = new Size(510, 145);
            listBoxEventLog.TabIndex = 0;
            // 
            // btnClearLog
            // 
            btnClearLog.BackColor = Color.FromArgb(60, 60, 63);
            btnClearLog.FlatAppearance.BorderSize = 0;
            btnClearLog.FlatStyle = FlatStyle.Flat;
            btnClearLog.ForeColor = Color.FromArgb(220, 220, 220);
            btnClearLog.Location = new Point(425, 195);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(100, 28);
            btnClearLog.TabIndex = 2;
            btnClearLog.Text = "Clear";
            btnClearLog.UseVisualStyleBackColor = false;
            btnClearLog.Click += btnClearLog_Click;
            // 
            // checkBoxAutoScroll
            // 
            checkBoxAutoScroll.AutoSize = true;
            checkBoxAutoScroll.Checked = true;
            checkBoxAutoScroll.CheckState = CheckState.Checked;
            checkBoxAutoScroll.ForeColor = Color.FromArgb(180, 180, 180);
            checkBoxAutoScroll.Location = new Point(15, 181);
            checkBoxAutoScroll.Name = "checkBoxAutoScroll";
            checkBoxAutoScroll.Size = new Size(85, 19);
            checkBoxAutoScroll.TabIndex = 1;
            checkBoxAutoScroll.Text = "Auto-scroll";
            checkBoxAutoScroll.UseVisualStyleBackColor = true;
            // 
            // groupBoxConsoleLog
            // 
            groupBoxConsoleLog.Controls.Add(textBoxConsoleLog);
            groupBoxConsoleLog.Controls.Add(btnClearConsole);
            groupBoxConsoleLog.ForeColor = Color.FromArgb(180, 180, 180);
            groupBoxConsoleLog.Location = new Point(558, 480);
            groupBoxConsoleLog.Name = "groupBoxConsoleLog";
            groupBoxConsoleLog.Size = new Size(734, 235);
            groupBoxConsoleLog.TabIndex = 11;
            groupBoxConsoleLog.TabStop = false;
            groupBoxConsoleLog.Text = "Console Output";
            // 
            // textBoxConsoleLog
            // 
            textBoxConsoleLog.BackColor = Color.FromArgb(40, 40, 43);
            textBoxConsoleLog.BorderStyle = BorderStyle.FixedSingle;
            textBoxConsoleLog.Font = new Font("Consolas", 8.25F);
            textBoxConsoleLog.ForeColor = Color.FromArgb(220, 220, 220);
            textBoxConsoleLog.Location = new Point(15, 25);
            textBoxConsoleLog.Multiline = true;
            textBoxConsoleLog.Name = "textBoxConsoleLog";
            textBoxConsoleLog.ReadOnly = true;
            textBoxConsoleLog.ScrollBars = ScrollBars.Vertical;
            textBoxConsoleLog.Size = new Size(704, 145);
            textBoxConsoleLog.TabIndex = 0;
            // 
            // btnClearConsole
            // 
            btnClearConsole.BackColor = Color.FromArgb(60, 60, 63);
            btnClearConsole.FlatAppearance.BorderSize = 0;
            btnClearConsole.FlatStyle = FlatStyle.Flat;
            btnClearConsole.ForeColor = Color.FromArgb(220, 220, 220);
            btnClearConsole.Location = new Point(619, 195);
            btnClearConsole.Name = "btnClearConsole";
            btnClearConsole.Size = new Size(100, 28);
            btnClearConsole.TabIndex = 1;
            btnClearConsole.Text = "Clear";
            btnClearConsole.UseVisualStyleBackColor = false;
            btnClearConsole.Click += btnClearConsole_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 33);
            ClientSize = new Size(1304, 723);
            Controls.Add(groupBoxConsoleLog);
            Controls.Add(groupBoxEventLog);
            Controls.Add(groupBoxAnnouncements);
            Controls.Add(groupBoxPlayerList);
            Controls.Add(groupBoxBotActions);
            Controls.Add(groupBoxRoomActions);
            Controls.Add(groupBoxConfig);
            Controls.Add(groupBoxStatus);
            Controls.Add(btnRemoveBot);
            Controls.Add(btnAddBot);
            Controls.Add(listBoxBots);
            Controls.Add(labelBotList);
            Name = "Form1";
            Text = "Odium Bot Manager - ULTIMATE EDITION";
            Load += Form1_Load;
            groupBoxStatus.ResumeLayout(false);
            groupBoxStatus.PerformLayout();
            groupBoxConfig.ResumeLayout(false);
            groupBoxConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericRank).EndInit();
            groupBoxRoomActions.ResumeLayout(false);
            groupBoxRoomActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericMaxPlayers).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMaxPing).EndInit();
            groupBoxBotActions.ResumeLayout(false);
            groupBoxPlayerList.ResumeLayout(false);
            groupBoxPlayerList.PerformLayout();
            groupBoxAnnouncements.ResumeLayout(false);
            groupBoxAnnouncements.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericDuration).EndInit();
            groupBoxEventLog.ResumeLayout(false);
            groupBoxEventLog.PerformLayout();
            groupBoxConsoleLog.ResumeLayout(false);
            groupBoxConsoleLog.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxBots;
        private Label labelBotList;
        private Button btnAddBot;
        private Button btnRemoveBot;

        private GroupBox groupBoxStatus;
        private Label labelStatus;
        private Label labelStatusValue;
        private Label labelPing;
        private Label labelPingValue;
        private Label labelRoom;
        private Label labelRoomValue;
        private Label labelPlayers;
        private Label labelPlayersValue;
        private Label labelMaster;
        private Label labelMasterValue;

        private GroupBox groupBoxConfig;
        private Label labelBotName;
        private TextBox textBoxBotName;
        private Label labelClanTag;
        private TextBox textBoxClanTag;
        private Label labelTeam;
        private ComboBox comboBoxTeam;
        private Label labelRank;
        private NumericUpDown numericRank;

        private GroupBox groupBoxRoomActions;
        private Button btnJoinRandom;
        private Button btnJoinRandomAll;
        private Button btnCreateRoom;
        private Button btnLeaveRoom;
        private Button btnLeaveRoomAll;
        private Label labelRoomName;
        private TextBox textBoxRoomName;
        private Button btnJoinByName;
        private Button btnJoinByNameAll;
        private Label labelMap;
        private ComboBox comboBoxMap;
        private Label labelGameMode;
        private ComboBox comboBoxGameMode;
        private Label labelMaxPlayers;
        private NumericUpDown numericMaxPlayers;
        private Label labelMaxPing;
        private NumericUpDown numericMaxPing;

        private GroupBox groupBoxBotActions;
        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnDisconnectAll;
        private Button btnSpawn;
        private Button btnStartMovement;
        private Button btnStopMovement;

        private GroupBox groupBoxPlayerList;
        private ListBox listBoxPlayers;
        private Button btnRefreshPlayers;
        private CheckBox checkBoxOrbitPlayer;
        private CheckBox checkBoxFollowPlayer;
        private Button btnOrbitAllBots;
        private Button btnFollowAllBots;

        private GroupBox groupBoxAnnouncements;
        private Label labelAnnouncementText;
        private TextBox textBoxAnnouncement;
        private Label labelDuration;
        private NumericUpDown numericDuration;
        private Button btnSendAnnouncement;

        private GroupBox groupBoxEventLog;
        private ListBox listBoxEventLog;
        private Button btnClearLog;
        private CheckBox checkBoxAutoScroll;

        private GroupBox groupBoxConsoleLog;
        private TextBox textBoxConsoleLog;
        private Button btnClearConsole;
        private Button button1;
    }
}