using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class LocalCanvas : MonoBehaviour
{
    public GameObject movePointImage;
    public Image filledHealthBar;
    public Transform moveBarTransform;
    public Image canAttack;

    public static void CreateLocalCanvas(GameObject canvaGameObject, Transform transform)
    {
        GameObject canvas = Instantiate(canvaGameObject, transform.position+5*Vector3.up, quaternion.identity, transform);
        canvas.GetComponent<Canvas>().enabled = false;

        if(transform?.GetComponent<General>() != null || transform?.GetComponent<Outpost>() != null){
            canvas.GetComponent<LocalCanvas>().moveBarTransform.gameObject.SetActive(false);
            canvas.GetComponent<LocalCanvas>().canAttack.gameObject.SetActive(false);
        }
        else{
            canvas.GetComponent<LocalCanvas>().moveBarTransform.gameObject.SetActive(true);
            canvas.GetComponent<LocalCanvas>().canAttack.gameObject.SetActive(true);

            for (int i = 0; i < transform.GetComponent<Unit>().UnitData.baseMovementPoints; i++)
            {
                Instantiate(canvas.GetComponent<LocalCanvas>().movePointImage, canvas.GetComponent<LocalCanvas>().moveBarTransform.GetChild(1));
            }
        }
    }

    public void ShowCanvas(){
        Canvas canvas = GetComponentInChildren<Canvas>();
        canvas.enabled=true;
        canvas.transform.LookAt(Camera.main.transform.position);
        filledHealthBar.fillAmount= (float) GetComponentInParent<PlaceableObject>().healthPoints/GetComponentInParent<PlaceableObject>().data.baseHealthPoints;

        if(transform?.GetComponentInParent<General>() == null && transform?.GetComponentInParent<Outpost>() == null){
            for (int i = 0; i < transform.GetComponentInParent<Unit>().UnitData.baseMovementPoints; i++)
            {
                canvas.GetComponent<LocalCanvas>().moveBarTransform.GetChild(1).GetChild(i).GetComponent<Image>().color = transform.GetComponentInParent<Unit>().movementPoints>i ? Color.green : Color.black;
            }

            if(transform.GetComponentInParent<Unit>().canAttack){
                canvas.GetComponent<LocalCanvas>().canAttack.color = Color.white;
            }
            else{
                canvas.GetComponent<LocalCanvas>().canAttack.color = Color.black;
            }
        }
    }

    public void HideCanvas(){
        GetComponentInChildren<Canvas>().enabled=false;
    }
}
