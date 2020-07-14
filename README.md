# SafelyUnsafe
## About

SafelyUnsafe provides `System.Runtime.CompilerServices.Unsafe` with a "little" more safety Wrapper Method.
And also provides several support methods.

- It is implemented in the `UnsafeUnmanaged` class.
  In most cases, you just replace `Unsafe` to `UnsafeUnmanaged`.
- All type argument is restricted to `unmanaged`
- overload for `ref readonly`, with an `ReadOnly` in the suffix.
- Utility methods like `OffsetOf`, `Overlaps`, `NullRef`, `IsNullRef`.
- .NET Standard 2.0 is supported.


## All generic type argument is restricted to `unmanaged`

Design of `System.Runtime.CompilerServices.Unsafe`  is older than C# 7.3.

There are no restrictions on type arguments of the `Unsafe` class, but all `SafelyUnsafe` methods have restrictions.

## Overload for `ref readonly`

References that do not require the change are given the `IsReadOnly` attribute(a.k.a `in` keyword).
They have the `ReadOnly` attribute at the end.

NOTE: **`in` is optional, callers should be careful.**

## Utility methods

### `MoveBlock`
It is a method that can correctly copy overlapping regions, like `memmove`.
The behavior of `Unsafe.CopyBlock` is unspecified if the source and destination areas overlap.

**This method is not implemented.**

### `OffsetOf`

It is similar to the C's `offsetof` macro.

```csharp
struct Foo
{
    public int bar;
    public int baz;
}

UnsafeUnmanaged.OffsetOf(f, f.baz); // 4
```

### `Overlaps`

Determines whether two region overlap in memory.

### `NullRef`

Get a null reference for interoperability.
In C++, it is similar to `(T&)(*((T*)nullptr))`.

### `IsNullRef`

Determines if it is a null reference.

### `UnsignedSizeOf`

Same as `Unsafe.dSizeOf`, which casts to unsigned.

### `AsByteRef`

Reinterpret a reference to `T` as a reference to `byte`.

### `IsAddressGeq`

Same as `AreSame || IsAddressGreaterThan`.

### `IsAddressLeq`

Same as `AreSame || IsAddressLessThan`.




