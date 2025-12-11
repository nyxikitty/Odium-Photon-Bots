using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OdiumPhoton.Core;
using Photon.Realtime;
using static OdiumPhoton.Core.PhotonBot;
using BLF_Odium_Network_Bots.Photon;

namespace BLF_Odium_Network_Bots
{
    public partial class Form1 : Form
    {
        #region Fields
        private readonly List<PhotonBot> _bots;
        private readonly System.Windows.Forms.Timer _refreshTimer;
        private PhotonBot _selected;
        #endregion

        #region Initialization
        public Form1()
        {
            InitializeComponent();

            _bots = new List<PhotonBot>();
            _refreshTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _refreshTimer.Tick += OnRefreshTick;
            _refreshTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeControls();
            Log("Bot Manager ready");
        }

        private void InitializeControls()
        {
            cmbTeam.SelectedIndex = 0;

            cmbMap.Items.AddRange(new[] { "Outpost", "Woods", "Park", "Urban", "Village", "SwampFox" });
            cmbMap.SelectedIndex = 0;

            cmbGameMode.Items.AddRange(new[] { "Team Deathmatch", "Free For All", "VIP", "Gun Game" });
            cmbGameMode.SelectedIndex = 0;

            // Initialize combat target player dropdown
            cmbTargetPlayer.Items.Clear();
        }
        #endregion

        #region UI Updates
        private void OnRefreshTick(object sender, EventArgs e)
        {
            if (_selected != null)
            {
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatus));
                return;
            }

            lblConnStatusVal.Text = _selected.State.ToString();
            lblPingVal.Text = _selected.LoadBalancingPeer != null
                ? $"{_selected.LoadBalancingPeer.RoundTripTime}ms"
                : "N/A";

            if (_selected.CurrentRoom != null)
            {
                var room = _selected.CurrentRoom;
                lblRoomVal.Text = room.Name;
                lblPlayerCountVal.Text = $"{room.PlayerCount}/{room.MaxPlayers}";
                lblMasterVal.Text = room.MasterClientId.ToString();
            }
            else
            {
                lblRoomVal.Text = "N/A";
                lblPlayerCountVal.Text = "N/A";
                lblMasterVal.Text = "N/A";
            }
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(Log), message);
                return;
            }

            txtConsole.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
        }
        #endregion

        #region Bot Management
        private void btnAddBot_Click(object sender, EventArgs e)
        {
            try
            {
                var bot = new PhotonBot("8c2cad3e-2e3f-4941-9044-b390ff2c4956", "1.84.0_1.99");
                _bots.Add(bot);
                lstBots.Items.Add($"Bot #{_bots.Count}");
                Log($"Bot #{_bots.Count} added");
            }
            catch (Exception ex)
            {
                Log($"Failed to add bot: {ex.Message}");
                ShowError("Failed to add bot", ex.Message);
            }
        }

        private void btnRemoveBot_Click(object sender, EventArgs e)
        {
            int index = lstBots.SelectedIndex;
            if (index < 0 || index >= _bots.Count) return;

            var bot = _bots[index];
            bot.Disconnect();

            _bots.RemoveAt(index);
            lstBots.Items.RemoveAt(index);

            if (_selected == bot)
                _selected = null;

            Log($"Bot #{index + 1} removed");
        }

        private void listBoxBots_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstBots.SelectedIndex;
            if (index >= 0 && index < _bots.Count)
            {
                _selected = _bots[index];
                RefreshPlayerList();
                RefreshTargetPlayerList();
            }
        }
        #endregion

        #region Room Operations
        private void btnJoinRandom_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.JoinRandom();
            Log("Joining random room");
        }

        private void btnJoinRandomAll_Click(object sender, EventArgs e)
        {
            if (_bots.Count == 0)
            {
                ShowWarning("No bots available");
                return;
            }

            int count = 0;
            foreach (var bot in GetReadyBots())
            {
                bot.JoinRandom();
                count++;
            }

            Log($"{count} bots joining random rooms");
        }

        private void btnJoinByName_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;
            if (!ValidateRoomName()) return;

            _selected.JoinRoom(txtRoomName.Text);
            Log($"Joining room: {txtRoomName.Text}");
        }

        private void btnJoinByNameAll_Click(object sender, EventArgs e)
        {
            if (_bots.Count == 0)
            {
                ShowWarning("No bots available");
                return;
            }
            if (!ValidateRoomName()) return;

            string roomName = txtRoomName.Text;
            int count = 0;

            foreach (var bot in GetReadyBots())
            {
                bot.JoinRoom(roomName);
                count++;
            }

            Log($"{count} bots joining: {roomName}");
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            string roomName = string.IsNullOrWhiteSpace(txtRoomName.Text)
                ? null
                : txtRoomName.Text;

            string map = cmbMap.SelectedItem?.ToString() ?? "Outpost";
            string mode = cmbGameMode.SelectedItem?.ToString() ?? "Team Deathmatch";
            byte maxPlayers = (byte)nudMaxPlayers.Value;

            _selected.CreateRoom(roomName, map, mode, maxPlayers);
            Log($"Creating room: {map} - {mode}");
        }

        private void btnLeaveRoom_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.LeaveRoom();
            Log("Leaving room");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (var bot in _bots.Where(b => b.CurrentRoom != null))
            {
                bot.LeaveRoom();
                count++;
            }

            if (count > 0)
                Log($"{count} bots leaving rooms");
        }
        #endregion

        #region Movement Control
        private void btnStartMovement_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.IsActive = true;
            Log("Movement started");
        }

        private void btnStopMovement_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.StopMovement();
            chkOrbit.Checked = false;
            chkFollow.Checked = false;
            Log("Movement stopped");
        }

        private void checkBoxOrbitPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (!ValidateSelection())
            {
                chkOrbit.Checked = false;
                return;
            }

            if (chkOrbit.Checked)
            {
                if (!TryGetSelectedActor(out int actor))
                {
                    chkOrbit.Checked = false;
                    ShowWarning("Select a player to orbit");
                    return;
                }

                _selected.SetMovement(MovementMode.Orbit, actor);
                chkFollow.Checked = false;
                Log($"Orbiting player {actor}");
            }
            else
            {
                _selected.StopMovement();
            }
        }

        private void checkBoxFollowPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (!ValidateSelection())
            {
                chkFollow.Checked = false;
                return;
            }

            if (chkFollow.Checked)
            {
                if (!TryGetSelectedActor(out int actor))
                {
                    chkFollow.Checked = false;
                    ShowWarning("Select a player to follow");
                    return;
                }

                _selected.SetMovement(MovementMode.Follow, actor);
                chkOrbit.Checked = false;
                Log($"Following player {actor}");
            }
            else
            {
                _selected.StopMovement();
            }
        }

        private void btnOrbitAllBots_Click(object sender, EventArgs e)
        {
            if (_bots.Count == 0 || !TryGetSelectedActor(out int actor))
            {
                ShowWarning("Select a player first");
                return;
            }

            float offset = 0f;
            float increment = (float)(2 * Math.PI / _bots.Count);

            foreach (var bot in _bots.Where(b => b.CurrentRoom != null))
            {
                bot.SetMovement(MovementMode.Orbit, actor, offset);
                offset += increment;
            }

            Log($"All bots orbiting player {actor}");
        }

        private void btnFollowAllBots_Click(object sender, EventArgs e)
        {
            if (_bots.Count == 0 || !TryGetSelectedActor(out int actor))
            {
                ShowWarning("Select a player first");
                return;
            }

            foreach (var bot in _bots.Where(b => b.CurrentRoom != null))
            {
                bot.SetMovement(MovementMode.Follow, actor);
            }

            Log($"All bots following player {actor}");
        }
        #endregion

        #region Combat Actions
        private void btnDamagePlayer_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            if (cmbTargetPlayer.SelectedIndex < 0)
            {
                ShowWarning("Select a target player");
                return;
            }

            if (!TryGetTargetActor(out int targetActor))
            {
                ShowWarning("Invalid target player");
                return;
            }

            float damage = (float)nudDamage.Value;
            bool headshot = chkHeadshot.Checked;

            // Send damage RPC
            _selected.RPC.AcknowledgeDamageDoneRPC("hit", damage, targetActor);
            _selected.RPC.RpcPlayerHitPlayerHitmarker(_selected.LocalPlayer.ActorNumber, targetActor, 14, headshot);

            Log($"Damaged player {targetActor} for {damage} HP{(headshot ? " (HEADSHOT)" : "")}");
        }

        private void btnShowHitmarker_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.RpcShowHitmarker();
            Log("Hitmarker shown");
        }

        private void btnMeleeAttack_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.MpMeleeAnimation();
            Log("Melee attack animation played");
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.LocalReload();
            Log("Reload animation played");
        }

        private void btnThrowGrenade_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.MpThrowGrenadeAnimation();
            Log("Grenade throw animation played");
        }
        #endregion

        #region Killstreaks
        private void btnActivateKillstreak_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            if (cmbKillstreak.SelectedIndex < 0)
            {
                ShowWarning("Select a killstreak");
                return;
            }

            int killstreakID = cmbKillstreak.SelectedIndex + 1; // 1-6
            bool sameTeam = chkSameTeam.Checked;

            _selected.RPC.RpcForceKillstreak(killstreakID, sameTeam);
            Log($"Activated killstreak: {cmbKillstreak.SelectedItem} (Team: {sameTeam})");
        }

        private void btnNuke_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var result = MessageBox.Show(
                "Are you sure you want to activate a NUKE? This will affect all players!",
                "Confirm Nuke",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _selected.RPC.NukeKill();
                Log("🔥 NUKE ACTIVATED! 🔥");
            }
        }
        #endregion

        #region Game Control
        private void btnFlash_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.Flash();
            Log("Flash effect triggered");
        }

        private void btnTeleport_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            // Teleport to a random position
            var random = new Random();
            var position = new Vec3(
                (float)(random.NextDouble() * 20 - 10),
                1.75f,
                (float)(random.NextDouble() * 20 - 10)
            );

            _selected.RPC.TeleportToPosition(position);
            Log($"Teleported to ({position.x:F2}, {position.y:F2}, {position.z:F2})");
        }

        private void btnSetTimeScale_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            float timeScale = (float)nudTimeScale.Value;
            _selected.RPC.SetTimeScale(timeScale);
            Log($"Time scale set to {timeScale}x");
        }

        private void btnMatchOver_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            bool matchOver = chkMatchOver.Checked;
            _selected.RPC.MatchOverChanged(matchOver);
            Log($"Match over state: {matchOver}");
        }
        #endregion

        #region Chat
        private void btnSendChat_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            string message = txtChatMessage.Text?.Trim();
            if (string.IsNullOrEmpty(message))
            {
                ShowWarning("Enter a chat message");
                return;
            }

            short r = (short)nudColorR.Value;
            short g = (short)nudColorG.Value;
            short b = (short)nudColorB.Value;

            _selected.RPC.RpcSendChatMessage($"[BOT] {_selected.BotName}", message, r, g, b);
            Log($"Chat sent: {message} (RGB: {r},{g},{b})");
        }
        #endregion

        #region Player List
        private void btnRefreshPlayers_Click(object sender, EventArgs e)
        {
            RefreshPlayerList();
            RefreshTargetPlayerList();
        }

        private void RefreshPlayerList()
        {
            lstPlayers.Items.Clear();

            if (_selected?.CurrentRoom == null)
            {
                lstPlayers.Items.Add("Not in a room");
                return;
            }

            var players = _selected.GetPlayers();
            foreach (var player in players)
            {
                string name = CleanName(player.NickName ?? "Unknown");
                lstPlayers.Items.Add($"[{player.ActorNumber}] {name}");
            }
        }

        private void RefreshTargetPlayerList()
        {
            cmbTargetPlayer.Items.Clear();

            if (_selected?.CurrentRoom == null)
                return;

            var players = _selected.GetPlayers();
            foreach (var player in players)
            {
                if (player.ActorNumber == _selected.LocalPlayer.ActorNumber)
                    continue; // Skip self

                string name = CleanName(player.NickName ?? "Unknown");
                cmbTargetPlayer.Items.Add($"[{player.ActorNumber}] {name}");
            }

            if (cmbTargetPlayer.Items.Count > 0)
                cmbTargetPlayer.SelectedIndex = 0;
        }

        private bool TryGetSelectedActor(out int actorNumber)
        {
            actorNumber = -1;

            if (lstPlayers.SelectedIndex < 0)
                return false;

            string text = lstPlayers.SelectedItem?.ToString();
            if (text == null) return false;

            int start = text.IndexOf('[') + 1;
            int end = text.IndexOf(']');

            if (start > 0 && end > start)
            {
                string idStr = text.Substring(start, end - start);
                return int.TryParse(idStr, out actorNumber);
            }

            return false;
        }

        private bool TryGetTargetActor(out int actorNumber)
        {
            actorNumber = -1;

            if (cmbTargetPlayer.SelectedIndex < 0)
                return false;

            string text = cmbTargetPlayer.SelectedItem?.ToString();
            if (text == null) return false;

            int start = text.IndexOf('[') + 1;
            int end = text.IndexOf(']');

            if (start > 0 && end > start)
            {
                string idStr = text.Substring(start, end - start);
                return int.TryParse(idStr, out actorNumber);
            }

            return false;
        }
        #endregion

        #region Messaging
        private void btnSendAnnouncement_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            string text = txtAnnounce.Text?.Trim();
            if (string.IsNullOrEmpty(text))
            {
                ShowWarning("Enter announcement text");
                return;
            }

            float duration = (float)nudDuration.Value;
            _selected.SendAnnouncement(text, duration);
            Log($"Announcement sent: {text}");
        }
        #endregion

        #region Utility
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lstEvents.Items.Clear();
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            txtConsole.Clear();
        }

        private bool ValidateSelection()
        {
            if (_selected == null)
            {
                ShowWarning("Select a bot first");
                return false;
            }
            return true;
        }

        private bool ValidateSpawned()
        {
            if (_selected.RPC == null)
            {
                ShowWarning("Bot must be spawned in a room first");
                return false;
            }
            return true;
        }

        private bool ValidateRoomName()
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text))
            {
                ShowWarning("Enter a room name");
                return false;
            }
            return true;
        }

        private IEnumerable<PhotonBot> GetReadyBots()
        {
            return _bots.Where(b =>
                b.State == ClientState.ConnectedToMasterServer ||
                b.State == ClientState.JoinedLobby);
        }

        private static string CleanName(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, "</?color[^>]*>", "");
        }

        private static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Legacy Event Handlers
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Connection is automatic
            Log("Connection is handled automatically when bots are added");
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;
            _selected.Disconnect();
            Log("Disconnecting");
        }

        private void btnSpawn_Click(object sender, EventArgs e)
        {
            // Spawning is automatic
            Log("Spawning is handled automatically by the bot system");
        }
        #endregion
    }
}