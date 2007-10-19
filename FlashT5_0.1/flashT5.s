* Created by Patrik Servin for flashing AM28F010 in a SAAB Trionic 5 ECU with BD32.
* Inspiration from Ville Pietikainen and an unnamed BDM interface vendor.
* Version 1.0


			include ipd.inc
NUM_VEC		EQU	40	Number of exception vectors


START	DC.L		PROG_START	
VEC_TAB		DS.L	NUM_VEC	Exception vector
FILE			DS.L	1	DOS-Filehandle
BUFFER		DS.B	256	Data buffer
Start_Msg		DC.B	'Programming T5 flash',13,10,0
Erase_Msg   	DC.B  'Eraseing flash',13,10,0
Program_MSG 	DC.B	'Programming flash',13,10,0

FMODE			DC.B	'rb',0	Binary read mode
			DS.W	0


PROG_START		LEA.L	(STACK,PC),A7	Define stackpointer
			MOVE.L	D0,D7	Number of parameters
			MOVE.L	A0,A6	Pointer to parameters
			CLR.L	(FILE,A5)	Filepointer 

*Set up vector table
			LEA.L	(VEC_TAB,PC),A1
			MOVEC	A1,VBR	vector table
			LEA.L	(EXCEPT,PC),A2	Address of exception handler
			MOVEQ	#NUM_VEC-1,D1	Anzahl Vektoren (DBF!)
INIT_VEC		MOVE.L	A2,(A1)+ Entry point
			DBF	D1,INIT_VEC

* Put start message on display
			LEA.L	(Start_Msg,PC),A0
			MOVEQ	#BD_PUTS,D0	function call
			BGND

* Test parameters
			LEA.L	0,A4	Offset := 0
			CMP.W	#2,D7 	Number of parameters
			BEQ	OPEN_FILE  	2 paramer is ok
			BRA	PAR_ERR	Wrong number of parameters

OPEN_FILE
			MOVE.L	(4,A6),A0	Pointer to file name
			LEA.L	(FMODE,PC),A1	File modus
			MOVEQ	#BD_FOPEN,D0	function call FOPEN
			BGND
			TST.W	D0
			BEQ	FILE_ERR	Error: File not found
			MOVE.L	D0,(FILE,A5)	Save file handle


*
* Erase module
*
* A0 = starting address
* D0 = lenght in bytes
* A1 = zero
			LEA.L	(Erase_Msg,PC),A0
			MOVEQ	#BD_PUTS,D0	function call
			BGND
BEGIN:		MOVE.L 	 #1000,D2
			LEA.L		 0,A1
			MOVEA.L	 #$00040000,A0	;start addr = 0
			MOVE.L	 #$00040000,D0  ;length = 40000

WRITE00:		

			MOVE.B	 #$19,D5
Loop1:		MOVE.W	 #$4040,(A0)	;Program CMD
			MOVE.W	 #$0000,(A0)	;program to zero
			MOVE.L	 D2,D4
Delay1:		SUBQ.L	 #$1,D4
			BCC	 	 Delay1
			MOVE.W	 #$C0C0,(A0)	;Program verify CMD
			MOVE.L	 D2,D4
Delay2:		SUBQ.L	 #$1,D4
			BCC	 	 Delay2
			CMPI.W	 #$0000,(A0)	;Is it zero?
			BEQ	 	 YesZero	;yep
			SUBQ.B	 #$1,D5
			BNE	 	 Loop1		;try again
Exit1:		
			MOVE.L	 #$FFFFFFFF,D0
			BRA		 ERASE_ERR

YesZero:		ADDQ.L	 #$2,A0
			SUBQ.L	 #$2,D0
			BNE	 	 WRITE00	;loop
			MOVE.W	 #$0000,($00040000).L
			

ERASE:	
			MOVE.L 	 #1000,D2
			LEA.L		 0,A1
			MOVEA.L	 #$00040000,A0	;start addr = 0
			MOVE.L	 #$00040000,D0  ;length = 40000
			MOVE.W	 #$03E8,D5
Loop3:		MOVE.W	 #$2020,(A0)	;Erase CMD
			MOVE.W	 #$2020,(A0)	;Erase CMD
			MOVE.L	 D2,D4
			MULU.L	 #$000003E8,D4
Delay3:		SUBQ.L	 #$1,D4
			BCC	 	 Delay3		;long delay
			MOVE.W	 #$A0A0,(A0)	;Erase verify CMD
			MOVE.L	 D2,D4
Delay4:		SUBQ.L	 #$1,D4
			BCC	       Delay4
Loop2:		CMPI.W	 #$FFFF,(A0)	;was it erased?
			BEQ	       Exit2		;yep
			SUBQ.W	 #$1,D5
			BEQ	       ERASE_ERR
			BRA	 	 Loop3		;try again
Exit2:		ADDQ.L	 #$2,A0
			SUBQ.L	 #$2,D0
			BNE	       Loop2		;loop
			MOVE.W	 #$0000,($00040000).L
			

ExitErase:		
			MOVE.W	 #$0000,($00040000).L		
			BRA PROGRAM

*
* Program words address
*
* D3 = lenght of the block in bytes
* A3 = starting address of the module to be programmed
* A0 = address of the buffer
PROGRAM:		LEA.L	(Start_Msg,PC),A0
			MOVEQ	#BD_PUTS,D0	function call
			BGND
			LEA.L  $40000,A3			Starting address
FILL_BUFFER:
			MOVE.B	#$2E,D1		Print dot
			MOVEQ		#BD_PUTCHAR,D0
			BGND	
			MOVE.L 	#256,D3		Lenght of block in bytes
			MOVE.L 	#256,D2		Number of bytes to read
			MOVE.L 	(FILE,A5),D1	File handle
			LEA.L 	(BUFFER,A5),A0	Intermediate buffer
			MOVE.L 	#256,D0		Number of bytes to put in intermediate buffer
			MOVEQ		#BD_FREAD,D0	function call FREAD
			BGND
			MOVE.W 	#1000,D2		Wait time constant (should be somewhere around 150?)


ProgramWords:		
			MOVE.B	#$19,D5
Loop4:		MOVE.W	#$4040,(A3)		Program CMD
			MOVE.W	(A0),(A3)		pgm word
			MOVE.L	D2,D0
Delay5:		SUBQ.L	#$1,D0
			BCC	 	Delay5
			MOVE.W	#$C0C0,(A3)		Program verify CMD
			MOVE.L	D2,D0
Delay6:		SUBQ.L	#$1,D0
			BCC	 	Delay6
			MOVE.W	(A0),D6		get data from buffer
			CMP.W	 	(A3),D6		compare to flash
			BEQ	 	Done			equal->next one
			SUBQ.B	#$1,D5
			BNE	 	Loop4			try again
			MOVE.W	#$0000,($00040000).L
			BRA		PROG_ERR

Done:			
			ADDQ.L	#$2,A0
			ADDQ.L	#$2,A3
			SUBQ.L	#$2,D3
			BNE	 	ProgramWords	loop
			CMP.L		#$80000,A3
			BNE		FILL_BUFFER
			MOVE.W	#$0000,($00040000).L
			CLR.L	 	D0


* Programing done
PROG_END		CLR.L		D7			No errors
CLOSE_END		MOVE.L	(FILE,A5),D1	Filepointer
			BEQ		IST_ZU	
			MOVEQ		#BD_FCLOSE,D0	Close file
			BGND

IST_ZU		MOVE.L	D7,D1			
			MOVEQ		#BD_QUIT,D0		
			BGND

OFFS_ERR		MOVEQ		#1,D7			Error in offset. Should not be used in this program
			BRA		CLOSE_END

SREC_ERR		MOVEQ		#2,D7			Invalid srec type. Should not be used in this program.
			BRA		CLOSE_END

FILE_ERR		MOVEQ		#3,D7			Error opening file.
			BRA		CLOSE_END

EXCEPT_ERR		MOVEQ		#4,D7			Exception
			BRA		CLOSE_END

PROG_ERR		MOVEQ		#5,D7			Programming error
			BRA		CLOSE_END

PAR_ERR		MOVEQ		#6,D7			Wrong number of parameters
			BRA		CLOSE_END

VER_ERR		MOVEQ		#7,D7			Verify error
			BRA		CLOSE_END

ZAHL_ERR		MOVEQ		#8,D7			Wrong number of bytes
			BRA		CLOSE_END

ERASE_ERR		MOVEQ		#10,D7		Erase error
			BRA		CLOSE_END

* Exception handler
EXCEPT		BRA		EXCEPT_ERR

* Time limit overrun error
PROG_ERRA		MOVEQ		#-1,D0
			RTS

PROG_OK		CLR.W	D0				No errors
			RTS

* End the program
			DS.W		100			Stack is 100 
STACK			DS.W		1

			END
