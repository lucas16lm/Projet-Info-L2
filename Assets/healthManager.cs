using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public Gradient sliderGradient;
    [SerializeField]
    private Image fill;

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
}
