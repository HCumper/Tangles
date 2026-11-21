100 REMark Tangles - restart after the runtime extensions have been loaded.
110 tanglesPath$=DATAD$
120 IF LEN(tanglesPath$)<5 THEN PRINT #0,"Set DATA_USE to the Tangles directory first." : STOP
130 tanglesTask$=tanglesPath$ & "tangles_exe"
140 IF tanglesPath$(1 TO 3)="nfa" THEN tanglesTask$="sfa" & tanglesPath$(4 TO LEN(tanglesPath$)) & "tangles_sfa_exe"
150 IF tanglesPath$(1 TO 3)="NFA" THEN tanglesTask$="sfa" & tanglesPath$(4 TO LEN(tanglesPath$)) & "tangles_sfa_exe"
160 IF tanglesPath$(1 TO 3)="sfa" THEN tanglesTask$=tanglesPath$ & "tangles_sfa_exe"
170 IF tanglesPath$(1 TO 3)="SFA" THEN tanglesTask$=tanglesPath$ & "tangles_sfa_exe"
180 IF tanglesPath$(1 TO 3)="dos" THEN tanglesTask$=tanglesPath$ & "tangles_sfa_exe"
190 IF tanglesPath$(1 TO 3)="DOS" THEN tanglesTask$=tanglesPath$ & "tangles_sfa_exe"
200 EXEC_W tanglesTask$
