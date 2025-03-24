using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDesigner : MonoBehaviour
{
    [Header("References")]
    public TMP_Dropdown unitTypeDropDown;
    public TMP_Dropdown materialDropDown;

    public Transform footman;
    public Slider footmanHeadSlider;
    public Slider footmanBodySlider;
    public Toggle footmanQuiverToggle;
    public Slider footmanQuiverSlider;
    public Toggle footmanRightHandToggle;
    public Slider footmanRightHandSlider;
    public Toggle footmanLeftHandToggle;
    public Slider footmanLeftHandSlider;

    public Transform horseman;
    public Slider horsemanHeadSlider;
    public Slider horsemanBodySlider;
    public Slider horsemanHorseSlider;
    public Toggle horsemanQuiverToggle;
    public Slider horsemanQuiverSlider;
    public Toggle horsemanRightHandToggle;
    public Slider horsemanRightHandSlider;
    public Toggle horsemanLeftHandToggle;
    public Slider horsemanLeftHandSlider;

    [Header("units parts")]
    public List<Material> unitMaterials;
    public List<Transform> footmanHeads;
    public List<Transform> footmanBodies;
    public List<Transform> footmanQuivers;
    public List<Transform> footmanRightHands;
    public List<Transform> footmanLeftHands;

    public List<Transform> horsemanHeads;
    public List<Transform> horsemanBodies;
    public List<Transform> horsemanHorses;
    public List<Transform> horsemanQuivers;
    public List<Transform> horsemanRightHands;
    public List<Transform> horsemanLeftHands;

    void Start()
    {
        foreach (Material material in unitMaterials)
        {
            materialDropDown.options.Add(new TMP_Dropdown.OptionData(material.name));
        }
        footmanHeadSlider.maxValue = footmanHeads.Count - 1;
        footmanBodySlider.maxValue = footmanBodies.Count - 1;
        footmanQuiverSlider.maxValue = footmanQuivers.Count - 1;
        footmanRightHandSlider.maxValue = footmanRightHands.Count - 1;
        footmanLeftHandSlider.maxValue = footmanLeftHands.Count - 1;

        horsemanHeadSlider.maxValue = horsemanHeads.Count - 1;
        horsemanBodySlider.maxValue = horsemanBodies.Count - 1;
        horsemanHorseSlider.maxValue = horsemanHorses.Count - 1;
        horsemanQuiverSlider.maxValue = horsemanQuivers.Count - 1;
        horsemanRightHandSlider.maxValue = horsemanRightHands.Count - 1;
        horsemanLeftHandSlider.maxValue = horsemanLeftHands.Count - 1;
        OnMaterialChange();
    }

    public void OnUnitTypeChange()
    {
        if (unitTypeDropDown.value == 0)
        {
            footman.gameObject.SetActive(true);
            horseman.gameObject.SetActive(false);
        }
        else
        {
            footman.gameObject.SetActive(false);
            horseman.gameObject.SetActive(true);
        }

        footmanHeadSlider.maxValue = footmanHeads.Count - 1;
        footmanBodySlider.maxValue = footmanBodies.Count - 1;
        OnMaterialChange();
    }

    public void OnMaterialChange()
    {
        foreach (Renderer renderer in footman.GetComponentsInChildren<Renderer>())
        {
            renderer.material = unitMaterials[materialDropDown.value];
        }

        foreach (Renderer renderer in horseman.GetComponentsInChildren<Renderer>())
        {
            renderer.material = unitMaterials[materialDropDown.value];
        }
    }

    public void RotateLeft(){
        footman.Rotate(new Vector3(0,15,0));
        horseman.Rotate(new Vector3(0,15,0));
    }

    public void RotateRight(){
        footman.Rotate(new Vector3(0,-15,0));
        horseman.Rotate(new Vector3(0,-15,0));
    }

    

    #region footman
    public void OnFootmanHeadChanged()
    {
        foreach (Transform head in footmanHeads)
        {
            head.gameObject.SetActive(false);
        }
        footmanHeads[(int)footmanHeadSlider.value].gameObject.SetActive(true);
        footmanHeadSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Head : " + (int)footmanHeadSlider.value;
        OnMaterialChange();
    }

    public void OnFootmanBodyChanged()
    {
        foreach (Transform body in footmanBodies)
        {
            body.gameObject.SetActive(false);
        }
        footmanBodies[(int)footmanBodySlider.value].gameObject.SetActive(true);
        footmanBodySlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Body : " + (int)footmanBodySlider.value;
        OnMaterialChange();
    }

    public void OnFootmanQuiverToggle()
    {
        foreach (Transform quiver in footmanQuivers)
        {
            quiver.gameObject.SetActive(false);
        }
        if (footmanQuiverToggle.isOn)
        {
            footmanQuivers[(int)footmanQuiverSlider.value].gameObject.SetActive(true);
        }
        OnMaterialChange();
    }

    public void OnFootmanQuiverChanged()
    {
        foreach (Transform quiver in footmanQuivers)
        {
            quiver.gameObject.SetActive(false);
        }
        footmanQuivers[(int)footmanQuiverSlider.value].gameObject.SetActive(true);
        footmanQuiverSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Quiver : " + (int)footmanQuiverSlider.value;
        OnMaterialChange();
    }

    public void OnFootmanRightHandToggle()
    {
        foreach (Transform rightHand in footmanRightHands)
        {
            rightHand.gameObject.SetActive(false);
        }
        if (footmanRightHandToggle.isOn)
        {
            footmanRightHands[(int)footmanRightHandSlider.value].gameObject.SetActive(true);
        }
        OnMaterialChange();
    }

    public void OnFootmanRightHandChanged()
    {
        foreach (Transform rightHand in footmanRightHands)
        {
            rightHand.gameObject.SetActive(false);
        }
        footmanRightHands[(int)footmanRightHandSlider.value].gameObject.SetActive(true);
        footmanRightHandSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Right hand : " + (int)footmanRightHandSlider.value;
        OnMaterialChange();
    }

    public void OnFootmanLeftHandToggle()
    {
        foreach (Transform leftHand in footmanLeftHands)
        {
            leftHand.gameObject.SetActive(false);
        }
        if (footmanLeftHandToggle.isOn)
        {
            footmanLeftHands[(int)footmanLeftHandSlider.value].gameObject.SetActive(true);
        }
        OnMaterialChange();
    }

    public void OnFootmanLeftHandChanged()
    {
        foreach (Transform leftHand in footmanLeftHands)
        {
            leftHand.gameObject.SetActive(false);
        }
        footmanLeftHands[(int)footmanLeftHandSlider.value].gameObject.SetActive(true);
        footmanLeftHandSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Left hand : " + (int)footmanLeftHandSlider.value;
        OnMaterialChange();
    }

    #endregion


    #region  horseman
    public void OnHorsemanHeadChanged()
    {
        foreach (Transform head in horsemanHeads)
        {
            head.gameObject.SetActive(false);
        }
        horsemanHeads[(int)horsemanHeadSlider.value].gameObject.SetActive(true);
        horsemanHeadSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Head : " + (int)horsemanHeadSlider.value;
        OnMaterialChange();
    }

    public void OnHorsemanBodyChanged()
    {
        foreach (Transform body in horsemanBodies)
        {
            body.gameObject.SetActive(false);
        }
        horsemanBodies[(int)horsemanBodySlider.value].gameObject.SetActive(true);
        horsemanBodySlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Body : " + (int)horsemanBodySlider.value;
        OnMaterialChange();
    }

    public void OnHorsemanHorseChanged()
    {
        foreach (Transform horse in horsemanHorses)
        {
            horse.gameObject.SetActive(false);
        }
        horsemanHorses[(int)horsemanHorseSlider.value].gameObject.SetActive(true);
        horsemanHorseSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Horse : " + (int)horsemanHorseSlider.value;
        OnMaterialChange();
    }

    public void OnHorsemanQuiverToggle()
    {
        foreach (Transform quiver in horsemanQuivers)
        {
            quiver.gameObject.SetActive(false);
        }
        if (horsemanQuiverToggle.isOn)
        {
            horsemanQuivers[(int)horsemanQuiverSlider.value].gameObject.SetActive(true);
        }
        OnMaterialChange();
    }

    public void OnHorsemanQuiverChanged()
    {
        foreach (Transform quiver in horsemanQuivers)
        {
            quiver.gameObject.SetActive(false);
        }
        horsemanQuivers[(int)horsemanQuiverSlider.value].gameObject.SetActive(true);
        horsemanQuiverSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Quiver : " + (int)horsemanQuiverSlider.value;
        OnMaterialChange();
    }

    public void OnHorsemanRightHandToggle()
    {
        foreach (Transform rightHand in horsemanRightHands)
        {
            rightHand.gameObject.SetActive(false);
        }
        if (horsemanRightHandToggle.isOn)
        {
            horsemanRightHands[(int)horsemanRightHandSlider.value].gameObject.SetActive(true);
        }
        OnMaterialChange();
    }

    public void OnHorsemanRightHandChanged()
    {
        foreach (Transform rightHand in horsemanRightHands)
        {
            rightHand.gameObject.SetActive(false);
        }
        horsemanRightHands[(int)horsemanRightHandSlider.value].gameObject.SetActive(true);
        horsemanRightHandSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Right hand : " + (int)horsemanRightHandSlider.value;
        OnMaterialChange();
    }

    public void OnHorsemanLeftHandToggle()
    {
        foreach (Transform leftHand in horsemanLeftHands)
        {
            leftHand.gameObject.SetActive(false);
        }
        if (horsemanLeftHandToggle.isOn)
        {
            horsemanLeftHands[(int)horsemanLeftHandSlider.value].gameObject.SetActive(true);
        }
        OnMaterialChange();
    }

    public void OnHorsemanLeftHandChanged()
    {
        foreach (Transform leftHand in horsemanLeftHands)
        {
            leftHand.gameObject.SetActive(false);
        }
        horsemanLeftHands[(int)horsemanLeftHandSlider.value].gameObject.SetActive(true);
        horsemanLeftHandSlider.transform.GetChild(3).GetComponent<TMP_Text>().text = "Left hand : " + (int)horsemanLeftHandSlider.value;
        OnMaterialChange();
    }

    #endregion
}
