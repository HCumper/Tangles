1 TURBO_objfil "ram1_target_exe"
2 TURBO_repfil "nfa1_report"
3 TURBO_taskn "Tangles generator"
4 TURBO_windo 0
5 TURBO_objdat 8192
6 TURBO_buffersz 64
7 TURBO_list 0
8 TURBO_model ">"
9 TURBO_optim 0
10 MANIFEST : G_MAX_SIDE% = 64
11 MANIFEST : G_MAX_CELLS% = 4096
12 MANIFEST : G_SOLVER_SIDE% = 16
13 MANIFEST : G_SEARCH_NODES = 20000
14 MANIFEST : G_ROUTE_CAP% = 1024
15 OPT_WIDTH% = 0
16 OPT_HEIGHT% = 0
17 OPT_PAIRS% = 0
18 OPT_MINIMUM% = 0
19 OPT_MAXIMUM% = 0
20 OPT_ATTEMPTS% = 0
21 OPT_COUNT% = 0
22 OPT_FULL% = 0
23 OPT_VARIED% = 0
24 OPT_SEED = 0
25 OPT_CANDIDATES% = 0
26 OPT_IMPROVE% = 0
27 OPT_SOLVERWORK = 0
28 OPT_TARGET% = 0
29 OPT_MINTRIALS = 0
30 OPT_MAXTRIALS = 0
31 OPT_MINDEPTH% = 0
32 OPT_MINNODES% = 0
33 OPT_ASSESS% = 0
34 OPT_UNIQUE% = 0
35 OPT_SHORTEST% = 0
36 OPT_SHOW% = 0
37 OPT_FORCE% = 0
38 DIM OPT_OUTPUT$(200)
39 OPT_OUTPUT$ = ""
40 OPT_RAMPFIRST% = 0
41 OPT_RAMPLAST% = 0
42 WORK_BOARDATTEMPTS = 0
43 WORK_TRAVERSALEVALUATIONS = 0
44 WORK_REFINEMENTEVALUATIONS = 0
45 WORK_STRUCTUREDSTARTS = 0
46 DIM SR_STATUS$(20)
47 SR_STATUS$ = ""
48 DIM SR_LIMITREASON$(128)
49 SR_LIMITREASON$ = ""
50 DIM SR_TIER$(20)
51 SR_TIER$ = ""
52 DIM SR_GRADE$(12)
53 SR_GRADE$ = ""
54 SR_SCORE% = 0
55 SR_LOGICAL% = 0
56 SR_LOCALDEDUCTIONS = 0
57 SR_ROUTEDEDUCTIONS = 0
58 SR_ROUTEPASSES = 0
59 SR_TRIALSTEPS = 0
60 SR_TRIALASSUMPTIONS = 0
61 SR_SEARCHNODES = 0
62 SR_FIRSTNODES = 0
63 SR_FIRSTDEPTH% = 0
64 SR_MAXDEPTH% = 0
65 SR_SOLUTIONCOUNT% = 0
66 SR_EXHAUSTIVE% = 0
67 DIM SR_UNIQUENESS$(20)
68 SR_UNIQUENESS$ = ""
69 SR_WORK = 0
70 SR_WORKLIMIT = 0
71 DIM SR_BOARDKEY$(64)
72 SR_BOARDKEY$ = ""
73 DIM gError$(255)
74 DIM gCommand$(4096)
75 DIM gOutput$(200)
76 DIM gSpool$(220)
77 DIM gTemp$(220)
78 DIM gBackup$(220)
79 DIM gOwner%(0)
80 DIM gOrder%(0)
81 DIM gLength%(0)
82 DIM gStart%(0)
83 DIM gFinish%(0)
84 DIM gEndpoint%(0)
85 DIM gNb%(0,3)
86 DIM gDegree%(0)
87 DIM gQueue%(0)
88 DIM gScratch%(0)
89 DIM gLocal%(0)
90 DIM gRng(2,55)
91 DIM gRi%(2)
92 DIM gRj%(2)
93 DIM gPower(32)
94 DIM gSeen$(0,64)
95 DIM gBatchKeys$(0,64)
96 DIM gSavedOwner%(0)
97 DIM gSavedOrder%(0)
98 DIM gCuts%(0)
99 DIM gEnds%(0)
100 DIM gNeeded%(0)
101 DIM gTraversal%(0)
102 DIM gPositions%(0)
103 DIM gDP%(0,0)
104 DIM gPrefix%(0)
105 DIM gWalk%(0)
106 DIM gWalkPos%(0)
107 DIM gBestWalk%(0)
108 DIM gGrowthCandidates%(0,3)
109 DIM gGrowthCount%(0)
110 DIM gGrowthAt%(0)
111 DIM gGrowthPath%(0)
112 DIM gCanonical%(0)
113 DIM gA%(0)
114 DIM gB%(0)
115 DIM gSymA%(0)
116 DIM gSymB%(0)
117 DIM gBaseA%(0)
118 DIM gBaseB%(0)
119 gExit% = 0
120 gCancel% = 0
121 gPoll = 0
122 gInput% = -1
123 gRngStream% = 0
124 gError$ = ""
125 gN% = 0
126 gValid% = 0
127 gSuccess% = 0
128 gSelected% = 0
129 gHasReason% = 0
130 gHasShortest% = 0
131 DEFine PROCedure GEN_DEFAULTS
132 OPT_WIDTH% = 0
133 OPT_HEIGHT% = 0
134 OPT_PAIRS% = 0
135 OPT_MINIMUM% = 0
136 OPT_MAXIMUM% = 0
137 OPT_ATTEMPTS% = 0
138 OPT_COUNT% = 0
139 OPT_FULL% = 0
140 OPT_VARIED% = 0
141 OPT_SEED = 0
142 OPT_CANDIDATES% = 0
143 OPT_IMPROVE% = 0
144 OPT_SOLVERWORK = 0
145 OPT_TARGET% = 0
146 OPT_MINTRIALS = 0
147 OPT_MAXTRIALS = 0
148 OPT_MINDEPTH% = 0
149 OPT_MINNODES% = 0
150 OPT_ASSESS% = 0
151 OPT_UNIQUE% = 0
152 OPT_SHORTEST% = 0
153 OPT_SHOW% = 0
154 OPT_FORCE% = 0
155 DIM OPT_OUTPUT$(200)
156 OPT_OUTPUT$ = ""
157 OPT_RAMPFIRST% = 0
158 OPT_RAMPLAST% = 0
159 OPT_WIDTH%=8
160 OPT_HEIGHT%=8
161 OPT_PAIRS%=6
162 OPT_MINIMUM%=4
163 OPT_MAXIMUM%=16
164 OPT_ATTEMPTS%=500
165 OPT_COUNT%=1
166 OPT_CANDIDATES%=100
167 OPT_IMPROVE%=100
168 OPT_SOLVERWORK=2000000
169 OPT_MAXTRIALS=-1
170 OPT_TARGET%=0
171 OPT_RAMPFIRST%=0
172 OPT_RAMPLAST%=0
173 OPT_SEED=0
174 END DEFine
175 DEFine PROCedure GEN_POLL
176 LOCal gpKey$(1)
177 gPoll = gPoll+1
178 IF gInput%<0 OR gPoll MOD 256<>0 THEN
179 RETurn 
180 END IF
181 gpKey$ = INKEY$(#gInput%,0)
182 IF LEN(gpKey$) THEN
183 IF CODE(gpKey$)=3 OR CODE(gpKey$)=27 THEN
184 gCancel% = 1
185 gExit% = 130
186 gError$ = "Generation cancelled."
187 END IF
188   END IF
189 END DEFine
190 DEFine FuNction GEN_MIN(GMA,GMB)
191 IF gmA<gmB THEN
192 RETurn gmA
193 END IF
194 RETurn gmB
195 END DEFine
196 DEFine FuNction GEN_MAX(GMA,GMB)
197 IF gmA>gmB THEN
198 RETurn gmA
199 END IF
200 RETurn gmB
201 END DEFine
202 DEFine FuNction GEN_MOD(GMA,GMB)
203 RETurn gmA-gmB*INT(gmA/gmB)
204 END DEFine
205 DEFine FuNction GEN_PRODUCT(GMA,GMB)
206 RETurn gmA*gmB
207 END DEFine
208 DEFine FuNction GEN_ROUND(GRV)
209 IF grV<0 THEN
210 RETurn -INT(-grV+0.5)
211 END IF
212 RETurn INT(grV+0.5)
213 END DEFine
214 DEFine FuNction GEN_SIX(GSV)
215 RETurn gen_round((gsV*1000000))/1000000
216 END DEFine
217 DEFine FuNction GEN_LOG2(GLV)
218 RETurn LN(glV)/LN(2)
219 END DEFine
220 DEFine FuNction GEN_LABEL$(GLP%)
221 IF glP%<=26 THEN
222 RETurn CHR$(64+glP%)
223 END IF
224 RETurn gen_int$((glP%))
225 END DEFine
226 DEFine FuNction GEN_INT$(GIV)
227 LOCal giText$(32)
228 LOCal giDigit%
229 LOCal giNegative%
230 giText$ = ""
231 giNegative% = (giV<0)
232 giV = ABS(giV)
233   REPeat giLoop
234 giDigit% = gen_mod((giV),(10))
235 giText$ = CHR$(48+giDigit%) & giText$
236 giV = INT(giV/10)
237 IF giV=0 THEN
238 EXIT giLoop
239 END IF
240   END REPeat giLoop
241 IF giNegative% THEN
242 giText$ = "-" & giText$
243 END IF
244 RETurn giText$
245 END DEFine
246 DEFine FuNction GEN_NUMBER$(GNV)
247 LOCal gnWhole
248 LOCal gnFrac
249 LOCal gnText$(48)
250 LOCal gnPart$(8)
251 gnV = gen_six((gnV))
252 gnWhole = INT(ABS(gnV))
253 gnFrac = gen_round(((ABS(gnV)-gnWhole)*1000000))
254 IF gnFrac=1000000 THEN
255 gnWhole = gnWhole+1
256 gnFrac = 0
257 END IF
258 gnText$ = gen_int$((gnWhole))
259 IF gnFrac THEN
260 gnPart$ = gen_int$((gnFrac))
261 gnPart$ = FILL$("0",6-LEN(gnPart$)) & gnPart$
262     REPeat gnTrim
263 IF gnPart$(LEN(gnPart$))<>"0" THEN
264 EXIT gnTrim
265 END IF
266 gnPart$ = gnPart$(1 TO LEN(gnPart$)-1)
267     END REPeat gnTrim
268 gnText$ = gnText$ & "." & gnPart$
269   END IF
270 IF gnV<0 THEN
271 gnText$ = "-" & gnText$
272 END IF
273 RETurn gnText$
274 END DEFine
275 DEFine FuNction GEN_BOOL$(GBV%)
276 IF gbV% THEN
277 RETurn "true"
278 END IF
279 RETurn "false"
280 END DEFine
281 DEFine FuNction GEN_GRADE$(GGV%)
282   SELect ON ggV%
283 =1:
284 RETurn "easy"
285 =2:
286 RETurn "medium"
287 =3:
288 RETurn "hard"
289 =4:
290 RETurn "expert"
291   END SELect
292 RETurn "any"
293 END DEFine
294 DEFine FuNction GEN_RANK%(GRTEXT$)
295   SELect ON grText$
296 ="easy":
297 RETurn 1
298 ="medium":
299 RETurn 2
300 ="hard":
301 RETurn 3
302 ="expert":
303 RETurn 4
304   END SELect
305 RETurn 0
306 END DEFine
307 DEFine FuNction GEN_MODE$
308 IF OPT_FULL% THEN
309 RETurn "full-coverage"
310 END IF
311 RETurn "connect-only"
312 END DEFine
313 DEFine FuNction GEN_CUT$
314 IF OPT_VARIED% THEN
315 RETurn "varied"
316 END IF
317 RETurn "random"
318 END DEFine
319 DEFine FuNction GEN_LESS%(GEA$,GEB$)
320 LOCal geI%
321 LOCal geCount%
322 geCount% = gen_min((LEN(geA$)),(LEN(geB$)))
323   FOR geI%=1 TO geCount%
324 IF CODE(geA$(geI%))<CODE(geB$(geI%)) THEN
325 RETurn 1
326 END IF
327 IF CODE(geA$(geI%))>CODE(geB$(geI%)) THEN
328 RETurn 0
329 END IF
330   END FOR geI%
331 RETurn LEN(geA$)<LEN(geB$)
332 END DEFine
333 DEFine PROCedure GEN_BOARD
334 LOCal gbI%
335 LOCal gbJ%
336 gN% = OPT_WIDTH%*OPT_HEIGHT%
337 DIM gOwner%(gN%-1)
338 DIM gOrder%(gN%-1)
339 DIM gEndpoint%(gN%-1)
340 DIM gLocal%(gN%-1)
341 DIM gNb%(gN%-1,3)
342 DIM gDegree%(gN%-1)
343 DIM gQueue%(gN%-1)
344 DIM gScratch%(gN%-1)
345 DIM gLength%(OPT_PAIRS%)
346 DIM gStart%(OPT_PAIRS%)
347 DIM gFinish%(OPT_PAIRS%)
348 DIM gCanonical%(OPT_PAIRS%)
349 DIM gA%(OPT_PAIRS%)
350 DIM gB%(OPT_PAIRS%)
351 DIM gSymA%(OPT_PAIRS%)
352 DIM gSymB%(OPT_PAIRS%)
353 DIM gBaseA%(OPT_PAIRS%)
354 DIM gBaseB%(OPT_PAIRS%)
355   FOR gbI%=0 TO gN%-1
356 gbJ% = 0
357 IF gbI% MOD OPT_WIDTH%>0 THEN
358 gNb%(gbI%,gbJ%)=gbI%-1
359 gbJ% = gbJ%+1
360 END IF
361 IF gbI% MOD OPT_WIDTH%+1<OPT_WIDTH% THEN
362 gNb%(gbI%,gbJ%)=gbI%+1
363 gbJ% = gbJ%+1
364 END IF
365 IF gbI%>=OPT_WIDTH% THEN
366 gNb%(gbI%,gbJ%)=gbI%-OPT_WIDTH%
367 gbJ% = gbJ%+1
368 END IF
369 IF gbI%+OPT_WIDTH%<gN% THEN
370 gNb%(gbI%,gbJ%)=gbI%+OPT_WIDTH%
371 gbJ% = gbJ%+1
372 END IF
373 gDegree%(gbI%)=gbJ%
374 gOwner%(gbI%)=0
375 gOrder%(gbI%)=0
376 gEndpoint%(gbI%)=0
377   END FOR gbI%
378 END DEFine
379 DEFine FuNction GEN_ADJACENT%(GAA%,GAB%)
380 RETurn ABS((gaA% MOD OPT_WIDTH%)-(gaB% MOD OPT_WIDTH%))+ABS(INT(gaA%/OPT_WIDTH%)-INT(gaB%/OPT_WIDTH%))=1
381 END DEFine
382 DEFine PROCedure GEN_VALIDATE
383 LOCal gvI%
384 LOCal gvP%
385 LOCal gvJ%
386 LOCal gvN%
387 LOCal gvCount%
388 LOCal gvFirst%
389 LOCal gvLast%
390 LOCal gvPrevious%
391 gValid% = 0
392 DIM gvSeen%(gN%-1)
393 gvCount% = 0
394   FOR gvP%=1 TO OPT_PAIRS%
395 IF gLength%(gvP%)<OPT_MINIMUM% OR gLength%(gvP%)>OPT_MAXIMUM% THEN
396 RETurn 
397 END IF
398 gvPrevious% = -1
399 gvFirst% = -1
400 gvLast% = -1
401     FOR gvJ%=1 TO gLength%(gvP%)
402 gvN% = -1
403       FOR gvI%=0 TO gN%-1
404 IF gOwner%(gvI%)=gvP% AND gOrder%(gvI%)=gvJ% THEN
405 IF gvN%>=0 THEN
406 RETurn 
407 END IF
408 gvN% = gvI%
409         END IF
410       END FOR gvI%
411 IF gvN%<0 THEN
412 RETurn 
413 END IF
414 IF gvPrevious%>=0 THEN
415 IF NOT gen_adjacent%((gvPrevious%),(gvN%)) THEN
416 RETurn 
417 END IF
418       END IF
419 IF gvJ%=1 THEN
420 gvFirst% = gvN%
421 END IF
422 gvLast% = gvN%
423 gvPrevious% = gvN%
424 gvSeen%(gvN%)=1
425 gvCount% = gvCount%+1
426     END FOR gvJ%
427 IF gvFirst%<>gStart%(gvP%) OR gvLast%<>gFinish%(gvP%) THEN
428 RETurn 
429 END IF
430   END FOR gvP%
431   FOR gvI%=0 TO gN%-1
432 IF (gOwner%(gvI%)<>0)<>gvSeen%(gvI%) THEN
433 RETurn 
434 END IF
435 IF gOwner%(gvI%) THEN
436       FOR gvJ%=0 TO gDegree%(gvI%)-1
437 gvN% = gNb%(gvI%,gvJ%)
438 IF gOwner%(gvN%)=gOwner%(gvI%) AND ABS(gOrder%(gvN%)-gOrder%(gvI%))<>1 THEN
439 RETurn 
440 END IF
441       END FOR gvJ%
442     END IF
443   END FOR gvI%
444 IF OPT_FULL% AND gvCount%<>gN% THEN
445 RETurn 
446 END IF
447 gValid% = 1
448 END DEFine
449 DEFine PROCedure GEN_ENDPOINTS
450 LOCal geI%
451   FOR geI%=0 TO gN%-1
452     gEndpoint%(geI%)=0
453   END FOR geI%
454   FOR geI%=1 TO OPT_PAIRS%
455 gEndpoint%(gStart%(geI%))=geI%
456 gEndpoint%(gFinish%(geI%))=geI%
457   END FOR geI%
458 END DEFine
459 DIM hashK(63)
460 DIM hashH(7)
461 DIM hashV(7)
462 DIM hashW(63)
463 DIM hashByte%(63)
464 DIM mixByte%(4,7)
465 hashAt% = 0
466 hashBytes = 0
467 FOR mathI%=0 TO 32
468   gPower(mathI%)=2^mathI%
469 END FOR mathI%
470 hashK(0)=1116352408
471 hashK(1)=1899447441
472 hashK(2)=-1245643825
473 hashK(3)=-373957723
474 hashK(4)=961987163
475 hashK(5)=1508970993
476 hashK(6)=-1841331548
477 hashK(7)=-1424204075
478 hashK(8)=-670586216
479 hashK(9)=310598401
480 hashK(10)=607225278
481 hashK(11)=1426881987
482 hashK(12)=1925078388
483 hashK(13)=-2132889090
484 hashK(14)=-1680079193
485 hashK(15)=-1046744716
486 hashK(16)=-459576895
487 hashK(17)=-272742522
488 hashK(18)=264347078
489 hashK(19)=604807628
490 hashK(20)=770255983
491 hashK(21)=1249150122
492 hashK(22)=1555081692
493 hashK(23)=1996064986
494 hashK(24)=-1740746414
495 hashK(25)=-1473132947
496 hashK(26)=-1341970488
497 hashK(27)=-1084653625
498 hashK(28)=-958395405
499 hashK(29)=-710438585
500 hashK(30)=113926993
501 hashK(31)=338241895
502 hashK(32)=666307205
503 hashK(33)=773529912
504 hashK(34)=1294757372
505 hashK(35)=1396182291
506 hashK(36)=1695183700
507 hashK(37)=1986661051
508 hashK(38)=-2117940946
509 hashK(39)=-1838011259
510 hashK(40)=-1564481375
511 hashK(41)=-1474664885
512 hashK(42)=-1035236496
513 hashK(43)=-949202525
514 hashK(44)=-778901479
515 hashK(45)=-694614492
516 hashK(46)=-200395387
517 hashK(47)=275423344
518 hashK(48)=430227734
519 hashK(49)=506948616
520 hashK(50)=659060556
521 hashK(51)=883997877
522 hashK(52)=958139571
523 hashK(53)=1322822218
524 hashK(54)=1537002063
525 hashK(55)=1747873779
526 hashK(56)=1955562222
527 hashK(57)=2024104815
528 hashK(58)=-2067236844
529 hashK(59)=-1933114872
530 hashK(60)=-1866530822
531 hashK(61)=-1538233109
532 hashK(62)=-1090935817
533 hashK(63)=-965641998
534 DEFine PROCedure RNG_SEED(RSSTREAM%,RSSEED)
535 LOCal rsI%
536 LOCal rsJ%
537 LOCal rsK%
538 LOCal rsM
539 LOCal rsN
540 rsM = 161803398-rsSeed
541 IF rsM<0 THEN
542 rsM = rsM+2147483647
543 END IF
544 gRng(rsStream%,55)=rsM
545 rsN = 1
546   FOR rsI%=1 TO 54
547 rsJ% = (21*rsI%) MOD 55
548 gRng(rsStream%,rsJ%)=rsN
549 rsN = rsM-rsN
550 IF rsN<0 THEN
551 rsN = rsN+2147483647
552 END IF
553 rsM = gRng(rsStream%,rsJ%)
554   END FOR rsI%
555   FOR rsK%=1 TO 4
556     FOR rsI%=1 TO 55
557 rsM = gRng(rsStream%,rsI%)-gRng(rsStream%,1+(rsI%+30) MOD 55)
558 IF rsM<0 THEN
559 rsM = rsM+2147483647
560 END IF
561       gRng(rsStream%,rsI%)=rsM
562     END FOR rsI%
563   END FOR rsK%
564 gRi%(rsStream%)=0
565 gRj%(rsStream%)=21
566 END DEFine
567 DEFine FuNction RNG_RAW
568 LOCal rrI%
569 LOCal rrJ%
570 LOCal rrValue
571 rrI% = gRi%(gRngStream%)+1
572 IF rrI%=56 THEN
573 rrI% = 1
574 END IF
575 rrJ% = gRj%(gRngStream%)+1
576 IF rrJ%=56 THEN
577 rrJ% = 1
578 END IF
579 rrValue = gRng(gRngStream%,rrI%)-gRng(gRngStream%,rrJ%)
580 IF rrValue=2147483647 THEN
581 rrValue = rrValue-1
582 END IF
583 IF rrValue<0 THEN
584 rrValue = rrValue+2147483647
585 END IF
586 gRng(gRngStream%,rrI%)=rrValue
587 gRi%(gRngStream%)=rrI%
588 gRj%(gRngStream%)=rrJ%
589 RETurn rrValue
590 END DEFine
591 DEFine FuNction RNG_NEXT%(RNBOUND%)
592 RETurn gen_min((rnBound%-1),(INT((rng_raw/2147483647)*rnBound%)))
593 END DEFine
594 DEFine FuNction RNG_UNIT
595 RETurn rng_raw/2147483647
596 END DEFine
597 DEFine FuNction WORD16%(WIV)
598 IF wiV>=32768 THEN
599 RETurn wiV-65536
600 END IF
601 RETurn wiV
602 END DEFine
603 DEFine FuNction XOR32(XBA,XBB)
604 LOCal xbLo
605 LOCal xbHi
606 xbLo = word16%((gen_mod((xbA),(65536)))) ^^ word16%((gen_mod((xbB),(65536))))
607 xbHi = word16%((INT(xbA/65536))) ^^ word16%((INT(xbB/65536)))
608 IF xbLo<0 THEN
609 xbLo = xbLo+65536
610 END IF
611 RETurn xbHi*65536+xbLo
612 END DEFine
613 DEFine FuNction AND32(ABA,ABB)
614 LOCal abLo
615 LOCal abHi
616 abLo = word16%((gen_mod((abA),(65536)))) && word16%((gen_mod((abB),(65536))))
617 abHi = word16%((INT(abA/65536))) && word16%((INT(abB/65536)))
618 IF abLo<0 THEN
619 abLo = abLo+65536
620 END IF
621 RETurn abHi*65536+abLo
622 END DEFine
623 DEFine FuNction ADD32(AAA,AAB)
624 LOCal aaLo
625 LOCal aaHi
626 aaLo = gen_mod((aaA),(65536))+gen_mod((aaB),(65536))
627 aaHi = INT(aaA/65536)+INT(aaB/65536)+INT(aaLo/65536)
628 aaHi = gen_mod((aaHi),(65536))
629 IF aaHi>=32768 THEN
630 aaHi = aaHi-65536
631 END IF
632 RETurn aaHi*65536+gen_mod((aaLo),(65536))
633 END DEFine
634 DEFine FuNction ROTATE32(RBV,RBN%)
635 LOCal rbRight
636 LOCal rbLeft
637 rbRight = INT(rbV/gPower(rbN%))
638 IF rbV<0 THEN
639 rbRight = rbRight+gPower(32-rbN%)
640 END IF
641 rbLeft = gen_mod((rbV),(gPower(rbN%)))
642 IF rbLeft>=gPower(rbN%-1) THEN
643 rbLeft = rbLeft-gPower(rbN%)
644 END IF
645 RETurn rbRight+rbLeft*gPower(32-rbN%)
646 END DEFine
647 DEFine PROCedure HASH_RESET
648 LOCal hrI%
649   hashH(0)=1779033703
650   hashH(1)=-1150833019
651   hashH(2)=1013904242
652   hashH(3)=-1521486534
653   hashH(4)=1359893119
654   hashH(5)=-1694144372
655   hashH(6)=528734635
656   hashH(7)=1541459225
657 hashAt% = 0
658 hashBytes = 0
659 END DEFine
660 DEFine PROCedure HASH_COMPRESS
661 LOCal hcI%
662 LOCal hcJ%
663 LOCal hcS0
664 LOCal hcS1
665 LOCal hcT1
666 LOCal hcT2
667 LOCal hcCh
668 LOCal hcMaj
669 LOCal hcHi
670 LOCal hcLo
671   FOR hcI%=0 TO 15
672 hcJ% = hcI%*4
673 hcHi = hashByte%(hcJ%)
674 hcHi = hcHi*256+hashByte%(hcJ%+1)
675 IF hcHi>=32768 THEN
676 hcHi = hcHi-65536
677 END IF
678 hcLo = hashByte%(hcJ%+2)
679 hcLo = hcLo*256+hashByte%(hcJ%+3)
680     hashW(hcI%)=hcHi*65536+hcLo
681   END FOR hcI%
682   FOR hcI%=16 TO 63
683 hcS0 = xor32((xor32((rotate32((hashW(hcI%-15)),(7))),(rotate32((hashW(hcI%-15)),(18))))),(shift32((hashW(hcI%-15)),(3))))
684 hcS1 = xor32((xor32((rotate32((hashW(hcI%-2)),(17))),(rotate32((hashW(hcI%-2)),(19))))),(shift32((hashW(hcI%-2)),(10))))
685     hashW(hcI%)=add32((add32((hashW(hcI%-16)),(hcS0))),(add32((hashW(hcI%-7)),(hcS1))))
686   END FOR hcI%
687   FOR hcI%=0 TO 7
688     hashV(hcI%)=hashH(hcI%)
689   END FOR hcI%
690   FOR hcI%=0 TO 63
691 hcS1 = xor32((xor32((rotate32((hashV(4)),(6))),(rotate32((hashV(4)),(11))))),(rotate32((hashV(4)),(25))))
692 hcCh = xor32((and32((hashV(4)),(hashV(5)))),(and32((-1-hashV(4)),(hashV(6)))))
693 hcT1 = add32((add32((add32((hashV(7)),(hcS1))),(add32((hcCh),(hashK(hcI%)))))),(hashW(hcI%)))
694 hcS0 = xor32((xor32((rotate32((hashV(0)),(2))),(rotate32((hashV(0)),(13))))),(rotate32((hashV(0)),(22))))
695 hcMaj = xor32((xor32((and32((hashV(0)),(hashV(1)))),(and32((hashV(0)),(hashV(2)))))),(and32((hashV(1)),(hashV(2)))))
696 hcT2 = add32((hcS0),(hcMaj))
697     FOR hcJ%=7 TO 1 STEP -1
698       hashV(hcJ%)=hashV(hcJ%-1)
699     END FOR hcJ%
700 hashV(4)=add32((hashV(4)),(hcT1))
701 hashV(0)=add32((hcT1),(hcT2))
702   END FOR hcI%
703   FOR hcI%=0 TO 7
704     hashH(hcI%)=add32((hashH(hcI%)),(hashV(hcI%)))
705   END FOR hcI%
706 END DEFine
707 DEFine PROCedure HASH_PUT(HPBYTE%)
708 hashByte%(hashAt%)=hpByte%
709 hashAt% = hashAt%+1
710 hashBytes = hashBytes+1
711 IF hashAt%=64 THEN
712 hash_compress
713 hashAt% = 0
714 END IF
715 END DEFine
716 DEFine PROCedure HASH_TEXT(HTTEXT$)
717 LOCal htI%
718   FOR htI%=1 TO LEN(htText$)
719 hash_put (CODE(htText$(htI%)))
720   END FOR htI%
721 END DEFine
722 DEFine FuNction HASH_FINISH$
723 LOCal hfLength
724 LOCal hfI%
725 LOCal hfJ%
726 LOCal hfV
727 LOCal hfHex$(16)
728 LOCal hfOut$(64)
729 hfLength = hashBytes*8
730 hash_put (128)
731   REPeat hfPad
732 IF hashAt%=56 THEN
733 EXIT hfPad
734 END IF
735 hash_put (0)
736   END REPeat hfPad
737   FOR hfI%=1 TO 4
738 hash_put (0)
739   END FOR hfI%
740   FOR hfI%=3 TO 0 STEP -1
741 hash_put (gen_mod((INT(hfLength/(256^hfI%))),(256)))
742   END FOR hfI%
743 hfOut$ = ""
744 hfHex$ = "0123456789ABCDEF"
745   FOR hfI%=0 TO 7
746     FOR hfJ%=7 TO 0 STEP -1
747 hfV = gen_mod((INT(hashH(hfI%)/(16^hfJ%))),(16))
748 hfOut$ = hfOut$ & hfHex$(hfV+1)
749     END FOR hfJ%
750   END FOR hfI%
751 RETurn hfOut$
752 END DEFine
753 DEFine PROCedure MIX_SHIFT(MSBITS%)
754 LOCal msI%
755 LOCal msJ%
756 LOCal msR%
757 LOCal msN%
758 msJ% = INT(msBits%/8)
759 msR% = msBits% MOD 8
760   FOR msI%=0 TO 7
761 msN% = msI%+msJ%
762 mixByte%(1,msI%)=0
763 IF msN%<8 THEN
764 mixByte%(1,msI%)=INT(mixByte%(0,msN%)/gPower(msR%))
765 END IF
766 IF msN%+1<8 AND msR% THEN
767 mixByte%(1,msI%)=mixByte%(1,msI%)+gen_mod((mixByte%(0,msN%+1)),(gPower(msR%)))*gPower(8-msR%)
768 END IF
769   END FOR msI%
770   FOR msI%=0 TO 7
771     mixByte%(0,msI%)=mixByte%(0,msI%) ^^ mixByte%(1,msI%)
772   END FOR msI%
773 END DEFine
774 DEFine PROCedure MIX_CONSTANT(MCHEX$)
775 LOCal mcI%
776 LOCal mcJ%
777 LOCal mcCode%
778 LOCal mcV
779   FOR mcI%=0 TO 7
780 mcV = 0
781     FOR mcJ%=0 TO 1
782 mcCode% = CODE(mcHex$(15-2*mcI%+mcJ%))-48
783 IF mcCode%>9 THEN
784 mcCode% = mcCode%-7
785 END IF
786 mcV = 16*mcV+mcCode%
787     END FOR mcJ%
788     mixByte%(2,mcI%)=mcV
789   END FOR mcI%
790 END DEFine
791 DEFine PROCedure MIX_MULTIPLY
792 LOCal mmI%
793 LOCal mmJ%
794 LOCal mmCarry
795 LOCal mmV
796 mmCarry = 0
797   FOR mmI%=0 TO 7
798 mmV = mmCarry
799     FOR mmJ%=0 TO mmI%
800 mmV = mmV+1.0*mixByte%(0,mmJ%)*mixByte%(2,mmI%-mmJ%)
801     END FOR mmJ%
802 mixByte%(3,mmI%)=gen_mod((mmV),(256))
803 mmCarry = INT(mmV/256)
804   END FOR mmI%
805   FOR mmI%=0 TO 7
806     mixByte%(0,mmI%)=mixByte%(3,mmI%)
807   END FOR mmI%
808 END DEFine
809 DEFine FuNction BATCH_SEED(BSSEED,BSREQUEST)
810 LOCal bsI%
811 LOCal bsCarry
812 LOCal bsV
813 LOCal bsOut
814 IF bsRequest=0 THEN
815 RETurn bsSeed
816 END IF
817   FOR bsI%=0 TO 7
818 mixByte%(0,bsI%)=gen_mod((bsRequest),(256))
819 bsRequest = INT(bsRequest/256)
820   END FOR bsI%
821 mix_constant ("9E3779B97F4A7C15")
822 mix_multiply
823 mix_constant ("54414E474C455342")
824 bsCarry = 0
825   FOR bsI%=0 TO 7
826 bsV = mixByte%(0,bsI%)+mixByte%(2,bsI%)+gen_mod((bsSeed),(256))+bsCarry
827 mixByte%(0,bsI%)=gen_mod((bsV),(256))
828 bsCarry = INT(bsV/256)
829 bsSeed = INT(bsSeed/256)
830   END FOR bsI%
831 mix_shift (30)
832 mix_constant ("BF58476D1CE4E5B9")
833 mix_multiply
834 mix_shift (27)
835 mix_constant ("94D049BB133111EB")
836 mix_multiply
837 mix_shift (31)
838 bsOut = mixByte%(0,3) MOD 128
839   FOR bsI%=2 TO 0 STEP -1
840 bsOut = bsOut*256+mixByte%(0,bsI%)
841   END FOR bsI%
842 RETurn bsOut
843 END DEFine
844 DEFine FuNction SHIFT32(SBV,SBN%)
845 LOCal sbOut
846 sbOut = INT(sbV/gPower(sbN%))
847 IF sbV<0 THEN
848 sbOut = sbOut+gPower(32-sbN%)
849 END IF
850 RETurn sbOut
851 END DEFine
852 DIM covTurn%(0)
853 DIM covBorder%(0)
854 DIM covSaved%(0)
855 covMemory = 0
856 covStride = 0
857 covScore% = 0
858 covFeasible% = 0
859 covFound% = 0
860 growBudget% = 0
861 DEFine PROCedure GROW_ENTER(GDDEPTH%)
862 LOCal gdI%
863 LOCal gdJ%
864 LOCal gdNext%
865 LOCal gdTouches%
866 LOCal gdCurrent%
867 LOCal gdCount%
868 LOCal gdSwap%
869 growBudget% = growBudget%-1
870 gdCurrent% = gGrowthPath%(gdDepth%)
871 gdCount% = 0
872 gGrowthAt%(gdDepth%)=0
873   FOR gdI%=0 TO gDegree%(gdCurrent%)-1
874 gdNext% = gNb%(gdCurrent%,gdI%)
875 IF NOT gOwner%(gdNext%) AND NOT gLocal%(gdNext%) THEN
876 gdTouches% = 0
877       FOR gdJ%=0 TO gDegree%(gdNext%)-1
878 gdTouches% = gdTouches%+gLocal%(gNb%(gdNext%,gdJ%))
879       END FOR gdJ%
880 IF gdTouches%=1 THEN
881 gGrowthCandidates%(gdDepth%,gdCount%)=gdNext%
882 gdCount% = gdCount%+1
883 END IF
884     END IF
885   END FOR gdI%
886   FOR gdI%=gdCount%-1 TO 1 STEP -1
887 gdJ% = rng_next%((gdI%+1))
888 gdSwap% = gGrowthCandidates%(gdDepth%,gdI%)
889 gGrowthCandidates%(gdDepth%,gdI%)=gGrowthCandidates%(gdDepth%,gdJ%)
890 gGrowthCandidates%(gdDepth%,gdJ%)=gdSwap%
891   END FOR gdI%
892   gGrowthCount%(gdDepth%)=gdCount%
893 END DEFine
894 DEFine FuNction GROW_PATH%(GPTARGET%)
895 LOCal gpI%
896 LOCal gpJ%
897 LOCal gpCount%
898 LOCal gpSwap%
899 LOCal gpStart%
900 LOCal gpDepth%
901 LOCal gpNext%
902 LOCal gpOK%
903 gpCount% = 0
904 growBudget% = 2048
905 gpOK% = 0
906   FOR gpI%=0 TO gN%-1
907     gLocal%(gpI%)=0
908 IF NOT gOwner%(gpI%) THEN
909 gScratch%(gpCount%)=gpI%
910 gpCount% = gpCount%+1
911 END IF
912   END FOR gpI%
913   FOR gpI%=gpCount%-1 TO 1 STEP -1
914 gpJ% = rng_next%((gpI%+1))
915 gpSwap% = gScratch%(gpI%)
916 gScratch%(gpI%)=gScratch%(gpJ%)
917 gScratch%(gpJ%)=gpSwap%
918   END FOR gpI%
919   FOR gpStart%=0 TO gpCount%-1
920 gpDepth% = 1
921 gGrowthPath%(1)=gScratch%(gpStart%)
922 gLocal%(gGrowthPath%(1))=1
923 grow_enter (1)
924     REPeat gpExtend
925 gen_poll
926 IF gCancel% THEN
927 EXIT gpExtend
928 END IF
929 IF gpDepth%=gpTarget% THEN
930 gpOK% = 1
931 EXIT gpExtend
932 END IF
933 IF growBudget%<=0 OR gpDepth%=0 THEN
934 EXIT gpExtend
935 END IF
936 gpI% = gGrowthAt%(gpDepth%)
937 IF gpI%>=gGrowthCount%(gpDepth%) THEN
938 gLocal%(gGrowthPath%(gpDepth%))=0
939 gpDepth% = gpDepth%-1
940       ELSE
941 gpNext% = gGrowthCandidates%(gpDepth%,gpI%)
942 gGrowthAt%(gpDepth%)=gpI%+1
943 gpDepth% = gpDepth%+1
944 gGrowthPath%(gpDepth%)=gpNext%
945 gLocal%(gpNext%)=1
946 grow_enter (gpDepth%)
947       END IF
948     END REPeat gpExtend
949     FOR gpI%=0 TO gN%-1
950       gLocal%(gpI%)=0
951     END FOR gpI%
952 IF gpOK% OR growBudget%<=0 OR gCancel% THEN
953 EXIT gpStart%
954 END IF
955   END FOR gpStart%
956 RETurn gpOK%
957 END DEFine
958 DEFine PROCedure GROW_GENERATE(GGSEED)
959 LOCal ggAttempt%
960 LOCal ggP%
961 LOCal ggI%
962 LOCal ggFree%
963 LOCal ggMax%
964 LOCal ggTarget%
965 LOCal ggOK%
966 gSuccess% = 0
967 gRngStream% = 0
968 rng_seed (0),(ggSeed)
969 DIM gGrowthCandidates%(gN%+1,3)
970 DIM gGrowthCount%(gN%+1)
971 DIM gGrowthAt%(gN%+1)
972 DIM gGrowthPath%(gN%+1)
973   FOR ggAttempt%=1 TO OPT_ATTEMPTS%
974 WORK_BOARDATTEMPTS=WORK_BOARDATTEMPTS+1
975 gen_poll
976 IF gCancel% THEN
977 RETurn 
978 END IF
979     FOR ggI%=0 TO gN%-1
980 gOwner%(ggI%)=0
981 gOrder%(ggI%)=0
982     END FOR ggI%
983 ggFree% = gN%
984 ggOK% = 1
985     FOR ggP%=1 TO OPT_PAIRS%
986 ggMax% = gen_min((OPT_MAXIMUM%),(ggFree%-(OPT_PAIRS%-ggP%)*OPT_MINIMUM%))
987 ggTarget% = OPT_MINIMUM%+rng_next%((ggMax%-OPT_MINIMUM%+1))
988 ggOK% = grow_path%((ggTarget%))
989 IF NOT ggOK% AND ggTarget%<>OPT_MINIMUM% THEN
990 ggTarget% = OPT_MINIMUM%
991 ggOK% = grow_path%((ggTarget%))
992 END IF
993 IF NOT ggOK% OR gCancel% THEN
994 EXIT ggP%
995 END IF
996 gLength%(ggP%)=ggTarget%
997 gStart%(ggP%)=gGrowthPath%(1)
998 gFinish%(ggP%)=gGrowthPath%(ggTarget%)
999       FOR ggI%=1 TO ggTarget%
1000 gOwner%(gGrowthPath%(ggI%))=ggP%
1001 gOrder%(gGrowthPath%(ggI%))=ggI%
1002       END FOR ggI%
1003 ggFree% = ggFree%-ggTarget%
1004     END FOR ggP%
1005 IF ggOK% THEN
1006 gen_endpoints
1007 gen_validate
1008 gSuccess% = gValid%
1009 RETurn 
1010 END IF
1011   END FOR ggAttempt%
1012 END DEFine
1013 DEFine PROCedure COVER_INIT
1014 LOCal ciAt
1015 LOCal ciBytes
1016 DIM gCuts%(OPT_PAIRS%+1)
1017 DIM gEnds%(gN%-1)
1018 DIM gNeeded%(gN%)
1019 DIM gTraversal%(gN%-1)
1020 DIM gPositions%(gN%-1)
1021 DIM gPrefix%(gN%+1)
1022 DIM gWalk%(gN%-1)
1023 DIM gWalkPos%(gN%-1)
1024 DIM gBestWalk%(gN%-1)
1025 DIM covTurn%(gN%)
1026 DIM covBorder%(gN%)
1027 DIM covSaved%(gN%-1)
1028 IF covMemory THEN
1029 RECHP covMemory
1030 END IF
1031 covMemory = 0
1032 covStride = 2*(1+INT(gN%/16))
1033 ciBytes = (OPT_PAIRS%+1)*covStride
1034 covMemory = ALCHP(ciBytes)
1035 IF NOT covMemory THEN
1036 gExit% = 1
1037 gError$ = "Not enough memory for coverage partition DP."
1038 RETurn 
1039 END IF
1040   FOR ciAt=0 TO ciBytes-2 STEP 2
1041     POKE_W covMemory+ciAt,0
1042   END FOR ciAt
1043 END DEFine
1044 DEFine FuNction COVER_DP%(CDP%,CDCELL%)
1045 LOCal cdWord%
1046 LOCal cdBit%
1047 cdWord% = word16%((PEEK_W(covMemory+cdP%*covStride+2*INT(cdCell%/16))))
1048 cdBit% = word16%((gPower(cdCell% MOD 16)))
1049 RETurn (cdWord% && cdBit%)<>0
1050 END DEFine
1051 DEFine PROCedure COVER_SET(CSP%,CSCELL%,CSVALUE%)
1052 LOCal csV
1053 LOCal csBit
1054 LOCal csOld%
1055 csV = PEEK_W(covMemory+csP%*covStride+2*INT(csCell%/16))
1056 IF csV<0 THEN
1057 csV = csV+65536
1058 END IF
1059 csBit = gPower(csCell% MOD 16)
1060 csOld% = ((word16%((csV)) && word16%((csBit)))<>0)
1061 csV = csV+(csValue%-csOld%)*csBit
1062   POKE_W covMemory+csP%*covStride+2*INT(csCell%/16),word16%((csV))
1063 END DEFine
1064 DEFine PROCedure COVER_BACKBITE
1065 LOCal cbFront%
1066 LOCal cbEnd%
1067 LOCal cbAdjacent%
1068 LOCal cbI%
1069 LOCal cbCount%
1070 LOCal cbJoin%
1071 LOCal cbLeft%
1072 LOCal cbRight%
1073 LOCal cbA%
1074 LOCal cbB%
1075 IF gN%<2 THEN
1076 RETurn 
1077 END IF
1078 cbFront% = (rng_next%((2))=0)
1079 IF cbFront% THEN
1080 cbEnd% = gWalk%(0)
1081 cbAdjacent% = gWalk%(1)
1082   ELSE
1083 cbEnd% = gWalk%(gN%-1)
1084 cbAdjacent% = gWalk%(gN%-2)
1085   END IF
1086 cbCount% = 0
1087   FOR cbI%=0 TO gDegree%(cbEnd%)-1
1088 IF gNb%(cbEnd%,cbI%)<>cbAdjacent% THEN
1089 gScratch%(cbCount%)=gNb%(cbEnd%,cbI%)
1090 cbCount% = cbCount%+1
1091 END IF
1092   END FOR cbI%
1093 IF cbCount%=0 THEN
1094 RETurn 
1095 END IF
1096 cbJoin% = gWalkPos%(gScratch%(rng_next%((cbCount%))))
1097 IF cbFront% THEN
1098 cbLeft% = 0
1099 cbRight% = cbJoin%-1
1100   ELSE
1101 cbLeft% = cbJoin%+1
1102 cbRight% = gN%-1
1103   END IF
1104   REPeat cbReverse
1105 IF cbLeft%>=cbRight% THEN
1106 EXIT cbReverse
1107 END IF
1108 cbA% = gWalk%(cbLeft%)
1109 cbB% = gWalk%(cbRight%)
1110 gWalk%(cbLeft%)=cbB%
1111 gWalk%(cbRight%)=cbA%
1112 gWalkPos%(cbA%)=cbRight%
1113 gWalkPos%(cbB%)=cbLeft%
1114 cbLeft% = cbLeft%+1
1115 cbRight% = cbRight%-1
1116   END REPeat cbReverse
1117 END DEFine
1118 DEFine PROCedure COVER_FRESH(CFMIX%)
1119 LOCal cfTranspose%
1120 LOCal cfFlipX%
1121 LOCal cfFlipY%
1122 LOCal cfColumns%
1123 LOCal cfRows%
1124 LOCal cfRow%
1125 LOCal cfCol%
1126 LOCal cfAt%
1127 LOCal cfX%
1128 LOCal cfY%
1129 LOCal cfI%
1130 LOCal cfMoves%
1131 cfTranspose% = (rng_next%((2))=0)
1132 IF NOT cfMix% AND OPT_WIDTH%<>OPT_HEIGHT% THEN
1133 cfTranspose% = (OPT_WIDTH%<OPT_HEIGHT%)
1134 END IF
1135 cfFlipX% = (rng_next%((2))=0)
1136 cfFlipY% = (rng_next%((2))=0)
1137 cfColumns% = OPT_WIDTH%
1138 cfRows% = OPT_HEIGHT%
1139 IF cfTranspose% THEN
1140 cfColumns% = OPT_HEIGHT%
1141 cfRows% = OPT_WIDTH%
1142 END IF
1143 cfAt% = 0
1144   FOR cfRow%=0 TO cfRows%-1
1145     FOR cfI%=0 TO cfColumns%-1
1146 cfCol% = cfI%
1147 IF cfRow% MOD 2 THEN
1148 cfCol% = cfColumns%-1-cfI%
1149 END IF
1150 cfX% = cfCol%
1151 cfY% = cfRow%
1152 IF cfTranspose% THEN
1153 cfX% = cfRow%
1154 cfY% = cfCol%
1155 END IF
1156 IF cfFlipX% THEN
1157 cfX% = OPT_WIDTH%-1-cfX%
1158 END IF
1159 IF cfFlipY% THEN
1160 cfY% = OPT_HEIGHT%-1-cfY%
1161 END IF
1162 gWalk%(cfAt%)=cfY%*OPT_WIDTH%+cfX%
1163 gWalkPos%(gWalk%(cfAt%))=cfAt%
1164 cfAt% = cfAt%+1
1165     END FOR cfI%
1166   END FOR cfRow%
1167 IF NOT cfMix% THEN
1168 WORK_STRUCTUREDSTARTS=WORK_STRUCTUREDSTARTS+1
1169 END IF
1170 IF cfMix% AND OPT_WIDTH%>1 AND OPT_HEIGHT%>1 THEN
1171 cfMoves% = gen_min((16384),(gen_max((256),(gen_product((48),(gN%))))))
1172     FOR cfI%=1 TO cfMoves%
1173 cover_backbite
1174 gen_poll
1175 IF gCancel% THEN
1176 RETurn 
1177 END IF
1178     END FOR cfI%
1179   END IF
1180 END DEFine
1181 DEFine PROCedure COVER_ANALYSE
1182 LOCal caJ%
1183 LOCal caD%
1184 LOCal caBound%
1185 LOCal caCell%
1186 LOCal caNext%
1187 LOCal caStart%
1188 LOCal caFinish%
1189 LOCal caMax%
1190 WORK_TRAVERSALEVALUATIONS=WORK_TRAVERSALEVALUATIONS+1
1191 caBound% = 0
1192 caMax% = gen_min((gN%),(OPT_MAXIMUM%))
1193   FOR caJ%=0 TO gN%-1
1194 caCell% = gWalk%(caJ%)
1195     FOR caD%=0 TO gDegree%(caCell%)-1
1196 caNext% = gWalkPos%(gNb%(caCell%,caD%))
1197 IF caNext%<caJ%-1 THEN
1198 caBound% = gen_max((caBound%),(caNext%+1))
1199 END IF
1200     END FOR caD%
1201     gTraversal%(caJ%)=caBound%
1202   END FOR caJ%
1203 caFinish% = 0
1204   FOR caStart%=0 TO gN%-1
1205 caFinish% = gen_max((caFinish%),(caStart%))
1206     REPeat caExtend
1207 IF caFinish%>=gN% THEN
1208 EXIT caExtend
1209 END IF
1210 IF gTraversal%(caFinish%)>caStart% THEN
1211 EXIT caExtend
1212 END IF
1213 caFinish% = caFinish%+1
1214     END REPeat caExtend
1215 gEnds%(caStart%)=gen_min((caFinish%),(caStart%+caMax%))
1216 gNeeded%(caStart%)=gN%+1
1217   END FOR caStart%
1218   gNeeded%(gN%)=0
1219   FOR caStart%=gN%-1 TO 0 STEP -1
1220     FOR caFinish%=caStart%+OPT_MINIMUM% TO gEnds%(caStart%)
1221       gNeeded%(caStart%)=gen_min((gNeeded%(caStart%)),(1+gNeeded%(caFinish%)))
1222     END FOR caFinish%
1223   END FOR caStart%
1224 covScore% = gNeeded%(0)
1225 END DEFine
1226 DEFine FuNction COVER_WEIGHT(CWSTART%,CWEND%)
1227 LOCal cwLen%
1228 LOCal cwBends%
1229 LOCal cwAverage
1230 LOCal cwBoundary
1231 LOCal cwVariety
1232 LOCal cwW
1233 cwLen% = cwEnd%-cwStart%
1234 cwBends% = 0
1235 IF cwLen%>=3 THEN
1236 cwBends% = covTurn%(cwEnd%-1)-covTurn%(cwStart%+1)
1237 END IF
1238 cwBoundary = covBorder%(cwEnd%)-covBorder%(cwStart%)
1239 cwBoundary = cwBoundary/cwLen%
1240 cwAverage = gN%
1241 cwAverage = cwAverage/OPT_PAIRS%
1242 cwVariety = gen_min((1),(ABS(cwLen%-cwAverage)/cwAverage))
1243 cwW = 1+0.3*gen_min((2),(cwBends%))-0.1*cwBoundary+0.15*cwVariety
1244 IF cwBends%=0 THEN
1245 cwW = cwW-0.2
1246 END IF
1247 RETurn cwW
1248 END DEFine
1249 DEFine PROCedure COVER_PARTITION
1250 LOCal cpP%
1251 LOCal cpI%
1252 LOCal cpFirst%
1253 LOCal cpLast%
1254 LOCal cpStart%
1255 LOCal cpEnd%
1256 LOCal cpMax%
1257 LOCal cpCount%
1258 LOCal cpPick%
1259 LOCal cpJ%
1260 LOCal cpSwap%
1261 LOCal cpReverse%
1262 LOCal cpCell%
1263 LOCal cpTurn%
1264 LOCal cpBorder%
1265 LOCal cpWeight
1266 LOCal cpTarget
1267 LOCal cpSum
1268 covFeasible% = 0
1269 cover_set (0),(gN%),(1)
1270 cpMax% = gen_min((gN%),(OPT_MAXIMUM%))
1271   FOR cpP%=1 TO OPT_PAIRS%
1272     gPrefix%(0)=0
1273     FOR cpI%=0 TO gN%
1274       gPrefix%(cpI%+1)=gPrefix%(cpI%)+cover_dp%((cpP%-1),(cpI%))
1275     END FOR cpI%
1276 cpFirst% = gen_max((0),(gN%-gen_product((cpP%),(cpMax%))))
1277 cpLast% = gN%-cpP%*OPT_MINIMUM%
1278     FOR cpStart%=cpFirst% TO cpLast%
1279 cpI% = cpStart%+OPT_MINIMUM%
1280 cpEnd% = gEnds%(cpStart%)
1281 cover_set (cpP%),(cpStart%),((cpI%<=cpEnd% AND gPrefix%(cpEnd%+1)>gPrefix%(cpI%)))
1282     END FOR cpStart%
1283   END FOR cpP%
1284 IF NOT cover_dp%((OPT_PAIRS%),(0)) THEN
1285 RETurn 
1286 END IF
1287 IF OPT_VARIED% THEN
1288 covTurn%(0)=0
1289 covBorder%(0)=0
1290     FOR cpI%=0 TO gN%-1
1291 cpTurn% = 0
1292 IF cpI%>0 AND cpI%+1<gN% THEN
1293 cpTurn% = (gWalk%(cpI%)-gWalk%(cpI%-1)<>gWalk%(cpI%+1)-gWalk%(cpI%))
1294 END IF
1295 cpCell% = gWalk%(cpI%)
1296 cpBorder% = (cpCell% MOD OPT_WIDTH%=0 OR cpCell% MOD OPT_WIDTH%=OPT_WIDTH%-1 OR cpCell%<OPT_WIDTH% OR cpCell%>=gN%-OPT_WIDTH%)
1297 covTurn%(cpI%+1)=covTurn%(cpI%)+cpTurn%
1298 covBorder%(cpI%+1)=covBorder%(cpI%)+cpBorder%
1299     END FOR cpI%
1300   END IF
1301 cpStart% = 0
1302 gCuts%(1)=0
1303   FOR cpP%=OPT_PAIRS% TO 1 STEP -1
1304 cpCount% = 0
1305 cpSum = 0
1306     FOR cpEnd%=cpStart%+OPT_MINIMUM% TO gEnds%(cpStart%)
1307 IF cover_dp%((cpP%-1),(cpEnd%)) THEN
1308 gScratch%(cpCount%)=cpEnd%
1309 cpCount% = cpCount%+1
1310 IF OPT_VARIED% THEN
1311 cpSum = cpSum+cover_weight((cpStart%),(cpEnd%))
1312 END IF
1313       END IF
1314     END FOR cpEnd%
1315 cpPick% = rng_next%((cpCount%))
1316 IF OPT_VARIED% THEN
1317 cpTarget = rng_unit*cpSum
1318 cpWeight = 0
1319 cpPick% = cpCount%-1
1320       FOR cpI%=0 TO cpCount%-1
1321 cpWeight = cpWeight+cover_weight((cpStart%),(gScratch%(cpI%)))
1322 IF cpWeight>cpTarget THEN
1323 cpPick% = cpI%
1324 EXIT cpI%
1325 END IF
1326       END FOR cpI%
1327     END IF
1328 cpStart% = gScratch%(cpPick%)
1329 gCuts%(OPT_PAIRS%-cpP%+2)=cpStart%
1330   END FOR cpP%
1331   FOR cpP%=1 TO OPT_PAIRS%
1332     gCanonical%(cpP%)=cpP%
1333   END FOR cpP%
1334   FOR cpP%=OPT_PAIRS% TO 2 STEP -1
1335 cpJ% = 1+rng_next%((cpP%))
1336 cpSwap% = gCanonical%(cpP%)
1337 gCanonical%(cpP%)=gCanonical%(cpJ%)
1338 gCanonical%(cpJ%)=cpSwap%
1339   END FOR cpP%
1340   FOR cpI%=0 TO gN%-1
1341 gOwner%(cpI%)=0
1342 gOrder%(cpI%)=0
1343   END FOR cpI%
1344   FOR cpP%=1 TO OPT_PAIRS%
1345 cpJ% = gCanonical%(cpP%)
1346 cpStart% = gCuts%(cpJ%)
1347 cpEnd% = gCuts%(cpJ%+1)
1348 cpReverse% = (rng_next%((2))=0)
1349 gLength%(cpP%)=cpEnd%-cpStart%
1350     FOR cpI%=0 TO gLength%(cpP%)-1
1351 cpCell% = gWalk%(cpStart%+cpI%)
1352 IF cpReverse% THEN
1353 cpCell% = gWalk%(cpEnd%-1-cpI%)
1354 END IF
1355 gOwner%(cpCell%)=cpP%
1356 gOrder%(cpCell%)=cpI%+1
1357 IF cpI%=0 THEN
1358 gStart%(cpP%)=cpCell%
1359 END IF
1360 IF cpI%=gLength%(cpP%)-1 THEN
1361 gFinish%(cpP%)=cpCell%
1362 END IF
1363     END FOR cpI%
1364   END FOR cpP%
1365 gen_endpoints
1366 covFeasible% = 1
1367 END DEFine
1368 DEFine PROCedure COVER_FIND(CFRESTARTS%,CFREFINE%,CFFALLBACK%,CFBURSTS%)
1369 LOCal cfAttempt%
1370 LOCal cfSince%
1371 LOCal cfFirst%
1372 LOCal cfScore%
1373 LOCal cfI%
1374 LOCal cfMoves%
1375 LOCal cfMove%
1376 LOCal cfNext%
1377 LOCal cfAccept%
1378 IF gExit% OR gCancel% THEN
1379 RETurn 
1380 END IF
1381 covFound% = 0
1382 cfFirst% = -1
1383 cfSince% = 0
1384 cfScore% = 32767
1385 cover_fresh ((cfFallback%<>0))
1386   FOR cfI%=0 TO gN%-1
1387     covSaved%(cfI%)=gWalk%(cfI%)
1388   END FOR cfI%
1389   FOR cfAttempt%=0 TO OPT_ATTEMPTS%-1
1390 gen_poll
1391 IF gCancel% THEN
1392 RETurn 
1393 END IF
1394 IF OPT_WIDTH%=1 OR OPT_HEIGHT%=1 THEN
1395 IF cfAttempt%>0 THEN
1396 EXIT cfAttempt%
1397 END IF
1398     END IF
1399 IF cfFirst%>=0 AND cfAttempt%-cfFirst%>cfRefine% THEN
1400 EXIT cfAttempt%
1401 END IF
1402 IF cfFirst%<0 AND cfAttempt%>0 AND (cfSince%=cfRestarts% OR cfAttempt%=cfFallback%) THEN
1403 cover_fresh ((cfAttempt%<>cfFallback%))
1404 cfSince% = 0
1405 cfScore% = 32767
1406       FOR cfI%=0 TO gN%-1
1407         covSaved%(cfI%)=gWalk%(cfI%)
1408       END FOR cfI%
1409     END IF
1410     FOR cfI%=0 TO gN%-1
1411 gWalk%(cfI%)=covSaved%(cfI%)
1412 gWalkPos%(gWalk%(cfI%))=cfI%
1413     END FOR cfI%
1414 IF cfSince%>0 THEN
1415 cfMoves% = 1
1416 IF cfBursts% AND cfSince% MOD 8=0 THEN
1417 cfMoves% = gen_max((2),(INT(gN%/4)))
1418 END IF
1419       FOR cfMove%=1 TO cfMoves%
1420 cover_backbite
1421       END FOR cfMove%
1422     END IF
1423 IF cfFirst%>=0 THEN
1424 WORK_REFINEMENTEVALUATIONS=WORK_REFINEMENTEVALUATIONS+1
1425 END IF
1426 cfSince% = cfSince%+1
1427 cover_analyse
1428 cfNext% = covScore%
1429 IF cfNext%<=OPT_PAIRS% THEN
1430 cover_partition
1431 IF covFeasible% THEN
1432 covFound% = 1
1433 IF cfFirst%<0 THEN
1434 cfFirst% = cfAttempt%
1435 END IF
1436         FOR cfI%=0 TO gN%-1
1437           gBestWalk%(cfI%)=gWalk%(cfI%)
1438         END FOR cfI%
1439       END IF
1440     END IF
1441 cfAccept% = (cfNext%<=cfScore%)
1442 IF NOT cfAccept% THEN
1443 cfAccept% = (rng_unit<EXP((cfScore%-cfNext%)/0.4))
1444 END IF
1445 IF cfAccept% THEN
1446 cfScore% = cfNext%
1447       FOR cfI%=0 TO gN%-1
1448         covSaved%(cfI%)=gWalk%(cfI%)
1449       END FOR cfI%
1450     END IF
1451   END FOR cfAttempt%
1452 END DEFine
1453 DEFine PROCedure IDENTITY_SORT
1454 LOCal isI%
1455 LOCal isJ%
1456 LOCal isA%
1457 LOCal isB%
1458 LOCal isMap%
1459   FOR isI%=2 TO OPT_PAIRS%
1460 isA% = gA%(isI%)
1461 isB% = gB%(isI%)
1462 isMap% = gCanonical%(isI%)
1463 isJ% = isI%-1
1464     REPeat isInsert
1465 IF isJ%<1 THEN
1466 EXIT isInsert
1467 END IF
1468 IF gA%(isJ%)<isA% OR (gA%(isJ%)=isA% AND gB%(isJ%)<=isB%) THEN
1469 EXIT isInsert
1470 END IF
1471 gA%(isJ%+1)=gA%(isJ%)
1472 gB%(isJ%+1)=gB%(isJ%)
1473 gCanonical%(isJ%+1)=gCanonical%(isJ%)
1474 isJ% = isJ%-1
1475     END REPeat isInsert
1476 gA%(isJ%+1)=isA%
1477 gB%(isJ%+1)=isB%
1478 gCanonical%(isJ%+1)=isMap%
1479   END FOR isI%
1480 END DEFine
1481 DEFine PROCedure IDENTITY_CANONICAL
1482 LOCal icP%
1483   FOR icP%=1 TO OPT_PAIRS%
1484 gA%(icP%)=gen_min((gStart%(icP%)),(gFinish%(icP%)))
1485 gB%(icP%)=gen_max((gStart%(icP%)),(gFinish%(icP%)))
1486     gCanonical%(icP%)=icP%
1487   END FOR icP%
1488 identity_sort
1489 END DEFine
1490 DEFine FuNction IDENTITY_PAIRS$
1491 LOCal ipP%
1492 LOCal ipFlag$(5)
1493 ipFlag$ = "False"
1494 IF OPT_FULL% THEN
1495 ipFlag$ = "True"
1496 END IF
1497 hash_reset
1498 hash_text (gen_int$((OPT_WIDTH%)) & "x" & gen_int$((OPT_HEIGHT%)) & ":" & ipFlag$ & ":")
1499   FOR ipP%=1 TO OPT_PAIRS%
1500 IF ipP%>1 THEN
1501 hash_text (";")
1502 END IF
1503 hash_text (gen_int$((gA%(ipP%))) & ":" & gen_int$((gB%(ipP%))))
1504   END FOR ipP%
1505 RETurn hash_finish$
1506 END DEFine
1507 DEFine FuNction IDENTITY_BOARD$
1508 identity_canonical
1509 RETurn identity_pairs$
1510 END DEFine
1511 DEFine FuNction IDENTITY_PUZZLE$
1512 LOCal iiP%
1513 LOCal iiCell%
1514 LOCal iiHash$(64)
1515 LOCal iiOut$(72)
1516 LOCal iiC%
1517 LOCal iiI%
1518 hash_reset
1519 hash_text ("tangles-v1")
1520 IF OPT_FULL% THEN
1521 hash_text (":full-coverage")
1522 END IF
1523 hash_text (":" & gen_int$((OPT_WIDTH%)) & "x" & gen_int$((OPT_HEIGHT%)) & ":")
1524   FOR iiP%=1 TO OPT_PAIRS%
1525 IF iiP%>1 THEN
1526 hash_text (";")
1527 END IF
1528 iiCell% = gStart%(iiP%)
1529 hash_text (gen_label$((iiP%)) & ":" & gen_int$((iiCell% MOD OPT_WIDTH%)) & "," & gen_int$((INT(iiCell%/OPT_WIDTH%))) & ":")
1530 iiCell% = gFinish%(iiP%)
1531 hash_text (gen_int$((iiCell% MOD OPT_WIDTH%)) & "," & gen_int$((INT(iiCell%/OPT_WIDTH%))))
1532   END FOR iiP%
1533 iiHash$ = hash_finish$
1534 iiOut$ = "tangles-"
1535   FOR iiI%=1 TO 64
1536 iiC% = CODE(iiHash$(iiI%))
1537 IF iiC%>=65 THEN
1538 iiC% = iiC%+32
1539 END IF
1540 iiOut$ = iiOut$ & CHR$(iiC%)
1541   END FOR iiI%
1542 RETurn iiOut$
1543 END DEFine
1544 DEFine FuNction IDENTITY_TRANSFORM%(ITCELL%,ITMODE%)
1545 LOCal itX%
1546 LOCal itY%
1547 LOCal itSwap%
1548 itX% = itCell% MOD OPT_WIDTH%
1549 itY% = INT(itCell%/OPT_WIDTH%)
1550 IF itMode%>=4 THEN
1551 itSwap% = itX%
1552 itX% = itY%
1553 itY% = itSwap%
1554 END IF
1555 IF itMode% MOD 2 THEN
1556 itX% = OPT_WIDTH%-1-itX%
1557 END IF
1558 IF INT(itMode%/2) MOD 2 THEN
1559 itY% = OPT_HEIGHT%-1-itY%
1560 END IF
1561 RETurn itY%*OPT_WIDTH%+itX%
1562 END DEFine
1563 DEFine FuNction IDENTITY_SYMMETRY$
1564 LOCal iyMode%
1565 LOCal iyCount%
1566 LOCal iyP%
1567 LOCal iyA%
1568 LOCal iyB%
1569 LOCal iyHash$(64)
1570 LOCal iyBest$(64)
1571 iyCount% = 4
1572 IF OPT_WIDTH%=OPT_HEIGHT% THEN
1573 iyCount% = 8
1574 END IF
1575 iyBest$ = ""
1576   FOR iyMode%=0 TO iyCount%-1
1577     FOR iyP%=1 TO OPT_PAIRS%
1578 iyA% = identity_transform%((gStart%(iyP%)),(iyMode%))
1579 iyB% = identity_transform%((gFinish%(iyP%)),(iyMode%))
1580 gA%(iyP%)=gen_min((iyA%),(iyB%))
1581 gB%(iyP%)=gen_max((iyA%),(iyB%))
1582 gCanonical%(iyP%)=iyP%
1583     END FOR iyP%
1584 identity_sort
1585 iyHash$ = identity_pairs$
1586 IF iyBest$="" OR gen_less%((iyHash$),(iyBest$)) THEN
1587 iyBest$ = iyHash$
1588 END IF
1589   END FOR iyMode%
1590 RETurn iyBest$
1591 END DEFine
1592 DIM sRequired%(0)
1593 DIM sDiscovery%(0)
1594 DIM sLow%(0)
1595 DIM sMandatory%(0)
1596 DIM sA%(0)
1597 DIM sB%(0)
1598 DIM sColourMap%(0)
1599 DIM sPathSeen%(0)
1600 DIM sWord%(63)
1601 DIM sBit%(63)
1602 DIM sAddress(256)
1603 sMemory = 0
1604 solverFrameBytes = 0
1605 sBad% = 0
1606 sLimit% = 0
1607 sColours% = 0
1608 sClock% = 0
1609 sRouteColour% = 0
1610 sSearchLimit = 20000
1611 DEFine FuNction SOLVER_SIGNED%(SWVALUE)
1612 IF swValue>32767 THEN
1613 RETurn swValue-65536
1614 END IF
1615 RETurn swValue
1616 END DEFine
1617 DEFine FuNction SOLVER_WORD%(SWLEVEL%,SWCELL%,SWWORD%)
1618 RETurn solver_signed%((PEEK_W(sAddress(swLevel%)+12*swCell%+2*swWord%)))
1619 END DEFine
1620 DEFine PROCedure SOLVER_PUT(SWLEVEL%,SWCELL%,SWWORD%,SWVALUE%)
1621   POKE_W sAddress(swLevel%)+12*swCell%+2*swWord%,swValue%
1622 END DEFine
1623 DEFine FuNction SOLVER_COUNT%(SWLEVEL%,SWCELL%)
1624 RETurn PEEK_W(sAddress(swLevel%)+12*swCell%+8)
1625 END DEFine
1626 DEFine PROCedure SOLVER_SET_COUNT(SWLEVEL%,SWCELL%,SWCOUNT%)
1627   POKE_W sAddress(swLevel%)+12*swCell%+8,swCount%
1628 END DEFine
1629 DEFine FuNction SOLVER_FIXED%(SWLEVEL%,SWCELL%)
1630 RETurn solver_signed%((PEEK_W(sAddress(swLevel%)+12*swCell%+10)))
1631 END DEFine
1632 DEFine PROCedure SOLVER_SET_FIXED(SWLEVEL%,SWCELL%,SWFIXED%)
1633   POKE_W sAddress(swLevel%)+12*swCell%+10,swFixed%
1634 END DEFine
1635 DEFine PROCedure SOLVER_SPEND
1636 gen_poll
1637 IF sLimit% OR gCancel% THEN
1638 RETurn 
1639 END IF
1640 IF SR_WORK>=SR_WORKLIMIT THEN
1641 sLimit% = 1
1642 SR_STATUS$="budgetExceeded"
1643 SR_LIMITREASON$="work"
1644 RETurn 
1645   END IF
1646   SR_WORK=SR_WORK+1
1647 END DEFine
1648 DEFine FuNction SOLVER_HAS%(SHLEVEL%,SHCELL%,SHCOLOUR%)
1649 RETurn (solver_word%((shLevel%),(shCell%),(sWord%(shColour%))) && sBit%(shColour%))<>0
1650 END DEFine
1651 DEFine FuNction SOLVER_DROP%(SXLEVEL%,SXCELL%,SXCOLOUR%,SXLOCAL%,SXROUTE%)
1652 LOCal sxV
1653 LOCal sxWord%
1654 LOCal sxC%
1655 IF NOT solver_has%((sxLevel%),(sxCell%),(sxColour%)) THEN
1656 RETurn 0
1657 END IF
1658 IF solver_count%((sxLevel%),(sxCell%))=1 THEN
1659 sBad% = 1
1660 RETurn 0
1661 END IF
1662 sxWord% = sWord%(sxColour%)
1663 solver_put (sxLevel%),(sxCell%),(sxWord%),((solver_word%((sxLevel%),(sxCell%),(sxWord%)) ^^ sBit%(sxColour%)))
1664 solver_set_count (sxLevel%),(sxCell%),(solver_count%((sxLevel%),(sxCell%))-1)
1665 IF solver_count%((sxLevel%),(sxCell%))=1 THEN
1666     FOR sxC%=0 TO sColours%-1
1667 IF solver_has%((sxLevel%),(sxCell%),(sxC%)) THEN
1668 solver_set_fixed (sxLevel%),(sxCell%),(sxC%)
1669 EXIT sxC%
1670 END IF
1671     END FOR sxC%
1672   END IF
1673 IF sxLocal% THEN
1674 SR_LOCALDEDUCTIONS=SR_LOCALDEDUCTIONS+1
1675 END IF
1676 IF sxRoute% THEN
1677 SR_ROUTEDEDUCTIONS=SR_ROUTEDEDUCTIONS+1
1678 END IF
1679 RETurn 1
1680 END DEFine
1681 DEFine FuNction SOLVER_FORCE%(SSLEVEL%,SSCELL%,SSCOLOUR%,SSLOCAL%,SSROUTE%)
1682 LOCal ssI%
1683 LOCal ssRemoved%
1684 IF NOT solver_has%((ssLevel%),(ssCell%),(ssColour%)) THEN
1685 sBad% = 1
1686 RETurn 0
1687 END IF
1688 IF solver_count%((ssLevel%),(ssCell%))=1 THEN
1689 RETurn 0
1690 END IF
1691 ssRemoved% = solver_count%((ssLevel%),(ssCell%))-1
1692   FOR ssI%=0 TO 3
1693 solver_put (ssLevel%),(ssCell%),(ssI%),(0)
1694   END FOR ssI%
1695 solver_put (ssLevel%),(ssCell%),(sWord%(ssColour%)),(sBit%(ssColour%))
1696 solver_set_count (ssLevel%),(ssCell%),(1)
1697 solver_set_fixed (ssLevel%),(ssCell%),(ssColour%)
1698 IF ssLocal% THEN
1699 SR_LOCALDEDUCTIONS=SR_LOCALDEDUCTIONS+ssRemoved%
1700 END IF
1701 IF ssRoute% THEN
1702 SR_ROUTEDEDUCTIONS=SR_ROUTEDEDUCTIONS+ssRemoved%
1703 END IF
1704 RETurn 1
1705 END DEFine
1706 DEFine PROCedure SOLVER_COPY(SYFROM%,SYTO%)
1707 LOCal syOffset
1708   FOR syOffset=0 TO solverFrameBytes-2 STEP 2
1709     POKE_W sAddress(syTo%)+syOffset,solver_signed%((PEEK_W(sAddress(syFrom%)+syOffset)))
1710   END FOR syOffset
1711 END DEFine
1712 DEFine PROCedure SOLVER_LOCAL(SLLEVEL%,SLRECORD%)
1713 LOCal slChanged%
1714 LOCal slCell%
1715 LOCal slColour%
1716 LOCal slD%
1717 LOCal slN%
1718 LOCal slPossible%
1719 LOCal slFixed%
1720 LOCal slOK%
1721 LOCal slFirst%
1722 LOCal slLast%
1723   REPeat slAgain
1724 slChanged% = 0
1725     FOR slCell%=0 TO gN%-1
1726 slFirst% = 0
1727 slLast% = OPT_PAIRS%-1
1728 IF solver_fixed%((slLevel%),(slCell%))>=0 THEN
1729 slFirst% = solver_fixed%((slLevel%),(slCell%))
1730 slLast% = gen_min((slFirst%),(OPT_PAIRS%-1))
1731       END IF
1732       FOR slColour%=slFirst% TO slLast%
1733 IF solver_has%((slLevel%),(slCell%),(slColour%)) THEN
1734 solver_spend
1735 IF sLimit% OR gCancel% THEN
1736 RETurn 
1737 END IF
1738 slPossible% = 0
1739 slFixed% = 0
1740           FOR slD%=0 TO gDegree%(slCell%)-1
1741 slN% = gNb%(slCell%,slD%)
1742 IF solver_has%((slLevel%),(slN%),(slColour%)) THEN
1743 slPossible% = slPossible%+1
1744 END IF
1745 IF solver_fixed%((slLevel%),(slN%))=slColour% THEN
1746 slFixed% = slFixed%+1
1747 END IF
1748           END FOR slD%
1749 IF slPossible%<sRequired%(slCell%) OR slFixed%>sRequired%(slCell%) THEN
1750 IF solver_drop%((slLevel%),(slCell%),(slColour%),(slRecord%),(0)) THEN
1751 slChanged% = 1
1752 END IF
1753           ELSE
1754 IF solver_fixed%((slLevel%),(slCell%))=slColour% THEN
1755               FOR slD%=0 TO gDegree%(slCell%)-1
1756 slN% = gNb%(slCell%,slD%)
1757 IF solver_has%((slLevel%),(slN%),(slColour%)) AND solver_fixed%((slLevel%),(slN%))<>slColour% THEN
1758 IF slPossible%=sRequired%(slCell%) THEN
1759 IF solver_force%((slLevel%),(slN%),(slColour%),(slRecord%),(0)) THEN
1760 slChanged% = 1
1761 END IF
1762                   ELSE
1763 IF slFixed%=sRequired%(slCell%) THEN
1764 IF solver_drop%((slLevel%),(slN%),(slColour%),(slRecord%),(0)) THEN
1765 slChanged% = 1
1766 END IF
1767                     END IF
1768                   END IF
1769                 END IF
1770 IF sBad% THEN
1771 RETurn 
1772 END IF
1773               END FOR slD%
1774             END IF
1775           END IF
1776 IF sBad% THEN
1777 RETurn 
1778 END IF
1779         END IF
1780       END FOR slColour%
1781     END FOR slCell%
1782 IF NOT slChanged% THEN
1783 EXIT slAgain
1784 END IF
1785   END REPeat slAgain
1786 END DEFine
1787 DEFine FuNction SOLVER_VISIT%(SVLEVEL%,SVCELL%,SVPARENT%)
1788 LOCal svD%
1789 LOCal svNext%
1790 LOCal svContains%
1791 LOCal svChild%
1792 solver_spend
1793 IF sLimit% OR gCancel% THEN
1794 RETurn 0
1795 END IF
1796 sDiscovery%(svCell%)=sClock%
1797 sLow%(svCell%)=sClock%
1798 sClock% = sClock%+1
1799 svContains% = (svCell%=sB%(sRouteColour%))
1800   FOR svD%=0 TO gDegree%(svCell%)-1
1801 solver_spend
1802 IF sLimit% OR gCancel% THEN
1803 RETurn 0
1804 END IF
1805 svNext% = gNb%(svCell%,svD%)
1806 IF solver_has%((svLevel%),(svNext%),(sRouteColour%)) THEN
1807 IF sDiscovery%(svNext%)<0 THEN
1808 svChild% = solver_visit%((svLevel%),(svNext%),(svCell%))
1809         sLow%(svCell%)=gen_min((sLow%(svCell%)),(sLow%(svNext%)))
1810 IF svChild% AND sLow%(svNext%)>=sDiscovery%(svCell%) THEN
1811 sMandatory%(svCell%)=1
1812 END IF
1813 svContains% = (svContains% OR svChild%)
1814       ELSE
1815 IF svNext%<>svParent% THEN
1816 sLow%(svCell%)=gen_min((sLow%(svCell%)),(sDiscovery%(svNext%)))
1817 END IF
1818       END IF
1819     END IF
1820 IF sLimit% OR gCancel% THEN
1821 RETurn 0
1822 END IF
1823   END FOR svD%
1824 RETurn svContains%
1825 END DEFine
1826 DEFine FuNction SOLVER_ROUTES%(SPLEVEL%,SPRECORD%)
1827 LOCal spColour%
1828 LOCal spCell%
1829 LOCal spChanged%
1830 LOCal spReach%
1831 spChanged% = 0
1832   FOR spColour%=0 TO OPT_PAIRS%-1
1833 solver_spend
1834 IF sLimit% OR gCancel% THEN
1835 RETurn 0
1836 END IF
1837 sRouteColour% = spColour%
1838 sClock% = 0
1839     FOR spCell%=0 TO gN%-1
1840 sDiscovery%(spCell%)=-1
1841 sLow%(spCell%)=0
1842 sMandatory%(spCell%)=0
1843     END FOR spCell%
1844 spReach% = solver_visit%((spLevel%),(sA%(spColour%)),(-1))
1845 IF sLimit% OR gCancel% THEN
1846 RETurn 0
1847 END IF
1848 IF NOT spReach% THEN
1849 sBad% = 1
1850 RETurn 0
1851 END IF
1852     FOR spCell%=0 TO gN%-1
1853 IF solver_has%((spLevel%),(spCell%),(spColour%)) THEN
1854 IF sDiscovery%(spCell%)<0 THEN
1855 IF solver_drop%((spLevel%),(spCell%),(spColour%),(0),(spRecord%)) THEN
1856 spChanged% = 1
1857 END IF
1858         ELSE
1859 IF sMandatory%(spCell%) THEN
1860 IF solver_force%((spLevel%),(spCell%),(spColour%),(0),(spRecord%)) THEN
1861 spChanged% = 1
1862 END IF
1863           END IF
1864         END IF
1865       END IF
1866 IF sBad% THEN
1867 RETurn 0
1868 END IF
1869     END FOR spCell%
1870   END FOR spColour%
1871 IF spChanged% AND spRecord% THEN
1872 SR_ROUTEPASSES=SR_ROUTEPASSES+1
1873 END IF
1874 RETurn spChanged%
1875 END DEFine
1876 DEFine PROCedure SOLVER_PROPAGATE(SULEVEL%)
1877 LOCal suChanged%
1878   REPeat suAgain
1879 solver_local (suLevel%),(0)
1880 IF sBad% OR sLimit% OR gCancel% THEN
1881 RETurn 
1882 END IF
1883 suChanged% = solver_routes%((suLevel%),(0))
1884 IF NOT suChanged% OR sBad% OR sLimit% OR gCancel% THEN
1885 EXIT suAgain
1886 END IF
1887   END REPeat suAgain
1888 END DEFine
1889 DEFine FuNction SOLVER_MATERIALISE%(SMLEVEL%)
1890 LOCal smC%
1891 LOCal smI%
1892 LOCal smAt%
1893 LOCal smPrevious%
1894 LOCal smNext%
1895 LOCal smFound%
1896 LOCal smLength%
1897 LOCal smTotal%
1898 LOCal smD%
1899   FOR smC%=0 TO OPT_PAIRS%-1
1900 smTotal% = 0
1901     FOR smI%=0 TO gN%-1
1902 IF solver_fixed%((smLevel%),(smI%))=smC% THEN
1903 smTotal% = smTotal%+1
1904 END IF
1905     END FOR smI%
1906 smAt% = sA%(smC%)
1907 smPrevious% = -1
1908 smLength% = 0
1909     REPeat smWalk
1910 solver_spend
1911 IF sLimit% OR gCancel% THEN
1912 RETurn 0
1913 END IF
1914 smLength% = smLength%+1
1915 IF smAt%=sB%(smC%) THEN
1916 EXIT smWalk
1917 END IF
1918 smFound% = 0
1919 smNext% = -1
1920       FOR smD%=0 TO gDegree%(smAt%)-1
1921 smI% = gNb%(smAt%,smD%)
1922 IF smI%<>smPrevious% AND solver_fixed%((smLevel%),(smI%))=smC% THEN
1923 smFound% = smFound%+1
1924 smNext% = smI%
1925 END IF
1926       END FOR smD%
1927 IF smFound%<>1 OR smLength%>gN% THEN
1928 sBad% = 1
1929 RETurn 0
1930 END IF
1931 smPrevious% = smAt%
1932 smAt% = smNext%
1933     END REPeat smWalk
1934 IF smLength%<>smTotal% THEN
1935 sBad% = 1
1936 RETurn 0
1937 END IF
1938   END FOR smC%
1939 RETurn 1
1940 END DEFine
1941 DEFine FuNction SOLVER_SEARCH%(SELEVEL%,SEDEPTH%)
1942 LOCal seBest%
1943 LOCal seSize%
1944 LOCal seI%
1945 LOCal seC%
1946 LOCal seComplete%
1947 LOCal seChild%
1948 LOCal seOK%
1949 IF SR_SOLUTIONCOUNT%>=2 THEN
1950 RETurn 0
1951 END IF
1952 solver_spend
1953 IF sLimit% OR gCancel% THEN
1954 RETurn 0
1955 END IF
1956 IF SR_SEARCHNODES>=sSearchLimit THEN
1957 sLimit% = 1
1958 SR_STATUS$="budgetExceeded"
1959 SR_LIMITREASON$="searchNodes"
1960 RETurn 0
1961 END IF
1962 SR_SEARCHNODES=SR_SEARCHNODES+1
1963 SR_MAXDEPTH%=gen_max((SR_MAXDEPTH%),(seDepth%))
1964 sBad% = 0
1965 solver_propagate (seLevel%)
1966 IF sLimit% OR gCancel% THEN
1967 RETurn 0
1968 END IF
1969 IF sBad% THEN
1970 sBad% = 0
1971 RETurn 1
1972 END IF
1973 seBest% = -1
1974 seSize% = 65
1975   FOR seI%=0 TO gN%-1
1976 IF solver_count%((seLevel%),(seI%))>1 AND solver_count%((seLevel%),(seI%))<seSize% THEN
1977 seBest% = seI%
1978 seSize% = solver_count%((seLevel%),(seI%))
1979 END IF
1980   END FOR seI%
1981 IF seBest%<0 THEN
1982 seOK% = solver_materialise%((seLevel%))
1983 IF sLimit% OR gCancel% THEN
1984 RETurn 0
1985 END IF
1986 IF sBad% THEN
1987 sBad% = 0
1988 RETurn 1
1989 END IF
1990 IF SR_SOLUTIONCOUNT%=0 THEN
1991 SR_FIRSTNODES=SR_SEARCHNODES
1992 SR_FIRSTDEPTH%=seDepth%
1993 END IF
1994 SR_SOLUTIONCOUNT%=SR_SOLUTIONCOUNT%+1
1995 RETurn 1
1996   END IF
1997 seComplete% = 1
1998   FOR seC%=0 TO sColours%-1
1999 IF solver_has%((seLevel%),(seBest%),(seC%)) THEN
2000 IF SR_SOLUTIONCOUNT%>=2 THEN
2001 RETurn 0
2002 END IF
2003 solver_copy (seLevel%),(seLevel%+1)
2004 seOK% = solver_force%((seLevel%+1),(seBest%),(seC%),(0),(0))
2005 seChild% = solver_search%((seLevel%+1),(seDepth%+1))
2006 seComplete% = (seComplete% AND seChild%)
2007 IF sLimit% OR gCancel% THEN
2008 RETurn 0
2009 END IF
2010     END IF
2011   END FOR seC%
2012 RETurn seComplete%
2013 END DEFine
2014 DEFine PROCedure SOLVER_ANALYSE(SABUDGET,SASEARCHLIMIT)
2015 LOCal saI%
2016 LOCal saJ%
2017 LOCal saC%
2018 LOCal saWord%
2019 LOCal saOK%
2020 LOCal saSingle%
2021 LOCal saRoutes%
2022 LOCal saEliminated%
2023 LOCal saGrading%
2024 DIM SR_STATUS$(20)
2025 SR_STATUS$ = ""
2026 DIM SR_LIMITREASON$(128)
2027 SR_LIMITREASON$ = ""
2028 DIM SR_TIER$(20)
2029 SR_TIER$ = ""
2030 DIM SR_GRADE$(12)
2031 SR_GRADE$ = ""
2032 SR_SCORE% = 0
2033 SR_LOGICAL% = 0
2034 SR_LOCALDEDUCTIONS = 0
2035 SR_ROUTEDEDUCTIONS = 0
2036 SR_ROUTEPASSES = 0
2037 SR_TRIALSTEPS = 0
2038 SR_TRIALASSUMPTIONS = 0
2039 SR_SEARCHNODES = 0
2040 SR_FIRSTNODES = 0
2041 SR_FIRSTDEPTH% = 0
2042 SR_MAXDEPTH% = 0
2043 SR_SOLUTIONCOUNT% = 0
2044 SR_EXHAUSTIVE% = 0
2045 DIM SR_UNIQUENESS$(20)
2046 SR_UNIQUENESS$ = ""
2047 SR_WORK = 0
2048 SR_WORKLIMIT = 0
2049 DIM SR_BOARDKEY$(64)
2050 SR_BOARDKEY$ = ""
2051 SR_STATUS$="complete"
2052 SR_TIER$="local"
2053 SR_GRADE$="unrated"
2054 SR_WORKLIMIT=saBudget
2055 SR_UNIQUENESS$="notEstablished"
2056 SR_BOARDKEY$=identity_board$
2057 sBad% = 0
2058 sLimit% = 0
2059 sSearchLimit = saSearchLimit
2060 gHasReason% = 1
2061 IF OPT_WIDTH%>16 OR OPT_HEIGHT%>16 OR OPT_PAIRS%>64-(NOT OPT_FULL%) THEN
2062 SR_STATUS$="unsupported"
2063 SR_TIER$="unknown"
2064 SR_LIMITREASON$="Solver supports dimensions up to 16x16 and at most 64 full-coverage or 63 connect-only pairs."
2065 RETurn 
2066   END IF
2067 sColours% = OPT_PAIRS%+(NOT OPT_FULL%)
2068   FOR saC%=0 TO 63
2069 sWord%(saC%)=INT(saC%/16)
2070 sBit%(saC%)=word16%((gPower(saC% MOD 16)))
2071   END FOR saC%
2072 IF sMemory THEN
2073 RECHP sMemory
2074 END IF
2075 sMemory = 0
2076 solverFrameBytes = 12*gN%
2077 sMemory = ALCHP((gN%+1)*solverFrameBytes)
2078 IF NOT sMemory THEN
2079 gExit% = 1
2080 gError$ = "Not enough memory for solver frames."
2081 RETurn 
2082 END IF
2083   FOR saI%=0 TO gN%
2084     sAddress(saI%)=sMemory+saI%*solverFrameBytes
2085   END FOR saI%
2086 DIM sRequired%(gN%-1)
2087 DIM sDiscovery%(gN%-1)
2088 DIM sLow%(gN%-1)
2089 DIM sMandatory%(gN%-1)
2090 DIM sA%(OPT_PAIRS%-1)
2091 DIM sB%(OPT_PAIRS%-1)
2092 DIM sColourMap%(OPT_PAIRS%-1)
2093   FOR saC%=0 TO OPT_PAIRS%-1
2094 sA%(saC%)=gA%(saC%+1)
2095 sB%(saC%)=gB%(saC%+1)
2096 sColourMap%(saC%)=gCanonical%(saC%+1)
2097   END FOR saC%
2098   FOR saI%=0 TO gN%-1
2099 solver_set_count (0),(saI%),(sColours%)
2100 solver_set_fixed (0),(saI%),(-1)
2101 sRequired%(saI%)=2
2102 IF sColours%=1 THEN
2103 solver_set_fixed (0),(saI%),(0)
2104 END IF
2105     FOR saWord%=0 TO 3
2106 saJ% = gen_max((0),(gen_min((16),(sColours%-16*saWord%))))
2107 solver_put (0),(saI%),(saWord%),(word16%((gPower(saJ%)-1)))
2108     END FOR saWord%
2109   END FOR saI%
2110   FOR saC%=0 TO OPT_PAIRS%-1
2111 saOK% = solver_force%((0),(sA%(saC%)),(saC%),(0),(0))
2112 saOK% = solver_force%((0),(sB%(saC%)),(saC%),(0),(0))
2113 sRequired%(sA%(saC%))=1
2114 sRequired%(sB%(saC%))=1
2115   END FOR saC%
2116 saGrading% = 1
2117   REPeat saGrade
2118 solver_local (0),(1)
2119 IF sBad% OR sLimit% OR gCancel% THEN
2120 EXIT saGrade
2121 END IF
2122 saSingle% = 1
2123     FOR saI%=0 TO gN%-1
2124 IF solver_count%((0),(saI%))<>1 THEN
2125 saSingle% = 0
2126 EXIT saI%
2127 END IF
2128     END FOR saI%
2129 IF saSingle% THEN
2130 saOK% = solver_materialise%((0))
2131 IF NOT sBad% AND NOT sLimit% AND NOT gCancel% THEN
2132 SR_SOLUTIONCOUNT%=1
2133 SR_LOGICAL%=1
2134 SR_EXHAUSTIVE%=1
2135 END IF
2136       EXIT saGrade
2137     END IF
2138 saRoutes% = solver_routes%((0),(1))
2139 IF sBad% OR sLimit% OR gCancel% THEN
2140 EXIT saGrade
2141 END IF
2142 IF saRoutes% THEN
2143 IF SR_TIER$="local" THEN
2144 SR_TIER$="routes"
2145 END IF
2146     ELSE
2147 saEliminated% = 0
2148       FOR saI%=0 TO gN%-1
2149 IF solver_count%((0),(saI%))>1 THEN
2150           FOR saC%=0 TO sColours%-1
2151 IF solver_has%((0),(saI%),(saC%)) THEN
2152 SR_TRIALASSUMPTIONS=SR_TRIALASSUMPTIONS+1
2153 solver_copy (0),(1)
2154 saOK% = solver_force%((1),(saI%),(saC%),(0),(0))
2155 solver_propagate (1)
2156 IF sLimit% OR gCancel% THEN
2157 EXIT saC%
2158 END IF
2159 IF sBad% THEN
2160 sBad% = 0
2161 saOK% = solver_drop%((0),(saI%),(saC%),(0),(0))
2162 SR_TRIALSTEPS=SR_TRIALSTEPS+1
2163 SR_TIER$="trial"
2164 saEliminated% = 1
2165 EXIT saC%
2166               END IF
2167             END IF
2168           END FOR saC%
2169         END IF
2170 IF saEliminated% OR sBad% OR sLimit% OR gCancel% THEN
2171 EXIT saI%
2172 END IF
2173       END FOR saI%
2174 IF sBad% OR sLimit% OR gCancel% THEN
2175 EXIT saGrade
2176 END IF
2177 IF NOT saEliminated% THEN
2178 SR_TIER$="search"
2179 EXIT saGrade
2180 END IF
2181     END IF
2182   END REPeat saGrade
2183 IF sBad% THEN
2184 SR_TIER$="unsatisfiable"
2185 SR_EXHAUSTIVE%=1
2186 sBad% = 0
2187   ELSE
2188 IF NOT SR_LOGICAL% AND NOT sLimit% AND NOT gCancel% THEN
2189       SR_EXHAUSTIVE%=solver_search%((0),(0))
2190 IF SR_EXHAUSTIVE% AND SR_SOLUTIONCOUNT%=0 THEN
2191 SR_TIER$="unsatisfiable"
2192 END IF
2193     END IF
2194   END IF
2195 IF SR_SOLUTIONCOUNT%>=2 THEN
2196     SR_UNIQUENESS$="multiple"
2197   ELSE
2198 IF SR_EXHAUSTIVE% THEN
2199 SR_UNIQUENESS$="none"
2200 IF SR_SOLUTIONCOUNT%=1 THEN
2201 SR_UNIQUENESS$="unique"
2202 END IF
2203     END IF
2204   END IF
2205 IF OPT_FULL% AND SR_STATUS$="complete" AND SR_UNIQUENESS$="unique" THEN
2206     SELect ON SR_TIER$
2207 ="local":
2208 SR_GRADE$="easy"
2209 SR_SCORE%=10+gen_min((19),(INT(SR_LOCALDEDUCTIONS/gN%)))
2210 ="routes":
2211 SR_GRADE$="medium"
2212 SR_SCORE%=30+gen_min((24),(SR_ROUTEDEDUCTIONS))
2213 ="trial":
2214 SR_GRADE$="hard"
2215 SR_SCORE%=55+gen_min((24),(SR_TRIALSTEPS))
2216 =REMAINDER:
2217 SR_GRADE$="expert"
2218 SR_SCORE%=80+gen_min((20),(INT(2*gen_log2((SR_SEARCHNODES+1)))))
2219     END SELect
2220   END IF
2221 END DEFine
2222 GM_BOARDCELLCOUNT = 0
2223 GM_PAIRCOUNT = 0
2224 GM_ENDPOINTCOUNT = 0
2225 GM_CORNERENDPOINTCOUNT = 0
2226 GM_EDGEENDPOINTCOUNT = 0
2227 GM_MEANCORNERDISTANCE = 0
2228 GM_MEANEDGEDISTANCE = 0
2229 GM_MEANAVAILABLEEXITS = 0
2230 GM_SINGLEEXITENDPOINTCOUNT = 0
2231 GM_ADJACENTPAIRCOUNT = 0
2232 GM_ALIGNEDPAIRCOUNT = 0
2233 GM_MEANMANHATTANDISTANCE = 0
2234 GM_MEANSHORTESTDISTANCE = 0
2235 GM_MINIMUMREQUIREDCELLS = 0
2236 GM_MINIMUMCOVERAGERATIO = 0
2237 GM_COVERAGESLACKCELLS = 0
2238 GM_REQUIREDEXTRACOVERAGECELLS = 0
2239 GM_FORCEDCORNERTURNCOUNT = 0
2240 GM_MEANLOG2SHORTESTROUTECOUNT = 0
2241 GM_SHORTESTROUTECOUNTLIMIT = 0
2242 GM_CAPPEDROUTEPAIRCOUNT = 0
2243 GM_ROUTEINTERIOROCCURRENCES = 0
2244 GM_SHAREDROUTEINTERIOROCCURRENCES = 0
2245 GM_ROUTEOVERLAPRATIO = 0
2246 GM_MAXROUTEOVERLAP = 0
2247 GM_CROSSLABELENDPOINTADJACENCIES = 0
2248 GM_CLUECROWDINGRATIO = 0
2249 rec_t1 = 0
2250 IF rec_t1 < 0 OR rec_t1 > 32767 OR rec_t1 <> INT(rec_t1) THEN PRINT "Invalid record bound: GEOPAIR": STOP
2251 DIM GEOPAIR_MANHATTANDISTANCE%(rec_t1)
2252 FOR rec_t2 = 0 TO rec_t1
2253 GEOPAIR_MANHATTANDISTANCE%(rec_t2) = 0
2254 END FOR rec_t2
2255 DIM GEOPAIR_SHORTESTDISTANCE%(rec_t1)
2256 FOR rec_t3 = 0 TO rec_t1
2257 GEOPAIR_SHORTESTDISTANCE%(rec_t3) = 0
2258 END FOR rec_t3
2259 DIM GEOPAIR_SHORTESTROUTECOUNT%(rec_t1)
2260 FOR rec_t4 = 0 TO rec_t1
2261 GEOPAIR_SHORTESTROUTECOUNT%(rec_t4) = 0
2262 END FOR rec_t4
2263 DIM GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t1)
2264 FOR rec_t5 = 0 TO rec_t1
2265 GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t5) = 0
2266 END FOR rec_t5
2267 DIM GEOPAIR_MINENDPOINTEXITS%(rec_t1)
2268 FOR rec_t6 = 0 TO rec_t1
2269 GEOPAIR_MINENDPOINTEXITS%(rec_t6) = 0
2270 END FOR rec_t6
2271 DIM GEOPAIR_MAXENDPOINTEXITS%(rec_t1)
2272 FOR rec_t7 = 0 TO rec_t1
2273 GEOPAIR_MAXENDPOINTEXITS%(rec_t7) = 0
2274 END FOR rec_t7
2275 DIM GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t1)
2276 FOR rec_t8 = 0 TO rec_t1
2277 GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t8) = 0
2278 END FOR rec_t8
2279 DIM GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t1)
2280 FOR rec_t9 = 0 TO rec_t1
2281 GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t9) = 0
2282 END FOR rec_t9
2283 DIM geoA%(0)
2284 DIM geoB%(0)
2285 DIM geoWays%(0)
2286 DIM geoUsage%(0)
2287 DIM geoMap%(0)
2288 DIM geoLog(0)
2289 DIM geoNames$(9,32)
2290 DIM geoValues(9)
2291 DIM geoWeights%(9)
2292 DIM geoPoints(9)
2293 DIM shortOwner%(0)
2294 DIM shortOrder%(0)
2295 DIM shortPrevious%(0)
2296 DIM shortPairOrder%(0)
2297 geoCount% = 0
2298 geoScore% = 0
2299 geoRaw = 0
2300 geoEasy% = 30
2301 geoHard% = 55
2302 shortSolved% = 0
2303 shortTrials% = 0
2304 shortLimit% = 32
2305 shortFirst% = 0
2306 DEFine PROCedure GEO_DISTANCES(GDPAIR%,GDSOURCE%,GDSECOND%)
2307 LOCal gdI%
2308 LOCal gdHead%
2309 LOCal gdTail%
2310 LOCal gdAt%
2311 LOCal gdN%
2312 LOCal gdD%
2313 LOCal gdNextDistance%
2314   FOR gdI%=0 TO gN%-1
2315 IF gdSecond% THEN
2316 geoB%(gdI%)=-1
2317 END IF
2318 IF NOT gdSecond% THEN
2319 geoA%(gdI%)=-1
2320 geoWays%(gdI%)=0
2321 END IF
2322   END FOR gdI%
2323 IF gdSecond% THEN
2324 geoB%(gdSource%)=0
2325 END IF
2326 IF NOT gdSecond% THEN
2327 geoA%(gdSource%)=0
2328 geoWays%(gdSource%)=1
2329 END IF
2330 gdHead% = 0
2331 gdTail% = 1
2332 gQueue%(0)=gdSource%
2333   REPeat gdBFS
2334 IF gdHead%>=gdTail% THEN
2335 EXIT gdBFS
2336 END IF
2337 gdAt% = gQueue%(gdHead%)
2338 gdHead% = gdHead%+1
2339 gdNextDistance% = geoA%(gdAt%)+1
2340 IF gdSecond% THEN
2341 gdNextDistance% = geoB%(gdAt%)+1
2342 END IF
2343     FOR gdD%=0 TO gDegree%(gdAt%)-1
2344 gdN% = gNb%(gdAt%,gdD%)
2345 IF gEndpoint%(gdN%)=0 OR gEndpoint%(gdN%)=gdPair% THEN
2346 IF gdSecond% THEN
2347 IF geoB%(gdN%)<0 THEN
2348 geoB%(gdN%)=gdNextDistance%
2349 gQueue%(gdTail%)=gdN%
2350 gdTail% = gdTail%+1
2351 END IF
2352         ELSE
2353 IF geoA%(gdN%)<0 THEN
2354 geoA%(gdN%)=gdNextDistance%
2355 gQueue%(gdTail%)=gdN%
2356 gdTail% = gdTail%+1
2357 END IF
2358 IF geoA%(gdN%)=gdNextDistance% THEN
2359 geoWays%(gdN%)=gen_min((1025),(geoWays%(gdN%)+geoWays%(gdAt%)))
2360 END IF
2361         END IF
2362       END IF
2363     END FOR gdD%
2364   END REPeat gdBFS
2365 END DEFine
2366 DEFine FuNction GEO_CORNER%(GCCELL%)
2367 RETurn (gcCell%=0 OR gcCell%=OPT_WIDTH%-1 OR gcCell%=gN%-OPT_WIDTH% OR gcCell%=gN%-1)
2368 END DEFine
2369 DEFine FuNction GEO_EDGE%(GECELL%)
2370 RETurn (geCell% MOD OPT_WIDTH%=0 OR geCell% MOD OPT_WIDTH%=OPT_WIDTH%-1 OR geCell%<OPT_WIDTH% OR geCell%>=gN%-OPT_WIDTH%)
2371 END DEFine
2372 DEFine FuNction GEO_EXITS%(GXCELL%,GXPAIR%,GXDISTANCE%)
2373 LOCal gxD%
2374 LOCal gxN%
2375 LOCal gxCount%
2376 IF gxDistance%=1 THEN
2377 RETurn 1
2378 END IF
2379 gxCount% = 0
2380   FOR gxD%=0 TO gDegree%(gxCell%)-1
2381 gxN% = gNb%(gxCell%,gxD%)
2382 IF gEndpoint%(gxN%)=0 OR gEndpoint%(gxN%)=gxPair% THEN
2383 gxCount% = gxCount%+1
2384 END IF
2385   END FOR gxD%
2386 RETurn gxCount%
2387 END DEFine
2388 DEFine PROCedure GEO_COMPONENT(GCNAME$,GCVALUE,GCWEIGHT%)
2389 geoNames$(geoCount%)=gcName$
2390 geoValues(geoCount%)=gen_six((gen_max((0),(gen_min((1),(gcValue))))))
2391 geoWeights%(geoCount%)=gcWeight%
2392 geoPoints(geoCount%)=gen_six((gcWeight%*geoValues(geoCount%)))
2393 geoRaw = geoRaw+geoPoints(geoCount%)
2394 geoCount% = geoCount%+1
2395 END DEFine
2396 DEFine PROCedure GEO_ASSESS
2397 LOCal rec_t1
2398 LOCal rec_t2
2399 LOCal rec_t3
2400 LOCal rec_t4
2401 LOCal rec_t5
2402 LOCal rec_t6
2403 LOCal rec_t7
2404 LOCal rec_t8
2405 LOCal rec_t9
2406 LOCal gaI%
2407 LOCal gaJ%
2408 LOCal gaP%
2409 LOCal gaActual%
2410 LOCal gaStart%
2411 LOCal gaEnd%
2412 LOCal gaM%
2413 LOCal gaX%
2414 LOCal gaY%
2415 LOCal gaCorner%
2416 LOCal gaEdge%
2417 LOCal gaExA%
2418 LOCal gaExB%
2419 LOCal gaN%
2420 LOCal gaD%
2421 LOCal gaCorridor%
2422 LOCal gaSwap%
2423 LOCal gaCornerSum
2424 LOCal gaEdgeSum
2425 LOCal gaExits
2426 LOCal gaDist
2427 LOCal gaManhattan
2428 LOCal gaLogSum
2429 LOCal gaLogValue
2430 LOCal gaDenom
2431 GM_BOARDCELLCOUNT = 0
2432 GM_PAIRCOUNT = 0
2433 GM_ENDPOINTCOUNT = 0
2434 GM_CORNERENDPOINTCOUNT = 0
2435 GM_EDGEENDPOINTCOUNT = 0
2436 GM_MEANCORNERDISTANCE = 0
2437 GM_MEANEDGEDISTANCE = 0
2438 GM_MEANAVAILABLEEXITS = 0
2439 GM_SINGLEEXITENDPOINTCOUNT = 0
2440 GM_ADJACENTPAIRCOUNT = 0
2441 GM_ALIGNEDPAIRCOUNT = 0
2442 GM_MEANMANHATTANDISTANCE = 0
2443 GM_MEANSHORTESTDISTANCE = 0
2444 GM_MINIMUMREQUIREDCELLS = 0
2445 GM_MINIMUMCOVERAGERATIO = 0
2446 GM_COVERAGESLACKCELLS = 0
2447 GM_REQUIREDEXTRACOVERAGECELLS = 0
2448 GM_FORCEDCORNERTURNCOUNT = 0
2449 GM_MEANLOG2SHORTESTROUTECOUNT = 0
2450 GM_SHORTESTROUTECOUNTLIMIT = 0
2451 GM_CAPPEDROUTEPAIRCOUNT = 0
2452 GM_ROUTEINTERIOROCCURRENCES = 0
2453 GM_SHAREDROUTEINTERIOROCCURRENCES = 0
2454 GM_ROUTEOVERLAPRATIO = 0
2455 GM_MAXROUTEOVERLAP = 0
2456 GM_CROSSLABELENDPOINTADJACENCIES = 0
2457 GM_CLUECROWDINGRATIO = 0
2458 rec_t1 = OPT_PAIRS%
2459 IF rec_t1 < 0 OR rec_t1 > 32767 OR rec_t1 <> INT(rec_t1) THEN PRINT "Invalid record bound: GEOPAIR": STOP
2460 DIM GEOPAIR_MANHATTANDISTANCE%(rec_t1)
2461 FOR rec_t2 = 0 TO rec_t1
2462 GEOPAIR_MANHATTANDISTANCE%(rec_t2) = 0
2463 END FOR rec_t2
2464 DIM GEOPAIR_SHORTESTDISTANCE%(rec_t1)
2465 FOR rec_t3 = 0 TO rec_t1
2466 GEOPAIR_SHORTESTDISTANCE%(rec_t3) = 0
2467 END FOR rec_t3
2468 DIM GEOPAIR_SHORTESTROUTECOUNT%(rec_t1)
2469 FOR rec_t4 = 0 TO rec_t1
2470 GEOPAIR_SHORTESTROUTECOUNT%(rec_t4) = 0
2471 END FOR rec_t4
2472 DIM GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t1)
2473 FOR rec_t5 = 0 TO rec_t1
2474 GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t5) = 0
2475 END FOR rec_t5
2476 DIM GEOPAIR_MINENDPOINTEXITS%(rec_t1)
2477 FOR rec_t6 = 0 TO rec_t1
2478 GEOPAIR_MINENDPOINTEXITS%(rec_t6) = 0
2479 END FOR rec_t6
2480 DIM GEOPAIR_MAXENDPOINTEXITS%(rec_t1)
2481 FOR rec_t7 = 0 TO rec_t1
2482 GEOPAIR_MAXENDPOINTEXITS%(rec_t7) = 0
2483 END FOR rec_t7
2484 DIM GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t1)
2485 FOR rec_t8 = 0 TO rec_t1
2486 GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t8) = 0
2487 END FOR rec_t8
2488 DIM GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t1)
2489 FOR rec_t9 = 0 TO rec_t1
2490 GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t9) = 0
2491 END FOR rec_t9
2492 DIM geoA%(gN%-1)
2493 DIM geoB%(gN%-1)
2494 DIM geoWays%(gN%-1)
2495 DIM geoUsage%(gN%-1)
2496 DIM geoMap%(OPT_PAIRS%)
2497 DIM geoLog(OPT_PAIRS%)
2498 GM_BOARDCELLCOUNT=gN%
2499 GM_PAIRCOUNT=OPT_PAIRS%
2500 GM_ENDPOINTCOUNT=2*OPT_PAIRS%
2501 GM_SHORTESTROUTECOUNTLIMIT=1024
2502 gaCornerSum = 0
2503 gaEdgeSum = 0
2504 gaExits = 0
2505 gaDist = 0
2506 gaManhattan = 0
2507 gaLogSum = 0
2508   FOR gaI%=0 TO gN%-1
2509     geoUsage%(gaI%)=0
2510 IF gEndpoint%(gaI%) THEN
2511 gaX% = gen_min((gaI% MOD OPT_WIDTH%),(OPT_WIDTH%-1-(gaI% MOD OPT_WIDTH%)))
2512 gaY% = gen_min((INT(gaI%/OPT_WIDTH%)),(OPT_HEIGHT%-1-INT(gaI%/OPT_WIDTH%)))
2513 gaCornerSum = gaCornerSum+gaX%+gaY%
2514 gaEdgeSum = gaEdgeSum+gen_min((gaX%),(gaY%))
2515 IF geo_corner%((gaI%)) THEN
2516         GM_CORNERENDPOINTCOUNT=GM_CORNERENDPOINTCOUNT+1
2517       ELSE
2518 IF geo_edge%((gaI%)) THEN
2519 GM_EDGEENDPOINTCOUNT=GM_EDGEENDPOINTCOUNT+1
2520 END IF
2521       END IF
2522       FOR gaD%=0 TO gDegree%(gaI%)-1
2523 gaN% = gNb%(gaI%,gaD%)
2524 IF gaN%>gaI% AND gEndpoint%(gaN%) AND gEndpoint%(gaN%)<>gEndpoint%(gaI%) THEN
2525 GM_CROSSLABELENDPOINTADJACENCIES=GM_CROSSLABELENDPOINTADJACENCIES+1
2526 END IF
2527       END FOR gaD%
2528     ELSE
2529 IF OPT_FULL% AND OPT_WIDTH%>1 AND OPT_HEIGHT%>1 AND geo_corner%((gaI%)) THEN
2530 GM_FORCEDCORNERTURNCOUNT=GM_FORCEDCORNERTURNCOUNT+1
2531 END IF
2532     END IF
2533   END FOR gaI%
2534   FOR gaP%=1 TO OPT_PAIRS%
2535     geoMap%(gaP%)=gaP%
2536   END FOR gaP%
2537   FOR gaP%=2 TO OPT_PAIRS%
2538 gaActual% = geoMap%(gaP%)
2539 gaJ% = gaP%-1
2540     REPeat gaSort
2541 IF gaJ%<1 THEN
2542 EXIT gaSort
2543 END IF
2544 IF NOT gen_less%((gen_label$((gaActual%))),(gen_label$((geoMap%(gaJ%))))) THEN
2545 EXIT gaSort
2546 END IF
2547 geoMap%(gaJ%+1)=geoMap%(gaJ%)
2548 gaJ% = gaJ%-1
2549     END REPeat gaSort
2550     geoMap%(gaJ%+1)=gaActual%
2551   END FOR gaP%
2552   FOR gaP%=1 TO OPT_PAIRS%
2553 gen_poll
2554 IF gCancel% THEN
2555 RETurn 
2556 END IF
2557 gaActual% = geoMap%(gaP%)
2558 gaStart% = gStart%(gaActual%)
2559 gaEnd% = gFinish%(gaActual%)
2560 gaM% = ABS((gaStart% MOD OPT_WIDTH%)-(gaEnd% MOD OPT_WIDTH%))+ABS(INT(gaStart%/OPT_WIDTH%)-INT(gaEnd%/OPT_WIDTH%))
2561 geo_distances (gaActual%),(gaStart%),(0)
2562 geo_distances (gaActual%),(gaEnd%),(1)
2563 IF geoA%(gaEnd%)<0 THEN
2564 gExit% = 1
2565 gError$ = "Internal error: clues have no connecting route."
2566 RETurn 
2567 END IF
2568 gaExA% = geo_exits%((gaStart%),(gaActual%),(gaM%))
2569 gaExB% = geo_exits%((gaEnd%),(gaActual%),(gaM%))
2570 GEOPAIR_MANHATTANDISTANCE%(gaP%)=gaM%
2571 GEOPAIR_SHORTESTDISTANCE%(gaP%)=geoA%(gaEnd%)
2572 GEOPAIR_SHORTESTROUTECOUNT%(gaP%)=gen_min((1024),(geoWays%(gaEnd%)))
2573 GEOPAIR_SHORTESTROUTECOUNTCAPPED%(gaP%)=(geoWays%(gaEnd%)>1024)
2574 GEOPAIR_MINENDPOINTEXITS%(gaP%)=gen_min((gaExA%),(gaExB%))
2575 GEOPAIR_MAXENDPOINTEXITS%(gaP%)=gen_max((gaExA%),(gaExB%))
2576     geoLog(gaP%)=gen_log2((GEOPAIR_SHORTESTROUTECOUNT%(gaP%)))
2577 IF gaM%=1 THEN
2578 GM_ADJACENTPAIRCOUNT=GM_ADJACENTPAIRCOUNT+1
2579 END IF
2580 IF gaStart% MOD OPT_WIDTH%=gaEnd% MOD OPT_WIDTH% OR INT(gaStart%/OPT_WIDTH%)=INT(gaEnd%/OPT_WIDTH%) THEN
2581 GM_ALIGNEDPAIRCOUNT=GM_ALIGNEDPAIRCOUNT+1
2582 END IF
2583 IF gaExA%=1 THEN
2584 GM_SINGLEEXITENDPOINTCOUNT=GM_SINGLEEXITENDPOINTCOUNT+1
2585 END IF
2586 IF gaExB%=1 THEN
2587 GM_SINGLEEXITENDPOINTCOUNT=GM_SINGLEEXITENDPOINTCOUNT+1
2588 END IF
2589 IF GEOPAIR_SHORTESTROUTECOUNTCAPPED%(gaP%) THEN
2590 GM_CAPPEDROUTEPAIRCOUNT=GM_CAPPEDROUTEPAIRCOUNT+1
2591 END IF
2592 gaExits = gaExits+gaExA%+gaExB%
2593 gaDist = gaDist+geoA%(gaEnd%)
2594 gaManhattan = gaManhattan+gaM%
2595     GM_MINIMUMREQUIREDCELLS=GM_MINIMUMREQUIREDCELLS+geoA%(gaEnd%)+1
2596 gaCorridor% = 0
2597     FOR gaI%=0 TO gN%-1
2598 IF NOT gEndpoint%(gaI%) AND geoA%(gaI%)>=0 AND geoB%(gaI%)>=0 AND geoA%(gaI%)+geoB%(gaI%)=geoA%(gaEnd%) THEN
2599 gaCorridor% = gaCorridor%+1
2600 geoUsage%(gaI%)=geoUsage%(gaI%)+1
2601       END IF
2602     END FOR gaI%
2603 GEOPAIR_SHORTESTROUTEINTERIORCELLS%(gaP%)=gaCorridor%
2604 GM_ROUTEINTERIOROCCURRENCES=GM_ROUTEINTERIOROCCURRENCES+gaCorridor%
2605   END FOR gaP%
2606   FOR gaP%=1 TO OPT_PAIRS%
2607 gaActual% = geoMap%(gaP%)
2608 gaEnd% = gFinish%(gaActual%)
2609 geo_distances (gaActual%),(gStart%(gaActual%)),(0)
2610 geo_distances (gaActual%),(gaEnd%),(1)
2611 gaCorridor% = 0
2612     FOR gaI%=0 TO gN%-1
2613 IF NOT gEndpoint%(gaI%) AND geoUsage%(gaI%)>1 AND geoA%(gaI%)>=0 AND geoB%(gaI%)>=0 AND geoA%(gaI%)+geoB%(gaI%)=geoA%(gaEnd%) THEN
2614 gaCorridor% = gaCorridor%+1
2615 END IF
2616     END FOR gaI%
2617 GEOPAIR_SHAREDROUTEINTERIORCELLS%(gaP%)=gaCorridor%
2618 GM_SHAREDROUTEINTERIOROCCURRENCES=GM_SHAREDROUTEINTERIOROCCURRENCES+gaCorridor%
2619   END FOR gaP%
2620   FOR gaI%=0 TO gN%-1
2621     GM_MAXROUTEOVERLAP=gen_max((GM_MAXROUTEOVERLAP),(geoUsage%(gaI%)))
2622   END FOR gaI%
2623   FOR gaP%=2 TO OPT_PAIRS%
2624 gaLogValue = geoLog(gaP%)
2625 gaJ% = gaP%-1
2626     REPeat gaLogSort
2627 IF gaJ%<1 THEN
2628 EXIT gaLogSort
2629 END IF
2630 IF geoLog(gaJ%)<=gaLogValue THEN
2631 EXIT gaLogSort
2632 END IF
2633 geoLog(gaJ%+1)=geoLog(gaJ%)
2634 gaJ% = gaJ%-1
2635     END REPeat gaLogSort
2636     geoLog(gaJ%+1)=gaLogValue
2637   END FOR gaP%
2638   FOR gaP%=1 TO OPT_PAIRS%
2639 gaLogSum = gaLogSum+geoLog(gaP%)
2640   END FOR gaP%
2641 GM_MEANCORNERDISTANCE=gen_six((gaCornerSum/GM_ENDPOINTCOUNT))
2642 GM_MEANEDGEDISTANCE=gen_six((gaEdgeSum/GM_ENDPOINTCOUNT))
2643 GM_MEANAVAILABLEEXITS=gen_six((gaExits/GM_ENDPOINTCOUNT))
2644 GM_MEANMANHATTANDISTANCE=gen_six((gaManhattan/OPT_PAIRS%))
2645 GM_MEANSHORTESTDISTANCE=gen_six((gaDist/OPT_PAIRS%))
2646 GM_MEANLOG2SHORTESTROUTECOUNT=gen_six((gaLogSum/OPT_PAIRS%))
2647 GM_MINIMUMCOVERAGERATIO=gen_six((GM_MINIMUMREQUIREDCELLS/gN%))
2648 GM_COVERAGESLACKCELLS=gN%-GM_MINIMUMREQUIREDCELLS
2649 IF OPT_FULL% THEN
2650 GM_REQUIREDEXTRACOVERAGECELLS=gen_max((0),(GM_COVERAGESLACKCELLS))
2651 END IF
2652 IF GM_ROUTEINTERIOROCCURRENCES THEN
2653 GM_ROUTEOVERLAPRATIO=gen_six((GM_SHAREDROUTEINTERIOROCCURRENCES/GM_ROUTEINTERIOROCCURRENCES))
2654 END IF
2655   GM_CLUECROWDINGRATIO=gen_six((GM_CROSSLABELENDPOINTADJACENCIES/(2*GM_ENDPOINTCOUNT)))
2656 geoCount% = 0
2657 geoRaw = 0
2658 geoEasy% = 30
2659 geoHard% = 55
2660 geo_component ("boardScale"),(gen_log2((gN%))/12),(10)
2661 gaX% = 10
2662 IF OPT_FULL% THEN
2663 gaX% = 8
2664 END IF
2665 gaLogValue = OPT_PAIRS%
2666 geo_component ("pairLoad"),(gaLogValue/16),(gaX%)
2667 gaX% = 18
2668 IF OPT_FULL% THEN
2669 gaX% = 16
2670 END IF
2671 geo_component ("endpointFreedom"),((GM_MEANAVAILABLEEXITS-1)/3),(gaX%)
2672 gaX% = 12
2673 IF OPT_FULL% THEN
2674 gaX% = 8
2675 END IF
2676 geo_component ("shortRouteAmbiguity"),(GM_MEANLOG2SHORTESTROUTECOUNT/10),(gaX%)
2677 gaX% = 22
2678 IF OPT_FULL% THEN
2679 gaX% = 24
2680 END IF
2681 geo_component ("routeOverlap"),(GM_ROUTEOVERLAPRATIO),(gaX%)
2682 geo_component ("clueCrowding"),(GM_CLUECROWDINGRATIO),(6)
2683 gaDenom = gen_max((1),(INT((OPT_WIDTH%-1)/2)+INT((OPT_HEIGHT%-1)/2)))
2684 geo_component ("cornerDistance"),(GM_MEANCORNERDISTANCE/gaDenom),(6)
2685 IF OPT_FULL% THEN
2686 geoEasy% = 35
2687 geoHard% = 60
2688 geo_component ("requiredCoverageDetours"),(GM_REQUIREDEXTRACOVERAGECELLS/gN%),(22)
2689 geo_component ("forcedEndpointCredit"),(GM_SINGLEEXITENDPOINTCOUNT/GM_ENDPOINTCOUNT),(-18)
2690 geo_component ("forcedCornerCredit"),(GM_FORCEDCORNERTURNCOUNT/4),(-6)
2691   ELSE
2692 geo_component ("minimumCoveragePressure"),(GM_MINIMUMCOVERAGERATIO),(16)
2693 geo_component ("forcedEndpointCredit"),(GM_SINGLEEXITENDPOINTCOUNT/GM_ENDPOINTCOUNT),(-20)
2694   END IF
2695 geoRaw = gen_six((geoRaw))
2696 geoScore% = gen_max((0),(gen_min((100),(gen_round((geoRaw))))))
2697 END DEFine
2698 DEFine PROCedure SHORTEST_SCREEN(SQTRIALS%)
2699 LOCal sqT%
2700 LOCal sqI%
2701 LOCal sqJ%
2702 LOCal sqSwap%
2703 LOCal sqP%
2704 LOCal sqActual%
2705 LOCal sqA%
2706 LOCal sqB%
2707 LOCal sqHead%
2708 LOCal sqTail%
2709 LOCal sqAt%
2710 LOCal sqD%
2711 LOCal sqN%
2712 LOCal sqCount%
2713 LOCal sqFailed%
2714 LOCal sqOrder%
2715 LOCal sqOK%
2716 shortSolved% = 0
2717 shortTrials% = 0
2718 shortFirst% = 0
2719 shortLimit% = sqTrials%
2720 DIM shortOwner%(gN%-1)
2721 DIM shortOrder%(gN%-1)
2722 DIM shortPrevious%(gN%-1)
2723 DIM shortPairOrder%(OPT_PAIRS%)
2724 identity_canonical
2725 rng_seed (1),(1413566023)
2726 gRngStream% = 1
2727   FOR sqT%=1 TO sqTrials%
2728 gen_poll
2729 IF gCancel% THEN
2730 EXIT sqT%
2731 END IF
2732     FOR sqI%=1 TO OPT_PAIRS%
2733       shortPairOrder%(sqI%)=sqI%
2734     END FOR sqI%
2735 IF sqT%=1 THEN
2736       FOR sqI%=2 TO OPT_PAIRS%
2737 sqP% = shortPairOrder%(sqI%)
2738 sqActual% = gCanonical%(sqP%)
2739 sqA% = gStart%(sqActual%)
2740 sqB% = gFinish%(sqActual%)
2741 sqCount% = ABS((sqA% MOD OPT_WIDTH%)-(sqB% MOD OPT_WIDTH%))+ABS(INT(sqA%/OPT_WIDTH%)-INT(sqB%/OPT_WIDTH%))
2742 sqJ% = sqI%-1
2743         REPeat sqSort
2744 IF sqJ%<1 THEN
2745 EXIT sqSort
2746 END IF
2747 sqActual% = gCanonical%(shortPairOrder%(sqJ%))
2748 sqA% = gStart%(sqActual%)
2749 sqB% = gFinish%(sqActual%)
2750 sqD% = ABS((sqA% MOD OPT_WIDTH%)-(sqB% MOD OPT_WIDTH%))+ABS(INT(sqA%/OPT_WIDTH%)-INT(sqB%/OPT_WIDTH%))
2751 IF sqD%<sqCount% OR (sqD%=sqCount% AND shortPairOrder%(sqJ%)<sqP%) THEN
2752 EXIT sqSort
2753 END IF
2754 shortPairOrder%(sqJ%+1)=shortPairOrder%(sqJ%)
2755 sqJ% = sqJ%-1
2756         END REPeat sqSort
2757         shortPairOrder%(sqJ%+1)=sqP%
2758       END FOR sqI%
2759     ELSE
2760       FOR sqI%=OPT_PAIRS% TO 2 STEP -1
2761 sqJ% = 1+rng_next%((sqI%))
2762 sqSwap% = shortPairOrder%(sqI%)
2763 shortPairOrder%(sqI%)=shortPairOrder%(sqJ%)
2764 shortPairOrder%(sqJ%)=sqSwap%
2765       END FOR sqI%
2766     END IF
2767     FOR sqI%=0 TO gN%-1
2768 shortOwner%(sqI%)=0
2769 shortOrder%(sqI%)=0
2770     END FOR sqI%
2771 sqFailed% = 0
2772     FOR sqP%=1 TO OPT_PAIRS%
2773 sqActual% = gCanonical%(shortPairOrder%(sqP%))
2774 sqA% = gA%(shortPairOrder%(sqP%))
2775 sqB% = gB%(shortPairOrder%(sqP%))
2776       FOR sqI%=0 TO gN%-1
2777         shortPrevious%(sqI%)=-1
2778       END FOR sqI%
2779 shortPrevious%(sqA%)=sqA%
2780 sqHead% = 0
2781 sqTail% = 1
2782 gQueue%(0)=sqA%
2783       REPeat sqBFS
2784 IF sqHead%>=sqTail% OR shortPrevious%(sqB%)>=0 THEN
2785 EXIT sqBFS
2786 END IF
2787 sqAt% = gQueue%(sqHead%)
2788 sqHead% = sqHead%+1
2789 sqCount% = gDegree%(sqAt%)
2790         FOR sqD%=0 TO sqCount%-1
2791           gScratch%(sqD%)=gNb%(sqAt%,sqD%)
2792         END FOR sqD%
2793         FOR sqD%=sqCount%-1 TO 1 STEP -1
2794 sqJ% = rng_next%((sqD%+1))
2795 sqSwap% = gScratch%(sqD%)
2796 gScratch%(sqD%)=gScratch%(sqJ%)
2797 gScratch%(sqJ%)=sqSwap%
2798         END FOR sqD%
2799         FOR sqD%=0 TO sqCount%-1
2800 sqN% = gScratch%(sqD%)
2801 IF shortPrevious%(sqN%)<0 AND NOT shortOwner%(sqN%) AND (gEndpoint%(sqN%)=0 OR gEndpoint%(sqN%)=sqActual%) THEN
2802 shortPrevious%(sqN%)=sqAt%
2803 gQueue%(sqTail%)=sqN%
2804 sqTail% = sqTail%+1
2805           END IF
2806         END FOR sqD%
2807       END REPeat sqBFS
2808 IF shortPrevious%(sqB%)<0 THEN
2809 sqFailed% = 1
2810 EXIT sqP%
2811 END IF
2812 sqAt% = sqB%
2813 sqOrder% = 1
2814       REPeat sqTrace
2815 shortOwner%(sqAt%)=sqActual%
2816 shortOrder%(sqAt%)=sqOrder%
2817 IF sqAt%=sqA% THEN
2818 EXIT sqTrace
2819 END IF
2820 sqOrder% = sqOrder%+1
2821 sqAt% = shortPrevious%(sqAt%)
2822       END REPeat sqTrace
2823     END FOR sqP%
2824 shortTrials% = sqT%
2825 IF NOT sqFailed% THEN
2826 sqOK% = 1
2827       FOR sqI%=0 TO gN%-1
2828 IF shortOwner%(sqI%) THEN
2829           FOR sqD%=0 TO gDegree%(sqI%)-1
2830 sqN% = gNb%(sqI%,sqD%)
2831 IF shortOwner%(sqN%)=shortOwner%(sqI%) AND ABS(shortOrder%(sqN%)-shortOrder%(sqI%))<>1 THEN
2832 sqOK% = 0
2833 END IF
2834           END FOR sqD%
2835         END IF
2836       END FOR sqI%
2837 IF sqOK% THEN
2838 shortSolved% = 1
2839 shortFirst% = sqT%
2840 EXIT sqT%
2841 END IF
2842     END IF
2843   END FOR sqT%
2844 gRngStream% = 0
2845 gHasShortest% = 1
2846 END DEFine
2847 DIM CHOSENREASON_STATUS$(20)
2848 CHOSENREASON_STATUS$ = ""
2849 DIM CHOSENREASON_LIMITREASON$(128)
2850 CHOSENREASON_LIMITREASON$ = ""
2851 DIM CHOSENREASON_TIER$(20)
2852 CHOSENREASON_TIER$ = ""
2853 DIM CHOSENREASON_GRADE$(12)
2854 CHOSENREASON_GRADE$ = ""
2855 CHOSENREASON_SCORE% = 0
2856 CHOSENREASON_LOGICAL% = 0
2857 CHOSENREASON_LOCALDEDUCTIONS = 0
2858 CHOSENREASON_ROUTEDEDUCTIONS = 0
2859 CHOSENREASON_ROUTEPASSES = 0
2860 CHOSENREASON_TRIALSTEPS = 0
2861 CHOSENREASON_TRIALASSUMPTIONS = 0
2862 CHOSENREASON_SEARCHNODES = 0
2863 CHOSENREASON_FIRSTNODES = 0
2864 CHOSENREASON_FIRSTDEPTH% = 0
2865 CHOSENREASON_MAXDEPTH% = 0
2866 CHOSENREASON_SOLUTIONCOUNT% = 0
2867 CHOSENREASON_EXHAUSTIVE% = 0
2868 DIM CHOSENREASON_UNIQUENESS$(20)
2869 CHOSENREASON_UNIQUENESS$ = ""
2870 CHOSENREASON_WORK = 0
2871 CHOSENREASON_WORKLIMIT = 0
2872 DIM CHOSENREASON_BOARDKEY$(64)
2873 CHOSENREASON_BOARDKEY$ = ""
2874 DIM selWalk%(0)
2875 DIM selScore(2,4)
2876 selConsidered% = 0
2877 selEvaluated% = 0
2878 selDuplicates% = 0
2879 selBudgetRejected% = 0
2880 selAssessments% = 0
2881 selAccepted% = 0
2882 selFirst% = -1
2883 selSeen% = 0
2884 DIM selStartingTier$(20)
2885 DIM selKey$(64)
2886 DIM selAlgorithm$(64)
2887 selStartingTrials = 0
2888 selChosenReason% = 0
2889 selChosenShortest% = 0
2890 selShortSolved% = 0
2891 selShortTrials% = 0
2892 selShortLimit% = 0
2893 selShortFirst% = 0
2894 DEFine FuNction SELECTION_ACTIVE%
2895 RETurn (OPT_ASSESS% OR OPT_TARGET%>0 OR OPT_UNIQUE% OR OPT_SHORTEST%)
2896 END DEFine
2897 DEFine FuNction SELECTION_STRENGTHENED%
2898 RETurn (OPT_MINTRIALS>0 OR OPT_MAXTRIALS>=0 OR OPT_MINDEPTH%>0 OR OPT_MINNODES%>0)
2899 END DEFine
2900 DEFine FuNction SELECTION_MINIMUMS%
2901 IF SR_STATUS$<>"complete" THEN
2902 RETurn 0
2903 END IF
2904 IF SR_TRIALSTEPS<OPT_MINTRIALS OR SR_FIRSTDEPTH%<OPT_MINDEPTH% OR SR_FIRSTNODES<OPT_MINNODES% THEN
2905 RETurn 0
2906 END IF
2907 IF OPT_MAXTRIALS>=0 AND SR_TRIALSTEPS>OPT_MAXTRIALS THEN
2908 RETurn 0
2909 END IF
2910 RETurn 1
2911 END DEFine
2912 DEFine FuNction SELECTION_COMPARE%(SCROW%)
2913 LOCal scI%
2914   FOR scI%=0 TO 4
2915 IF selScore(0,scI%)>selScore(scRow%,scI%) THEN
2916 RETurn 1
2917 END IF
2918 IF selScore(0,scI%)<selScore(scRow%,scI%) THEN
2919 RETurn -1
2920 END IF
2921   END FOR scI%
2922 RETurn 0
2923 END DEFine
2924 DEFine PROCedure SELECTION_SAVE
2925 LOCal svI%
2926   FOR svI%=0 TO gN%-1
2927 gSavedOwner%(svI%)=gOwner%(svI%)
2928 gSavedOrder%(svI%)=gOrder%(svI%)
2929   END FOR svI%
2930 selChosenReason% = gHasReason%
2931 selChosenShortest% = gHasShortest%
2932 selShortSolved% = shortSolved%
2933 selShortTrials% = shortTrials%
2934 selShortLimit% = shortLimit%
2935 selShortFirst% = shortFirst%
2936   CHOSENREASON_STATUS$=SR_STATUS$
2937   CHOSENREASON_LIMITREASON$=SR_LIMITREASON$
2938   CHOSENREASON_TIER$=SR_TIER$
2939   CHOSENREASON_GRADE$=SR_GRADE$
2940   CHOSENREASON_SCORE%=SR_SCORE%
2941   CHOSENREASON_LOGICAL%=SR_LOGICAL%
2942   CHOSENREASON_LOCALDEDUCTIONS=SR_LOCALDEDUCTIONS
2943   CHOSENREASON_ROUTEDEDUCTIONS=SR_ROUTEDEDUCTIONS
2944   CHOSENREASON_ROUTEPASSES=SR_ROUTEPASSES
2945   CHOSENREASON_TRIALSTEPS=SR_TRIALSTEPS
2946   CHOSENREASON_TRIALASSUMPTIONS=SR_TRIALASSUMPTIONS
2947   CHOSENREASON_SEARCHNODES=SR_SEARCHNODES
2948   CHOSENREASON_FIRSTNODES=SR_FIRSTNODES
2949   CHOSENREASON_FIRSTDEPTH%=SR_FIRSTDEPTH%
2950   CHOSENREASON_MAXDEPTH%=SR_MAXDEPTH%
2951   CHOSENREASON_SOLUTIONCOUNT%=SR_SOLUTIONCOUNT%
2952   CHOSENREASON_EXHAUSTIVE%=SR_EXHAUSTIVE%
2953   CHOSENREASON_UNIQUENESS$=SR_UNIQUENESS$
2954   CHOSENREASON_WORK=SR_WORK
2955   CHOSENREASON_WORKLIMIT=SR_WORKLIMIT
2956   CHOSENREASON_BOARDKEY$=SR_BOARDKEY$
2957 END DEFine
2958 DEFine PROCedure SELECTION_RESTORE
2959 LOCal svI%
2960 LOCal svP%
2961   FOR svP%=1 TO OPT_PAIRS%
2962     gLength%(svP%)=0
2963   END FOR svP%
2964   FOR svI%=0 TO gN%-1
2965 gOwner%(svI%)=gSavedOwner%(svI%)
2966 gOrder%(svI%)=gSavedOrder%(svI%)
2967 svP% = gOwner%(svI%)
2968 IF svP% THEN
2969       gLength%(svP%)=gen_max((gLength%(svP%)),(gOrder%(svI%)))
2970 IF gOrder%(svI%)=1 THEN
2971 gStart%(svP%)=svI%
2972 END IF
2973     END IF
2974   END FOR svI%
2975   FOR svI%=0 TO gN%-1
2976 svP% = gOwner%(svI%)
2977 IF svP% THEN
2978 IF gOrder%(svI%)=gLength%(svP%) THEN
2979 gFinish%(svP%)=svI%
2980 END IF
2981     END IF
2982   END FOR svI%
2983 gen_endpoints
2984 gHasReason% = selChosenReason%
2985 gHasShortest% = selChosenShortest%
2986 shortSolved% = selShortSolved%
2987 shortTrials% = selShortTrials%
2988 shortLimit% = selShortLimit%
2989 shortFirst% = selShortFirst%
2990   SR_STATUS$=CHOSENREASON_STATUS$
2991   SR_LIMITREASON$=CHOSENREASON_LIMITREASON$
2992   SR_TIER$=CHOSENREASON_TIER$
2993   SR_GRADE$=CHOSENREASON_GRADE$
2994   SR_SCORE%=CHOSENREASON_SCORE%
2995   SR_LOGICAL%=CHOSENREASON_LOGICAL%
2996   SR_LOCALDEDUCTIONS=CHOSENREASON_LOCALDEDUCTIONS
2997   SR_ROUTEDEDUCTIONS=CHOSENREASON_ROUTEDEDUCTIONS
2998   SR_ROUTEPASSES=CHOSENREASON_ROUTEPASSES
2999   SR_TRIALSTEPS=CHOSENREASON_TRIALSTEPS
3000   SR_TRIALASSUMPTIONS=CHOSENREASON_TRIALASSUMPTIONS
3001   SR_SEARCHNODES=CHOSENREASON_SEARCHNODES
3002   SR_FIRSTNODES=CHOSENREASON_FIRSTNODES
3003   SR_FIRSTDEPTH%=CHOSENREASON_FIRSTDEPTH%
3004   SR_MAXDEPTH%=CHOSENREASON_MAXDEPTH%
3005   SR_SOLUTIONCOUNT%=CHOSENREASON_SOLUTIONCOUNT%
3006   SR_EXHAUSTIVE%=CHOSENREASON_EXHAUSTIVE%
3007   SR_UNIQUENESS$=CHOSENREASON_UNIQUENESS$
3008   SR_WORK=CHOSENREASON_WORK
3009   SR_WORKLIMIT=CHOSENREASON_WORKLIMIT
3010   SR_BOARDKEY$=CHOSENREASON_BOARDKEY$
3011 END DEFine
3012 DEFine PROCedure SELECTION_CONSIDER
3013 LOCal skI%
3014 LOCal skMatches%
3015 LOCal skRank%
3016 LOCal skDesired%
3017 LOCal skPriority%
3018 LOCal skDistance
3019 gHasReason% = 0
3020 gHasShortest% = 0
3021 selKey$ = identity_symmetry$
3022   FOR skI%=1 TO selSeen%
3023 IF cli_equal%((gSeen$(skI%)),(selKey$)) THEN
3024 selDuplicates% = selDuplicates%+1
3025 selDistinct% = 0
3026 RETurn 
3027 END IF
3028   END FOR skI%
3029 selSeen% = selSeen%+1
3030 gSeen$(selSeen%)=selKey$
3031 selDistinct% = 1
3032 selEvaluated% = selEvaluated%+1
3033 IF OPT_SHORTEST% THEN
3034 shortest_screen (32)
3035 IF shortSolved% THEN
3036 selDistinct% = 0
3037 RETurn 
3038 END IF
3039   END IF
3040 IF OPT_ASSESS% OR OPT_TARGET%>0 OR OPT_UNIQUE% THEN
3041 selAssessments% = selAssessments%+1
3042 solver_analyse (OPT_SOLVERWORK),(20000)
3043 IF SR_UNIQUENESS$="none" THEN
3044 gExit% = 1
3045 gError$ = "Internal solver contradiction for a validated witness."
3046 RETurn 
3047 END IF
3048 IF selStartingTier$="notAssessed" THEN
3049 selStartingTier$ = SR_TIER$
3050 selStartingTrials = SR_TRIALSTEPS
3051 END IF
3052   END IF
3053 IF gCancel% THEN
3054 RETurn 
3055 END IF
3056 IF (OPT_TARGET%>0 OR OPT_UNIQUE%) AND gHasReason% AND SR_STATUS$<>"complete" THEN
3057 selBudgetRejected% = selBudgetRejected%+1
3058 END IF
3059 skMatches% = 1
3060 IF OPT_TARGET%>0 THEN
3061 skMatches% = (SR_STATUS$="complete" AND SR_GRADE$=gen_grade$((OPT_TARGET%)))
3062 END IF
3063 IF OPT_UNIQUE% AND SR_UNIQUENESS$<>"unique" THEN
3064 skMatches% = 0
3065 END IF
3066 IF selection_strengthened% THEN
3067 IF NOT gHasReason% OR NOT selection_minimums% THEN
3068 skMatches% = 0
3069 END IF
3070   END IF
3071   FOR skI%=0 TO 4
3072     selScore(0,skI%)=0
3073   END FOR skI%
3074   selScore(0,0)=-100
3075 IF gHasReason% AND SR_STATUS$="complete" AND SR_GRADE$<>"unrated" THEN
3076 skRank% = gen_rank%((SR_GRADE$))
3077 skDesired% = OPT_TARGET%
3078 IF skDesired%=0 THEN
3079 skDesired% = 3
3080 END IF
3081 skPriority% = skRank%
3082 IF skRank%>skDesired% THEN
3083 skPriority% = -9-skRank%
3084 END IF
3085 skDistance = 0
3086 IF OPT_MAXTRIALS>=0 THEN
3087 skDistance = gen_max((0),(gen_max((OPT_MINTRIALS-SR_TRIALSTEPS),(SR_TRIALSTEPS-OPT_MAXTRIALS))))
3088 END IF
3089 selScore(0,0)=skPriority%
3090 selScore(0,1)=-skDistance
3091 IF OPT_MINDEPTH%>0 OR OPT_MINNODES%>0 THEN
3092 selScore(0,2)=SR_FIRSTDEPTH%
3093 selScore(0,3)=SR_FIRSTNODES
3094 selScore(0,4)=SR_TRIALSTEPS
3095     ELSE
3096 selScore(0,2)=SR_TRIALSTEPS
3097 selScore(0,3)=SR_ROUTEDEDUCTIONS
3098     END IF
3099   END IF
3100 IF skMatches% AND (NOT selAccepted% OR selection_compare%((2))>=0) THEN
3101 selection_save
3102 selAccepted% = 1
3103 IF selFirst%<0 THEN
3104 selFirst% = selConsidered%
3105 END IF
3106     FOR skI%=0 TO 4
3107       selScore(2,skI%)=selScore(0,skI%)
3108     END FOR skI%
3109   END IF
3110 END DEFine
3111 DEFine PROCedure GENERATION_BASE(GBSEED)
3112 LOCal gbFallback%
3113 gSuccess% = 0
3114 gRngStream% = 0
3115 IF NOT OPT_FULL% THEN
3116 grow_generate (gbSeed)
3117 RETurn 
3118 END IF
3119 rng_seed (0),(gbSeed)
3120 cover_init
3121 gbFallback% = -1
3122 IF OPT_ATTEMPTS%>=128 THEN
3123 gbFallback% = gen_max((64),(OPT_ATTEMPTS%-128))
3124 END IF
3125 cover_find (128),(gen_max((64),(gN%))),(gbFallback%),(1)
3126 IF covFound% THEN
3127 gen_validate
3128 gSuccess% = gValid%
3129 END IF
3130 END DEFine
3131 DEFine PROCedure SELECTION_GENERATE(SGSEED)
3132 LOCal sgI%
3133 LOCal sgMove%
3134 LOCal sgMoves%
3135 LOCal sgCurrent%
3136 LOCal sgStagnant%
3137 LOCal sgStructured%
3138 LOCal sgTight%
3139 LOCal sgRestart%
3140 LOCal sgCompare%
3141 LOCal sgProposal%
3142 LOCal sgCandidateSeed
3143 WORK_BOARDATTEMPTS = 0
3144 WORK_TRAVERSALEVALUATIONS = 0
3145 WORK_REFINEMENTEVALUATIONS = 0
3146 WORK_STRUCTUREDSTARTS = 0
3147 DIM SR_STATUS$(20)
3148 SR_STATUS$ = ""
3149 DIM SR_LIMITREASON$(128)
3150 SR_LIMITREASON$ = ""
3151 DIM SR_TIER$(20)
3152 SR_TIER$ = ""
3153 DIM SR_GRADE$(12)
3154 SR_GRADE$ = ""
3155 SR_SCORE% = 0
3156 SR_LOGICAL% = 0
3157 SR_LOCALDEDUCTIONS = 0
3158 SR_ROUTEDEDUCTIONS = 0
3159 SR_ROUTEPASSES = 0
3160 SR_TRIALSTEPS = 0
3161 SR_TRIALASSUMPTIONS = 0
3162 SR_SEARCHNODES = 0
3163 SR_FIRSTNODES = 0
3164 SR_FIRSTDEPTH% = 0
3165 SR_MAXDEPTH% = 0
3166 SR_SOLUTIONCOUNT% = 0
3167 SR_EXHAUSTIVE% = 0
3168 DIM SR_UNIQUENESS$(20)
3169 SR_UNIQUENESS$ = ""
3170 SR_WORK = 0
3171 SR_WORKLIMIT = 0
3172 DIM SR_BOARDKEY$(64)
3173 SR_BOARDKEY$ = ""
3174 gSuccess% = 0
3175 gHasReason% = 0
3176 gHasShortest% = 0
3177 selConsidered% = 0
3178 selEvaluated% = 0
3179 selDuplicates% = 0
3180 selBudgetRejected% = 0
3181 selAssessments% = 0
3182 selAccepted% = 0
3183 selFirst% = -1
3184 selSeen% = 0
3185 selStartingTier$ = "notAssessed"
3186 selStartingTrials = 0
3187 gSelected% = selection_active%
3188 selAlgorithm$ = "ql-random-path-growth-v1"
3189 IF OPT_FULL% THEN
3190 selAlgorithm$ = "ql-hamiltonian-path-partition-v3"
3191 END IF
3192 IF OPT_FULL% AND OPT_VARIED% THEN
3193 selAlgorithm$ = "ql-hamiltonian-path-partition-varied-v1"
3194 END IF
3195 IF NOT gSelected% THEN
3196 generation_base (sgSeed)
3197 RETurn 
3198 END IF
3199 DIM gSeen$(OPT_CANDIDATES%,64)
3200 DIM gSavedOwner%(gN%-1)
3201 DIM gSavedOrder%(gN%-1)
3202 DIM selWalk%(gN%-1)
3203 gRngStream% = 0
3204 rng_seed (0),(sgSeed)
3205 rng_seed (2),(sgSeed)
3206 sgCurrent% = 0
3207 sgStagnant% = 0
3208 IF OPT_FULL% AND OPT_TARGET%>0 THEN
3209 selAlgorithm$ = "ql-solver-guided-hamiltonian-partition-v6"
3210 IF OPT_VARIED% THEN
3211 selAlgorithm$ = "ql-solver-guided-varied-cuts-v2"
3212 END IF
3213 cover_init
3214     REPeat sgGuided
3215 IF selConsidered%>=OPT_CANDIDATES% OR gExit% OR gCancel% THEN
3216 EXIT sgGuided
3217 END IF
3218 IF selFirst%>=0 AND selConsidered%-selFirst%>=OPT_IMPROVE% THEN
3219 EXIT sgGuided
3220 END IF
3221 selConsidered% = selConsidered%+1
3222 sgProposal% = 0
3223 sgStructured% = 0
3224 IF sgStagnant%>=32 THEN
3225 sgCurrent% = 0
3226 sgStagnant% = 0
3227 END IF
3228 IF NOT sgCurrent% THEN
3229 sgRestart% = 128
3230 IF gN%>64 THEN
3231 sgRestart% = 16*gN%
3232 END IF
3233 sgTight% = (gen_product((OPT_MINIMUM%),(OPT_PAIRS%))=gN% OR gen_product((OPT_MAXIMUM%),(OPT_PAIRS%))=gN%)
3234 sgStructured% = ((sgTight% AND selConsidered%=1) OR (selEvaluated%=0 AND selConsidered%=OPT_CANDIDATES%))
3235 sgCompare% = -1
3236 IF sgStructured% THEN
3237 sgCompare% = 0
3238 END IF
3239 cover_find (sgRestart%),(0),(sgCompare%),(0)
3240 IF covFound% THEN
3241 sgProposal% = 1
3242           FOR sgI%=0 TO gN%-1
3243 gWalk%(sgI%)=gBestWalk%(sgI%)
3244 gWalkPos%(gWalk%(sgI%))=sgI%
3245           END FOR sgI%
3246         END IF
3247       ELSE
3248         FOR sgI%=0 TO gN%-1
3249 gWalk%(sgI%)=selWalk%(sgI%)
3250 gWalkPos%(gWalk%(sgI%))=sgI%
3251         END FOR sgI%
3252 sgMoves% = 1+rng_next%((4))
3253         FOR sgMove%=1 TO sgMoves%
3254 cover_backbite
3255         END FOR sgMove%
3256 cover_analyse
3257 IF covScore%<=OPT_PAIRS% THEN
3258 cover_partition
3259 sgProposal% = covFeasible%
3260 END IF
3261       END IF
3262 IF sgProposal% THEN
3263 gen_validate
3264 IF NOT gValid% THEN
3265 gExit% = 1
3266 gError$ = "Internal full-coverage witness validation failed."
3267 EXIT sgGuided
3268 END IF
3269 selection_consider
3270 IF selDistinct% AND (NOT sgCurrent% OR selection_compare%((1))>=0) THEN
3271 sgCompare% = selection_compare%((1))
3272 IF sgCurrent% AND sgCompare%>0 THEN
3273 sgStagnant% = 0
3274           ELSE
3275 sgStagnant% = sgStagnant%+1
3276           END IF
3277 IF NOT sgStructured% OR selScore(0,0)=OPT_TARGET% THEN
3278 sgCurrent% = 1
3279             FOR sgI%=0 TO gN%-1
3280               selWalk%(sgI%)=gWalk%(sgI%)
3281             END FOR sgI%
3282             FOR sgI%=0 TO 4
3283               selScore(1,sgI%)=selScore(0,sgI%)
3284             END FOR sgI%
3285           END IF
3286         ELSE
3287 sgStagnant% = sgStagnant%+1
3288         END IF
3289       ELSE
3290 sgStagnant% = sgStagnant%+1
3291       END IF
3292 gen_poll
3293     END REPeat sgGuided
3294   ELSE
3295     REPeat sgUnguided
3296 IF selAccepted% OR selConsidered%>=OPT_CANDIDATES% OR gExit% OR gCancel% THEN
3297 EXIT sgUnguided
3298 END IF
3299 sgCandidateSeed = sgSeed
3300 IF selConsidered% THEN
3301 gRngStream% = 2
3302 sgCandidateSeed = rng_raw
3303 gRngStream% = 0
3304 END IF
3305 selConsidered% = selConsidered%+1
3306 generation_base (sgCandidateSeed)
3307 IF gSuccess% THEN
3308 selection_consider
3309 END IF
3310     END REPeat sgUnguided
3311   END IF
3312 IF selAccepted% AND NOT gCancel% AND NOT gExit% THEN
3313 selection_restore
3314 gen_validate
3315 gSuccess% = gValid%
3316 IF NOT gValid% THEN
3317 gExit% = 1
3318 gError$ = "Internal selected witness validation failed."
3319 END IF
3320   ELSE
3321 gSuccess% = 0
3322   END IF
3323 END DEFine
3324 DIM cliArgs$(63,4096)
3325 DIM cliNames$(24,32)
3326 DIM cliSeen%(24)
3327 DIM rampPairs%(4)
3328 DIM rampMax%(4)
3329 DIM rampAttempts%(4)
3330 DIM rampCounts%(4)
3331 DIM rampBudget(4)
3332 DIM rampRawMax(4)
3333 cliCount% = 0
3334 cliHelp% = 0
3335 cliNumber = 0
3336 gRawMaximum = 16
3337 rampBaseTrials = 0
3338 rampBaseDepth% = 0
3339 rampBaseNodes% = 0
3340 rampBaseMinimum% = 4
3341 cliNames$(0)="--width"
3342 cliNames$(1)="--height"
3343 cliNames$(2)="--pairs"
3344 cliNames$(3)="--min-length"
3345 cliNames$(4)="--max-length"
3346 cliNames$(5)="--seed"
3347 cliNames$(6)="--count"
3348 cliNames$(7)="--max-attempts"
3349 cliNames$(8)="--candidates"
3350 cliNames$(9)="--improve-steps"
3351 cliNames$(10)="--solver-work"
3352 cliNames$(11)="--min-trials"
3353 cliNames$(12)="--max-trials"
3354 cliNames$(13)="--min-depth"
3355 cliNames$(14)="--min-search-nodes"
3356 cliNames$(15)="--mode"
3357 cliNames$(16)="--cut-style"
3358 cliNames$(17)="--difficulty"
3359 cliNames$(18)="--ramp"
3360 cliNames$(19)="--output"
3361 cliNames$(20)="--show-solution"
3362 cliNames$(21)="--force"
3363 cliNames$(22)="--assess"
3364 cliNames$(23)="--require-unique"
3365 cliNames$(24)="--avoid-shortest"
3366 DEFine FuNction CLI_EQUAL%(CEA$,CEB$)
3367 LOCal ceI%
3368 IF LEN(ceA$)<>LEN(ceB$) THEN
3369 RETurn 0
3370 END IF
3371   FOR ceI%=1 TO LEN(ceA$)
3372 IF CODE(ceA$(ceI%))<>CODE(ceB$(ceI%)) THEN
3373 RETurn 0
3374 END IF
3375   END FOR ceI%
3376 RETurn 1
3377 END DEFine
3378 DEFine PROCedure CLI_ERROR(CEMESSAGE$)
3379 gExit% = 2
3380 gError$ = ceMessage$
3381 END DEFine
3382 DEFine PROCedure CLI_TOKENS(CTINPUT$)
3383 LOCal ctAt%
3384 LOCal ctCode%
3385 LOCal ctQuote%
3386 LOCal ctStarted%
3387 LOCal ctText$(4096)
3388 cliCount% = 0
3389 ctText$ = ""
3390 ctQuote% = 0
3391 ctStarted% = 0
3392   FOR ctAt%=1 TO LEN(ctInput$)
3393 ctCode% = CODE(ctInput$(ctAt%))
3394 IF ctQuote% THEN
3395 IF ctCode%=ctQuote% THEN
3396 ctQuote% = 0
3397       ELSE
3398 ctText$ = ctText$ & CHR$(ctCode%)
3399       END IF
3400     ELSE
3401 IF ctCode%=34 OR ctCode%=39 THEN
3402 ctQuote% = ctCode%
3403 ctStarted% = 1
3404       ELSE
3405 IF ctCode%=32 OR ctCode%=9 OR ctCode%=10 OR ctCode%=13 THEN
3406 IF ctStarted% THEN
3407 IF cliCount%>=64 THEN
3408 cli_error ("Too many arguments.")
3409 RETurn 
3410 END IF
3411 cliArgs$(cliCount%)=ctText$
3412 cliCount% = cliCount%+1
3413 ctText$ = ""
3414 ctStarted% = 0
3415           END IF
3416         ELSE
3417 ctText$ = ctText$ & CHR$(ctCode%)
3418 ctStarted% = 1
3419         END IF
3420       END IF
3421     END IF
3422   END FOR ctAt%
3423 IF ctQuote% THEN
3424 cli_error ("Unclosed quote in option string.")
3425 RETurn 
3426 END IF
3427 IF ctStarted% THEN
3428 IF cliCount%>=64 THEN
3429 cli_error ("Too many arguments.")
3430 RETurn 
3431 END IF
3432 cliArgs$(cliCount%)=ctText$
3433 cliCount% = cliCount%+1
3434   END IF
3435 END DEFine
3436 DEFine FuNction CLI_INTEGER%(CITEXT$)
3437 LOCal ciAt%
3438 LOCal ciSign%
3439 LOCal ciCode%
3440 LOCal ciLimit
3441 cliNumber = 0
3442 ciAt% = 1
3443 ciSign% = 1
3444 ciLimit = 2147483647
3445 IF LEN(ciText$)=0 THEN
3446 RETurn 0
3447 END IF
3448 IF ciText$(1)="-" THEN
3449 ciSign% = -1
3450 ciAt% = 2
3451 ciLimit = 2147483648
3452 END IF
3453 IF ciText$(1)="+" THEN
3454 ciAt% = 2
3455 END IF
3456 IF ciAt%>LEN(ciText$) THEN
3457 RETurn 0
3458 END IF
3459   FOR ciAt%=ciAt% TO LEN(ciText$)
3460 ciCode% = CODE(ciText$(ciAt%))-48
3461 IF ciCode%<0 OR ciCode%>9 THEN
3462 RETurn 0
3463 END IF
3464 IF cliNumber>INT((ciLimit-ciCode%)/10) THEN
3465 RETurn 0
3466 END IF
3467 cliNumber = cliNumber*10+ciCode%
3468   END FOR ciAt%
3469 cliNumber = cliNumber*ciSign%
3470 RETurn 1
3471 END DEFine
3472 DEFine FuNction CLI_GRADE%(CGTEXT$)
3473 LOCal cgI%
3474   FOR cgI%=1 TO 4
3475 IF cli_equal%((cgText$),(gen_grade$((cgI%)))) THEN
3476 RETurn cgI%
3477 END IF
3478   END FOR cgI%
3479 RETurn 0
3480 END DEFine
3481 DEFine PROCedure CLI_VALIDATE
3482 IF OPT_MAXIMUM%<OPT_MINIMUM% THEN
3483 cli_error ("Maximum length must not be less than minimum length.")
3484 RETurn 
3485 END IF
3486 IF gen_product((OPT_PAIRS%),(OPT_MINIMUM%))>OPT_WIDTH%*OPT_HEIGHT% THEN
3487 cli_error ("Pairs and minimum lengths do not fit the board.")
3488 RETurn 
3489 END IF
3490 IF OPT_FULL% THEN
3491 IF gen_product((OPT_PAIRS%),(OPT_MAXIMUM%))<OPT_WIDTH%*OPT_HEIGHT% THEN
3492 cli_error ("Full coverage needs sufficient pairs and maximum length.")
3493 RETurn 
3494 END IF
3495 IF OPT_PAIRS%=1 AND OPT_WIDTH%>1 AND OPT_HEIGHT%>1 THEN
3496 cli_error ("One induced path cannot fill a two-dimensional rectangle.")
3497 RETurn 
3498 END IF
3499   ELSE
3500 IF OPT_VARIED% THEN
3501 cli_error ("--cut-style varied requires full-coverage.")
3502 RETurn 
3503 END IF
3504 IF OPT_TARGET%>0 THEN
3505 cli_error ("Reasoning targets require full-coverage; use --avoid-shortest for connections-only.")
3506 RETurn 
3507 END IF
3508   END IF
3509 IF OPT_SHORTEST% AND OPT_FULL% THEN
3510 cli_error ("--avoid-shortest requires connect-only.")
3511 RETurn 
3512 END IF
3513 IF OPT_MAXTRIALS>=0 AND (OPT_MAXTRIALS<OPT_MINTRIALS OR OPT_TARGET%=0) THEN
3514 cli_error ("--max-trials needs an explicit target and must be at least --min-trials.")
3515 RETurn 
3516 END IF
3517 IF OPT_MINTRIALS>0 AND OPT_TARGET%<3 THEN
3518 cli_error ("--min-trials requires hard or expert.")
3519 RETurn 
3520 END IF
3521 IF (OPT_MINDEPTH%>0 OR OPT_MINNODES%>0) AND OPT_TARGET%<>4 THEN
3522 cli_error ("Depth and node floors require expert.")
3523 RETurn 
3524 END IF
3525 IF OPT_TARGET%>0 OR OPT_UNIQUE% THEN
3526 IF OPT_WIDTH%>16 OR OPT_HEIGHT%>16 OR OPT_PAIRS%>64-(NOT OPT_FULL%) THEN
3527 cli_error ("Solver selection supports up to 16x16 and 64 full/63 connect-only pairs.")
3528 END IF
3529   END IF
3530 IF OPT_FORCE% AND LEN(OPT_OUTPUT$)=0 THEN
3531 cli_error ("--force requires --output.")
3532 END IF
3533 END DEFine
3534 DEFine PROCedure CLI_RAMP
3535 LOCal crI%
3536 LOCal crIndex%
3537 LOCal crSections%
3538 LOCal crArea
3539 LOCal crP%
3540 LOCal crSide
3541 LOCal crPairs
3542 LOCal crMaximum
3543 IF NOT OPT_FULL% OR OPT_WIDTH%>16 OR OPT_HEIGHT%>16 THEN
3544 cli_error ("--ramp requires full coverage on a board up to 16x16.")
3545 RETurn 
3546 END IF
3547 IF cliSeen%(2) OR cliSeen%(17) THEN
3548 cli_error ("--ramp chooses pairs and targets; do not specify --pairs or --difficulty.")
3549 RETurn 
3550 END IF
3551 IF NOT cliSeen%(6) THEN
3552 OPT_COUNT%=12
3553 END IF
3554 crSections% = OPT_RAMPLAST%-OPT_RAMPFIRST%+1
3555 IF OPT_COUNT%<crSections% THEN
3556 cli_error ("Ramp count must include every requested grade.")
3557 RETurn 
3558 END IF
3559 IF OPT_MINTRIALS>0 AND OPT_RAMPLAST%<3 THEN
3560 cli_error ("Trial floor requires a hard/expert ramp section.")
3561 RETurn 
3562 END IF
3563 IF (OPT_MINDEPTH%>0 OR OPT_MINNODES%>0) AND OPT_RAMPLAST%<4 THEN
3564 cli_error ("Depth/node floors require an expert ramp section.")
3565 RETurn 
3566 END IF
3567 crArea = OPT_WIDTH%*OPT_HEIGHT%
3568 crSide = SQRT(crArea)
3569 rampBaseTrials = OPT_MINTRIALS
3570 rampBaseDepth% = OPT_MINDEPTH%
3571 rampBaseNodes% = OPT_MINNODES%
3572 rampBaseMinimum% = OPT_MINIMUM%
3573   FOR crI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
3574     SELect ON crI%
3575       =1
3576 crPairs = crArea/6
3577 IF crArea<=64 THEN
3578 crPairs = 1.25*crSide
3579 END IF
3580 =2:
3581 crPairs = crArea/7.2
3582 =3:
3583 crPairs = crArea/8
3584 =4:
3585 crPairs = 0.5*crSide
3586     END SELect
3587 crP% = gen_max((2),(gen_min((INT(crArea/gen_max((2),(OPT_MINIMUM%)))),(gen_round((crPairs))))))
3588     rampPairs%(crI%)=crP%
3589 crMaximum = gen_max((16),(2*(-INT(-crSide))))
3590 IF crI%=4 THEN
3591 crMaximum = gen_max((16),(2*(-INT(-crArea/crP%))))
3592 END IF
3593 IF cliSeen%(4) THEN
3594 crMaximum = gRawMaximum
3595 END IF
3596 rampRawMax(crI%)=crMaximum
3597 rampMax%(crI%)=gen_min((4096),(crMaximum))
3598 rampAttempts%(crI%)=500
3599 rampBudget(crI%)=2000000
3600 IF crArea>64 THEN
3601 rampAttempts%(crI%)=5000
3602 rampBudget(crI%)=20000000
3603 END IF
3604 IF crArea>144 THEN
3605 rampAttempts%(crI%)=10000
3606 rampBudget(crI%)=100000000
3607 END IF
3608 IF cliSeen%(7) THEN
3609 rampAttempts%(crI%)=OPT_ATTEMPTS%
3610 END IF
3611 IF cliSeen%(10) THEN
3612 rampBudget(crI%)=OPT_SOLVERWORK
3613 END IF
3614 crIndex% = crI%-OPT_RAMPFIRST%
3615 rampCounts%(crI%)=INT(OPT_COUNT%/crSections%)
3616 IF crIndex%<OPT_COUNT% MOD crSections% THEN
3617 rampCounts%(crI%)=rampCounts%(crI%)+1
3618 END IF
3619   END FOR crI%
3620   FOR crI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
3621 cli_section (crI%)
3622 cli_validate
3623 IF gExit% THEN
3624 RETurn 
3625 END IF
3626   END FOR crI%
3627 END DEFine
3628 DEFine PROCedure CLI_SECTION(CSGRADE%)
3629 OPT_TARGET%=csGrade%
3630 OPT_PAIRS%=rampPairs%(csGrade%)
3631 OPT_MAXIMUM%=rampMax%(csGrade%)
3632 gRawMaximum = rampRawMax(csGrade%)
3633 OPT_ATTEMPTS%=rampAttempts%(csGrade%)
3634 OPT_SOLVERWORK=rampBudget(csGrade%)
3635 OPT_MINTRIALS=0
3636 OPT_MINDEPTH%=0
3637 OPT_MINNODES%=0
3638 IF csGrade%>=3 THEN
3639 OPT_MINTRIALS=rampBaseTrials
3640 END IF
3641 IF csGrade%=4 THEN
3642 OPT_MINDEPTH%=rampBaseDepth%
3643 OPT_MINNODES%=rampBaseNodes%
3644 END IF
3645 END DEFine
3646 DEFine PROCedure CLI_PARSE(CPINPUT$)
3647 LOCal cpI%
3648 LOCal cpJ%
3649 LOCal cpFound%
3650 LOCal cpKey%
3651 LOCal cpValue$(4096)
3652 LOCal cpRange%
3653 LOCal cpFirst$(16)
3654 LOCal cpLast$(16)
3655 rampBaseTrials = 0
3656 rampBaseDepth% = 0
3657 rampBaseNodes% = 0
3658 gen_defaults
3659 gRawMaximum = 16
3660 OPT_SEED=gen_mod((DATE),(2147483647))
3661 gExit% = 0
3662 gError$ = ""
3663 cliHelp% = 0
3664 cli_tokens (cpInput$)
3665 IF gExit% THEN
3666 RETurn 
3667 END IF
3668   FOR cpI%=0 TO cliCount%-1
3669 IF cli_equal%((cliArgs$(cpI%)),("--help")) OR cli_equal%((cliArgs$(cpI%)),("-h")) THEN
3670 cliHelp% = 1
3671 RETurn 
3672 END IF
3673   END FOR cpI%
3674   FOR cpI%=0 TO 24
3675     cliSeen%(cpI%)=0
3676   END FOR cpI%
3677 cpI% = 0
3678   REPeat cpParse
3679 IF cpI%>=cliCount% OR gExit% THEN
3680 EXIT cpParse
3681 END IF
3682 cpKey% = -1
3683     FOR cpJ%=0 TO 24
3684 IF cli_equal%((cliArgs$(cpI%)),(cliNames$(cpJ%))) THEN
3685 cpKey% = cpJ%
3686 EXIT cpJ%
3687 END IF
3688     END FOR cpJ%
3689 IF cpKey%<0 THEN
3690 cli_error ("Unknown option. Use --help.")
3691 RETurn 
3692 END IF
3693 IF cliSeen%(cpKey%) THEN
3694 cli_error ("Option specified more than once.")
3695 RETurn 
3696 END IF
3697     cliSeen%(cpKey%)=1
3698 IF cpKey%<20 THEN
3699 cpI% = cpI%+1
3700 IF cpI%>=cliCount% THEN
3701 cli_error ("Missing option value.")
3702 RETurn 
3703 END IF
3704 cpValue$ = cliArgs$(cpI%)
3705     END IF
3706 IF cpKey%<15 THEN
3707 IF NOT cli_integer%((cpValue$)) THEN
3708 cli_error ("Expected a 32-bit integer option value.")
3709 RETurn 
3710 END IF
3711     END IF
3712     SELect ON cpKey%
3713       =0
3714 IF cliNumber<1 OR cliNumber>64 THEN
3715 cli_error ("Out of range: --width (1..64).")
3716 RETurn 
3717 END IF
3718         OPT_WIDTH%=cliNumber
3719       =1
3720 IF cliNumber<1 OR cliNumber>64 THEN
3721 cli_error ("Out of range: --height (1..64).")
3722 RETurn 
3723 END IF
3724         OPT_HEIGHT%=cliNumber
3725       =2
3726 IF cliNumber<1 OR cliNumber>2048 THEN
3727 cli_error ("Out of range: --pairs (1..2048).")
3728 RETurn 
3729 END IF
3730         OPT_PAIRS%=cliNumber
3731       =3
3732 IF cliNumber<2 OR cliNumber>4096 THEN
3733 cli_error ("Out of range: --min-length (2..4096).")
3734 RETurn 
3735 END IF
3736         OPT_MINIMUM%=cliNumber
3737       =4
3738 IF cliNumber<2 OR cliNumber>2147483647 THEN
3739 cli_error ("Out of range: --max-length (2..2147483647).")
3740 RETurn 
3741 END IF
3742 gRawMaximum = cliNumber
3743 OPT_MAXIMUM%=gen_min((4096),(cliNumber))
3744       =5
3745 IF cliNumber<0 OR cliNumber>2147483647 THEN
3746 cli_error ("Out of range: --seed (0..2147483647).")
3747 RETurn 
3748 END IF
3749         OPT_SEED=cliNumber
3750       =6
3751 IF cliNumber<1 OR cliNumber>10000 THEN
3752 cli_error ("Out of range: --count (1..10000).")
3753 RETurn 
3754 END IF
3755         OPT_COUNT%=cliNumber
3756       =7
3757 IF cliNumber<1 OR cliNumber>10000 THEN
3758 cli_error ("Out of range: --max-attempts (1..10000).")
3759 RETurn 
3760 END IF
3761         OPT_ATTEMPTS%=cliNumber
3762       =8
3763 IF cliNumber<1 OR cliNumber>10000 THEN
3764 cli_error ("Out of range: --candidates (1..10000).")
3765 RETurn 
3766 END IF
3767         OPT_CANDIDATES%=cliNumber
3768       =9
3769 IF cliNumber<0 OR cliNumber>10000 THEN
3770 cli_error ("Out of range: --improve-steps (0..10000).")
3771 RETurn 
3772 END IF
3773         OPT_IMPROVE%=cliNumber
3774       =10
3775 IF cliNumber<1 OR cliNumber>100000000 THEN
3776 cli_error ("Out of range: --solver-work (1..100000000).")
3777 RETurn 
3778 END IF
3779         OPT_SOLVERWORK=cliNumber
3780       =11
3781 IF cliNumber<0 OR cliNumber>1000000 THEN
3782 cli_error ("Out of range: --min-trials (0..1000000).")
3783 RETurn 
3784 END IF
3785         OPT_MINTRIALS=cliNumber
3786       =12
3787 IF cliNumber<0 OR cliNumber>1000000 THEN
3788 cli_error ("Out of range: --max-trials (0..1000000).")
3789 RETurn 
3790 END IF
3791         OPT_MAXTRIALS=cliNumber
3792       =13
3793 IF cliNumber<0 OR cliNumber>256 THEN
3794 cli_error ("Out of range: --min-depth (0..256).")
3795 RETurn 
3796 END IF
3797         OPT_MINDEPTH%=cliNumber
3798       =14
3799 IF cliNumber<0 OR cliNumber>20000 THEN
3800 cli_error ("Out of range: --min-search-nodes (0..20000).")
3801 RETurn 
3802 END IF
3803         OPT_MINNODES%=cliNumber
3804       =15
3805 IF NOT cli_equal%((cpValue$),("connect-only")) AND NOT cli_equal%((cpValue$),("full-coverage")) THEN
3806 cli_error ("Mode must be connect-only or full-coverage.")
3807 RETurn 
3808 END IF
3809         OPT_FULL%=cli_equal%((cpValue$),("full-coverage"))
3810       =16
3811 IF NOT cli_equal%((cpValue$),("random")) AND NOT cli_equal%((cpValue$),("varied")) THEN
3812 cli_error ("Cut style must be random or varied.")
3813 RETurn 
3814 END IF
3815         OPT_VARIED%=cli_equal%((cpValue$),("varied"))
3816       =17
3817         OPT_TARGET%=cli_grade%((cpValue$))
3818 IF NOT OPT_TARGET% AND NOT cli_equal%((cpValue$),("any")) THEN
3819 cli_error ("Difficulty must be any/easy/medium/hard/expert.")
3820 RETurn 
3821 END IF
3822       =18
3823 cpRange% = ".." INSTR cpValue$
3824 IF cpRange%<2 OR cpRange%+2>LEN(cpValue$) THEN
3825 cli_error ("Ramp must ascend, e.g. easy..hard.")
3826 RETurn 
3827 END IF
3828 cpFirst$ = cpValue$(1 TO cpRange%-1)
3829 cpLast$ = cpValue$(cpRange%+2 TO)
3830 OPT_RAMPFIRST%=cli_grade%((cpFirst$))
3831 OPT_RAMPLAST%=cli_grade%((cpLast$))
3832 IF OPT_RAMPFIRST%<1 OR OPT_RAMPLAST%<=OPT_RAMPFIRST% THEN
3833 cli_error ("Ramp must ascend, e.g. easy..hard.")
3834 RETurn 
3835 END IF
3836       =19
3837 IF LEN(cpValue$)=0 OR LEN(cpValue$)>200 THEN
3838 cli_error ("Output must be a nonempty QL filename of at most 200 bytes.")
3839 RETurn 
3840 END IF
3841         OPT_OUTPUT$=cpValue$
3842       =20
3843         OPT_SHOW%=1
3844       =21
3845         OPT_FORCE%=1
3846       =22
3847         OPT_ASSESS%=1
3848       =23
3849         OPT_UNIQUE%=1
3850       =24
3851         OPT_SHORTEST%=1
3852     END SELect
3853 cpI% = cpI%+1
3854   END REPeat cpParse
3855 IF gExit% THEN
3856 RETurn 
3857 END IF
3858 IF OPT_RAMPFIRST% THEN
3859 cli_ramp
3860   ELSE
3861 cli_validate
3862   END IF
3863 END DEFine
3864 DEFine PROCedure CLI_HELP
3865 PRINT #0,"Tangles native generator / SSB 2.8 and Turbo 5.09"
3866 PRINT #0,"EW win1_generator_exe;";CHR$(34);"--seed 42 --output win1_pack_json";CHR$(34)
3867 PRINT #0,"--mode connect-only|full-coverage   --width N --height N (1..64)"
3868 PRINT #0,"--pairs N --min-length N --max-length N --max-attempts N"
3869 PRINT #0,"--seed N (0..2147483647) --count N (1..10000)"
3870 PRINT #0,"--cut-style random|varied --difficulty any|easy|medium|hard|expert"
3871 PRINT #0,"--ramp easy..hard (ascending grades; default count 12)"
3872 PRINT #0,"--candidates N --improve-steps N --solver-work N"
3873 PRINT #0,"--min-trials N --max-trials N --min-depth N --min-search-nodes N"
3874 PRINT #0,"--assess --require-unique --avoid-shortest --show-solution"
3875 PRINT #0,"--output QL_FILENAME --force --help|-h"
3876 PRINT #0,"Defaults: 8x8, six pairs, connect-only, lengths 4..16, 500 attempts."
3877 PRINT #0,"Orthogonal, disjoint induced paths. Full coverage fills every square."
3878 PRINT #0,"Solver targets are exact, full-coverage only, and require a unique proof."
3879 PRINT #0,"Easy=local, medium=routes, hard=trials, expert=branching."
3880 PRINT #0,"Budgets are deterministic work units, not elapsed time."
3881 PRINT #0,"A failed/budget-limited search never proves impossibility or difficulty."
3882 PRINT #0,"All stored solutions are validated. Grades remain provisional."
3883 PRINT #0,"Escape/Ctrl+C cancels. Native seed streams are versioned; see README."
3884 END DEFine
3885 DIM jsonFirst%(16)
3886 DIM jsonEscaped$(1024)
3887 gJ% = -1
3888 jsonLevel% = 0
3889 DEFine PROCedure JSON_OUT(JOTEXT$)
3890 PRINT #gJ%,joText$;
3891 END DEFine
3892 DEFine FuNction JSON_QUOTE$(JQTEXT$)
3893 LOCal jqI%
3894 LOCal jqC%
3895 LOCal jqOut$(1024)
3896 LOCal jqHex$(16)
3897 jqOut$ = CHR$(34)
3898 jqHex$ = "0123456789ABCDEF"
3899   FOR jqI%=1 TO LEN(jqText$)
3900 jqC% = CODE(jqText$(jqI%))
3901 IF jqC%=34 OR jqC%=92 THEN
3902 jqOut$ = jqOut$ & CHR$(92) & CHR$(jqC%)
3903     ELSE
3904 IF jqC%<32 THEN
3905 jqOut$ = jqOut$ & CHR$(92) & "u00" & jqHex$(1+INT(jqC%/16)) & jqHex$(1+jqC% MOD 16)
3906       ELSE
3907 jqOut$ = jqOut$ & CHR$(jqC%)
3908       END IF
3909     END IF
3910   END FOR jqI%
3911 RETurn jqOut$ & CHR$(34)
3912 END DEFine
3913 DEFine PROCedure JSON_KEY(JKKEY$)
3914 IF jsonLevel%=0 THEN
3915 RETurn 
3916 END IF
3917 IF NOT jsonFirst%(jsonLevel%) THEN
3918 json_out (",")
3919 END IF
3920   jsonFirst%(jsonLevel%)=0
3921 IF LEN(jkKey$) THEN
3922 json_out (json_quote$((jkKey$)) & ":")
3923 END IF
3924 END DEFine
3925 DEFine PROCedure JSON_OBJECT(JAKEY$)
3926 json_key (jaKey$)
3927 json_out ("{")
3928 jsonLevel% = jsonLevel%+1
3929 jsonFirst%(jsonLevel%)=1
3930 END DEFine
3931 DEFine PROCedure JSON_ARRAY(JAKEY$)
3932 json_key (jaKey$)
3933 json_out ("[")
3934 jsonLevel% = jsonLevel%+1
3935 jsonFirst%(jsonLevel%)=1
3936 END DEFine
3937 DEFine PROCedure JSON_END(JECLOSE$)
3938 json_out (jeClose$)
3939 jsonLevel% = jsonLevel%-1
3940 END DEFine
3941 DEFine PROCedure JSON_STRING(JSKEY$,JSTEXT$)
3942 json_key (jsKey$)
3943 json_out (json_quote$((jsText$)))
3944 END DEFine
3945 DEFine PROCedure JSON_INTEGER(JIKEY$,JINUMBER)
3946 json_key (jiKey$)
3947 json_out (gen_int$((jiNumber)))
3948 END DEFine
3949 DEFine PROCedure JSON_NUMBER(JNKEY$,JNNUMBER)
3950 json_key (jnKey$)
3951 json_out (gen_number$((jnNumber)))
3952 END DEFine
3953 DEFine PROCedure JSON_BOOLEAN(JBKEY$,JBVALUE%)
3954 json_key (jbKey$)
3955 json_out (gen_bool$((jbValue%)))
3956 END DEFine
3957 DEFine PROCedure JSON_NULL(JZKEY$)
3958 json_key (jzKey$)
3959 json_out ("null")
3960 END DEFine
3961 DEFine PROCedure JSON_POINT(JPKEY$,JPCELL%)
3962 json_object (jpKey$)
3963 json_integer ("x"),(jpCell% MOD OPT_WIDTH%)
3964 json_integer ("y"),(INT(jpCell%/OPT_WIDTH%))
3965 json_end ("}")
3966 END DEFine
3967 DEFine PROCedure JSON_RECIPE
3968 json_integer ("pairCount"),(OPT_PAIRS%)
3969 json_integer ("minPathLength"),(OPT_MINIMUM%)
3970 json_integer ("maxPathLength"),(gRawMaximum)
3971 json_integer ("maxAttempts"),(OPT_ATTEMPTS%)
3972 json_string ("cutStyle"),(gen_cut$)
3973 END DEFine
3974 DEFine PROCedure JSON_REASONING
3975 json_string ("method"),("cell-domain-reasoning-v1")
3976 json_string ("boardKey"),(SR_BOARDKEY$)
3977 json_string ("mode"),(gen_mode$)
3978 json_string ("status"),(SR_STATUS$)
3979 json_string ("limitReason"),(SR_LIMITREASON$)
3980 json_string ("tier"),(SR_TIER$)
3981 json_string ("grade"),(SR_GRADE$)
3982 json_integer ("score"),(SR_SCORE%)
3983 json_boolean ("provisional"),(1)
3984 json_boolean ("solvedByReasoning"),(SR_LOGICAL%)
3985 json_integer ("localDeductions"),(SR_LOCALDEDUCTIONS)
3986 json_integer ("routeDeductions"),(SR_ROUTEDEDUCTIONS)
3987 json_integer ("routePasses"),(SR_ROUTEPASSES)
3988 json_integer ("trialSteps"),(SR_TRIALSTEPS)
3989 json_integer ("trialAssumptions"),(SR_TRIALASSUMPTIONS)
3990 json_integer ("searchNodes"),(SR_SEARCHNODES)
3991 json_integer ("firstSolutionSearchNodes"),(SR_FIRSTNODES)
3992 json_integer ("firstSolutionBranchDepth"),(SR_FIRSTDEPTH%)
3993 json_integer ("maxSearchDepth"),(SR_MAXDEPTH%)
3994 json_integer ("solutionCount"),(SR_SOLUTIONCOUNT%)
3995 json_integer ("work"),(SR_WORK)
3996 json_integer ("workLimit"),(SR_WORKLIMIT)
3997 json_integer ("solutionCountLimit"),(2)
3998 json_boolean ("solutionCountComplete"),(SR_EXHAUSTIVE%)
3999 json_string ("uniqueness"),(SR_UNIQUENESS$)
4000 END DEFine
4001 DEFine PROCedure JSON_GEOMETRY_COMPONENTS
4002 LOCal jgI%
4003 json_array ("components")
4004   FOR jgI%=0 TO geoCount%-1
4005 json_object ("")
4006 json_string ("name"),(geoNames$(jgI%))
4007 json_number ("value"),(geoValues(jgI%))
4008 json_integer ("weight"),(geoWeights%(jgI%))
4009 json_number ("points"),(geoPoints(jgI%))
4010 json_end ("}")
4011   END FOR jgI%
4012 json_end ("]")
4013 END DEFine
4014 DEFine PROCedure JSON_THRESHOLDS(JTREASON%)
4015 json_object ("thresholds")
4016 IF jtReason% THEN
4017 json_integer ("easyBelow"),(30)
4018 json_integer ("hardAtLeast"),(55)
4019   ELSE
4020 json_integer ("easyBelow"),(geoEasy%)
4021 json_integer ("hardAtLeast"),(geoHard%)
4022   END IF
4023 json_end ("}")
4024 END DEFine
4025 DEFine FuNction GEOMETRY_GRADE$
4026 IF geoScore%<geoEasy% THEN
4027 RETurn "easy"
4028 END IF
4029 IF geoScore%<geoHard% THEN
4030 RETurn "medium"
4031 END IF
4032 RETurn "hard"
4033 END DEFine
4034 DEFine FuNction REASONING_PRIMARY%
4035 RETurn (gHasReason% AND OPT_FULL% AND SR_STATUS$="complete" AND SR_GRADE$<>"unrated")
4036 END DEFine
4037 DEFine PROCedure JSON_DIFFICULTY
4038 LOCal jdI%
4039 LOCal jdP%
4040 LOCal jdBase%
4041 LOCal jdBonus$(32)
4042 json_object ("difficulty")
4043 IF reasoning_primary% THEN
4044 json_string ("method"),("cell-domain-reasoning-v1")
4045 json_string ("profile"),("full-coverage-reasoning-v1")
4046 json_string ("grade"),(SR_GRADE$)
4047 json_integer ("score"),(SR_SCORE%)
4048 json_integer ("rawScore"),(SR_SCORE%)
4049   ELSE
4050 json_string ("method"),("geometric-heuristic-v1")
4051 json_string ("profile"),(gen_mode$ & "-v1")
4052 json_string ("grade"),(geometry_grade$)
4053 json_integer ("score"),(geoScore%)
4054 json_number ("rawScore"),(geoRaw)
4055   END IF
4056 json_string ("mode"),(gen_mode$)
4057 json_boolean ("provisional"),(1)
4058 json_thresholds (reasoning_primary%)
4059 json_object ("measurements")
4060 json_number ("boardCellCount"),(GM_BOARDCELLCOUNT)
4061 json_number ("pairCount"),(GM_PAIRCOUNT)
4062 json_number ("endpointCount"),(GM_ENDPOINTCOUNT)
4063 json_number ("cornerEndpointCount"),(GM_CORNERENDPOINTCOUNT)
4064 json_number ("edgeEndpointCount"),(GM_EDGEENDPOINTCOUNT)
4065 json_number ("meanCornerDistance"),(GM_MEANCORNERDISTANCE)
4066 json_number ("meanEdgeDistance"),(GM_MEANEDGEDISTANCE)
4067 json_number ("meanAvailableExits"),(GM_MEANAVAILABLEEXITS)
4068 json_number ("singleExitEndpointCount"),(GM_SINGLEEXITENDPOINTCOUNT)
4069 json_number ("adjacentPairCount"),(GM_ADJACENTPAIRCOUNT)
4070 json_number ("alignedPairCount"),(GM_ALIGNEDPAIRCOUNT)
4071 json_number ("meanManhattanDistance"),(GM_MEANMANHATTANDISTANCE)
4072 json_number ("meanShortestDistance"),(GM_MEANSHORTESTDISTANCE)
4073 json_number ("minimumRequiredCells"),(GM_MINIMUMREQUIREDCELLS)
4074 json_number ("minimumCoverageRatio"),(GM_MINIMUMCOVERAGERATIO)
4075 json_number ("coverageSlackCells"),(GM_COVERAGESLACKCELLS)
4076 json_number ("requiredExtraCoverageCells"),(GM_REQUIREDEXTRACOVERAGECELLS)
4077 json_number ("forcedCornerTurnCount"),(GM_FORCEDCORNERTURNCOUNT)
4078 json_number ("meanLog2ShortestRouteCount"),(GM_MEANLOG2SHORTESTROUTECOUNT)
4079 json_number ("shortestRouteCountLimit"),(GM_SHORTESTROUTECOUNTLIMIT)
4080 json_number ("cappedRoutePairCount"),(GM_CAPPEDROUTEPAIRCOUNT)
4081 json_number ("routeInteriorOccurrences"),(GM_ROUTEINTERIOROCCURRENCES)
4082 json_number ("sharedRouteInteriorOccurrences"),(GM_SHAREDROUTEINTERIOROCCURRENCES)
4083 json_number ("routeOverlapRatio"),(GM_ROUTEOVERLAPRATIO)
4084 json_number ("maxRouteOverlap"),(GM_MAXROUTEOVERLAP)
4085 json_number ("crossLabelEndpointAdjacencies"),(GM_CROSSLABELENDPOINTADJACENCIES)
4086 json_number ("clueCrowdingRatio"),(GM_CLUECROWDINGRATIO)
4087 json_end ("}")
4088 json_array ("pairMeasurements")
4089   FOR jdP%=1 TO OPT_PAIRS%
4090 json_object ("")
4091 json_string ("label"),(gen_label$((geoMap%(jdP%))))
4092 json_integer ("manhattanDistance"),(GEOPAIR_MANHATTANDISTANCE%(jdP%))
4093 json_integer ("shortestDistance"),(GEOPAIR_SHORTESTDISTANCE%(jdP%))
4094 json_integer ("shortestRouteCount"),(GEOPAIR_SHORTESTROUTECOUNT%(jdP%))
4095 json_boolean ("shortestRouteCountCapped"),(GEOPAIR_SHORTESTROUTECOUNTCAPPED%(jdP%))
4096 json_integer ("minEndpointExits"),(GEOPAIR_MINENDPOINTEXITS%(jdP%))
4097 json_integer ("maxEndpointExits"),(GEOPAIR_MAXENDPOINTEXITS%(jdP%))
4098 json_integer ("shortestRouteInteriorCells"),(GEOPAIR_SHORTESTROUTEINTERIORCELLS%(jdP%))
4099 json_integer ("sharedRouteInteriorCells"),(GEOPAIR_SHAREDROUTEINTERIORCELLS%(jdP%))
4100 json_end ("}")
4101   END FOR jdP%
4102 json_end ("]")
4103 IF reasoning_primary% THEN
4104 jdBase% = 80
4105 jdBonus$ = "searchNodeBonus"
4106     SELect ON SR_TIER$
4107 ="local":
4108 jdBase% = 10
4109 jdBonus$ = "localDeductionBonus"
4110 ="routes":
4111 jdBase% = 30
4112 jdBonus$ = "routeDeductionBonus"
4113 ="trial":
4114 jdBase% = 55
4115 jdBonus$ = "trialStepBonus"
4116     END SELect
4117 json_array ("components")
4118 json_object ("")
4119 json_string ("name"),("reasoningTier")
4120 json_integer ("value"),(1)
4121 json_integer ("weight"),(jdBase%)
4122 json_integer ("points"),(jdBase%)
4123 json_end ("}")
4124 json_object ("")
4125 json_string ("name"),(jdBonus$)
4126 json_integer ("value"),(SR_SCORE%-jdBase%)
4127 json_integer ("weight"),(1)
4128 json_integer ("points"),(SR_SCORE%-jdBase%)
4129 json_end ("}")
4130 json_end ("]")
4131 json_object ("geometricEstimate")
4132 json_string ("method"),("geometric-heuristic-v1")
4133 json_string ("profile"),(gen_mode$ & "-v1")
4134 json_string ("grade"),(geometry_grade$)
4135 json_integer ("score"),(geoScore%)
4136 json_number ("rawScore"),(geoRaw)
4137 json_thresholds (0)
4138 json_geometry_components
4139 json_end ("}")
4140   ELSE
4141 json_geometry_components
4142 json_null ("geometricEstimate")
4143   END IF
4144 IF gHasReason% THEN
4145 json_object ("reasoning")
4146 json_reasoning
4147 json_end ("}")
4148   ELSE
4149 json_null ("reasoning")
4150   END IF
4151 json_end ("}")
4152 END DEFine
4153 DEFine PROCedure JSON_SHORTEST
4154 IF NOT gHasShortest% THEN
4155 json_null ("shortestRoutes")
4156 RETurn 
4157 END IF
4158 json_object ("shortestRoutes")
4159 json_string ("method"),("greedy-shortest-routes-v1")
4160 json_integer ("trials"),(shortTrials%)
4161 json_integer ("trialLimit"),(shortLimit%)
4162 json_boolean ("solved"),(shortSolved%)
4163 IF shortFirst% THEN
4164 json_integer ("firstSuccessTrial"),(shortFirst%)
4165   ELSE
4166 json_null ("firstSuccessTrial")
4167   END IF
4168 json_end ("}")
4169 END DEFine
4170 DEFine PROCedure JSON_POLICY
4171 json_string ("policyVersion"),("symmetry-effort-v1")
4172 json_string ("target"),(gen_grade$((OPT_TARGET%)))
4173 json_integer ("candidateLimit"),(OPT_CANDIDATES%)
4174 json_integer ("improvementLimit"),(OPT_IMPROVE%)
4175 json_integer ("solverWorkLimit"),(OPT_SOLVERWORK)
4176 json_integer ("minTrials"),(OPT_MINTRIALS)
4177 IF OPT_MAXTRIALS<0 THEN
4178 json_null ("maxTrials")
4179   ELSE
4180 json_integer ("maxTrials"),(OPT_MAXTRIALS)
4181   END IF
4182 json_integer ("minBranchDepth"),(OPT_MINDEPTH%)
4183 json_integer ("minFirstSolutionNodes"),(OPT_MINNODES%)
4184 json_boolean ("assess"),(OPT_ASSESS%)
4185 json_boolean ("requireUnique"),(OPT_UNIQUE%)
4186 json_boolean ("avoidShortest"),(OPT_SHORTEST%)
4187 END DEFine
4188 DEFine PROCedure JSON_SELECTION
4189 IF NOT gSelected% THEN
4190 json_null ("selection")
4191 RETurn 
4192 END IF
4193 json_object ("selection")
4194 json_string ("algorithm"),(selAlgorithm$)
4195 json_policy
4196 json_integer ("candidatesConsidered"),(selConsidered%)
4197 json_integer ("candidatesEvaluated"),(selEvaluated%)
4198 json_integer ("traversalEvaluations"),(WORK_TRAVERSALEVALUATIONS)
4199 json_integer ("solverAssessments"),(selAssessments%)
4200 json_integer ("duplicatesRejected"),(selDuplicates%)
4201 json_integer ("budgetRejections"),(selBudgetRejected%)
4202 json_string ("startingTier"),(selStartingTier$)
4203 json_integer ("startingTrialSteps"),(selStartingTrials)
4204 json_end ("}")
4205 END DEFine
4206 DEFine PROCedure JSON_PUZZLE(JPSEED)
4207 LOCal jpP%
4208 LOCal jpI%
4209 LOCal jpJ%
4210 LOCal jpFound%
4211 LOCal jpIdentity$(72)
4212 LOCal jpVerify$(20)
4213 gen_validate
4214 IF NOT gValid% THEN
4215 gExit% = 1
4216 gError$ = "Export refused an invalid solution."
4217 RETurn 
4218 END IF
4219 IF gHasReason% THEN
4220 IF NOT cli_equal%((SR_BOARDKEY$),(identity_board$)) THEN
4221 gExit% = 1
4222 gError$ = "Solver assessment belongs to different clues."
4223 RETurn 
4224 END IF
4225   END IF
4226 IF OPT_TARGET%>0 THEN
4227 IF NOT gHasReason% OR SR_GRADE$<>gen_grade$((OPT_TARGET%)) OR NOT selection_minimums% THEN
4228 gExit% = 1
4229 gError$ = "Selected puzzle did not meet its exact grade and effort bounds."
4230 RETurn 
4231 END IF
4232   END IF
4233 IF OPT_UNIQUE% THEN
4234 IF NOT gHasReason% OR SR_UNIQUENESS$<>"unique" THEN
4235 gExit% = 1
4236 gError$ = "Selected puzzle has no uniqueness proof."
4237 RETurn 
4238 END IF
4239   END IF
4240 geo_assess
4241 IF gExit% OR gCancel% THEN
4242 RETurn 
4243 END IF
4244 jpIdentity$ = identity_puzzle$
4245 jsonLevel% = 0
4246 json_object ("")
4247 json_string ("id"),(jpIdentity$)
4248 json_integer ("width"),(OPT_WIDTH%)
4249 json_integer ("height"),(OPT_HEIGHT%)
4250 json_integer ("seed"),(jpSeed)
4251 json_integer ("attempts"),(WORK_BOARDATTEMPTS+WORK_TRAVERSALEVALUATIONS)
4252 json_object ("work")
4253 json_integer ("boardAttempts"),(WORK_BOARDATTEMPTS)
4254 json_integer ("traversalEvaluations"),(WORK_TRAVERSALEVALUATIONS)
4255 json_integer ("refinementEvaluations"),(WORK_REFINEMENTEVALUATIONS)
4256 json_integer ("structuredStarts"),(WORK_STRUCTUREDSTARTS)
4257 json_end ("}")
4258 IF OPT_RAMPFIRST% THEN
4259 json_object ("recipe")
4260 json_recipe
4261 json_end ("}")
4262   ELSE
4263 json_null ("recipe")
4264   END IF
4265 json_array ("pairs")
4266   FOR jpP%=1 TO OPT_PAIRS%
4267 json_object ("")
4268 json_string ("label"),(gen_label$((jpP%)))
4269 json_point ("start"),(gStart%(jpP%))
4270 json_point ("finish"),(gFinish%(jpP%))
4271 json_end ("}")
4272   END FOR jpP%
4273 json_end ("]")
4274 json_array ("solution")
4275   FOR jpP%=1 TO OPT_PAIRS%
4276 json_object ("")
4277 json_string ("label"),(gen_label$((jpP%)))
4278 json_array ("cells")
4279     FOR jpJ%=1 TO gLength%(jpP%)
4280 jpFound% = -1
4281       FOR jpI%=0 TO gN%-1
4282 IF gOwner%(jpI%)=jpP% AND gOrder%(jpI%)=jpJ% THEN
4283 jpFound% = jpI%
4284 EXIT jpI%
4285 END IF
4286       END FOR jpI%
4287 json_point (""),(jpFound%)
4288     END FOR jpJ%
4289 json_end ("]")
4290 json_end ("}")
4291   END FOR jpP%
4292 json_end ("]")
4293 json_difficulty
4294 json_selection
4295 json_shortest
4296 json_object ("verification")
4297 json_boolean ("solutionValid"),(1)
4298 jpVerify$ = "notChecked"
4299 IF gHasReason% THEN
4300 jpVerify$ = SR_UNIQUENESS$
4301 END IF
4302 json_string ("uniqueness"),(jpVerify$)
4303 jpVerify$ = "heuristicEstimate"
4304 IF reasoning_primary% THEN
4305 jpVerify$ = "reasoningEstimate"
4306 END IF
4307 json_string ("difficulty"),(jpVerify$)
4308 json_end ("}")
4309 json_end ("}")
4310 END DEFine
4311 DIM batchOffset(0)
4312 DIM batchLength(0)
4313 DIM batchConsoleOffset(0)
4314 DIM batchConsoleLength(0)
4315 DIM batchOrder%(0)
4316 DIM batchConsole$(220)
4317 DIM batchBase$(220)
4318 DIM batchCopy$(512)
4319 batchSpool% = -1
4320 batchConsole% = -1
4321 batchRead% = -1
4322 batchFinal% = -1
4323 batchAccepted% = 0
4324 batchRequests = 0
4325 batchRejected = 0
4326 batchDuplicates = 0
4327 batchConsumed = 0
4328 batchBudget = 0
4329 batchRequestLimit = 0
4330 batchBacked% = 0
4331 batchCommitted% = 0
4332 batchSortMemory = 0
4333 batchSortStride = 28
4334 batchDepthSort% = 0
4335 batchUseSelection% = 0
4336 batchTemporary% = 0
4337 batchRestoring% = 0
4338 batchCopyOK% = 0
4339 batchDestTouched% = 0
4340 DEFine FuNction BATCH_VALUE(BVROW%,BVCOLUMN%)
4341 RETurn PEEK_L(batchSortMemory+bvRow%*batchSortStride+4*bvColumn%)
4342 END DEFine
4343 DEFine PROCedure BATCH_METRIC(BMROW%,BMCOLUMN%,BMVALUE)
4344   POKE_L batchSortMemory+bmRow%*batchSortStride+4*bmColumn%,bmValue
4345 END DEFine
4346 DEFine FuNction BATCH_NEW$(BNBASE$)
4347 LOCal bnI%
4348 LOCal bnChannel%
4349 LOCal bnName$(220)
4350   FOR bnI%=0 TO 9999
4351 bnName$ = bnBase$ & "_" & gen_int$((bnI%))
4352 IF FTEST(bnName$)=0 THEN
4353 NEXT bnI%
4354 END IF
4355 bnChannel% = FOP_NEW(bnName$)
4356 IF bnChannel%>=0 THEN
4357 CLOSE #bnChannel%
4358 RETurn bnName$
4359 END IF
4360 IF bnChannel%<>-8 THEN
4361 gExit% = 1
4362 gError$ = "Cannot create temporary file: " & bnBase$
4363 RETurn ""
4364 END IF
4365   END FOR bnI%
4366 gExit% = 1
4367 gError$ = "Cannot reserve a unique temporary filename."
4368 RETurn ""
4369 END DEFine
4370 DEFine PROCedure BATCH_CLOSE
4371 IF batchSpool%>=0 THEN
4372 CLOSE #batchSpool%
4373 batchSpool% = -1
4374 END IF
4375 IF batchConsole%>=0 THEN
4376 CLOSE #batchConsole%
4377 batchConsole% = -1
4378 END IF
4379 IF batchRead%>=0 THEN
4380 CLOSE #batchRead%
4381 batchRead% = -1
4382 END IF
4383 IF batchFinal%>=0 THEN
4384 CLOSE #batchFinal%
4385 batchFinal% = -1
4386 END IF
4387 gJ% = -1
4388 END DEFine
4389 DEFine PROCedure BATCH_CLEANUP
4390 batch_close
4391 IF batchSortMemory THEN
4392 RECHP batchSortMemory
4393 batchSortMemory = 0
4394 END IF
4395 IF sMemory THEN
4396 RECHP sMemory
4397 sMemory = 0
4398 END IF
4399 IF covMemory THEN
4400 RECHP covMemory
4401 covMemory = 0
4402 END IF
4403 IF LEN(gSpool$) THEN
4404 IF FTEST(gSpool$)=0 THEN
4405 DELETE gSpool$
4406 END IF
4407   END IF
4408 IF LEN(batchConsole$) THEN
4409 IF FTEST(batchConsole$)=0 THEN
4410 DELETE batchConsole$
4411 END IF
4412   END IF
4413 IF LEN(gTemp$) THEN
4414 IF FTEST(gTemp$)=0 THEN
4415 DELETE gTemp$
4416 END IF
4417   END IF
4418 IF LEN(gBackup$) AND NOT batchBacked% THEN
4419 IF FTEST(gBackup$)=0 THEN
4420 DELETE gBackup$
4421 END IF
4422   END IF
4423 IF batchCommitted% AND batchBacked% THEN
4424 IF FTEST(gBackup$)=0 THEN
4425 DELETE gBackup$
4426 END IF
4427 batchBacked% = 0
4428   END IF
4429 END DEFine
4430 DEFine PROCedure BATCH_RUNTIME_ERROR(BRERROR)
4431 gExit% = 1
4432 gError$ = "Operating-system error " & gen_int$((brError)) & ". Checking staged output and any previous-file backup."
4433 batch_close
4434 IF batchBacked% AND NOT batchCommitted% THEN
4435 batchRestoring% = 1
4436 batch_clone (gBackup$),(gOutput$)
4437 batchRestoring% = 0
4438 IF batchCopyOK% THEN
4439 batchBacked% = 0
4440 END IF
4441   ELSE
4442 IF batchDestTouched% AND NOT batchCommitted% THEN
4443 IF FTEST(gOutput$)=0 THEN
4444 DELETE gOutput$
4445 END IF
4446     END IF
4447   END IF
4448 PRINT #0,gError$
4449 batch_cleanup
4450 IF COMPILED THEN
4451 RJOB -1,-1
4452 END IF
4453   STOP
4454 END DEFine
4455 DEFine PROCedure BATCH_COPY(BCFROM%,BCTO%,BCAT,BCLENGTH)
4456 LOCal bcI%
4457 LOCal bcCount%
4458 LOCal bcByte$(1)
4459   SET_POSITION #bcFrom%,bcAt
4460   REPeat bcLoop
4461 IF bcLength<=0 THEN
4462 EXIT bcLoop
4463 END IF
4464 IF NOT batchRestoring% AND (gCancel% OR gExit%) THEN
4465 EXIT bcLoop
4466 END IF
4467 bcCount% = gen_min((512),(bcLength))
4468 batchCopy$ = FILL$(" ",bcCount%)
4469     FOR bcI%=1 TO bcCount%
4470 bcByte$ = INKEY$(#bcFrom%,-1)
4471 IF LEN(bcByte$)<>1 THEN
4472 gExit% = 1
4473 gError$ = "Unexpected end of spool file."
4474 RETurn 
4475 END IF
4476       batchCopy$(bcI%)=bcByte$
4477     END FOR bcI%
4478 PRINT #bcTo%,batchCopy$;
4479 bcLength = bcLength-bcCount%
4480 IF NOT batchRestoring% THEN
4481 gen_poll
4482 END IF
4483   END REPeat bcLoop
4484 END DEFine
4485 DEFine PROCedure BATCH_CLONE(BKFROM$,BKTO$)
4486 LOCal bkLength
4487 batchCopyOK% = 0
4488 batchRead% = FOP_IN(bkFrom$)
4489 IF batchRead%<0 THEN
4490 batchRead% = -1
4491 gExit% = 1
4492 gError$ = "Cannot open publication source."
4493 RETurn 
4494 END IF
4495 bkLength = FLEN(#batchRead%)
4496 batchFinal% = FOP_OVER(bkTo$)
4497 IF batchFinal%<0 THEN
4498 CLOSE #batchRead%
4499 batchRead% = -1
4500 batchFinal% = -1
4501 gExit% = 1
4502 gError$ = "Cannot open publication destination."
4503 RETurn 
4504 END IF
4505 batch_copy (batchRead%),(batchFinal%),(0),(bkLength)
4506 IF POSITION(#batchFinal%)=bkLength THEN
4507 batchCopyOK% = 1
4508 END IF
4509 CLOSE #batchRead%
4510 batchRead% = -1
4511 CLOSE #batchFinal%
4512 batchFinal% = -1
4513 END DEFine
4514 DEFine PROCedure BATCH_GRID(BGSOLUTION%)
4515 LOCal bgY%
4516 LOCal bgX%
4517 LOCal bgP%
4518 LOCal bgCell%
4519 LOCal bgWidth%
4520 LOCal bgText$(16)
4521 LOCal bgLine$(512)
4522 bgWidth% = LEN(gen_label$((OPT_PAIRS%)))
4523   FOR bgY%=0 TO OPT_HEIGHT%-1
4524 bgLine$ = ""
4525     FOR bgX%=0 TO OPT_WIDTH%-1
4526 bgCell% = bgY%*OPT_WIDTH%+bgX%
4527 bgP% = gEndpoint%(bgCell%)
4528 IF bgSolution% THEN
4529 bgP% = gOwner%(bgCell%)
4530 END IF
4531 bgText$ = "."
4532 IF bgP% THEN
4533 bgText$ = gen_label$((bgP%))
4534 END IF
4535 IF bgX% THEN
4536 bgLine$ = bgLine$ & " "
4537 END IF
4538 bgLine$ = bgLine$ & FILL$(" ",bgWidth%-LEN(bgText$)) & bgText$
4539     END FOR bgX%
4540 PRINT #batchConsole%,bgLine$
4541   END FOR bgY%
4542 END DEFine
4543 DEFine PROCedure BATCH_CONSOLE(BPSEED)
4544 PRINT #batchConsole%,""
4545 PRINT #batchConsole%,gen_int$((OPT_WIDTH%)) & "x" & gen_int$((OPT_HEIGHT%)) & ", " & gen_int$((OPT_PAIRS%)) & " pairs, seed " & gen_int$((bpSeed))
4546 PRINT #batchConsole%,"Mode: " & gen_mode$
4547 PRINT #batchConsole%,"Work: " & gen_int$((WORK_BOARDATTEMPTS)) & " board attempts; " & gen_int$((WORK_TRAVERSALEVALUATIONS)) & " traversal evaluations."
4548 IF reasoning_primary% THEN
4549 PRINT #batchConsole%,"Difficulty: " & SR_GRADE$ & " (" & gen_int$((SR_SCORE%)) & "/100; reasoning; provisional)"
4550   ELSE
4551 PRINT #batchConsole%,"Difficulty: " & geometry_grade$ & " (" & gen_int$((geoScore%)) & "/100; geometric; provisional)"
4552   END IF
4553 IF gHasReason% THEN
4554 PRINT #batchConsole%,"Solver: " & SR_TIER$ & "; " & gen_int$((SR_TRIALSTEPS)) & " trial eliminations; uniqueness " & SR_UNIQUENESS$
4555 PRINT #batchConsole%,"Assessment: " & SR_STATUS$ & "; " & gen_int$((SR_WORK)) & "/" & gen_int$((SR_WORKLIMIT)) & " work units."
4556   END IF
4557 IF gHasShortest% THEN
4558 PRINT #batchConsole%,"Shortest-route screen: solved=" & gen_bool$((shortSolved%)) & "; " & gen_int$((shortTrials%)) & " trials."
4559 END IF
4560 PRINT #batchConsole%,"Puzzle (. = empty):"
4561 batch_grid (0)
4562 IF OPT_SHOW% THEN
4563 PRINT #batchConsole%,"Known solution (. = unused):"
4564 batch_grid (1)
4565 END IF
4566 END DEFine
4567 DEFine FuNction BATCH_BEFORE%(BBA%,BBB%)
4568 LOCal bbI%
4569   FOR bbI%=0 TO 6
4570 IF batch_value((bbA%),(bbI%))<batch_value((bbB%),(bbI%)) THEN
4571 RETurn 1
4572 END IF
4573 IF batch_value((bbA%),(bbI%))>batch_value((bbB%),(bbI%)) THEN
4574 RETurn 0
4575 END IF
4576   END FOR bbI%
4577 RETurn 0
4578 END DEFine
4579 DEFine PROCedure BATCH_SORT
4580 LOCal bsI%
4581 LOCal bsJ%
4582 LOCal bsAt%
4583   FOR bsI%=1 TO batchAccepted%
4584     batchOrder%(bsI%)=bsI%
4585   END FOR bsI%
4586 IF NOT batchUseSelection% OR NOT OPT_FULL% THEN
4587 RETurn 
4588 END IF
4589   FOR bsI%=2 TO batchAccepted%
4590 bsAt% = batchOrder%(bsI%)
4591 bsJ% = bsI%-1
4592     REPeat bsInsert
4593 IF bsJ%<1 THEN
4594 EXIT bsInsert
4595 END IF
4596 IF NOT batch_before%((bsAt%),(batchOrder%(bsJ%))) THEN
4597 EXIT bsInsert
4598 END IF
4599 batchOrder%(bsJ%+1)=batchOrder%(bsJ%)
4600 bsJ% = bsJ%-1
4601     END REPeat bsInsert
4602     batchOrder%(bsJ%+1)=bsAt%
4603   END FOR bsI%
4604 END DEFine
4605 DEFine PROCedure BATCH_HEADER
4606 LOCal bhI%
4607 LOCal bhSame%
4608 LOCal bhPairs%
4609 LOCal bhMax
4610 LOCal bhAttempts%
4611 LOCal bhAlgorithm$(64)
4612 jsonLevel% = 0
4613 json_object ("")
4614 json_integer ("formatVersion"),(1)
4615 json_object ("rules")
4616 json_boolean ("orthogonalOnly"),(1)
4617 json_boolean ("noSharedCells"),(1)
4618 json_boolean ("noSelfTouching"),(1)
4619 json_boolean ("requireFullCoverage"),(OPT_FULL%)
4620 json_end ("}")
4621 json_object ("generation")
4622 bhAlgorithm$ = selAlgorithm$
4623 IF OPT_RAMPFIRST% THEN
4624 bhAlgorithm$ = "ql-difficulty-ramp-v1"
4625 END IF
4626 json_string ("algorithm"),(bhAlgorithm$)
4627 json_string ("implementation"),("SSB 2.8 / Turbo 5.09")
4628 json_string ("randomAlgorithm"),("ql-subtractive-int31-v1")
4629 json_string ("batchSeedAlgorithm"),("splitmix64-batch-v1")
4630 json_string ("deduplication"),("board-symmetry-v1")
4631 json_string ("mode"),(gen_mode$)
4632 json_integer ("seed"),(OPT_SEED)
4633 json_integer ("width"),(OPT_WIDTH%)
4634 json_integer ("height"),(OPT_HEIGHT%)
4635 json_string ("cutStyle"),(gen_cut$)
4636 json_integer ("minPathLength"),(OPT_MINIMUM%)
4637 IF OPT_RAMPFIRST% THEN
4638 bhPairs% = rampPairs%(OPT_RAMPFIRST%)
4639 bhMax = rampRawMax(OPT_RAMPFIRST%)
4640 bhAttempts% = rampAttempts%(OPT_RAMPFIRST%)
4641     FOR bhI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
4642 IF rampPairs%(bhI%)<>rampPairs%(OPT_RAMPFIRST%) THEN
4643 bhPairs% = -1
4644 END IF
4645 IF rampRawMax(bhI%)<>rampRawMax(OPT_RAMPFIRST%) THEN
4646 bhMax = -1
4647 END IF
4648 IF rampAttempts%(bhI%)<>rampAttempts%(OPT_RAMPFIRST%) THEN
4649 bhAttempts% = -1
4650 END IF
4651     END FOR bhI%
4652 IF bhPairs%<0 THEN
4653 json_null ("pairCount")
4654     ELSE
4655 json_integer ("pairCount"),(bhPairs%)
4656     END IF
4657 IF bhMax<0 THEN
4658 json_null ("maxPathLength")
4659     ELSE
4660 json_integer ("maxPathLength"),(bhMax)
4661     END IF
4662 IF bhAttempts%<0 THEN
4663 json_null ("maxAttempts")
4664     ELSE
4665 json_integer ("maxAttempts"),(bhAttempts%)
4666     END IF
4667 json_string ("ramp"),(gen_grade$((OPT_RAMPFIRST%)) & ".." & gen_grade$((OPT_RAMPLAST%)))
4668 json_array ("rampSections")
4669     FOR bhI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
4670 json_object ("")
4671 json_string ("target"),(gen_grade$((bhI%)))
4672 json_integer ("count"),(rampCounts%(bhI%))
4673 json_object ("recipe")
4674 json_integer ("pairCount"),(rampPairs%(bhI%))
4675 json_integer ("minPathLength"),(OPT_MINIMUM%)
4676 json_integer ("maxPathLength"),(rampRawMax(bhI%))
4677 json_integer ("maxAttempts"),(rampAttempts%(bhI%))
4678 json_string ("cutStyle"),(gen_cut$)
4679 json_end ("}")
4680 json_end ("}")
4681     END FOR bhI%
4682 json_end ("]")
4683 json_null ("selection")
4684   ELSE
4685 json_integer ("pairCount"),(OPT_PAIRS%)
4686 json_integer ("maxPathLength"),(gRawMaximum)
4687 json_integer ("maxAttempts"),(OPT_ATTEMPTS%)
4688 json_null ("ramp")
4689 json_null ("rampSections")
4690 IF gSelected% THEN
4691 json_object ("selection")
4692 json_policy
4693 json_integer ("batchRequests"),(batchRequests)
4694 json_integer ("rejectedSearches"),(batchRejected)
4695 json_integer ("rejectedDuplicates"),(batchDuplicates)
4696 json_integer ("batchCandidateBudget"),(batchBudget)
4697 json_integer ("batchCandidatesConsidered"),(batchConsumed)
4698 json_end ("}")
4699     ELSE
4700 json_null ("selection")
4701     END IF
4702   END IF
4703 json_object ("batch")
4704 json_integer ("requests"),(batchRequests)
4705 json_integer ("rejectedSearches"),(batchRejected)
4706 json_integer ("rejectedDuplicates"),(batchDuplicates)
4707 json_integer ("candidatesConsidered"),(batchConsumed)
4708 IF batchUseSelection% THEN
4709 json_null ("requestLimit")
4710 json_integer ("candidateBudget"),(batchBudget)
4711   ELSE
4712 json_integer ("requestLimit"),(batchRequestLimit)
4713 json_null ("candidateBudget")
4714   END IF
4715 json_end ("}")
4716 json_end ("}")
4717 json_array ("puzzles")
4718 END DEFine
4719 DEFine PROCedure BATCH_PUBLISH
4720 LOCal bfI%
4721 LOCal bfEntry%
4722 batch_sort
4723 IF LEN(OPT_OUTPUT$) THEN
4724 gTemp$ = batch_new$((batchBase$ & "_pack"))
4725 IF gExit% THEN
4726 RETurn 
4727 END IF
4728 batchFinal% = FOP_OVER(gTemp$)
4729 IF batchFinal%<0 THEN
4730 gExit% = 1
4731 gError$ = "Cannot open completed pack staging file."
4732 RETurn 
4733 END IF
4734 gJ% = batchFinal%
4735 batch_header
4736 batchRead% = FOP_IN(gSpool$)
4737 IF batchRead%<0 THEN
4738 gExit% = 1
4739 gError$ = "Cannot reopen puzzle spool."
4740 RETurn 
4741 END IF
4742     FOR bfI%=1 TO batchAccepted%
4743 bfEntry% = batchOrder%(bfI%)
4744 json_key ("")
4745 batch_copy (batchRead%),(batchFinal%),(batchOffset(bfEntry%)),(batchLength(bfEntry%))
4746 IF gExit% OR gCancel% THEN
4747 RETurn 
4748 END IF
4749     END FOR bfI%
4750 CLOSE #batchRead%
4751 batchRead% = -1
4752 json_end ("]")
4753 json_end ("}")
4754 PRINT #batchFinal%
4755 CLOSE #batchFinal%
4756 batchFinal% = -1
4757 gJ% = -1
4758 IF FTEST(gOutput$)=0 THEN
4759 IF NOT OPT_FORCE% THEN
4760 gExit% = 2
4761 gError$ = "Output appeared during generation; --force is required."
4762 RETurn 
4763 END IF
4764 gBackup$ = batch_new$((batchBase$ & "_previous"))
4765 IF gExit% THEN
4766 RETurn 
4767 END IF
4768 batch_clone (gOutput$),(gBackup$)
4769 IF NOT batchCopyOK% THEN
4770 RETurn 
4771 END IF
4772 batchBacked% = 1
4773     END IF
4774 batchDestTouched% = 1
4775 batch_clone (gTemp$),(gOutput$)
4776 IF NOT batchCopyOK% THEN
4777 IF batchBacked% THEN
4778 batchRestoring% = 1
4779 batch_clone (gBackup$),(gOutput$)
4780 batchRestoring% = 0
4781       ELSE
4782 IF FTEST(gOutput$)=0 THEN
4783 DELETE gOutput$
4784 END IF
4785       END IF
4786 gExit% = 1
4787 gError$ = "Publication did not complete; previous output was retained for recovery."
4788 RETurn 
4789     END IF
4790 batchCommitted% = 1
4791 PRINT #0,"Saved " & gen_int$((batchAccepted%)) & " puzzle(s) to " & gOutput$
4792   END IF
4793 batchRead% = FOP_IN(batchConsole$)
4794 IF batchRead%>=0 THEN
4795     FOR bfI%=1 TO batchAccepted%
4796 bfEntry% = batchOrder%(bfI%)
4797 batch_copy (batchRead%),(0),(batchConsoleOffset(bfEntry%)),(batchConsoleLength(bfEntry%))
4798 IF gCancel% OR gExit% THEN
4799 EXIT bfI%
4800 END IF
4801     END FOR bfI%
4802 CLOSE #batchRead%
4803 batchRead% = -1
4804   END IF
4805 PRINT #0,"Known solutions validated. Difficulty estimates are provisional."
4806 END DEFine
4807 DEFine PROCedure BATCH_RUN
4808 LOCal btI%
4809 LOCal btJ%
4810 LOCal btP%
4811 LOCal btDuplicate%
4812 LOCal btSection%
4813 LOCal btSectionCount%
4814 LOCal btSeed
4815 LOCal btBefore
4816 LOCal btCan%
4817 LOCal btUsed
4818 gOutput$ = OPT_OUTPUT$
4819 gSpool$ = ""
4820 gTemp$ = ""
4821 gBackup$ = ""
4822 batchConsole$ = ""
4823 batchSpool% = -1
4824 batchConsole% = -1
4825 batchRead% = -1
4826 batchFinal% = -1
4827 batchBacked% = 0
4828 batchCommitted% = 0
4829 batchDestTouched% = 0
4830 IF LEN(gOutput$) THEN
4831 IF FTEST(gOutput$)=0 AND NOT OPT_FORCE% THEN
4832 gExit% = 2
4833 gError$ = "Output exists; use --force to replace it."
4834 RETurn 
4835 END IF
4836   END IF
4837 batchBase$ = gOutput$
4838 IF LEN(batchBase$)=0 THEN
4839 batchBase$ = DATAD$ & "tangles"
4840 END IF
4841 gSpool$ = batch_new$((batchBase$ & "_spool"))
4842 IF gExit% THEN
4843 RETurn 
4844 END IF
4845 batchConsole$ = batch_new$((batchBase$ & "_console"))
4846 IF gExit% THEN
4847 RETurn 
4848 END IF
4849 batchSpool% = FOP_OVER(gSpool$)
4850 batchConsole% = FOP_OVER(batchConsole$)
4851 IF batchSpool%<0 OR batchConsole%<0 THEN
4852 gExit% = 1
4853 gError$ = "Cannot open batch spool files."
4854 RETurn 
4855 END IF
4856 DIM batchOffset(OPT_COUNT%)
4857 DIM batchLength(OPT_COUNT%)
4858 DIM batchConsoleOffset(OPT_COUNT%)
4859 DIM batchConsoleLength(OPT_COUNT%)
4860 DIM batchOrder%(OPT_COUNT%)
4861 DIM gBatchKeys$(OPT_COUNT%,64)
4862 batchSortMemory = ALCHP((OPT_COUNT%+1)*batchSortStride)
4863 IF NOT batchSortMemory THEN
4864 gExit% = 1
4865 gError$ = "Not enough memory for batch ordering."
4866 RETurn 
4867 END IF
4868 batchAccepted% = 0
4869 batchRequests = 0
4870 batchRejected = 0
4871 batchDuplicates = 0
4872 batchConsumed = 0
4873 batchUseSelection% = (selection_active% OR OPT_RAMPFIRST%>0)
4874 batchRequestLimit = gen_product((10),(OPT_COUNT%))
4875 batchBudget = batchRequestLimit
4876 IF batchUseSelection% THEN
4877 batchBudget = gen_product((gen_product((2),(OPT_COUNT%))),(OPT_CANDIDATES%))
4878 END IF
4879 batchDepthSort% = (OPT_MINDEPTH%>0 OR OPT_MINNODES%>0 OR rampBaseDepth%>0 OR rampBaseNodes%>0)
4880 btSection% = OPT_RAMPFIRST%
4881 btSectionCount% = 0
4882   REPeat btGenerate
4883 IF batchAccepted%>=OPT_COUNT% OR gExit% OR gCancel% THEN
4884 EXIT btGenerate
4885 END IF
4886 btCan% = (batchRequests<batchRequestLimit)
4887 IF batchUseSelection% THEN
4888 btCan% = (batchConsumed+OPT_CANDIDATES%<=batchBudget)
4889 END IF
4890 IF NOT btCan% THEN
4891 EXIT btGenerate
4892 END IF
4893 IF OPT_RAMPFIRST% THEN
4894 cli_section (btSection%)
4895 END IF
4896 btSeed = batch_seed((OPT_SEED),(batchRequests))
4897 batchRequests = batchRequests+1
4898 gen_board
4899 selection_generate (btSeed)
4900 IF gExit% OR gCancel% THEN
4901 EXIT btGenerate
4902 END IF
4903 btUsed = 1
4904 IF batchUseSelection% THEN
4905 btUsed = OPT_CANDIDATES%
4906 END IF
4907 IF gSuccess% THEN
4908 IF batchUseSelection% THEN
4909 btUsed = selConsidered%
4910 END IF
4911 selKey$ = identity_symmetry$
4912 btDuplicate% = 0
4913       FOR btI%=1 TO batchAccepted%
4914 IF cli_equal%((gBatchKeys$(btI%)),(selKey$)) THEN
4915 btDuplicate% = 1
4916 EXIT btI%
4917 END IF
4918       END FOR btI%
4919 IF btDuplicate% THEN
4920 batchDuplicates = batchDuplicates+1
4921       ELSE
4922 batchAccepted% = batchAccepted%+1
4923 gBatchKeys$(batchAccepted%)=selKey$
4924 batchOffset(batchAccepted%)=POSITION(#batchSpool%)
4925 gJ% = batchSpool%
4926 json_puzzle (btSeed)
4927         batchLength(batchAccepted%)=POSITION(#batchSpool%)-batchOffset(batchAccepted%)
4928 IF gExit% OR gCancel% THEN
4929 EXIT btGenerate
4930 END IF
4931 batchConsoleOffset(batchAccepted%)=POSITION(#batchConsole%)
4932 batch_console (btSeed)
4933         batchConsoleLength(batchAccepted%)=POSITION(#batchConsole%)-batchConsoleOffset(batchAccepted%)
4934         FOR btI%=0 TO 6
4935 batch_metric (batchAccepted%),(btI%),(0)
4936         END FOR btI%
4937 IF OPT_RAMPFIRST% THEN
4938 batch_metric (batchAccepted%),(0),(OPT_TARGET%)
4939 END IF
4940 IF gHasReason% THEN
4941 batch_metric (batchAccepted%),(1),(SR_SCORE%)
4942 IF batchDepthSort% THEN
4943 batch_metric (batchAccepted%),(1),(0)
4944 batch_metric (batchAccepted%),(2),(SR_FIRSTDEPTH%)
4945 batch_metric (batchAccepted%),(3),(SR_FIRSTNODES)
4946 END IF
4947 batch_metric (batchAccepted%),(4),(SR_TRIALSTEPS)
4948 batch_metric (batchAccepted%),(5),(SR_ROUTEDEDUCTIONS)
4949         END IF
4950 batch_metric (batchAccepted%),(6),(btSeed)
4951 PRINT #0,"Selected " & gen_int$((batchAccepted%)) & "/" & gen_int$((OPT_COUNT%)) & " (seed " & gen_int$((btSeed)) & ")."
4952 IF OPT_RAMPFIRST% THEN
4953 btSectionCount% = btSectionCount%+1
4954 IF btSectionCount%>=rampCounts%(btSection%) THEN
4955 btSection% = btSection%+1
4956 btSectionCount% = 0
4957 END IF
4958         END IF
4959       END IF
4960     ELSE
4961 IF NOT batchUseSelection% THEN
4962 gExit% = 1
4963 gError$ = "No candidate found within --max-attempts. This does not prove impossibility."
4964 EXIT btGenerate
4965 END IF
4966 batchRejected = batchRejected+1
4967 PRINT #0,"Skipped seed " & gen_int$((btSeed)) & ": no candidate met the bounded selection policy."
4968     END IF
4969 batchConsumed = batchConsumed+btUsed
4970 gen_poll
4971   END REPeat btGenerate
4972 CLOSE #batchSpool%
4973 batchSpool% = -1
4974 CLOSE #batchConsole%
4975 batchConsole% = -1
4976 IF NOT gExit% AND NOT gCancel% THEN
4977 IF batchAccepted%<>OPT_COUNT% THEN
4978 gExit% = 1
4979 gError$ = "Batch budget exhausted. No partial pack was written; no easier puzzle was substituted."
4980     ELSE
4981 batch_publish
4982     END IF
4983   END IF
4984 END DEFine
4985 WHEN_ERROR 0
4986   RJOB -1,-1
4987 END_WHEN
4988 WHEN_ERROR 1
4989 batch_runtime_error (ERNUM%)
4990 END_WHEN
4991 OPEN #0,con
4992 WINDOW #0,512,256,0,0
4993 PAPER #0,0
4994 INK #0,4
4995 CLS #0
4996 gInput% = 0
4997 gCommand$ = ""
4998 IF COMPILED THEN
4999 IF LEN(OPTION_CMD$)>4096 THEN
5000 cli_error ("Option string exceeds 4096 bytes.")
5001 END IF
5002 IF NOT gExit% THEN
5003 gCommand$ = OPTION_CMD$
5004 END IF
5005 ELSE
5006 gCommand$ = CMD$
5007 END IF
5008 IF NOT gExit% THEN
5009 cli_parse (gCommand$)
5010 END IF
5011 IF NOT gExit% THEN
5012 IF cliHelp% THEN
5013 cli_help
5014   ELSE
5015 batch_run
5016   END IF
5017 END IF
5018 batch_cleanup
5019 IF gCancel% THEN
5020 gExit% = 130
5021 END IF
5022 IF gExit% THEN
5023 PRINT #0,"Tangles: " & gError$ & " (exit " & gen_int$((gExit%)) & ")."
5024 END IF
5025 IF COMPILED THEN
5026 RJOB -1,-gExit%
5027 END IF
5028 STOP
