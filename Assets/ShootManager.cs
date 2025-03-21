using UnityEngine;

public class ShootManager : MonoBehaviour
{
    public GameObject proj;
    public Transform target;
    public Transform[] SpawnPoint;
    public float gravity;
    public float angletir;
    public float precision;

    public void Start()
    {
        int i = 0;
        foreach(Shoot shooting in transform.GetComponentsInChildren<Shoot>())
        {
            shooting.setShoot(proj, target, SpawnPoint[i], gravity, angletir,precision);
            i++;
        }
    }
}
