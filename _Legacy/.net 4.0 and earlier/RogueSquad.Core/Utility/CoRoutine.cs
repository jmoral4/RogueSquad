using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Utility
{
    public class Coroutine 
    {
        public bool Finished { get; private set; }
        public bool RemoveOnComplete = true;
        public bool UseRawDeltaTime = false;

        private Stack<IEnumerator> enumerators;
        private float waitTimer;
        private bool ended;
        bool Active;

        public Coroutine(IEnumerator functionCall)           
        {
            enumerators = new Stack<IEnumerator>();
            enumerators.Push(functionCall);
        }

        public Coroutine()
            
        {
            enumerators = new Stack<IEnumerator>();
        }

        public void Update(GameTime gameTime)
        {
            ended = false;
            IEnumerator now = enumerators.Peek();

            if (waitTimer > 0)
                waitTimer -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds);

            else if (now.MoveNext())
            {
                if (now.Current is int)
                    waitTimer = (int)now.Current;
                if (now.Current is float)
                    waitTimer = (float)now.Current;
                else if (now.Current is IEnumerator)
                    enumerators.Push(now.Current as IEnumerator);
            }
            else if (!ended)
            {
                enumerators.Pop();
                if (enumerators.Count == 0)
                {
                    Finished = true;
                    Active = false;
                    //if (RemoveOnComplete)
                    //    RemoveSelf();
                }
            }
        }

        public void Cancel()
        {
            Active = false;
            Finished = true;
            waitTimer = 0;
            enumerators.Clear();

            ended = true;
        }

        public void Replace(IEnumerator functionCall)
        {
            Active = true;
            Finished = false;
            waitTimer = 0;
            enumerators.Clear();
            enumerators.Push(functionCall);

            ended = true;
        }
    }

    public class CoroutineHolder 
    {
        private List<CoroutineData> coroutineList;
        private HashSet<CoroutineData> toRemove;
        private int nextID;
        private bool isRunning;
        public bool IsEmpty => coroutineList.Count == 0;

        public CoroutineHolder()
            
        {
            coroutineList = new List<CoroutineData>();
            toRemove = new HashSet<CoroutineData>();
        }

        public  void Update()
        {
            isRunning = true;
            for (int i = 0; i < coroutineList.Count; i++)
            {
                var now = coroutineList[i].Data.Peek();

                if (now.MoveNext())
                {
                    if (now.Current is IEnumerator)
                        coroutineList[i].Data.Push(now.Current as IEnumerator);
                }
                else
                {
                    coroutineList[i].Data.Pop();
                    if (coroutineList[i].Data.Count == 0)
                        toRemove.Add(coroutineList[i]);
                }
            }
            isRunning = false;

            if (toRemove.Count > 0)
            {
                foreach (var r in toRemove)
                    coroutineList.Remove(r);
                toRemove.Clear();
            }
        }


        public void EndCoroutine(int id)
        {
            foreach (var c in coroutineList)
            {
                if (c.ID == id)
                {
                    if (isRunning)
                        toRemove.Add(c);
                    else
                        coroutineList.Remove(c);
                    break;
                }
            }
        }

        public int StartCoroutine(IEnumerator functionCall)
        {
            var data = new CoroutineData(nextID++, functionCall);
            coroutineList.Add(data);
            return data.ID;
        }

        static public IEnumerator WaitForFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
                yield return 0;
        }

        private class CoroutineData
        {
            public int ID;
            public Stack<IEnumerator> Data;

            public CoroutineData(int id, IEnumerator functionCall)
            {
                ID = id;
                Data = new Stack<IEnumerator>();
                Data.Push(functionCall);
            }
        }
    }
}
