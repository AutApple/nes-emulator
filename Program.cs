// https://github.com/nwidger/nintengo/blob/master/m65go2/test-roms/nestest/nestest.log
using NESEmulator;

string path = args[0] ?? "test.nes";

NESMemoryBus bus = new NESMemoryBus();
NESRom rom = new NESRom(bus);
NESCpu cpu = new NESCpu(bus);
rom.Load(path);

Console.WriteLine("Successfully loaded ROM into memory!");
int opcodesToExecute = 1;
cpu.ExecuteAndLog(opcodesToExecute, "./test.log");
Console.WriteLine($"Successfully executed {opcodesToExecute} opcodes and written to the test.log!");