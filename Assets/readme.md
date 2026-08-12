# RoadWise – Road Safety Simulator
Assignment for Integrated Project / STLD

**GitHub Repository:** https://github.com/J1zuki/StarryCJO
**Gameplay Walkthrough:** 

# ROADWISE
**3D Interactive Road Safety Educational Simulator**

---

## Project Overview
RoadWise is an interactive road-safety simulation created using Unity. The game puts the player inside a neighborhood that looks like a place in Singapore. This area has HDB blocks, a deck, a 7-Eleven store, roads, pedestrian crossings, traffic lights, cars, people and a bus stop.

The goal of RoadWise is to teach players about the importance of crossing roads. This is done through situations instead of just giving information in text form.

As the player moves through the game they face both safe crossing situations. At first they see what happens when someone jaywalks. Then they are shown the way to use a pedestrian crossing and the traffic-light system.

The game uses interactions with people, conversations, moving cars how people act and tasks that need to be completed to help the player learn.

In the end RoadWise mixes gameplay, with road-safety lessons. It helps players stay aware not jaywalk use the places to cross and follow the traffic signals when they cross a road.

---

## Game Objective
The main objective of RoadWise is to complete a series of road-safety interactions and successfully demonstrate the correct way to cross a road.

The player progresses through the level by:

1. Exploring the HDB neighbourhood.
2. Talking to the Store Owner.
3. Attempting the jaywalking scenario.
4. Learning about the dangers of jaywalking.
5. Talking to the Police Officer.
6. Travelling towards the pedestrian crossing.
7. Observing an NPC demonstrate an unsafe crossing.
8. Using the pedestrian traffic-light button.
9. Waiting for the correct traffic signal.
10. Talking to the Girl NPC.
11. Performing the safe-crossing sequence.
12. Reaching the bus stop to complete the level.

Successful completion awards the player **100 points**.

---

## Design Process
RoadWise was designed around the idea of **learning through experience**.

Instead of immediately telling the player the correct answer, the level first allows the player to experience or observe unsafe road behaviour. Feedback is then provided to explain why the behaviour is dangerous.

The player is subsequently guided towards the correct road-crossing process.

The experience follows this learning structure:
**Explore → Experience Unsafe Behaviour → Receive Feedback → Observe → Practise Safe Behaviour → Complete Level**

The environment was designed to resemble a familiar Singapore neighbourhood so that the road-safety situations feel more relatable to the intended audience.

Important environmental elements include:
* HDB residential blocks
* HDB void deck
* 7-Eleven store located under the void deck
* Main road
* Pedestrian crossing
* Pedestrian traffic lights
* Traffic-light control button
* Moving vehicles
* Street lights
* NPC pedestrians
* Police Officer NPC
* Store Owner NPC
* Girl NPC
* Bus stop

The level layout guides the player naturally from the starting area towards increasingly important road-safety interactions.

---

## Learning Objectives
RoadWise aims to teach players several important road-safety principles:

* Avoid jaywalking.
* Cross roads only at appropriate crossing locations.
* Remain aware of surrounding traffic.
* Observe traffic signals before crossing.
* Wait for the pedestrian signal before entering the road.
* Look for approaching vehicles before crossing.
* Understand that drivers may not always see or react to pedestrians immediately.
* Use pedestrian traffic-light controls where available.
* Observe safe behaviour demonstrated by others.
* Apply the correct road-crossing method independently.

---

## User Stories
* As a player, I want clear instructions so that I know what objective I should complete next.
* As a player, I want to interact with NPCs so that I can learn road-safety information through the game.
* As a learner, I want to experience the consequences of jaywalking so that I understand why it is dangerous.
* As a player, I want feedback after making an unsafe decision so that I understand what I did incorrectly.
* As a player, I want to observe NPC behaviour so that I can compare unsafe and safe road-crossing methods.
* As a player, I want to interact with a pedestrian traffic light so that I can practise using a proper road crossing.
* As a learner, I want to practise the correct road-crossing procedure so that I can apply the knowledge outside the game.
* As a player, I want clear completion feedback so that I know when I have successfully completed the simulation.

---

## Gameplay Flow
The main gameplay progression is:
**Start Game**
↓
**Explore HDB Void Deck**
↓
**Talk to Store Owner**
↓
**Try Jaywalking**
↓
**Jaywalking Consequence / Game Over Feedback**
↓
**Learn Why Jaywalking Is Dangerous**
↓
**Talk to Police Officer**
↓
**Go to Traffic Light**
↓
**Observe NPC Crossing Behaviour**
↓
**Interact with Traffic-Light Button**
↓
**Wait for Green Pedestrian Signal**
↓
**Talk to Girl NPC**
↓
**Cross the Road Safely**
↓
**Proceed to Bus Stop**
↓
**Level Complete**
↓
**Receive 100 Points**

---

## Controls
| Control           | Action                         |
| ----------------- | ------------------------------ |
| **W / A / S / D** | Move Player                    |
| **Mouse**         | Control Camera                 |
| **E**             | Interact / Talk to NPC         |
| **B**             | Use Pedestrian Traffic Light   |
| **UI Buttons**    | Start, Continue, Next and Quit |

The game also displays an instruction panel during gameplay to remind the player of the main objectives and interaction controls.

---

# Features

## Existing Features
### Start Menu
RoadWise begins with a welcome screen displaying the game title and a Start button before the player enters the simulation.

### 3D Singapore-Inspired Environment
The game contains a neighbourhood environment inspired by Singapore, including:
* HDB blocks
* Void decks
* 7-Eleven
* Roads
* Pedestrian crossings
* Traffic lights
* Street lights
* Trees and vegetation
* Bus stop
* Moving vehicles

### Player Exploration
Players can freely move around the level and explore the environment while following the displayed objectives.

### Objective / Instruction System
An instruction panel is displayed during gameplay to guide the player through the intended sequence of interactions.

Examples include:
* Talk to Store Owner
* Try Jaywalking
* Talk to Police
* Go to Traffic Light
* Watch NPC Cross
* Use Traffic Light
* Talk to NPC Girl
* Cross Safely
* Go to Bus Stop

### NPC Interaction System
Players can interact with different NPCs using the interaction key.

Important NPCs include:
* Store Owner
* Police Officer
* Girl NPC
* Pedestrian NPCs

Each NPC plays a different role in communicating or demonstrating road-safety information.

### Jaywalking Scenario
The player can attempt to jaywalk across the road.
This demonstrates an unsafe road-crossing decision and allows the game to provide immediate educational feedback.

### Jaywalking Failure Feedback
If the player performs the unsafe jaywalking action, the game displays a failure screen explaining that the player has been caught jaywalking.
The player is then encouraged to continue to the next part of the simulation to learn the correct road-crossing method.

### Police Officer Interaction
The Police Officer explains the danger of jaywalking and why crossing incorrectly can cause accidents.
This reinforces the educational purpose of the scenario after the player experiences the incorrect behaviour.

### NPC Road-Crossing Demonstration
NPC pedestrians are used to visually demonstrate road-crossing behaviour.
An unsafe example is presented to show players what should not be done when crossing the road.

### Pedestrian Traffic-Light System
The player can interact with a pedestrian traffic-light control button.
The traffic light changes between its pedestrian signal states, allowing the player to practise waiting for the appropriate signal before crossing.

### Moving Vehicle System
Vehicles travel along the roads throughout the level, creating an active traffic environment.
This increases the importance of observing the road before attempting to cross.

### Dialogue and Feedback System
Dialogue and instructional messages are used throughout the simulation to:
* Explain road-safety concepts
* Communicate objectives
* Provide feedback
* Warn the player about unsafe behaviour
* Guide the player towards the correct action

### Safe Crossing Scenario
After observing the unsafe example, the player is given the opportunity to demonstrate the correct road-crossing behaviour.
The player must use the designated pedestrian crossing and follow the traffic-light system before crossing.

### Mission Progression
Gameplay interactions are organised into a sequence so that the player learns each road-safety concept before progressing towards the final objective.

### Bus Stop Final Objective
After successfully completing the road-crossing scenario, the player proceeds towards the bus stop as the final destination.

### Score System
Successfully completing the road-safety simulation rewards the player with:
**+100 Points**

### Completion Screen
At the end of the experience, RoadWise displays a completion screen informing the player that they demonstrated the correct road-safety behaviour.

---

## Features Left to Implement / Possible Future Improvements
The current prototype contains the main gameplay and road-safety learning experience. Future improvements could include:
* Additional road-safety scenarios
* Distracted walking and mobile-phone scenarios
* More pedestrian behaviours
* More advanced vehicle AI
* Additional NPC dialogue
* Sound effects for traffic and pedestrian signals
* Voice-over instructions
* Additional road environments
* Difficulty levels
* More detailed scoring system
* Player performance statistics
* Road-safety quiz after completing the level
* Achievement system
* Improved accessibility options
* Improved environmental optimisation and visual polish

---

# Game Mechanics

## NPC Interaction
The player approaches designated NPCs and presses **E** to begin an interaction.
NPC interactions are used to provide information and progress the player through the learning sequence.

---

## Jaywalking Detection
The game detects when the player attempts to cross the road using an unsafe location.
The player then receives feedback informing them that the action represents dangerous road behaviour.

---

## Traffic-Light Interaction
The pedestrian traffic light can be activated using **B** when the player is within the correct interaction area.
The traffic-light system provides the player with a visual indication of when it is appropriate to cross.

---

## Vehicle Movement
Vehicles move along designated road paths to simulate active traffic.
Their movement makes the road-crossing scenarios more realistic and requires the player to pay attention to the surrounding environment.

---

## NPC Behaviour
NPCs are used both as interactive characters and as visual examples of pedestrian behaviour.

This allows the player to learn through:
**Dialogue + Observation + Player Action**
rather than text alone.

---

## Mission System
Objectives are completed in a planned sequence.
Each interaction helps move the player towards the next stage of the simulation until the final destination at the bus stop is reached.

---

## Feedback System
RoadWise provides immediate feedback when the player performs an unsafe or correct action.
Unsafe behaviour results in corrective information, while correct behaviour eventually leads to successful level completion and points.

---

# Technologies Used
* **Unity 6** – Game engine used to create the RoadWise simulation
* **C#** – Gameplay scripting and interaction logic
* **Unity Input System** – Player and interaction input
* **Unity UI** – Menus, instructional panels, dialogue and game feedback
* **TextMeshPro** – In-game text and UI
* **NavMesh / AI Navigation** – NPC navigation and movement
* **Unity Physics** – Triggers, colliders and interaction detection
* **3D Models and Materials** – Environment, characters, vehicles and road assets

---

# Testing

## Player Movement Testing
* Tested player movement around the HDB environment.
* Checked that the player can navigate between objectives.
* Verified that environmental colliders prevent unintended movement through objects.
* Checked movement around roads and pedestrian crossings.

## NPC Interaction Testing
* Approached each interactive NPC.
* Tested the **E** interaction input.
* Confirmed that the correct dialogue appears.
* Confirmed that interactions progress the intended gameplay sequence.

## Jaywalking Testing
* Entered the unsafe road-crossing area.
* Confirmed that jaywalking is detected.
* Confirmed that the failure feedback appears.
* Confirmed that the player can continue to the educational section afterwards.

## Police Officer Testing
* Interacted with the Police Officer.
* Verified that road-safety dialogue appears correctly.
* Checked that the player can continue towards the pedestrian crossing.

## NPC Behaviour Testing
* Confirmed NPC pedestrians perform their intended movement.
* Checked the unsafe road-crossing demonstration.
* Verified that instructional feedback appears during the demonstration.

## Traffic-Light Testing
* Approached the pedestrian traffic-light pole.
* Pressed **B** to interact with the traffic light.
* Confirmed that the traffic-light state changes correctly.
* Checked that the signals are visible to the player.
* Verified that the player can continue with the safe-crossing objective.

## Vehicle Testing
* Observed vehicles travelling along the road.
* Checked that vehicles follow their intended paths.
* Tested road-crossing gameplay while vehicles are active.

## Safe Crossing Testing
* Followed the intended pedestrian-crossing route.
* Activated the traffic light.
* Waited for the correct signal.
* Crossed using the pedestrian crossing.
* Confirmed that the safe-crossing objective is completed successfully.

## Final Objective Testing
* Travelled towards the bus stop after completing the road-crossing scenario.
* Confirmed that reaching the final objective triggers the completion sequence.
* Verified that the player receives **100 points**.
* Verified that the completion screen appears correctly.

---

# Credits

## Development
RoadWise was developed as part of an educational Integrated Project focusing on interactive media, game development and road-safety awareness.

## Content
* Road-safety scenarios were created for educational purposes.
* Game dialogue and learning objectives were designed around safe pedestrian behaviour.
* The environment was designed to resemble a Singapore residential neighbourhood.

## Media and Assets
* 3D environment assets and audio used within the project are either educational resources or appropriately sourced third-party assets.
* 3D Models, Textures, Animations are original work.

---

# Assistive AI
AI tools such as ChatGPT were used during the development of RoadWise as an assistive learning and development tool.
AI assistance was mainly used for:
1. **C# Script Development**
   Assistance was used to understand and implement selected gameplay systems such as NPC interaction, mission triggers, pedestrian traffic-light behaviour and player interaction.

2. **Debugging**
   AI was used to help identify programming errors, Unity component issues and gameplay logic problems during development.

3. **Code Refinement**
   AI assistance was used to improve code organisation, naming, comments and readability.

4. **Game Design Documentation**
   AI was used to assist in organising and refining documentation such as level descriptions, gameplay flow, mechanics and learning objectives.

5. **UI and Dialogue Refinement**
   AI was used to help improve instructional text, NPC dialogue and player feedback so that road-safety information could be communicated more clearly.

All AI-assisted outputs were reviewed, modified, tested and adapted to meet the requirements of the RoadWise project. AI was used as a supporting development tool and did not replace the project's design decisions, implementation, testing or final development work.

---

# Acknowledgements
* Developed using Unity 6.
* Inspired by Singapore road environments and pedestrian road-safety practices.
* Created as part of an Integrated Project assignment.
* RoadWise was designed to explore how interactive media and game-based learning can be used to communicate important road-safety knowledge.

---

# RoadWise
**Stop. Look. Listen. Cross Safely.**
