# Aurora
[Created with ![heart](http://i.imgur.com/oXJmdtz.gif) in Poland by RapidDev](https://rdev.cc/)<br />
Open alternative to Ambilight written in C# for Windows 10.
Aurora uses the Drawing library for the Net Framework to acquire screen colors and then send them to a controller like Arduino or ESP-based boards.


## How does it work?
The Aurora gets a pixel bar from your screen, then reads the colors of individual pixels and sends them to Arduino / ESP.<br/>
All configuration is done from the Windows application level. You don't have to change anything in the code for Arduino. Unless you want to change the communication port for the RGB led strip, which is set to 2 by default.

## What is needed?
To run the Aurora application you will need:
- .NET Framework 4.8 installed
- Windows 10 version at least 1909
- Atmega / ESP device compatible with Arduino
- At least 20 addresible RGB led strips

#### RGB Led's
The program written for Arduino is configured by default for the most popular addressable LEDs, i.e. WS2812B.
If for some reason you want to change it, edit this code in the Aurora.ino file
```c
  FastLED.addLeds<WS2812B, LED_PIN, GRB>(totalLeds, 256);
```

## Illustrative photos
The following pictures show the appearance of the application
<br/><br/>
![Aurora Settings](https://github.com/rapiddev/Aurora/blob/master/.gitimages/aurora-application-settings.png?raw=true)


### Copyright
I wrote the entire application for Windows 10 using basic libraries.<br />
I was inspired to write by a repository created by Matthias Böffel. He writes his own Ambilight alternative: [Bambilight on GitHub](https://github.com/MrBoe/Bambilight/)