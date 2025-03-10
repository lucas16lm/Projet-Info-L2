using System.Collections;
using UnityEngine;

public class Infantry : Unit
{
    public override void Attack(PlaceableObject target)
    {
        throw new System.NotImplementedException();
    }
    private void Start()
    {
        setHealthBar();


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            takeDammage(10);

        }

    }
   
   
}
