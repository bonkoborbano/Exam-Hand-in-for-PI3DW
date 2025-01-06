The idea came from the monster-chasing Garry’s Mod games, where you are endlessly chased by a monster that is faster than you. So, basically, a simple game of tag. Inspired by professional tag (yes, that is a thing), I created an arena and some obstacles like in the real games. You start in one corner, and your opponent starts in the opposite corner. It is an endless runner-type game, with a timer to track progress.

Scripts The game has 5 scripts: CharacterControllerTest, EnemyBehaviour, AudioManager, ReloadScene, and Timer.

• ReloadScene 
o A very simple script that reloads the scene when the player clicks the restart button.
By reloading the scene the game is reset and can be played again.

• AudioManager and Timer 
o Singleton patterns, having their methods called in other scripts. They take care of most SFX, the soundtrack, and counting the time between loading the level and getting caught.

• CharacterControllerTest 
o Responsible for the movement and orientation of the player. Additionally, it has logic to handle camera positions, ragdoll effects, and some SFX. The script is made for Unity’s Character Controller component, which can handle both movement and collision. Aside from movement and orientation, one of the most important functions, OnControllerCollisionHit(), handles everything that happens once the player is caught, like camera settings, restraining movement, SFX, time tracking, and the ragdoll effect.

• EnemyBehaviour 
o Handles what the enemy does before and after a collision with the player. It does so using the NavMeshAgent component, giving it a transform.position to chase and another one to return to. It also controls the animation between the two states. To check when the player is caught, a set of float variables is compared to the distance from the object the enemy is set to chase. If those float variables match, the player has been caught, and the enemy will change its behavior.

• Level 
o The level was built using ProBuilder. It consists of a plane, four slim cubes as walls and stairs, elongated cubes, and cylinders stitched together in various ways as obstacles. A NavMesh was added and baked into the level in order for the enemy to navigate and traverse it. Outside the actual level, a 3D arena and a crowd were added. These have no effect on the gameplay.

• Materials 
o To spice up the level, an assortment of materials was used on the various parts of the level. A brick texture was used on the walls, while different concrete textures were used for the floor and obstacles. These textures were not altered much other than some tiling sizes. o A few simple materials were made to change the colors of some elements of the outer arena. Another was created for “fake” spotlights along the octagon structure. This material used emission to give the sphere objects a glow that would simulate a bright spotlight.

• Lights o The light in the scene consists of two point lights above the level with a range that will cover the edges of the crowd. These lights were baked into the scene.
• Character Models 
o Two models were imported to represent the player and the enemy. The player was rigged to use ragdoll functionality

Game Objects
In the hierarchy, the most important, aside from the level, are the Enemy, Player (BananaMan), Lights, GameManager, and SoundManager.

• Enemy object: 
o Nav Mesh Agent to control how the enemy interacts with the level and where it should go. 
o Collider to check for collision with the Player object. 
o Audio Source to play footsteps in 3D space so they won’t be heard within a certain distance of the player. 
o Animator to switch between the running animation while chasing the player and the walking animation once the player is caught. 
o Script (EnemyBehaviour) to handle the logic of when to execute commands for these components.

• Player (BananaMan) has 5 types of components: 
o Character Controller to enable controlling the character with WASD keys and check for collision. 
o Rigidbody, Character Joint, and Capsule Collider attached to the armature of the 3D model to enable ragdoll physics. 
o Script (CharacterControllerTest) to handle the execution of these components and whether the UICanvas is active or not.

• GameManager (holds the UI canvas which shows the Timer, game-over text, and restart button): 
o Timer is a text object that holds the Timer script, which controls how the time is displayed. 
o Game-over text and restart button appear once the player is caught. On the button is a script (ReloadScene) that calls a method to reload the scene and thus restart the game.

• SoundManager 
o AudioManager script attached, which holds the audio clips that are played throughout the game. 
o The child objects hold the audio sources that play the SFX and music to control volume.

Task and time spent:
CharacterControllerScript - 12 hrs 
Enemy Behaviour - 7 hrs 
NavMesh Implementation - 3 hrs 
Ragdoll physics 4 (8) - hrs 
UI 1 hr 
Materials - 2 hrs 
ProBuilder (Level design) - 5 hrs 
Sound - 3 hrs 
Lights - 4 hrs


Resources:
How to Make an In-Game Timer in Unity - Beginner Tutorial - YouTube
How to use Animation Transitions (Unity Tutorial) - YouTube
Triggering Ragdoll Physics (Unity Tutorial) - YouTube 
Unity NavMesh Tutorial - Basics - YouTube 
FIRST PERSON MOVEMENT in 10 MINUTES - Unity Tutorial - YouTube
Unity AUDIO MANAGER Tutorial - YouTube 
Audience Crowd | Characters | Unity Asset Store 
Banana Man | 3D Humanoids | Unity Asset Store 
Max - iClone Character | Characters | Unity Asset Store 
Urban Night Sky | 2D Sky | Unity Asset Store 
Basic Motions FREE | 3D Animations | Unity Asset Store 
Brick Textures 4k | 2D Textures & Materials | Unity Asset Store 
5 Concrete Materials #1 | 2D Concrete | Unity Asset Store 
Brick Material Red Rough-Hewn | 2D Brick | Unity Asset Store 
Plank Textures PBR | 2D Wood | Unity Asset Store 
Effort Sounds (Male) - NPC/Player Audio Pack | Voices Sound FX | Unity Asset Store 
Damage Sounds (Male) - NPC/Player Audio Pack | Voices Sound FX | Unity Asset Store 
Free Deadly Kombat | Audio Sound FX | Unity Asset Store 
Free Crowd Cheering Sounds | Audio Sound FX | Unity Asset Store 
Hexagon Colloseum free VR / AR / low-poly 3D model | CGTrader 

