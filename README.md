# Everdusk

Everdusk est un jeu immersif développé avec Unity, mettant en avant des mécaniques de déplacement avancées, des interactions dynamiques avec l'environnement, et des modes de transport variés pour une expérience captivante. Ce projet vise à offrir une jouabilité fluide, intuitive et accessible à tous les joueurs.

## 🏗️ Technologies utilisées

- **Moteur de jeu** : [Unity](https://unity.com/)
- **Langage principal** : C#
- **Gestion de version** : Git
- **Plateformes cibles** : Windows, macOS, Linux, PlayStation, Xbox, Nintendo Switch

---

## 🌟 Fonctionnalités principales

### Mécaniques de déplacement
- **Déplacement fluide et réactif** : Les contrôles intuitifs permettent une navigation précise (ex. : touches WASD pour le mouvement, souris pour ajuster la caméra).
- **Terrain dynamique** : La vitesse de déplacement varie en fonction des types de terrain (marécage, sable, neige) avec des bonus de vitesse dans des zones spécifiques.
- **Esquive et téléportation** : Permet aux joueurs d'éviter les attaques ou de naviguer rapidement en utilisant des compétences spécifiques.
- **Prise en charge des pentes** : Les montées et descentes sont accompagnées d’animations adaptées et de contrôles fluides.
- **Interaction environnementale** : Capacité à interagir avec des objets comme des portes, barrières ou structures via des commandes simples.

### Interaction avec l’environnement
- **Obstacles réalistes** : Les murs, arbres et rochers bloquent le déplacement, mais peuvent être contournés ou manipulés.
- **Zones à effets spéciaux** : Des environnements comme des marécages, des sols glacés ou volcaniques influencent le comportement du personnage.
- **Éléments interactifs** : Possibilité d’utiliser des toits, échelles ou autres structures pour prendre de la hauteur ou accéder à de nouvelles zones.

### Système de sprint et d'esquive
- **Sprint limité par l’endurance** : Augmente temporairement la vitesse de déplacement avec une consommation d'énergie.
- **Esquive directionnelle** : Réagit immédiatement aux commandes pour éviter les attaques ou obstacles.

### Modes de déplacement avancés
- **Montures et véhicules** : Disponibles pour parcourir de grandes distances (chevaux, dragons, véhicules magiques).
- **Déplacements aériens et sous-marins** : Offerts grâce à des compétences ou objets spéciaux.

### Accessibilité et retours visuels
- **Indications directionnelles** : Flèches, lignes lumineuses et aides visuelles pour guider le joueur.
- **Interface claire** : Affiche l’endurance ou l’énergie en temps réel, facilitant la gestion des actions comme le sprint ou l’esquive.

---

## 🚀 Installation et démarrage

### Prérequis
- Unity version 2022.3 LTS ou supérieure.
- Git pour le contrôle de version.

### Étapes d'installation
1. Clonez le dépôt Git :
   ```bash
   git clone https://github.com/<votre-utilisateur>/everdusk.git
   
## Ouvrir le projet dans Unity

1. Lancez **Unity Hub**.
2. Cliquez sur **Open Project** et sélectionnez le dossier `everdusk`.
3. Lancez le jeu en mode **Play** dans l’éditeur Unity.

---

## 🧪 Tests et validation

Les exigences spécifiques sont testées selon les scénarios définis dans le **Tableau des cas de test**.

### Exemples de tests :
- **EX-DEPLACEMENT-MEC-1** : Vérification de la fluidité des contrôles de base (WASD, souris).
- **EX-DEPLACEMENT-ENV-1** : Interaction avec des obstacles physiques et animations de collision.
- **EX-DEPLACEMENT-SYSTEME-1** : Validation de la gestion de l’endurance pendant le sprint.

### Pour exécuter les tests :
1. Ouvrez la scène `TestEnvironment` dans Unity.
2. Lancez les tests à l’aide du package **Unity Test Framework**.

---

## 📁 Structure du projet

```bash
everdusk/
├── Assets/
│   ├── Scripts/                # Scripts C# pour les mécaniques de jeu
│   ├── Art/                    # Modèles, textures, et animations
│   ├── Scenes/                 # Scènes Unity
│   └── Prefabs/                # Objets préfabriqués réutilisables
├── Docs/                       # Documentation du projet
│   └── test_cases.md           # Cas de test détaillés
├── README.md                   # Documentation principale
└── .gitignore                  # Fichiers et dossiers ignorés par Git
🤝 Contribution
