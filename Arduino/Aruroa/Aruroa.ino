#include "FastLED.h"

// 3 wellcome, 1 direction, 1 mode, 1 led count 128 * 3 (384) led
#define BUFFER_SIZE 3 + 1 + 1 + 1 + (128 * 3)

#define LED_PIN 2
#define SAMPLE_LEDS 20
#define STATUS_CONNECTED 1
#define STATUS_DISCONNECTED 0

uint8_t wellcome_length = 3;
uint8_t wellcome_position = 0;
uint8_t ledIndex = 0;
uint8_t status = STATUS_DISCONNECTED;
byte WELLCOME[] = { 0x00, 0x01, 0x02};
byte buffer[BUFFER_SIZE];

CRGB totalLeds[128];

void setup()
{
  Serial.begin(115200);
  FastLED.clear(true);
  FastLED.addLeds<WS2812B, LED_PIN, GRB>(totalLeds, 128);
  FastLED.setBrightness(50);

  Serial.write("\nAurora v.1.0.0");
}

void loop()
{
  switch (status) {
    case STATUS_DISCONNECTED: 
    rainbowFill();
    break;
    
    case STATUS_CONNECTED:
    fetchData();
    break;
  }
}

void fetchData()
{
  if (Wellcome())
  {
    Serial.readBytes(buffer, BUFFER_SIZE);

    int lds = 0;
    int bufferOffset = (3 + 1 + 1 + 1);
    for (int i = 0; i < 128; ++i)
    {
      totalLeds[lds++] = CRGB(buffer[bufferOffset++], buffer[bufferOffset++], buffer[bufferOffset++]);
    }

    FastLED.show();
  }
  else
  {
    status = STATUS_DISCONNECTED;
  }
}

bool Wellcome()
{
  unsigned long last_serial_available = millis();
  Serial.readBytes(buffer, BUFFER_SIZE);

  while (wellcome_position < wellcome_length)
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
  for( int i = 0; i < SAMPLE_LEDS; i++) {
    totalLeds[i] = ColorFromPalette(RainbowColors_p, colorIndex, 50, LINEARBLEND);
    colorIndex += 3;
  }

  FastLED.delay(50);

  if (Serial.available() > 0)
  {
    status = STATUS_CONNECTED;
  }
}
