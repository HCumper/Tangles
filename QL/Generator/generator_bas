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
295 IF grText$="easy" THEN
296 RETurn 1
297 END IF
298 IF grText$="medium" THEN
299 RETurn 2
300 END IF
301 IF grText$="hard" THEN
302 RETurn 3
303 END IF
304 IF grText$="expert" THEN
305 RETurn 4
306 END IF
307 RETurn 0
308 END DEFine
309 DEFine FuNction GEN_MODE$
310 IF OPT_FULL% THEN
311 RETurn "full-coverage"
312 END IF
313 RETurn "connect-only"
314 END DEFine
315 DEFine FuNction GEN_CUT$
316 IF OPT_VARIED% THEN
317 RETurn "varied"
318 END IF
319 RETurn "random"
320 END DEFine
321 DEFine FuNction GEN_LESS%(GEA$,GEB$)
322 LOCal geI%
323 LOCal geCount%
324 geCount% = gen_min((LEN(geA$)),(LEN(geB$)))
325   FOR geI%=1 TO geCount%
326 IF CODE(geA$(geI%))<CODE(geB$(geI%)) THEN
327 RETurn 1
328 END IF
329 IF CODE(geA$(geI%))>CODE(geB$(geI%)) THEN
330 RETurn 0
331 END IF
332   END FOR geI%
333 RETurn LEN(geA$)<LEN(geB$)
334 END DEFine
335 DEFine PROCedure GEN_BOARD
336 LOCal gbI%
337 LOCal gbJ%
338 gN% = OPT_WIDTH%*OPT_HEIGHT%
339 DIM gOwner%(gN%-1)
340 DIM gOrder%(gN%-1)
341 DIM gEndpoint%(gN%-1)
342 DIM gLocal%(gN%-1)
343 DIM gNb%(gN%-1,3)
344 DIM gDegree%(gN%-1)
345 DIM gQueue%(gN%-1)
346 DIM gScratch%(gN%-1)
347 DIM gLength%(OPT_PAIRS%)
348 DIM gStart%(OPT_PAIRS%)
349 DIM gFinish%(OPT_PAIRS%)
350 DIM gCanonical%(OPT_PAIRS%)
351 DIM gA%(OPT_PAIRS%)
352 DIM gB%(OPT_PAIRS%)
353 DIM gSymA%(OPT_PAIRS%)
354 DIM gSymB%(OPT_PAIRS%)
355 DIM gBaseA%(OPT_PAIRS%)
356 DIM gBaseB%(OPT_PAIRS%)
357   FOR gbI%=0 TO gN%-1
358 gbJ% = 0
359 IF gbI% MOD OPT_WIDTH%>0 THEN
360 gNb%(gbI%,gbJ%)=gbI%-1
361 gbJ% = gbJ%+1
362 END IF
363 IF gbI% MOD OPT_WIDTH%+1<OPT_WIDTH% THEN
364 gNb%(gbI%,gbJ%)=gbI%+1
365 gbJ% = gbJ%+1
366 END IF
367 IF gbI%>=OPT_WIDTH% THEN
368 gNb%(gbI%,gbJ%)=gbI%-OPT_WIDTH%
369 gbJ% = gbJ%+1
370 END IF
371 IF gbI%+OPT_WIDTH%<gN% THEN
372 gNb%(gbI%,gbJ%)=gbI%+OPT_WIDTH%
373 gbJ% = gbJ%+1
374 END IF
375 gDegree%(gbI%)=gbJ%
376 gOwner%(gbI%)=0
377 gOrder%(gbI%)=0
378 gEndpoint%(gbI%)=0
379   END FOR gbI%
380 END DEFine
381 DEFine FuNction GEN_ADJACENT%(GAA%,GAB%)
382 RETurn ABS((gaA% MOD OPT_WIDTH%)-(gaB% MOD OPT_WIDTH%))+ABS(INT(gaA%/OPT_WIDTH%)-INT(gaB%/OPT_WIDTH%))=1
383 END DEFine
384 DEFine PROCedure GEN_VALIDATE
385 LOCal gvI%
386 LOCal gvP%
387 LOCal gvJ%
388 LOCal gvN%
389 LOCal gvCount%
390 LOCal gvFirst%
391 LOCal gvLast%
392 LOCal gvPrevious%
393 gValid% = 0
394 DIM gvSeen%(gN%-1)
395 gvCount% = 0
396   FOR gvP%=1 TO OPT_PAIRS%
397 IF gLength%(gvP%)<OPT_MINIMUM% OR gLength%(gvP%)>OPT_MAXIMUM% THEN
398 RETurn 
399 END IF
400 gvPrevious% = -1
401 gvFirst% = -1
402 gvLast% = -1
403     FOR gvJ%=1 TO gLength%(gvP%)
404 gvN% = -1
405       FOR gvI%=0 TO gN%-1
406 IF gOwner%(gvI%)=gvP% AND gOrder%(gvI%)=gvJ% THEN
407 IF gvN%>=0 THEN
408 RETurn 
409 END IF
410 gvN% = gvI%
411         END IF
412       END FOR gvI%
413 IF gvN%<0 THEN
414 RETurn 
415 END IF
416 IF gvPrevious%>=0 THEN
417 IF NOT gen_adjacent%((gvPrevious%),(gvN%)) THEN
418 RETurn 
419 END IF
420       END IF
421 IF gvJ%=1 THEN
422 gvFirst% = gvN%
423 END IF
424 gvLast% = gvN%
425 gvPrevious% = gvN%
426 gvSeen%(gvN%)=1
427 gvCount% = gvCount%+1
428     END FOR gvJ%
429 IF gvFirst%<>gStart%(gvP%) OR gvLast%<>gFinish%(gvP%) THEN
430 RETurn 
431 END IF
432   END FOR gvP%
433   FOR gvI%=0 TO gN%-1
434 IF (gOwner%(gvI%)<>0)<>gvSeen%(gvI%) THEN
435 RETurn 
436 END IF
437 IF gOwner%(gvI%) THEN
438       FOR gvJ%=0 TO gDegree%(gvI%)-1
439 gvN% = gNb%(gvI%,gvJ%)
440 IF gOwner%(gvN%)=gOwner%(gvI%) AND ABS(gOrder%(gvN%)-gOrder%(gvI%))<>1 THEN
441 RETurn 
442 END IF
443       END FOR gvJ%
444     END IF
445   END FOR gvI%
446 IF OPT_FULL% AND gvCount%<>gN% THEN
447 RETurn 
448 END IF
449 gValid% = 1
450 END DEFine
451 DEFine PROCedure GEN_ENDPOINTS
452 LOCal geI%
453   FOR geI%=0 TO gN%-1
454     gEndpoint%(geI%)=0
455   END FOR geI%
456   FOR geI%=1 TO OPT_PAIRS%
457 gEndpoint%(gStart%(geI%))=geI%
458 gEndpoint%(gFinish%(geI%))=geI%
459   END FOR geI%
460 END DEFine
461 DIM hashK(63)
462 DIM hashH(7)
463 DIM hashV(7)
464 DIM hashW(63)
465 DIM hashByte%(63)
466 DIM mixByte%(4,7)
467 hashAt% = 0
468 hashBytes = 0
469 FOR mathI%=0 TO 32
470   gPower(mathI%)=2^mathI%
471 END FOR mathI%
472 hashK(0)=1116352408
473 hashK(1)=1899447441
474 hashK(2)=-1245643825
475 hashK(3)=-373957723
476 hashK(4)=961987163
477 hashK(5)=1508970993
478 hashK(6)=-1841331548
479 hashK(7)=-1424204075
480 hashK(8)=-670586216
481 hashK(9)=310598401
482 hashK(10)=607225278
483 hashK(11)=1426881987
484 hashK(12)=1925078388
485 hashK(13)=-2132889090
486 hashK(14)=-1680079193
487 hashK(15)=-1046744716
488 hashK(16)=-459576895
489 hashK(17)=-272742522
490 hashK(18)=264347078
491 hashK(19)=604807628
492 hashK(20)=770255983
493 hashK(21)=1249150122
494 hashK(22)=1555081692
495 hashK(23)=1996064986
496 hashK(24)=-1740746414
497 hashK(25)=-1473132947
498 hashK(26)=-1341970488
499 hashK(27)=-1084653625
500 hashK(28)=-958395405
501 hashK(29)=-710438585
502 hashK(30)=113926993
503 hashK(31)=338241895
504 hashK(32)=666307205
505 hashK(33)=773529912
506 hashK(34)=1294757372
507 hashK(35)=1396182291
508 hashK(36)=1695183700
509 hashK(37)=1986661051
510 hashK(38)=-2117940946
511 hashK(39)=-1838011259
512 hashK(40)=-1564481375
513 hashK(41)=-1474664885
514 hashK(42)=-1035236496
515 hashK(43)=-949202525
516 hashK(44)=-778901479
517 hashK(45)=-694614492
518 hashK(46)=-200395387
519 hashK(47)=275423344
520 hashK(48)=430227734
521 hashK(49)=506948616
522 hashK(50)=659060556
523 hashK(51)=883997877
524 hashK(52)=958139571
525 hashK(53)=1322822218
526 hashK(54)=1537002063
527 hashK(55)=1747873779
528 hashK(56)=1955562222
529 hashK(57)=2024104815
530 hashK(58)=-2067236844
531 hashK(59)=-1933114872
532 hashK(60)=-1866530822
533 hashK(61)=-1538233109
534 hashK(62)=-1090935817
535 hashK(63)=-965641998
536 DEFine PROCedure RNG_SEED(RSSTREAM%,RSSEED)
537 LOCal rsI%
538 LOCal rsJ%
539 LOCal rsK%
540 LOCal rsM
541 LOCal rsN
542 rsM = 161803398-rsSeed
543 IF rsM<0 THEN
544 rsM = rsM+2147483647
545 END IF
546 gRng(rsStream%,55)=rsM
547 rsN = 1
548   FOR rsI%=1 TO 54
549 rsJ% = (21*rsI%) MOD 55
550 gRng(rsStream%,rsJ%)=rsN
551 rsN = rsM-rsN
552 IF rsN<0 THEN
553 rsN = rsN+2147483647
554 END IF
555 rsM = gRng(rsStream%,rsJ%)
556   END FOR rsI%
557   FOR rsK%=1 TO 4
558     FOR rsI%=1 TO 55
559 rsM = gRng(rsStream%,rsI%)-gRng(rsStream%,1+(rsI%+30) MOD 55)
560 IF rsM<0 THEN
561 rsM = rsM+2147483647
562 END IF
563       gRng(rsStream%,rsI%)=rsM
564     END FOR rsI%
565   END FOR rsK%
566 gRi%(rsStream%)=0
567 gRj%(rsStream%)=21
568 END DEFine
569 DEFine FuNction RNG_RAW
570 LOCal rrI%
571 LOCal rrJ%
572 LOCal rrValue
573 rrI% = gRi%(gRngStream%)+1
574 IF rrI%=56 THEN
575 rrI% = 1
576 END IF
577 rrJ% = gRj%(gRngStream%)+1
578 IF rrJ%=56 THEN
579 rrJ% = 1
580 END IF
581 rrValue = gRng(gRngStream%,rrI%)-gRng(gRngStream%,rrJ%)
582 IF rrValue=2147483647 THEN
583 rrValue = rrValue-1
584 END IF
585 IF rrValue<0 THEN
586 rrValue = rrValue+2147483647
587 END IF
588 gRng(gRngStream%,rrI%)=rrValue
589 gRi%(gRngStream%)=rrI%
590 gRj%(gRngStream%)=rrJ%
591 RETurn rrValue
592 END DEFine
593 DEFine FuNction RNG_NEXT%(RNBOUND%)
594 RETurn gen_min((rnBound%-1),(INT((rng_raw/2147483647)*rnBound%)))
595 END DEFine
596 DEFine FuNction RNG_UNIT
597 RETurn rng_raw/2147483647
598 END DEFine
599 DEFine FuNction WORD16%(WIV)
600 LOCal wiWord
601 wiWord = wiV
602 IF wiWord<0 THEN
603 wiWord = wiWord+65536
604 END IF
605 IF wiWord>=32768 THEN
606 wiWord = wiWord-65536
607 END IF
608 RETurn wiWord
609 END DEFine
610 DEFine FuNction XOR32(XBA,XBB)
611 LOCal xbLo
612 LOCal xbHi
613 xbLo = word16%((gen_mod((xbA),(65536)))) ^^ word16%((gen_mod((xbB),(65536))))
614 xbHi = word16%((INT(xbA/65536))) ^^ word16%((INT(xbB/65536)))
615 IF xbLo<0 THEN
616 xbLo = xbLo+65536
617 END IF
618 RETurn xbHi*65536+xbLo
619 END DEFine
620 DEFine FuNction AND32(ABA,ABB)
621 LOCal abLo
622 LOCal abHi
623 abLo = word16%((gen_mod((abA),(65536)))) && word16%((gen_mod((abB),(65536))))
624 abHi = word16%((INT(abA/65536))) && word16%((INT(abB/65536)))
625 IF abLo<0 THEN
626 abLo = abLo+65536
627 END IF
628 RETurn abHi*65536+abLo
629 END DEFine
630 DEFine FuNction ADD32(AAA,AAB)
631 LOCal aaLo
632 LOCal aaHi
633 aaLo = gen_mod((aaA),(65536))+gen_mod((aaB),(65536))
634 aaHi = INT(aaA/65536)+INT(aaB/65536)+INT(aaLo/65536)
635 aaHi = gen_mod((aaHi),(65536))
636 IF aaHi>=32768 THEN
637 aaHi = aaHi-65536
638 END IF
639 RETurn aaHi*65536+gen_mod((aaLo),(65536))
640 END DEFine
641 DEFine FuNction ROTATE32(RBV,RBN%)
642 LOCal rbRight
643 LOCal rbLeft
644 rbRight = INT(rbV/gPower(rbN%))
645 IF rbV<0 THEN
646 rbRight = rbRight+gPower(32-rbN%)
647 END IF
648 rbLeft = gen_mod((rbV),(gPower(rbN%)))
649 IF rbLeft>=gPower(rbN%-1) THEN
650 rbLeft = rbLeft-gPower(rbN%)
651 END IF
652 RETurn rbRight+rbLeft*gPower(32-rbN%)
653 END DEFine
654 DEFine PROCedure HASH_RESET
655 LOCal hrI%
656   hashH(0)=1779033703
657   hashH(1)=-1150833019
658   hashH(2)=1013904242
659   hashH(3)=-1521486534
660   hashH(4)=1359893119
661   hashH(5)=-1694144372
662   hashH(6)=528734635
663   hashH(7)=1541459225
664 hashAt% = 0
665 hashBytes = 0
666 END DEFine
667 DEFine PROCedure HASH_COMPRESS
668 LOCal hcI%
669 LOCal hcJ%
670 LOCal hcS0
671 LOCal hcS1
672 LOCal hcT1
673 LOCal hcT2
674 LOCal hcCh
675 LOCal hcMaj
676 LOCal hcHi
677 LOCal hcLo
678   FOR hcI%=0 TO 15
679 hcJ% = hcI%*4
680 hcHi = hashByte%(hcJ%)
681 hcHi = hcHi*256+hashByte%(hcJ%+1)
682 IF hcHi>=32768 THEN
683 hcHi = hcHi-65536
684 END IF
685 hcLo = hashByte%(hcJ%+2)
686 hcLo = hcLo*256+hashByte%(hcJ%+3)
687     hashW(hcI%)=hcHi*65536+hcLo
688   END FOR hcI%
689   FOR hcI%=16 TO 63
690 hcS0 = xor32((xor32((rotate32((hashW(hcI%-15)),(7))),(rotate32((hashW(hcI%-15)),(18))))),(shift32((hashW(hcI%-15)),(3))))
691 hcS1 = xor32((xor32((rotate32((hashW(hcI%-2)),(17))),(rotate32((hashW(hcI%-2)),(19))))),(shift32((hashW(hcI%-2)),(10))))
692     hashW(hcI%)=add32((add32((hashW(hcI%-16)),(hcS0))),(add32((hashW(hcI%-7)),(hcS1))))
693   END FOR hcI%
694   FOR hcI%=0 TO 7
695     hashV(hcI%)=hashH(hcI%)
696   END FOR hcI%
697   FOR hcI%=0 TO 63
698 hcS1 = xor32((xor32((rotate32((hashV(4)),(6))),(rotate32((hashV(4)),(11))))),(rotate32((hashV(4)),(25))))
699 hcCh = xor32((and32((hashV(4)),(hashV(5)))),(and32((-1-hashV(4)),(hashV(6)))))
700 hcT1 = add32((add32((add32((hashV(7)),(hcS1))),(add32((hcCh),(hashK(hcI%)))))),(hashW(hcI%)))
701 hcS0 = xor32((xor32((rotate32((hashV(0)),(2))),(rotate32((hashV(0)),(13))))),(rotate32((hashV(0)),(22))))
702 hcMaj = xor32((xor32((and32((hashV(0)),(hashV(1)))),(and32((hashV(0)),(hashV(2)))))),(and32((hashV(1)),(hashV(2)))))
703 hcT2 = add32((hcS0),(hcMaj))
704     FOR hcJ%=7 TO 1 STEP -1
705       hashV(hcJ%)=hashV(hcJ%-1)
706     END FOR hcJ%
707 hashV(4)=add32((hashV(4)),(hcT1))
708 hashV(0)=add32((hcT1),(hcT2))
709   END FOR hcI%
710   FOR hcI%=0 TO 7
711     hashH(hcI%)=add32((hashH(hcI%)),(hashV(hcI%)))
712   END FOR hcI%
713 END DEFine
714 DEFine PROCedure HASH_PUT(HPBYTE%)
715 hashByte%(hashAt%)=hpByte%
716 hashAt% = hashAt%+1
717 hashBytes = hashBytes+1
718 IF hashAt%=64 THEN
719 hash_compress
720 hashAt% = 0
721 END IF
722 END DEFine
723 DEFine PROCedure HASH_TEXT(HTTEXT$)
724 LOCal htI%
725   FOR htI%=1 TO LEN(htText$)
726 hash_put (CODE(htText$(htI%)))
727   END FOR htI%
728 END DEFine
729 DEFine FuNction HASH_FINISH$
730 LOCal hfLength
731 LOCal hfI%
732 LOCal hfJ%
733 LOCal hfV
734 LOCal hfHex$(16)
735 LOCal hfOut$(64)
736 hfLength = hashBytes*8
737 hash_put (128)
738   REPeat hfPad
739 IF hashAt%=56 THEN
740 EXIT hfPad
741 END IF
742 hash_put (0)
743   END REPeat hfPad
744   FOR hfI%=1 TO 4
745 hash_put (0)
746   END FOR hfI%
747   FOR hfI%=3 TO 0 STEP -1
748 hash_put (gen_mod((INT(hfLength/(256^hfI%))),(256)))
749   END FOR hfI%
750 hfOut$ = ""
751 hfHex$ = "0123456789ABCDEF"
752   FOR hfI%=0 TO 7
753     FOR hfJ%=7 TO 0 STEP -1
754 hfV = gen_mod((INT(hashH(hfI%)/(16^hfJ%))),(16))
755 hfOut$ = hfOut$ & hfHex$(hfV+1)
756     END FOR hfJ%
757   END FOR hfI%
758 RETurn hfOut$
759 END DEFine
760 DEFine PROCedure MIX_SHIFT(MSBITS%)
761 LOCal msI%
762 LOCal msJ%
763 LOCal msR%
764 LOCal msN%
765 msJ% = INT(msBits%/8)
766 msR% = msBits% MOD 8
767   FOR msI%=0 TO 7
768 msN% = msI%+msJ%
769 mixByte%(1,msI%)=0
770 IF msN%<8 THEN
771 mixByte%(1,msI%)=INT(mixByte%(0,msN%)/gPower(msR%))
772 END IF
773 IF msN%+1<8 AND msR% THEN
774 mixByte%(1,msI%)=mixByte%(1,msI%)+gen_mod((mixByte%(0,msN%+1)),(gPower(msR%)))*gPower(8-msR%)
775 END IF
776   END FOR msI%
777   FOR msI%=0 TO 7
778     mixByte%(0,msI%)=mixByte%(0,msI%) ^^ mixByte%(1,msI%)
779   END FOR msI%
780 END DEFine
781 DEFine PROCedure MIX_CONSTANT(MCHEX$)
782 LOCal mcI%
783 LOCal mcJ%
784 LOCal mcCode%
785 LOCal mcV
786   FOR mcI%=0 TO 7
787 mcV = 0
788     FOR mcJ%=0 TO 1
789 mcCode% = CODE(mcHex$(15-2*mcI%+mcJ%))-48
790 IF mcCode%>9 THEN
791 mcCode% = mcCode%-7
792 END IF
793 mcV = 16*mcV+mcCode%
794     END FOR mcJ%
795     mixByte%(2,mcI%)=mcV
796   END FOR mcI%
797 END DEFine
798 DEFine PROCedure MIX_MULTIPLY
799 LOCal mmI%
800 LOCal mmJ%
801 LOCal mmCarry
802 LOCal mmV
803 mmCarry = 0
804   FOR mmI%=0 TO 7
805 mmV = mmCarry
806     FOR mmJ%=0 TO mmI%
807 mmV = mmV+1.0*mixByte%(0,mmJ%)*mixByte%(2,mmI%-mmJ%)
808     END FOR mmJ%
809 mixByte%(3,mmI%)=gen_mod((mmV),(256))
810 mmCarry = INT(mmV/256)
811   END FOR mmI%
812   FOR mmI%=0 TO 7
813     mixByte%(0,mmI%)=mixByte%(3,mmI%)
814   END FOR mmI%
815 END DEFine
816 DEFine FuNction BATCH_SEED(BSSEED,BSREQUEST)
817 LOCal bsI%
818 LOCal bsCarry
819 LOCal bsV
820 LOCal bsOut
821 IF bsRequest=0 THEN
822 RETurn bsSeed
823 END IF
824   FOR bsI%=0 TO 7
825 mixByte%(0,bsI%)=gen_mod((bsRequest),(256))
826 bsRequest = INT(bsRequest/256)
827   END FOR bsI%
828 mix_constant ("9E3779B97F4A7C15")
829 mix_multiply
830 mix_constant ("54414E474C455342")
831 bsCarry = 0
832   FOR bsI%=0 TO 7
833 bsV = mixByte%(0,bsI%)+mixByte%(2,bsI%)+gen_mod((bsSeed),(256))+bsCarry
834 mixByte%(0,bsI%)=gen_mod((bsV),(256))
835 bsCarry = INT(bsV/256)
836 bsSeed = INT(bsSeed/256)
837   END FOR bsI%
838 mix_shift (30)
839 mix_constant ("BF58476D1CE4E5B9")
840 mix_multiply
841 mix_shift (27)
842 mix_constant ("94D049BB133111EB")
843 mix_multiply
844 mix_shift (31)
845 bsOut = mixByte%(0,3) MOD 128
846   FOR bsI%=2 TO 0 STEP -1
847 bsOut = bsOut*256+mixByte%(0,bsI%)
848   END FOR bsI%
849 RETurn bsOut
850 END DEFine
851 DEFine FuNction SHIFT32(SBV,SBN%)
852 LOCal sbOut
853 sbOut = INT(sbV/gPower(sbN%))
854 IF sbV<0 THEN
855 sbOut = sbOut+gPower(32-sbN%)
856 END IF
857 RETurn sbOut
858 END DEFine
859 DIM covTurn%(0)
860 DIM covBorder%(0)
861 DIM covSaved%(0)
862 covMemory = 0
863 covStride = 0
864 covScore% = 0
865 covFeasible% = 0
866 covFound% = 0
867 growBudget% = 0
868 DEFine PROCedure GROW_ENTER(GDDEPTH%)
869 LOCal gdI%
870 LOCal gdJ%
871 LOCal gdNext%
872 LOCal gdTouches%
873 LOCal gdCurrent%
874 LOCal gdCount%
875 LOCal gdSwap%
876 growBudget% = growBudget%-1
877 gdCurrent% = gGrowthPath%(gdDepth%)
878 gdCount% = 0
879 gGrowthAt%(gdDepth%)=0
880   FOR gdI%=0 TO gDegree%(gdCurrent%)-1
881 gdNext% = gNb%(gdCurrent%,gdI%)
882 IF NOT gOwner%(gdNext%) AND NOT gLocal%(gdNext%) THEN
883 gdTouches% = 0
884       FOR gdJ%=0 TO gDegree%(gdNext%)-1
885 gdTouches% = gdTouches%+gLocal%(gNb%(gdNext%,gdJ%))
886       END FOR gdJ%
887 IF gdTouches%=1 THEN
888 gGrowthCandidates%(gdDepth%,gdCount%)=gdNext%
889 gdCount% = gdCount%+1
890 END IF
891     END IF
892   END FOR gdI%
893   FOR gdI%=gdCount%-1 TO 1 STEP -1
894 gdJ% = rng_next%((gdI%+1))
895 gdSwap% = gGrowthCandidates%(gdDepth%,gdI%)
896 gGrowthCandidates%(gdDepth%,gdI%)=gGrowthCandidates%(gdDepth%,gdJ%)
897 gGrowthCandidates%(gdDepth%,gdJ%)=gdSwap%
898   END FOR gdI%
899   gGrowthCount%(gdDepth%)=gdCount%
900 END DEFine
901 DEFine FuNction GROW_PATH%(GPTARGET%)
902 LOCal gpI%
903 LOCal gpJ%
904 LOCal gpCount%
905 LOCal gpSwap%
906 LOCal gpStart%
907 LOCal gpDepth%
908 LOCal gpNext%
909 LOCal gpOK%
910 gpCount% = 0
911 growBudget% = 2048
912 gpOK% = 0
913   FOR gpI%=0 TO gN%-1
914     gLocal%(gpI%)=0
915 IF NOT gOwner%(gpI%) THEN
916 gScratch%(gpCount%)=gpI%
917 gpCount% = gpCount%+1
918 END IF
919   END FOR gpI%
920   FOR gpI%=gpCount%-1 TO 1 STEP -1
921 gpJ% = rng_next%((gpI%+1))
922 gpSwap% = gScratch%(gpI%)
923 gScratch%(gpI%)=gScratch%(gpJ%)
924 gScratch%(gpJ%)=gpSwap%
925   END FOR gpI%
926   FOR gpStart%=0 TO gpCount%-1
927 gpDepth% = 1
928 gGrowthPath%(1)=gScratch%(gpStart%)
929 gLocal%(gGrowthPath%(1))=1
930 grow_enter (1)
931     REPeat gpExtend
932 gen_poll
933 IF gCancel% THEN
934 EXIT gpExtend
935 END IF
936 IF gpDepth%=gpTarget% THEN
937 gpOK% = 1
938 EXIT gpExtend
939 END IF
940 IF growBudget%<=0 OR gpDepth%=0 THEN
941 EXIT gpExtend
942 END IF
943 gpI% = gGrowthAt%(gpDepth%)
944 IF gpI%>=gGrowthCount%(gpDepth%) THEN
945 gLocal%(gGrowthPath%(gpDepth%))=0
946 gpDepth% = gpDepth%-1
947       ELSE
948 gpNext% = gGrowthCandidates%(gpDepth%,gpI%)
949 gGrowthAt%(gpDepth%)=gpI%+1
950 gpDepth% = gpDepth%+1
951 gGrowthPath%(gpDepth%)=gpNext%
952 gLocal%(gpNext%)=1
953 grow_enter (gpDepth%)
954       END IF
955     END REPeat gpExtend
956     FOR gpI%=0 TO gN%-1
957       gLocal%(gpI%)=0
958     END FOR gpI%
959 IF gpOK% OR growBudget%<=0 OR gCancel% THEN
960 EXIT gpStart%
961 END IF
962   END FOR gpStart%
963 RETurn gpOK%
964 END DEFine
965 DEFine PROCedure GROW_GENERATE(GGSEED)
966 LOCal ggAttempt%
967 LOCal ggP%
968 LOCal ggI%
969 LOCal ggFree%
970 LOCal ggMax%
971 LOCal ggTarget%
972 LOCal ggOK%
973 gSuccess% = 0
974 gRngStream% = 0
975 rng_seed (0),(ggSeed)
976 DIM gGrowthCandidates%(gN%+1,3)
977 DIM gGrowthCount%(gN%+1)
978 DIM gGrowthAt%(gN%+1)
979 DIM gGrowthPath%(gN%+1)
980   FOR ggAttempt%=1 TO OPT_ATTEMPTS%
981 WORK_BOARDATTEMPTS=WORK_BOARDATTEMPTS+1
982 gen_poll
983 IF gCancel% THEN
984 RETurn 
985 END IF
986     FOR ggI%=0 TO gN%-1
987 gOwner%(ggI%)=0
988 gOrder%(ggI%)=0
989     END FOR ggI%
990 ggFree% = gN%
991 ggOK% = 1
992     FOR ggP%=1 TO OPT_PAIRS%
993 ggMax% = gen_min((OPT_MAXIMUM%),(ggFree%-(OPT_PAIRS%-ggP%)*OPT_MINIMUM%))
994 ggTarget% = OPT_MINIMUM%+rng_next%((ggMax%-OPT_MINIMUM%+1))
995 ggOK% = grow_path%((ggTarget%))
996 IF NOT ggOK% AND ggTarget%<>OPT_MINIMUM% THEN
997 ggTarget% = OPT_MINIMUM%
998 ggOK% = grow_path%((ggTarget%))
999 END IF
1000 IF NOT ggOK% OR gCancel% THEN
1001 EXIT ggP%
1002 END IF
1003 gLength%(ggP%)=ggTarget%
1004 gStart%(ggP%)=gGrowthPath%(1)
1005 gFinish%(ggP%)=gGrowthPath%(ggTarget%)
1006       FOR ggI%=1 TO ggTarget%
1007 gOwner%(gGrowthPath%(ggI%))=ggP%
1008 gOrder%(gGrowthPath%(ggI%))=ggI%
1009       END FOR ggI%
1010 ggFree% = ggFree%-ggTarget%
1011     END FOR ggP%
1012 IF ggOK% THEN
1013 gen_endpoints
1014 gen_validate
1015 gSuccess% = gValid%
1016 RETurn 
1017 END IF
1018   END FOR ggAttempt%
1019 END DEFine
1020 DEFine PROCedure COVER_INIT
1021 LOCal ciAt
1022 LOCal ciBytes
1023 DIM gCuts%(OPT_PAIRS%+1)
1024 DIM gEnds%(gN%-1)
1025 DIM gNeeded%(gN%)
1026 DIM gTraversal%(gN%-1)
1027 DIM gPositions%(gN%-1)
1028 DIM gPrefix%(gN%+1)
1029 DIM gWalk%(gN%-1)
1030 DIM gWalkPos%(gN%-1)
1031 DIM gBestWalk%(gN%-1)
1032 DIM covTurn%(gN%)
1033 DIM covBorder%(gN%)
1034 DIM covSaved%(gN%-1)
1035 IF covMemory THEN
1036 RECHP covMemory
1037 END IF
1038 covMemory = 0
1039 covStride = 2*(1+INT(gN%/16))
1040 ciBytes = (OPT_PAIRS%+1)*covStride
1041 covMemory = ALCHP(ciBytes)
1042 IF NOT covMemory THEN
1043 gExit% = 1
1044 gError$ = "Not enough memory for coverage partition DP."
1045 RETurn 
1046 END IF
1047   FOR ciAt=0 TO ciBytes-2 STEP 2
1048     POKE_W covMemory+ciAt,0
1049   END FOR ciAt
1050 END DEFine
1051 DEFine FuNction COVER_DP%(CDP%,CDCELL%)
1052 LOCal cdWord%
1053 LOCal cdBit%
1054 cdWord% = word16%((PEEK_W(covMemory+cdP%*covStride+2*INT(cdCell%/16))))
1055 cdBit% = word16%((gPower(cdCell% MOD 16)))
1056 RETurn (cdWord% && cdBit%)<>0
1057 END DEFine
1058 DEFine PROCedure COVER_SET(CSP%,CSCELL%,CSVALUE%)
1059 LOCal csV
1060 LOCal csBit
1061 LOCal csOld%
1062 csV = PEEK_W(covMemory+csP%*covStride+2*INT(csCell%/16))
1063 IF csV<0 THEN
1064 csV = csV+65536
1065 END IF
1066 csBit = gPower(csCell% MOD 16)
1067 csOld% = ((word16%((csV)) && word16%((csBit)))<>0)
1068 csV = csV+(csValue%-csOld%)*csBit
1069   POKE_W covMemory+csP%*covStride+2*INT(csCell%/16),word16%((csV))
1070 END DEFine
1071 DEFine PROCedure COVER_BACKBITE
1072 LOCal cbFront%
1073 LOCal cbEnd%
1074 LOCal cbAdjacent%
1075 LOCal cbI%
1076 LOCal cbCount%
1077 LOCal cbJoin%
1078 LOCal cbLeft%
1079 LOCal cbRight%
1080 LOCal cbA%
1081 LOCal cbB%
1082 IF gN%<2 THEN
1083 RETurn 
1084 END IF
1085 cbFront% = (rng_next%((2))=0)
1086 IF cbFront% THEN
1087 cbEnd% = gWalk%(0)
1088 cbAdjacent% = gWalk%(1)
1089   ELSE
1090 cbEnd% = gWalk%(gN%-1)
1091 cbAdjacent% = gWalk%(gN%-2)
1092   END IF
1093 cbCount% = 0
1094   FOR cbI%=0 TO gDegree%(cbEnd%)-1
1095 IF gNb%(cbEnd%,cbI%)<>cbAdjacent% THEN
1096 gScratch%(cbCount%)=gNb%(cbEnd%,cbI%)
1097 cbCount% = cbCount%+1
1098 END IF
1099   END FOR cbI%
1100 IF cbCount%=0 THEN
1101 RETurn 
1102 END IF
1103 cbJoin% = gWalkPos%(gScratch%(rng_next%((cbCount%))))
1104 IF cbFront% THEN
1105 cbLeft% = 0
1106 cbRight% = cbJoin%-1
1107   ELSE
1108 cbLeft% = cbJoin%+1
1109 cbRight% = gN%-1
1110   END IF
1111   REPeat cbReverse
1112 IF cbLeft%>=cbRight% THEN
1113 EXIT cbReverse
1114 END IF
1115 cbA% = gWalk%(cbLeft%)
1116 cbB% = gWalk%(cbRight%)
1117 gWalk%(cbLeft%)=cbB%
1118 gWalk%(cbRight%)=cbA%
1119 gWalkPos%(cbA%)=cbRight%
1120 gWalkPos%(cbB%)=cbLeft%
1121 cbLeft% = cbLeft%+1
1122 cbRight% = cbRight%-1
1123   END REPeat cbReverse
1124 END DEFine
1125 DEFine PROCedure COVER_FRESH(CFMIX%)
1126 LOCal cfTranspose%
1127 LOCal cfFlipX%
1128 LOCal cfFlipY%
1129 LOCal cfColumns%
1130 LOCal cfRows%
1131 LOCal cfRow%
1132 LOCal cfCol%
1133 LOCal cfAt%
1134 LOCal cfX%
1135 LOCal cfY%
1136 LOCal cfI%
1137 LOCal cfMoves%
1138 cfTranspose% = (rng_next%((2))=0)
1139 IF NOT cfMix% AND OPT_WIDTH%<>OPT_HEIGHT% THEN
1140 cfTranspose% = (OPT_WIDTH%<OPT_HEIGHT%)
1141 END IF
1142 cfFlipX% = (rng_next%((2))=0)
1143 cfFlipY% = (rng_next%((2))=0)
1144 cfColumns% = OPT_WIDTH%
1145 cfRows% = OPT_HEIGHT%
1146 IF cfTranspose% THEN
1147 cfColumns% = OPT_HEIGHT%
1148 cfRows% = OPT_WIDTH%
1149 END IF
1150 cfAt% = 0
1151   FOR cfRow%=0 TO cfRows%-1
1152     FOR cfI%=0 TO cfColumns%-1
1153 cfCol% = cfI%
1154 IF cfRow% MOD 2 THEN
1155 cfCol% = cfColumns%-1-cfI%
1156 END IF
1157 cfX% = cfCol%
1158 cfY% = cfRow%
1159 IF cfTranspose% THEN
1160 cfX% = cfRow%
1161 cfY% = cfCol%
1162 END IF
1163 IF cfFlipX% THEN
1164 cfX% = OPT_WIDTH%-1-cfX%
1165 END IF
1166 IF cfFlipY% THEN
1167 cfY% = OPT_HEIGHT%-1-cfY%
1168 END IF
1169 gWalk%(cfAt%)=cfY%*OPT_WIDTH%+cfX%
1170 gWalkPos%(gWalk%(cfAt%))=cfAt%
1171 cfAt% = cfAt%+1
1172     END FOR cfI%
1173   END FOR cfRow%
1174 IF NOT cfMix% THEN
1175 WORK_STRUCTUREDSTARTS=WORK_STRUCTUREDSTARTS+1
1176 END IF
1177 IF cfMix% AND OPT_WIDTH%>1 AND OPT_HEIGHT%>1 THEN
1178 cfMoves% = gen_min((16384),(gen_max((256),(gen_product((48),(gN%))))))
1179     FOR cfI%=1 TO cfMoves%
1180 cover_backbite
1181 gen_poll
1182 IF gCancel% THEN
1183 RETurn 
1184 END IF
1185     END FOR cfI%
1186   END IF
1187 END DEFine
1188 DEFine PROCedure COVER_ANALYSE
1189 LOCal caJ%
1190 LOCal caD%
1191 LOCal caBound%
1192 LOCal caCell%
1193 LOCal caNext%
1194 LOCal caStart%
1195 LOCal caFinish%
1196 LOCal caMax%
1197 WORK_TRAVERSALEVALUATIONS=WORK_TRAVERSALEVALUATIONS+1
1198 caBound% = 0
1199 caMax% = gen_min((gN%),(OPT_MAXIMUM%))
1200   FOR caJ%=0 TO gN%-1
1201 caCell% = gWalk%(caJ%)
1202     FOR caD%=0 TO gDegree%(caCell%)-1
1203 caNext% = gWalkPos%(gNb%(caCell%,caD%))
1204 IF caNext%<caJ%-1 THEN
1205 caBound% = gen_max((caBound%),(caNext%+1))
1206 END IF
1207     END FOR caD%
1208     gTraversal%(caJ%)=caBound%
1209   END FOR caJ%
1210 caFinish% = 0
1211   FOR caStart%=0 TO gN%-1
1212 caFinish% = gen_max((caFinish%),(caStart%))
1213     REPeat caExtend
1214 IF caFinish%>=gN% THEN
1215 EXIT caExtend
1216 END IF
1217 IF gTraversal%(caFinish%)>caStart% THEN
1218 EXIT caExtend
1219 END IF
1220 caFinish% = caFinish%+1
1221     END REPeat caExtend
1222 gEnds%(caStart%)=gen_min((caFinish%),(caStart%+caMax%))
1223 gNeeded%(caStart%)=gN%+1
1224   END FOR caStart%
1225   gNeeded%(gN%)=0
1226   FOR caStart%=gN%-1 TO 0 STEP -1
1227     FOR caFinish%=caStart%+OPT_MINIMUM% TO gEnds%(caStart%)
1228       gNeeded%(caStart%)=gen_min((gNeeded%(caStart%)),(1+gNeeded%(caFinish%)))
1229     END FOR caFinish%
1230   END FOR caStart%
1231 covScore% = gNeeded%(0)
1232 END DEFine
1233 DEFine FuNction COVER_WEIGHT(CWSTART%,CWEND%)
1234 LOCal cwLen%
1235 LOCal cwBends%
1236 LOCal cwAverage
1237 LOCal cwBoundary
1238 LOCal cwVariety
1239 LOCal cwW
1240 cwLen% = cwEnd%-cwStart%
1241 cwBends% = 0
1242 IF cwLen%>=3 THEN
1243 cwBends% = covTurn%(cwEnd%-1)-covTurn%(cwStart%+1)
1244 END IF
1245 cwBoundary = covBorder%(cwEnd%)-covBorder%(cwStart%)
1246 cwBoundary = cwBoundary/cwLen%
1247 cwAverage = gN%
1248 cwAverage = cwAverage/OPT_PAIRS%
1249 cwVariety = gen_min((1),(ABS(cwLen%-cwAverage)/cwAverage))
1250 cwW = 1+0.3*gen_min((2),(cwBends%))-0.1*cwBoundary+0.15*cwVariety
1251 IF cwBends%=0 THEN
1252 cwW = cwW-0.2
1253 END IF
1254 RETurn cwW
1255 END DEFine
1256 DEFine PROCedure COVER_PARTITION
1257 LOCal cpP%
1258 LOCal cpI%
1259 LOCal cpFirst%
1260 LOCal cpLast%
1261 LOCal cpStart%
1262 LOCal cpEnd%
1263 LOCal cpMax%
1264 LOCal cpCount%
1265 LOCal cpPick%
1266 LOCal cpJ%
1267 LOCal cpSwap%
1268 LOCal cpReverse%
1269 LOCal cpCell%
1270 LOCal cpTurn%
1271 LOCal cpBorder%
1272 LOCal cpWeight
1273 LOCal cpTarget
1274 LOCal cpSum
1275 covFeasible% = 0
1276 cover_set (0),(gN%),(1)
1277 cpMax% = gen_min((gN%),(OPT_MAXIMUM%))
1278   FOR cpP%=1 TO OPT_PAIRS%
1279     gPrefix%(0)=0
1280     FOR cpI%=0 TO gN%
1281       gPrefix%(cpI%+1)=gPrefix%(cpI%)+cover_dp%((cpP%-1),(cpI%))
1282     END FOR cpI%
1283 cpFirst% = gen_max((0),(gN%-gen_product((cpP%),(cpMax%))))
1284 cpLast% = gN%-cpP%*OPT_MINIMUM%
1285     FOR cpStart%=cpFirst% TO cpLast%
1286 cpI% = cpStart%+OPT_MINIMUM%
1287 cpEnd% = gEnds%(cpStart%)
1288 cover_set (cpP%),(cpStart%),((cpI%<=cpEnd% AND gPrefix%(cpEnd%+1)>gPrefix%(cpI%)))
1289     END FOR cpStart%
1290   END FOR cpP%
1291 IF NOT cover_dp%((OPT_PAIRS%),(0)) THEN
1292 RETurn 
1293 END IF
1294 IF OPT_VARIED% THEN
1295 covTurn%(0)=0
1296 covBorder%(0)=0
1297     FOR cpI%=0 TO gN%-1
1298 cpTurn% = 0
1299 IF cpI%>0 AND cpI%+1<gN% THEN
1300 cpTurn% = (gWalk%(cpI%)-gWalk%(cpI%-1)<>gWalk%(cpI%+1)-gWalk%(cpI%))
1301 END IF
1302 cpCell% = gWalk%(cpI%)
1303 cpBorder% = (cpCell% MOD OPT_WIDTH%=0 OR cpCell% MOD OPT_WIDTH%=OPT_WIDTH%-1 OR cpCell%<OPT_WIDTH% OR cpCell%>=gN%-OPT_WIDTH%)
1304 covTurn%(cpI%+1)=covTurn%(cpI%)+cpTurn%
1305 covBorder%(cpI%+1)=covBorder%(cpI%)+cpBorder%
1306     END FOR cpI%
1307   END IF
1308 cpStart% = 0
1309 gCuts%(1)=0
1310   FOR cpP%=OPT_PAIRS% TO 1 STEP -1
1311 cpCount% = 0
1312 cpSum = 0
1313     FOR cpEnd%=cpStart%+OPT_MINIMUM% TO gEnds%(cpStart%)
1314 IF cover_dp%((cpP%-1),(cpEnd%)) THEN
1315 gScratch%(cpCount%)=cpEnd%
1316 cpCount% = cpCount%+1
1317 IF OPT_VARIED% THEN
1318 cpSum = cpSum+cover_weight((cpStart%),(cpEnd%))
1319 END IF
1320       END IF
1321     END FOR cpEnd%
1322 cpPick% = rng_next%((cpCount%))
1323 IF OPT_VARIED% THEN
1324 cpTarget = rng_unit*cpSum
1325 cpWeight = 0
1326 cpPick% = cpCount%-1
1327       FOR cpI%=0 TO cpCount%-1
1328 cpWeight = cpWeight+cover_weight((cpStart%),(gScratch%(cpI%)))
1329 IF cpWeight>cpTarget THEN
1330 cpPick% = cpI%
1331 EXIT cpI%
1332 END IF
1333       END FOR cpI%
1334     END IF
1335 cpStart% = gScratch%(cpPick%)
1336 gCuts%(OPT_PAIRS%-cpP%+2)=cpStart%
1337   END FOR cpP%
1338   FOR cpP%=1 TO OPT_PAIRS%
1339     gCanonical%(cpP%)=cpP%
1340   END FOR cpP%
1341   FOR cpP%=OPT_PAIRS% TO 2 STEP -1
1342 cpJ% = 1+rng_next%((cpP%))
1343 cpSwap% = gCanonical%(cpP%)
1344 gCanonical%(cpP%)=gCanonical%(cpJ%)
1345 gCanonical%(cpJ%)=cpSwap%
1346   END FOR cpP%
1347   FOR cpI%=0 TO gN%-1
1348 gOwner%(cpI%)=0
1349 gOrder%(cpI%)=0
1350   END FOR cpI%
1351   FOR cpP%=1 TO OPT_PAIRS%
1352 cpJ% = gCanonical%(cpP%)
1353 cpStart% = gCuts%(cpJ%)
1354 cpEnd% = gCuts%(cpJ%+1)
1355 cpReverse% = (rng_next%((2))=0)
1356 gLength%(cpP%)=cpEnd%-cpStart%
1357     FOR cpI%=0 TO gLength%(cpP%)-1
1358 cpCell% = gWalk%(cpStart%+cpI%)
1359 IF cpReverse% THEN
1360 cpCell% = gWalk%(cpEnd%-1-cpI%)
1361 END IF
1362 gOwner%(cpCell%)=cpP%
1363 gOrder%(cpCell%)=cpI%+1
1364 IF cpI%=0 THEN
1365 gStart%(cpP%)=cpCell%
1366 END IF
1367 IF cpI%=gLength%(cpP%)-1 THEN
1368 gFinish%(cpP%)=cpCell%
1369 END IF
1370     END FOR cpI%
1371   END FOR cpP%
1372 gen_endpoints
1373 covFeasible% = 1
1374 END DEFine
1375 DEFine PROCedure COVER_FIND(CFRESTARTS%,CFREFINE%,CFFALLBACK%,CFBURSTS%)
1376 LOCal cfAttempt%
1377 LOCal cfSince%
1378 LOCal cfFirst%
1379 LOCal cfScore%
1380 LOCal cfI%
1381 LOCal cfMoves%
1382 LOCal cfMove%
1383 LOCal cfNext%
1384 LOCal cfAccept%
1385 IF gExit% OR gCancel% THEN
1386 RETurn 
1387 END IF
1388 covFound% = 0
1389 cfFirst% = -1
1390 cfSince% = 0
1391 cfScore% = 32767
1392 cover_fresh ((cfFallback%<>0))
1393   FOR cfI%=0 TO gN%-1
1394     covSaved%(cfI%)=gWalk%(cfI%)
1395   END FOR cfI%
1396   FOR cfAttempt%=0 TO OPT_ATTEMPTS%-1
1397 gen_poll
1398 IF gCancel% THEN
1399 RETurn 
1400 END IF
1401 IF OPT_WIDTH%=1 OR OPT_HEIGHT%=1 THEN
1402 IF cfAttempt%>0 THEN
1403 EXIT cfAttempt%
1404 END IF
1405     END IF
1406 IF cfFirst%>=0 AND cfAttempt%-cfFirst%>cfRefine% THEN
1407 EXIT cfAttempt%
1408 END IF
1409 IF cfFirst%<0 AND cfAttempt%>0 AND (cfSince%=cfRestarts% OR cfAttempt%=cfFallback%) THEN
1410 cover_fresh ((cfAttempt%<>cfFallback%))
1411 cfSince% = 0
1412 cfScore% = 32767
1413       FOR cfI%=0 TO gN%-1
1414         covSaved%(cfI%)=gWalk%(cfI%)
1415       END FOR cfI%
1416     END IF
1417     FOR cfI%=0 TO gN%-1
1418 gWalk%(cfI%)=covSaved%(cfI%)
1419 gWalkPos%(gWalk%(cfI%))=cfI%
1420     END FOR cfI%
1421 IF cfSince%>0 THEN
1422 cfMoves% = 1
1423 IF cfBursts% AND cfSince% MOD 8=0 THEN
1424 cfMoves% = gen_max((2),(INT(gN%/4)))
1425 END IF
1426       FOR cfMove%=1 TO cfMoves%
1427 cover_backbite
1428       END FOR cfMove%
1429     END IF
1430 IF cfFirst%>=0 THEN
1431 WORK_REFINEMENTEVALUATIONS=WORK_REFINEMENTEVALUATIONS+1
1432 END IF
1433 cfSince% = cfSince%+1
1434 cover_analyse
1435 cfNext% = covScore%
1436 IF cfNext%<=OPT_PAIRS% THEN
1437 cover_partition
1438 IF covFeasible% THEN
1439 covFound% = 1
1440 IF cfFirst%<0 THEN
1441 cfFirst% = cfAttempt%
1442 END IF
1443         FOR cfI%=0 TO gN%-1
1444           gBestWalk%(cfI%)=gWalk%(cfI%)
1445         END FOR cfI%
1446       END IF
1447     END IF
1448 cfAccept% = (cfNext%<=cfScore%)
1449 IF NOT cfAccept% THEN
1450 cfAccept% = (rng_unit<EXP((cfScore%-cfNext%)/0.4))
1451 END IF
1452 IF cfAccept% THEN
1453 cfScore% = cfNext%
1454       FOR cfI%=0 TO gN%-1
1455         covSaved%(cfI%)=gWalk%(cfI%)
1456       END FOR cfI%
1457     END IF
1458   END FOR cfAttempt%
1459 END DEFine
1460 DEFine PROCedure IDENTITY_SORT
1461 LOCal isI%
1462 LOCal isJ%
1463 LOCal isA%
1464 LOCal isB%
1465 LOCal isMap%
1466   FOR isI%=2 TO OPT_PAIRS%
1467 isA% = gA%(isI%)
1468 isB% = gB%(isI%)
1469 isMap% = gCanonical%(isI%)
1470 isJ% = isI%-1
1471     REPeat isInsert
1472 IF isJ%<1 THEN
1473 EXIT isInsert
1474 END IF
1475 IF gA%(isJ%)<isA% OR (gA%(isJ%)=isA% AND gB%(isJ%)<=isB%) THEN
1476 EXIT isInsert
1477 END IF
1478 gA%(isJ%+1)=gA%(isJ%)
1479 gB%(isJ%+1)=gB%(isJ%)
1480 gCanonical%(isJ%+1)=gCanonical%(isJ%)
1481 isJ% = isJ%-1
1482     END REPeat isInsert
1483 gA%(isJ%+1)=isA%
1484 gB%(isJ%+1)=isB%
1485 gCanonical%(isJ%+1)=isMap%
1486   END FOR isI%
1487 END DEFine
1488 DEFine PROCedure IDENTITY_CANONICAL
1489 LOCal icP%
1490   FOR icP%=1 TO OPT_PAIRS%
1491 gA%(icP%)=gen_min((gStart%(icP%)),(gFinish%(icP%)))
1492 gB%(icP%)=gen_max((gStart%(icP%)),(gFinish%(icP%)))
1493     gCanonical%(icP%)=icP%
1494   END FOR icP%
1495 identity_sort
1496 END DEFine
1497 DEFine FuNction IDENTITY_PAIRS$
1498 LOCal ipP%
1499 LOCal ipFlag$(5)
1500 ipFlag$ = "False"
1501 IF OPT_FULL% THEN
1502 ipFlag$ = "True"
1503 END IF
1504 hash_reset
1505 hash_text (gen_int$((OPT_WIDTH%)) & "x" & gen_int$((OPT_HEIGHT%)) & ":" & ipFlag$ & ":")
1506   FOR ipP%=1 TO OPT_PAIRS%
1507 IF ipP%>1 THEN
1508 hash_text (";")
1509 END IF
1510 hash_text (gen_int$((gA%(ipP%))) & ":" & gen_int$((gB%(ipP%))))
1511   END FOR ipP%
1512 RETurn hash_finish$
1513 END DEFine
1514 DEFine FuNction IDENTITY_BOARD$
1515 identity_canonical
1516 RETurn identity_pairs$
1517 END DEFine
1518 DEFine FuNction IDENTITY_PUZZLE$
1519 LOCal iiP%
1520 LOCal iiCell%
1521 LOCal iiHash$(64)
1522 LOCal iiOut$(72)
1523 LOCal iiC%
1524 LOCal iiI%
1525 hash_reset
1526 hash_text ("tangles-v1")
1527 IF OPT_FULL% THEN
1528 hash_text (":full-coverage")
1529 END IF
1530 hash_text (":" & gen_int$((OPT_WIDTH%)) & "x" & gen_int$((OPT_HEIGHT%)) & ":")
1531   FOR iiP%=1 TO OPT_PAIRS%
1532 IF iiP%>1 THEN
1533 hash_text (";")
1534 END IF
1535 iiCell% = gStart%(iiP%)
1536 hash_text (gen_label$((iiP%)) & ":" & gen_int$((iiCell% MOD OPT_WIDTH%)) & "," & gen_int$((INT(iiCell%/OPT_WIDTH%))) & ":")
1537 iiCell% = gFinish%(iiP%)
1538 hash_text (gen_int$((iiCell% MOD OPT_WIDTH%)) & "," & gen_int$((INT(iiCell%/OPT_WIDTH%))))
1539   END FOR iiP%
1540 iiHash$ = hash_finish$
1541 iiOut$ = "tangles-"
1542   FOR iiI%=1 TO 64
1543 iiC% = CODE(iiHash$(iiI%))
1544 IF iiC%>=65 THEN
1545 iiC% = iiC%+32
1546 END IF
1547 iiOut$ = iiOut$ & CHR$(iiC%)
1548   END FOR iiI%
1549 RETurn iiOut$
1550 END DEFine
1551 DEFine FuNction IDENTITY_TRANSFORM%(ITCELL%,ITMODE%)
1552 LOCal itX%
1553 LOCal itY%
1554 LOCal itSwap%
1555 itX% = itCell% MOD OPT_WIDTH%
1556 itY% = INT(itCell%/OPT_WIDTH%)
1557 IF itMode%>=4 THEN
1558 itSwap% = itX%
1559 itX% = itY%
1560 itY% = itSwap%
1561 END IF
1562 IF itMode% MOD 2 THEN
1563 itX% = OPT_WIDTH%-1-itX%
1564 END IF
1565 IF INT(itMode%/2) MOD 2 THEN
1566 itY% = OPT_HEIGHT%-1-itY%
1567 END IF
1568 RETurn itY%*OPT_WIDTH%+itX%
1569 END DEFine
1570 DEFine FuNction IDENTITY_SYMMETRY$
1571 LOCal iyMode%
1572 LOCal iyCount%
1573 LOCal iyP%
1574 LOCal iyA%
1575 LOCal iyB%
1576 LOCal iyHash$(64)
1577 LOCal iyBest$(64)
1578 iyCount% = 4
1579 IF OPT_WIDTH%=OPT_HEIGHT% THEN
1580 iyCount% = 8
1581 END IF
1582 iyBest$ = ""
1583   FOR iyMode%=0 TO iyCount%-1
1584     FOR iyP%=1 TO OPT_PAIRS%
1585 iyA% = identity_transform%((gStart%(iyP%)),(iyMode%))
1586 iyB% = identity_transform%((gFinish%(iyP%)),(iyMode%))
1587 gA%(iyP%)=gen_min((iyA%),(iyB%))
1588 gB%(iyP%)=gen_max((iyA%),(iyB%))
1589 gCanonical%(iyP%)=iyP%
1590     END FOR iyP%
1591 identity_sort
1592 iyHash$ = identity_pairs$
1593 IF iyBest$="" OR gen_less%((iyHash$),(iyBest$)) THEN
1594 iyBest$ = iyHash$
1595 END IF
1596   END FOR iyMode%
1597 RETurn iyBest$
1598 END DEFine
1599 DIM sRequired%(0)
1600 DIM sDiscovery%(0)
1601 DIM sLow%(0)
1602 DIM sMandatory%(0)
1603 DIM sA%(0)
1604 DIM sB%(0)
1605 DIM sColourMap%(0)
1606 DIM sPathSeen%(0)
1607 DIM sWord%(63)
1608 DIM sBit%(63)
1609 DIM sAddress(256)
1610 sMemory = 0
1611 solverFrameBytes = 0
1612 sBad% = 0
1613 sLimit% = 0
1614 sColours% = 0
1615 sClock% = 0
1616 sRouteColour% = 0
1617 sSearchLimit = 20000
1618 DEFine FuNction SOLVER_SIGNED%(SWVALUE)
1619 IF swValue>32767 THEN
1620 RETurn swValue-65536
1621 END IF
1622 RETurn swValue
1623 END DEFine
1624 DEFine FuNction SOLVER_WORD%(SWLEVEL%,SWCELL%,SWWORD%)
1625 RETurn solver_signed%((PEEK_W(sAddress(swLevel%)+12*swCell%+2*swWord%)))
1626 END DEFine
1627 DEFine PROCedure SOLVER_PUT(SWLEVEL%,SWCELL%,SWWORD%,SWVALUE%)
1628   POKE_W sAddress(swLevel%)+12*swCell%+2*swWord%,swValue%
1629 END DEFine
1630 DEFine FuNction SOLVER_COUNT%(SWLEVEL%,SWCELL%)
1631 RETurn PEEK_W(sAddress(swLevel%)+12*swCell%+8)
1632 END DEFine
1633 DEFine PROCedure SOLVER_SET_COUNT(SWLEVEL%,SWCELL%,SWCOUNT%)
1634   POKE_W sAddress(swLevel%)+12*swCell%+8,swCount%
1635 END DEFine
1636 DEFine FuNction SOLVER_FIXED%(SWLEVEL%,SWCELL%)
1637 RETurn solver_signed%((PEEK_W(sAddress(swLevel%)+12*swCell%+10)))
1638 END DEFine
1639 DEFine PROCedure SOLVER_SET_FIXED(SWLEVEL%,SWCELL%,SWFIXED%)
1640   POKE_W sAddress(swLevel%)+12*swCell%+10,swFixed%
1641 END DEFine
1642 DEFine PROCedure SOLVER_SPEND
1643 gen_poll
1644 IF sLimit% OR gCancel% THEN
1645 RETurn 
1646 END IF
1647 IF SR_WORK>=SR_WORKLIMIT THEN
1648 sLimit% = 1
1649 SR_STATUS$="budgetExceeded"
1650 SR_LIMITREASON$="work"
1651 RETurn 
1652   END IF
1653   SR_WORK=SR_WORK+1
1654 END DEFine
1655 DEFine FuNction SOLVER_HAS%(SHLEVEL%,SHCELL%,SHCOLOUR%)
1656 RETurn (solver_word%((shLevel%),(shCell%),(sWord%(shColour%))) && sBit%(shColour%))<>0
1657 END DEFine
1658 DEFine FuNction SOLVER_DROP%(SXLEVEL%,SXCELL%,SXCOLOUR%,SXLOCAL%,SXROUTE%)
1659 LOCal sxV
1660 LOCal sxWord%
1661 LOCal sxC%
1662 IF NOT solver_has%((sxLevel%),(sxCell%),(sxColour%)) THEN
1663 RETurn 0
1664 END IF
1665 IF solver_count%((sxLevel%),(sxCell%))=1 THEN
1666 sBad% = 1
1667 RETurn 0
1668 END IF
1669 sxWord% = sWord%(sxColour%)
1670 solver_put (sxLevel%),(sxCell%),(sxWord%),((solver_word%((sxLevel%),(sxCell%),(sxWord%)) ^^ sBit%(sxColour%)))
1671 solver_set_count (sxLevel%),(sxCell%),(solver_count%((sxLevel%),(sxCell%))-1)
1672 IF solver_count%((sxLevel%),(sxCell%))=1 THEN
1673     FOR sxC%=0 TO sColours%-1
1674 IF solver_has%((sxLevel%),(sxCell%),(sxC%)) THEN
1675 solver_set_fixed (sxLevel%),(sxCell%),(sxC%)
1676 EXIT sxC%
1677 END IF
1678     END FOR sxC%
1679   END IF
1680 IF sxLocal% THEN
1681 SR_LOCALDEDUCTIONS=SR_LOCALDEDUCTIONS+1
1682 END IF
1683 IF sxRoute% THEN
1684 SR_ROUTEDEDUCTIONS=SR_ROUTEDEDUCTIONS+1
1685 END IF
1686 RETurn 1
1687 END DEFine
1688 DEFine FuNction SOLVER_FORCE%(SSLEVEL%,SSCELL%,SSCOLOUR%,SSLOCAL%,SSROUTE%)
1689 LOCal ssI%
1690 LOCal ssRemoved%
1691 IF NOT solver_has%((ssLevel%),(ssCell%),(ssColour%)) THEN
1692 sBad% = 1
1693 RETurn 0
1694 END IF
1695 IF solver_count%((ssLevel%),(ssCell%))=1 THEN
1696 RETurn 0
1697 END IF
1698 ssRemoved% = solver_count%((ssLevel%),(ssCell%))-1
1699   FOR ssI%=0 TO 3
1700 solver_put (ssLevel%),(ssCell%),(ssI%),(0)
1701   END FOR ssI%
1702 solver_put (ssLevel%),(ssCell%),(sWord%(ssColour%)),(sBit%(ssColour%))
1703 solver_set_count (ssLevel%),(ssCell%),(1)
1704 solver_set_fixed (ssLevel%),(ssCell%),(ssColour%)
1705 IF ssLocal% THEN
1706 SR_LOCALDEDUCTIONS=SR_LOCALDEDUCTIONS+ssRemoved%
1707 END IF
1708 IF ssRoute% THEN
1709 SR_ROUTEDEDUCTIONS=SR_ROUTEDEDUCTIONS+ssRemoved%
1710 END IF
1711 RETurn 1
1712 END DEFine
1713 DEFine PROCedure SOLVER_COPY(SYFROM%,SYTO%)
1714 LOCal syOffset
1715   FOR syOffset=0 TO solverFrameBytes-2 STEP 2
1716     POKE_W sAddress(syTo%)+syOffset,solver_signed%((PEEK_W(sAddress(syFrom%)+syOffset)))
1717   END FOR syOffset
1718 END DEFine
1719 DEFine PROCedure SOLVER_LOCAL(SLLEVEL%,SLRECORD%)
1720 LOCal slChanged%
1721 LOCal slCell%
1722 LOCal slColour%
1723 LOCal slD%
1724 LOCal slN%
1725 LOCal slPossible%
1726 LOCal slFixed%
1727 LOCal slOK%
1728 LOCal slFirst%
1729 LOCal slLast%
1730   REPeat slAgain
1731 slChanged% = 0
1732     FOR slCell%=0 TO gN%-1
1733 slFirst% = 0
1734 slLast% = OPT_PAIRS%-1
1735 IF solver_fixed%((slLevel%),(slCell%))>=0 THEN
1736 slFirst% = solver_fixed%((slLevel%),(slCell%))
1737 slLast% = gen_min((slFirst%),(OPT_PAIRS%-1))
1738       END IF
1739       FOR slColour%=slFirst% TO slLast%
1740 IF solver_has%((slLevel%),(slCell%),(slColour%)) THEN
1741 solver_spend
1742 IF sLimit% OR gCancel% THEN
1743 RETurn 
1744 END IF
1745 slPossible% = 0
1746 slFixed% = 0
1747           FOR slD%=0 TO gDegree%(slCell%)-1
1748 slN% = gNb%(slCell%,slD%)
1749 IF solver_has%((slLevel%),(slN%),(slColour%)) THEN
1750 slPossible% = slPossible%+1
1751 END IF
1752 IF solver_fixed%((slLevel%),(slN%))=slColour% THEN
1753 slFixed% = slFixed%+1
1754 END IF
1755           END FOR slD%
1756 IF slPossible%<sRequired%(slCell%) OR slFixed%>sRequired%(slCell%) THEN
1757 IF solver_drop%((slLevel%),(slCell%),(slColour%),(slRecord%),(0)) THEN
1758 slChanged% = 1
1759 END IF
1760           ELSE
1761 IF solver_fixed%((slLevel%),(slCell%))=slColour% THEN
1762               FOR slD%=0 TO gDegree%(slCell%)-1
1763 slN% = gNb%(slCell%,slD%)
1764 IF solver_has%((slLevel%),(slN%),(slColour%)) AND solver_fixed%((slLevel%),(slN%))<>slColour% THEN
1765 IF slPossible%=sRequired%(slCell%) THEN
1766 IF solver_force%((slLevel%),(slN%),(slColour%),(slRecord%),(0)) THEN
1767 slChanged% = 1
1768 END IF
1769                   ELSE
1770 IF slFixed%=sRequired%(slCell%) THEN
1771 IF solver_drop%((slLevel%),(slN%),(slColour%),(slRecord%),(0)) THEN
1772 slChanged% = 1
1773 END IF
1774                     END IF
1775                   END IF
1776                 END IF
1777 IF sBad% THEN
1778 RETurn 
1779 END IF
1780               END FOR slD%
1781             END IF
1782           END IF
1783 IF sBad% THEN
1784 RETurn 
1785 END IF
1786         END IF
1787       END FOR slColour%
1788     END FOR slCell%
1789 IF NOT slChanged% THEN
1790 EXIT slAgain
1791 END IF
1792   END REPeat slAgain
1793 END DEFine
1794 DEFine FuNction SOLVER_VISIT%(SVLEVEL%,SVCELL%,SVPARENT%)
1795 LOCal svD%
1796 LOCal svNext%
1797 LOCal svContains%
1798 LOCal svChild%
1799 solver_spend
1800 IF sLimit% OR gCancel% THEN
1801 RETurn 0
1802 END IF
1803 sDiscovery%(svCell%)=sClock%
1804 sLow%(svCell%)=sClock%
1805 sClock% = sClock%+1
1806 svContains% = (svCell%=sB%(sRouteColour%))
1807   FOR svD%=0 TO gDegree%(svCell%)-1
1808 solver_spend
1809 IF sLimit% OR gCancel% THEN
1810 RETurn 0
1811 END IF
1812 svNext% = gNb%(svCell%,svD%)
1813 IF solver_has%((svLevel%),(svNext%),(sRouteColour%)) THEN
1814 IF sDiscovery%(svNext%)<0 THEN
1815 svChild% = solver_visit%((svLevel%),(svNext%),(svCell%))
1816         sLow%(svCell%)=gen_min((sLow%(svCell%)),(sLow%(svNext%)))
1817 IF svChild% AND sLow%(svNext%)>=sDiscovery%(svCell%) THEN
1818 sMandatory%(svCell%)=1
1819 END IF
1820 svContains% = (svContains% OR svChild%)
1821       ELSE
1822 IF svNext%<>svParent% THEN
1823 sLow%(svCell%)=gen_min((sLow%(svCell%)),(sDiscovery%(svNext%)))
1824 END IF
1825       END IF
1826     END IF
1827 IF sLimit% OR gCancel% THEN
1828 RETurn 0
1829 END IF
1830   END FOR svD%
1831 RETurn svContains%
1832 END DEFine
1833 DEFine FuNction SOLVER_ROUTES%(SPLEVEL%,SPRECORD%)
1834 LOCal spColour%
1835 LOCal spCell%
1836 LOCal spChanged%
1837 LOCal spReach%
1838 spChanged% = 0
1839   FOR spColour%=0 TO OPT_PAIRS%-1
1840 solver_spend
1841 IF sLimit% OR gCancel% THEN
1842 RETurn 0
1843 END IF
1844 sRouteColour% = spColour%
1845 sClock% = 0
1846     FOR spCell%=0 TO gN%-1
1847 sDiscovery%(spCell%)=-1
1848 sLow%(spCell%)=0
1849 sMandatory%(spCell%)=0
1850     END FOR spCell%
1851 spReach% = solver_visit%((spLevel%),(sA%(spColour%)),(-1))
1852 IF sLimit% OR gCancel% THEN
1853 RETurn 0
1854 END IF
1855 IF NOT spReach% THEN
1856 sBad% = 1
1857 RETurn 0
1858 END IF
1859     FOR spCell%=0 TO gN%-1
1860 IF solver_has%((spLevel%),(spCell%),(spColour%)) THEN
1861 IF sDiscovery%(spCell%)<0 THEN
1862 IF solver_drop%((spLevel%),(spCell%),(spColour%),(0),(spRecord%)) THEN
1863 spChanged% = 1
1864 END IF
1865         ELSE
1866 IF sMandatory%(spCell%) THEN
1867 IF solver_force%((spLevel%),(spCell%),(spColour%),(0),(spRecord%)) THEN
1868 spChanged% = 1
1869 END IF
1870           END IF
1871         END IF
1872       END IF
1873 IF sBad% THEN
1874 RETurn 0
1875 END IF
1876     END FOR spCell%
1877   END FOR spColour%
1878 IF spChanged% AND spRecord% THEN
1879 SR_ROUTEPASSES=SR_ROUTEPASSES+1
1880 END IF
1881 RETurn spChanged%
1882 END DEFine
1883 DEFine PROCedure SOLVER_PROPAGATE(SULEVEL%)
1884 LOCal suChanged%
1885   REPeat suAgain
1886 solver_local (suLevel%),(0)
1887 IF sBad% OR sLimit% OR gCancel% THEN
1888 RETurn 
1889 END IF
1890 suChanged% = solver_routes%((suLevel%),(0))
1891 IF NOT suChanged% OR sBad% OR sLimit% OR gCancel% THEN
1892 EXIT suAgain
1893 END IF
1894   END REPeat suAgain
1895 END DEFine
1896 DEFine FuNction SOLVER_MATERIALISE%(SMLEVEL%)
1897 LOCal smC%
1898 LOCal smI%
1899 LOCal smAt%
1900 LOCal smPrevious%
1901 LOCal smNext%
1902 LOCal smFound%
1903 LOCal smLength%
1904 LOCal smTotal%
1905 LOCal smD%
1906   FOR smC%=0 TO OPT_PAIRS%-1
1907 smTotal% = 0
1908     FOR smI%=0 TO gN%-1
1909 IF solver_fixed%((smLevel%),(smI%))=smC% THEN
1910 smTotal% = smTotal%+1
1911 END IF
1912     END FOR smI%
1913 smAt% = sA%(smC%)
1914 smPrevious% = -1
1915 smLength% = 0
1916     REPeat smWalk
1917 solver_spend
1918 IF sLimit% OR gCancel% THEN
1919 RETurn 0
1920 END IF
1921 smLength% = smLength%+1
1922 IF smAt%=sB%(smC%) THEN
1923 EXIT smWalk
1924 END IF
1925 smFound% = 0
1926 smNext% = -1
1927       FOR smD%=0 TO gDegree%(smAt%)-1
1928 smI% = gNb%(smAt%,smD%)
1929 IF smI%<>smPrevious% AND solver_fixed%((smLevel%),(smI%))=smC% THEN
1930 smFound% = smFound%+1
1931 smNext% = smI%
1932 END IF
1933       END FOR smD%
1934 IF smFound%<>1 OR smLength%>gN% THEN
1935 sBad% = 1
1936 RETurn 0
1937 END IF
1938 smPrevious% = smAt%
1939 smAt% = smNext%
1940     END REPeat smWalk
1941 IF smLength%<>smTotal% THEN
1942 sBad% = 1
1943 RETurn 0
1944 END IF
1945   END FOR smC%
1946 RETurn 1
1947 END DEFine
1948 DEFine FuNction SOLVER_SEARCH%(SELEVEL%,SEDEPTH%)
1949 LOCal seBest%
1950 LOCal seSize%
1951 LOCal seI%
1952 LOCal seC%
1953 LOCal seComplete%
1954 LOCal seChild%
1955 LOCal seOK%
1956 IF SR_SOLUTIONCOUNT%>=2 THEN
1957 RETurn 0
1958 END IF
1959 solver_spend
1960 IF sLimit% OR gCancel% THEN
1961 RETurn 0
1962 END IF
1963 IF SR_SEARCHNODES>=sSearchLimit THEN
1964 sLimit% = 1
1965 SR_STATUS$="budgetExceeded"
1966 SR_LIMITREASON$="searchNodes"
1967 RETurn 0
1968 END IF
1969 SR_SEARCHNODES=SR_SEARCHNODES+1
1970 SR_MAXDEPTH%=gen_max((SR_MAXDEPTH%),(seDepth%))
1971 sBad% = 0
1972 solver_propagate (seLevel%)
1973 IF sLimit% OR gCancel% THEN
1974 RETurn 0
1975 END IF
1976 IF sBad% THEN
1977 sBad% = 0
1978 RETurn 1
1979 END IF
1980 seBest% = -1
1981 seSize% = 65
1982   FOR seI%=0 TO gN%-1
1983 IF solver_count%((seLevel%),(seI%))>1 AND solver_count%((seLevel%),(seI%))<seSize% THEN
1984 seBest% = seI%
1985 seSize% = solver_count%((seLevel%),(seI%))
1986 END IF
1987   END FOR seI%
1988 IF seBest%<0 THEN
1989 seOK% = solver_materialise%((seLevel%))
1990 IF sLimit% OR gCancel% THEN
1991 RETurn 0
1992 END IF
1993 IF sBad% THEN
1994 sBad% = 0
1995 RETurn 1
1996 END IF
1997 IF SR_SOLUTIONCOUNT%=0 THEN
1998 SR_FIRSTNODES=SR_SEARCHNODES
1999 SR_FIRSTDEPTH%=seDepth%
2000 END IF
2001 SR_SOLUTIONCOUNT%=SR_SOLUTIONCOUNT%+1
2002 RETurn 1
2003   END IF
2004 seComplete% = 1
2005   FOR seC%=0 TO sColours%-1
2006 IF solver_has%((seLevel%),(seBest%),(seC%)) THEN
2007 IF SR_SOLUTIONCOUNT%>=2 THEN
2008 RETurn 0
2009 END IF
2010 solver_copy (seLevel%),(seLevel%+1)
2011 seOK% = solver_force%((seLevel%+1),(seBest%),(seC%),(0),(0))
2012 seChild% = solver_search%((seLevel%+1),(seDepth%+1))
2013 seComplete% = (seComplete% AND seChild%)
2014 IF sLimit% OR gCancel% THEN
2015 RETurn 0
2016 END IF
2017     END IF
2018   END FOR seC%
2019 RETurn seComplete%
2020 END DEFine
2021 DEFine PROCedure SOLVER_ANALYSE(SABUDGET,SASEARCHLIMIT)
2022 LOCal saI%
2023 LOCal saJ%
2024 LOCal saC%
2025 LOCal saWord%
2026 LOCal saOK%
2027 LOCal saSingle%
2028 LOCal saRoutes%
2029 LOCal saEliminated%
2030 LOCal saGrading%
2031 DIM SR_STATUS$(20)
2032 SR_STATUS$ = ""
2033 DIM SR_LIMITREASON$(128)
2034 SR_LIMITREASON$ = ""
2035 DIM SR_TIER$(20)
2036 SR_TIER$ = ""
2037 DIM SR_GRADE$(12)
2038 SR_GRADE$ = ""
2039 SR_SCORE% = 0
2040 SR_LOGICAL% = 0
2041 SR_LOCALDEDUCTIONS = 0
2042 SR_ROUTEDEDUCTIONS = 0
2043 SR_ROUTEPASSES = 0
2044 SR_TRIALSTEPS = 0
2045 SR_TRIALASSUMPTIONS = 0
2046 SR_SEARCHNODES = 0
2047 SR_FIRSTNODES = 0
2048 SR_FIRSTDEPTH% = 0
2049 SR_MAXDEPTH% = 0
2050 SR_SOLUTIONCOUNT% = 0
2051 SR_EXHAUSTIVE% = 0
2052 DIM SR_UNIQUENESS$(20)
2053 SR_UNIQUENESS$ = ""
2054 SR_WORK = 0
2055 SR_WORKLIMIT = 0
2056 DIM SR_BOARDKEY$(64)
2057 SR_BOARDKEY$ = ""
2058 SR_STATUS$="complete"
2059 SR_TIER$="local"
2060 SR_GRADE$="unrated"
2061 SR_WORKLIMIT=saBudget
2062 SR_UNIQUENESS$="notEstablished"
2063 SR_BOARDKEY$=identity_board$
2064 sBad% = 0
2065 sLimit% = 0
2066 sSearchLimit = saSearchLimit
2067 gHasReason% = 1
2068 IF OPT_WIDTH%>16 OR OPT_HEIGHT%>16 OR OPT_PAIRS%>64-(NOT OPT_FULL%) THEN
2069 SR_STATUS$="unsupported"
2070 SR_TIER$="unknown"
2071 SR_LIMITREASON$="Solver supports dimensions up to 16x16 and at most 64 full-coverage or 63 connect-only pairs."
2072 RETurn 
2073   END IF
2074 sColours% = OPT_PAIRS%+(NOT OPT_FULL%)
2075   FOR saC%=0 TO 63
2076 sWord%(saC%)=INT(saC%/16)
2077 sBit%(saC%)=word16%((gPower(saC% MOD 16)))
2078   END FOR saC%
2079 IF sMemory THEN
2080 RECHP sMemory
2081 END IF
2082 sMemory = 0
2083 solverFrameBytes = 12*gN%
2084 sMemory = ALCHP((gN%+1)*solverFrameBytes)
2085 IF NOT sMemory THEN
2086 gExit% = 1
2087 gError$ = "Not enough memory for solver frames."
2088 RETurn 
2089 END IF
2090   FOR saI%=0 TO gN%
2091     sAddress(saI%)=sMemory+saI%*solverFrameBytes
2092   END FOR saI%
2093 DIM sRequired%(gN%-1)
2094 DIM sDiscovery%(gN%-1)
2095 DIM sLow%(gN%-1)
2096 DIM sMandatory%(gN%-1)
2097 DIM sA%(OPT_PAIRS%-1)
2098 DIM sB%(OPT_PAIRS%-1)
2099 DIM sColourMap%(OPT_PAIRS%-1)
2100   FOR saC%=0 TO OPT_PAIRS%-1
2101 sA%(saC%)=gA%(saC%+1)
2102 sB%(saC%)=gB%(saC%+1)
2103 sColourMap%(saC%)=gCanonical%(saC%+1)
2104   END FOR saC%
2105   FOR saI%=0 TO gN%-1
2106 solver_set_count (0),(saI%),(sColours%)
2107 solver_set_fixed (0),(saI%),(-1)
2108 sRequired%(saI%)=2
2109 IF sColours%=1 THEN
2110 solver_set_fixed (0),(saI%),(0)
2111 END IF
2112     FOR saWord%=0 TO 3
2113 saJ% = gen_max((0),(gen_min((16),(sColours%-16*saWord%))))
2114 solver_put (0),(saI%),(saWord%),(word16%((gPower(saJ%)-1)))
2115     END FOR saWord%
2116   END FOR saI%
2117   FOR saC%=0 TO OPT_PAIRS%-1
2118 saOK% = solver_force%((0),(sA%(saC%)),(saC%),(0),(0))
2119 saOK% = solver_force%((0),(sB%(saC%)),(saC%),(0),(0))
2120 sRequired%(sA%(saC%))=1
2121 sRequired%(sB%(saC%))=1
2122   END FOR saC%
2123 saGrading% = 1
2124   REPeat saGrade
2125 solver_local (0),(1)
2126 IF sBad% OR sLimit% OR gCancel% THEN
2127 EXIT saGrade
2128 END IF
2129 saSingle% = 1
2130     FOR saI%=0 TO gN%-1
2131 IF solver_count%((0),(saI%))<>1 THEN
2132 saSingle% = 0
2133 EXIT saI%
2134 END IF
2135     END FOR saI%
2136 IF saSingle% THEN
2137 saOK% = solver_materialise%((0))
2138 IF NOT sBad% AND NOT sLimit% AND NOT gCancel% THEN
2139 SR_SOLUTIONCOUNT%=1
2140 SR_LOGICAL%=1
2141 SR_EXHAUSTIVE%=1
2142 END IF
2143       EXIT saGrade
2144     END IF
2145 saRoutes% = solver_routes%((0),(1))
2146 IF sBad% OR sLimit% OR gCancel% THEN
2147 EXIT saGrade
2148 END IF
2149 IF saRoutes% THEN
2150 IF SR_TIER$="local" THEN
2151 SR_TIER$="routes"
2152 END IF
2153     ELSE
2154 saEliminated% = 0
2155       FOR saI%=0 TO gN%-1
2156 IF solver_count%((0),(saI%))>1 THEN
2157           FOR saC%=0 TO sColours%-1
2158 IF solver_has%((0),(saI%),(saC%)) THEN
2159 SR_TRIALASSUMPTIONS=SR_TRIALASSUMPTIONS+1
2160 solver_copy (0),(1)
2161 saOK% = solver_force%((1),(saI%),(saC%),(0),(0))
2162 solver_propagate (1)
2163 IF sLimit% OR gCancel% THEN
2164 EXIT saC%
2165 END IF
2166 IF sBad% THEN
2167 sBad% = 0
2168 saOK% = solver_drop%((0),(saI%),(saC%),(0),(0))
2169 SR_TRIALSTEPS=SR_TRIALSTEPS+1
2170 SR_TIER$="trial"
2171 saEliminated% = 1
2172 EXIT saC%
2173               END IF
2174             END IF
2175           END FOR saC%
2176         END IF
2177 IF saEliminated% OR sBad% OR sLimit% OR gCancel% THEN
2178 EXIT saI%
2179 END IF
2180       END FOR saI%
2181 IF sBad% OR sLimit% OR gCancel% THEN
2182 EXIT saGrade
2183 END IF
2184 IF NOT saEliminated% THEN
2185 SR_TIER$="search"
2186 EXIT saGrade
2187 END IF
2188     END IF
2189   END REPeat saGrade
2190 IF sBad% THEN
2191 SR_TIER$="unsatisfiable"
2192 SR_EXHAUSTIVE%=1
2193 sBad% = 0
2194   ELSE
2195 IF NOT SR_LOGICAL% AND NOT sLimit% AND NOT gCancel% THEN
2196       SR_EXHAUSTIVE%=solver_search%((0),(0))
2197 IF SR_EXHAUSTIVE% AND SR_SOLUTIONCOUNT%=0 THEN
2198 SR_TIER$="unsatisfiable"
2199 END IF
2200     END IF
2201   END IF
2202 IF SR_SOLUTIONCOUNT%>=2 THEN
2203     SR_UNIQUENESS$="multiple"
2204   ELSE
2205 IF SR_EXHAUSTIVE% THEN
2206 SR_UNIQUENESS$="none"
2207 IF SR_SOLUTIONCOUNT%=1 THEN
2208 SR_UNIQUENESS$="unique"
2209 END IF
2210     END IF
2211   END IF
2212 IF OPT_FULL% AND SR_STATUS$="complete" AND SR_UNIQUENESS$="unique" THEN
2213 SR_GRADE$="expert"
2214 SR_SCORE%=80+gen_min((20),(INT(2*gen_log2((SR_SEARCHNODES+1)))))
2215 IF SR_TIER$="local" THEN
2216 SR_GRADE$="easy"
2217 SR_SCORE%=10+gen_min((19),(INT(SR_LOCALDEDUCTIONS/gN%)))
2218 END IF
2219 IF SR_TIER$="routes" THEN
2220 SR_GRADE$="medium"
2221 SR_SCORE%=30+gen_min((24),(SR_ROUTEDEDUCTIONS))
2222 END IF
2223 IF SR_TIER$="trial" THEN
2224 SR_GRADE$="hard"
2225 SR_SCORE%=55+gen_min((24),(SR_TRIALSTEPS))
2226 END IF
2227   END IF
2228 END DEFine
2229 GM_BOARDCELLCOUNT = 0
2230 GM_PAIRCOUNT = 0
2231 GM_ENDPOINTCOUNT = 0
2232 GM_CORNERENDPOINTCOUNT = 0
2233 GM_EDGEENDPOINTCOUNT = 0
2234 GM_MEANCORNERDISTANCE = 0
2235 GM_MEANEDGEDISTANCE = 0
2236 GM_MEANAVAILABLEEXITS = 0
2237 GM_SINGLEEXITENDPOINTCOUNT = 0
2238 GM_ADJACENTPAIRCOUNT = 0
2239 GM_ALIGNEDPAIRCOUNT = 0
2240 GM_MEANMANHATTANDISTANCE = 0
2241 GM_MEANSHORTESTDISTANCE = 0
2242 GM_MINIMUMREQUIREDCELLS = 0
2243 GM_MINIMUMCOVERAGERATIO = 0
2244 GM_COVERAGESLACKCELLS = 0
2245 GM_REQUIREDEXTRACOVERAGECELLS = 0
2246 GM_FORCEDCORNERTURNCOUNT = 0
2247 GM_MEANLOG2SHORTESTROUTECOUNT = 0
2248 GM_SHORTESTROUTECOUNTLIMIT = 0
2249 GM_CAPPEDROUTEPAIRCOUNT = 0
2250 GM_ROUTEINTERIOROCCURRENCES = 0
2251 GM_SHAREDROUTEINTERIOROCCURRENCES = 0
2252 GM_ROUTEOVERLAPRATIO = 0
2253 GM_MAXROUTEOVERLAP = 0
2254 GM_CROSSLABELENDPOINTADJACENCIES = 0
2255 GM_CLUECROWDINGRATIO = 0
2256 rec_t1 = 0
2257 IF rec_t1 < 0 OR rec_t1 > 32767 OR rec_t1 <> INT(rec_t1) THEN PRINT "Invalid record bound: GEOPAIR": STOP
2258 DIM GEOPAIR_MANHATTANDISTANCE%(rec_t1)
2259 FOR rec_t2 = 0 TO rec_t1
2260 GEOPAIR_MANHATTANDISTANCE%(rec_t2) = 0
2261 END FOR rec_t2
2262 DIM GEOPAIR_SHORTESTDISTANCE%(rec_t1)
2263 FOR rec_t3 = 0 TO rec_t1
2264 GEOPAIR_SHORTESTDISTANCE%(rec_t3) = 0
2265 END FOR rec_t3
2266 DIM GEOPAIR_SHORTESTROUTECOUNT%(rec_t1)
2267 FOR rec_t4 = 0 TO rec_t1
2268 GEOPAIR_SHORTESTROUTECOUNT%(rec_t4) = 0
2269 END FOR rec_t4
2270 DIM GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t1)
2271 FOR rec_t5 = 0 TO rec_t1
2272 GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t5) = 0
2273 END FOR rec_t5
2274 DIM GEOPAIR_MINENDPOINTEXITS%(rec_t1)
2275 FOR rec_t6 = 0 TO rec_t1
2276 GEOPAIR_MINENDPOINTEXITS%(rec_t6) = 0
2277 END FOR rec_t6
2278 DIM GEOPAIR_MAXENDPOINTEXITS%(rec_t1)
2279 FOR rec_t7 = 0 TO rec_t1
2280 GEOPAIR_MAXENDPOINTEXITS%(rec_t7) = 0
2281 END FOR rec_t7
2282 DIM GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t1)
2283 FOR rec_t8 = 0 TO rec_t1
2284 GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t8) = 0
2285 END FOR rec_t8
2286 DIM GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t1)
2287 FOR rec_t9 = 0 TO rec_t1
2288 GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t9) = 0
2289 END FOR rec_t9
2290 DIM geoA%(0)
2291 DIM geoB%(0)
2292 DIM geoWays%(0)
2293 DIM geoUsage%(0)
2294 DIM geoMap%(0)
2295 DIM geoLog(0)
2296 DIM geoNames$(9,32)
2297 DIM geoValues(9)
2298 DIM geoWeights%(9)
2299 DIM geoPoints(9)
2300 DIM shortOwner%(0)
2301 DIM shortOrder%(0)
2302 DIM shortPrevious%(0)
2303 DIM shortPairOrder%(0)
2304 geoCount% = 0
2305 geoScore% = 0
2306 geoRaw = 0
2307 geoEasy% = 30
2308 geoHard% = 55
2309 shortSolved% = 0
2310 shortTrials% = 0
2311 shortLimit% = 32
2312 shortFirst% = 0
2313 DEFine PROCedure GEO_DISTANCES(GDPAIR%,GDSOURCE%,GDSECOND%)
2314 LOCal gdI%
2315 LOCal gdHead%
2316 LOCal gdTail%
2317 LOCal gdAt%
2318 LOCal gdN%
2319 LOCal gdD%
2320 LOCal gdNextDistance%
2321   FOR gdI%=0 TO gN%-1
2322 IF gdSecond% THEN
2323 geoB%(gdI%)=-1
2324 END IF
2325 IF NOT gdSecond% THEN
2326 geoA%(gdI%)=-1
2327 geoWays%(gdI%)=0
2328 END IF
2329   END FOR gdI%
2330 IF gdSecond% THEN
2331 geoB%(gdSource%)=0
2332 END IF
2333 IF NOT gdSecond% THEN
2334 geoA%(gdSource%)=0
2335 geoWays%(gdSource%)=1
2336 END IF
2337 gdHead% = 0
2338 gdTail% = 1
2339 gQueue%(0)=gdSource%
2340   REPeat gdBFS
2341 IF gdHead%>=gdTail% THEN
2342 EXIT gdBFS
2343 END IF
2344 gdAt% = gQueue%(gdHead%)
2345 gdHead% = gdHead%+1
2346 gdNextDistance% = geoA%(gdAt%)+1
2347 IF gdSecond% THEN
2348 gdNextDistance% = geoB%(gdAt%)+1
2349 END IF
2350     FOR gdD%=0 TO gDegree%(gdAt%)-1
2351 gdN% = gNb%(gdAt%,gdD%)
2352 IF gEndpoint%(gdN%)=0 OR gEndpoint%(gdN%)=gdPair% THEN
2353 IF gdSecond% THEN
2354 IF geoB%(gdN%)<0 THEN
2355 geoB%(gdN%)=gdNextDistance%
2356 gQueue%(gdTail%)=gdN%
2357 gdTail% = gdTail%+1
2358 END IF
2359         ELSE
2360 IF geoA%(gdN%)<0 THEN
2361 geoA%(gdN%)=gdNextDistance%
2362 gQueue%(gdTail%)=gdN%
2363 gdTail% = gdTail%+1
2364 END IF
2365 IF geoA%(gdN%)=gdNextDistance% THEN
2366 geoWays%(gdN%)=gen_min((1025),(geoWays%(gdN%)+geoWays%(gdAt%)))
2367 END IF
2368         END IF
2369       END IF
2370     END FOR gdD%
2371   END REPeat gdBFS
2372 END DEFine
2373 DEFine FuNction GEO_CORNER%(GCCELL%)
2374 RETurn (gcCell%=0 OR gcCell%=OPT_WIDTH%-1 OR gcCell%=gN%-OPT_WIDTH% OR gcCell%=gN%-1)
2375 END DEFine
2376 DEFine FuNction GEO_EDGE%(GECELL%)
2377 RETurn (geCell% MOD OPT_WIDTH%=0 OR geCell% MOD OPT_WIDTH%=OPT_WIDTH%-1 OR geCell%<OPT_WIDTH% OR geCell%>=gN%-OPT_WIDTH%)
2378 END DEFine
2379 DEFine FuNction GEO_EXITS%(GXCELL%,GXPAIR%,GXDISTANCE%)
2380 LOCal gxD%
2381 LOCal gxN%
2382 LOCal gxCount%
2383 IF gxDistance%=1 THEN
2384 RETurn 1
2385 END IF
2386 gxCount% = 0
2387   FOR gxD%=0 TO gDegree%(gxCell%)-1
2388 gxN% = gNb%(gxCell%,gxD%)
2389 IF gEndpoint%(gxN%)=0 OR gEndpoint%(gxN%)=gxPair% THEN
2390 gxCount% = gxCount%+1
2391 END IF
2392   END FOR gxD%
2393 RETurn gxCount%
2394 END DEFine
2395 DEFine PROCedure GEO_COMPONENT(GCNAME$,GCVALUE,GCWEIGHT%)
2396 geoNames$(geoCount%)=gcName$
2397 geoValues(geoCount%)=gen_six((gen_max((0),(gen_min((1),(gcValue))))))
2398 geoWeights%(geoCount%)=gcWeight%
2399 geoPoints(geoCount%)=gen_six((gcWeight%*geoValues(geoCount%)))
2400 geoRaw = geoRaw+geoPoints(geoCount%)
2401 geoCount% = geoCount%+1
2402 END DEFine
2403 DEFine PROCedure GEO_ASSESS
2404 LOCal rec_t1
2405 LOCal rec_t2
2406 LOCal rec_t3
2407 LOCal rec_t4
2408 LOCal rec_t5
2409 LOCal rec_t6
2410 LOCal rec_t7
2411 LOCal rec_t8
2412 LOCal rec_t9
2413 LOCal gaI%
2414 LOCal gaJ%
2415 LOCal gaP%
2416 LOCal gaActual%
2417 LOCal gaStart%
2418 LOCal gaEnd%
2419 LOCal gaM%
2420 LOCal gaX%
2421 LOCal gaY%
2422 LOCal gaCorner%
2423 LOCal gaEdge%
2424 LOCal gaExA%
2425 LOCal gaExB%
2426 LOCal gaN%
2427 LOCal gaD%
2428 LOCal gaCorridor%
2429 LOCal gaSwap%
2430 LOCal gaCornerSum
2431 LOCal gaEdgeSum
2432 LOCal gaExits
2433 LOCal gaDist
2434 LOCal gaManhattan
2435 LOCal gaLogSum
2436 LOCal gaLogValue
2437 LOCal gaDenom
2438 GM_BOARDCELLCOUNT = 0
2439 GM_PAIRCOUNT = 0
2440 GM_ENDPOINTCOUNT = 0
2441 GM_CORNERENDPOINTCOUNT = 0
2442 GM_EDGEENDPOINTCOUNT = 0
2443 GM_MEANCORNERDISTANCE = 0
2444 GM_MEANEDGEDISTANCE = 0
2445 GM_MEANAVAILABLEEXITS = 0
2446 GM_SINGLEEXITENDPOINTCOUNT = 0
2447 GM_ADJACENTPAIRCOUNT = 0
2448 GM_ALIGNEDPAIRCOUNT = 0
2449 GM_MEANMANHATTANDISTANCE = 0
2450 GM_MEANSHORTESTDISTANCE = 0
2451 GM_MINIMUMREQUIREDCELLS = 0
2452 GM_MINIMUMCOVERAGERATIO = 0
2453 GM_COVERAGESLACKCELLS = 0
2454 GM_REQUIREDEXTRACOVERAGECELLS = 0
2455 GM_FORCEDCORNERTURNCOUNT = 0
2456 GM_MEANLOG2SHORTESTROUTECOUNT = 0
2457 GM_SHORTESTROUTECOUNTLIMIT = 0
2458 GM_CAPPEDROUTEPAIRCOUNT = 0
2459 GM_ROUTEINTERIOROCCURRENCES = 0
2460 GM_SHAREDROUTEINTERIOROCCURRENCES = 0
2461 GM_ROUTEOVERLAPRATIO = 0
2462 GM_MAXROUTEOVERLAP = 0
2463 GM_CROSSLABELENDPOINTADJACENCIES = 0
2464 GM_CLUECROWDINGRATIO = 0
2465 rec_t1 = OPT_PAIRS%
2466 IF rec_t1 < 0 OR rec_t1 > 32767 OR rec_t1 <> INT(rec_t1) THEN PRINT "Invalid record bound: GEOPAIR": STOP
2467 DIM GEOPAIR_MANHATTANDISTANCE%(rec_t1)
2468 FOR rec_t2 = 0 TO rec_t1
2469 GEOPAIR_MANHATTANDISTANCE%(rec_t2) = 0
2470 END FOR rec_t2
2471 DIM GEOPAIR_SHORTESTDISTANCE%(rec_t1)
2472 FOR rec_t3 = 0 TO rec_t1
2473 GEOPAIR_SHORTESTDISTANCE%(rec_t3) = 0
2474 END FOR rec_t3
2475 DIM GEOPAIR_SHORTESTROUTECOUNT%(rec_t1)
2476 FOR rec_t4 = 0 TO rec_t1
2477 GEOPAIR_SHORTESTROUTECOUNT%(rec_t4) = 0
2478 END FOR rec_t4
2479 DIM GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t1)
2480 FOR rec_t5 = 0 TO rec_t1
2481 GEOPAIR_SHORTESTROUTECOUNTCAPPED%(rec_t5) = 0
2482 END FOR rec_t5
2483 DIM GEOPAIR_MINENDPOINTEXITS%(rec_t1)
2484 FOR rec_t6 = 0 TO rec_t1
2485 GEOPAIR_MINENDPOINTEXITS%(rec_t6) = 0
2486 END FOR rec_t6
2487 DIM GEOPAIR_MAXENDPOINTEXITS%(rec_t1)
2488 FOR rec_t7 = 0 TO rec_t1
2489 GEOPAIR_MAXENDPOINTEXITS%(rec_t7) = 0
2490 END FOR rec_t7
2491 DIM GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t1)
2492 FOR rec_t8 = 0 TO rec_t1
2493 GEOPAIR_SHORTESTROUTEINTERIORCELLS%(rec_t8) = 0
2494 END FOR rec_t8
2495 DIM GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t1)
2496 FOR rec_t9 = 0 TO rec_t1
2497 GEOPAIR_SHAREDROUTEINTERIORCELLS%(rec_t9) = 0
2498 END FOR rec_t9
2499 DIM geoA%(gN%-1)
2500 DIM geoB%(gN%-1)
2501 DIM geoWays%(gN%-1)
2502 DIM geoUsage%(gN%-1)
2503 DIM geoMap%(OPT_PAIRS%)
2504 DIM geoLog(OPT_PAIRS%)
2505 GM_BOARDCELLCOUNT=gN%
2506 GM_PAIRCOUNT=OPT_PAIRS%
2507 GM_ENDPOINTCOUNT=2*OPT_PAIRS%
2508 GM_SHORTESTROUTECOUNTLIMIT=1024
2509 gaCornerSum = 0
2510 gaEdgeSum = 0
2511 gaExits = 0
2512 gaDist = 0
2513 gaManhattan = 0
2514 gaLogSum = 0
2515   FOR gaI%=0 TO gN%-1
2516     geoUsage%(gaI%)=0
2517 IF gEndpoint%(gaI%) THEN
2518 gaX% = gen_min((gaI% MOD OPT_WIDTH%),(OPT_WIDTH%-1-(gaI% MOD OPT_WIDTH%)))
2519 gaY% = gen_min((INT(gaI%/OPT_WIDTH%)),(OPT_HEIGHT%-1-INT(gaI%/OPT_WIDTH%)))
2520 gaCornerSum = gaCornerSum+gaX%+gaY%
2521 gaEdgeSum = gaEdgeSum+gen_min((gaX%),(gaY%))
2522 IF geo_corner%((gaI%)) THEN
2523         GM_CORNERENDPOINTCOUNT=GM_CORNERENDPOINTCOUNT+1
2524       ELSE
2525 IF geo_edge%((gaI%)) THEN
2526 GM_EDGEENDPOINTCOUNT=GM_EDGEENDPOINTCOUNT+1
2527 END IF
2528       END IF
2529       FOR gaD%=0 TO gDegree%(gaI%)-1
2530 gaN% = gNb%(gaI%,gaD%)
2531 IF gaN%>gaI% AND gEndpoint%(gaN%) AND gEndpoint%(gaN%)<>gEndpoint%(gaI%) THEN
2532 GM_CROSSLABELENDPOINTADJACENCIES=GM_CROSSLABELENDPOINTADJACENCIES+1
2533 END IF
2534       END FOR gaD%
2535     ELSE
2536 IF OPT_FULL% AND OPT_WIDTH%>1 AND OPT_HEIGHT%>1 AND geo_corner%((gaI%)) THEN
2537 GM_FORCEDCORNERTURNCOUNT=GM_FORCEDCORNERTURNCOUNT+1
2538 END IF
2539     END IF
2540   END FOR gaI%
2541   FOR gaP%=1 TO OPT_PAIRS%
2542     geoMap%(gaP%)=gaP%
2543   END FOR gaP%
2544   FOR gaP%=2 TO OPT_PAIRS%
2545 gaActual% = geoMap%(gaP%)
2546 gaJ% = gaP%-1
2547     REPeat gaSort
2548 IF gaJ%<1 THEN
2549 EXIT gaSort
2550 END IF
2551 IF NOT gen_less%((gen_label$((gaActual%))),(gen_label$((geoMap%(gaJ%))))) THEN
2552 EXIT gaSort
2553 END IF
2554 geoMap%(gaJ%+1)=geoMap%(gaJ%)
2555 gaJ% = gaJ%-1
2556     END REPeat gaSort
2557     geoMap%(gaJ%+1)=gaActual%
2558   END FOR gaP%
2559   FOR gaP%=1 TO OPT_PAIRS%
2560 gen_poll
2561 IF gCancel% THEN
2562 RETurn 
2563 END IF
2564 gaActual% = geoMap%(gaP%)
2565 gaStart% = gStart%(gaActual%)
2566 gaEnd% = gFinish%(gaActual%)
2567 gaM% = ABS((gaStart% MOD OPT_WIDTH%)-(gaEnd% MOD OPT_WIDTH%))+ABS(INT(gaStart%/OPT_WIDTH%)-INT(gaEnd%/OPT_WIDTH%))
2568 geo_distances (gaActual%),(gaStart%),(0)
2569 geo_distances (gaActual%),(gaEnd%),(1)
2570 IF geoA%(gaEnd%)<0 THEN
2571 gExit% = 1
2572 gError$ = "Internal error: clues have no connecting route."
2573 RETurn 
2574 END IF
2575 gaExA% = geo_exits%((gaStart%),(gaActual%),(gaM%))
2576 gaExB% = geo_exits%((gaEnd%),(gaActual%),(gaM%))
2577 GEOPAIR_MANHATTANDISTANCE%(gaP%)=gaM%
2578 GEOPAIR_SHORTESTDISTANCE%(gaP%)=geoA%(gaEnd%)
2579 GEOPAIR_SHORTESTROUTECOUNT%(gaP%)=gen_min((1024),(geoWays%(gaEnd%)))
2580 GEOPAIR_SHORTESTROUTECOUNTCAPPED%(gaP%)=(geoWays%(gaEnd%)>1024)
2581 GEOPAIR_MINENDPOINTEXITS%(gaP%)=gen_min((gaExA%),(gaExB%))
2582 GEOPAIR_MAXENDPOINTEXITS%(gaP%)=gen_max((gaExA%),(gaExB%))
2583     geoLog(gaP%)=gen_log2((GEOPAIR_SHORTESTROUTECOUNT%(gaP%)))
2584 IF gaM%=1 THEN
2585 GM_ADJACENTPAIRCOUNT=GM_ADJACENTPAIRCOUNT+1
2586 END IF
2587 IF gaStart% MOD OPT_WIDTH%=gaEnd% MOD OPT_WIDTH% OR INT(gaStart%/OPT_WIDTH%)=INT(gaEnd%/OPT_WIDTH%) THEN
2588 GM_ALIGNEDPAIRCOUNT=GM_ALIGNEDPAIRCOUNT+1
2589 END IF
2590 IF gaExA%=1 THEN
2591 GM_SINGLEEXITENDPOINTCOUNT=GM_SINGLEEXITENDPOINTCOUNT+1
2592 END IF
2593 IF gaExB%=1 THEN
2594 GM_SINGLEEXITENDPOINTCOUNT=GM_SINGLEEXITENDPOINTCOUNT+1
2595 END IF
2596 IF GEOPAIR_SHORTESTROUTECOUNTCAPPED%(gaP%) THEN
2597 GM_CAPPEDROUTEPAIRCOUNT=GM_CAPPEDROUTEPAIRCOUNT+1
2598 END IF
2599 gaExits = gaExits+gaExA%+gaExB%
2600 gaDist = gaDist+geoA%(gaEnd%)
2601 gaManhattan = gaManhattan+gaM%
2602     GM_MINIMUMREQUIREDCELLS=GM_MINIMUMREQUIREDCELLS+geoA%(gaEnd%)+1
2603 gaCorridor% = 0
2604     FOR gaI%=0 TO gN%-1
2605 IF NOT gEndpoint%(gaI%) AND geoA%(gaI%)>=0 AND geoB%(gaI%)>=0 AND geoA%(gaI%)+geoB%(gaI%)=geoA%(gaEnd%) THEN
2606 gaCorridor% = gaCorridor%+1
2607 geoUsage%(gaI%)=geoUsage%(gaI%)+1
2608       END IF
2609     END FOR gaI%
2610 GEOPAIR_SHORTESTROUTEINTERIORCELLS%(gaP%)=gaCorridor%
2611 GM_ROUTEINTERIOROCCURRENCES=GM_ROUTEINTERIOROCCURRENCES+gaCorridor%
2612   END FOR gaP%
2613   FOR gaP%=1 TO OPT_PAIRS%
2614 gaActual% = geoMap%(gaP%)
2615 gaEnd% = gFinish%(gaActual%)
2616 geo_distances (gaActual%),(gStart%(gaActual%)),(0)
2617 geo_distances (gaActual%),(gaEnd%),(1)
2618 gaCorridor% = 0
2619     FOR gaI%=0 TO gN%-1
2620 IF NOT gEndpoint%(gaI%) AND geoUsage%(gaI%)>1 AND geoA%(gaI%)>=0 AND geoB%(gaI%)>=0 AND geoA%(gaI%)+geoB%(gaI%)=geoA%(gaEnd%) THEN
2621 gaCorridor% = gaCorridor%+1
2622 END IF
2623     END FOR gaI%
2624 GEOPAIR_SHAREDROUTEINTERIORCELLS%(gaP%)=gaCorridor%
2625 GM_SHAREDROUTEINTERIOROCCURRENCES=GM_SHAREDROUTEINTERIOROCCURRENCES+gaCorridor%
2626   END FOR gaP%
2627   FOR gaI%=0 TO gN%-1
2628     GM_MAXROUTEOVERLAP=gen_max((GM_MAXROUTEOVERLAP),(geoUsage%(gaI%)))
2629   END FOR gaI%
2630   FOR gaP%=2 TO OPT_PAIRS%
2631 gaLogValue = geoLog(gaP%)
2632 gaJ% = gaP%-1
2633     REPeat gaLogSort
2634 IF gaJ%<1 THEN
2635 EXIT gaLogSort
2636 END IF
2637 IF geoLog(gaJ%)<=gaLogValue THEN
2638 EXIT gaLogSort
2639 END IF
2640 geoLog(gaJ%+1)=geoLog(gaJ%)
2641 gaJ% = gaJ%-1
2642     END REPeat gaLogSort
2643     geoLog(gaJ%+1)=gaLogValue
2644   END FOR gaP%
2645   FOR gaP%=1 TO OPT_PAIRS%
2646 gaLogSum = gaLogSum+geoLog(gaP%)
2647   END FOR gaP%
2648 GM_MEANCORNERDISTANCE=gen_six((gaCornerSum/GM_ENDPOINTCOUNT))
2649 GM_MEANEDGEDISTANCE=gen_six((gaEdgeSum/GM_ENDPOINTCOUNT))
2650 GM_MEANAVAILABLEEXITS=gen_six((gaExits/GM_ENDPOINTCOUNT))
2651 GM_MEANMANHATTANDISTANCE=gen_six((gaManhattan/OPT_PAIRS%))
2652 GM_MEANSHORTESTDISTANCE=gen_six((gaDist/OPT_PAIRS%))
2653 GM_MEANLOG2SHORTESTROUTECOUNT=gen_six((gaLogSum/OPT_PAIRS%))
2654 GM_MINIMUMCOVERAGERATIO=gen_six((GM_MINIMUMREQUIREDCELLS/gN%))
2655 GM_COVERAGESLACKCELLS=gN%-GM_MINIMUMREQUIREDCELLS
2656 IF OPT_FULL% THEN
2657 GM_REQUIREDEXTRACOVERAGECELLS=gen_max((0),(GM_COVERAGESLACKCELLS))
2658 END IF
2659 IF GM_ROUTEINTERIOROCCURRENCES THEN
2660 GM_ROUTEOVERLAPRATIO=gen_six((GM_SHAREDROUTEINTERIOROCCURRENCES/GM_ROUTEINTERIOROCCURRENCES))
2661 END IF
2662   GM_CLUECROWDINGRATIO=gen_six((GM_CROSSLABELENDPOINTADJACENCIES/(2*GM_ENDPOINTCOUNT)))
2663 geoCount% = 0
2664 geoRaw = 0
2665 geoEasy% = 30
2666 geoHard% = 55
2667 geo_component ("boardScale"),(gen_log2((gN%))/12),(10)
2668 gaX% = 10
2669 IF OPT_FULL% THEN
2670 gaX% = 8
2671 END IF
2672 gaLogValue = OPT_PAIRS%
2673 geo_component ("pairLoad"),(gaLogValue/16),(gaX%)
2674 gaX% = 18
2675 IF OPT_FULL% THEN
2676 gaX% = 16
2677 END IF
2678 geo_component ("endpointFreedom"),((GM_MEANAVAILABLEEXITS-1)/3),(gaX%)
2679 gaX% = 12
2680 IF OPT_FULL% THEN
2681 gaX% = 8
2682 END IF
2683 geo_component ("shortRouteAmbiguity"),(GM_MEANLOG2SHORTESTROUTECOUNT/10),(gaX%)
2684 gaX% = 22
2685 IF OPT_FULL% THEN
2686 gaX% = 24
2687 END IF
2688 geo_component ("routeOverlap"),(GM_ROUTEOVERLAPRATIO),(gaX%)
2689 geo_component ("clueCrowding"),(GM_CLUECROWDINGRATIO),(6)
2690 gaDenom = gen_max((1),(INT((OPT_WIDTH%-1)/2)+INT((OPT_HEIGHT%-1)/2)))
2691 geo_component ("cornerDistance"),(GM_MEANCORNERDISTANCE/gaDenom),(6)
2692 IF OPT_FULL% THEN
2693 geoEasy% = 35
2694 geoHard% = 60
2695 geo_component ("requiredCoverageDetours"),(GM_REQUIREDEXTRACOVERAGECELLS/gN%),(22)
2696 geo_component ("forcedEndpointCredit"),(GM_SINGLEEXITENDPOINTCOUNT/GM_ENDPOINTCOUNT),(-18)
2697 geo_component ("forcedCornerCredit"),(GM_FORCEDCORNERTURNCOUNT/4),(-6)
2698   ELSE
2699 geo_component ("minimumCoveragePressure"),(GM_MINIMUMCOVERAGERATIO),(16)
2700 geo_component ("forcedEndpointCredit"),(GM_SINGLEEXITENDPOINTCOUNT/GM_ENDPOINTCOUNT),(-20)
2701   END IF
2702 geoRaw = gen_six((geoRaw))
2703 geoScore% = gen_max((0),(gen_min((100),(gen_round((geoRaw))))))
2704 END DEFine
2705 DEFine PROCedure SHORTEST_SCREEN(SQTRIALS%)
2706 LOCal sqT%
2707 LOCal sqI%
2708 LOCal sqJ%
2709 LOCal sqSwap%
2710 LOCal sqP%
2711 LOCal sqActual%
2712 LOCal sqA%
2713 LOCal sqB%
2714 LOCal sqHead%
2715 LOCal sqTail%
2716 LOCal sqAt%
2717 LOCal sqD%
2718 LOCal sqN%
2719 LOCal sqCount%
2720 LOCal sqFailed%
2721 LOCal sqOrder%
2722 LOCal sqOK%
2723 shortSolved% = 0
2724 shortTrials% = 0
2725 shortFirst% = 0
2726 shortLimit% = sqTrials%
2727 DIM shortOwner%(gN%-1)
2728 DIM shortOrder%(gN%-1)
2729 DIM shortPrevious%(gN%-1)
2730 DIM shortPairOrder%(OPT_PAIRS%)
2731 identity_canonical
2732 rng_seed (1),(1413566023)
2733 gRngStream% = 1
2734   FOR sqT%=1 TO sqTrials%
2735 gen_poll
2736 IF gCancel% THEN
2737 EXIT sqT%
2738 END IF
2739     FOR sqI%=1 TO OPT_PAIRS%
2740       shortPairOrder%(sqI%)=sqI%
2741     END FOR sqI%
2742 IF sqT%=1 THEN
2743       FOR sqI%=2 TO OPT_PAIRS%
2744 sqP% = shortPairOrder%(sqI%)
2745 sqActual% = gCanonical%(sqP%)
2746 sqA% = gStart%(sqActual%)
2747 sqB% = gFinish%(sqActual%)
2748 sqCount% = ABS((sqA% MOD OPT_WIDTH%)-(sqB% MOD OPT_WIDTH%))+ABS(INT(sqA%/OPT_WIDTH%)-INT(sqB%/OPT_WIDTH%))
2749 sqJ% = sqI%-1
2750         REPeat sqSort
2751 IF sqJ%<1 THEN
2752 EXIT sqSort
2753 END IF
2754 sqActual% = gCanonical%(shortPairOrder%(sqJ%))
2755 sqA% = gStart%(sqActual%)
2756 sqB% = gFinish%(sqActual%)
2757 sqD% = ABS((sqA% MOD OPT_WIDTH%)-(sqB% MOD OPT_WIDTH%))+ABS(INT(sqA%/OPT_WIDTH%)-INT(sqB%/OPT_WIDTH%))
2758 IF sqD%<sqCount% OR (sqD%=sqCount% AND shortPairOrder%(sqJ%)<sqP%) THEN
2759 EXIT sqSort
2760 END IF
2761 shortPairOrder%(sqJ%+1)=shortPairOrder%(sqJ%)
2762 sqJ% = sqJ%-1
2763         END REPeat sqSort
2764         shortPairOrder%(sqJ%+1)=sqP%
2765       END FOR sqI%
2766     ELSE
2767       FOR sqI%=OPT_PAIRS% TO 2 STEP -1
2768 sqJ% = 1+rng_next%((sqI%))
2769 sqSwap% = shortPairOrder%(sqI%)
2770 shortPairOrder%(sqI%)=shortPairOrder%(sqJ%)
2771 shortPairOrder%(sqJ%)=sqSwap%
2772       END FOR sqI%
2773     END IF
2774     FOR sqI%=0 TO gN%-1
2775 shortOwner%(sqI%)=0
2776 shortOrder%(sqI%)=0
2777     END FOR sqI%
2778 sqFailed% = 0
2779     FOR sqP%=1 TO OPT_PAIRS%
2780 sqActual% = gCanonical%(shortPairOrder%(sqP%))
2781 sqA% = gA%(shortPairOrder%(sqP%))
2782 sqB% = gB%(shortPairOrder%(sqP%))
2783       FOR sqI%=0 TO gN%-1
2784         shortPrevious%(sqI%)=-1
2785       END FOR sqI%
2786 shortPrevious%(sqA%)=sqA%
2787 sqHead% = 0
2788 sqTail% = 1
2789 gQueue%(0)=sqA%
2790       REPeat sqBFS
2791 IF sqHead%>=sqTail% OR shortPrevious%(sqB%)>=0 THEN
2792 EXIT sqBFS
2793 END IF
2794 sqAt% = gQueue%(sqHead%)
2795 sqHead% = sqHead%+1
2796 sqCount% = gDegree%(sqAt%)
2797         FOR sqD%=0 TO sqCount%-1
2798           gScratch%(sqD%)=gNb%(sqAt%,sqD%)
2799         END FOR sqD%
2800         FOR sqD%=sqCount%-1 TO 1 STEP -1
2801 sqJ% = rng_next%((sqD%+1))
2802 sqSwap% = gScratch%(sqD%)
2803 gScratch%(sqD%)=gScratch%(sqJ%)
2804 gScratch%(sqJ%)=sqSwap%
2805         END FOR sqD%
2806         FOR sqD%=0 TO sqCount%-1
2807 sqN% = gScratch%(sqD%)
2808 IF shortPrevious%(sqN%)<0 AND NOT shortOwner%(sqN%) AND (gEndpoint%(sqN%)=0 OR gEndpoint%(sqN%)=sqActual%) THEN
2809 shortPrevious%(sqN%)=sqAt%
2810 gQueue%(sqTail%)=sqN%
2811 sqTail% = sqTail%+1
2812           END IF
2813         END FOR sqD%
2814       END REPeat sqBFS
2815 IF shortPrevious%(sqB%)<0 THEN
2816 sqFailed% = 1
2817 EXIT sqP%
2818 END IF
2819 sqAt% = sqB%
2820 sqOrder% = 1
2821       REPeat sqTrace
2822 shortOwner%(sqAt%)=sqActual%
2823 shortOrder%(sqAt%)=sqOrder%
2824 IF sqAt%=sqA% THEN
2825 EXIT sqTrace
2826 END IF
2827 sqOrder% = sqOrder%+1
2828 sqAt% = shortPrevious%(sqAt%)
2829       END REPeat sqTrace
2830     END FOR sqP%
2831 shortTrials% = sqT%
2832 IF NOT sqFailed% THEN
2833 sqOK% = 1
2834       FOR sqI%=0 TO gN%-1
2835 IF shortOwner%(sqI%) THEN
2836           FOR sqD%=0 TO gDegree%(sqI%)-1
2837 sqN% = gNb%(sqI%,sqD%)
2838 IF shortOwner%(sqN%)=shortOwner%(sqI%) AND ABS(shortOrder%(sqN%)-shortOrder%(sqI%))<>1 THEN
2839 sqOK% = 0
2840 END IF
2841           END FOR sqD%
2842         END IF
2843       END FOR sqI%
2844 IF sqOK% THEN
2845 shortSolved% = 1
2846 shortFirst% = sqT%
2847 EXIT sqT%
2848 END IF
2849     END IF
2850   END FOR sqT%
2851 gRngStream% = 0
2852 gHasShortest% = 1
2853 END DEFine
2854 DIM CHOSENREASON_STATUS$(20)
2855 CHOSENREASON_STATUS$ = ""
2856 DIM CHOSENREASON_LIMITREASON$(128)
2857 CHOSENREASON_LIMITREASON$ = ""
2858 DIM CHOSENREASON_TIER$(20)
2859 CHOSENREASON_TIER$ = ""
2860 DIM CHOSENREASON_GRADE$(12)
2861 CHOSENREASON_GRADE$ = ""
2862 CHOSENREASON_SCORE% = 0
2863 CHOSENREASON_LOGICAL% = 0
2864 CHOSENREASON_LOCALDEDUCTIONS = 0
2865 CHOSENREASON_ROUTEDEDUCTIONS = 0
2866 CHOSENREASON_ROUTEPASSES = 0
2867 CHOSENREASON_TRIALSTEPS = 0
2868 CHOSENREASON_TRIALASSUMPTIONS = 0
2869 CHOSENREASON_SEARCHNODES = 0
2870 CHOSENREASON_FIRSTNODES = 0
2871 CHOSENREASON_FIRSTDEPTH% = 0
2872 CHOSENREASON_MAXDEPTH% = 0
2873 CHOSENREASON_SOLUTIONCOUNT% = 0
2874 CHOSENREASON_EXHAUSTIVE% = 0
2875 DIM CHOSENREASON_UNIQUENESS$(20)
2876 CHOSENREASON_UNIQUENESS$ = ""
2877 CHOSENREASON_WORK = 0
2878 CHOSENREASON_WORKLIMIT = 0
2879 DIM CHOSENREASON_BOARDKEY$(64)
2880 CHOSENREASON_BOARDKEY$ = ""
2881 DIM selWalk%(0)
2882 DIM selScore(2,4)
2883 selConsidered% = 0
2884 selEvaluated% = 0
2885 selDuplicates% = 0
2886 selBudgetRejected% = 0
2887 selAssessments% = 0
2888 selAccepted% = 0
2889 selFirst% = -1
2890 selSeen% = 0
2891 DIM selStartingTier$(20)
2892 DIM selKey$(64)
2893 DIM selAlgorithm$(64)
2894 selStartingTrials = 0
2895 selChosenReason% = 0
2896 selChosenShortest% = 0
2897 selShortSolved% = 0
2898 selShortTrials% = 0
2899 selShortLimit% = 0
2900 selShortFirst% = 0
2901 DEFine FuNction SELECTION_ACTIVE%
2902 RETurn (OPT_ASSESS% OR OPT_TARGET%>0 OR OPT_UNIQUE% OR OPT_SHORTEST%)
2903 END DEFine
2904 DEFine FuNction SELECTION_STRENGTHENED%
2905 RETurn (OPT_MINTRIALS>0 OR OPT_MAXTRIALS>=0 OR OPT_MINDEPTH%>0 OR OPT_MINNODES%>0)
2906 END DEFine
2907 DEFine FuNction SELECTION_MINIMUMS%
2908 IF SR_STATUS$<>"complete" THEN
2909 RETurn 0
2910 END IF
2911 IF SR_TRIALSTEPS<OPT_MINTRIALS OR SR_FIRSTDEPTH%<OPT_MINDEPTH% OR SR_FIRSTNODES<OPT_MINNODES% THEN
2912 RETurn 0
2913 END IF
2914 IF OPT_MAXTRIALS>=0 AND SR_TRIALSTEPS>OPT_MAXTRIALS THEN
2915 RETurn 0
2916 END IF
2917 RETurn 1
2918 END DEFine
2919 DEFine FuNction SELECTION_COMPARE%(SCROW%)
2920 LOCal scI%
2921   FOR scI%=0 TO 4
2922 IF selScore(0,scI%)>selScore(scRow%,scI%) THEN
2923 RETurn 1
2924 END IF
2925 IF selScore(0,scI%)<selScore(scRow%,scI%) THEN
2926 RETurn -1
2927 END IF
2928   END FOR scI%
2929 RETurn 0
2930 END DEFine
2931 DEFine PROCedure SELECTION_SAVE
2932 LOCal svI%
2933   FOR svI%=0 TO gN%-1
2934 gSavedOwner%(svI%)=gOwner%(svI%)
2935 gSavedOrder%(svI%)=gOrder%(svI%)
2936   END FOR svI%
2937 selChosenReason% = gHasReason%
2938 selChosenShortest% = gHasShortest%
2939 selShortSolved% = shortSolved%
2940 selShortTrials% = shortTrials%
2941 selShortLimit% = shortLimit%
2942 selShortFirst% = shortFirst%
2943   CHOSENREASON_STATUS$=SR_STATUS$
2944   CHOSENREASON_LIMITREASON$=SR_LIMITREASON$
2945   CHOSENREASON_TIER$=SR_TIER$
2946   CHOSENREASON_GRADE$=SR_GRADE$
2947   CHOSENREASON_SCORE%=SR_SCORE%
2948   CHOSENREASON_LOGICAL%=SR_LOGICAL%
2949   CHOSENREASON_LOCALDEDUCTIONS=SR_LOCALDEDUCTIONS
2950   CHOSENREASON_ROUTEDEDUCTIONS=SR_ROUTEDEDUCTIONS
2951   CHOSENREASON_ROUTEPASSES=SR_ROUTEPASSES
2952   CHOSENREASON_TRIALSTEPS=SR_TRIALSTEPS
2953   CHOSENREASON_TRIALASSUMPTIONS=SR_TRIALASSUMPTIONS
2954   CHOSENREASON_SEARCHNODES=SR_SEARCHNODES
2955   CHOSENREASON_FIRSTNODES=SR_FIRSTNODES
2956   CHOSENREASON_FIRSTDEPTH%=SR_FIRSTDEPTH%
2957   CHOSENREASON_MAXDEPTH%=SR_MAXDEPTH%
2958   CHOSENREASON_SOLUTIONCOUNT%=SR_SOLUTIONCOUNT%
2959   CHOSENREASON_EXHAUSTIVE%=SR_EXHAUSTIVE%
2960   CHOSENREASON_UNIQUENESS$=SR_UNIQUENESS$
2961   CHOSENREASON_WORK=SR_WORK
2962   CHOSENREASON_WORKLIMIT=SR_WORKLIMIT
2963   CHOSENREASON_BOARDKEY$=SR_BOARDKEY$
2964 END DEFine
2965 DEFine PROCedure SELECTION_RESTORE
2966 LOCal svI%
2967 LOCal svP%
2968   FOR svP%=1 TO OPT_PAIRS%
2969     gLength%(svP%)=0
2970   END FOR svP%
2971   FOR svI%=0 TO gN%-1
2972 gOwner%(svI%)=gSavedOwner%(svI%)
2973 gOrder%(svI%)=gSavedOrder%(svI%)
2974 svP% = gOwner%(svI%)
2975 IF svP% THEN
2976       gLength%(svP%)=gen_max((gLength%(svP%)),(gOrder%(svI%)))
2977 IF gOrder%(svI%)=1 THEN
2978 gStart%(svP%)=svI%
2979 END IF
2980     END IF
2981   END FOR svI%
2982   FOR svI%=0 TO gN%-1
2983 svP% = gOwner%(svI%)
2984 IF svP% THEN
2985 IF gOrder%(svI%)=gLength%(svP%) THEN
2986 gFinish%(svP%)=svI%
2987 END IF
2988     END IF
2989   END FOR svI%
2990 gen_endpoints
2991 gHasReason% = selChosenReason%
2992 gHasShortest% = selChosenShortest%
2993 shortSolved% = selShortSolved%
2994 shortTrials% = selShortTrials%
2995 shortLimit% = selShortLimit%
2996 shortFirst% = selShortFirst%
2997   SR_STATUS$=CHOSENREASON_STATUS$
2998   SR_LIMITREASON$=CHOSENREASON_LIMITREASON$
2999   SR_TIER$=CHOSENREASON_TIER$
3000   SR_GRADE$=CHOSENREASON_GRADE$
3001   SR_SCORE%=CHOSENREASON_SCORE%
3002   SR_LOGICAL%=CHOSENREASON_LOGICAL%
3003   SR_LOCALDEDUCTIONS=CHOSENREASON_LOCALDEDUCTIONS
3004   SR_ROUTEDEDUCTIONS=CHOSENREASON_ROUTEDEDUCTIONS
3005   SR_ROUTEPASSES=CHOSENREASON_ROUTEPASSES
3006   SR_TRIALSTEPS=CHOSENREASON_TRIALSTEPS
3007   SR_TRIALASSUMPTIONS=CHOSENREASON_TRIALASSUMPTIONS
3008   SR_SEARCHNODES=CHOSENREASON_SEARCHNODES
3009   SR_FIRSTNODES=CHOSENREASON_FIRSTNODES
3010   SR_FIRSTDEPTH%=CHOSENREASON_FIRSTDEPTH%
3011   SR_MAXDEPTH%=CHOSENREASON_MAXDEPTH%
3012   SR_SOLUTIONCOUNT%=CHOSENREASON_SOLUTIONCOUNT%
3013   SR_EXHAUSTIVE%=CHOSENREASON_EXHAUSTIVE%
3014   SR_UNIQUENESS$=CHOSENREASON_UNIQUENESS$
3015   SR_WORK=CHOSENREASON_WORK
3016   SR_WORKLIMIT=CHOSENREASON_WORKLIMIT
3017   SR_BOARDKEY$=CHOSENREASON_BOARDKEY$
3018 END DEFine
3019 DEFine PROCedure SELECTION_CONSIDER
3020 LOCal skI%
3021 LOCal skMatches%
3022 LOCal skRank%
3023 LOCal skDesired%
3024 LOCal skPriority%
3025 LOCal skDistance
3026 gHasReason% = 0
3027 gHasShortest% = 0
3028 selKey$ = identity_symmetry$
3029   FOR skI%=1 TO selSeen%
3030 IF cli_equal%((gSeen$(skI%)),(selKey$)) THEN
3031 selDuplicates% = selDuplicates%+1
3032 selDistinct% = 0
3033 RETurn 
3034 END IF
3035   END FOR skI%
3036 selSeen% = selSeen%+1
3037 gSeen$(selSeen%)=selKey$
3038 selDistinct% = 1
3039 selEvaluated% = selEvaluated%+1
3040 IF OPT_SHORTEST% THEN
3041 shortest_screen (32)
3042 IF shortSolved% THEN
3043 selDistinct% = 0
3044 RETurn 
3045 END IF
3046   END IF
3047 IF OPT_ASSESS% OR OPT_TARGET%>0 OR OPT_UNIQUE% THEN
3048 selAssessments% = selAssessments%+1
3049 solver_analyse (OPT_SOLVERWORK),(20000)
3050 IF SR_UNIQUENESS$="none" THEN
3051 gExit% = 1
3052 gError$ = "Internal solver contradiction for a validated witness."
3053 RETurn 
3054 END IF
3055 IF selStartingTier$="notAssessed" THEN
3056 selStartingTier$ = SR_TIER$
3057 selStartingTrials = SR_TRIALSTEPS
3058 END IF
3059   END IF
3060 IF gCancel% THEN
3061 RETurn 
3062 END IF
3063 IF (OPT_TARGET%>0 OR OPT_UNIQUE%) AND gHasReason% AND SR_STATUS$<>"complete" THEN
3064 selBudgetRejected% = selBudgetRejected%+1
3065 END IF
3066 skMatches% = 1
3067 IF OPT_TARGET%>0 THEN
3068 skMatches% = (SR_STATUS$="complete" AND SR_GRADE$=gen_grade$((OPT_TARGET%)))
3069 END IF
3070 IF OPT_UNIQUE% AND SR_UNIQUENESS$<>"unique" THEN
3071 skMatches% = 0
3072 END IF
3073 IF selection_strengthened% THEN
3074 IF NOT gHasReason% OR NOT selection_minimums% THEN
3075 skMatches% = 0
3076 END IF
3077   END IF
3078   FOR skI%=0 TO 4
3079     selScore(0,skI%)=0
3080   END FOR skI%
3081   selScore(0,0)=-100
3082 IF gHasReason% AND SR_STATUS$="complete" AND SR_GRADE$<>"unrated" THEN
3083 skRank% = gen_rank%((SR_GRADE$))
3084 skDesired% = OPT_TARGET%
3085 IF skDesired%=0 THEN
3086 skDesired% = 3
3087 END IF
3088 skPriority% = skRank%
3089 IF skRank%>skDesired% THEN
3090 skPriority% = -9-skRank%
3091 END IF
3092 skDistance = 0
3093 IF OPT_MAXTRIALS>=0 THEN
3094 skDistance = gen_max((0),(gen_max((OPT_MINTRIALS-SR_TRIALSTEPS),(SR_TRIALSTEPS-OPT_MAXTRIALS))))
3095 END IF
3096 selScore(0,0)=skPriority%
3097 selScore(0,1)=-skDistance
3098 IF OPT_MINDEPTH%>0 OR OPT_MINNODES%>0 THEN
3099 selScore(0,2)=SR_FIRSTDEPTH%
3100 selScore(0,3)=SR_FIRSTNODES
3101 selScore(0,4)=SR_TRIALSTEPS
3102     ELSE
3103 selScore(0,2)=SR_TRIALSTEPS
3104 selScore(0,3)=SR_ROUTEDEDUCTIONS
3105     END IF
3106   END IF
3107 IF skMatches% AND (NOT selAccepted% OR selection_compare%((2))>=0) THEN
3108 selection_save
3109 selAccepted% = 1
3110 IF selFirst%<0 THEN
3111 selFirst% = selConsidered%
3112 END IF
3113     FOR skI%=0 TO 4
3114       selScore(2,skI%)=selScore(0,skI%)
3115     END FOR skI%
3116   END IF
3117 END DEFine
3118 DEFine PROCedure GENERATION_BASE(GBSEED)
3119 LOCal gbFallback%
3120 gSuccess% = 0
3121 gRngStream% = 0
3122 IF NOT OPT_FULL% THEN
3123 grow_generate (gbSeed)
3124 RETurn 
3125 END IF
3126 rng_seed (0),(gbSeed)
3127 cover_init
3128 gbFallback% = -1
3129 IF OPT_ATTEMPTS%>=128 THEN
3130 gbFallback% = gen_max((64),(OPT_ATTEMPTS%-128))
3131 END IF
3132 cover_find (128),(gen_max((64),(gN%))),(gbFallback%),(1)
3133 IF covFound% THEN
3134 gen_validate
3135 gSuccess% = gValid%
3136 END IF
3137 END DEFine
3138 DEFine PROCedure SELECTION_GENERATE(SGSEED)
3139 LOCal sgI%
3140 LOCal sgMove%
3141 LOCal sgMoves%
3142 LOCal sgCurrent%
3143 LOCal sgStagnant%
3144 LOCal sgStructured%
3145 LOCal sgTight%
3146 LOCal sgRestart%
3147 LOCal sgCompare%
3148 LOCal sgProposal%
3149 LOCal sgCandidateSeed
3150 WORK_BOARDATTEMPTS = 0
3151 WORK_TRAVERSALEVALUATIONS = 0
3152 WORK_REFINEMENTEVALUATIONS = 0
3153 WORK_STRUCTUREDSTARTS = 0
3154 DIM SR_STATUS$(20)
3155 SR_STATUS$ = ""
3156 DIM SR_LIMITREASON$(128)
3157 SR_LIMITREASON$ = ""
3158 DIM SR_TIER$(20)
3159 SR_TIER$ = ""
3160 DIM SR_GRADE$(12)
3161 SR_GRADE$ = ""
3162 SR_SCORE% = 0
3163 SR_LOGICAL% = 0
3164 SR_LOCALDEDUCTIONS = 0
3165 SR_ROUTEDEDUCTIONS = 0
3166 SR_ROUTEPASSES = 0
3167 SR_TRIALSTEPS = 0
3168 SR_TRIALASSUMPTIONS = 0
3169 SR_SEARCHNODES = 0
3170 SR_FIRSTNODES = 0
3171 SR_FIRSTDEPTH% = 0
3172 SR_MAXDEPTH% = 0
3173 SR_SOLUTIONCOUNT% = 0
3174 SR_EXHAUSTIVE% = 0
3175 DIM SR_UNIQUENESS$(20)
3176 SR_UNIQUENESS$ = ""
3177 SR_WORK = 0
3178 SR_WORKLIMIT = 0
3179 DIM SR_BOARDKEY$(64)
3180 SR_BOARDKEY$ = ""
3181 gSuccess% = 0
3182 gHasReason% = 0
3183 gHasShortest% = 0
3184 selConsidered% = 0
3185 selEvaluated% = 0
3186 selDuplicates% = 0
3187 selBudgetRejected% = 0
3188 selAssessments% = 0
3189 selAccepted% = 0
3190 selFirst% = -1
3191 selSeen% = 0
3192 selStartingTier$ = "notAssessed"
3193 selStartingTrials = 0
3194 gSelected% = selection_active%
3195 selAlgorithm$ = "ql-random-path-growth-v1"
3196 IF OPT_FULL% THEN
3197 selAlgorithm$ = "ql-hamiltonian-path-partition-v3"
3198 END IF
3199 IF OPT_FULL% AND OPT_VARIED% THEN
3200 selAlgorithm$ = "ql-hamiltonian-path-partition-varied-v1"
3201 END IF
3202 IF NOT gSelected% THEN
3203 generation_base (sgSeed)
3204 RETurn 
3205 END IF
3206 DIM gSeen$(OPT_CANDIDATES%,64)
3207 DIM gSavedOwner%(gN%-1)
3208 DIM gSavedOrder%(gN%-1)
3209 DIM selWalk%(gN%-1)
3210 gRngStream% = 0
3211 rng_seed (0),(sgSeed)
3212 rng_seed (2),(sgSeed)
3213 sgCurrent% = 0
3214 sgStagnant% = 0
3215 IF OPT_FULL% AND OPT_TARGET%>0 THEN
3216 selAlgorithm$ = "ql-solver-guided-hamiltonian-partition-v6"
3217 IF OPT_VARIED% THEN
3218 selAlgorithm$ = "ql-solver-guided-varied-cuts-v2"
3219 END IF
3220 cover_init
3221     REPeat sgGuided
3222 IF selConsidered%>=OPT_CANDIDATES% OR gExit% OR gCancel% THEN
3223 EXIT sgGuided
3224 END IF
3225 IF selFirst%>=0 AND selConsidered%-selFirst%>=OPT_IMPROVE% THEN
3226 EXIT sgGuided
3227 END IF
3228 selConsidered% = selConsidered%+1
3229 sgProposal% = 0
3230 sgStructured% = 0
3231 IF sgStagnant%>=32 THEN
3232 sgCurrent% = 0
3233 sgStagnant% = 0
3234 END IF
3235 IF NOT sgCurrent% THEN
3236 sgRestart% = 128
3237 IF gN%>64 THEN
3238 sgRestart% = 16*gN%
3239 END IF
3240 sgTight% = (gen_product((OPT_MINIMUM%),(OPT_PAIRS%))=gN% OR gen_product((OPT_MAXIMUM%),(OPT_PAIRS%))=gN%)
3241 sgStructured% = ((sgTight% AND selConsidered%=1) OR (selEvaluated%=0 AND selConsidered%=OPT_CANDIDATES%))
3242 sgCompare% = -1
3243 IF sgStructured% THEN
3244 sgCompare% = 0
3245 END IF
3246 cover_find (sgRestart%),(0),(sgCompare%),(0)
3247 IF covFound% THEN
3248 sgProposal% = 1
3249           FOR sgI%=0 TO gN%-1
3250 gWalk%(sgI%)=gBestWalk%(sgI%)
3251 gWalkPos%(gWalk%(sgI%))=sgI%
3252           END FOR sgI%
3253         END IF
3254       ELSE
3255         FOR sgI%=0 TO gN%-1
3256 gWalk%(sgI%)=selWalk%(sgI%)
3257 gWalkPos%(gWalk%(sgI%))=sgI%
3258         END FOR sgI%
3259 sgMoves% = 1+rng_next%((4))
3260         FOR sgMove%=1 TO sgMoves%
3261 cover_backbite
3262         END FOR sgMove%
3263 cover_analyse
3264 IF covScore%<=OPT_PAIRS% THEN
3265 cover_partition
3266 sgProposal% = covFeasible%
3267 END IF
3268       END IF
3269 IF sgProposal% THEN
3270 gen_validate
3271 IF NOT gValid% THEN
3272 gExit% = 1
3273 gError$ = "Internal full-coverage witness validation failed."
3274 EXIT sgGuided
3275 END IF
3276 selection_consider
3277 IF selDistinct% AND (NOT sgCurrent% OR selection_compare%((1))>=0) THEN
3278 sgCompare% = selection_compare%((1))
3279 IF sgCurrent% AND sgCompare%>0 THEN
3280 sgStagnant% = 0
3281           ELSE
3282 sgStagnant% = sgStagnant%+1
3283           END IF
3284 IF NOT sgStructured% OR selScore(0,0)=OPT_TARGET% THEN
3285 sgCurrent% = 1
3286             FOR sgI%=0 TO gN%-1
3287               selWalk%(sgI%)=gWalk%(sgI%)
3288             END FOR sgI%
3289             FOR sgI%=0 TO 4
3290               selScore(1,sgI%)=selScore(0,sgI%)
3291             END FOR sgI%
3292           END IF
3293         ELSE
3294 sgStagnant% = sgStagnant%+1
3295         END IF
3296       ELSE
3297 sgStagnant% = sgStagnant%+1
3298       END IF
3299 gen_poll
3300     END REPeat sgGuided
3301   ELSE
3302     REPeat sgUnguided
3303 IF selAccepted% OR selConsidered%>=OPT_CANDIDATES% OR gExit% OR gCancel% THEN
3304 EXIT sgUnguided
3305 END IF
3306 sgCandidateSeed = sgSeed
3307 IF selConsidered% THEN
3308 gRngStream% = 2
3309 sgCandidateSeed = rng_raw
3310 gRngStream% = 0
3311 END IF
3312 selConsidered% = selConsidered%+1
3313 generation_base (sgCandidateSeed)
3314 IF gSuccess% THEN
3315 selection_consider
3316 END IF
3317     END REPeat sgUnguided
3318   END IF
3319 IF selAccepted% AND NOT gCancel% AND NOT gExit% THEN
3320 selection_restore
3321 gen_validate
3322 gSuccess% = gValid%
3323 IF NOT gValid% THEN
3324 gExit% = 1
3325 gError$ = "Internal selected witness validation failed."
3326 END IF
3327   ELSE
3328 gSuccess% = 0
3329   END IF
3330 END DEFine
3331 DIM cliArgs$(63,4096)
3332 DIM cliNames$(24,32)
3333 DIM cliSeen%(24)
3334 DIM rampPairs%(4)
3335 DIM rampMax%(4)
3336 DIM rampAttempts%(4)
3337 DIM rampCounts%(4)
3338 DIM rampBudget(4)
3339 DIM rampRawMax(4)
3340 cliCount% = 0
3341 cliHelp% = 0
3342 cliNumber = 0
3343 gRawMaximum = 16
3344 rampBaseTrials = 0
3345 rampBaseDepth% = 0
3346 rampBaseNodes% = 0
3347 rampBaseMinimum% = 4
3348 cliNames$(0)="--width"
3349 cliNames$(1)="--height"
3350 cliNames$(2)="--pairs"
3351 cliNames$(3)="--min-length"
3352 cliNames$(4)="--max-length"
3353 cliNames$(5)="--seed"
3354 cliNames$(6)="--count"
3355 cliNames$(7)="--max-attempts"
3356 cliNames$(8)="--candidates"
3357 cliNames$(9)="--improve-steps"
3358 cliNames$(10)="--solver-work"
3359 cliNames$(11)="--min-trials"
3360 cliNames$(12)="--max-trials"
3361 cliNames$(13)="--min-depth"
3362 cliNames$(14)="--min-search-nodes"
3363 cliNames$(15)="--mode"
3364 cliNames$(16)="--cut-style"
3365 cliNames$(17)="--difficulty"
3366 cliNames$(18)="--ramp"
3367 cliNames$(19)="--output"
3368 cliNames$(20)="--show-solution"
3369 cliNames$(21)="--force"
3370 cliNames$(22)="--assess"
3371 cliNames$(23)="--require-unique"
3372 cliNames$(24)="--avoid-shortest"
3373 DEFine FuNction CLI_EQUAL%(CEA$,CEB$)
3374 LOCal ceI%
3375 IF LEN(ceA$)<>LEN(ceB$) THEN
3376 RETurn 0
3377 END IF
3378   FOR ceI%=1 TO LEN(ceA$)
3379 IF CODE(ceA$(ceI%))<>CODE(ceB$(ceI%)) THEN
3380 RETurn 0
3381 END IF
3382   END FOR ceI%
3383 RETurn 1
3384 END DEFine
3385 DEFine PROCedure CLI_ERROR(CEMESSAGE$)
3386 gExit% = 2
3387 gError$ = ceMessage$
3388 END DEFine
3389 DEFine PROCedure CLI_TOKENS(CTINPUT$)
3390 LOCal ctAt%
3391 LOCal ctCode%
3392 LOCal ctQuote%
3393 LOCal ctStarted%
3394 LOCal ctText$(4096)
3395 cliCount% = 0
3396 ctText$ = ""
3397 ctQuote% = 0
3398 ctStarted% = 0
3399   FOR ctAt%=1 TO LEN(ctInput$)
3400 ctCode% = CODE(ctInput$(ctAt%))
3401 IF ctQuote% THEN
3402 IF ctCode%=ctQuote% THEN
3403 ctQuote% = 0
3404       ELSE
3405 ctText$ = ctText$ & CHR$(ctCode%)
3406       END IF
3407     ELSE
3408 IF ctCode%=34 OR ctCode%=39 THEN
3409 ctQuote% = ctCode%
3410 ctStarted% = 1
3411       ELSE
3412 IF ctCode%=32 OR ctCode%=9 OR ctCode%=10 OR ctCode%=13 THEN
3413 IF ctStarted% THEN
3414 IF cliCount%>=64 THEN
3415 cli_error ("Too many arguments.")
3416 RETurn 
3417 END IF
3418 cliArgs$(cliCount%)=ctText$
3419 cliCount% = cliCount%+1
3420 ctText$ = ""
3421 ctStarted% = 0
3422           END IF
3423         ELSE
3424 ctText$ = ctText$ & CHR$(ctCode%)
3425 ctStarted% = 1
3426         END IF
3427       END IF
3428     END IF
3429   END FOR ctAt%
3430 IF ctQuote% THEN
3431 cli_error ("Unclosed quote in option string.")
3432 RETurn 
3433 END IF
3434 IF ctStarted% THEN
3435 IF cliCount%>=64 THEN
3436 cli_error ("Too many arguments.")
3437 RETurn 
3438 END IF
3439 cliArgs$(cliCount%)=ctText$
3440 cliCount% = cliCount%+1
3441   END IF
3442 END DEFine
3443 DEFine FuNction CLI_INTEGER%(CITEXT$)
3444 LOCal ciAt%
3445 LOCal ciSign%
3446 LOCal ciCode%
3447 LOCal ciLimit
3448 cliNumber = 0
3449 ciAt% = 1
3450 ciSign% = 1
3451 ciLimit = 2147483647
3452 IF LEN(ciText$)=0 THEN
3453 RETurn 0
3454 END IF
3455 IF ciText$(1)="-" THEN
3456 ciSign% = -1
3457 ciAt% = 2
3458 ciLimit = 2147483648
3459 END IF
3460 IF ciText$(1)="+" THEN
3461 ciAt% = 2
3462 END IF
3463 IF ciAt%>LEN(ciText$) THEN
3464 RETurn 0
3465 END IF
3466   FOR ciAt%=ciAt% TO LEN(ciText$)
3467 ciCode% = CODE(ciText$(ciAt%))-48
3468 IF ciCode%<0 OR ciCode%>9 THEN
3469 RETurn 0
3470 END IF
3471 IF cliNumber>INT((ciLimit-ciCode%)/10) THEN
3472 RETurn 0
3473 END IF
3474 cliNumber = cliNumber*10+ciCode%
3475   END FOR ciAt%
3476 cliNumber = cliNumber*ciSign%
3477 RETurn 1
3478 END DEFine
3479 DEFine FuNction CLI_GRADE%(CGTEXT$)
3480 LOCal cgI%
3481   FOR cgI%=1 TO 4
3482 IF cli_equal%((cgText$),(gen_grade$((cgI%)))) THEN
3483 RETurn cgI%
3484 END IF
3485   END FOR cgI%
3486 RETurn 0
3487 END DEFine
3488 DEFine PROCedure CLI_VALIDATE
3489 IF OPT_MAXIMUM%<OPT_MINIMUM% THEN
3490 cli_error ("Maximum length must not be less than minimum length.")
3491 RETurn 
3492 END IF
3493 IF gen_product((OPT_PAIRS%),(OPT_MINIMUM%))>OPT_WIDTH%*OPT_HEIGHT% THEN
3494 cli_error ("Pairs and minimum lengths do not fit the board.")
3495 RETurn 
3496 END IF
3497 IF OPT_FULL% THEN
3498 IF gen_product((OPT_PAIRS%),(OPT_MAXIMUM%))<OPT_WIDTH%*OPT_HEIGHT% THEN
3499 cli_error ("Full coverage needs sufficient pairs and maximum length.")
3500 RETurn 
3501 END IF
3502 IF OPT_PAIRS%=1 AND OPT_WIDTH%>1 AND OPT_HEIGHT%>1 THEN
3503 cli_error ("One induced path cannot fill a two-dimensional rectangle.")
3504 RETurn 
3505 END IF
3506   ELSE
3507 IF OPT_VARIED% THEN
3508 cli_error ("--cut-style varied requires full-coverage.")
3509 RETurn 
3510 END IF
3511 IF OPT_TARGET%>0 THEN
3512 cli_error ("Reasoning targets require full-coverage; use --avoid-shortest for connections-only.")
3513 RETurn 
3514 END IF
3515   END IF
3516 IF OPT_SHORTEST% AND OPT_FULL% THEN
3517 cli_error ("--avoid-shortest requires connect-only.")
3518 RETurn 
3519 END IF
3520 IF OPT_MAXTRIALS>=0 AND (OPT_MAXTRIALS<OPT_MINTRIALS OR OPT_TARGET%=0) THEN
3521 cli_error ("--max-trials needs an explicit target and must be at least --min-trials.")
3522 RETurn 
3523 END IF
3524 IF OPT_MINTRIALS>0 AND OPT_TARGET%<3 THEN
3525 cli_error ("--min-trials requires hard or expert.")
3526 RETurn 
3527 END IF
3528 IF (OPT_MINDEPTH%>0 OR OPT_MINNODES%>0) AND OPT_TARGET%<>4 THEN
3529 cli_error ("Depth and node floors require expert.")
3530 RETurn 
3531 END IF
3532 IF OPT_TARGET%>0 OR OPT_UNIQUE% THEN
3533 IF OPT_WIDTH%>16 OR OPT_HEIGHT%>16 OR OPT_PAIRS%>64-(NOT OPT_FULL%) THEN
3534 cli_error ("Solver selection supports up to 16x16 and 64 full/63 connect-only pairs.")
3535 END IF
3536   END IF
3537 IF OPT_FORCE% AND LEN(OPT_OUTPUT$)=0 THEN
3538 cli_error ("--force requires --output.")
3539 END IF
3540 END DEFine
3541 DEFine PROCedure CLI_RAMP
3542 LOCal crI%
3543 LOCal crIndex%
3544 LOCal crSections%
3545 LOCal crArea
3546 LOCal crP%
3547 LOCal crSide
3548 LOCal crPairs
3549 LOCal crMaximum
3550 IF NOT OPT_FULL% OR OPT_WIDTH%>16 OR OPT_HEIGHT%>16 THEN
3551 cli_error ("--ramp requires full coverage on a board up to 16x16.")
3552 RETurn 
3553 END IF
3554 IF cliSeen%(2) OR cliSeen%(17) THEN
3555 cli_error ("--ramp chooses pairs and targets; do not specify --pairs or --difficulty.")
3556 RETurn 
3557 END IF
3558 IF NOT cliSeen%(6) THEN
3559 OPT_COUNT%=12
3560 END IF
3561 crSections% = OPT_RAMPLAST%-OPT_RAMPFIRST%+1
3562 IF OPT_COUNT%<crSections% THEN
3563 cli_error ("Ramp count must include every requested grade.")
3564 RETurn 
3565 END IF
3566 IF OPT_MINTRIALS>0 AND OPT_RAMPLAST%<3 THEN
3567 cli_error ("Trial floor requires a hard/expert ramp section.")
3568 RETurn 
3569 END IF
3570 IF (OPT_MINDEPTH%>0 OR OPT_MINNODES%>0) AND OPT_RAMPLAST%<4 THEN
3571 cli_error ("Depth/node floors require an expert ramp section.")
3572 RETurn 
3573 END IF
3574 crArea = OPT_WIDTH%*OPT_HEIGHT%
3575 crSide = SQRT(crArea)
3576 rampBaseTrials = OPT_MINTRIALS
3577 rampBaseDepth% = OPT_MINDEPTH%
3578 rampBaseNodes% = OPT_MINNODES%
3579 rampBaseMinimum% = OPT_MINIMUM%
3580   FOR crI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
3581     SELect ON crI%
3582       =1
3583 crPairs = crArea/6
3584 IF crArea<=64 THEN
3585 crPairs = 1.25*crSide
3586 END IF
3587 =2:
3588 crPairs = crArea/7.2
3589 =3:
3590 crPairs = crArea/8
3591 =4:
3592 crPairs = 0.5*crSide
3593     END SELect
3594 crP% = gen_max((2),(gen_min((INT(crArea/gen_max((2),(OPT_MINIMUM%)))),(gen_round((crPairs))))))
3595     rampPairs%(crI%)=crP%
3596 crMaximum = gen_max((16),(2*(-INT(-crSide))))
3597 IF crI%=4 THEN
3598 crMaximum = gen_max((16),(2*(-INT(-crArea/crP%))))
3599 END IF
3600 IF cliSeen%(4) THEN
3601 crMaximum = gRawMaximum
3602 END IF
3603 rampRawMax(crI%)=crMaximum
3604 rampMax%(crI%)=gen_min((4096),(crMaximum))
3605 rampAttempts%(crI%)=500
3606 rampBudget(crI%)=2000000
3607 IF crArea>64 THEN
3608 rampAttempts%(crI%)=5000
3609 rampBudget(crI%)=20000000
3610 END IF
3611 IF crArea>144 THEN
3612 rampAttempts%(crI%)=10000
3613 rampBudget(crI%)=100000000
3614 END IF
3615 IF cliSeen%(7) THEN
3616 rampAttempts%(crI%)=OPT_ATTEMPTS%
3617 END IF
3618 IF cliSeen%(10) THEN
3619 rampBudget(crI%)=OPT_SOLVERWORK
3620 END IF
3621 crIndex% = crI%-OPT_RAMPFIRST%
3622 rampCounts%(crI%)=INT(OPT_COUNT%/crSections%)
3623 IF crIndex%<OPT_COUNT% MOD crSections% THEN
3624 rampCounts%(crI%)=rampCounts%(crI%)+1
3625 END IF
3626   END FOR crI%
3627   FOR crI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
3628 cli_section (crI%)
3629 cli_validate
3630 IF gExit% THEN
3631 RETurn 
3632 END IF
3633   END FOR crI%
3634 END DEFine
3635 DEFine PROCedure CLI_SECTION(CSGRADE%)
3636 OPT_TARGET%=csGrade%
3637 OPT_PAIRS%=rampPairs%(csGrade%)
3638 OPT_MAXIMUM%=rampMax%(csGrade%)
3639 gRawMaximum = rampRawMax(csGrade%)
3640 OPT_ATTEMPTS%=rampAttempts%(csGrade%)
3641 OPT_SOLVERWORK=rampBudget(csGrade%)
3642 OPT_MINTRIALS=0
3643 OPT_MINDEPTH%=0
3644 OPT_MINNODES%=0
3645 IF csGrade%>=3 THEN
3646 OPT_MINTRIALS=rampBaseTrials
3647 END IF
3648 IF csGrade%=4 THEN
3649 OPT_MINDEPTH%=rampBaseDepth%
3650 OPT_MINNODES%=rampBaseNodes%
3651 END IF
3652 END DEFine
3653 DEFine PROCedure CLI_PARSE(CPINPUT$)
3654 LOCal cpI%
3655 LOCal cpJ%
3656 LOCal cpFound%
3657 LOCal cpKey%
3658 LOCal cpValue$(4096)
3659 LOCal cpRange%
3660 LOCal cpFirst$(16)
3661 LOCal cpLast$(16)
3662 rampBaseTrials = 0
3663 rampBaseDepth% = 0
3664 rampBaseNodes% = 0
3665 gen_defaults
3666 gRawMaximum = 16
3667 OPT_SEED=gen_mod((DATE),(2147483647))
3668 gExit% = 0
3669 gError$ = ""
3670 cliHelp% = 0
3671 cli_tokens (cpInput$)
3672 IF gExit% THEN
3673 RETurn 
3674 END IF
3675   FOR cpI%=0 TO cliCount%-1
3676 IF cli_equal%((cliArgs$(cpI%)),("--help")) OR cli_equal%((cliArgs$(cpI%)),("-h")) THEN
3677 cliHelp% = 1
3678 RETurn 
3679 END IF
3680   END FOR cpI%
3681   FOR cpI%=0 TO 24
3682     cliSeen%(cpI%)=0
3683   END FOR cpI%
3684 cpI% = 0
3685   REPeat cpParse
3686 IF cpI%>=cliCount% OR gExit% THEN
3687 EXIT cpParse
3688 END IF
3689 cpKey% = -1
3690     FOR cpJ%=0 TO 24
3691 IF cli_equal%((cliArgs$(cpI%)),(cliNames$(cpJ%))) THEN
3692 cpKey% = cpJ%
3693 EXIT cpJ%
3694 END IF
3695     END FOR cpJ%
3696 IF cpKey%<0 THEN
3697 cli_error ("Unknown option. Use --help.")
3698 RETurn 
3699 END IF
3700 IF cliSeen%(cpKey%) THEN
3701 cli_error ("Option specified more than once.")
3702 RETurn 
3703 END IF
3704     cliSeen%(cpKey%)=1
3705 IF cpKey%<20 THEN
3706 cpI% = cpI%+1
3707 IF cpI%>=cliCount% THEN
3708 cli_error ("Missing option value.")
3709 RETurn 
3710 END IF
3711 cpValue$ = cliArgs$(cpI%)
3712     END IF
3713 IF cpKey%<15 THEN
3714 IF NOT cli_integer%((cpValue$)) THEN
3715 cli_error ("Expected a 32-bit integer option value.")
3716 RETurn 
3717 END IF
3718     END IF
3719     SELect ON cpKey%
3720       =0
3721 IF cliNumber<1 OR cliNumber>64 THEN
3722 cli_error ("Out of range: --width (1..64).")
3723 RETurn 
3724 END IF
3725         OPT_WIDTH%=cliNumber
3726       =1
3727 IF cliNumber<1 OR cliNumber>64 THEN
3728 cli_error ("Out of range: --height (1..64).")
3729 RETurn 
3730 END IF
3731         OPT_HEIGHT%=cliNumber
3732       =2
3733 IF cliNumber<1 OR cliNumber>2048 THEN
3734 cli_error ("Out of range: --pairs (1..2048).")
3735 RETurn 
3736 END IF
3737         OPT_PAIRS%=cliNumber
3738       =3
3739 IF cliNumber<2 OR cliNumber>4096 THEN
3740 cli_error ("Out of range: --min-length (2..4096).")
3741 RETurn 
3742 END IF
3743         OPT_MINIMUM%=cliNumber
3744       =4
3745 IF cliNumber<2 OR cliNumber>2147483647 THEN
3746 cli_error ("Out of range: --max-length (2..2147483647).")
3747 RETurn 
3748 END IF
3749 gRawMaximum = cliNumber
3750 OPT_MAXIMUM%=gen_min((4096),(cliNumber))
3751       =5
3752 IF cliNumber<0 OR cliNumber>2147483647 THEN
3753 cli_error ("Out of range: --seed (0..2147483647).")
3754 RETurn 
3755 END IF
3756         OPT_SEED=cliNumber
3757       =6
3758 IF cliNumber<1 OR cliNumber>10000 THEN
3759 cli_error ("Out of range: --count (1..10000).")
3760 RETurn 
3761 END IF
3762         OPT_COUNT%=cliNumber
3763       =7
3764 IF cliNumber<1 OR cliNumber>10000 THEN
3765 cli_error ("Out of range: --max-attempts (1..10000).")
3766 RETurn 
3767 END IF
3768         OPT_ATTEMPTS%=cliNumber
3769       =8
3770 IF cliNumber<1 OR cliNumber>10000 THEN
3771 cli_error ("Out of range: --candidates (1..10000).")
3772 RETurn 
3773 END IF
3774         OPT_CANDIDATES%=cliNumber
3775       =9
3776 IF cliNumber<0 OR cliNumber>10000 THEN
3777 cli_error ("Out of range: --improve-steps (0..10000).")
3778 RETurn 
3779 END IF
3780         OPT_IMPROVE%=cliNumber
3781       =10
3782 IF cliNumber<1 OR cliNumber>100000000 THEN
3783 cli_error ("Out of range: --solver-work (1..100000000).")
3784 RETurn 
3785 END IF
3786         OPT_SOLVERWORK=cliNumber
3787       =11
3788 IF cliNumber<0 OR cliNumber>1000000 THEN
3789 cli_error ("Out of range: --min-trials (0..1000000).")
3790 RETurn 
3791 END IF
3792         OPT_MINTRIALS=cliNumber
3793       =12
3794 IF cliNumber<0 OR cliNumber>1000000 THEN
3795 cli_error ("Out of range: --max-trials (0..1000000).")
3796 RETurn 
3797 END IF
3798         OPT_MAXTRIALS=cliNumber
3799       =13
3800 IF cliNumber<0 OR cliNumber>256 THEN
3801 cli_error ("Out of range: --min-depth (0..256).")
3802 RETurn 
3803 END IF
3804         OPT_MINDEPTH%=cliNumber
3805       =14
3806 IF cliNumber<0 OR cliNumber>20000 THEN
3807 cli_error ("Out of range: --min-search-nodes (0..20000).")
3808 RETurn 
3809 END IF
3810         OPT_MINNODES%=cliNumber
3811       =15
3812 IF NOT cli_equal%((cpValue$),("connect-only")) AND NOT cli_equal%((cpValue$),("full-coverage")) THEN
3813 cli_error ("Mode must be connect-only or full-coverage.")
3814 RETurn 
3815 END IF
3816         OPT_FULL%=cli_equal%((cpValue$),("full-coverage"))
3817       =16
3818 IF NOT cli_equal%((cpValue$),("random")) AND NOT cli_equal%((cpValue$),("varied")) THEN
3819 cli_error ("Cut style must be random or varied.")
3820 RETurn 
3821 END IF
3822         OPT_VARIED%=cli_equal%((cpValue$),("varied"))
3823       =17
3824         OPT_TARGET%=cli_grade%((cpValue$))
3825 IF NOT OPT_TARGET% AND NOT cli_equal%((cpValue$),("any")) THEN
3826 cli_error ("Difficulty must be any/easy/medium/hard/expert.")
3827 RETurn 
3828 END IF
3829       =18
3830 cpRange% = ".." INSTR cpValue$
3831 IF cpRange%<2 OR cpRange%+2>LEN(cpValue$) THEN
3832 cli_error ("Ramp must ascend, e.g. easy..hard.")
3833 RETurn 
3834 END IF
3835 cpFirst$ = cpValue$(1 TO cpRange%-1)
3836 cpLast$ = cpValue$(cpRange%+2 TO)
3837 OPT_RAMPFIRST%=cli_grade%((cpFirst$))
3838 OPT_RAMPLAST%=cli_grade%((cpLast$))
3839 IF OPT_RAMPFIRST%<1 OR OPT_RAMPLAST%<=OPT_RAMPFIRST% THEN
3840 cli_error ("Ramp must ascend, e.g. easy..hard.")
3841 RETurn 
3842 END IF
3843       =19
3844 IF LEN(cpValue$)=0 OR LEN(cpValue$)>200 THEN
3845 cli_error ("Output must be a nonempty QL filename of at most 200 bytes.")
3846 RETurn 
3847 END IF
3848         OPT_OUTPUT$=cpValue$
3849       =20
3850         OPT_SHOW%=1
3851       =21
3852         OPT_FORCE%=1
3853       =22
3854         OPT_ASSESS%=1
3855       =23
3856         OPT_UNIQUE%=1
3857       =24
3858         OPT_SHORTEST%=1
3859     END SELect
3860 cpI% = cpI%+1
3861   END REPeat cpParse
3862 IF gExit% THEN
3863 RETurn 
3864 END IF
3865 IF OPT_RAMPFIRST% THEN
3866 cli_ramp
3867   ELSE
3868 cli_validate
3869   END IF
3870 END DEFine
3871 DEFine PROCedure CLI_HELP
3872 PRINT #0,"Tangles native generator / SSB 2.8 and Turbo 5.09"
3873 PRINT #0,"EW win1_generator_exe;";CHR$(34);"--seed 42 --output win1_pack_json";CHR$(34)
3874 PRINT #0,"--mode connect-only|full-coverage   --width N --height N (1..64)"
3875 PRINT #0,"--pairs N --min-length N --max-length N --max-attempts N"
3876 PRINT #0,"--seed N (0..2147483647) --count N (1..10000)"
3877 PRINT #0,"--cut-style random|varied --difficulty any|easy|medium|hard|expert"
3878 PRINT #0,"--ramp easy..hard (ascending grades; default count 12)"
3879 PRINT #0,"--candidates N --improve-steps N --solver-work N"
3880 PRINT #0,"--min-trials N --max-trials N --min-depth N --min-search-nodes N"
3881 PRINT #0,"--assess --require-unique --avoid-shortest --show-solution"
3882 PRINT #0,"--output QL_FILENAME --force --help|-h"
3883 PRINT #0,"Defaults: 8x8, six pairs, connect-only, lengths 4..16, 500 attempts."
3884 PRINT #0,"Orthogonal, disjoint induced paths. Full coverage fills every square."
3885 PRINT #0,"Solver targets are exact, full-coverage only, and require a unique proof."
3886 PRINT #0,"Easy=local, medium=routes, hard=trials, expert=branching."
3887 PRINT #0,"Budgets are deterministic work units, not elapsed time."
3888 PRINT #0,"A failed/budget-limited search never proves impossibility or difficulty."
3889 PRINT #0,"All stored solutions are validated. Grades remain provisional."
3890 PRINT #0,"Escape/Ctrl+C cancels. Native seed streams are versioned; see README."
3891 END DEFine
3892 DIM jsonFirst%(16)
3893 DIM jsonEscaped$(1024)
3894 gJ% = -1
3895 jsonLevel% = 0
3896 DEFine PROCedure JSON_OUT(JOTEXT$)
3897 PRINT #gJ%,joText$;
3898 END DEFine
3899 DEFine FuNction JSON_QUOTE$(JQTEXT$)
3900 LOCal jqI%
3901 LOCal jqC%
3902 LOCal jqOut$(1024)
3903 LOCal jqHex$(16)
3904 jqOut$ = CHR$(34)
3905 jqHex$ = "0123456789ABCDEF"
3906   FOR jqI%=1 TO LEN(jqText$)
3907 jqC% = CODE(jqText$(jqI%))
3908 IF jqC%=34 OR jqC%=92 THEN
3909 jqOut$ = jqOut$ & CHR$(92) & CHR$(jqC%)
3910     ELSE
3911 IF jqC%<32 THEN
3912 jqOut$ = jqOut$ & CHR$(92) & "u00" & jqHex$(1+INT(jqC%/16)) & jqHex$(1+jqC% MOD 16)
3913       ELSE
3914 jqOut$ = jqOut$ & CHR$(jqC%)
3915       END IF
3916     END IF
3917   END FOR jqI%
3918 RETurn jqOut$ & CHR$(34)
3919 END DEFine
3920 DEFine PROCedure JSON_KEY(JKKEY$)
3921 IF jsonLevel%=0 THEN
3922 RETurn 
3923 END IF
3924 IF NOT jsonFirst%(jsonLevel%) THEN
3925 json_out (",")
3926 END IF
3927   jsonFirst%(jsonLevel%)=0
3928 IF LEN(jkKey$) THEN
3929 json_out (json_quote$((jkKey$)) & ":")
3930 END IF
3931 END DEFine
3932 DEFine PROCedure JSON_OBJECT(JAKEY$)
3933 json_key (jaKey$)
3934 json_out ("{")
3935 jsonLevel% = jsonLevel%+1
3936 jsonFirst%(jsonLevel%)=1
3937 END DEFine
3938 DEFine PROCedure JSON_ARRAY(JAKEY$)
3939 json_key (jaKey$)
3940 json_out ("[")
3941 jsonLevel% = jsonLevel%+1
3942 jsonFirst%(jsonLevel%)=1
3943 END DEFine
3944 DEFine PROCedure JSON_END(JECLOSE$)
3945 json_out (jeClose$)
3946 jsonLevel% = jsonLevel%-1
3947 END DEFine
3948 DEFine PROCedure JSON_STRING(JSKEY$,JSTEXT$)
3949 json_key (jsKey$)
3950 json_out (json_quote$((jsText$)))
3951 END DEFine
3952 DEFine PROCedure JSON_INTEGER(JIKEY$,JINUMBER)
3953 json_key (jiKey$)
3954 json_out (gen_int$((jiNumber)))
3955 END DEFine
3956 DEFine PROCedure JSON_NUMBER(JNKEY$,JNNUMBER)
3957 json_key (jnKey$)
3958 json_out (gen_number$((jnNumber)))
3959 END DEFine
3960 DEFine PROCedure JSON_BOOLEAN(JBKEY$,JBVALUE%)
3961 json_key (jbKey$)
3962 json_out (gen_bool$((jbValue%)))
3963 END DEFine
3964 DEFine PROCedure JSON_NULL(JZKEY$)
3965 json_key (jzKey$)
3966 json_out ("null")
3967 END DEFine
3968 DEFine PROCedure JSON_POINT(JPKEY$,JPCELL%)
3969 json_object (jpKey$)
3970 json_integer ("x"),(jpCell% MOD OPT_WIDTH%)
3971 json_integer ("y"),(INT(jpCell%/OPT_WIDTH%))
3972 json_end ("}")
3973 END DEFine
3974 DEFine PROCedure JSON_RECIPE
3975 json_integer ("pairCount"),(OPT_PAIRS%)
3976 json_integer ("minPathLength"),(OPT_MINIMUM%)
3977 json_integer ("maxPathLength"),(gRawMaximum)
3978 json_integer ("maxAttempts"),(OPT_ATTEMPTS%)
3979 json_string ("cutStyle"),(gen_cut$)
3980 END DEFine
3981 DEFine PROCedure JSON_REASONING
3982 json_string ("method"),("cell-domain-reasoning-v1")
3983 json_string ("boardKey"),(SR_BOARDKEY$)
3984 json_string ("mode"),(gen_mode$)
3985 json_string ("status"),(SR_STATUS$)
3986 json_string ("limitReason"),(SR_LIMITREASON$)
3987 json_string ("tier"),(SR_TIER$)
3988 json_string ("grade"),(SR_GRADE$)
3989 json_integer ("score"),(SR_SCORE%)
3990 json_boolean ("provisional"),(1)
3991 json_boolean ("solvedByReasoning"),(SR_LOGICAL%)
3992 json_integer ("localDeductions"),(SR_LOCALDEDUCTIONS)
3993 json_integer ("routeDeductions"),(SR_ROUTEDEDUCTIONS)
3994 json_integer ("routePasses"),(SR_ROUTEPASSES)
3995 json_integer ("trialSteps"),(SR_TRIALSTEPS)
3996 json_integer ("trialAssumptions"),(SR_TRIALASSUMPTIONS)
3997 json_integer ("searchNodes"),(SR_SEARCHNODES)
3998 json_integer ("firstSolutionSearchNodes"),(SR_FIRSTNODES)
3999 json_integer ("firstSolutionBranchDepth"),(SR_FIRSTDEPTH%)
4000 json_integer ("maxSearchDepth"),(SR_MAXDEPTH%)
4001 json_integer ("solutionCount"),(SR_SOLUTIONCOUNT%)
4002 json_integer ("work"),(SR_WORK)
4003 json_integer ("workLimit"),(SR_WORKLIMIT)
4004 json_integer ("solutionCountLimit"),(2)
4005 json_boolean ("solutionCountComplete"),(SR_EXHAUSTIVE%)
4006 json_string ("uniqueness"),(SR_UNIQUENESS$)
4007 END DEFine
4008 DEFine PROCedure JSON_GEOMETRY_COMPONENTS
4009 LOCal jgI%
4010 json_array ("components")
4011   FOR jgI%=0 TO geoCount%-1
4012 json_object ("")
4013 json_string ("name"),(geoNames$(jgI%))
4014 json_number ("value"),(geoValues(jgI%))
4015 json_integer ("weight"),(geoWeights%(jgI%))
4016 json_number ("points"),(geoPoints(jgI%))
4017 json_end ("}")
4018   END FOR jgI%
4019 json_end ("]")
4020 END DEFine
4021 DEFine PROCedure JSON_THRESHOLDS(JTREASON%)
4022 json_object ("thresholds")
4023 IF jtReason% THEN
4024 json_integer ("easyBelow"),(30)
4025 json_integer ("hardAtLeast"),(55)
4026   ELSE
4027 json_integer ("easyBelow"),(geoEasy%)
4028 json_integer ("hardAtLeast"),(geoHard%)
4029   END IF
4030 json_end ("}")
4031 END DEFine
4032 DEFine FuNction GEOMETRY_GRADE$
4033 IF geoScore%<geoEasy% THEN
4034 RETurn "easy"
4035 END IF
4036 IF geoScore%<geoHard% THEN
4037 RETurn "medium"
4038 END IF
4039 RETurn "hard"
4040 END DEFine
4041 DEFine FuNction REASONING_PRIMARY%
4042 RETurn (gHasReason% AND OPT_FULL% AND SR_STATUS$="complete" AND SR_GRADE$<>"unrated")
4043 END DEFine
4044 DEFine PROCedure JSON_DIFFICULTY
4045 LOCal jdI%
4046 LOCal jdP%
4047 LOCal jdBase%
4048 LOCal jdBonus$(32)
4049 json_object ("difficulty")
4050 IF reasoning_primary% THEN
4051 json_string ("method"),("cell-domain-reasoning-v1")
4052 json_string ("profile"),("full-coverage-reasoning-v1")
4053 json_string ("grade"),(SR_GRADE$)
4054 json_integer ("score"),(SR_SCORE%)
4055 json_integer ("rawScore"),(SR_SCORE%)
4056   ELSE
4057 json_string ("method"),("geometric-heuristic-v1")
4058 json_string ("profile"),(gen_mode$ & "-v1")
4059 json_string ("grade"),(geometry_grade$)
4060 json_integer ("score"),(geoScore%)
4061 json_number ("rawScore"),(geoRaw)
4062   END IF
4063 json_string ("mode"),(gen_mode$)
4064 json_boolean ("provisional"),(1)
4065 json_thresholds (reasoning_primary%)
4066 json_object ("measurements")
4067 json_number ("boardCellCount"),(GM_BOARDCELLCOUNT)
4068 json_number ("pairCount"),(GM_PAIRCOUNT)
4069 json_number ("endpointCount"),(GM_ENDPOINTCOUNT)
4070 json_number ("cornerEndpointCount"),(GM_CORNERENDPOINTCOUNT)
4071 json_number ("edgeEndpointCount"),(GM_EDGEENDPOINTCOUNT)
4072 json_number ("meanCornerDistance"),(GM_MEANCORNERDISTANCE)
4073 json_number ("meanEdgeDistance"),(GM_MEANEDGEDISTANCE)
4074 json_number ("meanAvailableExits"),(GM_MEANAVAILABLEEXITS)
4075 json_number ("singleExitEndpointCount"),(GM_SINGLEEXITENDPOINTCOUNT)
4076 json_number ("adjacentPairCount"),(GM_ADJACENTPAIRCOUNT)
4077 json_number ("alignedPairCount"),(GM_ALIGNEDPAIRCOUNT)
4078 json_number ("meanManhattanDistance"),(GM_MEANMANHATTANDISTANCE)
4079 json_number ("meanShortestDistance"),(GM_MEANSHORTESTDISTANCE)
4080 json_number ("minimumRequiredCells"),(GM_MINIMUMREQUIREDCELLS)
4081 json_number ("minimumCoverageRatio"),(GM_MINIMUMCOVERAGERATIO)
4082 json_number ("coverageSlackCells"),(GM_COVERAGESLACKCELLS)
4083 json_number ("requiredExtraCoverageCells"),(GM_REQUIREDEXTRACOVERAGECELLS)
4084 json_number ("forcedCornerTurnCount"),(GM_FORCEDCORNERTURNCOUNT)
4085 json_number ("meanLog2ShortestRouteCount"),(GM_MEANLOG2SHORTESTROUTECOUNT)
4086 json_number ("shortestRouteCountLimit"),(GM_SHORTESTROUTECOUNTLIMIT)
4087 json_number ("cappedRoutePairCount"),(GM_CAPPEDROUTEPAIRCOUNT)
4088 json_number ("routeInteriorOccurrences"),(GM_ROUTEINTERIOROCCURRENCES)
4089 json_number ("sharedRouteInteriorOccurrences"),(GM_SHAREDROUTEINTERIOROCCURRENCES)
4090 json_number ("routeOverlapRatio"),(GM_ROUTEOVERLAPRATIO)
4091 json_number ("maxRouteOverlap"),(GM_MAXROUTEOVERLAP)
4092 json_number ("crossLabelEndpointAdjacencies"),(GM_CROSSLABELENDPOINTADJACENCIES)
4093 json_number ("clueCrowdingRatio"),(GM_CLUECROWDINGRATIO)
4094 json_end ("}")
4095 json_array ("pairMeasurements")
4096   FOR jdP%=1 TO OPT_PAIRS%
4097 json_object ("")
4098 json_string ("label"),(gen_label$((geoMap%(jdP%))))
4099 json_integer ("manhattanDistance"),(GEOPAIR_MANHATTANDISTANCE%(jdP%))
4100 json_integer ("shortestDistance"),(GEOPAIR_SHORTESTDISTANCE%(jdP%))
4101 json_integer ("shortestRouteCount"),(GEOPAIR_SHORTESTROUTECOUNT%(jdP%))
4102 json_boolean ("shortestRouteCountCapped"),(GEOPAIR_SHORTESTROUTECOUNTCAPPED%(jdP%))
4103 json_integer ("minEndpointExits"),(GEOPAIR_MINENDPOINTEXITS%(jdP%))
4104 json_integer ("maxEndpointExits"),(GEOPAIR_MAXENDPOINTEXITS%(jdP%))
4105 json_integer ("shortestRouteInteriorCells"),(GEOPAIR_SHORTESTROUTEINTERIORCELLS%(jdP%))
4106 json_integer ("sharedRouteInteriorCells"),(GEOPAIR_SHAREDROUTEINTERIORCELLS%(jdP%))
4107 json_end ("}")
4108   END FOR jdP%
4109 json_end ("]")
4110 IF reasoning_primary% THEN
4111 jdBase% = 80
4112 jdBonus$ = "searchNodeBonus"
4113 IF SR_TIER$="local" THEN
4114 jdBase% = 10
4115 jdBonus$ = "localDeductionBonus"
4116 END IF
4117 IF SR_TIER$="routes" THEN
4118 jdBase% = 30
4119 jdBonus$ = "routeDeductionBonus"
4120 END IF
4121 IF SR_TIER$="trial" THEN
4122 jdBase% = 55
4123 jdBonus$ = "trialStepBonus"
4124 END IF
4125 json_array ("components")
4126 json_object ("")
4127 json_string ("name"),("reasoningTier")
4128 json_integer ("value"),(1)
4129 json_integer ("weight"),(jdBase%)
4130 json_integer ("points"),(jdBase%)
4131 json_end ("}")
4132 json_object ("")
4133 json_string ("name"),(jdBonus$)
4134 json_integer ("value"),(SR_SCORE%-jdBase%)
4135 json_integer ("weight"),(1)
4136 json_integer ("points"),(SR_SCORE%-jdBase%)
4137 json_end ("}")
4138 json_end ("]")
4139 json_object ("geometricEstimate")
4140 json_string ("method"),("geometric-heuristic-v1")
4141 json_string ("profile"),(gen_mode$ & "-v1")
4142 json_string ("grade"),(geometry_grade$)
4143 json_integer ("score"),(geoScore%)
4144 json_number ("rawScore"),(geoRaw)
4145 json_thresholds (0)
4146 json_geometry_components
4147 json_end ("}")
4148   ELSE
4149 json_geometry_components
4150 json_null ("geometricEstimate")
4151   END IF
4152 IF gHasReason% THEN
4153 json_object ("reasoning")
4154 json_reasoning
4155 json_end ("}")
4156   ELSE
4157 json_null ("reasoning")
4158   END IF
4159 json_end ("}")
4160 END DEFine
4161 DEFine PROCedure JSON_SHORTEST
4162 IF NOT gHasShortest% THEN
4163 json_null ("shortestRoutes")
4164 RETurn 
4165 END IF
4166 json_object ("shortestRoutes")
4167 json_string ("method"),("greedy-shortest-routes-v1")
4168 json_integer ("trials"),(shortTrials%)
4169 json_integer ("trialLimit"),(shortLimit%)
4170 json_boolean ("solved"),(shortSolved%)
4171 IF shortFirst% THEN
4172 json_integer ("firstSuccessTrial"),(shortFirst%)
4173   ELSE
4174 json_null ("firstSuccessTrial")
4175   END IF
4176 json_end ("}")
4177 END DEFine
4178 DEFine PROCedure JSON_POLICY
4179 json_string ("policyVersion"),("symmetry-effort-v1")
4180 json_string ("target"),(gen_grade$((OPT_TARGET%)))
4181 json_integer ("candidateLimit"),(OPT_CANDIDATES%)
4182 json_integer ("improvementLimit"),(OPT_IMPROVE%)
4183 json_integer ("solverWorkLimit"),(OPT_SOLVERWORK)
4184 json_integer ("minTrials"),(OPT_MINTRIALS)
4185 IF OPT_MAXTRIALS<0 THEN
4186 json_null ("maxTrials")
4187   ELSE
4188 json_integer ("maxTrials"),(OPT_MAXTRIALS)
4189   END IF
4190 json_integer ("minBranchDepth"),(OPT_MINDEPTH%)
4191 json_integer ("minFirstSolutionNodes"),(OPT_MINNODES%)
4192 json_boolean ("assess"),(OPT_ASSESS%)
4193 json_boolean ("requireUnique"),(OPT_UNIQUE%)
4194 json_boolean ("avoidShortest"),(OPT_SHORTEST%)
4195 END DEFine
4196 DEFine PROCedure JSON_SELECTION
4197 IF NOT gSelected% THEN
4198 json_null ("selection")
4199 RETurn 
4200 END IF
4201 json_object ("selection")
4202 json_string ("algorithm"),(selAlgorithm$)
4203 json_policy
4204 json_integer ("candidatesConsidered"),(selConsidered%)
4205 json_integer ("candidatesEvaluated"),(selEvaluated%)
4206 json_integer ("traversalEvaluations"),(WORK_TRAVERSALEVALUATIONS)
4207 json_integer ("solverAssessments"),(selAssessments%)
4208 json_integer ("duplicatesRejected"),(selDuplicates%)
4209 json_integer ("budgetRejections"),(selBudgetRejected%)
4210 json_string ("startingTier"),(selStartingTier$)
4211 json_integer ("startingTrialSteps"),(selStartingTrials)
4212 json_end ("}")
4213 END DEFine
4214 DEFine PROCedure JSON_PUZZLE(JPSEED)
4215 LOCal jpP%
4216 LOCal jpI%
4217 LOCal jpJ%
4218 LOCal jpFound%
4219 LOCal jpIdentity$(72)
4220 LOCal jpVerify$(20)
4221 gen_validate
4222 IF NOT gValid% THEN
4223 gExit% = 1
4224 gError$ = "Export refused an invalid solution."
4225 RETurn 
4226 END IF
4227 IF gHasReason% THEN
4228 IF NOT cli_equal%((SR_BOARDKEY$),(identity_board$)) THEN
4229 gExit% = 1
4230 gError$ = "Solver assessment belongs to different clues."
4231 RETurn 
4232 END IF
4233   END IF
4234 IF OPT_TARGET%>0 THEN
4235 IF NOT gHasReason% OR SR_GRADE$<>gen_grade$((OPT_TARGET%)) OR NOT selection_minimums% THEN
4236 gExit% = 1
4237 gError$ = "Selected puzzle did not meet its exact grade and effort bounds."
4238 RETurn 
4239 END IF
4240   END IF
4241 IF OPT_UNIQUE% THEN
4242 IF NOT gHasReason% OR SR_UNIQUENESS$<>"unique" THEN
4243 gExit% = 1
4244 gError$ = "Selected puzzle has no uniqueness proof."
4245 RETurn 
4246 END IF
4247   END IF
4248 geo_assess
4249 IF gExit% OR gCancel% THEN
4250 RETurn 
4251 END IF
4252 jpIdentity$ = identity_puzzle$
4253 jsonLevel% = 0
4254 json_object ("")
4255 json_string ("id"),(jpIdentity$)
4256 json_integer ("width"),(OPT_WIDTH%)
4257 json_integer ("height"),(OPT_HEIGHT%)
4258 json_integer ("seed"),(jpSeed)
4259 json_integer ("attempts"),(WORK_BOARDATTEMPTS+WORK_TRAVERSALEVALUATIONS)
4260 json_object ("work")
4261 json_integer ("boardAttempts"),(WORK_BOARDATTEMPTS)
4262 json_integer ("traversalEvaluations"),(WORK_TRAVERSALEVALUATIONS)
4263 json_integer ("refinementEvaluations"),(WORK_REFINEMENTEVALUATIONS)
4264 json_integer ("structuredStarts"),(WORK_STRUCTUREDSTARTS)
4265 json_end ("}")
4266 IF OPT_RAMPFIRST% THEN
4267 json_object ("recipe")
4268 json_recipe
4269 json_end ("}")
4270   ELSE
4271 json_null ("recipe")
4272   END IF
4273 json_array ("pairs")
4274   FOR jpP%=1 TO OPT_PAIRS%
4275 json_object ("")
4276 json_string ("label"),(gen_label$((jpP%)))
4277 json_point ("start"),(gStart%(jpP%))
4278 json_point ("finish"),(gFinish%(jpP%))
4279 json_end ("}")
4280   END FOR jpP%
4281 json_end ("]")
4282 json_array ("solution")
4283   FOR jpP%=1 TO OPT_PAIRS%
4284 json_object ("")
4285 json_string ("label"),(gen_label$((jpP%)))
4286 json_array ("cells")
4287     FOR jpJ%=1 TO gLength%(jpP%)
4288 jpFound% = -1
4289       FOR jpI%=0 TO gN%-1
4290 IF gOwner%(jpI%)=jpP% AND gOrder%(jpI%)=jpJ% THEN
4291 jpFound% = jpI%
4292 EXIT jpI%
4293 END IF
4294       END FOR jpI%
4295 json_point (""),(jpFound%)
4296     END FOR jpJ%
4297 json_end ("]")
4298 json_end ("}")
4299   END FOR jpP%
4300 json_end ("]")
4301 json_difficulty
4302 json_selection
4303 json_shortest
4304 json_object ("verification")
4305 json_boolean ("solutionValid"),(1)
4306 jpVerify$ = "notChecked"
4307 IF gHasReason% THEN
4308 jpVerify$ = SR_UNIQUENESS$
4309 END IF
4310 json_string ("uniqueness"),(jpVerify$)
4311 jpVerify$ = "heuristicEstimate"
4312 IF reasoning_primary% THEN
4313 jpVerify$ = "reasoningEstimate"
4314 END IF
4315 json_string ("difficulty"),(jpVerify$)
4316 json_end ("}")
4317 json_end ("}")
4318 END DEFine
4319 DIM batchOffset(0)
4320 DIM batchLength(0)
4321 DIM batchConsoleOffset(0)
4322 DIM batchConsoleLength(0)
4323 DIM batchOrder%(0)
4324 DIM batchConsole$(220)
4325 DIM batchBase$(220)
4326 DIM batchCopy$(512)
4327 batchSpool% = -1
4328 batchConsole% = -1
4329 batchRead% = -1
4330 batchFinal% = -1
4331 batchAccepted% = 0
4332 batchRequests = 0
4333 batchRejected = 0
4334 batchDuplicates = 0
4335 batchConsumed = 0
4336 batchBudget = 0
4337 batchRequestLimit = 0
4338 batchBacked% = 0
4339 batchCommitted% = 0
4340 batchSortMemory = 0
4341 batchSortStride = 28
4342 batchDepthSort% = 0
4343 batchUseSelection% = 0
4344 batchTemporary% = 0
4345 batchRestoring% = 0
4346 batchCopyOK% = 0
4347 batchDestTouched% = 0
4348 DEFine FuNction BATCH_VALUE(BVROW%,BVCOLUMN%)
4349 RETurn PEEK_L(batchSortMemory+bvRow%*batchSortStride+4*bvColumn%)
4350 END DEFine
4351 DEFine PROCedure BATCH_METRIC(BMROW%,BMCOLUMN%,BMVALUE)
4352   POKE_L batchSortMemory+bmRow%*batchSortStride+4*bmColumn%,bmValue
4353 END DEFine
4354 DEFine FuNction BATCH_NEW$(BNBASE$)
4355 LOCal bnI%
4356 LOCal bnChannel%
4357 LOCal bnName$(220)
4358   FOR bnI%=0 TO 9999
4359 bnName$ = bnBase$ & "_" & gen_int$((bnI%))
4360 IF FTEST(bnName$)=0 THEN
4361 NEXT bnI%
4362 END IF
4363 bnChannel% = FOP_NEW(bnName$)
4364 IF bnChannel%>=0 THEN
4365 CLOSE #bnChannel%
4366 RETurn bnName$
4367 END IF
4368 IF bnChannel%<>-8 THEN
4369 gExit% = 1
4370 gError$ = "Cannot create temporary file: " & bnBase$
4371 RETurn ""
4372 END IF
4373   END FOR bnI%
4374 gExit% = 1
4375 gError$ = "Cannot reserve a unique temporary filename."
4376 RETurn ""
4377 END DEFine
4378 DEFine PROCedure BATCH_CLOSE
4379 IF batchSpool%>=0 THEN
4380 CLOSE #batchSpool%
4381 batchSpool% = -1
4382 END IF
4383 IF batchConsole%>=0 THEN
4384 CLOSE #batchConsole%
4385 batchConsole% = -1
4386 END IF
4387 IF batchRead%>=0 THEN
4388 CLOSE #batchRead%
4389 batchRead% = -1
4390 END IF
4391 IF batchFinal%>=0 THEN
4392 CLOSE #batchFinal%
4393 batchFinal% = -1
4394 END IF
4395 gJ% = -1
4396 END DEFine
4397 DEFine PROCedure BATCH_CLEANUP
4398 batch_close
4399 IF batchSortMemory THEN
4400 RECHP batchSortMemory
4401 batchSortMemory = 0
4402 END IF
4403 IF sMemory THEN
4404 RECHP sMemory
4405 sMemory = 0
4406 END IF
4407 IF covMemory THEN
4408 RECHP covMemory
4409 covMemory = 0
4410 END IF
4411 IF LEN(gSpool$) THEN
4412 IF FTEST(gSpool$)=0 THEN
4413 DELETE gSpool$
4414 END IF
4415   END IF
4416 IF LEN(batchConsole$) THEN
4417 IF FTEST(batchConsole$)=0 THEN
4418 DELETE batchConsole$
4419 END IF
4420   END IF
4421 IF LEN(gTemp$) THEN
4422 IF FTEST(gTemp$)=0 THEN
4423 DELETE gTemp$
4424 END IF
4425   END IF
4426 IF LEN(gBackup$) AND NOT batchBacked% THEN
4427 IF FTEST(gBackup$)=0 THEN
4428 DELETE gBackup$
4429 END IF
4430   END IF
4431 IF batchCommitted% AND batchBacked% THEN
4432 IF FTEST(gBackup$)=0 THEN
4433 DELETE gBackup$
4434 END IF
4435 batchBacked% = 0
4436   END IF
4437 END DEFine
4438 DEFine PROCedure BATCH_EXIT
4439 IF COMPILED THEN
4440 RJOB -1,-gExit%
4441 END IF
4442 IF JOBID<>0 THEN
4443 QUIT -gExit%
4444 END IF
4445   STOP
4446 END DEFine
4447 DEFine PROCedure BATCH_RUNTIME_ERROR(BRERROR)
4448 gExit% = 1
4449 gError$ = "Operating-system error " & gen_int$((brError)) & ". Checking staged output and any previous-file backup."
4450 batch_close
4451 IF batchBacked% AND NOT batchCommitted% THEN
4452 batchRestoring% = 1
4453 batch_clone (gBackup$),(gOutput$)
4454 batchRestoring% = 0
4455 IF batchCopyOK% THEN
4456 batchBacked% = 0
4457 END IF
4458   ELSE
4459 IF batchDestTouched% AND NOT batchCommitted% THEN
4460 IF FTEST(gOutput$)=0 THEN
4461 DELETE gOutput$
4462 END IF
4463     END IF
4464   END IF
4465 PRINT #0,gError$
4466 batch_cleanup
4467 batch_exit
4468 END DEFine
4469 DEFine PROCedure BATCH_COPY(BCFROM%,BCTO%,BCAT,BCLENGTH)
4470 LOCal bcI%
4471 LOCal bcCount%
4472 LOCal bcByte$(1)
4473   SET_POSITION #bcFrom%,bcAt
4474   REPeat bcLoop
4475 IF bcLength<=0 THEN
4476 EXIT bcLoop
4477 END IF
4478 IF NOT batchRestoring% AND (gCancel% OR gExit%) THEN
4479 EXIT bcLoop
4480 END IF
4481 bcCount% = gen_min((512),(bcLength))
4482 batchCopy$ = FILL$(" ",bcCount%)
4483     FOR bcI%=1 TO bcCount%
4484 bcByte$ = INKEY$(#bcFrom%,-1)
4485 IF LEN(bcByte$)<>1 THEN
4486 gExit% = 1
4487 gError$ = "Unexpected end of spool file."
4488 RETurn 
4489 END IF
4490       batchCopy$(bcI%)=bcByte$
4491     END FOR bcI%
4492 PRINT #bcTo%,batchCopy$;
4493 bcLength = bcLength-bcCount%
4494 IF NOT batchRestoring% THEN
4495 gen_poll
4496 END IF
4497   END REPeat bcLoop
4498 END DEFine
4499 DEFine PROCedure BATCH_CLONE(BKFROM$,BKTO$)
4500 LOCal bkLength
4501 batchCopyOK% = 0
4502 batchRead% = FOP_IN(bkFrom$)
4503 IF batchRead%<0 THEN
4504 batchRead% = -1
4505 gExit% = 1
4506 gError$ = "Cannot open publication source."
4507 RETurn 
4508 END IF
4509 bkLength = FLEN(#batchRead%)
4510 batchFinal% = FOP_OVER(bkTo$)
4511 IF batchFinal%<0 THEN
4512 CLOSE #batchRead%
4513 batchRead% = -1
4514 batchFinal% = -1
4515 gExit% = 1
4516 gError$ = "Cannot open publication destination."
4517 RETurn 
4518 END IF
4519 batch_copy (batchRead%),(batchFinal%),(0),(bkLength)
4520 IF POSITION(#batchFinal%)=bkLength THEN
4521 batchCopyOK% = 1
4522 END IF
4523 CLOSE #batchRead%
4524 batchRead% = -1
4525 CLOSE #batchFinal%
4526 batchFinal% = -1
4527 END DEFine
4528 DEFine PROCedure BATCH_GRID(BGSOLUTION%)
4529 LOCal bgY%
4530 LOCal bgX%
4531 LOCal bgP%
4532 LOCal bgCell%
4533 LOCal bgWidth%
4534 LOCal bgText$(16)
4535 LOCal bgLine$(512)
4536 bgWidth% = LEN(gen_label$((OPT_PAIRS%)))
4537   FOR bgY%=0 TO OPT_HEIGHT%-1
4538 bgLine$ = ""
4539     FOR bgX%=0 TO OPT_WIDTH%-1
4540 bgCell% = bgY%*OPT_WIDTH%+bgX%
4541 bgP% = gEndpoint%(bgCell%)
4542 IF bgSolution% THEN
4543 bgP% = gOwner%(bgCell%)
4544 END IF
4545 bgText$ = "."
4546 IF bgP% THEN
4547 bgText$ = gen_label$((bgP%))
4548 END IF
4549 IF bgX% THEN
4550 bgLine$ = bgLine$ & " "
4551 END IF
4552 bgLine$ = bgLine$ & FILL$(" ",bgWidth%-LEN(bgText$)) & bgText$
4553     END FOR bgX%
4554 PRINT #batchConsole%,bgLine$
4555   END FOR bgY%
4556 END DEFine
4557 DEFine PROCedure BATCH_CONSOLE(BPSEED)
4558 PRINT #batchConsole%,""
4559 PRINT #batchConsole%,gen_int$((OPT_WIDTH%)) & "x" & gen_int$((OPT_HEIGHT%)) & ", " & gen_int$((OPT_PAIRS%)) & " pairs, seed " & gen_int$((bpSeed))
4560 PRINT #batchConsole%,"Mode: " & gen_mode$
4561 PRINT #batchConsole%,"Work: " & gen_int$((WORK_BOARDATTEMPTS)) & " board attempts; " & gen_int$((WORK_TRAVERSALEVALUATIONS)) & " traversal evaluations."
4562 IF reasoning_primary% THEN
4563 PRINT #batchConsole%,"Difficulty: " & SR_GRADE$ & " (" & gen_int$((SR_SCORE%)) & "/100; reasoning; provisional)"
4564   ELSE
4565 PRINT #batchConsole%,"Difficulty: " & geometry_grade$ & " (" & gen_int$((geoScore%)) & "/100; geometric; provisional)"
4566   END IF
4567 IF gHasReason% THEN
4568 PRINT #batchConsole%,"Solver: " & SR_TIER$ & "; " & gen_int$((SR_TRIALSTEPS)) & " trial eliminations; uniqueness " & SR_UNIQUENESS$
4569 PRINT #batchConsole%,"Assessment: " & SR_STATUS$ & "; " & gen_int$((SR_WORK)) & "/" & gen_int$((SR_WORKLIMIT)) & " work units."
4570   END IF
4571 IF gHasShortest% THEN
4572 PRINT #batchConsole%,"Shortest-route screen: solved=" & gen_bool$((shortSolved%)) & "; " & gen_int$((shortTrials%)) & " trials."
4573 END IF
4574 PRINT #batchConsole%,"Puzzle (. = empty):"
4575 batch_grid (0)
4576 IF OPT_SHOW% THEN
4577 PRINT #batchConsole%,"Known solution (. = unused):"
4578 batch_grid (1)
4579 END IF
4580 END DEFine
4581 DEFine FuNction BATCH_BEFORE%(BBA%,BBB%)
4582 LOCal bbI%
4583   FOR bbI%=0 TO 6
4584 IF batch_value((bbA%),(bbI%))<batch_value((bbB%),(bbI%)) THEN
4585 RETurn 1
4586 END IF
4587 IF batch_value((bbA%),(bbI%))>batch_value((bbB%),(bbI%)) THEN
4588 RETurn 0
4589 END IF
4590   END FOR bbI%
4591 RETurn 0
4592 END DEFine
4593 DEFine PROCedure BATCH_SORT
4594 LOCal bsI%
4595 LOCal bsJ%
4596 LOCal bsAt%
4597   FOR bsI%=1 TO batchAccepted%
4598     batchOrder%(bsI%)=bsI%
4599   END FOR bsI%
4600 IF NOT batchUseSelection% OR NOT OPT_FULL% THEN
4601 RETurn 
4602 END IF
4603   FOR bsI%=2 TO batchAccepted%
4604 bsAt% = batchOrder%(bsI%)
4605 bsJ% = bsI%-1
4606     REPeat bsInsert
4607 IF bsJ%<1 THEN
4608 EXIT bsInsert
4609 END IF
4610 IF NOT batch_before%((bsAt%),(batchOrder%(bsJ%))) THEN
4611 EXIT bsInsert
4612 END IF
4613 batchOrder%(bsJ%+1)=batchOrder%(bsJ%)
4614 bsJ% = bsJ%-1
4615     END REPeat bsInsert
4616     batchOrder%(bsJ%+1)=bsAt%
4617   END FOR bsI%
4618 END DEFine
4619 DEFine PROCedure BATCH_HEADER
4620 LOCal bhI%
4621 LOCal bhSame%
4622 LOCal bhPairs%
4623 LOCal bhMax
4624 LOCal bhAttempts%
4625 LOCal bhAlgorithm$(64)
4626 jsonLevel% = 0
4627 json_object ("")
4628 json_integer ("formatVersion"),(1)
4629 json_object ("rules")
4630 json_boolean ("orthogonalOnly"),(1)
4631 json_boolean ("noSharedCells"),(1)
4632 json_boolean ("noSelfTouching"),(1)
4633 json_boolean ("requireFullCoverage"),(OPT_FULL%)
4634 json_end ("}")
4635 json_object ("generation")
4636 bhAlgorithm$ = selAlgorithm$
4637 IF OPT_RAMPFIRST% THEN
4638 bhAlgorithm$ = "ql-difficulty-ramp-v1"
4639 END IF
4640 json_string ("algorithm"),(bhAlgorithm$)
4641 json_string ("implementation"),("SSB 2.8 / Turbo 5.09")
4642 json_string ("randomAlgorithm"),("ql-subtractive-int31-v1")
4643 json_string ("batchSeedAlgorithm"),("splitmix64-batch-v1")
4644 json_string ("deduplication"),("board-symmetry-v1")
4645 json_string ("mode"),(gen_mode$)
4646 json_integer ("seed"),(OPT_SEED)
4647 json_integer ("width"),(OPT_WIDTH%)
4648 json_integer ("height"),(OPT_HEIGHT%)
4649 json_string ("cutStyle"),(gen_cut$)
4650 json_integer ("minPathLength"),(OPT_MINIMUM%)
4651 IF OPT_RAMPFIRST% THEN
4652 bhPairs% = rampPairs%(OPT_RAMPFIRST%)
4653 bhMax = rampRawMax(OPT_RAMPFIRST%)
4654 bhAttempts% = rampAttempts%(OPT_RAMPFIRST%)
4655     FOR bhI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
4656 IF rampPairs%(bhI%)<>rampPairs%(OPT_RAMPFIRST%) THEN
4657 bhPairs% = -1
4658 END IF
4659 IF rampRawMax(bhI%)<>rampRawMax(OPT_RAMPFIRST%) THEN
4660 bhMax = -1
4661 END IF
4662 IF rampAttempts%(bhI%)<>rampAttempts%(OPT_RAMPFIRST%) THEN
4663 bhAttempts% = -1
4664 END IF
4665     END FOR bhI%
4666 IF bhPairs%<0 THEN
4667 json_null ("pairCount")
4668     ELSE
4669 json_integer ("pairCount"),(bhPairs%)
4670     END IF
4671 IF bhMax<0 THEN
4672 json_null ("maxPathLength")
4673     ELSE
4674 json_integer ("maxPathLength"),(bhMax)
4675     END IF
4676 IF bhAttempts%<0 THEN
4677 json_null ("maxAttempts")
4678     ELSE
4679 json_integer ("maxAttempts"),(bhAttempts%)
4680     END IF
4681 json_string ("ramp"),(gen_grade$((OPT_RAMPFIRST%)) & ".." & gen_grade$((OPT_RAMPLAST%)))
4682 json_array ("rampSections")
4683     FOR bhI%=OPT_RAMPFIRST% TO OPT_RAMPLAST%
4684 json_object ("")
4685 json_string ("target"),(gen_grade$((bhI%)))
4686 json_integer ("count"),(rampCounts%(bhI%))
4687 json_object ("recipe")
4688 json_integer ("pairCount"),(rampPairs%(bhI%))
4689 json_integer ("minPathLength"),(OPT_MINIMUM%)
4690 json_integer ("maxPathLength"),(rampRawMax(bhI%))
4691 json_integer ("maxAttempts"),(rampAttempts%(bhI%))
4692 json_string ("cutStyle"),(gen_cut$)
4693 json_end ("}")
4694 json_end ("}")
4695     END FOR bhI%
4696 json_end ("]")
4697 json_null ("selection")
4698   ELSE
4699 json_integer ("pairCount"),(OPT_PAIRS%)
4700 json_integer ("maxPathLength"),(gRawMaximum)
4701 json_integer ("maxAttempts"),(OPT_ATTEMPTS%)
4702 json_null ("ramp")
4703 json_null ("rampSections")
4704 IF gSelected% THEN
4705 json_object ("selection")
4706 json_policy
4707 json_integer ("batchRequests"),(batchRequests)
4708 json_integer ("rejectedSearches"),(batchRejected)
4709 json_integer ("rejectedDuplicates"),(batchDuplicates)
4710 json_integer ("batchCandidateBudget"),(batchBudget)
4711 json_integer ("batchCandidatesConsidered"),(batchConsumed)
4712 json_end ("}")
4713     ELSE
4714 json_null ("selection")
4715     END IF
4716   END IF
4717 json_object ("batch")
4718 json_integer ("requests"),(batchRequests)
4719 json_integer ("rejectedSearches"),(batchRejected)
4720 json_integer ("rejectedDuplicates"),(batchDuplicates)
4721 json_integer ("candidatesConsidered"),(batchConsumed)
4722 IF batchUseSelection% THEN
4723 json_null ("requestLimit")
4724 json_integer ("candidateBudget"),(batchBudget)
4725   ELSE
4726 json_integer ("requestLimit"),(batchRequestLimit)
4727 json_null ("candidateBudget")
4728   END IF
4729 json_end ("}")
4730 json_end ("}")
4731 json_array ("puzzles")
4732 END DEFine
4733 DEFine PROCedure BATCH_PUBLISH
4734 LOCal bfI%
4735 LOCal bfEntry%
4736 batch_sort
4737 IF LEN(OPT_OUTPUT$) THEN
4738 gTemp$ = batch_new$((batchBase$ & "_pack"))
4739 IF gExit% THEN
4740 RETurn 
4741 END IF
4742 batchFinal% = FOP_OVER(gTemp$)
4743 IF batchFinal%<0 THEN
4744 gExit% = 1
4745 gError$ = "Cannot open completed pack staging file."
4746 RETurn 
4747 END IF
4748 gJ% = batchFinal%
4749 batch_header
4750 batchRead% = FOP_IN(gSpool$)
4751 IF batchRead%<0 THEN
4752 gExit% = 1
4753 gError$ = "Cannot reopen puzzle spool."
4754 RETurn 
4755 END IF
4756     FOR bfI%=1 TO batchAccepted%
4757 bfEntry% = batchOrder%(bfI%)
4758 json_key ("")
4759 batch_copy (batchRead%),(batchFinal%),(batchOffset(bfEntry%)),(batchLength(bfEntry%))
4760 IF gExit% OR gCancel% THEN
4761 RETurn 
4762 END IF
4763     END FOR bfI%
4764 CLOSE #batchRead%
4765 batchRead% = -1
4766 json_end ("]")
4767 json_end ("}")
4768 PRINT #batchFinal%
4769 CLOSE #batchFinal%
4770 batchFinal% = -1
4771 gJ% = -1
4772 IF FTEST(gOutput$)=0 THEN
4773 IF NOT OPT_FORCE% THEN
4774 gExit% = 2
4775 gError$ = "Output appeared during generation; --force is required."
4776 RETurn 
4777 END IF
4778 gBackup$ = batch_new$((batchBase$ & "_previous"))
4779 IF gExit% THEN
4780 RETurn 
4781 END IF
4782 batch_clone (gOutput$),(gBackup$)
4783 IF NOT batchCopyOK% THEN
4784 RETurn 
4785 END IF
4786 batchBacked% = 1
4787     END IF
4788 batchDestTouched% = 1
4789 batch_clone (gTemp$),(gOutput$)
4790 IF NOT batchCopyOK% THEN
4791 IF batchBacked% THEN
4792 batchRestoring% = 1
4793 batch_clone (gBackup$),(gOutput$)
4794 batchRestoring% = 0
4795       ELSE
4796 IF FTEST(gOutput$)=0 THEN
4797 DELETE gOutput$
4798 END IF
4799       END IF
4800 gExit% = 1
4801 gError$ = "Publication did not complete; previous output was retained for recovery."
4802 RETurn 
4803     END IF
4804 batchCommitted% = 1
4805 PRINT #0,"Saved " & gen_int$((batchAccepted%)) & " puzzle(s) to " & gOutput$
4806   END IF
4807 batchRead% = FOP_IN(batchConsole$)
4808 IF batchRead%>=0 THEN
4809     FOR bfI%=1 TO batchAccepted%
4810 bfEntry% = batchOrder%(bfI%)
4811 batch_copy (batchRead%),(0),(batchConsoleOffset(bfEntry%)),(batchConsoleLength(bfEntry%))
4812 IF gCancel% OR gExit% THEN
4813 EXIT bfI%
4814 END IF
4815     END FOR bfI%
4816 CLOSE #batchRead%
4817 batchRead% = -1
4818   END IF
4819 PRINT #0,"Known solutions validated. Difficulty estimates are provisional."
4820 END DEFine
4821 DEFine PROCedure BATCH_RUN
4822 LOCal btI%
4823 LOCal btJ%
4824 LOCal btP%
4825 LOCal btDuplicate%
4826 LOCal btSection%
4827 LOCal btSectionCount%
4828 LOCal btSeed
4829 LOCal btBefore
4830 LOCal btCan%
4831 LOCal btUsed
4832 gOutput$ = OPT_OUTPUT$
4833 gSpool$ = ""
4834 gTemp$ = ""
4835 gBackup$ = ""
4836 batchConsole$ = ""
4837 batchSpool% = -1
4838 batchConsole% = -1
4839 batchRead% = -1
4840 batchFinal% = -1
4841 batchBacked% = 0
4842 batchCommitted% = 0
4843 batchDestTouched% = 0
4844 IF LEN(gOutput$) THEN
4845 IF FTEST(gOutput$)=0 AND NOT OPT_FORCE% THEN
4846 gExit% = 2
4847 gError$ = "Output exists; use --force to replace it."
4848 RETurn 
4849 END IF
4850   END IF
4851 batchBase$ = gOutput$
4852 IF LEN(batchBase$)=0 THEN
4853 batchBase$ = DATAD$ & "tangles"
4854 END IF
4855 gSpool$ = batch_new$((batchBase$ & "_spool"))
4856 IF gExit% THEN
4857 RETurn 
4858 END IF
4859 batchConsole$ = batch_new$((batchBase$ & "_console"))
4860 IF gExit% THEN
4861 RETurn 
4862 END IF
4863 batchSpool% = FOP_OVER(gSpool$)
4864 batchConsole% = FOP_OVER(batchConsole$)
4865 IF batchSpool%<0 OR batchConsole%<0 THEN
4866 gExit% = 1
4867 gError$ = "Cannot open batch spool files."
4868 RETurn 
4869 END IF
4870 DIM batchOffset(OPT_COUNT%)
4871 DIM batchLength(OPT_COUNT%)
4872 DIM batchConsoleOffset(OPT_COUNT%)
4873 DIM batchConsoleLength(OPT_COUNT%)
4874 DIM batchOrder%(OPT_COUNT%)
4875 DIM gBatchKeys$(OPT_COUNT%,64)
4876 batchSortMemory = ALCHP((OPT_COUNT%+1)*batchSortStride)
4877 IF NOT batchSortMemory THEN
4878 gExit% = 1
4879 gError$ = "Not enough memory for batch ordering."
4880 RETurn 
4881 END IF
4882 batchAccepted% = 0
4883 batchRequests = 0
4884 batchRejected = 0
4885 batchDuplicates = 0
4886 batchConsumed = 0
4887 batchUseSelection% = (selection_active% OR OPT_RAMPFIRST%>0)
4888 batchRequestLimit = gen_product((10),(OPT_COUNT%))
4889 batchBudget = batchRequestLimit
4890 IF batchUseSelection% THEN
4891 batchBudget = gen_product((gen_product((2),(OPT_COUNT%))),(OPT_CANDIDATES%))
4892 END IF
4893 batchDepthSort% = (OPT_MINDEPTH%>0 OR OPT_MINNODES%>0 OR rampBaseDepth%>0 OR rampBaseNodes%>0)
4894 btSection% = OPT_RAMPFIRST%
4895 btSectionCount% = 0
4896   REPeat btGenerate
4897 IF batchAccepted%>=OPT_COUNT% OR gExit% OR gCancel% THEN
4898 EXIT btGenerate
4899 END IF
4900 btCan% = (batchRequests<batchRequestLimit)
4901 IF batchUseSelection% THEN
4902 btCan% = (batchConsumed+OPT_CANDIDATES%<=batchBudget)
4903 END IF
4904 IF NOT btCan% THEN
4905 EXIT btGenerate
4906 END IF
4907 IF OPT_RAMPFIRST% THEN
4908 cli_section (btSection%)
4909 END IF
4910 btSeed = batch_seed((OPT_SEED),(batchRequests))
4911 batchRequests = batchRequests+1
4912 gen_board
4913 selection_generate (btSeed)
4914 IF gExit% OR gCancel% THEN
4915 EXIT btGenerate
4916 END IF
4917 btUsed = 1
4918 IF batchUseSelection% THEN
4919 btUsed = OPT_CANDIDATES%
4920 END IF
4921 IF gSuccess% THEN
4922 IF batchUseSelection% THEN
4923 btUsed = selConsidered%
4924 END IF
4925 selKey$ = identity_symmetry$
4926 btDuplicate% = 0
4927       FOR btI%=1 TO batchAccepted%
4928 IF cli_equal%((gBatchKeys$(btI%)),(selKey$)) THEN
4929 btDuplicate% = 1
4930 EXIT btI%
4931 END IF
4932       END FOR btI%
4933 IF btDuplicate% THEN
4934 batchDuplicates = batchDuplicates+1
4935       ELSE
4936 batchAccepted% = batchAccepted%+1
4937 gBatchKeys$(batchAccepted%)=selKey$
4938 batchOffset(batchAccepted%)=POSITION(#batchSpool%)
4939 gJ% = batchSpool%
4940 json_puzzle (btSeed)
4941         batchLength(batchAccepted%)=POSITION(#batchSpool%)-batchOffset(batchAccepted%)
4942 IF gExit% OR gCancel% THEN
4943 EXIT btGenerate
4944 END IF
4945 batchConsoleOffset(batchAccepted%)=POSITION(#batchConsole%)
4946 batch_console (btSeed)
4947         batchConsoleLength(batchAccepted%)=POSITION(#batchConsole%)-batchConsoleOffset(batchAccepted%)
4948         FOR btI%=0 TO 6
4949 batch_metric (batchAccepted%),(btI%),(0)
4950         END FOR btI%
4951 IF OPT_RAMPFIRST% THEN
4952 batch_metric (batchAccepted%),(0),(OPT_TARGET%)
4953 END IF
4954 IF gHasReason% THEN
4955 batch_metric (batchAccepted%),(1),(SR_SCORE%)
4956 IF batchDepthSort% THEN
4957 batch_metric (batchAccepted%),(1),(0)
4958 batch_metric (batchAccepted%),(2),(SR_FIRSTDEPTH%)
4959 batch_metric (batchAccepted%),(3),(SR_FIRSTNODES)
4960 END IF
4961 batch_metric (batchAccepted%),(4),(SR_TRIALSTEPS)
4962 batch_metric (batchAccepted%),(5),(SR_ROUTEDEDUCTIONS)
4963         END IF
4964 batch_metric (batchAccepted%),(6),(btSeed)
4965 PRINT #0,"Selected " & gen_int$((batchAccepted%)) & "/" & gen_int$((OPT_COUNT%)) & " (seed " & gen_int$((btSeed)) & ")."
4966 IF OPT_RAMPFIRST% THEN
4967 btSectionCount% = btSectionCount%+1
4968 IF btSectionCount%>=rampCounts%(btSection%) THEN
4969 btSection% = btSection%+1
4970 btSectionCount% = 0
4971 END IF
4972         END IF
4973       END IF
4974     ELSE
4975 IF NOT batchUseSelection% THEN
4976 gExit% = 1
4977 gError$ = "No candidate found within --max-attempts. This does not prove impossibility."
4978 EXIT btGenerate
4979 END IF
4980 batchRejected = batchRejected+1
4981 PRINT #0,"Skipped seed " & gen_int$((btSeed)) & ": no candidate met the bounded selection policy."
4982     END IF
4983 batchConsumed = batchConsumed+btUsed
4984 gen_poll
4985   END REPeat btGenerate
4986 CLOSE #batchSpool%
4987 batchSpool% = -1
4988 CLOSE #batchConsole%
4989 batchConsole% = -1
4990 IF NOT gExit% AND NOT gCancel% THEN
4991 IF batchAccepted%<>OPT_COUNT% THEN
4992 gExit% = 1
4993 gError$ = "Batch budget exhausted. No partial pack was written; no easier puzzle was substituted."
4994     ELSE
4995 batch_publish
4996     END IF
4997   END IF
4998 END DEFine
4999 IF COMPILED THEN
5000   WHEN_ERROR 0
5001     RJOB -1,-1
5002   END_WHEN
5003   WHEN_ERROR 1
5004 batch_runtime_error (ERNUM%)
5005   END_WHEN
5006 ELSE
5007   WHEN ERRor
5008 batch_runtime_error (ERNUM)
5009   END WHEN
5010 END IF
5011 OPEN #0,con
5012 WINDOW #0,512,256,0,0
5013 PAPER #0,0
5014 INK #0,4
5015 CLS #0
5016 gInput% = 0
5017 gCommand$ = ""
5018 IF COMPILED THEN
5019 IF LEN(OPTION_CMD$)>4096 THEN
5020 cli_error ("Option string exceeds 4096 bytes.")
5021 END IF
5022 IF NOT gExit% THEN
5023 gCommand$ = OPTION_CMD$
5024 END IF
5025 ELSE
5026 IF LEN(CMD$)>4096 THEN
5027 cli_error ("Option string exceeds 4096 bytes.")
5028 END IF
5029 IF NOT gExit% THEN
5030 gCommand$ = CMD$
5031 END IF
5032 END IF
5033 IF NOT gExit% THEN
5034 cli_parse (gCommand$)
5035 END IF
5036 IF NOT gExit% THEN
5037 IF cliHelp% THEN
5038 cli_help
5039   ELSE
5040 batch_run
5041   END IF
5042 END IF
5043 batch_cleanup
5044 IF gCancel% THEN
5045 gExit% = 130
5046 END IF
5047 IF gExit% THEN
5048 PRINT #0,"Tangles: " & gError$ & " (exit " & gen_int$((gExit%)) & ")."
5049 END IF
5050 batch_exit
