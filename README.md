A game about exploring floating islands in your airship. inspired by airships-qts (airship mechanics), terraria (exploaration and combat) ,barotrauma (missions and perhaps coop mp) and forts (weapon mechanics).
The game is still in very early development
I dont have an actual name for this yet
also the code is undocumented, basically uncommented and made of spagetti by a noob
It is also very inconsistent
The game uses the godot engine
I do not have an actual name for this



Folder Structure:

-Game (where everything goes)
  -Content (the games vanilla content is here, content and core is sepparated because i will add an option to add other content folders as a built in modding option.)
    -Assets (images and resources)
    -Entities (read by code, contains scenes that can be spawned in as entities, for example as enemies)
    -Internal (can contain everything, not read by the game, has to be used manually)
    -Items (scenes for the items go here, read by the game)
    -Tiles (scenes for the tiles go here, read by the game)
    -Worldgen (scenes for worldgeneration go here, not yet read by the game)
    -Crafting (does not yet exist, crafting recipes will go here)

  -Core(the essential background work of the game, this is also where the code is)
    -Assets (images and resources used by the game itself)
    -Events (the event system, not yet fully implemented. events will also be networked at some point for multiplayer)
    -GUI (code and scenes for the games GUI)
    -Items (the base forms of the items as well as their code but without any configuration)
      -Crafting (everything related to crafting)
    -Visual (shaders and co, empty as of now)
    -World 
      -Entities (contains the unconfiguered base version of entities)
      -Grids (contains code for grids and ships)
      -Tiles (contains the unconfiguered base version of the tiles)
      -Worldgen (contains the games world generation system)
