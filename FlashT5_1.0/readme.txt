**********************************************************************
* Important note!
* 
* Some T5 flashes are so old that it is no longer possible to write 
* to them. Doing this will make them inoperable and the only way to
* fix them is to physicaly replace the flashes.
* Unfortunately the only way to determine the status of the flash
* is to write to it. A rumor says that AMD flashes are the worst.
*
********************************************************************** 

prepT5.do 
Sets up some of the registers of SIM (System integration module)
for flashing. This script must be executed before any of the target
resident programs can be executed.

flashT5.s
Writes a binary file to flash. Give the command "flashT5 <filename.bin>" 
in BD32 to flash a file called filename.in. prepT5.do must be executed
before flashT5 is used.

dumpT5.s
Target resident program that dumps the content of the flash to a file.

