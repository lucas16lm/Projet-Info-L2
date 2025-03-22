using UnityEngine;

public class Projectile : MonoBehaviour
{
   
    public Transform target;  // La cible
    [SerializeField]
    private float speed;
    public float gravity;
    [SerializeField]
    private float angletir;
    private Vector3 targetPos;
    private Vector3 startPosVect;
    private float time;
    public float precision;


    

    public void SetProjectile(Transform target, float gravity,float angletir,float precision) 
    {
        
        this.target = target;
        this.gravity = gravity;
        this.angletir = angletir;
        this.precision= precision;

    }
    void Start()
    {
        if (target != null)
        {
            startPosVect = transform.position;
            targetPos = target.position+Random.Range(-precision,precision)*Vector3.right+ Random.Range(-precision, precision) * Vector3.forward;
            speed = CalculateInitialVelocity();
        }
        
        time = 0;

    }
    private void Update()
    {
        if (target != null)
        {

            time += Time.deltaTime;

            // Mise à jour de la position
            transform.position = CalculatePosition(time);

            // Orienter le projectile dans la direction du mouvement
            transform.LookAt( CalculatePosition(time + 0.1f));
            transform.Rotate(-90,0,0);
            

            if (Vector3.Distance(transform.position, targetPos) < 2f || transform.position.y<-1f)
            {
                Destroy(transform.gameObject);
            }
        }

    }
    private float CalculateInitialVelocity()
    {
        Vector3 pointA_XZ = new Vector3(startPosVect.x, 0, startPosVect.z);
        Vector3 pointB_XZ = new Vector3(target.position.x, 0, target.position.z);
        float d = Vector3.Distance(pointA_XZ, pointB_XZ);
        float h = Mathf.Abs(startPosVect.y - target.position.y);


        return (d / Mathf.Cos(angletir*Mathf.Deg2Rad)) * Mathf.Sqrt(gravity / (2 * (h + d * Mathf.Tan(angletir*Mathf.Deg2Rad))));
    }
    public Vector3 CalculatePosition(float t)
    {
        Vector3 direction = targetPos - startPosVect;
        float yOffset = direction.y+2f;
        direction.y = 0;
        float distance = direction.magnitude;

        float angle = angletir * Mathf.Deg2Rad; // Angle de tir
        float timeMax = distance / (speed * Mathf.Cos(angle));

        float xzPos = t / timeMax;
        float yPos = yOffset + speed * Mathf.Sin(angle) * t - 0.5f * gravity * t * t;
        Vector3 startHorVect = new Vector3(startPosVect.x, 0, startPosVect.z);
        Vector3 targetHorVect = new Vector3(targetPos.x, 0, targetPos.z);
        return Vector3.Lerp(startHorVect, targetHorVect, xzPos) + Vector3.up * yPos;
    }
}
