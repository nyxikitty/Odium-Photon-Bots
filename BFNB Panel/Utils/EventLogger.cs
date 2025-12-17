// ADVANCED EVENT LOGGER - Add this to your PhotonBot.cs or create a separate EventLogger.cs

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

namespace OdiumPhoton.Core
{
    public static class EventLogger
    {
        private static readonly Dictionary<byte, string> EventCodeNames = new Dictionary<byte, string>
        {
            { 200, "RPC_EVENT" },
            { 201, "MOVEMENT_EVENT" },
            { 202, "SPAWN_EVENT" },
            { 207, "ACTOR_EVENT" },
            { 253, "Join" },
            { 254, "Leave" },
            { 255, "PropertiesChanged" }
        };

        private static readonly Dictionary<byte, string> RpcNames = new Dictionary<byte, string>
        {
            { 0, "AcknowledgeDamageDoneRPC" },
            { 24, "LocalHurt" },
            { 48, "RpcDie" },
            { 51, "RpcShoot" },
            { 87, "RpcRespawned" },
            // Add more as you discover them
        };

        public static void LogEvent(EventData evt, int? localActorNumber = null)
        {
            // Skip our own events if localActorNumber provided
            if (localActorNumber.HasValue && evt.Sender == localActorNumber.Value)
                return;

            string eventName = EventCodeNames.TryGetValue(evt.Code, out string name) ? name : $"Event_{evt.Code}";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n╔════════════════════════════════════════════════════════════════════════");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"║ [{DateTime.Now:HH:mm:ss.fff}] {eventName} (Code: {evt.Code})");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"║ Sender: Actor #{evt.Sender}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"╠════════════════════════════════════════════════════════════════════════");
            Console.ResetColor();

            // Special handling for known event types
            switch (evt.Code)
            {
                case 200: // RPC
                    LogRpcEvent(evt.CustomData);
                    break;
                case 201: // Movement
                    LogMovementEvent(evt.CustomData);
                    break;
                case 202: // Spawn
                    LogSpawnEvent(evt.CustomData);
                    break;
                default:
                    UnpackData(evt.CustomData, 1);
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"╚════════════════════════════════════════════════════════════════════════\n");
            Console.ResetColor();
        }

        private static void LogRpcEvent(object data)
        {
            if (!(data is Hashtable table))
            {
                UnpackData(data, 1);
                return;
            }

            byte? rpcId = table[(byte)5] as byte?;
            object[] parameters = table[(byte)4] as object[];

            string rpcName = rpcId.HasValue && RpcNames.TryGetValue(rpcId.Value, out string name)
                ? name
                : $"RPC_{rpcId}";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  RPC: {rpcName} (ID: {rpcId})");
            Console.ResetColor();

            if (table.ContainsKey((byte)7))
                Console.WriteLine($"  ViewID: {table[(byte)7]}");

            if (parameters != null && parameters.Length > 0)
            {
                Console.WriteLine($"  Parameters ({parameters.Length}):");
                for (int i = 0; i < parameters.Length; i++)
                {
                    Console.Write($"    [{i}] = ");
                    PrintValue(parameters[i], 3);
                }
            }

            // Show all other keys
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Raw Data:");
            Console.ResetColor();
            UnpackData(data, 2);
        }

        private static void LogMovementEvent(object data)
        {
            if (!(data is Hashtable table))
            {
                UnpackData(data, 1);
                return;
            }

            object[] movement = table[(byte)10] as object[];
            long? timestamp = table[(byte)0] as long?;

            Console.WriteLine($"  Timestamp: {timestamp}");

            if (movement != null && movement.Length >= 23)
            {
                int viewId = movement[0] as int? ?? 0;
                bool grounded = movement[1] as bool? ?? false;

                Console.WriteLine($"  ViewID: {viewId} (Actor: {viewId / 1000})");
                Console.WriteLine($"  Grounded: {grounded}");

                // Position (indices 4, 5, 9 are x, y, z as shorts)
                if (movement[4] is short x && movement[5] is short y && movement[9] is short z)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"  Position: ({x / 1000f:F3}, {y / 1000f:F3}, {z / 1000f:F3})");
                    Console.ResetColor();
                }

                // Rotation quaternion (indices 6, 7, 8, 10)
                if (movement.Length > 10 && movement[6] is short qx && movement[7] is short qy &&
                    movement[8] is short qz && movement[10] is short qw)
                {
                    Console.WriteLine($"  Rotation: ({qx}, {qy}, {qz}, {qw})");
                }

                // Encrypted position (index 22)
                if (movement.Length > 22 && movement[22] != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"  Encrypted: {movement[22]}");
                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Raw Data:");
            Console.ResetColor();
            UnpackData(data, 2);
        }

        private static void LogSpawnEvent(object data)
        {
            if (!(data is Hashtable table))
            {
                UnpackData(data, 1);
                return;
            }

            string prefab = table[(byte)0] as string;
            int? viewId = table[(byte)7] as int?;
            long? timestamp = table[(byte)6] as long?;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  Prefab: {prefab}");
            Console.ResetColor();
            Console.WriteLine($"  ViewID: {viewId} (Actor: {(viewId.HasValue ? viewId.Value / 1000 : -1)})");
            Console.WriteLine($"  Timestamp: {timestamp}");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Raw Data:");
            Console.ResetColor();
            UnpackData(data, 2);
        }

        private static void UnpackData(object data, int indent)
        {
            string indentation = new string(' ', indent * 2);

            if (data == null)
            {
                Console.WriteLine($"{indentation}null");
                return;
            }

            if (data is Hashtable hashtable)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"{indentation}Hashtable ({hashtable.Count} entries):");
                Console.ResetColor();

                foreach (DictionaryEntry entry in hashtable)
                {
                    Console.Write($"{indentation}  ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"[{FormatKey(entry.Key)}]");
                    Console.ResetColor();
                    Console.Write(" = ");

                    if (entry.Value is Hashtable || entry.Value is object[] || entry.Value is Dictionary<object, object>)
                    {
                        Console.WriteLine();
                        UnpackData(entry.Value, indent + 2);
                    }
                    else
                    {
                        PrintValue(entry.Value, 0);
                    }
                }
            }
            else if (data is object[] array)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"{indentation}Array ({array.Length} elements):");
                Console.ResetColor();

                for (int i = 0; i < array.Length; i++)
                {
                    Console.Write($"{indentation}  ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"[{i}]");
                    Console.ResetColor();
                    Console.Write(" = ");

                    if (array[i] is Hashtable || array[i] is object[] || array[i] is Dictionary<object, object>)
                    {
                        Console.WriteLine();
                        UnpackData(array[i], indent + 2);
                    }
                    else
                    {
                        PrintValue(array[i], 0);
                    }
                }
            }
            else if (data is byte[] byteArray)
            {
                if (byteArray.Length <= 16)
                {
                    Console.WriteLine($"{indentation}byte[{byteArray.Length}]: [{string.Join(", ", byteArray)}]");
                }
                else
                {
                    Console.WriteLine($"{indentation}byte[{byteArray.Length}]: [{string.Join(", ", byteArray.Take(16))}... (truncated)]");
                }
            }
            else
            {
                PrintValue(data, indent);
            }
        }

        private static void PrintValue(object value, int indent)
        {
            string indentation = indent > 0 ? new string(' ', indent * 2) : "";

            if (value == null)
            {
                Console.WriteLine($"{indentation}null");
                return;
            }

            Type type = value.GetType();

            // Handle custom types
            if (type.Name == "Vec3")
            {
                dynamic vec = value;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{indentation}Vec3({vec.x:F2}, {vec.y:F2}, {vec.z:F2})");
                Console.ResetColor();
            }
            else if (type.Name == "Quat")
            {
                dynamic quat = value;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{indentation}Quat({quat.x:F2}, {quat.y:F2}, {quat.z:F2}, {quat.w:F2})");
                Console.ResetColor();
            }
            else if (value is string str)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{indentation}\"{str}\"");
                Console.ResetColor();
            }
            else if (value is bool b)
            {
                Console.ForegroundColor = b ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"{indentation}{b}");
                Console.ResetColor();
            }
            else if (value is byte || value is short || value is int || value is long)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{indentation}{value} ({type.Name})");
                Console.ResetColor();
            }
            else if (value is float f)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{indentation}{f:F3}f");
                Console.ResetColor();
            }
            else if (value is double d)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{indentation}{d:F3}d");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{indentation}{value} ({type.Name})");
            }
        }

        private static string FormatKey(object key)
        {
            if (key is byte b)
                return $"{b}";
            else if (key is string s)
                return $"\"{s}\"";
            else
                return key?.ToString() ?? "null";
        }
    }
}

// USAGE IN PhotonBot.cs:
/*
private void OnEventReceived(EventData evt)
{
    // Log all events to console
    EventLogger.LogEvent(evt, LocalPlayer.ActorNumber);
    
    // Your existing event handling code...
    if (evt.Code == MOVEMENT_EVENT)
    {
        // ... handle movement
    }
}
*/