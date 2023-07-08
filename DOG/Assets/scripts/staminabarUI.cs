using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class staminabarUI : MonoBehaviour
{
    public float Stamina, MaxStamina, Width, Height;

    [SerializeField]
    private RectTransform staminaBar;

    public void SetMaxStamina(float maxStamina) {
        MaxStamina = maxStamina;
    }

    public void SetStamina(float stamina) {
        Stamina = stamina;
        float newHeight = (Stamina/MaxStamina) * Height;

        staminaBar.sizeDelta = new Vector2(Width,newHeight);
    }
}
