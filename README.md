Expect Better
==================================
### Better Unit Testing for .NET
Would you like your assertions to be shorter and more expressive?  NUnit gets us far, but Assert is limited in what it can express - you can't add to it.  xUnit gets us a little farther, but has the same problem.  We can do better:

```csharp
Expect.The(stringValue).Not.ToBeNullOrEmpty();
Expect.The(method).ToThrow<DivideByZeroException>();
Expect.The(response).ToBeBadRequest();
Expect.The(listOfStuff).ToContainInOrder("foo", "bar", "qux");
```

ExpectBetter allows you to write tests that say what you mean while still using the frameworks you already know.  Taking a cue from great-expectations and Hamcrest, ExpectBetter lets you compose matchers for the types you own.  What's more, matchers both built-in and bespoke are always at your fingertips via type inferencing and IDE autocompletion - you'll never again need to resort to documentation to discover assertions.

Example
========================

Here's what ExpectBetter looks like when used in an NUnit fixture (excerpted from this project):

```csharp
using System;

using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Matchers;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class StringMatcherTest
    {
        string actual;
        StringMatcher matcher;

        [SetUp]
        public void Setup()
        {
            actual = Factory.RandomString(20, 5);
            matcher = Expect.The(actual);
        }

        [Test]
        public void ToContain_WhenExpectedIsContained_ReturnsTrue()
        {
            var expectedLen = Math.Min(actual.Length / 2 + 1, actual.Length);
            var expected = actual.Substring(expectedLen);
            var result = matcher.ToContain(expected);

            Expect.The(result).ToBeTrue();
        }
    }
}
```

Get Started
========================
If you use NuGet: `Install-Package ExpectBetter` in the Package Management Console for your test projects.

If you don't use NuGet, please consider using it.  After your consideration, you can check out this repository and build it.

If you'd rather not build it, you can download the library itself here at the Github project site:
* https://github.com/downloads/benjamin-bader/ExpectBetter/ExpectBetter.dll

Once you have the library, add a reference to `ExpectBetter.dll` to your test projects, add the ExpectBetter namespace, and get to testing!

Expectations
========================
In ExpectBetter, assertions are framed as an expectation - you expect that an object has certain properties.  If that expectation is not met, an `ExpectationException` informs you of the fact.

Any expectation you can state has a logical negation - if you can say "expect the list to be null", you can just as easily say "expect the list *not* to be null."  Here's what that looks like in ExpectBetter:

```csharp
object nullObj = null,
       otherObj = new object();

Expect.The(nullObj).ToBeNull();
Expect.The(otherObj).Not.ToBeNull();
```

Your First Tests
========================
ExpectBetter works seamlessly with your existing unit test framework.  "Arrange" and "Act" as you normally would, but instead of asserting... expect!

```csharp
// Arrange
var dict = new Dictionary<string, string>();

// Act
dict["foo"] = "bar";
dict["baz"] = "quux";

// Expect
Expect.The(dict).Not.ToBeEmpty();
```

There are built-in matchers for many common data types, including IEnumerable<T>, ICollection<T>, IDictionary<K, V>, DateTime, string, numeric types, and many more; auto-completing IDEs will show you all applicable test methods based on type inference.

Writing Tests
========================
Add ExpectBetter assertions to your current unit test framework - in NUnit, for example:

```csharp
using ExpectBetter;

[TestFixture]
public class Tests
{
    private FileInfo file;
    
    // some setup
    
    [Test]
    public void TestFileExists()
    {
        Expect.The(file).ToExist();
    }
    
    [Test]
    public void TestFileIsNotEmpty()
    {
        var contents = file.ReadAllText();
        Expect.The(contents).Not.ToBeNullOrEmpty();
    }
}
```
    
Writing Matchers
========================
Matchers are simple to write.  The only restrictions are that they must inherit from `ExpectBetter.BaseMatcher<T,M>`, and their test methods need to be virtual and return bools.  Other than that, your imagination is the limit.

```csharp
public class FileInfoMatcher : BaseMatcher<FileInfo, FileMatcher>
{
    public virtual bool ToExist()
    {
        return actual.Exists;
    }
    
    public virtual bool ToBeEmpty()
    {
        return actual.Length == 0;
    }

    public virtual bool ToHaveBeenModifiedSince(DateTime expected)
    {
        actual.Refresh();
        
        if (expected.Kind == DateTimeKind.Utc)
        {
            return actual.LastWriteTimeUtc > expected;
        }

        return actual.LastWriteTime > expected;
    }
}
```

There's some boilerplate to write, unfortunately - you'll need to subclass `ExpectBetter.Expect`, and add a static method that returns your new matchers wrapped in some magic.

```csharp
public class Expect : ExpectBetter.Expect
{
    public static FileInfoMatcher The(FileInfo actual)
    {
        return Expectations.Wrap<FileInfo, FileInfoMatcher>(actual);
    }
}
```

Now use your new `Expect` class instead of `ExpectBetter.Expect`, and your matcher will be available with all the rest in your tests.

Customizing Messages
========================

The default error message is generated from the name of your test method; it follows this pattern: "Expected [actual] [method] [expected]".  For example, if `ToHaveBeenModifiedSince` returns false, the error message would be: `Failure: Expected [System.FileInfo] to have been modified since [2012-01-01]`.  Descriptions of actual and expected values are generated with a call to `.ToString()`.  By default, .  That `System.FileInfo` part isn't so informative - what if you want a better message?  Set the `BaseMatcher<T, M>.actualDescription` field.

Let's try:

```csharp
public virtual bool ToHaveBeenModifiedSince(DateTime expected)
{
    this.actualDescription = actual.FullName;
    // as above
}
```

Now the failure message might read: `Failure: Expected [C:\autoexec.bat] to have been modified since [2012-01-01].`

You can also customize the "expected" description by setting the `expectedDescription` field.  Should you want to provide your own message entirely, the `failureMessage` field allows you to override all message generation.

FAQ
========================

How does it work?
------------------------
Generics, runtime code generation, and magic - it's a little messy, but feel free to take a look.

Basically, the mechanics of assertions are the same no matter what the language or framework - check a condition, then report an error if the condition doesn't hold.  These mechanics are provided at runtime by a generated wrapper class around each matcher, the result of a call to `Expectations.Wrap<TActual, TMatcher>(TActual actual)`.  The behavior of generated wrappers is straightforward - it implements a null check for actual, invokes the test method, checks the result, and perhaps constructs a meaningful failure message and raises an error.  This is accomplished using the built-in `System.Reflection.Emit` facility - there is no other dependency incurred.

This code requires sufficient .NET privileges to define and run dynamic assemblies; if your test environment has tighter constraints, you may encounter problems.

What if null is a legal value for my object?
------------------------
ExpectBetter is opinionated and dislikes `null`; it would much prefer that some form of `Option<T>` be used instead.  Nevertheless it respects that others feel differently, and so you can add an `[AllowNullActual]` attribute to your test methods.  With this in place, you will no longer get failures when your tested value is `null`.

Known Issues
================================
While ExpectBetter works as expected on the CLR, matchers of value-types cause segfaults and InvalidProgramExceptions in Mono, verified on OS X 10.7.4.

Next Steps
================================
* Spies!  For interfaces and virtual methods, at least.
* Figure out the mono story.
* Better integration with test frameworks - specify exception type thrown, etc.

Credit Where Credit Is Due
================================
This is an attempt at a .NET port of [jasmine](http://pivotal.github.com/jasmine/) and [great-expectations](https://github.com/xian/great-expectations), keeping true to the expressiveness of Jasmine, the generics finesse of great-expectations, and to the spirit and practice of .NET development.
