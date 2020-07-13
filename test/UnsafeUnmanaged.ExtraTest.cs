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
        static void* MakePtr(ulong value) => (void*)value;
        static T* MakePtr<T>(ulong value) where T : unmanaged => (T*)value;

        int[] int100 = new int[100];

        [Fact]
        public void ByteOffsetVoidPtr()
        {
            if (Is64Bit)
            {
                Assert.Equal(IntPtr.Zero, UnsafeUnmanaged.ByteOffset(MakePtr(0xFFFFFFFFU), MakePtr(0xFFFFFFFFU)));
                Assert.Equal(new IntPtr(0x100000000L), UnsafeUnmanaged.ByteOffset(MakePtr(0xFFFFFFFFU), MakePtr(0x1FFFFFFFFL)));
            }

            Assert.Equal(IntPtr.Zero, UnsafeUnmanaged.ByteOffset(MakePtr(0xFFFFFFFU), MakePtr(0xFFFFFFFU)));
            Assert.Equal(new IntPtr(0x10000000), UnsafeUnmanaged.ByteOffset(MakePtr(0xFFFFFFF), MakePtr(0x1FFFFFFFU)));

        }

        struct Foo
        {
            public int bar;
            public int baz;
        }
        [Fact]
        public void OffsetOf()
        {
            Foo f = new Foo();
            Foo f2 = new Foo();

            Assert.Equal(0, UnsafeUnmanaged.OffsetOf(f, f.bar));
            Assert.Equal(4, UnsafeUnmanaged.OffsetOf(f, f.baz));
            Assert.Throws<ArgumentOutOfRangeException>(() => UnsafeUnmanaged.OffsetOf(f2, f.bar));
            Assert.Throws<ArgumentOutOfRangeException>(() => UnsafeUnmanaged.OffsetOf(f2, f.baz));
            Assert.Throws<ArgumentOutOfRangeException>(() => UnsafeUnmanaged.OffsetOf(f, f2.bar));
            Assert.Throws<ArgumentOutOfRangeException>(() => UnsafeUnmanaged.OffsetOf(f, f2.baz));
        }


        [Fact]
        public void Operlaps()
        {
            var a1 = new int[10];

            {
                Assert.False(UnsafeUnmanaged.Overlaps(ref a1[0], 2, ref a1[2], 2));
                Assert.False(UnsafeUnmanaged.Overlaps(ref a1[0], 2, ref a1[2], 2, out var elemOffset2));
                Assert.Equal(0, elemOffset2);

                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[1], 2, ref a1[2], 2));
                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[1], 2, ref a1[2], 2, out var elemOffset1));
                Assert.Equal(1, elemOffset1);

                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[2], 2, ref a1[1], 2));
                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[2], 2, ref a1[1], 2, out var elemOffset3));
                Assert.Equal(-1, elemOffset3);
            }
            {
                Assert.False(UnsafeUnmanaged.Overlaps(ref a1[0], 2, ref a1[2], 8));
                Assert.False(UnsafeUnmanaged.Overlaps(ref a1[0], 2, ref a1[2], 8, out var elemOffset2));
                Assert.Equal(0, elemOffset2);

                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[1], 2, ref a1[2], 8));
                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[1], 2, ref a1[2], 8, out var elemOffset1));
                Assert.Equal(1, elemOffset1);

                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[2], 2, ref a1[1], 8));
                Assert.True(UnsafeUnmanaged.Overlaps(ref a1[2], 2, ref a1[1], 8, out var elemOffset3));
                Assert.Equal(-1, elemOffset3);
            }
        }

        [Fact]
        public void NullRef()
        {
            Assert.True(UnsafeUnmanaged.IsNullRef(ref UnsafeUnmanaged.NullRef<int>()));
            int n = 0;
            Assert.False(UnsafeUnmanaged.IsNullRef(ref n));
        }

        [Fact]
        public void NullRefReadOnly()
        {
            Assert.True(UnsafeUnmanaged.IsNullRefReadOnly(UnsafeUnmanaged.NullRefReadOnly<decimal>()));
            decimal n = 0;
            Assert.False(UnsafeUnmanaged.IsNullRef(ref n));
        }

    }
}
