using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{


    internal class NESCpu
    {
        private readonly NESCpuRegisters _registers;
        private readonly NESMemoryBus _bus;
        private readonly NESCpuAddressModes _addressModes;
        public NESCpu(NESMemoryBus bus)
        {
            _bus = bus;
            _registers = new NESCpuRegisters();
            _addressModes = new NESCpuAddressModes(_registers, _bus);
        }
        public void Reset()
        {
            // set pc to reset vector
            //byte lo = _bus.Read(0xFFFC);
            //byte hi = _bus.Read(0xFFFD);
            //registers.PC = (ushort)((hi << 16) | lo);
            _registers.Reset();
        }


        private void Lda(byte value) { 
            this._registers.A = value; 
        }
        private void Ldx(byte value)
        {
            this._registers.X = value;
        }
        private void Ldy(byte value)
        {
            this._registers.Y = value;
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


        // Method for testing purposes only. executes N instructions and writes the result to log
        public void ExecuteAndLog(int instructionsNumber, string logFilePath)
        {
            Reset();
            int instructionsExecuted = 0;
            List<string> logData = new List<string>();
            while (instructionsExecuted < instructionsNumber)
            {
                instructionsExecuted++;
                string logString = $"{ _registers.PC.ToString("X4") }\t";
                byte op = _bus.Read(_registers.PC++);
                switch (op)
                {
                    // JMP
                    case 0x4C: // JMP (Absolute)
                        logString += $"JMP (ABSOLUTE)";
                        JmpAbs();
                        break; 
                    case 0x6C: // JMP (Indirect)
                        logString += $"JMP (INDIRECT)";
                        JmpInd();
                        break;
                    // LDA
                    case 0xA9: // immediate
                        logString += $"LDA (IMMEDIATE)";
                        Lda(_addressModes.ReadImmediate());
                        break;
                    case 0xA5: // zero page
                        logString += $"LDA (ZP)";
                        Lda(_addressModes.ReadZeroPage());
                        break;
                    case 0xB5: // zp x
                        logString += $"LDA (ZPX)";
                        Lda(_addressModes.ReadZeroPageX());
                        break;
                    case 0xAD: // absolute
                        logString += $"LDA (ABS)";
                        Lda(_addressModes.ReadAbsolute());
                        break;
                    case 0xBD: // abs x
                        logString += $"LDA (ABS X)";
                        Lda(_addressModes.ReadAbsoluteX());
                        break;
                    case 0xB9: // abs y
                        logString += $"LDA (ABS Y)";
                        Lda(_addressModes.ReadAbsoluteY());
                        break;
                    case 0xA1: // ind x
                        logString += $"LDA (IND X)";
                        Lda(_addressModes.ReadIndirectX());
                        break;
                    case 0xB1: // ind y
                        logString += $"LDA (IND Y)";
                        Lda(_addressModes.ReadIndirectY());
                        break;
                    // LDX
                    case 0xA2: // immediate
                        logString += $"LDX (IMMEDIATE)";
                        Ldx(_addressModes.ReadImmediate());
                        break;
                    case 0xA6: // zp
                        logString += $"LDX (ZP)";
                        Ldx(_addressModes.ReadZeroPage());
                        break;
                    case 0xB6: // zp y
                        logString += $"LDX (ZP Y)";
                        Ldx(_addressModes.ReadZeroPageY());
                        break;
                    case 0xAE: // abs
                        logString += $"LDX (ABS)";
                        Ldx(_addressModes.ReadAbsolute());
                        break;
                    case 0xBE: // absY
                        logString += $"LDX (ABS Y)";
                        Ldx(_addressModes.ReadAbsoluteY());
                        break;
                    // LDY
                    case 0xA0: // immediate
                        logString += $"LDY (IMMEDIATE)";
                        Ldy(_addressModes.ReadImmediate());
                        break;
                    case 0xA4: // zp
                        logString += $"LDY (ZP)";
                        Ldy(_addressModes.ReadZeroPage());
                        break;
                    case 0xB4: // zp x
                        logString += $"LDY (ZPX)";
                        Ldy(_addressModes.ReadZeroPageX());
                        break;
                    case 0xAC: // abs
                        logString += $"LDY (ABS)";
                        Ldy(_addressModes.ReadAbsolute());
                        break;
                    case 0xBC: // absX
                        logString += $"LDY (ABS X)";
                        Ldy(_addressModes.ReadAbsoluteX());
                        break;

                }
                logString += $"\t{_registers.ToString()}";
                logData.Add(logString);
            }


            File.WriteAllLines(logFilePath, logData);

        }
    }
}
