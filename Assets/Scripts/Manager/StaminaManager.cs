using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author Louis Sutopo
public class StaminaManager : MonoBehaviour
{

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    // Update the Stamina value
    public void OnStaminaUpdate(float stamina)
    {
        slider.value = stamina;
    }
}
