using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator
{
    // for logging purposes
    internal static class NESLoggingTables
    {
        public static readonly string?[] InstructionNameTable = new string?[256];

        static NESLoggingTables()
        {
            InstructionNameTable[0x4C] = "JMP"; // JMP (Absolute)
            InstructionNameTable[0x6C] = "JMP"; // JMP (Indirect)
            // LDA
            InstructionNameTable[0xA9] = "LDA"; // Immediate
            InstructionNameTable[0xA5] = "LDA"; // ZP
            InstructionNameTable[0xB5] = "LDA"; // ZP X
            InstructionNameTable[0xAD] = "LDA"; // ABS
            InstructionNameTable[0xBD] = "LDA"; // ABS X
            InstructionNameTable[0xB9] = "LDA"; // ABS Y
            InstructionNameTable[0xA1] = "LDA"; // IND X
            InstructionNameTable[0xB1] = "LDA"; // IND Y
            // LDX
            InstructionNameTable[0xA2] = "LDX"; // Immediate
            InstructionNameTable[0xA6] = "LDX"; // ZP
            InstructionNameTable[0xB6] = "LDX"; // ZP Y
            InstructionNameTable[0xAE] = "LDX"; // ABS
            InstructionNameTable[0xBE] = "LDX"; // ABS Y
            // LDY
            InstructionNameTable[0xA0] = "LDY"; // Immediate
            InstructionNameTable[0xA4] = "LDY"; // ZP
            InstructionNameTable[0xB4] = "LDY"; // ZP X
            InstructionNameTable[0xAC] = "LDY"; // ABS
            InstructionNameTable[0xBC] = "LDY"; // ABS X
        }

    }
}
