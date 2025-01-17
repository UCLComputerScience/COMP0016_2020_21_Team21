# COMP0016_2020_21_Team21

AR App for Patient Education and Awareness Prior to MRI Scan


# User Manual and Deployment Guide
### Unity
There are two ways to deploy this project on Unity:
1. Download the project zip
2. 
- Navigate to the top of the page and click on `code`
- Copy the url in `Clone with HTTPS`
- Open your terminal and go to the location where you wish to clone the repository
- Type `git clone https://github.com/UCLComputerScience/COMP0016_2020_21_Team21.git`
- Press `Enter` to create your local clone



Open the project in Unity and you will be able to use the repository. 
For more details, please visit https://docs.github.com/en/github/creating-cloning-and-archiving-repositories/cloning-a-repository . 



### Mobile app (APK File)
1. Download the APK file from the master branch > AR MRI final.apk
2. Install the app on your device. 




# IBM Watson
- You will need an IBM Cloud account.
- The Watson Credentials will only be provided to our clients, NTT Data and UCL. 
- The IBM SDK Core (Assets > IBMSDKCore) and Watson (Assets > Watson) file must be used together in order to use the Watson Services. 
- For questions related to the Watson Configuration, please visit https://github.com/watson-developer-cloud/unity-sdk to find out more. 


# Configuring Unity
- Change the build settings in Unity (File > Build Settings) to any platform except for web player/Web GL. The IBM Watson SDK for Unity does not support Unity Web Player.
- If using Unity 2018.2 or later you'll need to set Scripting Runtime Version and Api Compatibility Level in Build Settings to .NET 4.x equivalent. We need to access security options to enable TLS 1.2.
- To set up the credentials for Watson, simply enter the API key and URLs as below


![CredentialsUnity](https://github.com/UCLComputerScience/COMP0016_2020_21_Team21/blob/dfbfd7442a864499016a7b88210688d76f1ce5dd/Watson%20Credentials.png)



# Code Review
### Overview
#### Chatbot.cs
This script is used for the authentication of the Watson API for the voice chatbot. The main logic behind the voice chatbot is 
1. Convert Speech To Text
2. Get the response from Watson Assistant
3. Convert the reponse from text to speech 

#### ChatbotManager.cs
This script is used for the text chatbot. The code is similar to the voice chatbot with just a slight modification.

#### FirebaseManager.cs
The authentication is performed using this script. The results such as "Login Successful" and "Invalid email or password" will be shown to the users.

#### QuizManager.cs
This generation of questions and scores are performed using this script

#### SequenceManager.cs
This is used to configure the sequence of the MRI demonstration. Only admins are allowed to configure the demonstration sequence

#### TableMove.cs
The animations for the movement of the MRI machine are performed using this script. 

#### TapToPlace.cs
The setup of the MRI room, machine and Practitioner are performed when the users tap on a their screen and plane detection is successful. 
