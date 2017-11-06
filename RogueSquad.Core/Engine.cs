using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core
{
    //http://csharpindepth.com/Articles/General/Singleton.aspx
    public sealed class Engine
    {

        private static readonly Lazy<Engine> lazy =
            new Lazy<Engine>(() => new Engine());

        public static Engine Instance { get { return lazy.Value; } }

        private Engine()
        {
        }

        private GraphicsDeviceManager _device;
        private ContentManager _content;

        public void Init(GraphicsDeviceManager device, ContentManager content) {
            _device = device;
            _content = content;
            _liveEntities = new BitArray(MAX_ENTITIES);            
        }

        const int MAX_ENTITIES = 20000;
        int _lastFreeEntity;
        BitArray _liveEntities;

        public int CreateUniqueEntityId()
        {
            return GetNextFreeEntity();
        }

        private int GetNextFreeEntity()
        {
            if (_lastFreeEntity >= MAX_ENTITIES  )
            {
                _lastFreeEntity = -1;
            }

            for (int i = 0; i < MAX_ENTITIES; i++)
            {
                if (!_liveEntities[i])
                {
                    _liveEntities[i] = true;
                    _lastFreeEntity = i;
                    break;
                }
            }

            if (_lastFreeEntity == -1)
                throw new Exception("Could not acquire a valid ID for the requested Entity!");

            return _lastFreeEntity;         
        }

        private void RemoveEntity(int id)
        {
            _liveEntities[id] = false;
        }

    }    

  

}
