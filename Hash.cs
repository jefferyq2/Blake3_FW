// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace Cryptography.Blake3
{
    /// <summary>
    /// An output of the default size, 32 bytes, which provides constant-time equality checking.
    /// </summary>
    /// <remarks>
    /// This hash is returned by <see cref="Hasher.Hash(System.ReadOnlySpan{byte})"/>.
    /// This hash struct provides structural equality.
    /// </remarks>
    public unsafe struct Hash : IEquatable<Hash>
    {
        /// <summary>
        /// The size of this hash is 32 bytes.
        /// </summary>
        public const int Size = 32;
        fixed byte _bytes[Size];

        /// <summary>
        /// Copies bytes to this hash. The input data must be 32 bytes.
        /// </summary>
        /// <param name="data">A 32-byte buffer.</param>
        public void CopyFromBytes(byte[] data)
        {
            if (data.Length != 32) ThrowArgumentOutOfRange(data.Length);

            for (int i = 0; i < Size; i++)
                _bytes[i] = data[i];
        }

        /// <summary>
        /// Creates a hash from an input data that must be 32 bytes.
        /// </summary>
        /// <param name="data">A 32-byte buffer.</param>
        /// <returns>The 32-byte hash.</returns>
        public static Hash FromBytes(byte[] data)
        {
            if (data.Length != 32) ThrowArgumentOutOfRange(data.Length);
            var hash = new Hash();
            hash.CopyFromBytes(data);
            return hash;
        }

        public bool Equals(Hash other)
        {
            for (int i = 0; i < Size; i++)
                if (_bytes[i] != other._bytes[i]) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Hash other && Equals(other);
        }

        public override int GetHashCode()
        {
            int hashcode = 0;
            for (int i = 0; i < 32; i++)
                hashcode = (hashcode * 397) ^ (int)_bytes[i];
            return hashcode;
        }

        public override string ToString()
        {
            var cha = new char[Size * 2];
            int cnt = 0;
            for (int i = 0; i < Size; i++)
            {
                var b = _bytes[i];
                cha[cnt++] = Hex[(b >> 4) & 0xF];
                cha[cnt++] = Hex[b & 0xF];
            }
            return new string(cha);
        }

        public static bool operator ==(Hash left, Hash right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Hash left, Hash right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRange(int size)
        {
            throw new ArgumentOutOfRangeException("data", $"Invalid size {size} of the data. Expecting 32");
        }

        private static char[] Hex => new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
        };
    }
}