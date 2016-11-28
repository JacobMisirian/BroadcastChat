using System;
using System.IO;

namespace BroadcastChatServer.Config
{
    public class BroadcastChatServerConfigParser
    {
        public static BroadcastChatServerConfig Parse(string path)
        {
            BroadcastChatServerConfig config = new BroadcastChatServerConfig();

            StreamReader reader = new StreamReader(path);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                string line = reader.ReadLine();
                if (line == null || line.Trim() == string.Empty || line.Trim().StartsWith("#"))
                    continue;

                string[] parts = line.Split(' ');

                switch (parts[0].ToUpper())
                {
                    case "MOTD":
                        config.Motd = line.Substring(line.IndexOf(" ") + 1);
                        break;
                    case "NAME":
                        config.ServerName = line.Substring(line.IndexOf(" ") + 1);
                        break;
                    case "PORT":
                        config.Port = Convert.ToInt32(parts[1]);
                        break;
                }
            }
            reader.Close();

            return config;
        }
    }
}

