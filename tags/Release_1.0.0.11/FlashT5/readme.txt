regdump.do
This script saves the values of some of the registers in the SIM
(System Integration Module). Nice to have if you need to restore them
at some time...
Make sure you execute this script before you start experimenting with
the flash.


prepT5.do 
Sets up some of the registers of SIM (System integration module)
for flashing. This script must be executed before any of the target
resident programs can be executed.

write00.s
Target resident program that writes 0x00 to all addresses of the flash.
This is a part of the erase algorithm for AM28F010.

eraseT5.s
Target resident program that erases all addresses of the flash.

dumpT5.s
Target resident program that dumps the content of the flash to a file.


My intention was to start with three target resident programs; write00, 
eraset5 and progT5 that togetheter implements the erase and write 
algorithms of AM28010. These three programs should then be combined
into one program.