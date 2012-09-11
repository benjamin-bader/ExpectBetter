Jasmine-style assertions in .NET
==================================

We in the .NET world have lacked this for a surprisingly long time, and honestly should expect a better testing experience.  Here is an attempt to recreate testing as fluent as that enjoyed by Javascripters (and some Java-drinkers) for years.

------------------
    
    Expect.The(stringValue).Not.ToBeNullOrEmpty();
    Expect.The(method).ToThrow<DivideByZeroException>();
    Expect.The(sqlConnection).ToBeOpen();
    Expect.The(listOfStuff).ToContainInOrder("foo", "bar", "qux");

FAQ
========================

How do I use this?
------------------------
You can use SharpExpect with all of your favorite test runners, from nUnit to xUnit.  Reference ExpectBetter.dll, use the ExpectBetter namespace, and test away.

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

    public static class Expect : ExpectBetter.Expect
    {
        public static FooMatcher The(Foo expected)
        {
            return ExpectBetter.ClassWrapper.Wrap<Foo, FooMatcher>(expected);
        }
    }
    
Now your domain validation is expressed nicely, in one place.

Known Issues
================================
While ExpectBetter works as expected on the CLR, matchers of value-types cause segfaults and InvalidProgramExceptions in Mono, verified on OS X 10.7.4.

Next Steps
================================
A NuGet package is in the works, as soon as I get some time on my Windows box.
Spies!  For interfaces and virtual methods, at least.

Credit Where Credit Is Due
================================
This is an attempt at a .NET port of [jasmine](http://pivotal.github.com/jasmine/) and [great-expectations](https://github.com/xian/great-expectations), keeping true to the finesse of Jasmine, the Java implementation of great-expectations, and to the spirit and practice of .NET development.
