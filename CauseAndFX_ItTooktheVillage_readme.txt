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
ASWD + left mouse click - Special Attack (hammer or axe only)
right mouse click - Block/Raise Shield
1 - equip hammer
2 - equip sword
3 - equip axe
Q - snap camera to behind player
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
\Assets\Scripts\ElevatorController.cs
\Assets\Scripts\EnemyAttackPower.cs
\Assets\Animations\BossAnimator

Added after Alpha:
Redesigned the Boss room into a multi-floor dungeon.

Parth Arora:

Worked on specific aspects of the game, such as:
- Adding collectibles throughout the scene, such as weapons.
- Introducing currency to the game by incorporating coins and displaying the current status in the menu.
- Implementing an inventory and functionality to grab and change equipment.
- Working on the flow of the game from the lobby to Act 1 and Act 2 by implementing scene changes.
- Adding functionalities and animations to chests and keys for user interaction.
- Implementing the entire tutorial, introducing users to every functionality of the game.
- Adding an NPC blacksmith in Act 2 for upgrading weapons.
- Adding an NPC healer in Act 2 to heal players before they fight the boss.

Elements added and worked on:
- `Assets/Prefabs/Shop_NPC.prefab`
- `Assets/Prefabs/Healer_NPC.prefab`
- `Assets/Scenes/Act_2_Intro.unity`
- `Assets/Scenes/Act_2_Outro.unity`
- `Assets/Scenes/Lobby.unity`
- `Assets/Scenes/Tutorial.unity`
- `Assets/Scripts/coin.cs`
- `Assets/Scripts/equipWeapon.cs`
- `Assets/Scripts/ExitGame.cs`
- `Assets/Scripts/GameStarter.cs`
- `Assets/Scripts/openScene.cs`
- `Assets/Scripts/rpg_collectable.cs`
- `Assets/Scripts/ShopHandler.cs`
- `Assets/Scripts/HealerHandler.cs`
- `Assets/Imported/Coin/coin.mat`
- `Assets/Imported/Coin/Coin.prefab`
- `Assets/Imported/Coin/coinInner.mat`
- `Assets/Imported/Weapons/Prefabs/Sword_01.prefab`
- `Assets/Imported/Weapons/Prefabs/Hammer_01.prefab`
- `Assets/Imported/Weapons/Prefabs/Ax_01.prefab`
- `Assets/Imported/Rust Key/Prefabs/rust_key.prefab`







Ruixuan Yu:

\Assets\Scripts\BGMManager.cs
\Assets\Scripts\BossEnemyController.cs
\Assets\Scripts\BossEnemyController.cs
\Assets\Scripts\NPCDialogHandler.cs
\Assets\Scripts\DamageCalculator.cs
\Assets\Scripts\DialogBoxController.cs
\Assets\Scripts\Door.cs
\Assets\Scripts\EnemyAttackPower.cs
\Assets\Scripts\EnemyPlantController.cs
\Assets\Scripts\EnemySkeletonController.cs
\Assets\Scripts\EnemySlimeController.cs
\Assets\Scripts\NewPlayerController.cs
\Assets\Scripts\NPCDialogHandler.cs
\Assets\Scripts\ShieldDefense.cs
\Assets\Scripts\WalkingNPCDialogHandler.cs
\Assets\Scripts\WeaponAttackPower.cs
\Assets\Animations\BossAnimator.controller
\Assets\Prefabs\Enemies\EnemyPlant.prefab
\Assets\Prefabs\Enemies\EnemySkeleton.prefab
\Assets\Prefabs\Enemies\EnemySlime.prefab
\Assets\Prefabs\NPC\
\Assets\Prefabs\UI\
\Assets\Prefabs\NewPlayerBody.prefab
\Assets\Imported\Character\Animations\Combat.mask
\Assets\Imported\Character\Animations\EnemyPlantController.controller
\Assets\Imported\Character\Animations\NewPlayerAnimator.controller
\Assets\Imported\Character\Animations\NewPlayerNonCombatAnimator.controller
\Assets\Imported\Character\Animations\Sick.mask
\Assets\Imported\Character\Animations\VillagerAnimator.controller
\Assets\Imported\Character\Animations\VillagerOldAnimator.controller
\Assets\Imported\Character\Animations\VillagerSickAnimator.controller
\Assets\Imported\Character\Animations\VillagerSickWalkingAnimator.controller
\Assets\Imported\Character\Animations\VillagerWalkingAnimator.controller

Built original basic action demo that started the project.
Provided bulk of character and enemy controller development, animations, and alpha core gameplay. 
Added after Alpha:
Added unique animations for special attacks
Fixed awkward walking and running animations for player model
Added health bars for enemies
Added detection FOV for enemies
For walking NPCs, stop walking animation and turn to face the player when talking
Added music to overworld, dungeon, and boss fight

Greg Frantzen:

\Assets\Scripts\NPCDialogHandler.cs
\Assets\Scripts\ChestInteraction.cs
\Assets\Scripts\DialogBoxController.cs
\Assets\Scripts\Door.cs
World interaction prototyping. NPC dialogue.  
Added after Alpha:
Added dialogs to walking NPCs

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
Added after Alpha:
Redesigned levels
Added tutorial level


