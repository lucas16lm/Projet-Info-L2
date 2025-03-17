using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Projectile : MonoBehaviour
{
    public Transform startPos;
    public Transform target;  // La cible
    private float speed;
    public float gravity;
    [SerializeField]
    private float angletir;
    private Transform lastPosition;
    private Vector3 targetPos;
    private Vector3 startPosVect;
    private float time;
    private Vector3 direction;


    private Rigidbody rb;

    void Start()
    {
        targetPos = target.position;
        startPosVect = startPos.position;
        

        startPosVect = transform.position;
        targetPos = target.position;
        time = 0;
        speed = calculateInitialVelocity();


        

    }
    private void Update()
    {
       
        Debug.Log(Vector3.Distance(targetPos, transform.position));
       
            time += Time.deltaTime;

            // Mise à jour de la position
            transform.position = CalculatePosition(time);

            // Orienter le projectile dans la direction du mouvement
            transform.LookAt(Quaternion.Euler(90, 0, 0) *  CalculatePosition(time + 0.1f));
           
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            Destroy(transform.gameObject);
        }

    }
    private float calculateInitialVelocity()
    {
        Vector3 pointA_XZ = new Vector3(startPos.position.x, 0, startPos.position.z);
        Vector3 pointB_XZ = new Vector3(target.position.x, 0, target.position.z);
        float d = Vector3.Distance(pointA_XZ, pointB_XZ);
        float h = Mathf.Abs(startPos.position.y - target.position.y);


        return (d / Mathf.Cos(angletir*Mathf.Deg2Rad)) * Mathf.Sqrt(gravity / (2 * (h + d * Mathf.Tan(angletir*Mathf.Deg2Rad))));
    }
    Vector3 CalculatePosition(float t)
    {
        Vector3 direction = targetPos - startPosVect;
        float yOffset = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;

        float angle = angletir * Mathf.Deg2Rad; // Angle de tir
        float timeMax = distance / (speed * Mathf.Cos(angle));

        float xzPos = t / timeMax;
        float yPos = yOffset + speed * Mathf.Sin(angle) * t - 0.5f * gravity * t * t;

        return Vector3.Lerp(startPosVect, targetPos, xzPos) + Vector3.up * yPos;
    }
}
