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
        public void Reset()
        {
            // set pc to reset vector
            //byte lo = _bus.Read(0xFFFC);
            //byte hi = _bus.Read(0xFFFD);
            //this.PC = (ushort)((hi << 16) | lo);
            this.PC = 0xC000; // for nestest

            A = 0x00;
            X = 0x00;
            Y = 0x00;
            P = 0x24;
            S = 0xFD;
        }

        
        private void JmpAbs()
        {
            byte lo = _bus.Read(PC++);
            byte hi = _bus.Read(PC++);
            ushort addr = (ushort)((hi << 8) | lo);
            PC = addr;
        }
        private void JmpInd()
        {
            byte loAddr = _bus.Read(PC++);
            byte hiAddr = _bus.Read(PC++);
            ushort addr = (ushort)((hiAddr << 8) | loAddr);
            ushort loVal = _bus.Read(addr);
            ushort hiVal = _bus.Read((ushort)(addr + 1));
            ushort val = (ushort)((hiVal << 8) | loVal);
            PC = val;
        }


        private string RegisterDataToString()
        {
            return $"PC: {PC.ToString("X4")}, A: {A.ToString("X2")}, X: {X.ToString("X2")}, Y: {Y.ToString("X2")}, S: {S.ToString("X2")}, P: {P.ToString("X2")}";
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
                string logString = $"{ PC.ToString("X4") }\t";
                byte op = _bus.Read(PC++);
                switch (op)
                {
                    // JMP
                    case 0x4C: // JMP (Absolute)
                        logString += $"JMP (ABSOLUTE)";
                        JmpAbs();
                        logString += $"\t{RegisterDataToString()}";
                        logData.Add(logString);
                        break; 
                    case 0x6C: // JMP (Indirect)
                        logString += $"JMP (ABSOLUTE)";
                        JmpInd();
                        logString += $"\t{RegisterDataToString()}";
                        logData.Add(logString);
                        break; 
                }
            }
            File.WriteAllLines(logFilePath, logData);

        }
    }
}
