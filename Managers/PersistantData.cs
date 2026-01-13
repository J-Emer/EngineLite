using System.Runtime.CompilerServices;

namespace EngineLite.Managers
{
    public static class PersistantData
    {
        private static Dictionary<string, object> _data = new Dictionary<string, object>();

        public static void Add(string key, object value)
        {
            if(_data.ContainsKey(key))
            {
                _data[key] = value;
                return;
            }

            _data.Add(key, value);
        }
        public static void Remove(string key)
        {
            _data.Remove(key);
        }
        public static object Find(string key)
        {
            if(_data.ContainsKey(key))
            {
                return _data[key];
            }

            return null;
        }     
        public static void Clear()
        {
            _data.Clear();
        }
        public static void Show()
        {
            foreach (var item in _data)
            {
                Console.WriteLine($"Key: {item.Key} | Value: {item.Value}");
            }
        }

    }
}