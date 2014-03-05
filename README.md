Emulate is an 8 bit CPU emulation library written entirely in managed c#. The core emulation is contained in a Portable Class Library. The first CPUs to be emulated are the Intel 8080 and MOS 6502. I have a vague plan of doing the Z80 and 6809 at some point.

I tried to chose straightforward implementation over efficiency. If you want an efficient emulation there's loads of really good implementations. :)

The Altair8800 project is a start at emulating the Altair 8800. It isn't complete but will run various flavours of basic and cpm and such. It emulates a set of Altair 8" floppy drives and an SIO2 connected to a vt100 type terminal.

The Apple1 project is a loose emulation of the Apple 1. Type E000R to start BASIC. Make sure you use capslock with this as it doesn't understand lower case at all. No attempt has been made to emulate the casssette interface so there's no way to save anything you write with this.

I've tried to give credit to the others who figured out the tricky details in code comments.

Dave.