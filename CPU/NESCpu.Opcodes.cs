using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{
    internal partial class NESCpu
    {
        public void InitOpcodesTable()
        {
            _opcodeTable[0x4C] = () => JmpAbs(); // JMP (Absolute)
            _opcodeTable[0x6C] = () => JmpInd(); // JMP (Indirect)
            // LDA
            _opcodeTable[0xA9] = () => Lda(_addressModes.ReadImmediate()); // Immediate
            _opcodeTable[0xA5] = () => Lda(_addressModes.ReadZeroPage()); // ZP
            _opcodeTable[0xB5] = () => Lda(_addressModes.ReadZeroPageX()); // ZP X
            _opcodeTable[0xAD] = () => Lda(_addressModes.ReadAbsolute()); // ABS
            _opcodeTable[0xBD] = () => Lda(_addressModes.ReadAbsoluteX()); // ABS X
            _opcodeTable[0xB9] = () => Lda(_addressModes.ReadAbsoluteY()); // ABS Y
            _opcodeTable[0xA1] = () => Lda(_addressModes.ReadIndirectX()); // IND X
            _opcodeTable[0xB1] = () => Lda(_addressModes.ReadIndirectY()); // IND Y
            // LDX
            _opcodeTable[0xA2] = () => Ldx(_addressModes.ReadImmediate()); // Immediate
            _opcodeTable[0xA6] = () => Ldx(_addressModes.ReadZeroPage()); // ZP
            _opcodeTable[0xB6] = () => Ldx(_addressModes.ReadZeroPageY()); // ZP Y
            _opcodeTable[0xAE] = () => Ldx(_addressModes.ReadAbsolute()); // ABS
            _opcodeTable[0xBE] = () => Ldx(_addressModes.ReadAbsoluteY()); // ABS Y
            // LDY
            _opcodeTable[0xA0] = () => Ldy(_addressModes.ReadImmediate()); // Immediate
            _opcodeTable[0xA4] = () => Ldy(_addressModes.ReadZeroPage()); // ZP
            _opcodeTable[0xB4] = () => Ldy(_addressModes.ReadZeroPageX()); // ZP X
            _opcodeTable[0xAC] = () => Ldy(_addressModes.ReadAbsolute()); // ABS
            _opcodeTable[0xBC] = () => Ldy(_addressModes.ReadAbsoluteX()); // ABS X
            // STA
            _opcodeTable[0x85] = () => Sta(_addressModes.GetZeroPageAddress()); // ZP
            _opcodeTable[0x95] = () => Sta(_addressModes.GetZeroPageXAddress()); // ZP X
            _opcodeTable[0x8D] = () => Sta(_addressModes.GetAbsoluteAddress()); // ABS
            _opcodeTable[0x9D] = () => Sta(_addressModes.GetAbsoluteXAddress()); // ABS X
            _opcodeTable[0x99] = () => Sta(_addressModes.GetAbsoluteYAddress()); // ABS Y
            _opcodeTable[0x81] = () => Sta(_addressModes.GetIndirectXAddress()); // IND X
            _opcodeTable[0x91] = () => Sta(_addressModes.GetIndirectYAddress()); // IND Y
            // STX
            _opcodeTable[0x86] = () => Stx(_addressModes.GetZeroPageAddress()); // ZP
            _opcodeTable[0x96] = () => Stx(_addressModes.GetZeroPageYAddress()); // ZP Y
            _opcodeTable[0x8E] = () => Stx(_addressModes.GetAbsoluteAddress()); // ABS
            // STY
            _opcodeTable[0x84] = () => Sty(_addressModes.GetZeroPageAddress()); // ZP
            _opcodeTable[0x94] = () => Sty(_addressModes.GetZeroPageXAddress()); // ZP X
            _opcodeTable[0x8C] = () => Sty(_addressModes.GetAbsoluteAddress()); // ABS


        }
    }
}
