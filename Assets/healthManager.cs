using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class healthManager : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public Gradient sliderGradient;
    [SerializeField]
    private Image fill;

    [SerializeField]
    private Camera cameras;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value= health;
        fill.color = sliderGradient.Evaluate(1f);
    }

    public void sethealth(int health)
    {
        slider.value = health;
        fill.color=sliderGradient.Evaluate(slider.normalizedValue);
    }
    

    
        public void setCamera(Camera newcamera)
    {
        cameras=newcamera;
    }
    private void Awake()
    {
        setCamera(Camera.main);
    }
    private void Update()
    {
        transform.LookAt(cameras.transform);
        transform.Rotate(0, 180, 0);
        
        
    }
    
   
}
