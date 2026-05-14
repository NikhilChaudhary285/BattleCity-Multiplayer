# 🎮 Multiplayer Tank Battle

Real-Time Multiplayer • Photon PUN • Android • iOS • WebGL • Windows • 1–4 Players

A Battle City–inspired real-time multiplayer tank game built with Unity and Photon PUN, focused on scalable multiplayer architecture, synchronized gameplay systems, and clean engineering practices.


------------------------------------------------------------


## 📌 Project Overview

Multiplayer Tank Battle is a cross-platform multiplayer tank combat game supporting 1–4 players per match.

The project was built to deeply understand multiplayer networking concepts such as:

- Real-time synchronization
- Room-based multiplayer architecture
- State consistency
- Scene synchronization
- Player authority
- Latency handling
- Modular gameplay systems

The game targets multiple platforms including:

- Android
- iOS
- WebGL
- Windows


------------------------------------------------------------


## 👨‍💻 My Responsibilities

- Designed and implemented the complete multiplayer architecture using Photon PUN
- Built room creation, matchmaking, and dynamic player count systems
- Implemented synchronized multiplayer gameplay flow
- Developed a player-ready system before gameplay initialization
- Created modular MVP-based multiplayer UI architecture
- Implemented synchronized player spawning
- Designed enemy AI using State and Strategy patterns
- Optimized multiplayer event flow to reduce unnecessary network traffic
- Structured gameplay systems using clean architecture principles


------------------------------------------------------------


## 🌐 Multiplayer Architecture

The networking layer is built using Photon PUN with a room-based multiplayer approach.


### Key Features

- Dynamic room capacity (1–4 players)
- Master-client authoritative game flow
- Automatic scene synchronization
- Deterministic player spawning
- Event-driven synchronization system
- Multiplayer-ready scene management


### Photon Systems Used

- PhotonNetwork
- AutomaticallySyncScene
- Custom Room Properties
- Custom Player Properties
- RaiseEvent
- RPC communication where required


------------------------------------------------------------


## 🧠 Networking Design Decisions

The architecture prioritizes deterministic synchronization and reduced network overhead.


### Why Custom Properties?

Custom Properties were used for:

- Player readiness
- Match state synchronization
- Shared gameplay state
- Room-level settings

Benefits:

- Persistent synchronized state
- Reduced RPC spam
- Easier reconnection handling
- Better scalability


### Why RaiseEvent Instead of Excessive RPCs?

RaiseEvent was preferred for gameplay events requiring lightweight synchronization.

Benefits:

- Lower network overhead
- Better event filtering
- More scalable event handling
- Cleaner multiplayer architecture


------------------------------------------------------------


## 🎯 Gameplay State Management

Gameplay logic and networking logic are separated to maintain clean architecture.


### Player & AI States

- Idle State
- Move State
- Attack State
- Dead State


### Enemy AI Strategies

- Chase Player
- Target Base
- Patrol Area

The AI system uses:

- State Pattern
- Strategy Pattern
- ScriptableObject-based configuration


------------------------------------------------------------


## ⚙️ Player Ready Synchronization System

One of the major multiplayer systems implemented was the player-ready workflow before gameplay starts.


### Problem

Players joined rooms at different times, causing:

- Desynchronized spawning
- Gameplay initialization issues
- Missing player references


### Solution

Implemented a synchronized ready-check system using Photon Player Properties.


### Workflow

1. Players join the multiplayer room
2. Each player confirms readiness
3. Master Client checks all readiness states
4. Scene synchronization begins only after all players are ready
5. Players spawn deterministically


------------------------------------------------------------


## 🚀 Cross-Platform Support

The architecture was designed to remain stable across:

- Android
- iOS
- WebGL
- Windows

Special focus was placed on:

- Network consistency
- UI responsiveness
- Platform-independent gameplay flow


------------------------------------------------------------


## 🧩 Engineering Challenges & Solutions

### Challenge: Multiplayer Game Start Synchronization

Players loading scenes at different times caused desynchronization.


### Solution

Implemented:

- Player-ready synchronization
- Photon Custom Player Properties
- Master-client controlled game start
- Auto scene synchronization


### Challenge: Handling Latency in Real-Time Gameplay

Frequent synchronization created unnecessary network load.


### Solution

- Reduced excessive RPC calls
- Used event-driven synchronization
- Applied local prediction for non-critical visuals
- Optimized update frequency


------------------------------------------------------------


## 🏗 Clean Architecture & Design Patterns

The project follows modular and scalable architecture practices.


### Design Patterns Used

- MVP Pattern
- Singleton Pattern
- State Pattern
- Strategy Pattern
- Event-Driven Architecture
- Object Pooling


### Benefits

- Modular gameplay systems
- Easier scalability
- Cleaner dependencies
- Improved maintainability
- Multiplayer-friendly structure


------------------------------------------------------------


## 📂 Core Systems

### Multiplayer Systems

- Matchmaking
- Room Management
- Lobby Flow
- Player Synchronization
- Spawn Management
- Ready State System


### Gameplay Systems

- Tank Combat
- Enemy AI
- Shooting System
- Health System
- Collision System
- State Machines


### UI Systems

- Multiplayer Lobby UI
- Room Browser
- Player List
- Ready Status UI
- Match Start Flow


------------------------------------------------------------


## 📈 Impact & Results

- Stable multiplayer gameplay for up to 4 players
- Scalable room-based networking architecture
- Deterministic multiplayer synchronization
- Cross-platform compatibility
- Reduced network overhead
- Clean and extensible project structure


------------------------------------------------------------


## 🎥 Gameplay & Source

🎥 Gameplay Video

💻 GitHub Repository


------------------------------------------------------------


## 📚 Key Learnings

This project helped strengthen understanding of:

- Multiplayer networking fundamentals
- Photon PUN architecture
- Real-time synchronization
- Event-driven networking
- Multiplayer state management
- Scene synchronization workflows
- Clean multiplayer architecture
- Cross-platform Unity development


------------------------------------------------------------


## ⭐ Feel free to explore this repository and connect with me. I’m always open to discussions around Unity multiplayer development, Photon networking, scalable architecture, and real-time gameplay systems.
