using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    public void OnStaminaUpdate(float stamina)
    {
        slider.value = stamina;
    }
}
