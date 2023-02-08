# WaiterApplication

[![.NET](https://github.com/Phumzakose/WaiterApplication/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Phumzakose/WaiterApplication/actions/workflows/dotnet.yml)

* This is a Waiter Application where waiters select the days they want to work on
* Waiters can also update the days they have selected previously
* The manager can view the working employees and can also clear the waiters on the schedule
* On the welcoming page there are statuses for each day this is to check if the day has enough waiters or not
* If the colour of the day status is green it means the day has enough waiters
* If the colour of the day status is gray it means more waiters are still needed for this day
* And if the colour of the day status is red it means that this day has more than enough waiters
# Installing .NET Core SDK on Ubuntu
* To install .NET Core SDK on Ubuntu 22.04 LTS system, execute the following commands:
* sudo apt install apt-transport-https 
* sudo apt update 
* sudo apt install dotnet-sdk-6.0 
# Installing .NET Core Runtime on Ubuntu
* sudo apt install apt-transport-https 
* sudo apt update 
* sudo apt install dotnet-runtime-6.0 
* Check if the .NET Core is installed successfully by running the command on the terminal: dotnet --version
#Clone The Application To run it Locally
* To clone the application run the command on the terminal : git clone https://github.com/Phumzakose/WaiterApplication.git
* After cloning the apllication run the command on the terminal to change into the waiter application folder: cd WaiterApplication
* Compile the application by running the commands on the terminal:
* Dotnet build
* Dotnet -c Release
* dotnet bin/Release/net6.0/Razor.dll --urls=http://localhost:7103/
# Packages that were used for the database
* To add the Npgsql package run the command on the terminal: Dotnet add package Npgsql
* To add the Dapper package run the command on the terminal: Dotnet add package Dapper
# Running The Application 
* Inside the folder of the WaiterApplication change the directory to Razor by running the command : cd Razor
* Inside Razor run the command on the terminal : dotnet watch
* The application will load and once it is done loading it will open on the browser
