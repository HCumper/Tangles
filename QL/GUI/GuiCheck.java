import java.nio.file.*;
import java.awt.image.BufferedImage;
import java.util.concurrent.atomic.AtomicReference;
import CPU.MC68000Cpu;
import screens.Screen32;
import smsqmulator.*;
import drivers.nxfa.*;

/** Integration check against actual SMSQ/E and the compiled 68000 executable.
 * Uses SMSQmulator's emulated mouse linkage, not direct calls to BASIC routines.
 * No Swing window or user disks are opened. External emulator/ROM not bundled.
 */
public class GuiCheck {
    static MC68000Cpu cpu;
    static ProbeScreen screen;
    static TrapDispatcher traps;
    static Path dir;
    static int mouseX=0,mouseY=0,originX=16,originY=16;
    static int checks=0,choice=1,cellSize=60;
    static final int[] colours={0xE34B4B,0xF38B32,0xE5BD28,0x9DBD30,
        0x38AE61,0x28B6A0,0x35B7D9,0x397CE0,0x6554D2,0x9854CB,0xD65EAE,
        0xEF87A1,0xAD7850,0x8F999E,0xCEE2B4,0xE4D4F5};
    static volatile boolean probing=false;
    static final java.util.concurrent.atomic.AtomicLong fills=new java.util.concurrent.atomic.AtomicLong();
    static final java.util.concurrent.atomic.AtomicLong pixels=new java.util.concurrent.atomic.AtomicLong();
    static final java.util.concurrent.atomic.AtomicLong legendWrites=new java.util.concurrent.atomic.AtomicLong();
    static final java.util.concurrent.atomic.AtomicLong endpointFlashes=new java.util.concurrent.atomic.AtomicLong();
    static class ProbeScreen extends Screen32 {
        ProbeScreen(){super(800,600,0,false,null,false,false,0,false,false);}
        int pixel(int x,int y){return this.dataBuffer.getElem(y*this.xsize+x);}
        @Override public void fillBlock(MC68000Cpu c,boolean stipple){
            int x=c.data_regs[2]>>>16,y=c.data_regs[2]&65535;
            int w=c.data_regs[1]>>>16,h=c.data_regs[1]&65535;
            boolean visible=c.addr_regs[1]>=this.startAddress&&c.addr_regs[1]<=this.stopAddress;
            super.fillBlock(c,stipple);
            if(probing&&visible){
                fills.incrementAndGet();pixels.addAndGet((long)w*h);
                if(x<originX+744&&x+w>originX+632&&y<originY+312&&y+h>originY+294)legendWrites.incrementAndGet();
                if(!near(pixel(originX+24,originY+68),colours[0]))endpointFlashes.incrementAndGet();
            }
        }
    }

    static void checkStrokeRendering(Path artifacts) throws Exception {
        key('c');move(cellX(0),cellY(0));
        fills.set(0);pixels.set(0);legendWrites.set(0);endpointFlashes.set(0);
        probing=true;button(1);
        long started=System.nanoTime();moveRaw(cellX(1),cellY(0));
        long until=started+2_000_000_000L;
        while(!near(screen.pixel(cellX(1)+5,cellY(0)+5),colours[0])&&System.nanoTime()<until)Thread.sleep(1);
        double latency=(System.nanoTime()-started)/1_000_000.0;
        for(int i=0;i<25;i++){moveRaw(cellX(1)+(i%5),cellY(0));Thread.sleep(3);}
        button(0);Thread.sleep(200);probing=false;
        String metrics="{\"screenFills\":"+fills.get()+",\"filledPixels\":"+pixels.get()+",\"unchangedLegendWrites\":"+legendWrites.get()+",\"endpointEraseEvents\":"+endpointFlashes.get()+",\"moveLatencyMs\":"+latency+"}";
        Files.writeString(artifacts.resolve("render-metrics.json"),metrics+"\n");
        System.out.println("RENDER: "+metrics);
        check(coloured(1,0,colours[0]),"render probe accepts a rapid move and release");
        if(!Boolean.getBoolean("ql.renderBaseline")){
            check(legendWrites.get()==0,"dragging does not repaint another pair's legend entry");
            check(endpointFlashes.get()==0,"endpoint remains painted throughout every screen fill in a drag");
            check(pixels.get()<10000,"ordinary path extension avoids large area clears");
        }
        checkBoardRedraw("incremental extension matches a complete redraw");
        key('c');
    }

    static void checkBoardRedraw(String message) throws Exception {
        move(originX+720,originY+500);BufferedImage before=picture(null);
        key('r');BufferedImage after=picture(null);boolean equal=true;
        for(int y=originY+60;y<originY+540&&equal;y++)
            for(int x=originX+16;x<originX+496;x++)
                if(before.getRGB(x,y)!=after.getRGB(x,y)){equal=false;break;}
        check(equal,message);
    }

    static BufferedImage picture(String name) throws Exception {
        BufferedImage pic=new BufferedImage(800,600,BufferedImage.TYPE_INT_RGB);
        screen.setSize(800,600);
        var graphics=pic.createGraphics();screen.paint(graphics);graphics.dispose();
        if(name!=null) javax.imageio.ImageIO.write(pic,"png",dir.resolve(name).toFile());
        return pic;
    }
    static boolean near(int actual,int expected) {
        for(int shift:new int[]{0,8,16})
            if(Math.abs(((actual>>shift)&255)-((expected>>shift)&255))>8) return false;
        return true;
    }
    static void check(boolean condition,String message) {
        if(!condition) throw new AssertionError(message);
        checks++;
        System.out.println("PASS: "+message);
    }
    static void moveRaw(int x,int y) {
        cpu.writeMemoryLong(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEPOS,(x<<16)|y);
        cpu.writeMemoryLong(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEREL,((x-mouseX)<<16)|((y-mouseY)&65535));
        traps.resetCounter();mouseX=x;mouseY=y;
    }
    static void move(int x,int y) throws Exception {moveRaw(x,y);Thread.sleep(180);}
    static void checkIdleLegend() throws Exception {
        move(cellX(1),cellY(1));Thread.sleep(250);
        boolean stable=true;
        // Hovering the grid must not repeatedly clear the right-hand legend.
        for(int i=0;i<80;i++){
            moveRaw(cellX(1)+(i%20),cellY(1));Thread.sleep(3);
            if(!near(picture(null).getRGB(originX+617,originY+308),colours[0])){stable=false;break;}
        }
        check(stable,"letter legend stays painted while the pointer moves without a button");
    }
    static void shortStroke(int fromX,int fromY,int toX,int toY) throws Exception {
        move(cellX(fromX),cellY(fromY));
        cpu.writeMemoryWord(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEBTN,1);traps.resetCounter();
        Thread.sleep(45);moveRaw(cellX(toX),cellY(toY));Thread.sleep(45);
        cpu.writeMemoryWord(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEBTN,0);traps.resetCounter();
        Thread.sleep(350);
    }
    static void button(int state) throws Exception {
        cpu.writeMemoryWord(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEBTN,state);
        traps.resetCounter();Thread.sleep(200);
    }
    static void click(int x,int y) throws Exception {move(x,y);button(1);button(0);}
    static void key(char value) throws Exception {
        move(originX+720,originY+500); // Pointer Environment keyboard focus.
        traps.addToKeyboardQueue(value);Thread.sleep(400);
    }
    static void commands(String text) throws Exception {
        for(char c:text.toCharArray()){traps.addToKeyboardQueue(c);Thread.sleep(140);}
    }
    static void waitForInput(boolean visible) throws Exception {
        long until=System.nanoTime()+5_000_000_000L;
        while(System.nanoTime()<until){
            boolean shown=near(screen.pixel(originX+30,originY+210),0xD8DFD4);
            if(shown==visible)return;
            Thread.sleep(20);
        }
        picture("input-timeout.png");
        throw new AssertionError("Input dialog did not become "+(visible?"visible":"dismissed"));
    }
    static int cellX(int x){return originX+16+cellSize*x+cellSize/2;}
    static int cellY(int y){return originY+60+cellSize*y+cellSize/2;}
    static void stroke(int[][] cells) throws Exception {
        move(cellX(cells[0][0]),cellY(cells[0][1]));button(1);
        for(int i=1;i<cells.length;i++)move(cellX(cells[i][0]),cellY(cells[i][1]));
        button(0);Thread.sleep(300);
    }
    static boolean coloured(int x,int y,int rgb) throws Exception {
        move(originX+720,originY+500); // Outside the grid, inside this QL job.
        return near(picture(null).getRGB(cellX(x)+5,cellY(y)+5),rgb);
    }
    static String entry(String id,int w,int h,int[][] pairs) {
        StringBuilder s=new StringBuilder("{\"id\":\""+id+"\",\"width\":"+w+",\"height\":"+h+",\"pairs\":[");
        for(int i=0;i<pairs.length;i++){
            if(i>0)s.append(',');
            int a=pairs[i][0],b=pairs[i][1];
            s.append("{\"label\":\"").append((char)('A'+i)).append("\",\"start\":{\"x\":").append(a%w).append(",\"y\":").append(a/w)
                .append("},\"finish\":{\"x\":").append(b%w).append(",\"y\":").append(b/w).append("}}");
        }
        return s.append("]}").toString();
    }
    static String pack(boolean full,String... entries){
        return "{\"formatVersion\":1,\"rules\":{\"orthogonalOnly\":true,\"noSharedCells\":true,\"noSelfTouching\":true,\"requireFullCoverage\":"+full+"},\"puzzles\":["+String.join(",",entries)+"]}";
    }
    static void choose(int index) throws Exception {
        key('k');for(int i=choice;i>index;i--)key((char)0xd0);
        for(int i=choice;i<index;i++)key((char)0xd8);key('\n');choice=index;Thread.sleep(500);
    }

    public static void main(String[] args) throws Exception {
        Path artifacts=Path.of(args[1]).toAbsolutePath();
        dir=artifacts.resolve("gui-check-"+System.currentTimeMillis());Files.createDirectories(dir);
        for(String name:new String[]{"ttk","tptr_ext","tangles_exe"})Files.copy(artifacts.resolve(name),dir.resolve(name));
        int[][] two={{0,2},{6,8}},three={{0,2},{3,5},{6,8}},sixteen=new int[16][2];
        for(int i=0;i<16;i++){sixteen[i][0]=16*i;sixteen[i][1]=16*i+15;}
        Files.writeString(dir.resolve("connect"),pack(false,entry("connect-one",3,3,two),entry("connect-two",3,3,two)));
        Files.writeString(dir.resolve("full"),pack(true,entry("full",3,3,three)));
        Files.writeString(dir.resolve("gaps"),pack(true,entry("full-gaps",3,3,two)));
        Files.writeString(dir.resolve("sixteen"),pack(true,entry("sixteen",16,16,sixteen)));
        Files.writeString(dir.resolve("tangles_catalog"),"4\nConnect / 3x3 / connections only\nconnect\nFull / 3x3 / full coverage\nfull\nGaps / 3x3 / full coverage\ngaps\nSixteen / 16x16 / full coverage\nsixteen\n");
        Files.writeString(dir.resolve("runtimeboot"),"100 DATA_USE \"nfa1_\"\n102 LRESPR \"nfa1_ttk\"\n105 LRESPR \"nfa1_tptr_ext\"\n108 OPEN_NEW #9,\"nfa1_environment\":PRINT #9,W_MAX;\",\";DISP_TYPE:CLOSE #9\n110 EXEC_W \"sfa1_tangles_exe\"\n120 OPEN_NEW #9,\"nfa1_exited\":PRINT #9,\"OK\":CLOSE #9\n");
        Files.deleteIfExists(dir.resolve("exited"));
        Files.deleteIfExists(dir.resolve("environment"));
        Files.deleteIfExists(dir.resolve("smoke.json"));
        for(int i=0;i<8;i++)Files.deleteIfExists(dir.resolve("trace"+i));
        inifile.IniFile ini=new inifile.IniFile();
        for(String k:new String[]{"NFA_USE","SFA_USE","WIN_USE","MEM_USE","FLP_USE","NFA1"})ini.addOption(k,"","");
        for(String k:new String[]{"SCREEN-MODE","TIME-OFFSET","NFA-FILENAME-CHANGE"})ini.addOption(k,"0","");
        ini.setOptionValue("SCREEN-MODE","3");ini.setOptionValue("SOUND-VOLUME","0");
        SMSQmulator.presetOptions(ini,args[0]);
        ini.setOptionValue("DISABLE-WIN-DEVICE","1");ini.setOptionValue("DISABLE-FLP-DEVICE","1");
        ini.setOptionValue("DISABLE-SFA-DEVICE","0");ini.setOptionValue("NFA_USE","");
        screen=new ProbeScreen();
        cpu=new MC68000Cpu(16*1024*1024,screen,350000);
        traps=new TrapDispatcher(null,null,ini){
            @Override public void dispatchTrap(int t,MC68000Cpu c){
                if(t==Types.TRAP5){
                    int operation=c.data_regs[0];
                    if(operation==12||operation==37){c.data_regs[0]=0;c.reg_sr|=4;return;}
                    if(operation>=32&&operation<=35){
                        if(operation<=33)screen.fillBlock(c,operation==32);
                        else screen.xorBlock(c,operation==34);
                        c.data_regs[0]=0;c.reg_sr|=4;
                        c.pc_reg=c.readMemoryLong(c.addr_regs[7])/2;c.addr_regs[7]+=4;return;
                    }
                }
                super.dispatchTrap(t,c);
            }
        };
        String[] mounts={dir.toString()+java.io.File.separator,"","","","","","",""};
        NfaDriver nfa=new NfaDriver(true);nfa.setCpu(cpu);nfa.setNames(mounts,ini,false,false);traps.register(nfa);
        SfaDriver sfa=new SfaDriver(false);sfa.setCpu(cpu);sfa.setNames(mounts,ini,false,false);traps.register(sfa);
        cpu.setTrapDispatcher(traps);
        if(!SMSQE.loadRom(args[0],cpu)||!SMSQE.checkForValidSMSQE(cpu)||!SMSQE.setupSMSQE(cpu,ini))throw new Exception("Cannot boot SMSQ/E");
        cpu.writeMemoryLong(cpu.getRamSize()+Types.LINKAGE_BOOT_DEVICE,Types.NFADriver+1);
        QL50HzInterrupt interrupts=new QL50HzInterrupt(cpu);interrupts.startInterrupts();
        interrupts.setScreenUpdateInterval(8); // Match the desktop launcher's display cadence.
        AtomicReference<Throwable> failure=new AtomicReference<>();
        Thread runner=new Thread(()->{try{cpu.executeContinuous();}catch(Throwable e){failure.set(e);}},"SMSQ/E CPU");
        runner.setDaemon(true);runner.start();
        try {
            Thread.sleep(3000);commands("\n\nLRUN nfa1_runtimeboot\n");
            long until=System.nanoTime()+30_000_000_000L;boolean ready=false;
            while(System.nanoTime()<until){
                if(failure.get()!=null)throw new Exception("Emulator failure",failure.get());
                if(near(picture(null).getRGB(42,86),colours[0])){ready=true;break;}
                Thread.sleep(200);
            }
            picture("initial.png");check(ready,"compiled player loaded generator-format clues at 800x600 in mode 32");
            checkStrokeRendering(artifacts);
            if(Boolean.getBoolean("ql.renderProbeOnly"))return;
            checkIdleLegend();
            shortStroke(0,0,2,0);
            check(coloured(1,0,colours[0]),"short press-drag-release keeps the first movement and release position");
            key('c');
            stroke(new int[][]{{0,0},{1,0},{1,1},{0,1}});
            check(coloured(1,1,colours[0])&&coloured(0,1,0x192D36),"no-self-touch rule rejects the last step");
            checkBoardRedraw("incremental corner rendering matches a complete redraw");
            stroke(new int[][]{{0,0},{0,1},{0,2}});
            check(coloured(0,1,colours[0])&&coloured(0,2,colours[1]),"foreign endpoint cannot be crossed");
            key('c');stroke(new int[][]{{0,0},{2,0}});
            check(coloured(1,0,colours[0]),"fast drag interpolates through the intervening square");
            move(cellX(1),cellY(1));check(coloured(1,1,0x192D36),"release stops drawing");
            key('u');check(coloured(1,0,0x192D36),"undo restores the whole previous stroke");
            stroke(new int[][]{{0,0},{2,0}});key('c');key('u');
            check(coloured(1,0,colours[0]),"reset is undoable");
            checkBoardRedraw("incremental reset and undo match a complete redraw");
            key('n');check(coloured(1,0,0x192D36),"next puzzle has its own progress");
            key('p');check(coloured(1,0,colours[0]),"previous puzzle restores saved progress");
            stroke(new int[][]{{0,0},{1,0},{0,0}});
            check(coloured(1,0,0x192D36),"retrace shortens the current path");
            checkBoardRedraw("incremental retracing restores grid lines and endpoints");
            key('c');click(originX+522,originY+302);key(' ');key((char)0xc8);key((char)0xc8);key(' ');
            check(coloured(1,0,colours[0]),"arrows and Space support keyboard drawing");
            stroke(new int[][]{{0,2},{2,2}});
            check(near(picture(null).getRGB(originX+748,originY+168),0x38AE61),"connections-only completion permits unused cells");
            key('l');waitForInput(true);key((char)21);commands("nfa1_missing\n");waitForInput(false);
            check(coloured(1,0,colours[0]),"failed JSON import retains the current game");
            key('j');waitForInput(true);key((char)21);commands("2\n");waitForInput(false);
            check(coloured(1,0,0x192D36),"jump selects the requested puzzle");key('p');
            key('r');check(coloured(1,0,colours[0]),"redraw preserves progress");
            picture("connect-complete.png");
            choose(3);stroke(new int[][]{{0,0},{2,0}});stroke(new int[][]{{0,2},{2,2}});
            check(near(picture(null).getRGB(originX+748,originY+168),0xE5BD28),"full coverage does not finish with uncovered squares");
            choose(2);for(int row=0;row<3;row++)stroke(new int[][]{{0,row},{2,row}});
            check(near(picture(null).getRGB(originX+748,originY+168),0x38AE61),"full coverage completes when every square is filled");
            picture("full-complete.png");
            choose(4);cellSize=30;
            for(int i=0;i<16;i++)check(near(picture(null).getRGB(originX+22,originY+66+30*i),colours[i]),"16x16 endpoint colour "+(char)('A'+i));
            stroke(new int[][]{{0,0},{2,0}});
            checkBoardRedraw("incremental 16x16 path rendering matches a complete redraw");
            key('+');key('-');key('r');
            check(near(picture(null).getRGB(originX+22,originY+66+30*15),colours[15]),"zoom and redraw retain a 16x16 board");
            picture("sixteen.png");
            key('m');move(mouseX+8,mouseY+8);button(1);button(0);Thread.sleep(300);
            originX+=8;originY+=8;
            check(near(picture(null).getRGB(100,26),0x263C35)&&!near(picture(null).getRGB(100,18),0x263C35),"native window move redraws the player");
            picture("proof.png");key('q');
            until=System.nanoTime()+5_000_000_000L;
            while(!Files.exists(dir.resolve("exited"))&&System.nanoTime()<until)Thread.sleep(100);
            check(Files.exists(dir.resolve("exited")),"Q saves progress and exits the compiled job cleanly");
            String report="{\"passed\":true,\"resolution\":\"800x600\",\"mode\":32,\"colours\":16,\"compiled\":true,\"assertions\":"+checks+",\"folder\":\""+dir.toString().replace("\\","/")+"\"}\n";
            Files.writeString(dir.resolve("smoke.json"),report);Files.writeString(artifacts.resolve("smoke.json"),report);
        } finally {
            picture("last-screen.png");cpu.endEmulation(false);runner.join(1000);interrupts.stopInterrupts();traps.closeAllFiles();
        }
    }
}
