using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject proj;
    public GameObject Fireproj;
    public Transform target;
    public Transform SpawnPoint;
    public float gravity;
    public float angletir;
    public float precision;

    public void shoot()
    {
        Projectile prjoMan = proj.GetComponent<Projectile>();
        if (prjoMan != null)
        {
            prjoMan.setProjectile(target, gravity, angletir,precision);
            transform.LookAt(Quaternion.Euler(90, 0, 0) * prjoMan.CalculatePosition(0.1f));
        }
        Instantiate(proj, SpawnPoint.position, Quaternion.Euler(90, 0, 0) );

    }
    public void fireshoot()
    {
        Projectile prjoMan = Fireproj.GetComponent<Projectile>();
        if (prjoMan != null)
        {
            prjoMan.setProjectile(target, gravity, angletir, precision);
            transform.LookAt(Quaternion.Euler(90, 0, 0) * prjoMan.CalculatePosition(0.1f));
        }
        Instantiate(Fireproj, SpawnPoint.position, Quaternion.Euler(90, 0, 0));

    }
    public void setShoot(GameObject proj, Transform target,Transform spawnPoint,float gravity, float angletir,float precision)
    {
        this.proj = proj;
        this.target = target;
        this.SpawnPoint = spawnPoint;
        this.gravity = gravity;
        this.angletir = angletir;
        this.precision = precision;
    } 
    
}
