namespace lru
{
    class LRU<Tkey, Tvalue>(int capacity)
        where Tkey : notnull
        where Tvalue : notnull
    {
        private class Entry(Tkey key, Tvalue value)
        {
            internal ulong orderKey = 0;
            internal Tvalue value = value;
            internal Tkey key = key;
        }

        private readonly int _capacity = capacity;
        private readonly SortedDictionary<ulong, Tkey> order = [];
        private readonly Dictionary<Tkey, Entry> values = [];

        public void Add(Tkey key, Tvalue value)
        {
            var entry = new Entry(key, value);
            
            var alreadyPresent = TryGet(key, ref value);
            if (alreadyPresent)
                order.Remove(entry.orderKey);
            else
                Limit(_capacity - 1);

            entry.orderKey = NextOrderKey;
            order.Add(entry.orderKey, key);
            
            if (alreadyPresent)
                values[key] = entry;
            else
                values.Add(key, entry);
        }

        private ulong NextOrderKey
        {
            get
            {
                if (Count == 0)
                    return ulong.MaxValue;
                var key = order.First().Value;
                return values[key].orderKey - 1;
            }
        }

        public bool TryDelete(Tkey key)
        {
            if (!values.TryGetValue(key, out Entry? entry))
                return false;
            values.Remove(key);
            order.Remove(entry.orderKey);
            return true;
        }

        public bool TryGet(Tkey key, ref Tvalue value)
        {
            if (!values.TryGetValue(key, out Entry? entry))
                return false;
            order.Remove(entry.orderKey);
            entry.orderKey = NextOrderKey;
            order.Add(entry.orderKey, entry.key);
            value = entry.value;
            return true;
        }

        public bool TryGet(int index, ref Tkey key, ref Tvalue value)
        {
            if (index < 0)
                return false;
            if (index >= values.Count)
                return false;
            key = order.Skip(index - 1).Take(1).First().Value;
            var entry = values[key];
            order.Remove(entry.orderKey);
            entry.orderKey = NextOrderKey;
            order.Add(entry.orderKey, key);
            value = entry.value;
            return true;
        }

        public int Count => values.Count;

        private void Limit(int count)
        {
            if (count <= 0)
                return;

            while (Count > count)
            {
                var key = order.Last().Value;
                var entry = values[key];
                order.Remove(entry.orderKey);
                values.Remove(entry.key);
            }
        }
    }

    class TestLRU
    {
        public static void Test()
        {
            var lru = new LRU<string, int>(2);
            lru.Add("apple", 10);
            lru.Add("orange", 20);
            lru.Add("banana", 30);
            int value = int.MinValue;
            string key = "";
            bool found = lru.TryGet(key="apple", ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"{key} not found");
            found = lru.TryGet(2, ref key, ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"2 not found");
            found = lru.TryGet(1, ref key, ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"1 not found");
            found = lru.TryGet(0, ref key, ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"0 not found");
            found = lru.TryGet(-1, ref key, ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"-1 not found");
            found = lru.TryGet(0, ref key, ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"0 not found");
            found = lru.TryGet(key="orange", ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"{key} not found");
            found = lru.TryGet(key="banana", ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"{key} not found");
            found = lru.TryGet(0, ref key, ref value);
            if (found) Console.WriteLine($"found: {key}={value}"); else Console.WriteLine($"0 not found");
            Console.ReadKey();
        }

        public static void Main()
        {
            Test();
        }
    }
}