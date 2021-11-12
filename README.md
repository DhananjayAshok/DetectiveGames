# Detective Games

This is a repository and Unity project to enable quick development and release of Detective Games in Unity. The goal of this project is to make a flexible, but simple to use tool that allows game makers who know barely any scripting, animation etc to create a detective game. 

## Game Format:
Using this tool you can create a game with the following features:
- A 3rd person game with a controllable character
- Suspects who have dialogue trees in response to questions from the player
- Clues and a clue database system
- A "Lab Report" system where you can submit clues to the Lab and get additional information about it
- A "Confrontation" system where you can confront a suspect with a clue and see their response. 

The game follows the following format:
- Scene 1: "The Crime Scene". The player can walk around, talk to suspects and discover clues. The player can not confront anyone at this time. To finish this scene the player can step into an "Exit Scene Portal"
- Scene 2: "The Follow Up Scene". The player can walk around, talk to suspects, discover clues and confront suspects with the clues they have found. To finish this scene the player must accuse a suspect of the crime.
- Scene 3: "The Courtroom Scene". The player is in a courtroom with a suspect, a judge and spectators (if the game dev wants), the player cannot move in this scene. The judge/suspects asks specific questions and offers certain defences and the player must select a set of clues/witness testimonies that they have found to answer each one. If the player answers sufficiently well the prosecution is successful and they win the game. 

It is possible to accuse the wrong person in Scene 2 and go through the courtroom scene (but the player will lose no matter what, often because they will fail to answer a question as it is not possible to). 

The rest of this document details how to start the most basic version of this, I then cover how to make a more advanced customized game using the tool. 

## Making a Detective Game in Unity - Basic Tutorial

  

A simple tutorial that will allow you to make a detective game using the existing setup - almost no code, should be pretttty simple.

  

### Step 1: Making a basic scene

Create a new scene and add any and all scenery to make it look pretty. (See Unity tutorials on scene creation and lighting)
You will have to do this twice (once for Scene 1, once for Scene 2) but the process is overall the same. 
Once you have done that open Scene 1 and go to File->Build Settings-> Add Open Scenes, then close Scene 1, open Scene 2 and do the same. We will add Scene 3 later as the procedure is different.  

  

Make sure that before you move on you add a God prefab object to Scene 1 (only, not Scene 2). This object has a script component which stores

- Total number of clues in the entire game
- Maximum number of lab reports available to the detective.

### Step 2: Readying the scene
- Create a "Mind Palace" this is a structure that the player will teleport to when they pause the game and want to review their clues. Make sure you create a GameObject within this Mind Palace at the exact location you want the player to spawn. Tag this object "PlayerPausePoint". (You can just drag the Mind Palace prefab if you want)
- Drag the Exit Portal prefab onto the scene and create some physical structure around the green collider under the structure child. You can reshape the green collider to change the portal shape and size.


### Step 3: Making a player character

- Make sure you have an fbx model of a humanoid before you start this step - let us call it model.fbx
- Go to Invector-> Basic Locomotion -> Basic Controller and select model.fbx
- The controller should have been added to the scene, take the PlayerAttachment prefab and make it a child of the controller. 


### Step 4: Making Clues
- Drag the clue prefab onto the scene. 
- Go to the ClueScript component and set the variables as required. The Clue Autopsy text field is the message returned if the user chooses to send this clue to the lab (leave it blank for a default message). The isAutopsySuccess boolean is true iff the clue gives back valuable information after being sent to the lab. 
- Give this clue prefab a child object, it should have a physical mesh renderer and collider,but no rigidbody component. Ensure that the child is tagged as clue. 
### Step 5: Making Suspects

This has many steps so the basic setup is
#### Part A: Basic Setup
- Drag the suspect prefab onto the scene
- Give the body child of this object a child of its own. This should have a physical representation that is a humanoid fbx model ( you can get some from Mixamo). Make sure it has a collider, give it a tag of suspect
	  -  Make sure you import only the character. 
	  - To do so from Mixamo go to characters and select one, 
	  - Select the t-pose animation 
	  - Download the fbx for unity format
	  - Click the fbx in the editor go to rig->animation type and select humanoid
	  - Go to Materials and extract Materials as well as extract Textures to any folder of your choosing
#### Part B: Suspect Data and Configuration
- Give the suspect a name
- If you are in the second scene add the Accused Clips - what the suspect will respond with when accused
- If you are in the second scene add the After Accused Clips - what they respond with when spoken to after being accused
- If you are in the second scene add Confrontations. These are a set of clues that you could present to the suspect and the audioclips they could react with when confronted with that clue, the isSuccessful boolean is true iff the clue is a good one to use for this suspect.
- Add audio for the default confrontation, this is the audioclip set the suspect will respond with if given any clue that is not specified in the above list
- If you are in the second scene - Set the isGuilty variable
#### Part C: Make the TreeCreationScript and Add Data
- Right click in your scene folder, then go to Create and select C# script. Name the script whatever you'd like, but bear in mind you can't change it later.
- In the script, make it inherit from TreeCreationScript. Delete all the existing functions.
- Create a new function called public override void CreateTree(). 
- In the function, create a baseTree and whatever children you would like the tree to have. Tip: use the CTFL function to simplify creating a tree. Look at the script binaryDepth2 for examples.
- Once the script is complete, drag it onto the Suspect object. 
- Next, add the Questions and Audio clips. In the Suspect object, create as many questions and audio clips as you created trees, and populate them as you wish.


#### Part D: Add Animations
- Create an animation controller
- Go to Assets->Animations->Suspects->Default Suspect Controller and copy all the states, paste these states into the controller
- Make sure you replicate the same connectivity structure found in the default controller, so set the default transition from entry to Idle and give both Confrontations a transition to Idle. 
- If you want to customize animation clips for any state you can do so in Motion animation of each state. 
- Then go to the object with the fbx model (the child of Body) and add an Animator (it may already have one which is okay), set the controller to this one you just made
- Go to the suspect object and in the suspect script set the animator to the one of the object with the fbx model
