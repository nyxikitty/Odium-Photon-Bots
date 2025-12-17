using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        private readonly List<PhotonBot> bots;
        private readonly DispatcherTimer refreshTimer;
        private PhotonBot selectedBot;

        private readonly List<string> proxyList;
        private int currentProxyIndex;

        public MainWindow()
        {
            AllocConsole();
            Console.Title = "Odium Bot Manager - Debug Console";

            InitializeComponent();

            bots = new List<PhotonBot>();
            proxyList = new List<string>();
            currentProxyIndex = 0;

            refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromMilliseconds(500);
            refreshTimer.Tick += OnRefreshTick;
            refreshTimer.Start();

            Log("Bot Manager ready");
        }

        private void OnRefreshTick(object sender, EventArgs e)
        {
            if (selectedBot != null)
            {
                UpdateStatus();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            foreach (var bot in bots)
            {
                bot?.Disconnect();
            }

            FreeConsole();
        }

        private void UpdateStatus()
        {
            lblConnStatusVal.Text = selectedBot.State.ToString();

            if (selectedBot.LoadBalancingPeer != null)
                lblPingVal.Text = $"{selectedBot.LoadBalancingPeer.RoundTripTime}ms";
            else
                lblPingVal.Text = "N/A";

            if (selectedBot.CurrentRoom != null)
            {
                var room = selectedBot.CurrentRoom;
                lblRoomVal.Text = room.Name;
                lblPlayerCountVal.Text = $"{room.PlayerCount}/{room.MaxPlayers}";
            }
            else
            {
                lblRoomVal.Text = "N/A";
                lblPlayerCountVal.Text = "N/A";
            }
        }

        private void Log(string message)
        {
            string msg = $"[{DateTime.Now:HH:mm:ss}] {message}";

            Console.WriteLine(msg);

            Dispatcher.Invoke(() =>
            {
                txtConsole.AppendText(msg + "\r\n");
                if (chkAutoScroll.IsChecked == true)
                {
                    txtConsole.ScrollToEnd();
                }
            });
        }

        private string GenerateBotName()
        {
            Random rand = new Random();
            string[] names = new string[]
            {
                "Shadow", "Phoenix", "Dragon", "Hunter", "Viper",
                "Ghost", "Blaze", "Storm", "Raven", "Wolf",
                "Titan", "Nova", "Cobra", "Reaper", "Ace",
                "Fury", "Frost", "Razor", "Hawk", "Venom",
                "Apex", "Rex", "Zeus", "Thor", "Loki",
                "Ninja", "Sniper", "Flash", "Rocket", "Bullet",
                "Knight", "Warrior", "Savage", "Legend", "Demon",
                "Angel", "Spike", "Dagger", "Echo", "Omega"
            };

            return names[rand.Next(names.Length)];
        }

        private void btnAddBot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string botName = txtBotName.Text?.Trim();
                if (string.IsNullOrWhiteSpace(botName))
                {
                    botName = GenerateBotName();
                }
                string clanTag = txtClanTag.Text?.Trim();
                byte team = (byte)(cmbTeam.SelectedIndex + 1);
                byte rank = 200;
                if (!byte.TryParse(nudRank.Text, out rank))
                    rank = 200;
                PhotonBot bot = new PhotonBot("8c2cad3e-2e3f-4941-9044-b390ff2c4956", "1.84.0_1.99");
                bot.BotName = botName;
                bot.ClanTag = clanTag;
                bot.Team = team;
                bot.Rank = rank;
                bots.Add(bot);
                lstBots.Items.Add($"{clanTag} {botName}");
                Log($"Bot added: {clanTag} {botName} (Team {team}, Rank {rank})");
            }
            catch (Exception ex)
            {
                Log($"Failed to add bot: {ex.Message}");
                CustomDialogs.ShowError("Error", $"Failed to add bot: {ex.Message}", this);
            }
        }

        private void btnRemoveBot_Click(object sender, RoutedEventArgs e)
        {
            int idx = lstBots.SelectedIndex;
            if (idx < 0 || idx >= bots.Count) return;

            PhotonBot bot = bots[idx];
            bot.Disconnect();

            bots.RemoveAt(idx);
            lstBots.Items.RemoveAt(idx);

            if (selectedBot == bot)
                selectedBot = null;

            Log($"Bot #{idx + 1} removed");
        }

        private void lstBots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = lstBots.SelectedIndex;
            if (idx >= 0 && idx < bots.Count)
            {
                selectedBot = bots[idx];
                RefreshPlayerList();
            }
        }

        private void btnJoinRandom_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            selectedBot.JoinRandom();
            Log("Joining random room");
        }

        private void btnJoinRandomAll_Click(object sender, RoutedEventArgs e)
        {
            if (bots.Count == 0)
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

            selectedBot.JoinRoom(txtRoomName.Text);
            Log($"Joining room: {txtRoomName.Text}");
        }

        private void btnJoinByNameAll_Click(object sender, RoutedEventArgs e)
        {
            if (bots.Count == 0)
            {
                ShowWarning("No bots available");
                return;
            }
            if (!ValidateRoomName()) return;

            string roomName = txtRoomName.Text;
            int cnt = 0;

            foreach (var bot in GetReadyBots())
            {
                bot.JoinRoom(roomName);
                cnt++;
            }

            Log($"{cnt} bots joining: {roomName}");
        }

        private void btnCreateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            string roomName = string.IsNullOrWhiteSpace(txtRoomName.Text)
                ? null
                : txtRoomName.Text;

            string map = "Outpost";
            string mode = "Team Deathmatch";

            if (cmbMap.SelectedItem is ComboBoxItem mapItem)
                map = mapItem.Content.ToString();

            if (cmbGameMode.SelectedItem is ComboBoxItem modeItem)
                mode = modeItem.Content.ToString();

            byte maxPlayers = 10;
            if (!byte.TryParse(nudMaxPlayers.Text, out maxPlayers))
                maxPlayers = 10;

            selectedBot.CreateRoom(roomName, map, mode, maxPlayers);
            Log($"Creating room: {map} - {mode}");
        }

        private void btnLeaveRoom_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            selectedBot.LeaveRoom();
            Log("Leaving room");
        }

        private void btnLeaveAll_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            foreach (var bot in bots.Where(b => b.CurrentRoom != null))
            {
                bot.LeaveRoom();
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots leaving rooms");
        }

        private void btnStartMovement_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            selectedBot.IsActive = true;
            Log("Movement started");
        }

        private void btnStopMovement_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            selectedBot.StopMovement();
            Log("Movement stopped");
        }

        private void btnSetWeaponType_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            byte weaponType;
            if (!byte.TryParse(txtWeaponType.Text, out weaponType))
            {
                ShowWarning("Enter a valid weapon type (0-255)");
                return;
            }

            selectedBot.RPC.WeaponTypeChanged(weaponType);
            Log($"Weapon type changed to {weaponType}");
        }

        private void btnShowHitmarker_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            selectedBot.RPC.RpcShowHitmarker();
            Log("Hitmarker shown");
        }

        private void btnMeleeAttack_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            selectedBot.RPC.MpMeleeAnimation();
            Log("Melee attack animation played");
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            selectedBot.RPC.LocalReload();
            Log("Reload animation played");
        }

        private void btnThrowGrenade_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            selectedBot.RPC.MpThrowGrenadeAnimation();
            Log("Grenade throw animation played");
        }

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

            selectedBot.RPC.RpcForceKillstreak(killstreakID, sameTeam);

            string ksName = "Unknown";
            if (cmbKillstreak.SelectedItem is ComboBoxItem item)
                ksName = item.Content.ToString();

            Log($"Activated killstreak: {ksName} (Team: {sameTeam})");
        }

        private void btnNuke_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            var result = CustomDialogs.ShowConfirm(
                "Confirm Nuke",
                "Are you sure you want to activate a NUKE? This will affect all players!",
                this);

            if (result == CustomDialogs.DialogResult.Yes)
            {
                selectedBot.RPC.NukeKill();
                Log("NUKE ACTIVATED!");
            }
        }

        private void btnAllUAV_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            bool sameTeam = chkSameTeam.IsChecked == true;

            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcForceKillstreak(1, sameTeam);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots activated UAV (Same Team: {sameTeam})");
        }

        private void btnAllSuperSoldier_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            bool sameTeam = chkSameTeam.IsChecked == true;

            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcForceKillstreak(2, sameTeam);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots activated Super Soldier (Same Team: {sameTeam})");
        }

        private void btnFlash_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            selectedBot.RPC.Flash();
            Log("Flash effect triggered");
        }

        private void btnTeleport_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            Random rnd = new Random();
            Vec3 position = new Vec3(
                (float)(rnd.NextDouble() * 20 - 10),
                1.75f,
                (float)(rnd.NextDouble() * 20 - 10)
            );

            selectedBot.RPC.TeleportToPosition(position);
            Log($"Teleported to ({position.x:F2}, {position.y:F2}, {position.z:F2})");
        }

        private void btnSetTimeScale_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            float timeScale = 1.0f;
            if (!float.TryParse(nudTimeScale.Text, out timeScale))
                timeScale = 1.0f;

            selectedBot.RPC.SetTimeScale(timeScale);
            Log($"Time scale set to {timeScale}x");
        }

        private void btnMatchOver_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            bool matchOver = chkMatchOver.IsChecked == true;
            selectedBot.RPC.MatchOverChanged(matchOver);
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

            float duration = 5.0f;
            if (!float.TryParse(nudDuration.Text, out duration))
                duration = 5.0f;

            selectedBot.SendAnnouncement(text, duration);
            Log($"Announcement sent: {text}");
        }

        private void btnSendChat_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            string message = txtChatMessage.Text?.Trim();
            if (string.IsNullOrEmpty(message))
            {
                ShowWarning("Enter a chat message");
                return;
            }

            short r = 255, g = 255, b = 255;
            if (!short.TryParse(nudColorR.Text, out r)) r = 255;
            if (!short.TryParse(nudColorG.Text, out g)) g = 255;
            if (!short.TryParse(nudColorB.Text, out b)) b = 255;

            selectedBot.RPC.RpcSendChatMessage($"[BOT] {selectedBot.BotName}", message, r, g, b);
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

            short r = 255, g = 255, b = 255;
            if (!short.TryParse(nudColorR.Text, out r)) r = 255;
            if (!short.TryParse(nudColorG.Text, out g)) g = 255;
            if (!short.TryParse(nudColorB.Text, out b)) b = 255;

            int cnt = 0;
            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcSendChatMessage($"[BOT] {bot.BotName}", message, r, g, b);
                cnt++;
            }

            Log($"Chat sent to {cnt} bots: {message} (RGB: {r},{g},{b})");
        }

        private void btnAllShowHitmarker_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.RpcShowHitmarker();
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots showing hitmarker");
        }

        private void btnAllMelee_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.MpMeleeAnimation();
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots performing melee attack");
        }

        private void btnAllReload_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.LocalReload();
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots reloading");
        }

        private void btnAllFlash_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            foreach (var bot in bots.Where(x => x.RPC != null))
            {
                bot.RPC.Flash();
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots triggered flash effect");
        }

        private void btnRefreshPlayers_Click(object sender, RoutedEventArgs e)
        {
            RefreshPlayerList();
        }

        private void RefreshPlayerList()
        {
            lstPlayers.Items.Clear();

            if (selectedBot?.CurrentRoom == null)
            {
                lstPlayers.Items.Add("Not in a room");
                return;
            }

            List<Player> players = selectedBot.GetPlayers();
            foreach (Player player in players)
            {
                string name = CleanName(player.NickName ?? "Unknown");
                lstPlayers.Items.Add($"[{player.ActorNumber}] {name}");
            }
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
            CustomDialogs.ShowInformation("Information", "Connection is handled automatically when bots are added", this);
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;
            selectedBot.Disconnect();
            Log("Disconnecting");
        }

        private void btnSpawn_Click(object sender, RoutedEventArgs e)
        {
            CustomDialogs.ShowInformation("Information", "Spawning is handled automatically by the bot system", this);
        }

        private bool ValidateSelection()
        {
            if (selectedBot == null)
            {
                ShowWarning("Select a bot first");
                return false;
            }
            return true;
        }

        private bool ValidateSpawned()
        {
            if (selectedBot.RPC == null)
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
            return bots.Where(b =>
                b.State == ClientState.ConnectedToMasterServer ||
                b.State == ClientState.JoinedLobby);
        }

        private static string CleanName(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, "</?color[^>]*>", "");
        }

        private void ShowWarning(string message)
        {
            CustomDialogs.ShowWarning("Warning", message, this);
        }

        private void chkRemoveFloorCollider_Checked(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned())
            {
                chkRemoveFloorCollider.IsChecked = false;
                return;
            }

            selectedBot.IsFloorColliderRemovalActive = true;
            Log("Floor collider removal activated - sending corrupted quaternion data");
        }

        private void chkRemoveFloorCollider_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedBot != null)
            {
                selectedBot.IsFloorColliderRemovalActive = false;
                Log("Floor collider removal deactivated");
            }
        }

        private void cmbRoomProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRoomProperty.SelectedItem is ComboBoxItem item)
            {
                string propertyType = item.Tag?.ToString() ?? "string";
            }
        }

        private void btnUpdateRoomProperty_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection())
            {
                ShowWarning("Select a bot first");
                return;
            }

            if (selectedBot.CurrentRoom == null)
            {
                ShowWarning("Bot must be in a room to modify properties");
                return;
            }

            if (cmbRoomProperty.SelectedItem == null)
            {
                ShowWarning("Select a property to update");
                return;
            }

            ComboBoxItem selectedItem = cmbRoomProperty.SelectedItem as ComboBoxItem;
            string propertyKey = selectedItem.Content.ToString();
            string propertyType = selectedItem.Tag?.ToString() ?? "string";
            string valueText = txtRoomPropertyValue.Text?.Trim();

            if (string.IsNullOrWhiteSpace(valueText))
            {
                ShowWarning("Enter a value for the property");
                return;
            }

            try
            {
                object parsedValue = ParsePropertyValue(valueText, propertyType);

                Hashtable properties = new Hashtable();
                properties.Add(propertyKey, parsedValue);

                selectedBot.CurrentRoom.SetCustomProperties(properties);

                Log($"Room property updated: {propertyKey} = {parsedValue} ({propertyType})");
            }
            catch (Exception ex)
            {
                ShowWarning($"Failed to parse value as {propertyType}:\n{ex.Message}");
                Log($"Error updating room property: {ex.Message}");
            }
        }

        private object ParsePropertyValue(string value, string type)
        {
            switch (type.ToLower())
            {
                case "string":
                    return value;

                case "int":
                    int intVal;
                    if (int.TryParse(value, out intVal))
                        return intVal;
                    throw new FormatException($"Cannot parse '{value}' as integer");

                case "byte":
                    byte byteVal;
                    if (byte.TryParse(value, out byteVal))
                        return byteVal;
                    throw new FormatException($"Cannot parse '{value}' as byte");

                case "float":
                    float floatVal;
                    if (float.TryParse(value, out floatVal))
                        return floatVal;
                    throw new FormatException($"Cannot parse '{value}' as float");

                case "bool":
                    bool boolVal;
                    if (bool.TryParse(value, out boolVal))
                        return boolVal;
                    if (value == "0") return false;
                    if (value == "1") return true;
                    throw new FormatException($"Cannot parse '{value}' as boolean (use true/false or 0/1)");

                default:
                    return value;
            }
        }

        private void lstPlayers_RightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void ContextMenu_Damage_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            float damage = 25f;
            byte weaponType = 14;

            Vec3 targetPos = new Vec3(0, 1.75f, 0);

            selectedBot.RPC.LocalHurt(
                selectedBot.LocalPlayer.ActorNumber,
                damage,
                targetPos,
                weaponType,
                75f
            );

            selectedBot.RPC.AcknowledgeDamageDoneRPC("hit", damage, targetActor);

            Log($"Damaged player {targetActor} ({CleanName(targetPlayer.NickName)}) for 25 HP");
        }

        private void ContextMenu_Kill_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            float damage = 100f;
            byte weaponType = 14;

            Vec3 targetPos = new Vec3(0, 1.75f, 0);

            selectedBot.RPC.LocalHurt(
                selectedBot.LocalPlayer.ActorNumber,
                damage,
                targetPos,
                weaponType,
                0f
            );

            selectedBot.RPC.AcknowledgeDamageDoneRPC("hit", damage, targetActor);

            selectedBot.RPC.RpcDie(
                targetActor,
                selectedBot.LocalPlayer.ActorNumber,
                (byte)100,
                weaponType,
                false
            );

            Log($"Killed player {targetActor} ({CleanName(targetPlayer.NickName)}) with 100 HP damage");
        }

        private void ContextMenu_Orbit_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection()) return;

            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == actor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.SetMovement(MovementMode.Orbit, actor);
            Log($"Orbiting player {actor} ({CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_AllOrbit_Click(object sender, RoutedEventArgs e)
        {
            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            Player targetPlayer = selectedBot?.GetPlayers().FirstOrDefault(p => p.ActorNumber == actor);
            string playerName = targetPlayer != null ? CleanName(targetPlayer.NickName) : $"Actor {actor}";

            int cnt = 0;
            var botsInRoom = bots.Where(b => b.CurrentRoom != null).ToList();
            int totalBots = botsInRoom.Count;

            foreach (var bot in botsInRoom)
            {
                float offsetAngle = (float)(2 * Math.PI * cnt / totalBots);
                bot.SetMovement(MovementMode.Orbit, actor, offsetAngle);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots now orbiting player {actor} ({playerName}) with distributed offsets");
        }

        private void ContextMenu_OrbitTight_Click(object sender, RoutedEventArgs e)
        {
            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            int cnt = 0;
            var botsInRoom = bots.Where(b => b.CurrentRoom != null).ToList();
            int totalBots = botsInRoom.Count;

            foreach (var bot in botsInRoom)
            {
                bot.SetOrbitRadius(1.5f);
                float offsetAngle = (float)(2 * Math.PI * cnt / totalBots);
                bot.SetMovement(MovementMode.Orbit, actor, offsetAngle);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots orbiting in tight circle (1.5m radius)");
        }

        private void ContextMenu_OrbitNormal_Click(object sender, RoutedEventArgs e)
        {
            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            int cnt = 0;
            var botsInRoom = bots.Where(b => b.CurrentRoom != null).ToList();
            int totalBots = botsInRoom.Count;

            foreach (var bot in botsInRoom)
            {
                bot.SetOrbitRadius(3f);
                float offsetAngle = (float)(2 * Math.PI * cnt / totalBots);
                bot.SetMovement(MovementMode.Orbit, actor, offsetAngle);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots orbiting in normal circle (3m radius)");
        }

        private void ContextMenu_OrbitWide_Click(object sender, RoutedEventArgs e)
        {
            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            int cnt = 0;
            var botsInRoom = bots.Where(b => b.CurrentRoom != null).ToList();
            int totalBots = botsInRoom.Count;

            foreach (var bot in botsInRoom)
            {
                bot.SetOrbitRadius(5f);
                float offsetAngle = (float)(2 * Math.PI * cnt / totalBots);
                bot.SetMovement(MovementMode.Orbit, actor, offsetAngle);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots orbiting in wide circle (5m radius)");
        }

        private void ContextMenu_OrbitFast_Click(object sender, RoutedEventArgs e)
        {
            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            int cnt = 0;
            var botsInRoom = bots.Where(b => b.CurrentRoom != null).ToList();
            int totalBots = botsInRoom.Count;

            foreach (var bot in botsInRoom)
            {
                bot.SetOrbitSpeed(0.15f);
                float offsetAngle = (float)(2 * Math.PI * cnt / totalBots);
                bot.SetMovement(MovementMode.Orbit, actor, offsetAngle);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots orbiting fast");
        }

        private void ContextMenu_OrbitSlow_Click(object sender, RoutedEventArgs e)
        {
            int actor;
            if (!TryGetSelectedActor(out actor))
            {
                ShowWarning("Select a player to orbit");
                return;
            }

            int cnt = 0;
            var botsInRoom = bots.Where(b => b.CurrentRoom != null).ToList();
            int totalBots = botsInRoom.Count;

            foreach (var bot in botsInRoom)
            {
                bot.SetOrbitSpeed(0.03f);
                float offsetAngle = (float)(2 * Math.PI * cnt / totalBots);
                bot.SetMovement(MovementMode.Orbit, actor, offsetAngle);
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots orbiting slow");
        }

        private void ContextMenu_StopAllOrbit_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            foreach (var bot in bots)
            {
                bot.StopMovement();
                cnt++;
            }

            if (cnt > 0)
                Log($"{cnt} bots stopped orbiting");
        }

        private void ContextMenu_Flash_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.RPC.Flash();
            Log($"Flash effect triggered (affects all players including {CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_Teleport_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            Dictionary<int, Vec3> playerPositions = selectedBot.GetPlayerPositions();

            Vec3 targetPos;
            if (playerPositions != null && playerPositions.TryGetValue(targetActor, out targetPos))
            {
                Vec3 teleportPos = new Vec3(
                    targetPos.x + 2f,
                    targetPos.y,
                    targetPos.z
                );

                selectedBot.RPC.TeleportToPosition(teleportPos);
                Log($"Teleported to player {targetActor} ({CleanName(targetPlayer.NickName)}) at position ({teleportPos.x:F2}, {teleportPos.y:F2}, {teleportPos.z:F2})");
            }
            else
            {
                Vec3 defaultPos = new Vec3(5f, 1.75f, 5f);
                selectedBot.RPC.TeleportToPosition(defaultPos);
                Log($"Teleported to approximate location (player position not tracked)");
            }
        }

        private void ContextMenu_ForceSuicide_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.RPC.RpcDieSpoofed(
                targetActor,
                targetActor,
                targetActor,
                (byte)100,
                14,
                false
            );

            Log($"Forced suicide on player {targetActor} ({CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_SpoofHitmarker_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.RPC.RpcShowHitmarkerSpoofed(targetActor);
            Log($"Spoofed hitmarker from player {targetActor} ({CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_SpoofMelee_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.RPC.MpMeleeAnimationSpoofed(targetActor);
            Log($"Spoofed melee attack from player {targetActor} ({CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_SpoofReload_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.RPC.LocalReloadSpoofed(targetActor);
            Log($"Spoofed reload from player {targetActor} ({CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_SpoofGrenade_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            selectedBot.RPC.MpThrowGrenadeAnimationSpoofed(targetActor);
            Log($"Spoofed grenade throw from player {targetActor} ({CleanName(targetPlayer.NickName)})");
        }

        private void ContextMenu_SpoofChat_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSelection() || !ValidateSpawned()) return;

            int targetActor;
            if (!TryGetSelectedActor(out targetActor))
            {
                ShowWarning("Select a player first");
                return;
            }

            Player targetPlayer = selectedBot.GetPlayers().FirstOrDefault(p => p.ActorNumber == targetActor);
            if (targetPlayer == null)
            {
                ShowWarning("Target player not found");
                return;
            }

            string result = CustomDialogs.ShowInput(
                "Spoof Chat Message",
                "Enter message to send as this player:",
                "I'm being controlled!",
                this
            );

            if (string.IsNullOrEmpty(result))
                return;

            string targetName = CleanName(targetPlayer.NickName);
            selectedBot.RPC.RpcSendChatMessageSpoofed(targetActor, targetName, result, 255, 255, 255);
            Log($"Spoofed chat from player {targetActor} ({targetName}): {result}");
        }
    }
}