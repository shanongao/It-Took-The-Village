Cause And FX - It Took the Village

Members:

Shanon Gao
Parth Arora
Ruixuan Yu
Greg Frantzen
Daniel Layson

Start Scene File: 

\Scenes\Lobby

How to Play:

ASWD - Walk
hold left Shift - Run
Spacebar - Jump
left mouse click - Attack/Swing
right mouse click - Block/Raise Shield
1 - equip hammer
2 - equip sword
3 - equip axe
Q - snap camera to behind player (still in progress)
x - open equipment menu, hover mouse and click to select equipment
ESC - Pause menu
mouse movement - pan camera
E - perform on-screen hint/interact with NPCs


This game is our take on the classic Action-RPG. All aspects of the game presented for meeting rubric requirements are contained in a relatively small area by the terrain and by tree and fence placement. NPC interaction teaches you the controls, gives you the plot of the game and gives you hints as to what you can anticipate you need to do to progress. NPCs are huddled into two areas and they are context-sensitive. This game's unique feature is that it is act-based and the world changes based on which act you are in. The dialogue and behavior of the NPCs change. There are 4 types of enemies overall including the boss. There is currently no ending implemented. The game must be manually exited via the ESC game menu. Returning to the main menu is also functional. There are collectable coins hidden in the village and a working currency tracker, but no shop has been implemented yet. The player must speak to an NPC to learn of the key location to unlock the door to the boss room or explore and notice the chest guarded by a large enemy. An HP system has been implemented into the UI and the player can die. Enemies give chase to the player. The chest behind the church and the door to the boss room are interacitble. The player should not be able to escape the intended bounds. 

Known issues:

- Camera clipping
- Boss clipping through ground
- Player runs through enemies 
- No game victory handling or end-game/story
- Key gets swung with the weapon
- UI scaling issues and text quality is low
- Character is much stronger than the enemies
- No indication of enemy health
- Weapons not yet placed throughout game to aid in sense of progression
- Player clips through church prefab

Teammate Contributions:

Shanon Gao: 

Boss room prefab, Boss prototype
\Assets\Scripts\BossEnemyController.cs
\Assets\Scripts\EnemyAttackPower.cs
\Assets\Animations\BossAnimator

Parth Arora:

\Assets\Scripts\GameStarter.cs
\Assets\Scripts\openScene.cs
\Assets\Scripts\coin.cs
Equipment menu and scene changing, collectables. 

Ruixuan Yu:

\Assets\Scripts\NPCDialogHandler.cs
\Assets\Scripts\DamageCalculator.cs
\Assets\Scripts\DialogBoxController.cs
\Assets\Scripts\Door.cs
Built original basic action demo that started the project.
Provided bulk of character and enemy controller development/alpha core gameplay. 


Greg Frantzen:

\Assets\Scripts\NPCDialogHandler.cs
\Assets\Scripts\ChestInteraction.cs
\Assets\Scripts\DialogBoxController.cs
\Assets\Scripts\Door.cs
World interaction prototyping. NPC dialogue.  

Daniel Layson:

\Assets\Scripts\InGameMenu.cs
\Assets\Scripts\GameStarter.cs (fix for time un-stopping on game resume)
\Assets\Scripts\ExitGame.cs
\Assets\Materials\*
\Assets\Prefabs\Tree Root
\Assets\Prefabs\House Root
\Assets\Terrains\Terrain\*
\Assets\Textures\*
Level design, requirement direction, early prototyping, textures, terain, story and concept.
Considerable early vision work that did not make the alpha as others were assigned to make better versions.



