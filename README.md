Jasmine-style assertions in .NET
==================================

We in the .NET world have lacked this for a surprisingly long time.  Here is an attempt to recreate testing as fluent as that enjoyed by Javascripters (and some Java-drinkers) for years.

------------------
    
    var value = "an expression of good will";
    Action action = delegate { throw new InvalidOperationException(); };
    Expect.The(value).ToContain("of good");
    Expect.The(value).Not.ToContain("statement");
    Expect.The(action).ToThrow<InvalidOperationException>();
    Expect.The(new object()).Not.ToBeNull();

This is an attempt at a .NET port of [great-expectations](https://github.com/xian/great-expectations), keeping true both to the original implementation and to the spirit and practice of .NET development.

