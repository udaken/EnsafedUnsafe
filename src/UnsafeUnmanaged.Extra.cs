using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SafelyUnsafe
{
    public static partial class UnsafeUnmanaged
    {
        [Conditional("DEBUG")]
        static unsafe void CheckAligned<T>(void* pointer) where T :unmanaged
        {
            if (!IsMaybeAligned<T>(pointer))
                throw new DataMisalignedException($"{new IntPtr(pointer)}");
        }

        #region MoveBlock
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlock(IntPtr destination, IntPtr source, ulong byteCount)
            => Buffer.MemoryCopy(source.ToPointer(), destination.ToPointer(), byteCount, byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlock<T>(T* destination, T* source, uint count)
            where T : unmanaged
            => Buffer.MemoryCopy(source, destination, count * sizeof(T) , count * sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlock(ref byte destination, in byte source, uint byteCount)
            => Buffer.MemoryCopy(Unsafe.AsPointer(ref Unsafe.AsRef(in source)), Unsafe.AsPointer(ref destination), byteCount, byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlock<T>(ref T destination, in T source, uint count)
            where T : unmanaged
            => Buffer.MemoryCopy(Unsafe.AsPointer(ref Unsafe.AsRef(in source)), Unsafe.AsPointer(ref destination), count * sizeof(T), count * sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlockUnaligned<T>(T* destination, T* source, uint count) 
            where T : unmanaged
            => Buffer.MemoryCopy(source, destination, count * sizeof(T), count * sizeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlockUnaligned(ref byte destination, in byte source, uint byteCount)
            => Buffer.MemoryCopy(Unsafe.AsPointer(ref Unsafe.AsRef(in source)), Unsafe.AsPointer(ref destination), byteCount, byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void MoveBlockUnaligned<T>(ref T destination, in T source, uint count)
            where T : unmanaged
            => Buffer.MemoryCopy(Unsafe.AsPointer(ref Unsafe.AsRef(in source)), Unsafe.AsPointer(ref destination), count * sizeof(T), count * sizeof(T));

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static IntPtr ByteOffset(void* origin, void* target)
            => ByteOffset((byte*)origin, (byte*)target);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static IntPtr ByteOffset(byte* origin, byte* target)
            => Unsafe.SizeOf<UIntPtr>() == sizeof(int) ? new IntPtr((int)(target - origin)) : new IntPtr((target - origin));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int OffsetOf<TOuter, TInner>(in TOuter outer, in TInner member)
            where TOuter : unmanaged
            where TInner : unmanaged
        {
            ref readonly TOuter mid = ref AsReadOnly<TInner, TOuter>(in member);

            IntPtr diff = ByteOffsetReadOnly(outer, mid);

            if ((uint)(diff) >= UnsignedSizeOf<TOuter>())
            {
                throw new ArgumentOutOfRangeException(nameof(member), $"given reference is not member of {nameof(TOuter)}");
            }
            else
            {
                return diff.ToInt32();
            }
        }

        #region Overlaps, Contains
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool Overlaps(void* origin, UIntPtr originByteCount, void* target, UIntPtr targetByteCount)
        {
            if (originByteCount == default || targetByteCount == default)
                return false;

            IntPtr byteOffset = ByteOffset(origin, target);

            if (Unsafe.SizeOf<UIntPtr>() == sizeof(int))
            {
                return (uint)byteOffset < (uint)(originByteCount) ||
                       (uint)byteOffset > (uint)-(targetByteCount.ToUInt32());
            }
            else
            {
                return (ulong)byteOffset < (ulong)(originByteCount) ||
                       (ulong)byteOffset > (ulong)-(long)(targetByteCount.ToUInt64());
            }
        }

        /// <summary>
        /// Determines whether two sequences overlap in memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool Overlaps<T>(ref T source, int elementCount, ref T other, int otherElementCount)
            where T : unmanaged
             => Overlaps(
                Unsafe.AsPointer(ref source), (UIntPtr)((uint)elementCount * UnsignedSizeOf<T>()),
                Unsafe.AsPointer(ref other), (UIntPtr)((uint)otherElementCount * UnsignedSizeOf<T>()));

        /// <summary>
        /// Determines whether two sequences overlap in memory and outputs the element offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Overlaps<T>(ref T source, int elementCount, ref T other, int otherElementCount, out int elementOffset)
            where T : unmanaged
        {
            if (elementCount == 0 || otherElementCount == 0)
            {
                elementOffset = 0;
                return false;
            }

            IntPtr byteOffset = Unsafe.ByteOffset(ref source, ref other);

            if (Unsafe.SizeOf<IntPtr>() == sizeof(int))
            {
                if ((uint)byteOffset < (uint)(elementCount * Unsafe.SizeOf<T>()) ||
                    (uint)byteOffset > (uint)-(otherElementCount * Unsafe.SizeOf<T>()))
                {
                    elementOffset = (int)byteOffset / Unsafe.SizeOf<T>();
                    return true;
                }
                else
                {
                    elementOffset = 0;
                    return false;
                }
            }
            else
            {
                if ((ulong)byteOffset < (ulong)((long)elementCount * Unsafe.SizeOf<T>()) ||
                    (ulong)byteOffset > (ulong)-((long)otherElementCount * Unsafe.SizeOf<T>()))
                {
                    elementOffset = (int)((long)byteOffset / Unsafe.SizeOf<T>());
                    return true;
                }
                else
                {
                    elementOffset = 0;
                    return false;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(ref T source, int elementCount, ref T other)
            where T : unmanaged
            => Overlaps(ref source, elementCount, ref other, 1);

        #endregion

        /// <summary>
        /// Create a dangerous null reference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref T NullRef<T>()
            where T : unmanaged
            => ref Unsafe.AsRef<T>(null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool IsNullRef<T>(ref T source)
            where T : unmanaged
            => Unsafe.AsPointer(ref source) == null;

        /// <summary>
        /// Create a dangerous null reference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref readonly T NullRefReadOnly<T>()
            where T : unmanaged
            => ref NullRef<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool IsNullRefReadOnly<T>(in T source)
            where T : unmanaged
            => IsNullRef(ref Unsafe.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool IsMaybeAligned<T>(void* source)
            where T : unmanaged
            => (uint)source % sizeof(double) == 0 || (uint)source % UnsignedSizeOf<T>() == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool IsMaybeAligned<T>(IntPtr source)
            where T : unmanaged
            => IsMaybeAligned<T>((void*)source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool IsMaybeAligned<T>(ref byte source)
            where T : unmanaged
            => IsMaybeAligned<T>(Unsafe.AsPointer(ref source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static uint UnsignedSizeOf<T>()
            where T : unmanaged
            => (uint)(Unsafe.SizeOf<T>());
    }

}
