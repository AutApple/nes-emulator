using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{


    internal class NESCpu
    {
        private readonly NESCpuRegisters registers;
        private readonly NESMemoryBus _bus;
        private readonly NESCpuAddressModes addressModes;
        public NESCpu(NESMemoryBus bus)
        {
            _bus = bus;
            registers = new NESCpuRegisters();
            addressModes = new NESCpuAddressModes(registers, _bus);
        }
        public void Reset()
        {
            // set pc to reset vector
            //byte lo = _bus.Read(0xFFFC);
            //byte hi = _bus.Read(0xFFFD);
            //registers.PC = (ushort)((hi << 16) | lo);
            registers.Reset();
        }

      
        
        private void JmpAbs()
        {
            byte lo = _bus.Read(registers.PC++);
            byte hi = _bus.Read(registers.PC++);
            ushort addr = (ushort)((hi << 8) | lo);
            registers.PC = addr;
        }
        private void JmpInd()
        {
            byte loAddr = _bus.Read(registers.PC++);
            byte hiAddr = _bus.Read(registers.PC++);
            ushort addr = (ushort)((hiAddr << 8) | loAddr);
            ushort loVal = _bus.Read(addr);
            ushort hiVal = _bus.Read((ushort)(addr + 1));
            ushort val = (ushort)((hiVal << 8) | loVal);
            registers.PC = val;
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
                string logString = $"{ registers.PC.ToString("X4") }\t";
                byte op = _bus.Read(registers.PC++);
                switch (op)
                {
                    // JMP
                    case 0x4C: // JMP (Absolute)
                        logString += $"JMP (ABSOLUTE)";
                        JmpAbs();
                        logString += $"\t{registers.ToString()}";
                        logData.Add(logString);
                        break; 
                    case 0x6C: // JMP (Indirect)
                        logString += $"JMP (INDIRECT)";
                        JmpInd();
                        logString += $"\t{registers.ToString()}";
                        logData.Add(logString);
                        break; 
                }
            }
            File.WriteAllLines(logFilePath, logData);

        }
    }
}
