using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BFBot.Core;
using Photon.Realtime;

namespace BLF_Odium_Network_Bots
{
    public partial class Form1 : Form
    {
        private List<BFPhotonClient> botClients = new List<BFPhotonClient>();
        private BFPhotonClient selectedBot = null;
        private System.Windows.Forms.Timer updateTimer;

        public Form1()
        {
            InitializeComponent();

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 500;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxTeam.SelectedIndex = 0;
            comboBoxMap.Items.AddRange(new string[] { "Outpost", "Tundra", "Desert", "Urban", "Forest" });
            comboBoxMap.SelectedIndex = 0;
            comboBoxGameMode.Items.AddRange(new string[] { "Team Deathmatch", "Free For All", "Domination", "Gun Game" });
            comboBoxGameMode.SelectedIndex = 0;
            LogConsole("Bot Manager initialized. Add a bot to get started.");
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (selectedBot != null)
            {
                UpdateStatusDisplay();
                UpdateEventLog();
            }
        }

        private void UpdateStatusDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatusDisplay));
                return;
            }

            if (selectedBot == null) return;

            labelStatusValue.Text = selectedBot.State.ToString();
            labelPingValue.Text = selectedBot.LoadBalancingPeer != null ?
                $"{selectedBot.LoadBalancingPeer.RoundTripTime}ms" : "N/A";

            if (selectedBot.CurrentRoom != null)
            {
                labelRoomValue.Text = selectedBot.CurrentRoom.Name;
                labelPlayersValue.Text = $"{selectedBot.CurrentRoom.PlayerCount}/{selectedBot.CurrentRoom.MaxPlayers}";
                labelMasterValue.Text = selectedBot.CurrentRoom.MasterClientId.ToString();
            }
            else
            {
                labelRoomValue.Text = "N/A";
                labelPlayersValue.Text = "N/A";
                labelMasterValue.Text = "N/A";
            }
        }

        private void UpdateEventLog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateEventLog));
                return;
            }

            if (selectedBot == null || selectedBot.eventLogs == null) return;

            int displayCount = Math.Min(20, selectedBot.eventLogs.Count);
            if (listBoxEventLog.Items.Count != displayCount)
            {
                listBoxEventLog.Items.Clear();
                for (int i = 0; i < displayCount; i++)
                {
                    listBoxEventLog.Items.Add(selectedBot.eventLogs[i]);
                }

                if (checkBoxAutoScroll.Checked && listBoxEventLog.Items.Count > 0)
                {
                    listBoxEventLog.TopIndex = listBoxEventLog.Items.Count - 1;
                }
            }
        }

        public void LogConsole(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogConsole), message);
                return;
            }

            textBoxConsoleLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
        }

        private void btnAddBot_Click(object sender, EventArgs e)
        {
            try
            {
                var newBot = new BFPhotonClient();
                botClients.Add(newBot);
                listBoxBots.Items.Add($"Bot #{botClients.Count} - Connecting...");
                LogConsole($"Added Bot #{botClients.Count}");
            }
            catch (Exception ex)
            {
                LogConsole($"Error adding bot: {ex.Message}");
                MessageBox.Show($"Failed to add bot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveBot_Click(object sender, EventArgs e)
        {
            if (listBoxBots.SelectedIndex >= 0 && listBoxBots.SelectedIndex < botClients.Count)
            {
                int index = listBoxBots.SelectedIndex;
                var bot = botClients[index];

                bot.Disconnect();
                botClients.RemoveAt(index);
                listBoxBots.Items.RemoveAt(index);

                if (selectedBot == bot)
                {
                    selectedBot = null;
                }

                LogConsole($"Removed Bot #{index + 1}");
            }
        }

        private void listBoxBots_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxBots.SelectedIndex >= 0 && listBoxBots.SelectedIndex < botClients.Count)
            {
                selectedBot = botClients[listBoxBots.SelectedIndex];
                LogConsole($"Selected Bot #{listBoxBots.SelectedIndex + 1}");
                RefreshPlayerList();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogConsole("Connect button clicked - bot automatically connects on creation");
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBot.Disconnect();
            LogConsole("Disconnecting bot...");
        }

        private void btnSpawn_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogConsole("Bot spawn is handled automatically on room join");
        }

        private void btnStartMovement_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBot.StartMovement();
            LogConsole("Started bot movement");
        }

        private void btnStopMovement_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBot.StopMovement();
            selectedBot.StopOrbiting();
            selectedBot.StopFollowing();
            checkBoxOrbitPlayer.Checked = false;
            checkBoxFollowPlayer.Checked = false;
            LogConsole("Stopped bot movement");
        }

        private void btnJoinRandom_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBot.JoinRandomRoom();
            LogConsole("Attempting to join random room...");
        }

        private void btnJoinRandomAll_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var bot in botClients)
            {
                if (bot.State == ClientState.ConnectedToMasterServer || bot.State == ClientState.JoinedLobby)
                {
                    bot.JoinRandomRoom();
                }
            }

            LogConsole($"Sending {botClients.Count} bots to join random rooms...");
        }

        private void btnJoinByName_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxRoomName.Text))
            {
                MessageBox.Show("Please enter a room name!", "No Room Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBot.JoinRoomByName(textBoxRoomName.Text);
            LogConsole($"Attempting to join room: {textBoxRoomName.Text}");
        }

        private void btnJoinByNameAll_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxRoomName.Text))
            {
                MessageBox.Show("Please enter a room name!", "No Room Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var bot in botClients)
            {
                if (bot.State == ClientState.ConnectedToMasterServer || bot.State == ClientState.JoinedLobby)
                {
                    bot.JoinRoomByName(textBoxRoomName.Text);
                }
            }

            LogConsole($"Sending all {botClients.Count} bots to room: {textBoxRoomName.Text}");
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roomName = string.IsNullOrWhiteSpace(textBoxRoomName.Text) ? null : textBoxRoomName.Text;
            string mapName = comboBoxMap.SelectedItem?.ToString() ?? "Outpost";
            string modeName = comboBoxGameMode.SelectedItem?.ToString() ?? "Team Deathmatch";
            int maxPlayers = (int)numericMaxPlayers.Value;
            int maxPing = (int)numericMaxPing.Value;

            selectedBot.CreateRoom(roomName, mapName, modeName, maxPlayers, maxPing);
            LogConsole($"Creating room: {roomName ?? "Auto"} | Map: {mapName} | Mode: {modeName} | Players: {maxPlayers} | Ping: {maxPing}");
        }

        private void btnLeaveRoom_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedBot.OpLeaveRoom(false);
            LogConsole("Leaving room...");
        }

        private void btnLeaveRoomAll_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int leftCount = 0;
            foreach (var bot in botClients)
            {
                if (bot.CurrentRoom != null)
                {
                    bot.OpLeaveRoom(false);
                    leftCount++;
                }
            }

            LogConsole($"Leaving rooms for {leftCount} bots...");
        }

        private void btnDisconnectAll_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var bot in botClients)
            {
                bot.Disconnect();
            }

            LogConsole($"Disconnecting all {botClients.Count} bots...");
        }

        private void btnRefreshPlayers_Click(object sender, EventArgs e)
        {
            RefreshPlayerList();
        }

        private void RefreshPlayerList()
        {
            listBoxPlayers.Items.Clear();

            if (selectedBot == null || selectedBot.CurrentRoom == null)
            {
                listBoxPlayers.Items.Add("Not in a room");
                return;
            }

            var players = selectedBot.GetPlayerList();
            foreach (var player in players)
            {
                string displayName = $"[{player.ActorNumber}] {player.NickName ?? "Unknown"}";
                listBoxPlayers.Items.Add(displayName);
            }

            LogConsole($"Refreshed player list: {players.Count} players");
        }

        private void checkBoxOrbitPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                checkBoxOrbitPlayer.Checked = false;
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (checkBoxOrbitPlayer.Checked)
            {
                if (listBoxPlayers.SelectedIndex < 0)
                {
                    checkBoxOrbitPlayer.Checked = false;
                    MessageBox.Show("Please select a player to orbit!", "No Player Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedItem = listBoxPlayers.SelectedItem.ToString();
                int startIdx = selectedItem.IndexOf('[') + 1;
                int endIdx = selectedItem.IndexOf(']');

                if (int.TryParse(selectedItem.Substring(startIdx, endIdx - startIdx), out int actorNumber))
                {
                    selectedBot.StartOrbitingPlayer(actorNumber);
                    checkBoxFollowPlayer.Checked = false;
                    LogConsole($"Started orbiting player {actorNumber}");
                }
                else
                {
                    checkBoxOrbitPlayer.Checked = false;
                    MessageBox.Show("Failed to parse player ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                selectedBot.StopOrbiting();
                LogConsole("Stopped orbiting");
            }
        }

        private void checkBoxFollowPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                checkBoxFollowPlayer.Checked = false;
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (checkBoxFollowPlayer.Checked)
            {
                if (listBoxPlayers.SelectedIndex < 0)
                {
                    checkBoxFollowPlayer.Checked = false;
                    MessageBox.Show("Please select a player to follow!", "No Player Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedItem = listBoxPlayers.SelectedItem.ToString();
                int startIdx = selectedItem.IndexOf('[') + 1;
                int endIdx = selectedItem.IndexOf(']');

                if (int.TryParse(selectedItem.Substring(startIdx, endIdx - startIdx), out int actorNumber))
                {
                    selectedBot.StartFollowingPlayer(actorNumber);
                    checkBoxOrbitPlayer.Checked = false;
                    LogConsole($"Started following player {actorNumber}");
                }
                else
                {
                    checkBoxFollowPlayer.Checked = false;
                    MessageBox.Show("Failed to parse player ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                selectedBot.StopFollowing();
                LogConsole("Stopped following");
            }
        }

        private void btnOrbitAllBots_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxPlayers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a player to orbit!", "No Player Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = listBoxPlayers.SelectedItem.ToString();
            int startIdx = selectedItem.IndexOf('[') + 1;
            int endIdx = selectedItem.IndexOf(']');

            if (int.TryParse(selectedItem.Substring(startIdx, endIdx - startIdx), out int actorNumber))
            {
                float angleOffset = 0f;
                float angleIncrement = (float)(2 * Math.PI / botClients.Count);

                foreach (var bot in botClients)
                {
                    if (bot.CurrentRoom != null)
                    {
                        bot.StartOrbitingPlayer(actorNumber, angleOffset);
                        angleOffset += angleIncrement;
                    }
                }

                LogConsole($"All bots orbiting player {actorNumber} with offsets");
            }
            else
            {
                MessageBox.Show("Failed to parse player ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFollowAllBots_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxPlayers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a player to follow!", "No Player Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = listBoxPlayers.SelectedItem.ToString();
            int startIdx = selectedItem.IndexOf('[') + 1;
            int endIdx = selectedItem.IndexOf(']');

            if (int.TryParse(selectedItem.Substring(startIdx, endIdx - startIdx), out int actorNumber))
            {
                foreach (var bot in botClients)
                {
                    if (bot.CurrentRoom != null)
                    {
                        bot.StartFollowingPlayer(actorNumber);
                    }
                }

                LogConsole($"All bots following player {actorNumber}");
            }
            else
            {
                MessageBox.Show("Failed to parse player ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSendAnnouncement_Click(object sender, EventArgs e)
        {
            if (selectedBot == null)
            {
                MessageBox.Show("Please select a bot first!", "No Bot Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxAnnouncement.Text))
            {
                MessageBox.Show("Please enter announcement text!", "No Text", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float duration = (float)numericDuration.Value;
            selectedBot.SendAnnouncement(textBoxAnnouncement.Text, duration);
            LogConsole($"Sent announcement: \"{textBoxAnnouncement.Text}\" ({duration}s)");
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            listBoxEventLog.Items.Clear();
            if (selectedBot != null)
            {
                selectedBot.eventLogs.Clear();
            }
            LogConsole("Event log cleared");
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            textBoxConsoleLog.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (botClients.Count == 0)
            {
                MessageBox.Show("No bots available!", "No Bots", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int leftCount = 0;
            foreach (var bot in botClients)
            {
                if (bot.CurrentRoom != null)
                {
                    bot.OpLeaveRoom(false);
                    leftCount++;
                }
            }

            LogConsole($"Leaving rooms for {leftCount} bots...");
        }
    }
}