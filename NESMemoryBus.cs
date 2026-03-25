using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator
{
    internal class NESMemoryBus
    {
        private readonly byte[] _ram = new byte[0x800];
        private readonly byte[] _ppu = new byte[0x008];
        private readonly byte[] _apu = new byte[0x017];

        private readonly byte[] _prgRam = new byte[0x2000];
        private byte[] _prgRom = new byte[0x8000];

        public void Write(byte data, ushort addr)
        {
            if (addr <= 0x1FFF) // map RAM
                _ram[addr & 0x07FF] = data;
            if (addr <= 0x3FFF) // map PPU
                _ppu[addr % 0x0008] = data; // repeats each 8 bytes
            if (addr <= 0x4017) // map APU
                _apu[addr - 0x4000] = data;
            if (addr <= 0x5FFF)
                return; // unused / mapper expansion
            if (addr <= 0x7FFF)
                _prgRam[addr - 0x6000] = data;  // cartridge RAM
            _prgRom[addr - 0x8000] = data;  // cartridge ROM (everything >= 0x8000 falls here)
        } 
        public byte Read(ushort addr)
        { 
            if (addr <= 0x1FFF) // map RAM
                return _ram[addr & 0x07FF];
            if (addr <= 0x3FFF) // map PPU
                return _ppu[addr % 0x0008]; // repeats each 8 bytes
            if (addr <= 0x4017) // map APU
                return _apu[addr - 0x4000];
            if (addr <= 0x5FFF)
                return 0; // unused / mapper expansion
            if (addr <= 0x7FFF)
                return _prgRam[addr - 0x6000];  // cartridge RAM
            return _prgRom[(addr - 0x8000) % _prgRom.Length];  // cartridge ROM (everything >= 0x8000 falls here) + mirroring across 
        }


        public void WriteROM(byte[] romData)
        {
            _prgRom = romData;
        }


    }
}
