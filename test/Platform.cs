using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    static class Platform
    {
        public static bool Is64Bit => IntPtr.Size == 8;
        public static bool IsX86 => IntPtr.Size == 4;
    }
}
