// https://github.com/nwidger/nintengo/blob/master/m65go2/test-roms/nestest/nestest.log
using NESEmulator;

string path = args[0] ?? "test.nes";

NESMemoryBus bus = new NESMemoryBus();
NESRom rom = new NESRom(bus);
NESCpu cpu = new NESCpu(bus);
rom.Load(path);

Console.WriteLine("Successfully loaded ROM into memory!");
Console.WriteLine($"Reset vector: lo byte {bus.Read(0xFFFC).ToString("X2")} hi byte {bus.Read(0xFFFD).ToString("X2")}");

cpu.ExecuteAndLog(1, "./test.log");