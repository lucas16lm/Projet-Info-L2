using System;
using TMPro;
using UnityEngine;



public class TroupTest : MonoBehaviour
{
    public Troup_Generator troup_Generator;
    public TMP_InputField xField;
    public TMP_InputField yField;
    public TMP_InputField zField;

    public void addTroup()
    {
        troup_Generator.CreateTroupe(Int32.Parse(xField.text),Int32.Parse( yField.text), 1);
    }
    





}
