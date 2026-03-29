using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{
    internal partial class NESCpu
    {
        private void UpdateFlagsLd(byte value)
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, value == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (value & 0b1000) != 0);
        }
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
            UpdateFlagsLd(value);
        }
        private void Ldx(byte value)
        {
            this._registers.X = value;
            UpdateFlagsLd(value);
        }

        private void Ldy(byte value)
        {
            this._registers.Y = value;
            UpdateFlagsLd(value);
        }

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

        private void Pha()
        {
            PushStack(_registers.A);
        }
        private void Pla()
        {
            byte value = PullStack();
            _registers.A = value;
            UpdateFlagsLd(value);
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
    }
}
