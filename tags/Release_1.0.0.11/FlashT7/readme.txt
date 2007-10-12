
Disclaimer
The author(s) of these files do not take any kind of responsibility 
for any damage that might be caused from using them. 

This directory contains scripts and programs that can be used
to erase and program the flash memory of a SAAB T7 ECU with BD32.
All the files should be put in the BD32 directory.

prepT7.do
	BD32 script file for preparing a SAAB T7 ECU for flash erasing/programming
	This script must be executed before any of the other sricpts/programs
	Type "do prepT7.do" in BD32 to execute the script.

eraseT7.do
	BD32 script file for erasing the flash in a SAAB T7 ECU.
	The script prepT7.do must be executed first.
	Type "do eraseT7.do" in BD32 to execute the script.

flashT7.d32
	Flash program for programming the flash in a SAAB T7 ECU.
	This program is "target resident" which means that it is executed in the ECU.
	Type "flashT7 <filename>" in BD32 to execute the program.
	Replace <filename> with a Motorola SREC file.

flashT7.msg
	Message file for flashT7.d32. Contains help and error messages.

flashT7.s
	Assembler source code for flashT7.d32.
	Not needed for flashing. Provided so others may improve the program.
	Can be compiled with AS32.

