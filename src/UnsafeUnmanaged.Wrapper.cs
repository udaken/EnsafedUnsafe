using System;
using System.Runtime.CompilerServices;

namespace SafelyUnsafe
{
    /// <summary>
    /// a restricted System.Runtime.CompilerServices.Unsafe class rapper for "unmanaged".
    /// </summary>
    public static partial class UnsafeUnmanaged
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref byte AsByteRef<T>(ref T source)
            where T : unmanaged
            => ref Unsafe.As<T, byte>(ref source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static IntPtr ByteOffset<T>(T* origin, T* target)
            where T : unmanaged
        {
            CheckAligned<T>(origin);
            CheckAligned<T>(target);
            return Unsafe.ByteOffset(ref Unsafe.AsRef<T>(origin), ref Unsafe.AsRef<T>(target));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ElementOffset<T>(T* origin, T* target)
            where T : unmanaged
        {
            CheckAligned<T>(origin);
            CheckAligned<T>(target);
            return (int)((long)ByteOffset(origin, target) / Unsafe.SizeOf<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ElementOffset<T>(ref T origin, ref T target)
            where T : unmanaged
            => (int)((long)ByteOffset(ref origin, ref target) / Unsafe.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGeq<T>(ref T left, ref T right)
            where T : unmanaged
            => AreSame(ref left, ref right) || IsAddressGreaterThan(ref left, ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLeq<T>(ref T left, ref T right)
            where T : unmanaged
            => AreSame(ref left, ref right) || IsAddressLessThan(ref left, ref right);

        // -----------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static T Read<T>(void* source)
            where T : unmanaged
            => Unsafe.Read<T>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static T ReadUnaligned<T>(void* source)
            where T : unmanaged
            => Unsafe.ReadUnaligned<T>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static T ReadUnaligned<T>(ref byte source)
            where T : unmanaged
            => Unsafe.ReadUnaligned<T>(ref source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void Write<T>(void* destination, T value)
            where T : unmanaged
            => Unsafe.Write<T>(destination, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUnaligned<T>(void* destination, T value)
            where T : unmanaged
            => Unsafe.WriteUnaligned(destination, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUnaligned<T>(ref byte destination, T value)
            where T : unmanaged
            => Unsafe.WriteUnaligned(ref destination, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void Copy<T>(void* destination, ref T source)
            where T : unmanaged
            => Unsafe.Copy(destination, ref source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void Copy<T>(ref T destination, void* source)
            where T : unmanaged
            => Unsafe.Copy(ref destination, source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static T* AsPointer<T>(ref T value)
            where T : unmanaged
            => (T*)Unsafe.AsPointer(ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int SizeOf<T>()
            where T : unmanaged
            => Unsafe.SizeOf<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ref T AsRef<T>(T* source)
            where T : unmanaged
            => ref Unsafe.AsRef<T>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AsRef<T>(in T source)
            where T : unmanaged
            => ref Unsafe.AsRef(in source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TTo As<TFrom, TTo>(ref TFrom source)
            where TFrom : unmanaged
            where TTo : unmanaged
            => ref Unsafe.As<TFrom, TTo>(ref source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(ref T source, int elementOffset)
            where T : unmanaged
            => ref Unsafe.Add(ref source, elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void* Add<T>(void* source, int elementOffset)
            where T : unmanaged
            => Unsafe.Add<T>(source, elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(ref T source, IntPtr elementOffset)
            where T : unmanaged
            => ref Unsafe.Add(ref source, elementOffset);

        [Obsolete]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
            where T : unmanaged
            => ref Unsafe.AddByteOffset(ref source, byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Subtract<T>(ref T source, int elementOffset)
            where T : unmanaged
            => ref Unsafe.Subtract(ref source, elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Subtract<T>(ref T source, IntPtr elementOffset)
            where T : unmanaged
            => ref Unsafe.Subtract(ref source, elementOffset);

        [Obsolete]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T SubtractByteOffset<T>(ref T source, IntPtr byteOffset)
            where T : unmanaged
            => ref Unsafe.SubtractByteOffset(ref source, byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr ByteOffset<T>(ref T origin, ref T target)
            where T : unmanaged
            => Unsafe.ByteOffset(ref origin, ref target);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreSame<T>(ref T left, ref T right)
            where T : unmanaged
            => Unsafe.AreSame(ref left, ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
            where T : unmanaged
            => Unsafe.IsAddressGreaterThan(ref left, ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLessThan<T>(ref T left, ref T right)
            where T : unmanaged
            => Unsafe.IsAddressLessThan(ref left, ref right);
    }

}
