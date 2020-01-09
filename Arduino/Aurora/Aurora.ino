/*
 * Aurora .NET
 * Copyright (c) 2020 RapidDev
 * Leszek Pomianowski
 * https://github.com/rapiddev/Aurora/
 * 
 * Inspired by Bambilight
 * Copyright (c) 2016 Matthias Böffel
 * https://github.com/MrBoe/Bambilight/
 */

#include "FastLED.h"

//3 - wellcome,  1 - mode, 256 * 3 - led count  (768)
#define BUFFER_SIZE 3 + 1 + (256 * 3)

#define LED_PIN 2

uint8_t wellcome_position = 0;
uint8_t ledIndex = 0;
uint8_t status = 0;
byte WELLCOME[] = { 0x00, 0x01, 0x02};
byte buffer[BUFFER_SIZE];

CRGB totalLeds[256];

void setup()
{
  Serial.begin(115200);
  FastLED.clear(true);
  FastLED.addLeds<WS2812B, LED_PIN, GRB>(totalLeds, 256);
  FastLED.setBrightness(50);

  Serial.write("\nAurora v.1.0.0");
}

void loop()
{
  if(status == 0)
  {
    rainbowFill();
  }
  else{
    fetchData();
  }
}

void fetchData()
{
  if (Wellcome())
  {
    Serial.readBytes(buffer, BUFFER_SIZE);

    int lds = 0;
    int bufferOffset = 3;
    for (int i = 0; i < 256; ++i)
    {
      totalLeds[lds++] = CRGB(buffer[bufferOffset++], buffer[bufferOffset++], buffer[bufferOffset++]);
    }
    FastLED.delay(100);
    FastLED.show();
  }
  else
  {
    status = 0;
  }
}

bool Wellcome()
{
  unsigned long last_serial_available = millis();
  Serial.readBytes(buffer, BUFFER_SIZE);

  while (wellcome_position < 3)
  {
    if (Serial.available() > 0)
    {
      last_serial_available = millis();
      if (Serial.read() == WELLCOME[wellcome_position])
      {
        wellcome_position++;
      }else{
        wellcome_position = 0;
      }
    }

    if (millis() - last_serial_available > 3000)
    {
      return false;
    }
  }
  wellcome_position = 0;
  return true;
}

void rainbowFill()
{
  ledIndex++;
  uint8_t colorIndex = ledIndex;
  for( int i = 0; i < 256; i++) {
    totalLeds[i] = ColorFromPalette(RainbowColors_p, colorIndex, 50, LINEARBLEND);
    colorIndex += 3;
  }

  FastLED.delay(10);

  if (Serial.available() > 0)
  {
    status = 1;
  }
}
