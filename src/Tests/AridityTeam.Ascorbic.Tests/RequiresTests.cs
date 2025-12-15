using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;

namespace AridityTeam.Ascorbic.Tests
{
    public class RequiresTests
    {
        [Fact]
        public void NotNull_ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => Requires.NotNull((object)null!, "foo"));
            Requires.NotNull(new object(), "foo");
        }

        [Fact]
        public void NotNull_ThrowsOnNull_CallerArgumentExpression()
        {
            string? foo = null;
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(foo!));
            Assert.Equal("foo", ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => Requires.NotNull(foo!, null));
            Assert.Null(ex.ParamName);
        }

        [Fact]
        public void NotNull_IntPtr_ThrowsOnZero()
        {
            Assert.Throws<ArgumentNullException>(() => Requires.NotNull(IntPtr.Zero, "foo"));
            Requires.NotNull(new IntPtr(5), "foo");
        }

        [Fact]
        public async Task NotNull_Task_ThrowsOnNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => Requires.NotNull((Task)null!, "foo"));
            await Requires.NotNull((Task)Task.FromResult(0), "foo");
        }

        [Fact]
        public async Task NotNull_TaskOfT_ThrowsOnNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => Requires.NotNull((Task<int>)null!, "foo"));
            await Requires.NotNull(Task.FromResult(0), "foo");
        }

        [Fact]
        public void NotNullAllowStructs()
        {
            Requires.NotNullAllowStructs(0, "paramName");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullAllowStructs((object?)null, "paramName"));
        }

        [Fact]
        public void NotNullOrEmpty()
        {
            Requires.NotNullOrEmpty("not empty", "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrEmpty(null!, "paramName"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(string.Empty, "paramName"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty("\0", "paramName"));
            ArgumentException ex = Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(string.Empty, null));
            Assert.Null(ex.ParamName);
        }

        [Fact]
        public void NotNullOrWhitespace()
        {
            Requires.NotNullOrEmpty("not empty", "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrWhiteSpace(null!, "paramName"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrWhiteSpace(string.Empty, "paramName"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrWhiteSpace("\0", "paramName"));
            ArgumentException ex = Assert.Throws<ArgumentException>(() => Requires.NotNullOrWhiteSpace(" \t\n\r ", "paramName"));
            Assert.Equal("paramName", ex.ParamName);
        }

        [Fact]
        public void NotNullOrEmpty_Enumerable()
        {
            System.Collections.IEnumerable? nullCollection = null;
            System.Collections.IEnumerable emptyCollection = Array.Empty<string>();
            System.Collections.IEnumerable collection = new[] { "hi" };
            Requires.NotNullOrEmpty(collection, "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrEmpty(nullCollection!, "param"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(emptyCollection, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_EnumerableOfT_Class()
        {
            IEnumerable<string>? nullCollection = null;
            IEnumerable<string> emptyCollection = Array.Empty<string>();
            IEnumerable<string> collection = new[] { "hi" };
            Requires.NotNullOrEmpty(collection, "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrEmpty(nullCollection!, "param"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(emptyCollection, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_EnumerableOfT_Struct()
        {
            IEnumerable<int>? nullCollection = null;
            IEnumerable<int> emptyCollection = Array.Empty<int>();
            IEnumerable<int> collection = new[] { 5 };
            Requires.NotNullOrEmpty(collection, "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrEmpty(nullCollection!, "param"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(emptyCollection, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_EnumerableOfT_AlsoImplementsICollectionOfT()
        {
            // Mock type that implements both IEnumerable<T> and ICollection<T>
            var collection = new Mock<ICollection<string>>(MockBehavior.Strict);
            Mock<IEnumerable<string>> enumerable = collection.As<IEnumerable<string>>();

            enumerable.Setup(m => m.GetEnumerator()).Throws(new Exception("Should not call GetEnumerator."));

            collection.SetupGet(m => m.Count).Returns(0);
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(enumerable.Object, "param"));

            collection.SetupGet(m => m.Count).Returns(1);
            Requires.NotNullOrEmpty(enumerable.Object, "param");
        }

        [Fact]
        public void NotNullOrEmpty_EnumerableOfT_AlsoImplementsIReadOnlyCollectionOfT()
        {
            // Mock type that implements both IEnumerable<T> and IReadOnlyCollection<T>
            var collection = new Mock<IReadOnlyCollection<string>>(MockBehavior.Strict);
            Mock<IEnumerable<string>> enumerable = collection.As<IEnumerable<string>>();

            enumerable.Setup(m => m.GetEnumerator()).Throws(new Exception("Should not call GetEnumerator."));

            collection.SetupGet(m => m.Count).Returns(0);
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(enumerable.Object, "param"));

            collection.SetupGet(m => m.Count).Returns(1);
            Requires.NotNullOrEmpty(enumerable.Object, "param");
        }

        [Fact]
        public void NotNullOrEmpty_CollectionOfT_Class()
        {
            ICollection<string>? nullCollection = null;
            ICollection<string> emptyCollection = Array.Empty<string>();
            ICollection<string> collection = new[] { "hi" };
            Requires.NotNullOrEmpty(collection, "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrEmpty(nullCollection!, "param"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(emptyCollection, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_CollectionOfT_Struct()
        {
            ICollection<int>? nullCollection = null;
            ICollection<int> emptyCollection = Array.Empty<int>();
            ICollection<int> collection = new[] { 5 };
            Requires.NotNullOrEmpty(collection, "param");
            Assert.Throws<ArgumentNullException>(() => Requires.NotNullOrEmpty(nullCollection!, "param"));
            Assert.Throws<ArgumentException>(() => Requires.NotNullOrEmpty(emptyCollection, "param"));
        }
    }
}
