# Making a Detective Game in Unity - Basic Tutorial

  
A simple tutorial that will allow you to make a detective game using the existing setup - almost no code, should be pretttty simple.


## Step 0: Make a Menu Scene
You can copy the scene from the demo scene or make your own and drag the menu canvas prefab onto the scene. Just make sure there is a button which has the MainMenuProgressScript as a component and add the MainMenuProgressScript->MenuAdvanceScene as a listener to its OnClick property. Go to File->Build Settings and add the currently open scene to the build order. 

# Making Scene 1 and Scene 2

## Step 1: Making a basic scene

Create a new scene and add any and all scenery to make it look pretty. (See Unity tutorials on scene creation and lighting)
You will have to do this twice (once for Scene 1, once for Scene 2) but the process is overall the same. 
Once you have done that open Scene 1 and go to File->Build Settings-> Add Open Scenes, then close Scene 1, open Scene 2 and do the same. We will add Scene 3 later as the procedure is different.  

  

Make sure that before you move on you add a God prefab object to Scene 1 (only, not Scene 2). This object has a script component which stores

- Total number of clues in the entire game
- Maximum number of lab reports available to the detective.

## Step 2: Readying the scene
- Create a "Mind Palace" this is a structure that the player will teleport to when they pause the game and want to review their clues. Make sure you create a GameObject within this Mind Palace at the exact location you want the player to spawn. Tag this object "PlayerPausePoint". (You can just drag the Mind Palace prefab if you want)
- Drag the Exit Portal prefab onto the scene and create some physical structure around the green collider under the structure child. You can reshape the green collider to change the portal shape and size.


## Step 3: Making a player character

- Make sure you have an fbx model of a humanoid before you start this step - let us call it model.fbx
- Go to Invector-> Basic Locomotion -> Basic Controller and select model.fbx
- The controller should have been added to the scene, take the PlayerAttachment prefab and make it a child of the controller. 


## Step 4: Making Clues
- Drag the clue prefab onto the scene. 
- Go to the ClueScript component and set the variables as required. The Clue Autopsy text field is the message returned if the user chooses to send this clue to the lab (leave it blank for a default message). The isAutopsySuccess boolean is true iff the clue gives back valuable information after being sent to the lab. 
- Give this clue prefab a child object, it should have a physical mesh renderer and collider,but no rigidbody component. Ensure that the child is tagged as clue. 
## Step 5: Making Suspects

This has many steps so the basic setup is
### Part A: Basic Setup
- Drag the suspect prefab onto the scene
- Give the body child of this object a child of its own. This should have a physical representation that is a humanoid fbx model ( you can get some from Mixamo). Make sure it has a collider, give it a tag of suspect
	  -  Make sure you import only the character. 
	  - To do so from Mixamo go to characters and select one, 
	  - Select the t-pose animation 
	  - Download the fbx for unity format
	  - Click the fbx in the editor go to rig->animation type and select humanoid
	  - Go to Materials and extract Materials as well as extract Textures to any folder of your choosing
### Part B: Suspect Data and Configuration
- Give the suspect a name
- If you are in the second scene add the Accused Clips - what the suspect will respond with when accused
- If you are in the second scene add the After Accused Clips - what they respond with when spoken to after being accused
- If you are in the second scene add Confrontations. These are a set of clues that you could present to the suspect and the audioclips they could react with when confronted with that clue, the isSuccessful boolean is true iff the clue is a good one to use for this suspect.
- Add audio for the default confrontation, this is the audioclip set the suspect will respond with if given any clue that is not specified in the above list
- If you are in the second scene - Set the isGuilty variable
### Part C: Make the TreeCreationScript and Add Data
- Right click in your scene folder, then go to Create and select C# script. Name the script whatever you'd like, but bear in mind you can't change it later.
- In the script, make it inherit from TreeCreationScript. Delete all the existing functions.
- Create a new function called public override void CreateTree(). 
- In the function, create a baseTree and whatever children you would like the tree to have. Tip: use the CTFL function to simplify creating a tree. Look at the script binaryDepth2 for examples.
- Once the script is complete, drag it onto the Suspect object. 
- Next, add the Questions and Audio clips. In the Suspect object, create as many questions and audio clips as you created trees, and populate them as you wish.


### Part D: Add Animations
- Go to Assets->Animations->Suspects->Default Suspect Controller and Ctrl+D to duplicate it
- If you want to customize animation clips for any state you can do so in Motion animation of each state. 
- Then go to the object with the fbx model (the child of Body) and add an Animator (it may already have one which is okay), set the controller to this one you just made
- Go to the suspect object and in the suspect script set the animator to the one of the object with the fbx model

## Step 6 (Optional): Make Environment Characters and Wanderers
These are objects/humanoids that walk around the scene and do certain actions. The player cannot interact with them in any way but the wanderers can play an audioclip if they come into contact with the player. Use these characters to populate the scene with NPC's who are not particularly people of interest and other automated objects (e.g. a drone moving around the scene). 
- To do so first make sure to mark all the objects where you want the AI to be able to walk over as static (top right of inspector when selecting the object). This should be done for all planes, cubes and surfaces that you expect them to be able to stand on. 
- Then go to Windows->AI->Navigation and select "Bake". This will show you blue spots on the scene view, these are the areas which your AI can walk over, if they don't seem right you might be missing some static objects or you may have overlapping objects. You need to do these 2 steps only once for all environment agents. If you add more structures or paths then bake again
- To actually make the character you will need to create a single gameobject that holds the body. (This could be a humanoid FBX that you drag onto the scene or any simple shape). Ensure that it has a collider. 
- Make sure to give it a NavMeshAgent if you want it to move around. Give it an audio source if you want it to play audioclips at any point. Give it an animator if you want it to play humanoid animations like idle and walking etc, set the animation controller to the Environment Wanderers Controller or a duplicate of it (Ctrl+D) if you want to customize animations.
- If you want the character to move between locations regularly, add the locations to the GoalSet GoalWaypoints list. You can create empty gameobjects and add them to this for convinience. 
- If you want the character to stay at the waypoint for longer before going to the next one change the waitTimeAtWaypoint float. 
### Part A (Optional): Customizing Animations
- If you want to customize animation that will happen when they reach the location go to the duplicate of the environment wanderers controller that you are using for this character and do the following changes
- If you want to change an existing clip, click on a state and change the motion. 
- If you want to delete a clip go to the AtDestinationSubstates and delete the **last state i.e New State with highest number**. You will then need to update the noDestinationAnimationClips variable in the script of this character to be the number of states left. 
- If you want to add a clip then create a new state in the AtDestinateSubstates statemachine (along with all the other New State variables). Create a transition from the entry to this new state and give it a condition of RNGInt Equals N-1 where N is the number of clip states that now exist in this layer (e.g. if after you add another state there are 5 New State states then make the condition RNGInt Equals 4). Create a transition from this state to itself with no condition. Then create a transition from this state to the exit, add a condition on this of isMoving true. Remove the has Exit Time option for this exit transition. Finally you have to update the noDestinationAnimationClips variable in the character

## Step 7 (Optional): Making Teleporters
Theser are objects that allow the player to jump from one spot on the scene to another. It is very useful if you want to have 2 closed rooms that the player cannot "walk" between but you want both of them to be accessible in the same scene.( e.g. a pirate ship at sea where the murder happens and the nearest port could both be linked with a teleporter). 
- To do so simply drag the Teleporter prefab onto the scene. 
- Place each of the spots in the desired locations and they are now linked. 
- If you want audio to play when the player teleports add clips in the audioGroup. 


# Making the Court Scene
## Step 1: Making the Player, Judge and Spectators

- Create a new scene and after Ctrl+Shift+B add it to the list of scenes just like you did the other two. 
- Drag the CourtSceneCanvasPrefab onto the scene
- Drag any FBX model onto the scene and tag it Player to make it a player object. Under animation controller select the CourtRoomPlayer Controller and give it an audiosource component
- Similarly create the Judge object and set its controller
- You can do the same for any spectators you might want but no audiosource

## Step 2:  Creating Accused characters
- Drag any FBX model 
- Tag it Accused
- Give it an AudioSource
- Drag a AccusedScript onto it
- Fill out the accused name and add audio clips that you want to play if they are convicted/ declared not guilty (This audioclip is the only thing that plays at the games conclusion so if you want the player to say something etc make sure the clip includes it)

### Filling out Line of Questioning Objects (LOQs)
The LOQ holds information on Lines of Questioning. Each will consist of 4 phases, in each of these phases the audio will go player, judge, accused, accused, judge, player. This will allow you to make most permutations and combinations of actors speaking. E.g. if you wish to have the player speak a line, then the accused speak a line while the judge remains silent you can click the skip option on the judge clips, and the second accused+player clips. This will result in only those two lines said in order. 

Phase 1: Pre questioning phase. 
Phase 2: Questioning phase where accused poses a question to the player
Phase 3: Player response phase where the player must pick a set of clues and statements/testimonies to answer the question
Phase 4: Line of Questioning Conlcusion (different based on success or failure of player response)

The LOQ has information on the question posed to the player, as well as the answers marked in lists of clueNames and suspectNames. If they player answers with the same clueNames and statementNames (suspectNames) the LOQ is a success. You can limit the total number of clues + statements the player can submit for this LOQ and you can also specify a cutoff such that even if the player does not guess all of the clues, statements correctly if they guess some number of them it is sufficient to be considered a success.

Each successful LOQ raises the score by 1, at the end of all provided LOQs the game checks if the score is greater than the minimum requirement stated by the user and iff it is the conclusion is success. 

### Repeat for all Accusable Characters
Repeat the above process for characters who are reasonable suspects i.e you want the player to be able to accuse them and take them to court. It is okay if they overlap and look weird in the scene view of unity, when you actually transition to the scene the  game will delete all of these but the suspect that the player is accusing. 

# Final Steps
## Step 1: Fixing Lighting
You may need to pre-bake lighting for all the scenes. Go to Windows-> Rendering->Lighting and compute lightmaps/ bake lighting with each scene open.  

## Step 2: Build and Play
That's it. You're done! 
- Go to File->Build Settings-> Player Settings and change the details to fit your needs. Pick the intended target platform here (you can change this easily). 
- When you are ready go to File->Build Settings-> Build and Run
