using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection.Emit;

public class UIcontroller : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetHP(float hp) {
        healthText.text = hp.ToString();
    }
}

