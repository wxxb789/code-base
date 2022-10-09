// State Design Pattern
//
// Intent: Lets an object alter its behavior when its internal state changes. It
// appears as if the object changed its class.

namespace Behavior.State;

using System;

// The Context defines the interface of interest to clients. It also
// maintains a reference to an instance of a State subclass, which
// represents the current state of the Context.
internal class Context
{
    // A reference to the current state of the Context.
    private State _state;

    public Context(State state)
    {
        TransitionTo(state);
    }

    // The Context allows changing the State object at runtime.
    public void TransitionTo(State state)
    {
        Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
        _state = state;
        _state.SetContext(this);
    }

    // The Context delegates part of its behavior to the current State
    // object.
    public void Request1()
    {
        _state.Handle1();
    }

    public void Request2()
    {
        _state.Handle2();
    }
}

// The base State class declares methods that all Concrete State should
// implement and also provides a backreference to the Context object,
// associated with the State. This backreference can be used by States to
// transition the Context to another State.
internal abstract class State
{
    protected Context _context;

    public void SetContext(Context context)
    {
        _context = context;
    }

    public abstract void Handle1();

    public abstract void Handle2();
}

// Concrete States implement various behaviors, associated with a state of
// the Context.
internal class ConcreteStateA : State
{
    public override void Handle1()
    {
        Console.WriteLine("ConcreteStateA handles request1.");
        Console.WriteLine("ConcreteStateA wants to change the state of the context.");
        _context.TransitionTo(new ConcreteStateB());
    }

    public override void Handle2()
    {
        Console.WriteLine("ConcreteStateA handles request2.");
    }
}

internal class ConcreteStateB : State
{
    public override void Handle1()
    {
        Console.Write("ConcreteStateB handles request1.");
    }

    public override void Handle2()
    {
        Console.WriteLine("ConcreteStateB handles request2.");
        Console.WriteLine("ConcreteStateB wants to change the state of the context.");
        _context.TransitionTo(new ConcreteStateA());
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // The client code.
        Context context = new(new ConcreteStateA());
        context.Request1();
        context.Request2();
    }
}
