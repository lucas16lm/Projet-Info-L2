using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundAudioSource;
    public AudioSource globalAudioSource;

    public AudioClip turnEndedSound;
    public AudioClip hoverSond;
    public AudioClip selectUnitSound;
    public AudioClip unitMoveSound;
    public AudioClip unitAttackSound;
    public AudioClip unitPlacementSound;
    public AudioClip GoldSound;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip cancelSound;

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "TurnEnded":
                globalAudioSource.PlayOneShot(turnEndedSound);
                break;
            case "Hover":
                globalAudioSource.PlayOneShot(hoverSond);
                break;
            case "UnitSelection":
                globalAudioSource.PlayOneShot(selectUnitSound);
                break;
            case "UnitMovement":
                globalAudioSource.PlayOneShot(unitMoveSound);
                break;
            case "UnitAttack":
                globalAudioSource.PlayOneShot(unitAttackSound);
                break;
            case "UnitPlacement":
                globalAudioSource.PlayOneShot(unitPlacementSound);
                break;
            case "Gold":
                globalAudioSource.PlayOneShot(GoldSound);
                break;
            case "ButtonHover":
                globalAudioSource.PlayOneShot(buttonHover);
                break;
            case "ButtonClick":
                globalAudioSource.PlayOneShot(buttonClick);
                break;
            case "Cancel":
                globalAudioSource.PlayOneShot(cancelSound);
                break;
        }
    }

}
