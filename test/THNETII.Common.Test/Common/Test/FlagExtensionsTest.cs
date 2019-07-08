using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace THNETII.Common.Test
{
    public static class FlagExtensionsTest
    {
        [Fact]
        public static void None_has_no_common_flags_with_all_Int8()
            => Assert.False(TestEnumInt8.None.HasAnyFlag(TestEnumInt8.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_Int16()
            => Assert.False(TestEnumInt16.None.HasAnyFlag(TestEnumInt16.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_Int32()
            => Assert.False(TestEnumInt32.None.HasAnyFlag(TestEnumInt32.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_Int64()
            => Assert.False(TestEnumInt64.None.HasAnyFlag(TestEnumInt64.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_UInt8()
            => Assert.False(TestEnumUInt8.None.HasAnyFlag(TestEnumUInt8.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_UInt16()
            => Assert.False(TestEnumUInt16.None.HasAnyFlag(TestEnumUInt16.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_UInt32()
            => Assert.False(TestEnumUInt32.None.HasAnyFlag(TestEnumUInt32.All));

        [Fact]
        public static void None_has_no_common_flags_with_all_UInt64()
            => Assert.False(TestEnumUInt64.None.HasAnyFlag(TestEnumUInt64.All));

        [Fact]
        public static void One_has_common_flags_with_all_Int8()
            => Assert.True(TestEnumInt8.One.HasAnyFlag(TestEnumInt8.All));

        [Fact]
        public static void One_has_common_flags_with_all_Int16()
            => Assert.True(TestEnumInt16.One.HasAnyFlag(TestEnumInt16.All));

        [Fact]
        public static void One_has_common_flags_with_all_Int32()
            => Assert.True(TestEnumInt32.One.HasAnyFlag(TestEnumInt32.All));

        [Fact]
        public static void One_has_common_flags_with_all_Int64()
            => Assert.True(TestEnumInt64.One.HasAnyFlag(TestEnumInt64.All));

        [Fact]
        public static void One_has_common_flags_with_all_UInt8()
            => Assert.True(TestEnumUInt8.One.HasAnyFlag(TestEnumUInt8.All));

        [Fact]
        public static void One_has_common_flags_with_all_UInt16()
            => Assert.True(TestEnumUInt16.One.HasAnyFlag(TestEnumUInt16.All));

        [Fact]
        public static void One_has_common_flags_with_all_UInt32()
            => Assert.True(TestEnumUInt32.One.HasAnyFlag(TestEnumUInt32.All));

        [Fact]
        public static void One_has_common_flags_with_all_UInt64()
            => Assert.True(TestEnumUInt64.One.HasAnyFlag(TestEnumUInt64.All));

        [Flags]
        private enum TestEnumInt32 : int
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = ~0
        }

        [Flags]
        private enum TestEnumInt64 : long
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = ~0
        }

        [Flags]
        private enum TestEnumInt16 : short
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = ~0
        }

        [Flags]
        private enum TestEnumInt8 : sbyte
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = unchecked((sbyte)(1 << 7)),
            All = ~0
        }

        [Flags]
        private enum TestEnumUInt32 : uint
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = ~0U
        }

        [Flags]
        private enum TestEnumUInt64 : ulong
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = ~0U
        }

        [Flags]
        private enum TestEnumUInt16 : ushort
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = unchecked((ushort)~0U)
        }

        [Flags]
        private enum TestEnumUInt8 : byte
        {
            None = 0,
            Zero = 1 << 0,
            One = 1 << 1,
            Two = 1 << 2,
            Three = 1 << 3,
            Four = 1 << 4,
            Five = 1 << 5,
            Six = 1 << 6,
            Seven = 1 << 7,
            All = unchecked((byte)~0U)
        }
    }
}
