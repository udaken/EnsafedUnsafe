using System;
using Xunit;
using Xunit.Extensions;
using Xunit.Abstractions;

using SafelyUnsafe;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using static test.Platform;

namespace test
{
    public unsafe partial class UnsafeExtraTest
    {
        [Fact]
        public void ByteOffset()
        {
            Assert.Equal(new IntPtr(0x10010), UnsafeUnmanaged.ByteOffset(MakePtr<double>(0xFFE0), MakePtr<double>(0x1FFF0)));
            Assert.Equal(new IntPtr(-0x10000 + 16), UnsafeUnmanaged.ByteOffset(MakePtr<double>(0x1FFE0), MakePtr<double>(0xFFF0)));
        }

        [Fact]
        public void ElementOffsetPtr()
        {
            Assert.Equal(0x10000, UnsafeUnmanaged.ElementOffset(MakePtr<double>(0xFFF0 * sizeof(double)), MakePtr<double>(0x1FFF0 * sizeof(double))));
            Assert.Equal(-0x10000, UnsafeUnmanaged.ElementOffset(MakePtr<double>(0x1FFF0 * sizeof(double)), MakePtr<double>(0xFFF0 * sizeof(double))));
        }
        [Fact]
        public void ElementOffset()
        {
            Assert.Equal(10, UnsafeUnmanaged.ElementOffset(ref int100[10], ref int100[20]));
            Assert.Equal(-10, UnsafeUnmanaged.ElementOffset(ref int100[20], ref int100[10]));
        }
        [Fact]
        public void IsAddressGeq()
        {
            Assert.True(UnsafeUnmanaged.IsAddressGeq(ref int100[1], ref int100[0]));
            Assert.True(UnsafeUnmanaged.IsAddressGeq(ref int100[0], ref int100[0]));
            Assert.False(UnsafeUnmanaged.IsAddressGeq(ref int100[0], ref int100[1]));
        }
        [Fact]
        public void IsAddressLeq()
        {
            Assert.False(UnsafeUnmanaged.IsAddressLeq(ref int100[1], ref int100[0]));
            Assert.True(UnsafeUnmanaged.IsAddressLeq(ref int100[0], ref int100[0]));
            Assert.True(UnsafeUnmanaged.IsAddressLeq(ref int100[0], ref int100[1]));
        }
    }
}
