# Rogue Squad (DN4)
Rogue Squad DN4 is a cross-platform DotNet 4.7.2 build of RogueSquad going against Monogame 3.7.1 and Monogame.Extended 1.1.
While many of the Monogame repos undergo a transition from .NET 4+  to .NET CORE, this repo presents a location where we can write platform compatible code that isn't going to break every few days.

## Local Setup
* Install .NET 4.7+  (4.7.2)
* Install Monogame 3.7.1 
* Copy resources from /Assets/Content.7z to RogueSquad/Content/  (if required)

## Building 
Run the following command to restore dependencies prior to building for the first time:
> nuget restore 