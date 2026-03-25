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

    }
}
