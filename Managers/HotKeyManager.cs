using EngineLite.Core;
using Microsoft.Xna.Framework.Input;

namespace EngineLite.Managers
{
    public class HotKeyManager
    {
        private Dictionary<Keys, Action> events = new Dictionary<Keys, Action>();


        public void Add(Keys key, Action callback)
        {
            if(events.ContainsKey(key))
            {
                throw new Exception("Cannot have multiple hot keys with the same key");
            }

            events.Add(key, callback);
        }

        public void Update()
        {
            foreach (var item in events)
            {
                if(Input.GetKeyDown(item.Key))
                {
                    item.Value?.Invoke();
                }
            }
        }
    }
}