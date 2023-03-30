﻿static void Main()
{
    StringsDictionary dictionary = new StringsDictionary();
    foreach (var line in File.ReadAllLines("dict.txt"))
    {
        string[] elements = line.Split("; ");
        string key = elements[0];
        string value = String.Join("; ", elements[1..]);
        dictionary.Add(key, value);
    }
    while (true)
    {
        Console.Write("Write a word or /break to stop: ");
        string user_key = Console.ReadLine();
        if (user_key == "/break")
        {
            break;
        }
        string user_value = dictionary.GetValue(user_key);
        Console.WriteLine(user_value);
    }
}

Main();


public class KeyValuePair
{
    public string Key { get; }
    public string Value { get; }

    public KeyValuePair(string key, string value)
    {
        Key = key;
        Value = value;
    }
}

public class LinkedListNode
{
    public KeyValuePair Pair { get; }
    public LinkedListNode Next { get; set; }

    public LinkedListNode(KeyValuePair pair, LinkedListNode next = null)
    {
        Pair = pair;
        Next = next;
    }
}

public class LinkedList
{
    public LinkedListNode _first;

    public void Add(KeyValuePair pair)
    {
        _first = new LinkedListNode(pair, _first);
    }

    public void RemoveByKey(string key)
    {
        LinkedListNode current = _first, previous = null;

        while (current != null && current.Pair.Key != key)
        {
            previous = current;
            current = current.Next;
        }

        if (current != null)
        {
            if (previous == null) _first = current.Next;
            else previous.Next = current.Next;
        }
    }

    public KeyValuePair? GetItemWithKey(string key)
    {
        LinkedListNode current = _first;

        while (current != null && current.Pair.Key != key)
        {
            current = current.Next;
        }

        return current?.Pair;
    }
}

public class StringsDictionary
{
    private const int InitialSize = 10;
    int newSize = 10;
    private int count;
    private LinkedList[] buckets = new LinkedList[InitialSize];

    public int GetHashCode(string key)
    {
        int hash = 0;
        int d = 1;
        foreach (char c in key)
        {
            hash += (int) (c+Math.Pow(d,2));
            d++;
            
        }

        return hash;
    }

    public string GetValue(string key)
    {
        int bucketIndex = Index(key);
        if (buckets[bucketIndex] != null)
        {
            var kvp = buckets[bucketIndex].GetItemWithKey(key);
            if (kvp != null)
            {
                return kvp.Value;
            }
        }

        return null;
    }

    public void Add(string key, string value)
    {
        int bucketIndex = Index(key);
        if (buckets[bucketIndex] == null)
        {
            buckets[bucketIndex] = new LinkedList();
            
        }
        buckets[bucketIndex].Add(new KeyValuePair(key,value));
        double loadFactor = LoadFactor();
        if (loadFactor > 0.8)
        {
            Resize();
        }
    }

    public void Remove(string key)
    {
        int bucketIndex = Index(key);
        if (buckets[bucketIndex] != null)
        {
            buckets[bucketIndex].RemoveByKey(key);
        }
    }

    int Index(string key)
    {
        int hash = GetHashCode(key);
        int bucketIndex = hash % InitialSize;
        return bucketIndex;
    }
    public double LoadFactor()
    {
        double count = 0;
        foreach (var list in buckets)
        {
            if (list != null)
            {
                count += 1;
            }
        }
        return count / newSize;
    }
    private void Resize()
    {
        newSize = newSize * 2;
        var newBuckets = new LinkedList[newSize];

        foreach (var linkedlist in buckets)
        {
            if (linkedlist != null)
            {
                var currentNode = linkedlist._first;
                while (currentNode != null)
                {
                    var pair = currentNode.Pair;
                    int bucketIndex = GetHashCode(pair.Key) % newSize;

                    if (newBuckets[bucketIndex] == null)
                    {
                        newBuckets[bucketIndex] = new LinkedList();
                    }

                    newBuckets[bucketIndex].Add(pair);
                    currentNode = currentNode.Next;
                }
            }
        }

        buckets = newBuckets;
    }
}