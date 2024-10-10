using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Codice.Client.Commands.WkTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Label = System.Reflection.Emit.Label;
using Toggle = UnityEngine.UI.Toggle;

[CustomEditor(typeof(PropBoxBuilderStats))]
public class PropBoxBuilderMain : Editor
{
    private PropBoxBuilderBox _propBoxBuilderBox;
    private PropBoxBuilderStats _propBoxBuilderStats;
    private SerializedProperty _prefabListProperty;
    private SerializedProperty _strengthSliderProperty;
    private SerializedProperty _spacingSliderProperty;
    [SerializeField] private VisualTreeAsset _visualTreeAsset;
    [SerializeField] private GameObject targetBox;
    
    private Button _togglePlaceBox;
    private Button _togglePlaceProps;

    private static GameObject _newBox;
    //private PropertyField _PrefabList;
    //private Slider _StrengthSlider;

    private void OnEnable()
    {
        _propBoxBuilderStats = (PropBoxBuilderStats)target;
        _prefabListProperty = serializedObject.FindProperty("ListOfPrefabs");
        _strengthSliderProperty = serializedObject.FindProperty("strengthValue");
        _spacingSliderProperty = serializedObject.FindProperty("SpacingValue");
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        _visualTreeAsset.CloneTree(root);

        _togglePlaceBox = root.Q<Button>("TogglePlaceBox");
        _togglePlaceBox.RegisterCallback<ClickEvent>(CanPlaceBox);

        _togglePlaceProps = root.Q<Button>("TogglePlaceProps");
        _togglePlaceProps.RegisterCallback<ClickEvent>(CanPlaceProps);

        var strengthSlider = root.Q<Slider>("StrengthSlider");
        if (strengthSlider != null)
        {
            strengthSlider.BindProperty(_strengthSliderProperty);
            
            strengthSlider.RegisterCallback<ChangeEvent<float>>(evt =>
            {
                _strengthSliderProperty.floatValue = evt.newValue;
                serializedObject.ApplyModifiedProperties(); 
            });
        }
        
        var spacingSlider = root.Q<Slider>("SpacingSlider");
        if (spacingSlider != null)
        {
            spacingSlider.BindProperty(_spacingSliderProperty);
            
            spacingSlider.RegisterCallback<ChangeEvent<float>>(evt =>
            {
                _spacingSliderProperty.floatValue = evt.newValue;
                serializedObject.ApplyModifiedProperties(); 
            });
        }
        
        var prefabListField = root.Q<PropertyField>("ListOfPrefabs");
        if (prefabListField != null)
        {
            prefabListField.BindProperty(_prefabListProperty);
        }
        
        LogPrefabsInList();
        LogStrengthSlider();
        LogSpacingSlider();
        
        return root;
    }

    private void CanPlaceBox(ClickEvent evt)
    {
        _newBox = Instantiate(targetBox, Vector3.zero, Quaternion.identity);
        _propBoxBuilderStats.placeBox = true;
    }

    private void CanPlaceProps(ClickEvent evt)
    {
        _newBox.GetComponent<PropBoxBuilderBox>().MyProps(); 
    }
    
    private void LogPrefabsInList()
    {
        for (int i = 0; i < _prefabListProperty.arraySize; i++)
        {
            SerializedProperty element = _prefabListProperty.GetArrayElementAtIndex(i);
            var prefab = element.objectReferenceValue as GameObject;

            if (prefab != null)
            {
                Debug.Log($"Prefab {i}: {prefab.name}");
            }
        }
    }

    private void LogStrengthSlider()
    {
        Debug.Log(_strengthSliderProperty.floatValue);
    }
    
    private void LogSpacingSlider()
    {
        Debug.Log(_spacingSliderProperty.floatValue);
    }
}