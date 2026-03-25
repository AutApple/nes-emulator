
namespace NESEmulator;
internal class NESRom(NESMemoryBus bus)
{
    private readonly NESMemoryBus _bus = bus;
    
    public void Load(string romPath)
    {
        using var f = File.OpenRead(romPath);
        using BinaryReader reader = new BinaryReader(f);
        // read the rom...
        byte[] headerSignature = [0x4E, 0x45, 0x53, 0x1A];

        byte[] header = reader.ReadBytes(4);
        if (!header.SequenceEqual(headerSignature))
            throw new Exception("Unexpected header signature. Did you provide valid .nes file?");
        byte prgRomBanksAmount = reader.ReadByte(); // * 16 KB 
        reader.ReadByte(); // chr rom * 8 KB, unused for now
        
        reader.ReadByte(); // flag 6 (skip for now)
        reader.ReadByte(); // flag 7 (skip for now)

        reader.ReadBytes(8); // unused in iNES 1.0 (skip)
        byte[] romData = reader.ReadBytes(prgRomBanksAmount * 16384); // read 16KB of rom banks
        _bus.WriteROM(romData);
    }
}
