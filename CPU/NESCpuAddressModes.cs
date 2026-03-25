
namespace NESEmulator.CPU
{
    internal class NESCpuAddressModes(NESCpuRegisters registers, NESMemoryBus memory)
    {
        private readonly NESCpuRegisters _registers = registers;
        private readonly NESMemoryBus _bus = memory;
        // Read immediate 
        public byte ReadImmediate() { return _bus.Read(_registers.PC++); } // value stored right after the operand
        
        
        // Get address methods
        public byte GetZeroPageAddress() {
            return _bus.Read(_registers.PC++);
        }
        public byte GetZeroPageXAddress()
        {
            return (byte)(GetZeroPageAddress() + _registers.X);
        }
        public byte GetZeroPageYAddress()
        {
            return (byte)(GetZeroPageAddress() + _registers.Y);
        }

        
        public ushort GetAbsoluteAddress()
        {
            byte lo = _bus.Read(_registers.PC++);
            byte hi = _bus.Read(_registers.PC++);
            ushort addr = (ushort)((hi << 8) | lo);
            return addr;
        }
        public ushort GetAbsoluteXAddress()
        {
            ushort addr = (ushort)(GetAbsoluteAddress() + _registers.X);
            return addr;
        }
        public ushort GetAbsoluteYAddress()
        {
            ushort addr = (ushort)(GetAbsoluteAddress() + _registers.Y);
            return addr;
        }

        public ushort GetIndirectXAddress() 
        {
            byte addr = _bus.Read(_registers.PC++);
            addr += _registers.X;

            byte lo = _bus.Read(addr);
            byte hi = _bus.Read((byte)(addr + 1));

            ushort finalAddr = (ushort)((hi << 8) | lo);
            return finalAddr;
        }

        public ushort GetIndirectYAddress()
        {
            byte addr = _bus.Read(_registers.PC++);
            byte lo = _bus.Read(addr);
            byte hi = _bus.Read((byte)(addr + 1));
            ushort resultingAddress = (ushort)((hi << 8) | lo);
            resultingAddress += _registers.Y;
            return resultingAddress;
        }

        public byte ReadZeroPage() => _bus.Read(GetZeroPageAddress()); // value stored in the address that is specified after the opcode - but it points only to 0x0000 to 0x00FF
        public byte ReadZeroPageX() => _bus.Read(GetZeroPageXAddress()); // same but add X register value to it
        public byte ReadZeroPageY() => _bus.Read(GetZeroPageYAddress()); // same but add Y register value to it

        // absolute modes
        public byte ReadAbsolute() => _bus.Read(GetAbsoluteAddress());
        public byte ReadAbsoluteX() => _bus.Read(GetAbsoluteXAddress());
        public byte ReadAbsoluteY() => _bus.Read(GetAbsoluteYAddress());
        // indirect modes
        public byte ReadIndirectX() => _bus.Read(GetIndirectXAddress());
        public byte ReadIndirectY() => _bus.Read(GetIndirectYAddress());

    }
}
