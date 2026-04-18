using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{
    internal partial class NESCpu
    {
      

        // ACCESS
        private void Sta(ushort addr)
        {
            this._bus.Write(this._registers.A, addr);
        }

        private void Stx(ushort addr)
        {
            this._bus.Write(this._registers.X, addr);

        }

        private void Sty(ushort addr)
        {
            this._bus.Write(this._registers.Y, addr);
        }

        private void Lda(byte value)
        {
            this._registers.A = value;
            UpdateZeroNegativeFlags(value);
        }
        private void Ldx(byte value)
        {
            this._registers.X = value;
            UpdateZeroNegativeFlags(value);
        }

        private void Ldy(byte value)
        {
            this._registers.Y = value;
            UpdateZeroNegativeFlags(value);
        }

        // TRANSFER 
        private void Tax()
        {
            this._registers.X = this._registers.A;
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, this._registers.X == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (this._registers.X & 0x80) != 0); // 0b1000000
        }
        private void Tay()
        {
            this._registers.Y = this._registers.A;
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, this._registers.Y == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (this._registers.Y & 0x80) != 0); // 0b1000000
        }
    

        private void Txa()
        {
            this._registers.A = this._registers.X;
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, this._registers.A == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (this._registers.A & 0x80) != 0); // 0b1000000
        }

      

        private void Tya()
        {
            this._registers.A = this._registers.Y;
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, this._registers.A == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (this._registers.A & 0x80) != 0); // 0b1000000
        }

        // ARITHMETIC
        private void Adc(byte memory)
        {
            int result = memory + this._registers.A + (this._registers.GetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry) ? 1 : 0);
            
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry, result > 0xFF) ;
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, result == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Overflow, (((byte)result ^ this._registers.A) & ((byte)result ^ memory) & 0x80) != 0); // sign bit changed - overflow!
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, ((byte)result & 0x80) != 0); // 0b10000000

            this._registers.A = (byte)result;
        }

        private void Sbc(byte memory)
        {
            int result = this._registers.A - memory - (this._registers.GetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry) ? 0 : 1); // 

            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry, result >= 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, result == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Overflow, (((byte)result ^ this._registers.A) & ((byte)result ^ ~memory) & 0x80) != 0); // If result's sign is different from A's and the same as memory's, signed overflow (or underflow) occurred.
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, ((byte)result & 0x80) != 0); // 0b1000000  1000 

            this._registers.A = (byte)result;
        }

        private void Inc(ushort addr)
        {
            byte newValue = (byte)(this._bus.Read(addr) + 1);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, newValue == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (newValue & 0x80) != 0);
            this._bus.Write(newValue, addr);
        }

        // SHIFT
        // BITWISE
        // COMPARE
        // BRANCH

        private void Bcs(byte addrShift)
        {
            if (!this._registers.GetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry))
                return;
            int offset = ((addrShift & 0x80) != 0 ? -1 : 1) * (addrShift & 0x7F);
            _registers.PC = (ushort) (_registers.PC + offset);
        }
        private void Beq(byte addrShift)
        {
            if (!this._registers.GetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero))
                return;
            int offset = ((addrShift & 0x80) != 0 ? -1 : 1) * (addrShift & 0x7F);
            _registers.PC = (ushort)(_registers.PC + offset);

        }

        // JUMP
        private void JmpAbs()
        {
            byte lo = _bus.Read(_registers.PC++);
            byte hi = _bus.Read(_registers.PC++);
            ushort addr = (ushort)((hi << 8) | lo);
            _registers.PC = addr;
        }
        private void JmpInd()
        {
            byte loAddr = _bus.Read(_registers.PC++);
            byte hiAddr = _bus.Read(_registers.PC++);
            ushort addr = (ushort)((hiAddr << 8) | loAddr);
            ushort loVal = _bus.Read(addr);
            ushort hiVal = _bus.Read((ushort)(addr + 1));
            ushort val = (ushort)((hiVal << 8) | loVal);
            _registers.PC = val;
        }

        private void Jsr(ushort addr)
        {
            byte high = (byte)(_registers.PC >> 2);
            byte low = (byte)(_registers.PC & 0xFF);

            PushStack(high);
            PushStack(low);

            _registers.PC = addr;
        }

        private void Rts()
        {
            byte low = PullStack();
            byte high = PullStack();
            ushort addr = (ushort)(((high << 2) | low) + 1);
            this._registers.PC = addr;
        }
        // STACK
        private void Pha()
        {
            PushStack(_registers.A);
        }
        private void Pla()
        {
            byte value = PullStack();
            _registers.A = value;
            UpdateZeroNegativeFlags(value);
        }

        private void Php()
        {
            PushStack(_registers.P);
        }
        private void Plp()
        {
            byte value = PullStack();
            _registers.P = value;
        }

        private void Txs()
        {
            this._registers.S = this._registers.X;
        }

        private void Tsx()
        {
            this._registers.X = this._registers.S;
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, this._registers.X == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (this._registers.X & 0x80) != 0); // 0b1000000
        }

        // FLAGS
        private void Sec()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry, true);
        }
        private void Sei()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.InterruptDisable, true);
        }
        private void Sed()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Decimal, true);
        }
        private void Clv()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Overflow, false);
        }
        private void Clc()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Carry, false);
        }
        private void Cli()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.InterruptDisable, false);
        }
        private void Cld()
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Decimal, false);
        }

        // OTHER
        private void Nop()
        {
            return; 
        }
 


    }
}
