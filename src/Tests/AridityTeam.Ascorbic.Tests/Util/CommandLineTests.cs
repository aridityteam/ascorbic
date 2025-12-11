using System;

using AridityTeam.Util;

namespace AridityTeam.Platform.Tests.Util
{
    public class CommandLineTests
    {
        [Fact]
        public void CommandLine_ParseArgs()
        {
            var parser = new CommandLine(["Foo", "Bar"]);
            var args = parser.ToString();
            Assert.NotNull(args);
        }

        [Fact]
        public void CommandLine_ParseArgs_FindParm()
        {
            var parser = new CommandLine(["Foo", "Bar"]);
            var args = parser.ToString();
            Assert.NotNull(args);

            Assert.True(parser.FindParm("foo"));
        }

        [Fact]
        public void CommandLine_ParseArgs_GetParm()
        {
            var parser = new CommandLine(["Foo=Hello", "Bar=World"]);
            var args = parser.ToString();
            Assert.NotNull(args);

            Assert.True(parser.FindParm("foo"));

            var parm = parser.GetParm("foo");
            Assert.NotNull(parm);
        }
    }
}
