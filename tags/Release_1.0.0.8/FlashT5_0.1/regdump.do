* This file saves the values of some of the registers in the
* SIM (System Integration Module) to a file called regdump.txt.
log regdump.txt
md 0xfffa00 2
md 0xfffa04 2
md 0xfffa11 2
md 0xfffa13 2
md 0xfffa15 2
md 0xfffa17 2
md 0xfffa19 2
md 0xfffa1b 2
md 0xfffa1d 2
md 0xfffa1f 2
md 0xfffa21 2
md 0xfffa22 2
md 0xfffa24 2
md 0xfffa27 2
md 0xfffa41 2
md 0xfffa44 2
md 0xfffa46 2
md 0xfffa48 2
md 0xfffa4c 40
md 0xfffa4a 2
md 0xfffa4e 40
md 0xfffb00 2
md 0xfffb04 2
log off

