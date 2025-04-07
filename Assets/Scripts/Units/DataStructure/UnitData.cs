using System.Collections.Generic;
using UnityEngine;

public abstract class UnitData : PlaceableData
{
    public RessourceBalance cost;
    public Sprite image;
    public int baseDamagePoints;
    public int baseMovementPoints;
    public float timeToMove;
    public AudioClip movementSound;
    public AudioClip attackSound;
    public AudioClip damageSound;
}