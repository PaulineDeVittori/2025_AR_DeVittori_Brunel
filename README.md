# Jeu en réalité augmenté

Une application de réalité augmentée réalisée avec Unity et AR Foundation. 
Elle met en scène un chien interactif qui peut manger, courir, et réagir à l’environnement, à travers des marqueurs image et la détection de plans.

---

## 🎯 Fonctionnalités

-  Reconnaissance d’images (chien, nourriture, balle)
-  Le chien mange quand la nourriture est proche
-  Le chien court quand la balle est proche
-  Génération d'herbe sur les plans horizontaux détectés
-  Interactions en touchant l'écran (déplacement de l'herbe)

---

## 🛠️ Technologies

- **Unity** (2022+ recommandé)
- **AR Foundation**
- **ARCore** (Android) / **ARKit** (iOS)
- **C#**
- **Canvas UI (Unity UI)**

---

## Installation & Build

### Prérequis :
- Unity installé avec le module **Android Build Support**
- Téléphone compatible **ARCore**
- [ARCore Services](https://play.google.com/store/apps/details?id=com.google.ar.core) installé sur le téléphone

### Installation de l'application

Pour lancer l'application sur un appareil Android :

1. Transférez le fichier **`build.apk`** sur votre téléphone.
2. Depuis votre téléphone, ouvrez ce fichier :
   - Si c’est la première fois que vous installez une application manuellement, Android vous demandera **d’autoriser l’installation d’applications provenant de sources inconnues** (par exemple : depuis Google Drive, depuis votre navigateur, etc.).
   - Suivez les instructions à l’écran pour accorder cette autorisation (Android vous redirigera automatiquement dans les paramètres).
3. Une fois autorisé, appuyez sur **"Installer"**.
4. Une fois l’installation terminée, cliquez sur **"Ouvrir"** ou retrouvez l’application sur votre écran d’accueil.

---

## 📁 Structure des Scripts

- `DogEat2.cs` – Gère l'interaction avec la nourriture (manger)
- `DogRun.cs` – Gère l'interaction avec la balle (courir)
- `GrassSpawner.cs` – Génère de l'herbe sur les plans détectés

## Installation de l'application

Pour lancer l'application sur un appareil Android :

1. Transférez le fichier **`build.apk`** sur votre téléphone.
2. Depuis votre téléphone, ouvrez ce fichier :
   - Si c’est la première fois que vous installez une application manuellement, Android vous demandera **d’autoriser l’installation d’applications provenant de sources inconnues** (par exemple : depuis Google Drive, depuis votre navigateur, etc.).
   - Suivez les instructions à l’écran pour accorder cette autorisation (Android vous redirigera automatiquement dans les paramètres).
3. Une fois autorisé, appuyez sur **"Installer"**.
4. Une fois l’installation terminée, cliquez sur **"Ouvrir"** ou retrouvez l’application sur votre écran d’accueil.

