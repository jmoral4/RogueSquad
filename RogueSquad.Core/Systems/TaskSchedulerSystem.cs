using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Systems
{

    public sealed class TaskSchedulerSystem
    {

        private static readonly Lazy<TaskSchedulerSystem> lazy =
            new Lazy<TaskSchedulerSystem>(() => new TaskSchedulerSystem());

        public static TaskSchedulerSystem Instance { get { return lazy.Value; } }

        Queue<Behavior> _behaviorQueue;


        private TaskSchedulerSystem()
        {
            _behaviorQueue = new Queue<Behavior>();
        }

        

        public void QueueTask(Behavior behavior)
        {

             _behaviorQueue.Enqueue(behavior);

        }

        public void Update(GameTime gametime)
        {
            //determine what we think is the best way to slice up the queue for a cycle

        }

    }

    public class Behavior
    {
        public Func<GameTime, bool> UpdateAction  {get;set;}

        public bool IsCompleted { get; set; }
        
    }

}
