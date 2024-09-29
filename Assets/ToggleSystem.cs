using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSystem : MonoBehaviour
{
    public bool isAM;

    public void AMToggle(Toggle toggle)
    {
        if (toggle.isOn)
            isAM = true;
        else
            isAM = false;
    }
}
