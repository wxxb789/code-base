﻿// Facade Design Pattern
//
// Intent: Provides a simplified interface to a library, a framework, or any
// other complex set of classes.

namespace Structure.Facade;

using System;

// The Facade class provides a simple interface to the complex logic of one
// or several subsystems. The Facade delegates the client requests to the
// appropriate objects within the subsystem. The Facade is also responsible
// for managing their lifecycle. All of this shields the client from the
// undesired complexity of the subsystem.
public class Facade
{
    protected Subsystem1 _subsystem1;

    protected Subsystem2 _subsystem2;

    public Facade(Subsystem1 subsystem1, Subsystem2 subsystem2)
    {
        _subsystem1 = subsystem1;
        _subsystem2 = subsystem2;
    }

    // The Facade's methods are convenient shortcuts to the sophisticated
    // functionality of the subsystems. However, clients get only to a
    // fraction of a subsystem's capabilities.
    public string Operation()
    {
        string result = "Facade initializes subsystems:\n";
        result += _subsystem1.operation1();
        result += _subsystem2.operation1();
        result += "Facade orders subsystems to perform the action:\n";
        result += _subsystem1.operationN();
        result += _subsystem2.operationZ();
        return result;
    }
}

// The Subsystem can accept requests either from the facade or client
// directly. In any case, to the Subsystem, the Facade is yet another
// client, and it's not a part of the Subsystem.
public class Subsystem1
{
    public string operation1()
    {
        return "Subsystem1: Ready!\n";
    }

    public string operationN()
    {
        return "Subsystem1: Go!\n";
    }
}

// Some facades can work with multiple subsystems at the same time.
public class Subsystem2
{
    public string operation1()
    {
        return "Subsystem2: Get ready!\n";
    }

    public string operationZ()
    {
        return "Subsystem2: Fire!\n";
    }
}

internal class Client
{
    // The client code works with complex subsystems through a simple
    // interface provided by the Facade. When a facade manages the lifecycle
    // of the subsystem, the client might not even know about the existence
    // of the subsystem. This approach lets you keep the complexity under
    // control.
    public static void ClientCode(Facade facade)
    {
        Console.Write(facade.Operation());
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // The client code may have some of the subsystem's objects already
        // created. In this case, it might be worthwhile to initialize the
        // Facade with these objects instead of letting the Facade create
        // new instances.
        Subsystem1 subsystem1 = new();
        Subsystem2 subsystem2 = new();
        Facade facade = new(subsystem1, subsystem2);
        Client.ClientCode(facade);
    }
}