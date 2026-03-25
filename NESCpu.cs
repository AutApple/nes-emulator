using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator
{
    internal class NESCpu(NESMemoryBus bus)
    {
        public byte A { get; private set; } // a register
        public byte X { get; private set; } // x register
        public byte Y { get; private set; } // y register

        public ushort PC { get; private set; } // 2 byte program counter 

        public byte S { get; private set; } // stack pointer
        public byte P { get; private set; } // status register
        private readonly NESMemoryBus _bus = bus;
    }
}
