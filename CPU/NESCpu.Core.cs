using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU
{


    internal partial class NESCpu
    {
        private readonly NESCpuRegisters _registers;
        private readonly NESMemoryBus _bus;
        private readonly NESCpuAddressModes _addressModes;
        private readonly Action[] _opcodeTable = new Action[0xFF];
        
        public NESCpu(NESMemoryBus bus)
        {
            _bus = bus;
            _registers = new NESCpuRegisters();
            _addressModes = new NESCpuAddressModes(_registers, _bus);
            this.InitOpcodesTable();
        }
        public void Reset()
        {
            // set pc to reset vector
            //byte lo = _bus.Read(0xFFFC);
            //byte hi = _bus.Read(0xFFFD);
            //registers.PC = (ushort)((hi << 16) | lo);
            _registers.Reset();
        }

        private void UpdateFlagsLd(byte value)
        {
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Zero, value == 0);
            this._registers.SetStatusRegisterFlag(NESCpuRegisters.StatusRegisterBit.Negative, (value & 0b1000) != 0);
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
                string logString = $"{_registers.PC.ToString("X4")}\t\t";
                byte op = _bus.Read(_registers.PC++);
                logString += $"{NESLoggingTables.InstructionNameTable[op] ?? "UNKNOWN"}\t\t{_registers.ToString()}";
                logData.Add(logString);

                _opcodeTable[op]?.Invoke(); // invoke specific opcode callback
            }


            File.WriteAllLines(logFilePath, logData);

        }
    }
}
