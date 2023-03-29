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
    private LinkedListNode _first;

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