100 REMark Tangles 1.00 - load extensions once per interpreter session.
110 tanglesPath$=DATAD$
120 IF LEN(tanglesPath$)<5 THEN PRINT #0,"Set DATA_USE to the Tangles directory first." : STOP
130 IF FTEST(tanglesPath$ & "tangles_catalog")<>0 THEN PRINT #0,"Cannot find tangles_catalog in " & tanglesPath$ : STOP
140 LRESPR tanglesPath$ & "ttk"
150 LRESPR tanglesPath$ & "tptr_ext"
160 LRUN tanglesPath$ & "start"
