import java.nio.file.*;
import java.util.concurrent.atomic.AtomicReference;
import CPU.MC68000Cpu;
import smsqmulator.*;
import drivers.nxfa.*;

/** Starts the extracted release through its actual boot/start files.
 * Headless, isolated SMSQ/E test; no desktop windows or user disks are used.
 * Reuses the product's existing pointer-test primitives and screen capture.
 */
public class ReleaseCheck extends GuiCheck {
    public static void main(String[] args) throws Exception {
        dir=Path.of(args[1]).toAbsolutePath();
        inifile.IniFile ini=new inifile.IniFile();
        for(String k:new String[]{"NFA_USE","SFA_USE","WIN_USE","MEM_USE","FLP_USE","NFA1"})ini.addOption(k,"","");
        for(String k:new String[]{"SCREEN-MODE","TIME-OFFSET","NFA-FILENAME-CHANGE"})ini.addOption(k,"0","");
        ini.setOptionValue("SCREEN-MODE","3");ini.setOptionValue("SOUND-VOLUME","0");
        SMSQmulator.presetOptions(ini,args[0]);
        ini.setOptionValue("DISABLE-WIN-DEVICE","1");ini.setOptionValue("DISABLE-FLP-DEVICE","1");
        ini.setOptionValue("DISABLE-SFA-DEVICE","0");ini.setOptionValue("NFA_USE","");
        screen=new ProbeScreen();cpu=new MC68000Cpu(16*1024*1024,screen,350000);
        traps=new TrapDispatcher(null,null,ini){
            @Override public void dispatchTrap(int t,MC68000Cpu c){
                if(t==Types.TRAP5){
                    int op=c.data_regs[0];
                    if(op==12||op==37){c.data_regs[0]=0;c.reg_sr|=4;return;}
                    if(op>=32&&op<=35){
                        if(op<=33)screen.fillBlock(c,op==32);else screen.xorBlock(c,op==34);
                        c.data_regs[0]=0;c.reg_sr|=4;c.pc_reg=c.readMemoryLong(c.addr_regs[7])/2;c.addr_regs[7]+=4;return;
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
        Thread runner=new Thread(()->{try{cpu.executeContinuous();}catch(Throwable t){failure.set(t);}},"Release SMSQ/E");
        runner.setDaemon(true);runner.start();
        try{
            Thread.sleep(3000);commands("\n\nDATA_USE \"nfa1_\"\nLRUN nfa1_boot\n");
            long until=System.nanoTime()+90_000_000_000L;
            while(!Files.exists(dir.resolve("tangles_session"))&&System.nanoTime()<until){
                if(failure.get()!=null)throw new Exception("Emulator failure",failure.get());Thread.sleep(100);
            }
            check(Files.exists(dir.resolve("tangles_session")),"extracted release starts through its portable boot file");
            check(Files.readString(dir.resolve("tangles_session")).startsWith("nfa1_pack00_json"),"fresh install selects the bundled starter pack");
            picture("release-start.png");
            for(int i=0;i<16&&!near(screen.pixel(originX+752,originY+170),0x38AE61);i++)key('h');
            check(near(screen.pixel(originX+752,originY+170),0x38AE61),"packaged hints complete the first puzzle");
            picture("release-solved.png");key('q');Thread.sleep(1500);
            check(!near(screen.pixel(originX+752,originY+170),0x38AE61),"packaged player closes before restart");
            commands("LRUN nfa1_start\n");Thread.sleep(3000);
            until=System.nanoTime()+60_000_000_000L;
            while(!near(screen.pixel(originX+752,originY+170),0x38AE61)&&System.nanoTime()<until)Thread.sleep(100);
            check(near(screen.pixel(originX+752,originY+170),0x38AE61),"packaged restart restores saved completed progress");
            picture("release-resumed.png");key('q');Thread.sleep(700);
            Files.writeString(dir.resolve("package-smoke.json"),"{\"passed\":true,\"assertions\":"+checks+",\"boot\":true,\"hints\":true,\"saveAndResume\":true}\n");
        }finally{
            picture("release-last.png");cpu.endEmulation(false);runner.join(1000);interrupts.stopInterrupts();traps.closeAllFiles();
        }
    }
}
