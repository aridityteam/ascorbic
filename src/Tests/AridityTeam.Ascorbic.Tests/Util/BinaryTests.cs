using AridityTeam.Util;

namespace AridityTeam.Platform.Tests.Util
{
    public class BinaryTests
    {
        private const string UTF8_TEXT = "Hello, world!";

        [Fact]
        public void Binary_Encrypt()
        {
            var binary = new Binary(UTF8_TEXT);
            Assert.NotNull(binary.Value);
        }

        [Fact]
        public void Binary_Decrypt()
        {
            var binary = new Binary(UTF8_TEXT);
            Assert.NotNull(binary.Value);

            var text = binary.ToString();
            Assert.NotNull(text);
            Assert.Equal(UTF8_TEXT, text);
        }
    }
}
