using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class height : MonoBehaviour
{
    public TMP_Text ageText;
    private float age = 0f;    // float instead of int

    public void Increase()
    {
        if (age >= 99f)
            return;

        age += 0.1f;
        ageText.text = age.ToString("0.0"); // format to one decimal
    }

    public void Decrease()
    {
        if (age <= 0f)
            return;

        age -= 0.1f;
        ageText.text = age.ToString("0.0");
    }
}