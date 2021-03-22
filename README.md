# COMP0016_2020_21_Team21

AR App for MRI demo


# User Manual and Deployment Guide
Unity
There are two ways to deploy this project on Unity:
1. Download the project zip
2. 
- Navigate to the top of the page and click on `code`
- Copy the url in `Clone with HTTPS`
- Open your terminal and go to the location where you wish to clone the repository
- Type `git clone [url]`
- Press `Enter` to create your local clone



Open the project in Unity and you will be able to use the repository. 
For more details, please visit https://docs.github.com/en/github/creating-cloning-and-archiving-repositories/cloning-a-repository to find out more. 



Mobile app (APK File)
1. Download the APK file from xxx
2. Install the app on your device. 




# IBM Watson
- You will need an IBM Cloud account.
- The Watson Credentials will only be provided to our clients, NTT Data and UCL. 
- The IBM SDK Core (Assets > IBMSDKCore) and Watson (Assets > Watson) file must be used together in order to use the Watson Services. 
- For questions related to the Watson Configuration, please visit https://github.com/watson-developer-cloud/unity-sdk to find out more. 


# Configuring Unity
- Change the build settings in Unity (File > Build Settings) to any platform except for web player/Web GL. The IBM Watson SDK for Unity does not support Unity Web Player.
- If using Unity 2018.2 or later you'll need to set Scripting Runtime Version and Api Compatibility Level in Build Settings to .NET 4.x equivalent. We need to access security options to enable TLS 1.2.















