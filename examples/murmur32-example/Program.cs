using System;
using System.Text;

using murmur;


namespace murmur32_example
{
    class Program
    {
        static void Main(string[] args)
        {
            const string source = "Hello world!";
            string hash;

            using (var murmur32 = new Murmur32())
            {
                var data = murmur32.ComputeHash(Encoding.UTF8.GetBytes(source));
                hash = ConvertBytesToString(data);
            }
            Console.WriteLine($"The SHA256 hash of '{source}' is: {hash}.");
        }

        private static string ConvertBytesToString(in byte[] data)
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
    }
}
