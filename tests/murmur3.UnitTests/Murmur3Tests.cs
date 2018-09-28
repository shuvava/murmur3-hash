using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using murmur;

using Xunit;


namespace murmur3.UnitTests
{
    public class Murmur3Tests
    {
        private string ConvertBytesToString(in byte[] data)
        {
            Array.Reverse(data);
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return $"0x{sBuilder.ToString().ToUpper()}";
        }


        [Fact]
        [Trait("Category", "Unit")]
        public void Murmur32_hash_check()
        {
            //arrange
            const string input = "Hello world!";
            string output;

            //atc
            using (var hash = new Murmur32())
            {
                var data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                output = ConvertBytesToString(data);
            }

            //assert
            Assert.Equal("0x627B0C2C", output);
        }


        [Fact]
        [Trait("Category", "Performance")]
        public void Murmur32_performace_test()
        {
            //arrange
            var fileName = "dictionary_english.dic";
            var fileLines = File.ReadAllLines(fileName);
            var sw = Stopwatch.StartNew();

            //act
            using (var hash = new Murmur32())
            {
                foreach (var line in fileLines)
                {
                    hash.ComputeHash(Encoding.UTF8.GetBytes(line));
                }
            }

            sw.Stop();
            var murmurTime = sw.Elapsed;
            sw.Restart();

            using (var hash = SHA256.Create())
            {
                foreach (var line in fileLines)
                {
                    hash.ComputeHash(Encoding.UTF8.GetBytes(line));
                }
            }

            sw.Stop();
            var sha256Time = sw.Elapsed;

            //assert
            Assert.True(murmurTime < sha256Time);
        }
    }
}
