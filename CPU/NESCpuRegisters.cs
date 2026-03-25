using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{
    internal class NESCpuRegisters
    {
        public byte A { get; set; } // a register
        public byte X { get; set; } // x register
        public byte Y { get; set; } // y register

        public ushort PC { get; set; } // 2 byte program counter 

        public byte S { get; set; } // stack pointer
        public byte P { get; set; } // status register
        public void Reset()
        {
            this.PC = 0xC000; // for nestest

            A = 0x00;
            X = 0x00;
            Y = 0x00;
            P = 0x24;
            S = 0xFD;
        }
        public enum StatusRegisterBit
        {
            Carry = 0,
            Zero = 1,
            InterruptDisable = 2,
            Decimal = 3,
            Overflow = 6,
            Negative = 7
        }
        public void SetStatusRegisterFlag(StatusRegisterBit bit, bool value) {
            if (value)
                P |= (byte)(1 << (int)bit);
            else
                P &= (byte)~(1 << (int)bit); 
        }
        
        public override string ToString()
        {
            return $"PC: {PC.ToString("X4")}, A: {A.ToString("X2")}, X: {X.ToString("X2")}, Y: {Y.ToString("X2")}, S: {S.ToString("X2")}, P: {P.ToString("X2")}";
        }

    }
}
