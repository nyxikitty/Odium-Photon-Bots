using System.Net.NetworkInformation;

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
            lstBots = new ListBox();
            lblBots = new Label();
            btnAddBot = new Button();
            btnRemoveBot = new Button();
            grpStatus = new GroupBox();
            lblConnStatus = new Label();
            lblConnStatusVal = new Label();
            lblPing = new Label();
            lblPingVal = new Label();
            lblCurrentRoom = new Label();
            lblRoomVal = new Label();
            lblPlayerCount = new Label();
            lblPlayerCountVal = new Label();
            lblMasterClient = new Label();
            lblMasterVal = new Label();
            grpBotConfig = new GroupBox();
            lblName = new Label();
            txtBotName = new TextBox();
            lblClan = new Label();
            txtClanTag = new TextBox();
            lblTeam = new Label();
            cmbTeam = new ComboBox();
            lblRank = new Label();
            nudRank = new NumericUpDown();
            grpRooms = new GroupBox();
            btnLeaveAll = new Button();
            btnJoinRandom = new Button();
            btnJoinRandomAll = new Button();
            btnCreateRoom = new Button();
            btnLeave = new Button();
            lblRoomName = new Label();
            txtRoomName = new TextBox();
            btnJoin = new Button();
            btnJoinAll = new Button();
            lblMap = new Label();
            cmbMap = new ComboBox();
            lblMode = new Label();
            cmbGameMode = new ComboBox();
            lblMaxPlayers = new Label();
            nudMaxPlayers = new NumericUpDown();
            lblMaxPing = new Label();
            nudMaxPing = new NumericUpDown();
            grpActions = new GroupBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            btnSpawn = new Button();
            btnMove = new Button();
            btnStopMove = new Button();
            grpPlayers = new GroupBox();
            lstPlayers = new ListBox();
            btnRefresh = new Button();
            chkOrbit = new CheckBox();
            chkFollow = new CheckBox();
            btnOrbitAll = new Button();
            btnFollowAll = new Button();
            grpAnnounce = new GroupBox();
            lblAnnounceText = new Label();
            txtAnnounce = new TextBox();
            lblDuration = new Label();
            nudDuration = new NumericUpDown();
            btnSend = new Button();
            grpEvents = new GroupBox();
            lstEvents = new ListBox();
            btnClearEvents = new Button();
            chkAutoScroll = new CheckBox();
            grpConsole = new GroupBox();
            txtConsole = new TextBox();
            btnClearConsole = new Button();
            groupBox1 = new GroupBox();
            grpStatus.SuspendLayout();
            grpBotConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRank).BeginInit();
            grpRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaxPlayers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxPing).BeginInit();
            grpActions.SuspendLayout();
            grpPlayers.SuspendLayout();
            grpAnnounce.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudDuration).BeginInit();
            grpEvents.SuspendLayout();
            grpConsole.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lstBots
            // 
            lstBots.BackColor = Color.FromArgb(40, 40, 43);
            lstBots.BorderStyle = BorderStyle.FixedSingle;
            lstBots.ForeColor = Color.FromArgb(220, 220, 220);
            lstBots.FormattingEnabled = true;
            lstBots.ItemHeight = 15;
            lstBots.Location = new Point(15, 22);
            lstBots.Name = "lstBots";
            lstBots.Size = new Size(170, 377);
            lstBots.TabIndex = 1;
            lstBots.SelectedIndexChanged += listBoxBots_SelectedIndexChanged;
            // 
            // lblBots
            // 
            lblBots.AutoSize = true;
            lblBots.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBots.ForeColor = Color.FromArgb(220, 220, 220);
            lblBots.Location = new Point(12, 9);
            lblBots.Name = "lblBots";
            lblBots.Size = new Size(0, 15);
            lblBots.TabIndex = 0;
            // 
            // btnAddBot
            // 
            btnAddBot.BackColor = Color.FromArgb(255, 68, 68);
            btnAddBot.FlatAppearance.BorderSize = 0;
            btnAddBot.FlatStyle = FlatStyle.Flat;
            btnAddBot.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddBot.ForeColor = Color.White;
            btnAddBot.Location = new Point(15, 416);
            btnAddBot.Name = "btnAddBot";
            btnAddBot.Size = new Size(79, 30);
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
            btnRemoveBot.Location = new Point(98, 416);
            btnRemoveBot.Name = "btnRemoveBot";
            btnRemoveBot.Size = new Size(87, 30);
            btnRemoveBot.TabIndex = 3;
            btnRemoveBot.Text = "Remove";
            btnRemoveBot.UseVisualStyleBackColor = false;
            btnRemoveBot.Click += btnRemoveBot_Click;
            // 
            // grpStatus
            // 
            grpStatus.Controls.Add(lblConnStatus);
            grpStatus.Controls.Add(lblConnStatusVal);
            grpStatus.Controls.Add(lblPing);
            grpStatus.Controls.Add(lblPingVal);
            grpStatus.Controls.Add(lblCurrentRoom);
            grpStatus.Controls.Add(lblRoomVal);
            grpStatus.Controls.Add(lblPlayerCount);
            grpStatus.Controls.Add(lblPlayerCountVal);
            grpStatus.Controls.Add(lblMasterClient);
            grpStatus.Controls.Add(lblMasterVal);
            grpStatus.ForeColor = Color.FromArgb(180, 180, 180);
            grpStatus.Location = new Point(220, 9);
            grpStatus.Name = "grpStatus";
            grpStatus.Size = new Size(1072, 70);
            grpStatus.TabIndex = 4;
            grpStatus.TabStop = false;
            grpStatus.Text = "Connection Status";
            // 
            // lblConnStatus
            // 
            lblConnStatus.AutoSize = true;
            lblConnStatus.ForeColor = Color.FromArgb(150, 150, 150);
            lblConnStatus.Location = new Point(15, 25);
            lblConnStatus.Name = "lblConnStatus";
            lblConnStatus.Size = new Size(42, 15);
            lblConnStatus.TabIndex = 0;
            lblConnStatus.Text = "Status:";
            // 
            // lblConnStatusVal
            // 
            lblConnStatusVal.AutoSize = true;
            lblConnStatusVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConnStatusVal.ForeColor = Color.FromArgb(100, 255, 100);
            lblConnStatusVal.Location = new Point(15, 40);
            lblConnStatusVal.Name = "lblConnStatusVal";
            lblConnStatusVal.Size = new Size(29, 15);
            lblConnStatusVal.TabIndex = 1;
            lblConnStatusVal.Text = "N/A";
            // 
            // lblPing
            // 
            lblPing.AutoSize = true;
            lblPing.ForeColor = Color.FromArgb(150, 150, 150);
            lblPing.Location = new Point(200, 25);
            lblPing.Name = "lblPing";
            lblPing.Size = new Size(34, 15);
            lblPing.TabIndex = 2;
            lblPing.Text = "Ping:";
            // 
            // lblPingVal
            // 
            lblPingVal.AutoSize = true;
            lblPingVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPingVal.ForeColor = Color.FromArgb(220, 220, 220);
            lblPingVal.Location = new Point(200, 40);
            lblPingVal.Name = "lblPingVal";
            lblPingVal.Size = new Size(29, 15);
            lblPingVal.TabIndex = 3;
            lblPingVal.Text = "N/A";
            // 
            // lblCurrentRoom
            // 
            lblCurrentRoom.AutoSize = true;
            lblCurrentRoom.ForeColor = Color.FromArgb(150, 150, 150);
            lblCurrentRoom.Location = new Point(380, 25);
            lblCurrentRoom.Name = "lblCurrentRoom";
            lblCurrentRoom.Size = new Size(42, 15);
            lblCurrentRoom.TabIndex = 4;
            lblCurrentRoom.Text = "Room:";
            // 
            // lblRoomVal
            // 
            lblRoomVal.AutoSize = true;
            lblRoomVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRoomVal.ForeColor = Color.FromArgb(220, 220, 220);
            lblRoomVal.Location = new Point(380, 40);
            lblRoomVal.Name = "lblRoomVal";
            lblRoomVal.Size = new Size(29, 15);
            lblRoomVal.TabIndex = 5;
            lblRoomVal.Text = "N/A";
            // 
            // lblPlayerCount
            // 
            lblPlayerCount.AutoSize = true;
            lblPlayerCount.ForeColor = Color.FromArgb(150, 150, 150);
            lblPlayerCount.Location = new Point(660, 25);
            lblPlayerCount.Name = "lblPlayerCount";
            lblPlayerCount.Size = new Size(47, 15);
            lblPlayerCount.TabIndex = 6;
            lblPlayerCount.Text = "Players:";
            // 
            // lblPlayerCountVal
            // 
            lblPlayerCountVal.AutoSize = true;
            lblPlayerCountVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPlayerCountVal.ForeColor = Color.FromArgb(220, 220, 220);
            lblPlayerCountVal.Location = new Point(660, 40);
            lblPlayerCountVal.Name = "lblPlayerCountVal";
            lblPlayerCountVal.Size = new Size(29, 15);
            lblPlayerCountVal.TabIndex = 7;
            lblPlayerCountVal.Text = "N/A";
            // 
            // lblMasterClient
            // 
            lblMasterClient.AutoSize = true;
            lblMasterClient.ForeColor = Color.FromArgb(150, 150, 150);
            lblMasterClient.Location = new Point(920, 25);
            lblMasterClient.Name = "lblMasterClient";
            lblMasterClient.Size = new Size(60, 15);
            lblMasterClient.TabIndex = 8;
            lblMasterClient.Text = "Master ID:";
            // 
            // lblMasterVal
            // 
            lblMasterVal.AutoSize = true;
            lblMasterVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMasterVal.ForeColor = Color.FromArgb(220, 220, 220);
            lblMasterVal.Location = new Point(920, 40);
            lblMasterVal.Name = "lblMasterVal";
            lblMasterVal.Size = new Size(29, 15);
            lblMasterVal.TabIndex = 9;
            lblMasterVal.Text = "N/A";
            // 
            // grpBotConfig
            // 
            grpBotConfig.Controls.Add(lblName);
            grpBotConfig.Controls.Add(txtBotName);
            grpBotConfig.Controls.Add(lblClan);
            grpBotConfig.Controls.Add(txtClanTag);
            grpBotConfig.Controls.Add(lblTeam);
            grpBotConfig.Controls.Add(cmbTeam);
            grpBotConfig.Controls.Add(lblRank);
            grpBotConfig.Controls.Add(nudRank);
            grpBotConfig.ForeColor = Color.FromArgb(180, 180, 180);
            grpBotConfig.Location = new Point(220, 85);
            grpBotConfig.Name = "grpBotConfig";
            grpBotConfig.Size = new Size(260, 212);
            grpBotConfig.TabIndex = 5;
            grpBotConfig.TabStop = false;
            grpBotConfig.Text = "Bot Configuration";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(180, 180, 180);
            lblName.Location = new Point(15, 25);
            lblName.Name = "lblName";
            lblName.Size = new Size(63, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Bot Name:";
            // 
            // txtBotName
            // 
            txtBotName.BackColor = Color.FromArgb(40, 40, 43);
            txtBotName.BorderStyle = BorderStyle.FixedSingle;
            txtBotName.ForeColor = Color.FromArgb(220, 220, 220);
            txtBotName.Location = new Point(15, 43);
            txtBotName.Name = "txtBotName";
            txtBotName.Size = new Size(230, 23);
            txtBotName.TabIndex = 1;
            txtBotName.Text = "PC-Bot_1234";
            // 
            // lblClan
            // 
            lblClan.AutoSize = true;
            lblClan.ForeColor = Color.FromArgb(180, 180, 180);
            lblClan.Location = new Point(15, 75);
            lblClan.Name = "lblClan";
            lblClan.Size = new Size(56, 15);
            lblClan.TabIndex = 2;
            lblClan.Text = "Clan Tag:";
            // 
            // txtClanTag
            // 
            txtClanTag.BackColor = Color.FromArgb(40, 40, 43);
            txtClanTag.BorderStyle = BorderStyle.FixedSingle;
            txtClanTag.ForeColor = Color.FromArgb(220, 220, 220);
            txtClanTag.Location = new Point(15, 93);
            txtClanTag.Name = "txtClanTag";
            txtClanTag.Size = new Size(230, 23);
            txtClanTag.TabIndex = 3;
            txtClanTag.Text = "[BOT]";
            // 
            // lblTeam
            // 
            lblTeam.AutoSize = true;
            lblTeam.ForeColor = Color.FromArgb(180, 180, 180);
            lblTeam.Location = new Point(13, 150);
            lblTeam.Name = "lblTeam";
            lblTeam.Size = new Size(39, 15);
            lblTeam.TabIndex = 4;
            lblTeam.Text = "Team:";
            // 
            // cmbTeam
            // 
            cmbTeam.BackColor = Color.FromArgb(40, 40, 43);
            cmbTeam.FlatStyle = FlatStyle.Flat;
            cmbTeam.ForeColor = Color.FromArgb(220, 220, 220);
            cmbTeam.FormattingEnabled = true;
            cmbTeam.Items.AddRange(new object[] { "Team 1", "Team 2" });
            cmbTeam.Location = new Point(15, 168);
            cmbTeam.Name = "cmbTeam";
            cmbTeam.Size = new Size(100, 23);
            cmbTeam.TabIndex = 5;
            // 
            // lblRank
            // 
            lblRank.AutoSize = true;
            lblRank.ForeColor = Color.FromArgb(180, 180, 180);
            lblRank.Location = new Point(137, 150);
            lblRank.Name = "lblRank";
            lblRank.Size = new Size(36, 15);
            lblRank.TabIndex = 6;
            lblRank.Text = "Rank:";
            // 
            // nudRank
            // 
            nudRank.BackColor = Color.FromArgb(40, 40, 43);
            nudRank.BorderStyle = BorderStyle.FixedSingle;
            nudRank.ForeColor = Color.FromArgb(220, 220, 220);
            nudRank.Location = new Point(140, 168);
            nudRank.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudRank.Name = "nudRank";
            nudRank.Size = new Size(105, 23);
            nudRank.TabIndex = 7;
            nudRank.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // grpRooms
            // 
            grpRooms.Controls.Add(btnLeaveAll);
            grpRooms.Controls.Add(btnJoinRandom);
            grpRooms.Controls.Add(btnJoinRandomAll);
            grpRooms.Controls.Add(btnCreateRoom);
            grpRooms.Controls.Add(btnLeave);
            grpRooms.Controls.Add(lblRoomName);
            grpRooms.Controls.Add(txtRoomName);
            grpRooms.Controls.Add(btnJoin);
            grpRooms.Controls.Add(btnJoinAll);
            grpRooms.Controls.Add(lblMap);
            grpRooms.Controls.Add(cmbMap);
            grpRooms.Controls.Add(lblMode);
            grpRooms.Controls.Add(cmbGameMode);
            grpRooms.Controls.Add(lblMaxPlayers);
            grpRooms.Controls.Add(nudMaxPlayers);
            grpRooms.Controls.Add(lblMaxPing);
            grpRooms.Controls.Add(nudMaxPing);
            grpRooms.ForeColor = Color.FromArgb(180, 180, 180);
            grpRooms.Location = new Point(486, 85);
            grpRooms.Name = "grpRooms";
            grpRooms.Size = new Size(540, 266);
            grpRooms.TabIndex = 6;
            grpRooms.TabStop = false;
            grpRooms.Text = "Room Actions";
            // 
            // btnLeaveAll
            // 
            btnLeaveAll.BackColor = Color.FromArgb(60, 60, 63);
            btnLeaveAll.FlatAppearance.BorderSize = 0;
            btnLeaveAll.FlatStyle = FlatStyle.Flat;
            btnLeaveAll.ForeColor = Color.FromArgb(220, 220, 220);
            btnLeaveAll.Location = new Point(403, 215);
            btnLeaveAll.Name = "btnLeaveAll";
            btnLeaveAll.Size = new Size(122, 35);
            btnLeaveAll.TabIndex = 6;
            btnLeaveAll.Text = "Leave (All)";
            btnLeaveAll.UseVisualStyleBackColor = false;
            btnLeaveAll.Click += button1_Click;
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
            // btnLeave
            // 
            btnLeave.BackColor = Color.FromArgb(60, 60, 63);
            btnLeave.FlatAppearance.BorderSize = 0;
            btnLeave.FlatStyle = FlatStyle.Flat;
            btnLeave.ForeColor = Color.FromArgb(220, 220, 220);
            btnLeave.Location = new Point(270, 215);
            btnLeave.Name = "btnLeave";
            btnLeave.Size = new Size(122, 35);
            btnLeave.TabIndex = 15;
            btnLeave.Text = "Leave Room";
            btnLeave.UseVisualStyleBackColor = false;
            btnLeave.Click += btnLeaveRoom_Click;
            // 
            // lblRoomName
            // 
            lblRoomName.AutoSize = true;
            lblRoomName.ForeColor = Color.FromArgb(180, 180, 180);
            lblRoomName.Location = new Point(15, 65);
            lblRoomName.Name = "lblRoomName";
            lblRoomName.Size = new Size(77, 15);
            lblRoomName.TabIndex = 2;
            lblRoomName.Text = "Room Name:";
            // 
            // txtRoomName
            // 
            txtRoomName.BackColor = Color.FromArgb(40, 40, 43);
            txtRoomName.BorderStyle = BorderStyle.FixedSingle;
            txtRoomName.ForeColor = Color.FromArgb(220, 220, 220);
            txtRoomName.Location = new Point(15, 83);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.PlaceholderText = "Enter room name...";
            txtRoomName.Size = new Size(319, 23);
            txtRoomName.TabIndex = 3;
            // 
            // btnJoin
            // 
            btnJoin.BackColor = Color.FromArgb(60, 60, 63);
            btnJoin.FlatAppearance.BorderSize = 0;
            btnJoin.FlatStyle = FlatStyle.Flat;
            btnJoin.ForeColor = Color.FromArgb(220, 220, 220);
            btnJoin.Location = new Point(340, 83);
            btnJoin.Name = "btnJoin";
            btnJoin.Size = new Size(79, 23);
            btnJoin.TabIndex = 4;
            btnJoin.Text = "Join";
            btnJoin.UseVisualStyleBackColor = false;
            btnJoin.Click += btnJoinByName_Click;
            // 
            // btnJoinAll
            // 
            btnJoinAll.BackColor = Color.FromArgb(255, 140, 0);
            btnJoinAll.FlatAppearance.BorderSize = 0;
            btnJoinAll.FlatStyle = FlatStyle.Flat;
            btnJoinAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnJoinAll.ForeColor = Color.White;
            btnJoinAll.Location = new Point(425, 83);
            btnJoinAll.Name = "btnJoinAll";
            btnJoinAll.Size = new Size(100, 23);
            btnJoinAll.TabIndex = 5;
            btnJoinAll.Text = "Join (All)";
            btnJoinAll.UseVisualStyleBackColor = false;
            btnJoinAll.Click += btnJoinByNameAll_Click;
            // 
            // lblMap
            // 
            lblMap.AutoSize = true;
            lblMap.ForeColor = Color.FromArgb(180, 180, 180);
            lblMap.Location = new Point(15, 109);
            lblMap.Name = "lblMap";
            lblMap.Size = new Size(34, 15);
            lblMap.TabIndex = 6;
            lblMap.Text = "Map:";
            // 
            // cmbMap
            // 
            cmbMap.BackColor = Color.FromArgb(40, 40, 43);
            cmbMap.FlatStyle = FlatStyle.Flat;
            cmbMap.ForeColor = Color.FromArgb(220, 220, 220);
            cmbMap.FormattingEnabled = true;
            cmbMap.Location = new Point(15, 127);
            cmbMap.Name = "cmbMap";
            cmbMap.Size = new Size(160, 23);
            cmbMap.TabIndex = 7;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.ForeColor = Color.FromArgb(180, 180, 180);
            lblMode.Location = new Point(190, 109);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(75, 15);
            lblMode.TabIndex = 8;
            lblMode.Text = "Game Mode:";
            // 
            // cmbGameMode
            // 
            cmbGameMode.BackColor = Color.FromArgb(40, 40, 43);
            cmbGameMode.FlatStyle = FlatStyle.Flat;
            cmbGameMode.ForeColor = Color.FromArgb(220, 220, 220);
            cmbGameMode.FormattingEnabled = true;
            cmbGameMode.Location = new Point(190, 127);
            cmbGameMode.Name = "cmbGameMode";
            cmbGameMode.Size = new Size(160, 23);
            cmbGameMode.TabIndex = 9;
            // 
            // lblMaxPlayers
            // 
            lblMaxPlayers.AutoSize = true;
            lblMaxPlayers.ForeColor = Color.FromArgb(180, 180, 180);
            lblMaxPlayers.Location = new Point(365, 109);
            lblMaxPlayers.Name = "lblMaxPlayers";
            lblMaxPlayers.Size = new Size(72, 15);
            lblMaxPlayers.TabIndex = 10;
            lblMaxPlayers.Text = "Max Players:";
            // 
            // nudMaxPlayers
            // 
            nudMaxPlayers.BackColor = Color.FromArgb(40, 40, 43);
            nudMaxPlayers.BorderStyle = BorderStyle.FixedSingle;
            nudMaxPlayers.ForeColor = Color.FromArgb(220, 220, 220);
            nudMaxPlayers.Location = new Point(365, 127);
            nudMaxPlayers.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            nudMaxPlayers.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            nudMaxPlayers.Name = "nudMaxPlayers";
            nudMaxPlayers.Size = new Size(160, 23);
            nudMaxPlayers.TabIndex = 11;
            nudMaxPlayers.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // lblMaxPing
            // 
            lblMaxPing.AutoSize = true;
            lblMaxPing.ForeColor = Color.FromArgb(180, 180, 180);
            lblMaxPing.Location = new Point(15, 162);
            lblMaxPing.Name = "lblMaxPing";
            lblMaxPing.Size = new Size(59, 15);
            lblMaxPing.TabIndex = 12;
            lblMaxPing.Text = "Max Ping:";
            // 
            // nudMaxPing
            // 
            nudMaxPing.BackColor = Color.FromArgb(40, 40, 43);
            nudMaxPing.BorderStyle = BorderStyle.FixedSingle;
            nudMaxPing.ForeColor = Color.FromArgb(220, 220, 220);
            nudMaxPing.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            nudMaxPing.Location = new Point(15, 180);
            nudMaxPing.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            nudMaxPing.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            nudMaxPing.Name = "nudMaxPing";
            nudMaxPing.Size = new Size(160, 23);
            nudMaxPing.TabIndex = 13;
            nudMaxPing.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // grpActions
            // 
            grpActions.Controls.Add(btnConnect);
            grpActions.Controls.Add(btnDisconnect);
            grpActions.Controls.Add(btnSpawn);
            grpActions.Controls.Add(btnMove);
            grpActions.Controls.Add(btnStopMove);
            grpActions.ForeColor = Color.FromArgb(180, 180, 180);
            grpActions.Location = new Point(486, 357);
            grpActions.Name = "grpActions";
            grpActions.Size = new Size(540, 117);
            grpActions.TabIndex = 7;
            grpActions.TabStop = false;
            grpActions.Text = "Bot Actions";
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
            // btnMove
            // 
            btnMove.BackColor = Color.FromArgb(60, 60, 63);
            btnMove.FlatAppearance.BorderSize = 0;
            btnMove.FlatStyle = FlatStyle.Flat;
            btnMove.ForeColor = Color.FromArgb(220, 220, 220);
            btnMove.Location = new Point(276, 66);
            btnMove.Name = "btnMove";
            btnMove.Size = new Size(122, 35);
            btnMove.TabIndex = 4;
            btnMove.Text = "Start Movement";
            btnMove.UseVisualStyleBackColor = false;
            btnMove.Click += btnStartMovement_Click;
            // 
            // btnStopMove
            // 
            btnStopMove.BackColor = Color.FromArgb(60, 60, 63);
            btnStopMove.FlatAppearance.BorderSize = 0;
            btnStopMove.FlatStyle = FlatStyle.Flat;
            btnStopMove.ForeColor = Color.FromArgb(220, 220, 220);
            btnStopMove.Location = new Point(404, 66);
            btnStopMove.Name = "btnStopMove";
            btnStopMove.Size = new Size(121, 35);
            btnStopMove.TabIndex = 5;
            btnStopMove.Text = "Stop Movement";
            btnStopMove.UseVisualStyleBackColor = false;
            btnStopMove.Click += btnStopMovement_Click;
            // 
            // grpPlayers
            // 
            grpPlayers.Controls.Add(lstPlayers);
            grpPlayers.Controls.Add(btnRefresh);
            grpPlayers.Controls.Add(chkOrbit);
            grpPlayers.Controls.Add(chkFollow);
            grpPlayers.Controls.Add(btnOrbitAll);
            grpPlayers.Controls.Add(btnFollowAll);
            grpPlayers.ForeColor = Color.FromArgb(180, 180, 180);
            grpPlayers.Location = new Point(1032, 85);
            grpPlayers.Name = "grpPlayers";
            grpPlayers.Size = new Size(260, 389);
            grpPlayers.TabIndex = 8;
            grpPlayers.TabStop = false;
            grpPlayers.Text = "Player List & Controls";
            // 
            // lstPlayers
            // 
            lstPlayers.BackColor = Color.FromArgb(40, 40, 43);
            lstPlayers.BorderStyle = BorderStyle.FixedSingle;
            lstPlayers.ForeColor = Color.FromArgb(220, 220, 220);
            lstPlayers.FormattingEnabled = true;
            lstPlayers.ItemHeight = 15;
            lstPlayers.Location = new Point(15, 25);
            lstPlayers.Name = "lstPlayers";
            lstPlayers.Size = new Size(230, 167);
            lstPlayers.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(60, 60, 63);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.FromArgb(220, 220, 220);
            btnRefresh.Location = new Point(15, 198);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(230, 26);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh Players";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefreshPlayers_Click;
            // 
            // chkOrbit
            // 
            chkOrbit.AutoSize = true;
            chkOrbit.ForeColor = Color.FromArgb(180, 180, 180);
            chkOrbit.Location = new Point(15, 235);
            chkOrbit.Name = "chkOrbit";
            chkOrbit.Size = new Size(132, 19);
            chkOrbit.TabIndex = 2;
            chkOrbit.Text = "Orbit Selected Actor";
            chkOrbit.UseVisualStyleBackColor = true;
            chkOrbit.CheckedChanged += checkBoxOrbitPlayer_CheckedChanged;
            // 
            // chkFollow
            // 
            chkFollow.AutoSize = true;
            chkFollow.ForeColor = Color.FromArgb(180, 180, 180);
            chkFollow.Location = new Point(15, 260);
            chkFollow.Name = "chkFollow";
            chkFollow.Size = new Size(140, 19);
            chkFollow.TabIndex = 3;
            chkFollow.Text = "Follow Selected Actor";
            chkFollow.UseVisualStyleBackColor = true;
            chkFollow.CheckedChanged += checkBoxFollowPlayer_CheckedChanged;
            // 
            // btnOrbitAll
            // 
            btnOrbitAll.BackColor = Color.FromArgb(255, 140, 0);
            btnOrbitAll.FlatAppearance.BorderSize = 0;
            btnOrbitAll.FlatStyle = FlatStyle.Flat;
            btnOrbitAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnOrbitAll.ForeColor = Color.White;
            btnOrbitAll.Location = new Point(15, 290);
            btnOrbitAll.Name = "btnOrbitAll";
            btnOrbitAll.Size = new Size(230, 38);
            btnOrbitAll.TabIndex = 4;
            btnOrbitAll.Text = "Orbit Actor (All)";
            btnOrbitAll.UseVisualStyleBackColor = false;
            btnOrbitAll.Click += btnOrbitAllBots_Click;
            // 
            // btnFollowAll
            // 
            btnFollowAll.BackColor = Color.FromArgb(100, 149, 237);
            btnFollowAll.FlatAppearance.BorderSize = 0;
            btnFollowAll.FlatStyle = FlatStyle.Flat;
            btnFollowAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFollowAll.ForeColor = Color.White;
            btnFollowAll.Location = new Point(15, 334);
            btnFollowAll.Name = "btnFollowAll";
            btnFollowAll.Size = new Size(230, 38);
            btnFollowAll.TabIndex = 5;
            btnFollowAll.Text = "Follow Actor (All)";
            btnFollowAll.UseVisualStyleBackColor = false;
            btnFollowAll.Click += btnFollowAllBots_Click;
            // 
            // grpAnnounce
            // 
            grpAnnounce.Controls.Add(lblAnnounceText);
            grpAnnounce.Controls.Add(txtAnnounce);
            grpAnnounce.Controls.Add(lblDuration);
            grpAnnounce.Controls.Add(nudDuration);
            grpAnnounce.Controls.Add(btnSend);
            grpAnnounce.ForeColor = Color.FromArgb(180, 180, 180);
            grpAnnounce.Location = new Point(220, 303);
            grpAnnounce.Name = "grpAnnounce";
            grpAnnounce.Size = new Size(260, 171);
            grpAnnounce.TabIndex = 9;
            grpAnnounce.TabStop = false;
            grpAnnounce.Text = "Announcements";
            // 
            // lblAnnounceText
            // 
            lblAnnounceText.AutoSize = true;
            lblAnnounceText.ForeColor = Color.FromArgb(180, 180, 180);
            lblAnnounceText.Location = new Point(15, 25);
            lblAnnounceText.Name = "lblAnnounceText";
            lblAnnounceText.Size = new Size(31, 15);
            lblAnnounceText.TabIndex = 0;
            lblAnnounceText.Text = "Text:";
            // 
            // txtAnnounce
            // 
            txtAnnounce.BackColor = Color.FromArgb(40, 40, 43);
            txtAnnounce.BorderStyle = BorderStyle.FixedSingle;
            txtAnnounce.ForeColor = Color.FromArgb(220, 220, 220);
            txtAnnounce.Location = new Point(15, 43);
            txtAnnounce.Multiline = true;
            txtAnnounce.Name = "txtAnnounce";
            txtAnnounce.PlaceholderText = "Enter announcement text...";
            txtAnnounce.Size = new Size(230, 60);
            txtAnnounce.TabIndex = 1;
            // 
            // lblDuration
            // 
            lblDuration.AutoSize = true;
            lblDuration.ForeColor = Color.FromArgb(180, 180, 180);
            lblDuration.Location = new Point(15, 110);
            lblDuration.Name = "lblDuration";
            lblDuration.Size = new Size(84, 15);
            lblDuration.TabIndex = 2;
            lblDuration.Text = "Duration (sec):";
            // 
            // nudDuration
            // 
            nudDuration.BackColor = Color.FromArgb(40, 40, 43);
            nudDuration.BorderStyle = BorderStyle.FixedSingle;
            nudDuration.DecimalPlaces = 1;
            nudDuration.ForeColor = Color.FromArgb(220, 220, 220);
            nudDuration.Location = new Point(120, 108);
            nudDuration.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            nudDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudDuration.Name = "nudDuration";
            nudDuration.Size = new Size(125, 23);
            nudDuration.TabIndex = 3;
            nudDuration.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(255, 68, 68);
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(15, 137);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(230, 22);
            btnSend.TabIndex = 4;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSendAnnouncement_Click;
            // 
            // grpEvents
            // 
            grpEvents.Controls.Add(lstEvents);
            grpEvents.Controls.Add(btnClearEvents);
            grpEvents.Controls.Add(chkAutoScroll);
            grpEvents.ForeColor = Color.FromArgb(180, 180, 180);
            grpEvents.Location = new Point(12, 480);
            grpEvents.Name = "grpEvents";
            grpEvents.Size = new Size(540, 235);
            grpEvents.TabIndex = 10;
            grpEvents.TabStop = false;
            grpEvents.Text = "Event Log";
            // 
            // lstEvents
            // 
            lstEvents.BackColor = Color.FromArgb(40, 40, 43);
            lstEvents.BorderStyle = BorderStyle.FixedSingle;
            lstEvents.Font = new Font("Consolas", 8.25F);
            lstEvents.ForeColor = Color.FromArgb(100, 255, 100);
            lstEvents.FormattingEnabled = true;
            lstEvents.ItemHeight = 13;
            lstEvents.Location = new Point(15, 25);
            lstEvents.Name = "lstEvents";
            lstEvents.Size = new Size(510, 145);
            lstEvents.TabIndex = 0;
            // 
            // btnClearEvents
            // 
            btnClearEvents.BackColor = Color.FromArgb(60, 60, 63);
            btnClearEvents.FlatAppearance.BorderSize = 0;
            btnClearEvents.FlatStyle = FlatStyle.Flat;
            btnClearEvents.ForeColor = Color.FromArgb(220, 220, 220);
            btnClearEvents.Location = new Point(425, 195);
            btnClearEvents.Name = "btnClearEvents";
            btnClearEvents.Size = new Size(100, 28);
            btnClearEvents.TabIndex = 2;
            btnClearEvents.Text = "Clear";
            btnClearEvents.UseVisualStyleBackColor = false;
            btnClearEvents.Click += btnClearLog_Click;
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.Checked = true;
            chkAutoScroll.CheckState = CheckState.Checked;
            chkAutoScroll.ForeColor = Color.FromArgb(180, 180, 180);
            chkAutoScroll.Location = new Point(15, 181);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new Size(85, 19);
            chkAutoScroll.TabIndex = 1;
            chkAutoScroll.Text = "Auto-scroll";
            chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // grpConsole
            // 
            grpConsole.Controls.Add(txtConsole);
            grpConsole.Controls.Add(btnClearConsole);
            grpConsole.ForeColor = Color.FromArgb(180, 180, 180);
            grpConsole.Location = new Point(558, 480);
            grpConsole.Name = "grpConsole";
            grpConsole.Size = new Size(734, 235);
            grpConsole.TabIndex = 11;
            grpConsole.TabStop = false;
            grpConsole.Text = "Console Output";
            // 
            // txtConsole
            // 
            txtConsole.BackColor = Color.FromArgb(40, 40, 43);
            txtConsole.BorderStyle = BorderStyle.FixedSingle;
            txtConsole.Font = new Font("Consolas", 8.25F);
            txtConsole.ForeColor = Color.FromArgb(220, 220, 220);
            txtConsole.Location = new Point(15, 25);
            txtConsole.Multiline = true;
            txtConsole.Name = "txtConsole";
            txtConsole.ReadOnly = true;
            txtConsole.ScrollBars = ScrollBars.Vertical;
            txtConsole.Size = new Size(704, 145);
            txtConsole.TabIndex = 0;
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
            // groupBox1
            // 
            groupBox1.Controls.Add(lstBots);
            groupBox1.Controls.Add(btnAddBot);
            groupBox1.Controls.Add(btnRemoveBot);
            groupBox1.ForeColor = Color.FromArgb(180, 180, 180);
            groupBox1.Location = new Point(12, 9);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 465);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Bot Instances";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 33);
            ClientSize = new Size(1304, 723);
            Controls.Add(groupBox1);
            Controls.Add(grpConsole);
            Controls.Add(grpEvents);
            Controls.Add(grpAnnounce);
            Controls.Add(grpPlayers);
            Controls.Add(grpActions);
            Controls.Add(grpRooms);
            Controls.Add(grpBotConfig);
            Controls.Add(grpStatus);
            Controls.Add(lblBots);
            Name = "Form1";
            Text = "Odium Bot Manager - ULTIMATE EDITION";
            Load += Form1_Load;
            grpStatus.ResumeLayout(false);
            grpStatus.PerformLayout();
            grpBotConfig.ResumeLayout(false);
            grpBotConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRank).EndInit();
            grpRooms.ResumeLayout(false);
            grpRooms.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMaxPlayers).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxPing).EndInit();
            grpActions.ResumeLayout(false);
            grpPlayers.ResumeLayout(false);
            grpPlayers.PerformLayout();
            grpAnnounce.ResumeLayout(false);
            grpAnnounce.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudDuration).EndInit();
            grpEvents.ResumeLayout(false);
            grpEvents.PerformLayout();
            grpConsole.ResumeLayout(false);
            grpConsole.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstBots;
        private Label lblBots;
        private Button btnAddBot;
        private Button btnRemoveBot;

        private GroupBox grpStatus;
        private Label lblConnStatus;
        private Label lblConnStatusVal;
        private Label lblPing;
        private Label lblPingVal;
        private Label lblCurrentRoom;
        private Label lblRoomVal;
        private Label lblPlayerCount;
        private Label lblPlayerCountVal;
        private Label lblMasterClient;
        private Label lblMasterVal;

        private GroupBox grpBotConfig;
        private Label lblName;
        private TextBox txtBotName;
        private Label lblClan;
        private TextBox txtClanTag;
        private Label lblTeam;
        private ComboBox cmbTeam;
        private Label lblRank;
        private NumericUpDown nudRank;

        private GroupBox grpRooms;
        private Button btnJoinRandom;
        private Button btnJoinRandomAll;
        private Button btnCreateRoom;
        private Button btnLeave;
        private Button btnLeaveAll;
        private Label lblRoomName;
        private TextBox txtRoomName;
        private Button btnJoin;
        private Button btnJoinAll;
        private Label lblMap;
        private ComboBox cmbMap;
        private Label lblMode;
        private ComboBox cmbGameMode;
        private Label lblMaxPlayers;
        private NumericUpDown nudMaxPlayers;
        private Label lblMaxPing;
        private NumericUpDown nudMaxPing;

        private GroupBox grpActions;
        private Button btnConnect;
        private Button btnDisconnect;
        private Button btnSpawn;
        private Button btnMove;
        private Button btnStopMove;

        private GroupBox grpPlayers;
        private ListBox lstPlayers;
        private Button btnRefresh;
        private CheckBox chkOrbit;
        private CheckBox chkFollow;
        private Button btnOrbitAll;
        private Button btnFollowAll;

        private GroupBox grpAnnounce;
        private Label lblAnnounceText;
        private TextBox txtAnnounce;
        private Label lblDuration;
        private NumericUpDown nudDuration;
        private Button btnSend;

        private GroupBox grpEvents;
        private ListBox lstEvents;
        private Button btnClearEvents;
        private CheckBox chkAutoScroll;

        private GroupBox grpConsole;
        private TextBox txtConsole;
        private Button btnClearConsole;
        private GroupBox groupBox1;
    }
}