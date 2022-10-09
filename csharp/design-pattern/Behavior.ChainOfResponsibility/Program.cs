namespace Behavior.ChainOfResponsibility;
// Chain of Responsibility Design Pattern
//
// Intent: Lets you pass requests along a chain of handlers. Upon receiving a
// request, each handler decides either to process the request or to pass it to
// the next handler in the chain.

using System;
using System.Collections.Generic;

// The Handler interface declares a method for building the chain of
// handlers. It also declares a method for executing a request.
public interface IHandler
{
    IHandler SetNext(IHandler handler);

    object Handle(object request);
}

// The default chaining behavior can be implemented inside a base handler
// class.
internal abstract class AbstractHandler : IHandler
{
    private IHandler _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;

        // Returning a handler from here will let us link handlers in a
        // convenient way like this:
        // monkey.SetNext(squirrel).SetNext(dog);
        return handler;
    }

    public virtual object Handle(object request)
    {
        if (_nextHandler != null)
            return _nextHandler.Handle(request);

        return null;
    }
}

internal class MonkeyHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if (request as string == "Banana")
            return $"Monkey: I'll eat the {request}.\n";

        return base.Handle(request);
    }
}

internal class SquirrelHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if (request.ToString() == "Nut")
            return $"Squirrel: I'll eat the {request}.\n";

        return base.Handle(request);
    }
}

internal class DogHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if (request.ToString() == "MeatBall")
            return $"Dog: I'll eat the {request}.\n";

        return base.Handle(request);
    }
}

internal class Client
{
    // The client code is usually suited to work with a single handler. In
    // most cases, it is not even aware that the handler is part of a chain.
    public static void ClientCode(AbstractHandler handler)
    {
        foreach (string food in new List<string> { "Nut", "Banana", "Cup of coffee" })
        {
            Console.WriteLine($"Client: Who wants a {food}?");

            object? result = handler.Handle(food);

            if (result != null)
                Console.Write($"   {result}");
            else
                Console.WriteLine($"   {food} was left untouched.");
        }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // The other part of the client code constructs the actual chain.
        MonkeyHandler monkey = new();
        SquirrelHandler squirrel = new();
        DogHandler dog = new();

        monkey.SetNext(squirrel).SetNext(dog);

        // The client should be able to send a request to any handler, not
        // just the first one in the chain.
        Console.WriteLine("Chain: Monkey > Squirrel > Dog\n");
        Client.ClientCode(monkey);
        Console.WriteLine();

        Console.WriteLine("Sub chain: Squirrel > Dog\n");
        Client.ClientCode(squirrel);
    }
}
