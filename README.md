# Projet Info L2 : Generals Clash

- [Télécharger le jeu](https://lucas16lm.itch.io/generals-clash)
- [Vidéo de démonstration](https://www.youtube.com/watch?v=P89Qz_7T3Bw)

## Membres :
- Fargas Raphael
- Corre-Gonzalez Esteban
- Maurin Lucas

## Documentations :
- Unity : https://docs.unity3d.com/ScriptReference/
- C# : https://learn.microsoft.com/fr-fr/dotnet/csharp/tour-of-csharp/overview
- PrimeTween : https://github.com/KyryloKuzyk/PrimeTween
- Linework : https://linework.ameye.dev/
- Cinemachine : https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/index.html

## Systèmes de base à implémenter :
- [x] Carte procédurale
- [x] Structure des factions
- [x] Structure des éléments placeables
- [x] Intégration des unités et des batiments sur la carte (déplacement, attaque, placement, etc)
- [x] Création du système de tours et de gestion des parties
- [x] Création du menu principal

## Liste des factions :
- Royaume de France (cavalerie lourde d'élite)
- Royaume d'Angleterre (archers d'élites)
- Empire Romain Oriental (Troupes d'élites mais chères)
- Ordre du temple (cavalerie et infanterie d'élite mais faiblesse à distance)
- Royaumes Viking (infanterie d'élite mais faibles cavalerie et archers)

## Règles du jeu :
### Général:
La partie prend fin quand il meurt

### Avant poste:
Etend la portée d'où on peut donner des ordres

### Infanterie : 
- 3 points de déplacement
- Reçoit 25% des dommages en moins si 2 infanteries alliées adjacentes
- Les piquiers font 3 fois plus de dégâts à la cavalerie
- Inflige 25% de dommage en moins si attaque une unité sur une colline


### Troupes à distance :
- 4 points de déplacement
- 2 à 4 de portée d'attaque
- Inflige 25% de dégâts en plus si positionné sur une colline
- Inflige 25% de dommage en moins si attaque une unité sur une foret

### Cavalerie :
- 5 points de déplacement
- Inflige 100% d'attaque en plus si au moins 3 cases parcourues pour attaquer
- Inflige 50% d'attaque en moins contre les piquiers
- Inflige 50% de dommage en moins si attaque une unité sur une colline






 
