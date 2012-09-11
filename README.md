Jasmine-style assertions in .NET
==================================

We in the .NET world have lacked this for a surprisingly long time.  Here is an attempt to recreate testing as fluent as that enjoyed by Javascripters (and some Java-drinkers) for years.

------------------
    
    Expect.The(stringValue).Not.ToBeNullOrEmpty();
    Expect.The(method).ToThrow<DivideByZeroException>();
    Expect.The(sqlConnection).ToBeOpen();
    Expect.The(listOfStuff).ToContainInOrder("foo", "bar", "qux");

FAQ
========================

How do I use this?
------------------------
You can use SharpExpect with all of your favorite test runners, from nUnit to xUnit.  Reference SharpExpect.dll, use the SharpExpect namespace, and test away.

How does it work?
------------------------
Generics, runtime code generation, and magic - it's a little messy, but feel free to take a look.

My tests are getting long.  Can I write expectations for my own objects?
---------------------------
SharpExpect is easy to extend - just write your own matcher!  Generic constraints aside, it's quite easy:

    public class FooMatcher : BaseMatcher<Foo, FooMatcher>
    {
        public virtual bool ToHaveFourBars()
        {
            return actual.GetBars().Count == 4;
        }
    }
    
Create your own subclass of Expect and add your new matcher:

    public static class Expect : SharpExpect.Expect
    {
        public static FooMatcher The(Foo expected)
        {
            return SharpExpect.ClassWrapper.Wrap<Foo, FooMatcher>(expected);
        }
    }
    
Now your domain validation is expressed nicely, in one place.

Known Issues
================================
While SharpExpect works admirably on the CLR, matchers of value-types cause segfaults in Mono, verified on OS X 10.7.4.

Credit Where Credit Is Due
================================
This is an attempt at a .NET port of [great-expectations](https://github.com/xian/great-expectations), keeping true both to the original implementation and to the spirit and practice of .NET development.