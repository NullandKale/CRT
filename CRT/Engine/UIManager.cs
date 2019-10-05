using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.Engine
{
    public class UIManager : ChexelProvider
    {
        public List<Message> messages;
        public List<Message> ephemeralMessages;

        public UIManager()
        {
            messages = new List<Message>();
            ephemeralMessages = new List<Message>();
        }

        public void AddMessage(Message toAdd, bool ephemeral)
        {
            if(ephemeral)
            {
                ephemeralMessages.Add(toAdd);
            }
            else
            {
                messages.Add(toAdd);
            }
        }

        public void AddMessages(List<Message> toAdd, bool ephemeral)
        {
            if (ephemeral)
            {
                ephemeralMessages.AddRange(toAdd);
            }
            else
            {
                messages.AddRange(toAdd);
            }
        }
        public void RemoveMessage(Message toRemove)
        {
            if(messages.Contains(toRemove))
            {
                messages.Remove(toRemove);
            }
        }

        public void Update()
        {
            ephemeralMessages.Clear();
        }

        public bool hasChexel(int x, int y)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].hasChexel(x, y))
                {
                    return true;
                }
            }

            for (int i = 0; i < ephemeralMessages.Count; i++)
            {
                if (ephemeralMessages[i].hasChexel(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        public Chexel getChexel(int x, int y)
        {
            for(int i = 0; i < messages.Count; i++)
            {
                if(messages[i].hasChexel(x,y))
                {
                    return messages[i].getChexel(x, y);
                }
            }

            for (int i = 0; i < ephemeralMessages.Count; i++)
            {
                if (ephemeralMessages[i].hasChexel(x, y))
                {
                    return ephemeralMessages[i].getChexel(x, y);
                }
            }

            return new Chexel();
        }

        public bool active()
        {
            return true;
        }
    }

    public class Message : ChexelProvider
    {
        public int x;
        public int y;
        public char[] m;
        public bool isActive = true;
        public ConsoleColor f = ConsoleColor.White;
        public ConsoleColor b = ConsoleColor.Black;

        public Message(int x, int y, string message, ConsoleColor f, ConsoleColor b)
        {
            this.x = x;
            this.y = y;
            this.f = f;
            this.b = b;
            m = message.ToCharArray();
        }

        public bool active()
        {
            return isActive;
        }

        public Chexel getChexel(int x, int y)
        {
            return new Chexel(f, b, m[x - this.x]);
        }

        public bool hasChexel(int x, int y)
        {
            return this.y == y && (x >= this.x && x < this.x + m.Length);
        }
    }
}
