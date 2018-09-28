# MurMur3 hash

----------------

An implementation of the Murmur3 hash for .NET

## Example

```csharp
const string source = "Hello world!";
string hash;

using (var murmur32 = new Murmur32())
{
    byte[] data = murmur32.ComputeHash(Encoding.UTF8.GetBytes(source));
    hash = ConvertBytesToString(data);
}
Console.WriteLine($"The SHA256 hash of '{source}' is: {hash}.");
```

## [License](./LICENSE)