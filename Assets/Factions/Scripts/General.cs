using UnityEngine;

public class General : MonoBehaviour
{
    public GeneralData data;
    public int healthPoints;

    void Start()
    {
        healthPoints=data.baseHealthPoints;
    }
}
