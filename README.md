# Blake3 FW

This project is .NET Framework wrapper for Blake3 hash implementation (with native DLL underneath for x86/x64).
Sources are adapted from repository [Blake3.NET](https://github.com/xoofx/Blake3.NET) (Blake3 for .NET Core),
see appropriate `license.txt` if mind any changes;

Blake3 is useful for fast computing of hash for emails, large text data, etc.
It is **not** recommended for passwords - use [Argon2](https://www.nuget.org/packages/Konscious.Security.Cryptography.Argon2/) hash instead.

## Quick usage

After you compiled sample project, take from `runtime` folder appropriate `blake3_dotnet.dll` (native code) and add to the binaries folder.

#### Hash a text:

```c#
using Cryptography.Blake3;
var hash = Hasher.HashUTF8("BLAKE3");
// or var hash = Hasher.Hash(Encoding.UTF32.GetBytes("BLAKE3")); if you have another encoding
Console.WriteLine(hash);
// => f890484173e516bfd935ef3d22b912dc9738de38743993cfedf2c9473b3216a4
```

#### Hash a few portions of text incrementally:

```c#
using var hasher = Hasher.New();
hasher.UpdateUTF8("BLAKE3");
hasher.UpdateUTF8("is modern hash");
var hash = hasher.Finalize();
Console.WriteLine(hash);
// => 99a35f4127b51c7826787778a59a69cb33980b7cef99260e73e8a9eca834f433
```

#### Hash a stream on the go with `Blake3Stream`:

```c#
using var blake3Stream = new Blake3Stream(new MemoryStream());
blake3Stream.Write(Encoding.UTF8.GetBytes("BLAKE3"));
var hash = blake3Stream.ComputeHash();
```

#### Sign text using a 256-bit key:

```c#
// this key is a secret and kept on both sides of conversation
var key = new byte[] { 0xB3, 0xCA, 0x11, 0x31, 0x59, 0x93, 0x62, 0x78, 0xD0, 0x92, 0x10, 0xE3, 0xF0, 0x90, 0x17, 0x26,
                       0x2D, 0x53, 0xB0, 0x55, 0x80, 0xA7, 0xC7, 0xC1, 0x01, 0xED, 0xB2, 0xD4, 0x68, 0x79, 0xA5, 0x08 };
using var blake3 = Hasher.NewKeyed(key);
blake3.UpdateUTF8("Hello, Santa!");
var signature = blake3.Finalize();
Console.WriteLine(signature);// signature sent with the text open way
// => 9eb01bdd0056b4d5f3f7b27f0c4bee181f3374d4e467ccef96cc58a176238859

var hash = Hasher.HashUTF8("Hello, Santa!");// hash the same text, but w/o key
Console.WriteLine("Unencoded hash = " + hash);
// => 7026f923845cc62805cb771487ddfeef070f0315dcad3a3a9cf06fd4c7714bd1
````
