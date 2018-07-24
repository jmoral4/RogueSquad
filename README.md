# RogueSquad

Rogue squad is a entity based monogame engine in development. Work is underway to clean it up, integrate work from my other networking and multiplayer code-bases, and begin work on making a simple ...rogue-like.. game.

Rogue squad, like many of my projects, was hacked together in a few intense days of coding. That's left me with a fair bit of technical debt which I will be cleaning up ver the coming weeks. 

At it's core, the model of Systems, Components, Entities, and Nodes are sound and I've been able to attach them together in a variety of ways to create new features.

On interesting todo list is thinking about how AI and AI states will be handled given that they need to track their own animation states based on movement or other actions they've taken. 

On the not-so-interesting todo list is cleanup old code, removing dead codepaths, removal of any uneeded libs or refs, and all warnings. 

Nice-to-haves, a platform agnostic (or inclusive) build script (nmake platform x)

Time to get to work..
