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

#define LED_PIN 2
#define LED_COUNT 50

uint8_t wellcome_position = 0;
uint8_t ledIndex = 0;
uint8_t status = 0;
byte WELLCOME[] = { 0x00, 0x01, 0x02};

#define BUFFER_SIZE 3 + 1 + (LED_COUNT * 3)
byte buffer[BUFFER_SIZE];

CRGB led_array[LED_COUNT];

void setup()
{
  Serial.begin(115200);
  FastLED.clear(true);
 222 FastLED.addLeds<WS2812B, LED_PIN, GRB>(led_array, LED_COUNT);
  FastLED.setBrightness(50);
}

void loop()
{
  if(status == 0)
  {
    rainbowFill();
  }
  else
  {
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
    for (int i = 0; i < LED_COUNT; ++i)
    {
      led_array[lds++] = CRGB(buffer[bufferOffset++], buffer[bufferOffset++], buffer[bufferOffset++]);
    }
    FastLED.delay(30);
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
  for( int i = 0; i < 30; i++)
  {
    led_array[i] = ColorFromPalette(RainbowColors_p, colorIndex, 50, LINEARBLEND);
    colorIndex += 3;
  }
  FastLED.delay(15);

  if (Serial.available() > 0)
  {
    status = 1;
  }
}
