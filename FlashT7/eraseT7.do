* Script for erasing the flash memory of SAAB Trionic 7 ECU
* Created by Patrik Servin
* Version 1.0
*
* Reset flash
mm 0xffff
0xf0f0.
* Erase procedure for Am29F400
mm 0xaaaa
0xaaaa.
*
mm 0x5555
0x5555.
*
mm 0xaaaa
0x8080.
*
mm 0xaaaa
0xaaaa.
*
mm 0x5555
0x5555.
*
mm 0xaaaa
0x1010.
* Reset flash
mm 0xffff
0xf0f0.
cls
* Flash should now be erased.
* Give the command "md 0x0" to verify that all addresses are set to "FFFF".
