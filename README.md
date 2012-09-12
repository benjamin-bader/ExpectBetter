Expect Better
==================================
## Better Unit Testing for .NET
    
    Expect.The(stringValue).Not.ToBeNullOrEmpty();
    Expect.The(method).ToThrow<DivideByZeroException>();
    Expect.The(response).ToBeBadRequest();
    Expect.The(listOfStuff).ToContainInOrder("foo", "bar", "qux");

NUnit gets us far, but Assert is limited in what it can express - you can't add to it.  xUnit gets us a little farther, but has the same problem.  We can do better.

ExpectBetter allows you to write tests that say what you mean while still using the frameworks you already know.  Taking a cue from great-expectations and Hamcrest, ExpectBetter lets you compose matchers for the types you own.  What's more, matchers both built-in and bespoke are always at your fingertips via type inferencing and IDE autocompletion.

Writing Tests
========================
Add ExpectBetter assertions to your current unit test framework - in NUnit, for example:

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
    
Writing Matchers
========================
Matchers are simple to write.  The only restrictions are that they must inherit from `ExpectBetter.BaseMatcher<T,M>`, and their test methods need to be virtual and return bools.  Other than that, your imagination is the limit.

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
    }
    
There's some boilerplate to write, unfortunately - you'll need to subclass `ExpectBetter.Expect`, and add a static method that returns your new matchers wrapped in some magic.

    public class Expect : ExpectBetter.Expect
    {
        public static FileInfoMatcher The(FileInfo actual)
        {
            return ExpectBetter.ClassWrapper.Wrap<FileInfo, FileInfoMatcher>(actual);
        }
    }

Now use your new `Expect` class instead of `ExpectBetter.Expect`, and your matcher will be available with all the rest in your tests.

FAQ
========================

How does it work?
------------------------
Generics, runtime code generation, and magic - it's a little messy, but feel free to take a look.
    
Can I change the error messages?
----------------------------------------
By default, error messages use `.ToString()` on your expected and actual values.  If you want something better, your matcher method can set the `actualDescription` and `expectedDescription` fields.

Known Issues
================================
While ExpectBetter works as expected on the CLR, matchers of value-types cause segfaults and InvalidProgramExceptions in Mono, verified on OS X 10.7.4.

Next Steps
================================
* Fully-customizable error messages
* A NuGet package is in the works, as soon as I get some time on my Windows box.
* Spies!  For interfaces and virtual methods, at least.

Credit Where Credit Is Due
================================
This is an attempt at a .NET port of [jasmine](http://pivotal.github.com/jasmine/) and [great-expectations](https://github.com/xian/great-expectations), keeping true to the expressiveness of Jasmine, the generics finesse of great-expectations, and to the spirit and practice of .NET development.
