#include <SPI.h>
#include <Mirf.h>
#include <nRF24L01.h>
#include <MirfHardwareSpiDriver.h>
#include <avr/sleep.h>

//-------------------------
//    CONSTS
//-------------------------
#define SERVERNAME "kserv"
#define KEYPADNAME "keyp1"
#define RADIO_POWER_PIN  5
#undef DEBUG

//-------------------------
//    debounce stuff
//-------------------------
#define MAXCHECKS 15         //# of checks before a switch is considered debounced
byte debounced_state;        //debounced state of the switches
byte debounced_state_prev;   //previous debounced state of the switches
byte state[MAXCHECKS];       //array maintaining bounce status
byte index;                  //pointer into state
byte transmission_data[2];   //contains data to be transmitted, [up, down]




void setup()
{
  //pull all pins low
  for(int i=0;i<A5;i++){
    pinMode (i, INPUT);  
    digitalWrite (i, LOW); 
  }
  //setup debounce vars
  debounced_state_prev=0;
  debounced_state=0;
  transmission_data[0]=0;
  transmission_data[1]=0;

  //init serial output
#ifdef DEBUG
  Serial.begin(9600);
#endif

  //-------------------------
  //   shutoff the adc....
  //-------------------------
  //23.8.2 ADCSRA – ADC Control and Status Register A
  //Bit 7 – ADEN: ADC Enable
  //Writing this bit to one enables the ADC. By writing it to zero, the ADC is turned off. Turning the
  //ADC off while a conversion is in progress, will terminate this conversion.
  //ADCSRA &= ~(1 << ADEN);
  ADCSRA = 0;

  // initialize the pushbutton pins as an input:
  pinMode(A0, INPUT_PULLUP);
  pinMode(A1, INPUT_PULLUP);
  pinMode(A2, INPUT_PULLUP);
  pinMode(A3, INPUT_PULLUP);
  pinMode(A4, INPUT_PULLUP);
  pinMode(A5, INPUT_PULLUP);

  //
  //-------------------------
  //    init wireless
  //-------------------------
  Mirf.spi = &MirfHardwareSpi;
  Mirf.init();

  //Configure reciving address.
  Mirf.setRADDR((byte *)KEYPADNAME);

  /*
   * Set the payload length to 2 bytes, the upstate and downstate
   * NB: payload on client and server must be the same.
   */
  Mirf.payload = 2;  
  //Write channel and payload config then power up reciver.  
  Mirf.channel = 110;   
  Mirf.config();

  PowerDownRadio();

  sleep();

}

void loop()
{
  //Serial.println(debounced_state,BIN);
  debounce();
  //Serial.println(debounced_state,BIN);
  if(debounced_state_prev != debounced_state){
    //key has been pressed or released

    if(GetKeysUp()!=0){
      PowerUpRadio();

      TransmitKeyData();

      PowerDownRadio();

      sleep();
    }

  }//end if(key is pressed)


}


void debounce(){
  byte i,j; 
  state[index]=readkeys();
  ++index;
  j=0xff;
  for(i=0;i<MAXCHECKS;i++){
    j = j & state[i]; 
  }
  debounced_state_prev=debounced_state;
  debounced_state=j;
  if(index >= MAXCHECKS){
    index =0; 
  }
}

//-------------------------
//    read the keys into an byte
//-------------------------
byte readkeys(){
  byte ret=0x3F;   //3F (0x00111111) 

  //pinc is the analog port. inverted because we are using active low
  ret &= ~PINC;

  return ret;
}

byte GetKeysUp(){
  //set bits that have changed to 1
  byte ret = debounced_state_prev ^ debounced_state;

  //set bits that have changed and are 1 in the current state
  ret = ret ^ debounced_state;
  return ret;
}

byte GetKeysDown(){

  //set bits that have changed to 1
  byte ret = debounced_state_prev ^ debounced_state;

  //set bits that have changed and are 1 in the current state
  ret = ret & debounced_state;
  return ret;
}

void PowerUpRadio(){
  // give power to radio
  //digitalWrite (RADIO_POWER_PIN, LOW);
  //pinMode (RADIO_POWER_PIN, OUTPUT);
#ifdef DEBUG
  Serial.println("Radio Powered, Configuring.");
#endif
  Mirf.init();
  Mirf.config();
#ifdef DEBUG
  Serial.println("Radio configuration complete.");
#endif
}

void PowerDownRadio(){
#ifdef DEBUG
  Serial.println("powering down radio.");
#endif
  Mirf.powerDown();
  //pinMode (RADIO_POWER_PIN, INPUT);  
  //digitalWrite (RADIO_POWER_PIN, HIGH); 
#ifdef DEBUG
  Serial.println("radio powered down");
#endif
}

void TransmitKeyData(){
  //[up, down]
  transmission_data[0]=GetKeysUp();
  transmission_data[1]=GetKeysDown();
#ifdef DEBUG
  Serial.write("Keys that are down::");
  Serial.println(transmission_data[0],BIN);
#endif

  //      Serial.write("Keystate::");
  //      Serial.println(debounced_state,BIN);
  //      Serial.write("Previous Keystate::");
  //      Serial.println(debounced_state_prev,BIN);
#ifdef DEBUG
  Serial.write("Keys that are up::");
  Serial.println(transmission_data[1],BIN);
  Serial.println();
#endif

  Mirf.setTADDR((byte *)SERVERNAME);

  Mirf.send((byte *)transmission_data);

  while(Mirf.isSending()){
  }
}


void sleep(){
  //debounce delays
  delay (50);

  // clear any outstanding interrupts
  PCIFR  |= _BV (PCIF0) | _BV (PCIF1) | _BV (PCIF2);   
  //setup interrupts
  PCICR |= (1 << PCIE1);    // set PCIE1 to enable PCMSK1 scan

  // set PCINT13 PCINT12	PCINT11	PCINT10	PCINT9	PCINT8 
  //to trigger an interrupt on state change 
  PCMSK1 |= 0b111111;
  sei();         // turn on interrupts 


  // disable ADC
  ADCSRA = 0;  
  // turn off various modules
  //PRR = 0xFF; 

  set_sleep_mode (SLEEP_MODE_PWR_DOWN);  
  sleep_enable();
  sleep_cpu ();

  // cancel sleep as a precaution
  //sleep_disable();
}

//the interrupt handler
ISR (PCINT1_vect, ISR_BLOCK)
{
  // cancel pin change interrupts
  PCICR = 0;


  //  Serial.print("A key has been touched ::");
  //  Serial.println(PINC,BIN);

  // cancel sleep
  sleep_disable();


  //  if(PINC==0x3F){
  //    //one of the keys has been released
  //    //it doesnt matter which one, probably
  //    Serial.println("A key has been released");
  //  }
}



