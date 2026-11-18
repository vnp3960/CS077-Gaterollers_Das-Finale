# 🐈 Pilot Cat: Skies of Settaria
**CS077 Finale Project Documentation & Team Charter** · **Team Gateroller (Gaterollin)**

> **Live Product Page:** [https://pilot-cat.vercel.app/](https://pilot-cat.vercel.app/) · **Milestone:** Action 12A — Independence Day

---

## 📌 Executive Summary
**Pilot Cat** is an original 2D side-scrolling aircraft game built in Unity by **Team Gateroller**. Conceived during a 10-minute whiteboard session in Action 11B as a complete departure from the *VOID RUNNERS* framework, the project introduces an original world, cast, and narrative.

* **The Premise:** Players pilot the *Golden Claw* biplane as **Duke ("Pilot Cat")**, an ace Catlandian aviator trained by the legendary **Trigger**. Flying across the **Catlantic** toward **K-9topia**, Duke smuggles high-value **Cat Nip** while evading the **Watch Dogs Order** and the **International Cat Police**.
* **The Twist:** Despite his outlaw reputation, Duke secretly donates his smuggled earnings to fund orphanages across **Settaria**.
* **Character Traits:** 💤 Sleeping · ✈️ Flying · 🐈 Collecting Cat Nip

---

## 👥 Team Roster & Ownership

| Role | Member | Primary Responsibilities |
| :--- | :--- | :--- |
| **Team Lead** | Vinh Khang Nguyen Pham | Project direction, technical coordination, Unity gameplay architecture & integration. |
| **Coordinator** | Pascual Ramirez | Team communication, task delegation, repository structure, and submission evidence. |
| **Developer** | Roman Calles | Gameplay programming, C# systems implementation, and functional testing. |
| **Developer** | Luisa Ramirez | Narrative design, character development, game design, and documentation. |
| **Media & PR** | Kevin F | Visual presentation, promotional graphics, branding assets, and project media. |

---

## 🌍 World & Cast Reference

| Entity | Classification | Role in Story & Gameplay |
| :--- | :--- | :--- |
| **Settaria** | World | Overarching fictional world containing all continents and oceans. |
| **Catlandia / K-9topia** | Territories | Duke's home continent (Catlandia) and target smuggling territory (K-9topia). |
| **Catlantic** | Environment | The expansive ocean setting where flight missions take place. |
| **Duke (Pilot Cat)** | Protagonist | Playable feline ace piloting the custom vintage biplane, the **Golden Claw**. |
| **Cat Nip** | Collectible | Primary smuggling resource that drives scoring and progression. |
| **Watch Dogs Order** | Opposing Faction | International police force pursuing Duke across the skies. |
| **Rouge** | Main Antagonist | Duke's childhood friend from the orphanage who tips off the Watch Dogs; final boss. |
| **Mason** | Ally / Support | Childhood friend providing Watch Dogs intel and in-flight radio support. |
| **Trigger** | Mentor | Legendary flight ace from Strangereal who trained Duke. |

---

## ⚙️ Gameplay Systems & Unity Architecture

| System | Gameplay Function | Core C# Scripts |
| :--- | :--- | :--- |
| **Flight & Controls** | Multi-axis horizontal/vertical movement, dynamic banking tilt, and propeller spin. | `PlayerMover.cs`, `PlaneTilt.cs`, `Spin.cs` |
| **World & Camera** | Parallax sky/ocean scrolling, moving obstacles, and camera-facing sprites. | `BackgroundScroll.cs`, `ScrollLeft.cs`, `SkyPan.cs`, `FaceCamera.cs` |
| **Combat & Spawning** | Projectile firing with particle effects and automated enemy/obstacle waves. | `Bullet.cs`, `Spawner.cs` |
| **Scoring & Progression** | Cat Nip resource collection, live score UI updates, and mission state tracking. | `ScoreKeeper.cs` |

---

## 📋 Master Requirements Tracker (731 Points)

> **Immediate Objective:** Assign an owner to each requirement below and update status tags (`NOT STARTED`, `IN PROGRESS`, `NEEDS REVIEW`, `COMPLETED`) as milestones are verified.

| # | Requirement | Pts | Owner | Status |
| :---: | :--- | :---: | :--- | :--- |
| **1** | **Working Project Foundation** | 150 | `Unassigned` | `Pending` |
| **2** | **Save and Restore Progress** | 50 | `Unassigned` | `Pending` |
| **3** | **Settings That Persist** | 50 | `Unassigned` | `Pending` |
| **4** | **Branching Story or Mission State** | 75 | `Unassigned` | `Pending` |
| **5** | **Inventory or Structured Collection** | 75 | `Unassigned` | `Pending` |
| **6** | **Public Release on Approved Platform** | 100 | `Unassigned` | `Pending` |
| **7** | **Front-Facing HTML Product Page** | 75 | `Unassigned` | `Pending` |
| **8** | **Project T-Shirt Design + Mockup** | 3 | `Unassigned` | `Pending` |
| **9** | **Illustrated Player Quick Start** | 3 | `Unassigned` | `Pending` |
| **10** | **External Playtest and Revision** | 75 | `Unassigned` | `Pending` |
| **11** | **Automated Regression Tests** | 75 | `Unassigned` | `Pending` |

<details>
<summary><strong>📂 View Detailed Deliverable Criteria (Click to Expand)</strong></summary>

<br>

1. **Working Project Foundation (150 pts):** Working source code and build, authored program logic, meaningful tests, run guide, evidence map, and individual explanation.
2. **Save and Restore Progress (50 pts):** Save meaningful player progress, close and reopen the game, demonstrate restoration, and handle invalid save files.
3. **Settings That Persist (50 pts):** Create useful settings, validate changes, save settings, and demonstrate restoration after reopening (separate from progress data).
4. **Branching Story or Mission State (75 pts):** Implement at least two meaningful outcomes, define outcome conditions, and demonstrate state-transition testing.
5. **Inventory or Structured Collection (75 pts):** Add, remove, and inspect items; test inventory limits and invalid actions.
6. **Public Release (100 pts):** Publish an accessible working build with versioned release notes and installation/run instructions on an approved platform.
7. **HTML Product Page (75 pts):** Responsive website with screenshots, accurate features, credits, accessibility info, and a working release/download link.
8. **Project T-Shirt Design (3 pts):** Original project graphic, editable source or export, readable PNG, shirt mockup, and short explanation.
9. **Illustrated Player Quick Start (3 pts):** One-page illustrated controls guide (source + export) and documentation of one person trying the instructions.
10. **External Playtest and Revision (75 pts):** Obtain consent from at least three testers, document feedback, implement improvements, and provide retest evidence.
11. **Automated Regression Tests (75 pts):** Run normal, boundary, and failure tests; demonstrate a defect detected by the test suite.

</details>

---

## 🛡️ Governance, Evidence & Project Standards

* **Evidence Standards:** All submissions are backed by Unity Editor scene screenshots, Play Mode captures, authored C# scripts, testing logs, and individual contribution records.
* **AI Verification Protocol:** Any AI-assisted technical work requires logged prompts/responses, verification against official Unity documentation, in-engine testing, and documented review decisions (kept, modified, or rejected).
* **Core Presentation Pillars:** 🎨 Original Creativity · 💻 Functional Unity Architecture · 👥 Transparent Collaboration · 🧪 Verified Testing · 🌐 Public Accessibility

---

*Pilot Cat: Skies of Settaria — An Original Production by Team Gateroller.* 🐈✈️
