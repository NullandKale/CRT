using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CRT.Engine
{
    public class SaveDataManager
    {
        public bool dirty;
        public Dictionary<string, string> savedPreferences;

        public SaveDataManager()
        {
            load();
        }

        public void stop()
        {
            if(dirty)
            {
                save();
            }
        }

        public void setValue(string key, string value)
        {
            if(savedPreferences.ContainsKey(key))
            {
                savedPreferences[key] = value;
            }
            else
            {
                savedPreferences.Add(key, value);
            }

            dirty = true;
        }

        public string getValue(string key, string defaultValue)
        {
            if (savedPreferences.ContainsKey(key))
            {
                return savedPreferences[key];
            }
            else
            {
                return defaultValue;
            }
        }

        public bool getBool(string key, bool defaultValue)
        {
            if (savedPreferences.ContainsKey(key))
            {
                return bool.Parse(savedPreferences[key]);
            }
            else
            {
                return defaultValue;
            }
        }

        public int getInt(string key, int defaultValue)
        {
            if (savedPreferences.ContainsKey(key))
            {
                return int.Parse(savedPreferences[key]);
            }
            else
            {
                return defaultValue;
            }
        }

        public double getDouble(string key, double defaultValue)
        {
            if (savedPreferences.ContainsKey(key))
            {
                return double.Parse(savedPreferences[key]);
            }
            else
            {
                return defaultValue;
            }
        }

        private void load()
        {
            if(File.Exists("./settings.set"))
            {
                string[] lines = File.ReadAllLines("./settings.set");

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] split = lines[i].Split("|");

                    if (split.Length == 2)
                    {
                        savedPreferences.Add(split[0], split[1]);
                    }
                }
            }
            dirty = false;
        }

        public void save()
        {
            List<string> lines = new List<string>();

            foreach (KeyValuePair<string, string> kvp in savedPreferences)
            {
                lines.Add(kvp.Key + "|" + kvp.Value);
            }

            File.WriteAllLines("./settings.set", lines);
            dirty = false;
        }
    }
}
