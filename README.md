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

## Prerequisistes and Installation

- Git
- Git LFS
- Unity
- A system that can run this

You may have to install the Invector Basic Locomotion package from the Asset store. 

[Making a Basic Detective Game in Unity](BasicTutorial.md)
