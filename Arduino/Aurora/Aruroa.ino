#include "FastLED.h"

//Buf size is: LED COUNT, EFFECT NUM, LED COLORS, MAX 64
#define BUFSIZE 194
#define TIMEOUT 3000
#define OFFLINE 0
#define ONLINE 1
uint8_t STATUS = OFFLINE;

void setup()
{
  Serial.begin(115200);
}

void loop()
{
  if(STATUS == OFFLINE)
  {
    //Show sample animation
    effectSample();
  }else{
    //Effect from serial
    effectInput();
  }
}

void effectSample()
{

  //Check if USB connection is available
  if (Serial.available() > 0)
  {
    STATUS = ONLINE;
  }
}

void effectInput()
{

  //Check if there is a connection
  if (!waitForPreamble(TIMEOUT))
  {
    STATUS = OFFLINE;
  }
  else
  {
    Serial.readBytes(buffer, BUFSIZE);
    for (int i = 0; i < BUFSIZE; i++)
    {
      Serial.write((char)buffer[i]);
    }
  }
}
