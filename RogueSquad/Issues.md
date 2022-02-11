
# Open/Known Issues
This is not quite a burndown list but will help inform refactoring and updates

* Movement Component ID (really any entity id) needs to either be an int to keep the object size small for cache coherence
* Perhaps textures and texturenames should be loaded outside of GameWorld?
* Collision completely broken and O(N+N^2) performance
* the mousestate triggers too quickly, this will affect any future mouse based code (like selected a character etc) particularly if it needs to toggle something on/off
* Move from AI.ini to a JSON file which can be deserialized into an object and used directly


* Code cleaniness. AITestbed is getting pretty polluted. Need to extract reusable elements out. Put rendering into a Rendering System. Put movement into a Movmeent System/etc. 
