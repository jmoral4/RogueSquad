using RogueSquad.Core.Components;
using System.Collections.Generic;

namespace RogueSquad.Core
{
    public class RogueEntity {
        public int ID { get; set; }               
        List<IRogueComponent> _components;

        public RogueEntity()
        {
            _components = new List<IRogueComponent>();
        }

        public void AddComponent(IRogueComponent component)
        {
            _components.Add(component);
        }

        public bool HasComponent(ComponentTypes type)
        {
            foreach (var component in _components)
            {
                if (component.ComponentType == type)
                    return true;
            }
            return false;
        }

        public IEnumerable<IRogueComponent> GetComponents()
        {
            return _components;
        }

        public IEnumerable<IRogueComponent> GetComponentsByType(ComponentTypes type)
        {
            foreach (var component in _components)
                if (component.ComponentType == type)
                    yield return component;
        }


        public IRogueComponent GetComponentByType(ComponentTypes type)
        {
            foreach (var component in _components)
                if (component.ComponentType == type)
                    return component;
            return null;
        }

        public static RogueEntity CreateNew()
        {
            return new RogueEntity { ID = Engine.Instance.CreateUniqueEntityId() };
        }
       
    }

  

}
