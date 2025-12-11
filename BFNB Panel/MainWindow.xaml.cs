using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using OdiumPhoton.Core;
using Photon.Realtime;
using static OdiumPhoton.Core.PhotonBot;

namespace OdiumBotManager
{
    public partial class MainWindow : Window
    {
        #region Fields
        private readonly List<PhotonBot> _bots;
        private readonly DispatcherTimer _refreshTimer;
        private PhotonBot _selected;
        #endregion

        #region Initialization
        public MainWindow()
        {
            InitializeComponent();

            _bots = new List<PhotonBot>();
            _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _refreshTimer.Tick += OnRefreshTick;
            _refreshTimer.Start();

            Log("Bot Manager ready");
        }

        private void OnRefreshTick(object sender, EventArgs e)
        {
            if (_selected != null)
            {
                UpdateStatus();
            }
        }
        #endregion

        #region UI Updates
        private void UpdateStatus()
        {
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
            Dispatcher.Invoke(() =>
            {
                txtConsole.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                if (chkAutoScroll.IsChecked == true)
                {
                    txtConsole.ScrollToEnd();
                }
            });
        }
        #endregion

        #region Bot Management
        private void btnAddBot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get configuration values from UI
                string botName = txtBotName.Text?.Trim();
                if (string.IsNullOrWhiteSpace(botName))
                    botName = $"PC-Bot_{new Random().Next(1000, 9999)}";

                string clanTag = txtClanTag.Text?.Trim();
                if (string.IsNullOrWhiteSpace(clanTag))
                    clanTag = "[BOT]";

                byte team = (byte)(cmbTeam.SelectedIndex + 1);
                if (!byte.TryParse(nudRank.Text, out byte rank))
                    rank = 200;

                var bot = new PhotonBot("8c2cad3e-2e3f-4941-9044-b390ff2c4956", "1.84.0_1.99");

                // Apply configuration
                bot.BotName = botName;
                bot.ClanTag = clanTag;
                bot.Team = team;
                bot.Rank = rank;

                _bots.Add(bot);
                lstBots.Items.Add($"{clanTag} {botName}");
                Log($"Bot added: {clanTag} {botName} (Team {team}, Rank {rank})");
            }
            catch (Exception ex)
            {
                Log($"Failed to add bot: {ex.Message}");
                MessageBox.Show($"Failed to add bot: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRemoveBot_Click(object sender, RoutedEventArgs e)
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

        private void lstBots_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        private void btnJoinRandom_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.JoinRandom();
            Log("Joining random room");
        }

        private void btnJoinRandomAll_Click(object sender, RoutedEventArgs e)
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

        private void btnJoinByName_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;
            if (!ValidateRoomName()) return;

            _selected.JoinRoom(txtRoomName.Text);
            Log($"Joining room: {txtRoomName.Text}");
        }

        private void btnJoinByNameAll_Click(object sender, RoutedEventArgs e)
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

        private void btnCreateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            string roomName = string.IsNullOrWhiteSpace(txtRoomName.Text)
                ? null
                : txtRoomName.Text;

            string map = (cmbMap.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Outpost";
            string mode = (cmbGameMode.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Team Deathmatch";

            if (!byte.TryParse(nudMaxPlayers.Text, out byte maxPlayers))
                maxPlayers = 10;

            _selected.CreateRoom(roomName, map, mode, maxPlayers);
            Log($"Creating room: {map} - {mode}");
        }

        private void btnLeaveRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.LeaveRoom();
            Log("Leaving room");
        }

        private void btnLeaveAll_Click(object sender, RoutedEventArgs e)
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
        private void btnStartMovement_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.IsActive = true;
            Log("Movement started");
        }

        private void btnStopMovement_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            _selected.StopMovement();
            chkOrbit.IsChecked = false;
            chkFollow.IsChecked = false;
            Log("Movement stopped");
        }

        private void chkOrbit_Checked(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection())
            {
                chkOrbit.IsChecked = false;
                return;
            }

            if (!TryGetSelectedActor(out int actor))
            {
                chkOrbit.IsChecked = false;
                ShowWarning("Select a player to orbit");
                return;
            }

            _selected.SetMovement(MovementMode.Orbit, actor);
            chkFollow.IsChecked = false;
            Log($"Orbiting player {actor}");
        }

        private void chkOrbit_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
                _selected.StopMovement();
        }

        private void chkFollow_Checked(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection())
            {
                chkFollow.IsChecked = false;
                return;
            }

            if (!TryGetSelectedActor(out int actor))
            {
                chkFollow.IsChecked = false;
                ShowWarning("Select a player to follow");
                return;
            }

            _selected.SetMovement(MovementMode.Follow, actor);
            chkOrbit.IsChecked = false;
            Log($"Following player {actor}");
        }

        private void chkFollow_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
                _selected.StopMovement();
        }

        private void btnOrbitAll_Click(object sender, RoutedEventArgs e)
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

        private void btnFollowAll_Click(object sender, RoutedEventArgs e)
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
        private void btnDamagePlayer_Click(object sender, RoutedEventArgs e)
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

            if (!float.TryParse(nudDamage.Text, out float damage))
                damage = 50f;

            bool headshot = chkHeadshot.IsChecked == true;

            _selected.RPC.AcknowledgeDamageDoneRPC("hit", damage, targetActor);
            _selected.RPC.RpcPlayerHitPlayerHitmarker(_selected.LocalPlayer.ActorNumber, targetActor, 14, headshot);

            Log($"Damaged player {targetActor} for {damage} HP{(headshot ? " (HEADSHOT)" : "")}");
        }

        private void btnShowHitmarker_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.RpcShowHitmarker();
            Log("Hitmarker shown");
        }

        private void btnMeleeAttack_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.MpMeleeAnimation();
            Log("Melee attack animation played");
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.LocalReload();
            Log("Reload animation played");
        }

        private void btnThrowGrenade_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.MpThrowGrenadeAnimation();
            Log("Grenade throw animation played");
        }
        #endregion

        #region Killstreaks
        private void btnActivateKillstreak_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            if (cmbKillstreak.SelectedIndex < 0)
            {
                ShowWarning("Select a killstreak");
                return;
            }

            int killstreakID = cmbKillstreak.SelectedIndex + 1;
            bool sameTeam = chkSameTeam.IsChecked == true;

            _selected.RPC.RpcForceKillstreak(killstreakID, sameTeam);
            Log($"Activated killstreak: {(cmbKillstreak.SelectedItem as ComboBoxItem)?.Content} (Team: {sameTeam})");
        }

        private void btnNuke_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var result = MessageBox.Show(
                "Are you sure you want to activate a NUKE? This will affect all players!",
                "Confirm Nuke",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _selected.RPC.NukeKill();
                Log("NUKE ACTIVATED!");
            }
        }

        private void btnAllUAV_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            bool sameTeam = chkSameTeam.IsChecked == true;

            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcForceKillstreak(1, sameTeam); // UAV = 1
                count++;
            }

            if (count > 0)
                Log($"{count} bots activated UAV (Same Team: {sameTeam})");
        }

        private void btnAllSuperSoldier_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            bool sameTeam = chkSameTeam.IsChecked == true;

            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcForceKillstreak(2, sameTeam); // Super Soldier = 2
                count++;
            }

            if (count > 0)
                Log($"{count} bots activated Super Soldier (Same Team: {sameTeam})");
        }
        #endregion

        #region Game Control
        private void btnFlash_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.Flash();
            Log("Flash effect triggered");
        }

        private void btnTeleport_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var random = new Random();
            var position = new Vec3(
                (float)(random.NextDouble() * 20 - 10),
                1.75f,
                (float)(random.NextDouble() * 20 - 10)
            );

            _selected.RPC.TeleportToPosition(position);
            Log($"Teleported to ({position.x:F2}, {position.y:F2}, {position.z:F2})");
        }

        private void btnSetTimeScale_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            if (!float.TryParse(nudTimeScale.Text, out float timeScale))
                timeScale = 1.0f;

            _selected.RPC.SetTimeScale(timeScale);
            Log($"Time scale set to {timeScale}x");
        }

        private void btnMatchOver_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            bool matchOver = chkMatchOver.IsChecked == true;
            _selected.RPC.MatchOverChanged(matchOver);
            Log($"Match over state: {matchOver}");
        }

        private void btnSendAnnouncement_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            string text = txtAnnounce.Text?.Trim();
            if (string.IsNullOrEmpty(text))
            {
                ShowWarning("Enter announcement text");
                return;
            }

            if (!float.TryParse(nudDuration.Text, out float duration))
                duration = 5.0f;

            _selected.SendAnnouncement(text, duration);
            Log($"Announcement sent: {text}");
        }
        #endregion

        #region Chat
        private void btnSendChat_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            string message = txtChatMessage.Text?.Trim();
            if (string.IsNullOrEmpty(message))
            {
                ShowWarning("Enter a chat message");
                return;
            }

            if (!short.TryParse(nudColorR.Text, out short r)) r = 255;
            if (!short.TryParse(nudColorG.Text, out short g)) g = 255;
            if (!short.TryParse(nudColorB.Text, out short b)) b = 255;

            _selected.RPC.RpcSendChatMessage($"[BOT] {_selected.BotName}", message, r, g, b);
            Log($"Chat sent: {message} (RGB: {r},{g},{b})");
        }

        private void btnSendChatAll_Click(object sender, RoutedEventArgs e)
        {
            string message = txtChatMessage.Text?.Trim();
            if (string.IsNullOrEmpty(message))
            {
                ShowWarning("Enter a chat message");
                return;
            }

            if (!short.TryParse(nudColorR.Text, out short r)) r = 255;
            if (!short.TryParse(nudColorG.Text, out short g)) g = 255;
            if (!short.TryParse(nudColorB.Text, out short b)) b = 255;

            int count = 0;
            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcSendChatMessage($"[BOT] {bot.BotName}", message, r, g, b);
                count++;
            }

            Log($"Chat sent to {count} bots: {message} (RGB: {r},{g},{b})");
        }
        #endregion

        #region RPC Playground
        private void btnShowPerk_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.RpcShowPerkMessage(_selected.BotName, "Perk Activated!");
            Log("Perk message shown");
        }

        private void btnSendAuth_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.RpcSendMultiplayerAuthToken("BOT_AUTH_TOKEN");
            Log("Auth token sent");
        }

        private void btnBecomeMaster_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.BecomeNewMasterClient();
            Log("Requested to become master client");
        }

        private void btnRequestPickups_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.RequestForPickupItems();
            _selected.RPC.RequestForPickupTimes();
            Log("Requested pickup items and times");
        }

        private void btnDebugCapsule_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var random = new Random();
            var position = new Vec3(
                (float)(random.NextDouble() * 10 - 5),
                2f,
                (float)(random.NextDouble() * 10 - 5)
            );

            _selected.RPC.ShowDebugCapsule(position);
            Log($"Debug capsule shown at ({position.x:F2}, {position.y:F2}, {position.z:F2})");
        }

        private void btnUpdateKD_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var random = new Random();
            float kd = (float)(random.NextDouble() * 10);

            _selected.RPC.SetKD(kd);
            Log($"K/D ratio updated to {kd:F2}");
        }
        #endregion

        #region Bulk Bot Actions
        private void btnAllShowHitmarker_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcShowHitmarker();
                count++;
            }

            if (count > 0)
                Log($"{count} bots showing hitmarker");
        }

        private void btnAllMelee_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.MpMeleeAnimation();
                count++;
            }

            if (count > 0)
                Log($"{count} bots performing melee attack");
        }

        private void btnAllReload_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.LocalReload();
                count++;
            }

            if (count > 0)
                Log($"{count} bots reloading");
        }

        private void btnAllFlash_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                bot.RPC.Flash();
                count++;
            }

            if (count > 0)
                Log($"{count} bots triggered flash effect");
        }
        #endregion

        #region Player List
        private void btnRefreshPlayers_Click(object sender, RoutedEventArgs e)
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
                    continue;

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

        #region Utility
        private void btnClearEvents_Click(object sender, RoutedEventArgs e)
        {
            lstEvents.Items.Clear();
        }

        private void btnClearConsole_Click(object sender, RoutedEventArgs e)
        {
            txtConsole.Clear();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            Log("Connection is handled automatically when bots are added");
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;
            _selected.Disconnect();
            Log("Disconnecting");
        }

        private void btnSpawn_Click(object sender, RoutedEventArgs e)
        {
            Log("Spawning is handled automatically by the bot system");
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
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        #endregion

        #region Exploits
        private void chkRemoveFloorCollider_Checked(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned())
            {
                chkRemoveFloorCollider.IsChecked = false;
                return;
            }

            _selected.IsFloorColliderRemovalActive = true;
            Log("Floor collider removal activated - sending corrupted quaternion data");
        }

        private void chkRemoveFloorCollider_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
            {
                _selected.IsFloorColliderRemovalActive = false;
                Log("Floor collider removal deactivated");
            }
        }

        private void chkSpamMovement_Checked(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned())
            {
                chkSpamMovement.IsChecked = false;
                return;
            }

            _selected.IsMovementSpamActive = true;
            Log("Movement packet spam activated");
        }

        private void chkSpamMovement_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
            {
                _selected.IsMovementSpamActive = false;
                Log("Movement packet spam deactivated");
            }
        }

        private void btnTeleportOOB_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var position = new Vec3(10000f, 10000f, 10000f);
            _selected.RPC.TeleportToPosition(position);
            Log($"Teleported out of bounds to ({position.x}, {position.y}, {position.z})");
        }

        private void btnTeleportSky_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var position = new Vec3(0f, 1000f, 0f);
            _selected.RPC.TeleportToPosition(position);
            Log($"Teleported to sky at height {position.y}");
        }

        private void btnTeleportUnderground_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var position = new Vec3(0f, -100f, 0f);
            _selected.RPC.TeleportToPosition(position);
            Log($"Teleported underground to depth {Math.Abs(position.y)}");
        }

        private void btnTeleportAllOOB_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Teleport ALL bots out of bounds? This will be very noticeable!",
                "Confirm Mass Teleport",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            int count = 0;
            var random = new Random();
            foreach (var bot in _bots.Where(x => x.RPC != null))
            {
                var position = new Vec3(
                    (float)(random.NextDouble() * 20000 - 10000),
                    (float)(random.NextDouble() * 20000 - 10000),
                    (float)(random.NextDouble() * 20000 - 10000)
                );
                bot.RPC.TeleportToPosition(position);
                count++;
            }

            if (count > 0)
                Log($"{count} bots teleported out of bounds");
        }

        private void btnInvalidSpawn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.PunRespawn(new Vec3(float.MaxValue, float.MaxValue, float.MaxValue));
            Log("Sent invalid spawn packet with max float values");
        }

        private void btnCorruptMovement_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.BroadcastCorruptedMovement();
            Log("Sent corrupted movement packet");
        }

        private void btnMaxValueRPC_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            _selected.RPC.UpdateMPKills(int.MaxValue);
            _selected.RPC.UpdateMPDeaths(0);
            _selected.RPC.SetKD(float.MaxValue);
            _selected.RPC.HealthUpdated(float.MaxValue);
            Log("Sent max value RPCs for kills, K/D, and health");
        }

        private void btnFloodRPC_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var result = MessageBox.Show(
                "Flood the server with RPC events? This may cause crashes or disconnects!",
                "Confirm RPC Flood",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            System.Threading.Tasks.Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    _selected.RPC.RpcShowHitmarker();
                    _selected.RPC.LocalReload();
                    _selected.RPC.MpMeleeAnimation();
                    System.Threading.Thread.Sleep(10);
                }
            });

            Log("Flooding server with 300 RPC events...");
        }
        #endregion
    }
}