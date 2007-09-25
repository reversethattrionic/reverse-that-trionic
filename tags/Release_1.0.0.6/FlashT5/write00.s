* This target resident program programs 0x00 to all addresses
* of the flash.

	include ipd.inc
NUM_VEC	EQU	40	Anzahl Exception Vektoren
S9REC	EQU	2

START	DC.L	PROG_START	
VEC_TAB	DS.L	NUM_VEC	Exception Vektoren
Start_Msg	DC.B	'Erasing of 8-Bit AMD-Flashes',13,10,0
	DS.W	0


PROG_START	LEA.L	(STACK,PC),A7	Stackpointer definieren
	MOVE.L	D0,D7	Anzahl Parameter
	MOVE.L	A0,A6	Pointer auf die Parameter

* Vector table
	LEA.L	(START,PC),A1
	MOVEC	A1,VBR
	ADDQ.L	#4,A1
	LEA.L	(EXCEPT,PC),A2	Adresse Exception Handler
	MOVEQ	#NUM_VEC-1,D1	Anzahl Vektoren (DBF!)
INIT_VEC	 MOVE.L	A2,A1+
	DBF	D1,INIT_VEC

* Startup message
	LEA.L	(Start_Msg,PC),A0
	MOVEQ	#BD_PUTS,D0	function call
	BGND

	LEA.L		$70000,A0
	MOVE.L	#$19,D5		PLSCNT
	MOVE.L	#200,D2		Delay
	MOVE.L	#0,D6

		

	

WRITE_00		
	MOVE.W	#$4040,(A0)		Program setup
	MOVE.W	#$0,(A0)		Write 0x0
	MOVE.L	D2,D4

WAIT1
	SUBQ.L	#1,D4
	BCC		WAIT1

	MOVE.W	#$C0C0,(A0)		Program 
	MOVE.L	D2,D4
WAIT2
	SUBQ.L	#1,D4
	BCC		WAIT2
	CMP.W		#0,(A0)		Verify
	BEQ		NEXT_ADDR

	MOVE.B	#$2F,D1		Print / if PLSCNT (D5) need to be incremented.
	MOVEQ	#BD_PUTCHAR,D0
	BGND

	SUB.L		#1,D5
	CMP.L		#0,D5
	BNE		WRITE_00
	BRA		DEVICE_FAILED

NEXT_ADDR			
	MOVE.L	#$19,D5		Erase next address
	ADD.L		#2,A0
	ADD.L		#2,D6
	CMP.L		#1024,D6
	BNE		NEXT_ADDR2
	MOVE.B	#$2E,D1		Print a dot for each kB.
	MOVEQ	#BD_PUTCHAR,D0
	BGND	
	MOVE		#1,D6
NEXT_ADDR2
	CMP.L		#$70020,A0
	BEQ		RESET	

	MOVE.B	#$2E,D1	
	MOVEQ	#BD_PUTCHAR,D0
	BGND
	
	BRA		WRITE_00


RESET				
	MOVE.W	#$FFFF,$70000		Erase is completed. Time to reset flash.
	MOVE.W	#$FFFF,$70000
	BRA		THE_END
	

DEVICE_FAILED
      MOVEQ	#1,D7	Exception aufgetreten
	BRA	THE_END

EXCEPT_ERR	
	MOVEQ	#1,D7	Exception aufgetreten
	BRA	THE_END

EXCEPT	BRA	EXCEPT_ERR	nur Fehlermeldung ausgeben

THE_END

	
* Ende des Programms
	DS.W	100	Stack ist 100 Worte gro§
STACK	DS.W	1

	END
