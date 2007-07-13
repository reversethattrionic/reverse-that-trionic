* Target resident program form erasing T5 flash.
* 0x00 should be written to all addresses of the memory
* before this program is executed


	include ipd.inc
NUM_VEC	EQU	40	Exception vector
S9REC	EQU	2

* Anfang des Treibers
START	DC.L	PROG_START
VEC_TAB	DS.L	NUM_VEC	Exception Vector
Start_Msg	DC.B	'Erasing of 8-Bit AMD-Flashes',13,10,0
	DS.W	0


PROG_START	LEA.L	(STACK,PC),A7	Stackpointer definieren
	MOVE.L	D0,D7	Nr of parameters
	MOVE.L	A0,A6	Pointer to parameters

*Vector table
	LEA.L	(START,PC),A1
	MOVEC	A1,VBR
	ADDQ.L	#4,A1
	LEA.L	(EXCEPT,PC),A2	Address of Exception Handler
	MOVEQ	#NUM_VEC-1,D1	
INIT_VEC	 MOVE.L	A2,A1+
	DBF	D1,INIT_VEC

* Startup message
	LEA.L	(Start_Msg,PC),A0
	MOVEQ	#BD_PUTS,D0	function call
	BGND

	LEA.L		0,A0
	MOVE.L	#$19,D5
	MOVE.L	#2,D2
	MOVE.L	#0,D6
	
ERASE

	MOVE.W	#$2020,0x0		Erase setup
	MOVE.W	#$2020,0x0		Erase write
	MOVE.L	D2,D4
	MULU.L	#$3E8,D4		
WAIT3
	SUBQ.L	#1,D4
	CMP.L		#0,D4
	BNE.L		WAIT3

	MOVE.W	#$A0A0,0x0		Erase
	MOVE.L	D2,D4
WAIT4
	SUBQ.L	#1,D4
	CMP.L		#0,D4
	BNE.L		WAIT4

	CMP.B		#$FF,(A0)		Verify
	BEQ		NEXT_ADDR

	MOVE.B	#$2F,D1		Print / if PLSCNT is incremented
	MOVEQ	#BD_PUTCHAR,D0
	BGND

	SUB.L		#1,D5
	CMP.L		#0,D5
	BEQ		DEVICE_FAILED
	BRA 		ERASE

NEXT_ADDR
	ADD.L		#1,D6				Erase next address
	CMP		#1024,D6
	BNE		NEXT_ADDR2
	MOVE.B	#$2E,D1			Print a dot for every kB
	MOVEQ	#BD_PUTCHAR,D0
	BGND
	MOVE.L	#0,D6
NEXT_ADDR2
	ADD.L		#1,A0
	CMP.L		#$40000,A0
	BEQ		RESET
	MOVE.L	#$19,D5
	BRA 		ERASE




RESET
	MOVE.B	#$FFFF,0x0	
	MOVE.B	#$FFFF,0x0
	BRA		THE_END
	

DEVICE_FAILED MOVEQ	#1,D7	
	MOVE.L	#$4040,(A0)
	MOVE.L	#$FFFF,(A0)	
	MOVE.L	#$FFFF,(A0)
	BRA	THE_END

EXCEPT_ERR	MOVEQ	#1,D7	
	BRA	THE_END

EXCEPT	BRA	EXCEPT_ERR	

THE_END
	
* End the program
	DS.W	100
STACK	DS.W	1

	END
