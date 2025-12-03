using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    [SerializeField] GameObject buildUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            buildUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            buildUI.SetActive(false);
        }
    }
}
