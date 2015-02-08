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



void setup()
{
  //setup keypress vars
  transmission_data[0]=0;
  transmission_data[1]=0;

  //init serial output
  Serial.begin(9600);

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
    Serial.println("Got packet");

    /* Get load the packet into the buffer. */
    // data transmitted, [up, down]
    Mirf.getData(transmission_data);

HandleKeyData();

    /* Set the send address.     */
    //    Mirf.setTADDR((byte *)KEYPADNAME);
    //    /* Send the data back to the client.   */
    //       Mirf.send(data);
    
    /* Wait untill sending has finished
     * NB: isSending returns the chip to receving after returning true.*/
    Serial.println("Reply sent.");
  }


}

void HandleKeyData(){
 //for now i only want to handle key up, to see when a key is released

switch( transmission_data[0]){
 case 0x1:
Serial.println("key 1 released");
break; 
case 0x2:
Serial.println("key 2 released");
break; 
case 0x4:
Serial.println("key 3 released");
break; 
case 0x8:
Serial.println("key 4 released");
break; 
case 0x10:
Serial.println("key 5 released");
break; 
case 0x20:
Serial.println("key 6 released");
keyboard_modifier_keys|=KEY_LEFT_CTRL;
usb_keyboard_send();
keyboard_keys[0] = 'k';
usb_keyboard_send();
keyboard_keys[0] = 0;
usb_keyboard_send();
keyboard_keys[1] = 'f';
usb_keyboard_send();
keyboard_keys[1] = 0;
usb_keyboard_send();
keyboard_modifier_keys = keyboard_modifier_keys& ~KEY_LEFT_CTRL;
usb_keyboard_send();
break; 
}
  
}





