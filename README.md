# herental
A study sandbox project

# Prerequisites

Install and run the RabbitMQ server (tested with v3.5.7)

# Running

To run the project in debugger:

   1. Build ``herental.sln`` (in Visual Studio 2013 or later), it should pull all the dependency Nuget packages
   2. Build should install the ``herental.backend`` Windows service using
    ``C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil $(TargetPath)``
    Should it fail, `rem` the Pre-Build step in the herental.backend.csproj and run an equivalent service installation
    command from the Windows `cmd` console
   3. In the Windows console run:
  
    ``$ net start herental.backend``
  
    To start up the backend service. Backend service will initialize a new LocalDB database.
   4. Run the ``herental`` project to run the MVC web application

