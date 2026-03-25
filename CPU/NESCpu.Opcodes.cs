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
        }
    }
}
