# Rogue Squad GL (DNCore)
This is the cross-platform .NET CORE build of the engine. 

Presently (12-16-19), Monogame is in a transitory state with many broken components, installers, templates, etc. 
As a result, attempting to build cross-platform can be an exercise in frustration if using anything other than Windows.

I recommend the RogueSquad/DN4/ if you want a branch that will build with relative easy on OSX/Linux

## Local Setup
* Install .Net CORE 2/3
* Install Monogame 
* Copy assets from /Assets/Content.7z

## Building
Run dotnet restore 
Build 
dotnet run RogueSquadGL.dll 