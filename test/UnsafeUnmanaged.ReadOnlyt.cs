using System;
using Xunit;
using Xunit.Extensions;
using Xunit.Abstractions;

using EnsafedUnsafe;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static test.Platform;

namespace test
{
    public unsafe partial class UnsafeExtraTest
    {
        [Fact]
        public void ElementOffsetReadOnly()
        {
            Assert.Equal(10, UnsafeUnmanaged.ElementOffsetReadOnly(int100[10], int100[20]));
            Assert.Equal(-10, UnsafeUnmanaged.ElementOffsetReadOnly(int100[20], int100[10]));
        }
        [Fact]
        public void IsAddressGeqReadOnly()
        {
            Assert.True(UnsafeUnmanaged.IsAddressGeqReadOnly(int100[1], int100[0]));
            Assert.True(UnsafeUnmanaged.IsAddressGeqReadOnly(int100[0], int100[0]));
            Assert.False(UnsafeUnmanaged.IsAddressGeqReadOnly(int100[0], int100[1]));
        }
        [Fact]
        public void IsAddressLeqReadOnly()
        {
            Assert.False(UnsafeUnmanaged.IsAddressLeqReadOnly(int100[1], int100[0]));
            Assert.True(UnsafeUnmanaged.IsAddressLeqReadOnly(int100[0], int100[0]));
            Assert.True(UnsafeUnmanaged.IsAddressLeqReadOnly(int100[0], int100[1]));
        }
    }
}
