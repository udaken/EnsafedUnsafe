using System;
using System.Runtime.CompilerServices;

namespace EnsafedUnsafe
{
    public static partial class UnsafeUnmanaged
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly byte AsByteRefReadOnly<T>(in T source)
            where T : unmanaged
            => ref AsReadOnly<T, byte>(in source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ElementOffsetReadOnly<T>(in T origin, in T target)
            where T : unmanaged
            => (int)((long)ByteOffsetReadOnly(in origin, in target) / Unsafe.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGeqReadOnly<T>(in T left, in T right)
            where T : unmanaged
            => AreSameReadOnly(in left, in right) || IsAddressGreaterThanReadOnly(in left, in right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLeqReadOnly<T>(in T left, in T right)
            where T : unmanaged
            => AreSameReadOnly(in left, in right) || IsAddressLessThanReadOnly(in left, in right);

        // -----------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadUnalignedReadOnly<T>(in byte source)
            where T : unmanaged
            => Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void CopyReadOnly<T>(void* destination, in T source)
            where T : unmanaged
        {
            CheckAligned<T>(destination);
            Unsafe.Copy(destination, ref Unsafe.AsRef(in source));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlockReadOnly(ref byte destination, in byte source, uint byteCount)
            => Unsafe.CopyBlock(ref destination, ref Unsafe.AsRef(in source), byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlockUnalignedReadOnly(ref byte destination, in byte source, uint byteCount)
            => Unsafe.CopyBlock(ref destination, ref Unsafe.AsRef(in source), byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly TTo AsReadOnly<TFrom, TTo>(in TFrom source)
            where TFrom : unmanaged
            where TTo : unmanaged
            => ref Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AddReadOnly<T>(in T source, int elementOffset)
            where T : unmanaged
            => ref Unsafe.Add(ref Unsafe.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AddReadOnly<T>(in T source, IntPtr elementOffset)
            where T : unmanaged
            => ref Unsafe.Add(ref Unsafe.AsRef(in source), elementOffset);

        [Obsolete]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AddByteOffsetReadOnly<T>(in T source, IntPtr byteOffset)
            where T : unmanaged
            => ref Unsafe.AddByteOffset(ref Unsafe.AsRef(in source), byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T SubtractReadOnly<T>(in T source, int elementOffset)
            where T : unmanaged
            => ref Unsafe.Subtract(ref Unsafe.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T SubtractReadOnlyReadOnly<T>(in T source, IntPtr elementOffset)
            where T : unmanaged
            => ref Unsafe.Subtract(ref Unsafe.AsRef(in source), elementOffset);

        [Obsolete]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T SubtractByteOffsetReadOnly<T>(in T source, IntPtr byteOffset)
            where T : unmanaged
            => ref Unsafe.SubtractByteOffset(ref Unsafe.AsRef(in source), byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr ByteOffsetReadOnly<T>(in T origin, in T target)
            where T : unmanaged
            => Unsafe.ByteOffset(ref Unsafe.AsRef(in origin), ref Unsafe.AsRef(in target));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreSameReadOnly<T>(in T left, in T right)
            where T : unmanaged
            => Unsafe.AreSame(ref Unsafe.AsRef(in left), ref Unsafe.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGreaterThanReadOnly<T>(in T left, in T right)
            where T : unmanaged
            => Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef(in left), ref Unsafe.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLessThanReadOnly<T>(in T left, in T right)
            where T : unmanaged
            => Unsafe.IsAddressLessThan(ref Unsafe.AsRef(in left), ref Unsafe.AsRef(in right));
    }
}
