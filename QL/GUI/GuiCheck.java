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
    static Screen32 screen;
    static TrapDispatcher traps;
    static Path dir;
    static int mouseX=0,mouseY=0;
    static final int[] colours={0xE34B4B,0xF38B32,0xE5BD28,0x9DBD30,
        0x38AE61,0x28B6A0,0x35B7D9,0x397CE0,0x6554D2,0x9854CB,0xD65EAE,
        0xEF87A1,0xAD7850,0x8F999E,0xCEE2B4,0xE4D4F5};

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
        System.out.println("PASS: "+message);
    }
    static void move(int x,int y) throws Exception {
        cpu.writeMemoryLong(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEPOS,(x<<16)|y);
        cpu.writeMemoryLong(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEREL,((x-mouseX)<<16)|((y-mouseY)&65535));
        traps.resetCounter();mouseX=x;mouseY=y;Thread.sleep(180);
    }
    static void button(int state) throws Exception {
        cpu.writeMemoryWord(cpu.getLinkageBlock()+Types.LINKAGE_MOUSEBTN,state);
        traps.resetCounter();Thread.sleep(200);
    }
    static void click(int x,int y) throws Exception {move(x,y);button(1);button(0);}
    static void key(char value) throws Exception {traps.addToKeyboardQueue(value);Thread.sleep(400);}
    static void commands(String text) throws Exception {
        for(char c:text.toCharArray()){traps.addToKeyboardQueue(c);Thread.sleep(140);}
    }
    static int cellX(int x){return 16+16+60*x+20;}
    static int cellY(int y){return 16+52+60*y+20;}

    public static void main(String[] args) throws Exception {
        dir=Path.of(args[1]).toAbsolutePath();
        Files.writeString(dir.resolve("runtimeboot"),"100 LRESPR \"nfa1_ttk\"\n105 LRESPR \"nfa1_tptr_ext\"\n108 OPEN_NEW #9,\"nfa1_environment\":PRINT #9,W_MAX;\",\";DISP_TYPE:CLOSE #9\n110 EXEC_W \"sfa1_tangles_exe\"\n120 OPEN_NEW #9,\"nfa1_exited\":PRINT #9,\"OK\":CLOSE #9\n");
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
        screen=new Screen32(800,600,0,false,null,false,false,0,false,false);
        cpu=new MC68000Cpu(8*1024*1024,screen,350000);
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
        AtomicReference<Throwable> failure=new AtomicReference<>();
        Thread runner=new Thread(()->{try{cpu.executeContinuous();}catch(Throwable e){failure.set(e);}},"SMSQ/E CPU");
        runner.setDaemon(true);runner.start();
        try {
            Thread.sleep(3000);commands("\n\nLRUN nfa1_runtimeboot\n");
            long until=System.nanoTime()+12_000_000_000L;boolean ready=false;
            while(System.nanoTime()<until){
                if(failure.get()!=null)throw new Exception("Emulator failure",failure.get());
                if(near(picture(null).getRGB(554,128),colours[0])){ready=true;break;}
                Thread.sleep(200);
            }
            picture("initial.png");check(ready,"compiled GUI started at 800x600 in mode 32");
            BufferedImage initial=picture(null);
            for(int i=0;i<16;i++)check(near(initial.getRGB(16+528+108*(i%2)+10,16+104+36*(i/2)+8),colours[i]),"colour "+(char)('A'+i));
            move(cellX(0),cellY(2));button(1);move(cellX(4),cellY(2));move(cellX(4),cellY(5));button(0);
            BufferedImage drawn=picture("dragged.png");
            for(int x=0;x<=4;x++)check(near(drawn.getRGB(cellX(x)+20,cellY(2)+20),colours[0]),"fast drag interpolated cell "+x+",2");
            for(int y=3;y<=5;y++)check(near(drawn.getRGB(cellX(4)+20,cellY(y)+20),colours[0]),"vertical drag cell 4,"+y);
            move(cellX(7),cellY(7));
            check(near(picture(null).getRGB(cellX(7)+20,cellY(7)+20),0x192D36),"button release stops painting");
            click(16+528+108+20,16+104+7*36+10);
            move(cellX(6),cellY(6));button(1);move(cellX(7),cellY(6));button(0);
            check(near(picture(null).getRGB(cellX(7)+20,cellY(6)+20),colours[15]),"palette P selects a different paint colour");
            key('r');check(near(picture(null).getRGB(cellX(4)+20,cellY(5)+20),colours[0]),"explicit redraw preserves painted grid");
            key('m');move(mouseX+8,mouseY+8);button(1);button(0);Thread.sleep(300);
            BufferedImage moved=picture("moved.png");
            check(near(moved.getRGB(100,26),0x263C35)&&!near(moved.getRGB(100,18),0x263C35),"native window move changes the client origin");
            check(near(moved.getRGB(cellX(4)+8,cellY(5)+8),colours[0]),"window move redraw preserves grid contents");
            picture("proof.png");
            key('c');check(near(picture(null).getRGB(cellX(4)+8,cellY(5)+8),0x192D36),"clear resets grid");
            key((char)27);
            until=System.nanoTime()+5_000_000_000L;
            while(!Files.exists(dir.resolve("exited"))&&System.nanoTime()<until)Thread.sleep(100);
            check(Files.exists(dir.resolve("exited")),"Escape exits compiled job cleanly");
            Files.writeString(dir.resolve("smoke.json"),"{\"passed\":true,\"resolution\":\"800x600\",\"mode\":32,\"colours\":16,\"compiled\":true,\"checks\":[\"palette\",\"drag\",\"interpolation\",\"release\",\"redraw\",\"move\",\"clear\",\"quit\"]}\n");
        } finally {
            picture("last-screen.png");cpu.endEmulation(false);runner.join(1000);interrupts.stopInterrupts();traps.closeAllFiles();
        }
    }
}
