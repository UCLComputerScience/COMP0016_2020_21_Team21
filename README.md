# COMP0016_2020_21_Team21

AR App for MRI demo


# User Manual and Deployment Guide
Unity
There are two ways to deploy this project on Unity:
1. Download the project zip
2. 

Mobile app (APK File)
1. Download the APK file from xxx
2. Install the app on your device. 


# IBM Watson
- You will need an IBM Cloud account.

# Configuring Unity
- Change the build settings in Unity (File > Build Settings) to any platform except for web player/Web GL. The IBM Watson SDK for Unity does not support Unity Web Player.
- If using Unity 2018.2 or later you'll need to set Scripting Runtime Version and Api Compatibility Level in Build Settings to .NET 4.x equivalent. We need to access security options to enable TLS 1.2.

# Watson SDK
- The IBM SDK Core (Assets -> IBMSDKCore) and Watson (Assets -> Watson) file must be used together in order to use the Watson Services. 
- The Watson Credentials will only be provided to our clients, NTT Data and UCL.









