using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSquadLib.BaseServices
{

    class DebugMessage
    {
        public double Lifetime { get; set; }
        public string Message { get; set; }

    }

    sealed class DebugMsgQueue
    {

        private static readonly Lazy<DebugMsgQueue> lazy =
            new Lazy<DebugMsgQueue>(() => new DebugMsgQueue());

        public static DebugMsgQueue Instance { get { return lazy.Value; } }
        private ConcurrentQueue<DebugMessage> MessageQueue { get; set; }
        private Queue<string> MessageHistory { get; set; }
        private DebugMessage LastMessage;

        private DebugMsgQueue()
        {
            MessageQueue = new ConcurrentQueue<DebugMessage>();
        }

        public void Write(string msg, double lifetime = 1000)
        {
            MessageQueue.Enqueue(new DebugMessage() { Message = msg, Lifetime = lifetime });
            MessageHistory.Enqueue(msg);
            if (MessageHistory.Count > 100)
            {
                MessageHistory.Dequeue();
            }
        }

        public void Update(GameTime elapsed)
        {
            if (LastMessage != null)
            {
                // see if the last message is still valid 
                LastMessage.Lifetime -= elapsed.ElapsedGameTime.TotalMilliseconds;
                if (LastMessage.Lifetime <= 0)
                {
                    LastMessage = null;
                }
            }
        }

        public List<string> GetHistory()
        {
            return MessageHistory.ToList();
        }

        public string GetLast()
        {
            if (LastMessage != null)
            {
                return LastMessage.Message;
            }
            else
            {
                // lastmessage was null so let's try dequeing something
                if (MessageQueue.TryDequeue(out LastMessage))
                {
                    // success
                    return LastMessage.Message;
                }
                else
                {
                    //no messages
                    return string.Empty;
                }

            }
        }
    }
}
