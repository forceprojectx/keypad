using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormMacro
{
    internal static class CodeGen
    {
        #region codebase
        static string codebase = @"
#include <SPI.h>
#include <Mirf.h>
#include <nRF24L01.h>
#include <MirfHardwareSpiDriver.h>


//-------------------------
//    debounce stuff
//-------------------------

byte transmission_data[2];   //contains data to be transmitted, [up, down]

//-------------------------
//    CONSTS
//-------------------------
#define SERVERNAME ""kserv""
#define KEYPADNAME ""keyp1""

#define DEBUG 1

void setup()
{
  //setup keypress vars
  transmission_data[0]=0;
  transmission_data[1]=0;

  //init serial output
#if DEBUG  
  Serial.begin(9600);
#endif
//init keyboard
Keyboard.begin();

  //-------------------------
  //   shutoff the adc....
  //-------------------------
  //23.8.2 ADCSRA – ADC Control and Status Register A
  //Bit 7 – ADEN: ADC Enable
  //Writing this bit to one enables the ADC. By writing it to zero, the ADC is turned off. Turning the
  //ADC off while a conversion is in progress, will terminate this conversion.
  ADCSRA &= ~(1 << ADEN);


  //
  //-------------------------
  //    init wireless
  //-------------------------
  Mirf.spi = &MirfHardwareSpi;
  Mirf.init();

  //Configure reciving address.
  Mirf.setRADDR((byte *)SERVERNAME);

  /*
   * Set the payload length to 2 bytes, the upstate and downstate
   * NB: payload on client and server must be the same.
   */
  Mirf.payload = 2;  
  //Write channel and payload config then power up reciver.  
  Mirf.channel = 110;   
  Mirf.config();


}

void loop()
{
  /*
   * If a packet has been recived.
   *
   * isSending also restores listening mode when it 
   * transitions from true to false.
   */

  if(!Mirf.isSending() && Mirf.dataReady()){
#if DEBUG
    Serial.println(""Got packet"");
#endif
    /* Get load the packet into the buffer. */
    // data transmitted, [up, down]
    Mirf.getData(transmission_data);

    HandleKeyData();
  }//endif


}

void HandleKeyData(){
  //for now i only want to handle key up, to see when a key is released

  switch( transmission_data[0]){
  case 0x1:
#if DEBUG 
    Serial.println(""key 1 released"");
#endif
";
        static string case0 = @" break; 

  case 0x2:
#if DEBUG 
    Serial.println(""key 2 released"");
#endif
";
        static string case1 = @"break; 

  case 0x4:
#if DEBUG 
    Serial.println(""key 3 released"");
#endif
";
        static string case2 = @" break; 

  case 0x8:
#if DEBUG 
    Serial.println(""key 4 released"");
#endif
";

        static string case3 = @"break; 

  case 0x10:
#if DEBUG 
    Serial.println(""key 5 released"");
#endif
";

        static string case4 = @" break; 

  case 0x20:
#if DEBUG 
    Serial.println(""key 6 released"");
#endif 
";

        static string case5 = @"break; 
  }

} ";









        #endregion


        static int[] keys;
        static int modifiers;
        internal static string generateCode(bool usescancode, List<KeyPress> blue,
            List<KeyPress> brown, List<KeyPress> red,
            List<KeyPress> green, List<KeyPress> white,
            List<KeyPress> black)
        {
            keys = new int[6] { 0, 0, 0, 0, 0, 0 };
            modifiers = 0;
            string s1 = ListToCode(usescancode,blue);
            string s2 = ListToCode(usescancode, brown);
            string s3 = ListToCode(usescancode, red);
            string s4 = ListToCode(usescancode, green);
            string s5 = ListToCode(usescancode, white);
            string s6 = ListToCode(usescancode, black);
            return codebase +
                s1 + case0 + s2 + case1 +
                s3 + case2 + s4 + case3 +
                s5 + case4 + s6 + case5
                ;

        }//generatecode

        internal static string ListToCode(bool usescancode, List<KeyPress> str)
        {
            string code = string.Empty;

            foreach (KeyPress k in str)
            {
                int i = 0;
                if (k.IsKeyDown)
                {
                    //press the key
                    // Add k to the key report only if it's not already present
                    // and if there is an empty slot.
                    int k1;
                    if (usescancode)
                    {

                        k1 = (int)Enum.Parse(typeof(KeyHooks.KeysUSB), Enum.GetName(typeof(KeyHooks.KeysScancode), k.ScanCode));
                    }
                    else
                    {
                        //use windows enum
                        k1 = (int)Enum.Parse(typeof(KeyHooks.KeysUSB), k.Key);
                    }
                    if (0x80 == (k1 >> 8))
                    {
                        //its a modifier key, so write the low byte to mod key and continue
                        modifiers |= (k1) & 0xff;
                        code += string.Format("Keyboard.set_modifier({0});Keyboard.send_now();", modifiers);
                        code += Environment.NewLine;
                        continue;

                    }
                    if (keys[0] != k1 && keys[1] != k1 &&
                        keys[2] != k1 && keys[3] != k1 &&
                        keys[4] != k1 && keys[5] != k1)
                    {

                        for (i = 0; i < 6; i++)
                        {
                            if (keys[i] == 0x00)
                            {
                                keys[i] = k1;
                                switch (i)
                                {
                                    case 0: code += string.Format("Keyboard.set_key1({0});Keyboard.send_now();", k1); break;
                                    case 1: code += string.Format("Keyboard.set_key2({0});Keyboard.send_now();", k1); break;
                                    case 2: code += string.Format("Keyboard.set_key3({0});Keyboard.send_now();", k1); break;
                                    case 3: code += string.Format("Keyboard.set_key4({0});Keyboard.send_now();", k1); break;
                                    case 4: code += string.Format("Keyboard.set_key5({0});Keyboard.send_now();", k1); break;
                                    case 5: code += string.Format("Keyboard.set_key6({0});Keyboard.send_now();", k1); break;
                                }
                                break;
                            }
                        }                        
                    }
                   
                }
                else
                {
                    //release the key

                    // Test the key report to see if k is present.  Clear it if it exists.
                    // Check all positions in case the key is present more than once (which it shouldn't be)
                    int k1;
                    if (usescancode)
                    {

                        k1 = (int)Enum.Parse(typeof(KeyHooks.KeysUSB), Enum.GetName(typeof(KeyHooks.KeysScancode), k.ScanCode));
                    }
                    else
                    {
                        //use windows enum
                        k1 = (int)Enum.Parse(typeof(KeyHooks.KeysUSB), k.Key);
                    }
                    if (0x80 == (k1 >> 8))
                    {
                        //its a modifier key, so write the low byte to mod key and continue
                        modifiers &= ~((k1) & 0xff);
                        code += string.Format("Keyboard.set_modifier({0});Keyboard.send_now();", modifiers);
                        code += Environment.NewLine;
                        continue;

                    }
                    for (i = 0; i < 6; i++)
                    {
                        if (0 != k1 && keys[i] == k1)
                        {
                            keys[i] = 0x00;
                            switch (i)
                            {
                                case 0: code += string.Format("Keyboard.set_key1({0});Keyboard.send_now();", 0); break;
                                case 1: code += string.Format("Keyboard.set_key2({0});Keyboard.send_now();", 0); break;
                                case 2: code += string.Format("Keyboard.set_key3({0});Keyboard.send_now();", 0); break;
                                case 3: code += string.Format("Keyboard.set_key4({0});Keyboard.send_now();", 0); break;
                                case 4: code += string.Format("Keyboard.set_key5({0});Keyboard.send_now();", 0); break;
                                case 5: code += string.Format("Keyboard.set_key6({0});Keyboard.send_now();", 0); break;
                            }
                        }
                    }
                   
                }

                code += Environment.NewLine;
            }

            return code;
        }
    }
}
