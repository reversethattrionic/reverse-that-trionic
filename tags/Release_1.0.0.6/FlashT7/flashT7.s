* Original file is called Amdp16.s
* Modified by Patrik Servin for flashing Amd29F400 in a SAAB Trionic 7 ECU.
* Version 1.0


* BD32-Treiber zum Programmieren externer Flashes (16-Bit breit)
* vorher wurden bereits die Chip-Selects definiert
* A4 wird für den Offset verwendet.



	include ipd.inc
NUM_VEC	EQU	40	Anzahl Exception Vektoren
S9REC	EQU	2	Kennzeichnung S9-Records

* Anfang des Treibers
START	DC.L	PROG_START	Adresse Anfang des Treibers
VEC_TAB	DS.L	NUM_VEC	Exception Vektoren
FILE	DS.L	1	DOS-Filehandle
BUFFER	DS.W	50	S-Record-Buffer
Start_Msg	DC.B	'Programming of 16-Bit AMD-Flashes',13,10,0

FMODE	DC.B	'r',0	Files zum Lesen aufmachen
	DS.W	0


PROG_START	LEA.L	(STACK,PC),A7	Stackpointer definieren
	MOVE.L	D0,D7	Anzahl Parameter
	MOVE.L	A0,A6	Pointer auf die Parameter
	CLR.L	(FILE,A5)	Filepointer vorher lîschen

*Vektortabelle aufbauen
	LEA.L	(VEC_TAB,PC),A1
	MOVEC	A1,VBR	VBR=Anfang Vektortabelle
	LEA.L	(EXCEPT,PC),A2	Adresse Exception Handler
	MOVEQ	#NUM_VEC-1,D1	Anzahl Vektoren (DBF!)
INIT_VEC	 MOVE.L	A2,(A1)+	Pointer eintragen
	DBF	D1,INIT_VEC

* Start-Message ausgeben
	LEA.L	(Start_Msg,PC),A0
	MOVEQ	#BD_PUTS,D0	function call
	BGND

* Parameter prüfen
* Kommando ist der erste Parameter, Filename der zweite
	LEA.L	0,A4	Offset := 0
	CMP.W	#2,D7	Anzahl Parameter
	BEQ	PAR_OK	Anzahl 2 ok
	CMP.W	#3,D7
	BNE	PAR_ERR	wenn nicht 3, dann Fehler
* Jetzt das File öffnen; der zweite Parameter ist der Filename
PAR_OK	MOVE.L	(4,A6),A0	Pointer auf Filename
	LEA.L	(FMODE,PC),A1	File-Modus
	MOVEQ	#BD_FOPEN,D0	function call FOPEN
	BGND
	TST.W	D0
	BEQ	FILE_ERR	Error: File nicht vorhanden
	MOVE.L	D0,(FILE,A5)	Filehandle abspeichern

* Offset einlesen
	CMP.W	#3,D7
	BNE	DO_SREC	wenn nicht 3, dann kein Offset
	MOVE.L	(8,A6),A0	Pointer auf Offset
	MOVEQ	#BD_EVAL,D0	function call EVAL Param
	BGND
	TST.W	D0
	BNE	OFFS_ERR	Error: Fehler im Offset
	MOVE.L	D1,A4	Offset merken

* File ist offen, also jetzt programmieren
DO_SREC	MOVE.L	(FILE,A5),D1	Filepointer
	LEA.L	(BUFFER,A5),A0	Adresse des Buffers
	MOVEQ	#BD_FREADSREC,D0	function call S-Record lesen
	BGND
	CMP.W	#S9REC,D0
	BEQ	PROG_END	S9 ist der letzte Record
	TST.W	D0
	BNE	SREC_ERR	Fehler im S-Record
	TST.B	(A0)	S-Record Typ
	BEQ	DO_SREC	S0-Record überlesen
	LEA.L	(BUFFER,A5),A0	Adresse des Buffers
* A0 Pointer auf Daten im S-Record (Source)
	BSR	PROG_RECORD	den Record programmieren
	MOVE.B	#$2E,D1	Pro S-Record einen Punkt ausgeben
	MOVEQ	#BD_PUTCHAR,D0
	BGND
	BRA	DO_SREC

* Fertig mit dem Programmieren
PROG_END	CLR.L	D7	kein Fehler
CLOSE_END	MOVE.L	(FILE,A5),D1	Filepointer
	BEQ	IST_ZU	wenn bereits zu, dann nicht wieder schlie˙en
	MOVEQ	#BD_FCLOSE,D0	File wieder schließen
	BGND
IST_ZU	MOVE.L	D7,D1	vorheriger Fehler
	MOVEQ	#BD_QUIT,D0	Fertig
	BGND

OFFS_ERR	MOVEQ	#1,D7	Fehler im Offset
	BRA	CLOSE_END

SREC_ERR	MOVEQ	#2,D7	kein gültiger S-Record Typ
	BRA	CLOSE_END

FILE_ERR	MOVEQ	#3,D7	Fehler beim Öffnen des Files
	BRA	CLOSE_END

EXCEPT_ERR	MOVEQ	#4,D7	Exception aufgetreten
	BRA	CLOSE_END

PROG_ERR	MOVEQ	#5,D7	Eine Speicherzelle konnte nicht programmiert werden
	BRA	CLOSE_END

PAR_ERR	MOVEQ	#6,D7	Anzahl Parameter stimmt nicht
	BRA	CLOSE_END

VER_ERR	MOVEQ	#7,D7	Verify ergibt Fehler
	BRA	CLOSE_END

ZAHL_ERR	MOVEQ	#8,D7	Anzahl Bytes sind ungerade
	BRA	CLOSE_END

* Handler für beliebige Exceptions
EXCEPT	BRA	EXCEPT_ERR	nur Fehlermeldung ausgeben

* A0 Pointer auf Daten im S-Record (Source)
PROG_RECORD	CLR.L	D7	es wird im folgenden nur ein Byte gelesen!!
	MOVE.B	(1,A0),D7	Anzahl Byte im S-Record
	SUBQ.B	#4,D7	Adresse ignorieren, nur Anzahl Datenbyte
* im 8-Bit Modus darf die Anzahl Bytes pro Zeile ungerade sein
	BTST.L	#0,D7	Gerade?
	BNE	ZAHL_ERR	bei ungerade Fehler
	ADDQ.L	#2,A0	Poitionieren auf Adresse
	MOVE.L	(A0)+,A1	Zieladresse aus dem S-Record
	TST.W	D7
	BEQ.B	PROG_RET	Wenn keine Daten im Record, dann Ende
	LSR.W	#1,D7	wortweise !!!
	SUBQ.W	#1,D7	wegen DBRA
NEXT_WORD	 MOVE.W	(A0)+,D1	zu programmierendes Wort
	 BSR	PROG_WORT	ein Byte programmieren
	 TST.W	D0	Fehler?
	 BEQ.B	NO_PROG_ERR
	 BSR	AMD_RESET
	 BSR	PROG_WORT	bei Fehler nochmals versuchen
	 TST.W	D0	Fehler?
	 BNE.B	PROG_ERR	Wenn wieder Fehler, dann Ende
NO_PROG_ERR	 CMP.W	(A1)+,D1	Verify
	 BNE	VER_ERR	bei ungleich, Verify Error
	DBF	D7,NEXT_WORD
PROG_RET	RTS

* Ein Wort programmieren
* A1 zu programmierende Adresse (darf nicht verändert werden)
* D1 zu programmierender Wert	(darf nicht verändert werden)
* Rückgabe D0=0: kein Fehler, sonst Programmierfehler
PROG_WORT	MOVE.W	#$AAAA,($5555*2,A4)	Spezialzyklen gem. AMD-Handbuch
	MOVE.W	#$5555,($2AAA*2,A4)	speziell für Byteweisen Anschluß von
	MOVE.W	#$A0A0,($5555*2,A4)	16-Bit breiten Flashes
	MOVE.W	D1,(A1)	Wort programmieren
	NOP		Warten, bis Schreibvorgang fertig

* jetzt warten, bis die Programmierung fertig ist
* D7 ist invertiert wahrend des Programmiervorgangs
* D5=1 bei Programmierfehler
PROG_WAIT	MOVE.W	(A1),D3	Wert zurücklesen
* erst unteres Byte testen
	MOVE.W	D1,D2	Wert kopieren für Berechnungen
	AND.W	#$80,D2	Low-Byte Bit 7 zeigt an, wenn fertig
	MOVE.W	D3,D4	gelesenen Wert kopieren (Arbeitswert)
	AND.W	#$80,D4	nur D7 betrachten (low Byte)
	CMP.W	D4,D2	D7-soll mit D7-ist vergleichen
	BEQ.B	Tst_High	Low-Byte ist fertig (korrekt)
	BTST	#5,D3	gelesenen Wert testen !!
	BNE	PROG_ERRA	D7 flasch, D5=1: Time Limit
	BRA	PROG_WAIT

* dasselbe für das obere Byte
Tst_High	MOVE.W	D1,D2	Wert kopieren für Berechnungen
	AND.W	#$8000,D2	High-Byte Bit 7 zeigt an, wenn fertig
	MOVE.W	D3,D4	gelesenen Wert kopieren (Arbeitswert)
	AND.W	#$8000,D4	nur D7 betrachten (High Byte)
	CMP.W	D4,D2	D7-soll mit D7-ist vergleichen
	BEQ.B	PROG_OK	High-Byte ist auch fertig (korrekt)
	BTST	#5+8,D3	High-Byte teste
	BNE	PROG_ERRA	D7 flasch, D5=1: Time Limit
	BRA	PROG_WAIT	kein Time-Limit: weiter warten

* Fehler: Time limit überschritten (bei mindestens einem Baustein)
PROG_ERRA	MOVEQ	#-1,D0	Rückgabewert: Fehler
	RTS

PROG_OK	CLR.W	D0	Rückgabe: kein Fehler
	RTS

AMD_RESET
	MOVE.W	#$F0,($5555*2,A4)	16-Bit breiten Flashes
	MOVE.W	(A4),D0	Dummy-Lesen
	RTS

* Ende des Programms
	DS.W	100	Stack ist 100 Worte groß
STACK	DS.W	1

	END
