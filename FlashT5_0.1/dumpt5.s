* Program for dumping the flash memory from SAAB T5 ECU
* Created by Patrik Servin
* Version 1.0



	include ipd.inc
NUM_VEC	EQU	40	Exception vector

START	DC.L	PROG_START	
VEC_TAB	DS.L	NUM_VEC	Exception vector
FILE	DS.L	1	DOS - File handle
Start_Msg	DC.B	'Dumping T5 flash',13,10,0

FMODE	DC.B	'wb',0	Write privileges for file
	DS.W	0


PROG_START	LEA.L	(STACK,PC),A7	Stack pointer definition
	MOVE.L	D0,D7	Number of parameters
	MOVE.L	A0,A6	Pointer to the parameters
	CLR.L	(FILE,A5)	File pointer 

*Do something to the vector table
	LEA.L	(VEC_TAB,PC),A1
	MOVEC	A1,VBR	
	LEA.L	(EXCEPT,PC),A2	Address of Exception Handler
	MOVEQ	#NUM_VEC-1,D1	Number of vectors
INIT_VEC	 MOVE.L	A2,(A1)+	
	DBF	D1,INIT_VEC

* Give start message
	LEA.L	(Start_Msg,PC),A0
	MOVEQ	#BD_PUTS,D0	function call
	BGND

* Read parameters
* Command name is first parameter, file name is the second
	LEA.L	0,A4	Offset := 0
	CMP.W	#2,D7	Number of parameters
	BEQ	PAR_OK	Number of parameters (2) ok
	CMP.W	#3,D7
	BNE	PAR_ERR	Error handling for parameters
* Use second parameter (file name) for opening the file
PAR_OK	MOVE.L	(4,A6),A0	Pointer to file name
	LEA.L	(FMODE,PC),A1	File-Modus
	MOVEQ	#BD_FOPEN,D0	function call FOPEN
	BGND
	TST.W	D0
	BEQ	FILE_ERR	Error: file could not be opened
	MOVE.L	D0,(FILE,A5)	

* Dump flash to file
	MOVE.L #0,D7
	LEA.L	$40000,A0 Address to copy from 
READ_WRITE
	MOVE.L   (FILE,A5),D1     File handle
	MOVE.L   #256,D2       Number of bytes to copy
	MOVEQ #BD_FWRITE,D0
	BGND
	MOVE.B	#$2E,D1
	MOVEQ	#BD_PUTCHAR,D0
	BGND
	ADD   #1,D7
	ADD.W	#256,A0
	CMP.W	#1024,D7
	BGE PROG_END
	BRA READ_WRITE

* Fertig mit dem Programmieren
PROG_END	CLR.L	D7	kein Fehler
CLOSE_END	MOVE.L	(FILE,A5),D1	Filepointer
	BEQ	IST_ZU	wenn bereits zu, dann nicht wieder schlieúen
	MOVEQ	#BD_FCLOSE,D0	File wieder schlie§en
	BGND
IST_ZU	MOVE.L	D7,D1	vorheriger Fehler
	MOVEQ	#BD_QUIT,D0	Fertig
	BGND

FILE_W_ERR	MOVEQ	#1,D7	Error, could not write to file
	BRA	CLOSE_END

FILE_ERR	MOVEQ	#2,D7 Could not open file
	BRA	CLOSE_END

EXCEPT_ERR	MOVEQ	#3,D7	Exception 
	BRA	CLOSE_END

PAR_ERR	MOVEQ	#4,D7 Wrong number of parameters
	BRA	CLOSE_END

* Handler fŸr beliebige Exceptions
EXCEPT	BRA	EXCEPT_ERR	nur Fehlermeldung ausgeben


* Ende des Programms
	DS.W	100	Stack ist 100 Worte gro§
STACK	DS.W	1

	END
