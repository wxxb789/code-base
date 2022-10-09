// Iterator Design Pattern
//
// Intent: Lets you traverse elements of a collection without exposing its
// underlying representation (list, stack, tree, etc.).

namespace Behavior.Iterator;

using System;
using System.Collections;
using System.Collections.Generic;

internal abstract class Iterator : IEnumerator
{
    object IEnumerator.Current => Current();

    // Returns the key of the current element
    public abstract int Key();

    // Returns the current element
    public abstract object Current();

    // Move forward to next element
    public abstract bool MoveNext();

    // Rewinds the Iterator to the first element
    public abstract void Reset();
}

internal abstract class IteratorAggregate : IEnumerable
{
    // Returns an Iterator or another IteratorAggregate for the implementing
    // object.
    public abstract IEnumerator GetEnumerator();
}

// Concrete Iterators implement various traversal algorithms. These classes
// store the current traversal position at all times.
internal class AlphabeticalOrderIterator : Iterator
{
    private readonly WordsCollection _collection;

    // Stores the current traversal position. An iterator may have a lot of
    // other fields for storing iteration state, especially when it is
    // supposed to work with a particular kind of collection.
    private int _position = -1;

    private readonly bool _reverse;

    public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
    {
        _collection = collection;
        _reverse = reverse;

        if (reverse)
            _position = collection.GetItems().Count;
    }

    public override object Current()
    {
        return _collection.GetItems()[_position];
    }

    public override int Key()
    {
        return _position;
    }

    public override bool MoveNext()
    {
        int updatedPosition = _position + (_reverse ? -1 : 1);

        if (updatedPosition >= 0 && updatedPosition < _collection.GetItems().Count)
        {
            _position = updatedPosition;
            return true;
        }
        return false;
    }

    public override void Reset()
    {
        _position = _reverse ? _collection.GetItems().Count - 1 : 0;
    }
}

// Concrete Collections provide one or several methods for retrieving fresh
// iterator instances, compatible with the collection class.
internal class WordsCollection : IteratorAggregate
{
    private readonly List<string> _collection = new();

    private bool _direction;

    public void ReverseDirection()
    {
        _direction = !_direction;
    }

    public List<string> GetItems()
    {
        return _collection;
    }

    public void AddItem(string item)
    {
        _collection.Add(item);
    }

    public override IEnumerator GetEnumerator()
    {
        return new AlphabeticalOrderIterator(this, _direction);
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // The client code may or may not know about the Concrete Iterator
        // or Collection classes, depending on the level of indirection you
        // want to keep in your program.
        WordsCollection collection = new();
        collection.AddItem("First");
        collection.AddItem("Second");
        collection.AddItem("Third");

        Console.WriteLine("Straight traversal:");

        foreach (object? element in collection)
            Console.WriteLine(element);

        Console.WriteLine("\nReverse traversal:");

        collection.ReverseDirection();

        foreach (object? element in collection)
            Console.WriteLine(element);
    }
}
