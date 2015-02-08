

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
#define SERVERNAME "kserv"
#define KEYPADNAME "keyp1"

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
usb_init();


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
    Serial.println("Got packet");
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
    Serial.println("key 1 released");
#endif
usb_keyboard_press(52,0x02);
___press(54);
___release(54);
___press(55);
___release(55);
___press(19);
___release(19);
___press(28);
___release(28);
___press(9);
___release(9);
___press(10);
___release(10);
___press(6);
___release(6);
___press(21);
___release(21);
___press(15);
___release(15);
___press(47);
___release(47);
___press(48);
___release(48);
___press(4);
___release(4);
___press(18);
___release(18);
___press(8);
___release(8);
___press(24);
___release(24);
___press(12);
___release(12);
___press(7);
___release(7);
___press(11);
___release(11);
___press(23);
___release(23);
___press(17);
___release(17);
___press(22);
___release(22);
___press(95);
___release(95);
___press(51);
___release(51);
___press(20);
___release(20);
___press(13);
___release(13);
___press(14);
___release(14);
___press(27);
___release(27);
___press(5);
___release(5);
___press(16);
___release(16);
___press(26);
___release(26);
___press(25);
___release(25);
___press(29);
___release(29);
___press(44);
___release(44);
___press(30);
___release(30);
___press(30);
___release(30);
___press(31);
___release(31);
___press(32);
___release(32);
___press(33);
___release(33);
___press(34);
___release(34);
___press(35);
___release(35);
___press(36);
___release(36);
___press(37);
___release(37);
___press(38);
___release(38);
___press(39);
___release(39);
___press(45);
___release(45);
___press(46);
___release(46);


 break; 

  case 0x2:
#if DEBUG 
    Serial.println("key 2 released");
#endif
break; 

  case 0x4:
#if DEBUG 
    Serial.println("key 3 released");
#endif
 break; 

  case 0x8:
#if DEBUG 
    Serial.println("key 4 released");
#endif
break; 

  case 0x10:
#if DEBUG 
    Serial.println("key 5 released");
#endif
 break; 

  case 0x20:
#if DEBUG 
    Serial.println("key 6 released");
#endif 
break; 
  }

} 
