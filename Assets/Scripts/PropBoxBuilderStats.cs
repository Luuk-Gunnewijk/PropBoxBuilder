using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PropBoxBuilderStats : MonoBehaviour
{
    
    public List<GameObject> ListOfPrefabs;
    public bool placeBox;
    
    [Range(0, 100)] public float strengthValue;
    [Range(0, 100)] public float SpacingValue;
    public bool toggleSpacing;

    private void Start()
    {
        placeBox = false;
    }

    private void Update()
    {
        CanPlaceBoxToggle();
    }

    public void CheckIfTheToggleIsChecked()
    {
        Debug.Log("Works");
    }

    public void CanPlaceBoxToggle()
    {
        if (placeBox == true)
        {
            Debug.Log("Can Place box");
            placeBox = false; // MOET WEG ANDERS KAN DE BOX NOOIT WORDEN GEPLAATS
        }
    }
}