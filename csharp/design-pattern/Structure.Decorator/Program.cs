// Decorator Design Pattern
//
// Intent: Lets you attach new behaviors to objects by placing these objects
// inside special wrapper objects that contain the behaviors.

namespace Structure.Decorator;

using System;

// The base Component interface defines operations that can be altered by
// decorators.
public abstract class Component
{
    public abstract string Operation();
}

// Concrete Components provide default implementations of the operations.
// There might be several variations of these classes.
internal class ConcreteComponent : Component
{
    public override string Operation()
    {
        return "ConcreteComponent";
    }
}

// The base Decorator class follows the same interface as the other
// components. The primary purpose of this class is to define the wrapping
// interface for all concrete decorators. The default implementation of the
// wrapping code might include a field for storing a wrapped component and
// the means to initialize it.
internal abstract class Decorator : Component
{
    protected Component _component;

    public Decorator(Component component)
    {
        _component = component;
    }

    public void SetComponent(Component component)
    {
        _component = component;
    }

    // The Decorator delegates all work to the wrapped component.
    public override string Operation()
    {
        if (_component != null)
            return _component.Operation();

        return string.Empty;
    }
}

// Concrete Decorators call the wrapped object and alter its result in some
// way.
internal class ConcreteDecoratorA : Decorator
{
    public ConcreteDecoratorA(Component comp) : base(comp)
    {
    }

    // Decorators may call parent implementation of the operation, instead
    // of calling the wrapped object directly. This approach simplifies
    // extension of decorator classes.
    public override string Operation()
    {
        return $"ConcreteDecoratorA({base.Operation()})";
    }
}

// Decorators can execute their behavior either before or after the call to
// a wrapped object.
internal class ConcreteDecoratorB : Decorator
{
    public ConcreteDecoratorB(Component comp) : base(comp)
    {
    }

    public override string Operation()
    {
        return $"ConcreteDecoratorB({base.Operation()})";
    }
}

public class Client
{
    // The client code works with all objects using the Component interface.
    // This way it can stay independent of the concrete classes of
    // components it works with.
    public void ClientCode(Component component)
    {
        Console.WriteLine("RESULT: " + component.Operation());
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Client client = new();

        ConcreteComponent simple = new();
        Console.WriteLine("Client: I get a simple component:");
        client.ClientCode(simple);
        Console.WriteLine();

        // ...as well as decorated ones.
        //
        // Note how decorators can wrap not only simple components but the
        // other decorators as well.
        ConcreteDecoratorA decorator1 = new(simple);
        ConcreteDecoratorB decorator2 = new(decorator1);
        Console.WriteLine("Client: Now I've got a decorated component:");
        client.ClientCode(decorator2);
    }
}
