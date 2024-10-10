using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class PropBoxBuilderBox : MonoBehaviour
{
    private PropBoxBuilderStats _propBoxBuilderStats;

    private bool isRayHittingCollider;
    private float amount;
    private Vector3 _MyY;
    
    private void Awake()
    {
        _propBoxBuilderStats = FindObjectOfType<PropBoxBuilderStats>();
    }

    private void Update()
    {
        CheckIfThereIsGround();
        TheAmountOfPrefabsThatWillSpawn();
    }

    public void MyProps()
    {
        if (!isRayHittingCollider) { return; }

        Vector3 boxSize = transform.localScale;
        Vector3 boxCenter = transform.position;
        
        float spacing = Mathf.Max(_propBoxBuilderStats.SpacingValue / 100f * Mathf.Min(boxSize.x, boxSize.z), 0.1f);
        
        int propsInX = Mathf.FloorToInt(boxSize.x / spacing);
        int propsInZ = Mathf.FloorToInt(boxSize.z / spacing);
        
        for (int x = 0; x < propsInX; x++)
        {
            for (int z = 0; z < propsInZ; z++)
            {
                Vector3 position = new Vector3(
                    boxCenter.x - boxSize.x / 2 + x * spacing + spacing / 2,
                    _MyY.y,
                    boxCenter.z - boxSize.z / 2 + z * spacing + spacing / 2
                );
                
                if (position.x > boxCenter.x + boxSize.x / 2 || position.z > boxCenter.z + boxSize.z / 2)
                {
                    continue;
                }
                
                int randomPrefabIndex = Random.Range(0, _propBoxBuilderStats.ListOfPrefabs.Count);
                Instantiate(_propBoxBuilderStats.ListOfPrefabs[randomPrefabIndex], position, quaternion.identity);
            }
        }

        DestroyImmediate(gameObject);
    }
    
    private void CheckIfThereIsGround()
    {
        Collider collider = GetComponent<Collider>();
        float height = transform.position.y;
        
        if (collider != null)
        {
            height = collider.bounds.max.y;
        }
        Vector3 myPosition = new Vector3(transform.position.x, height, transform.position.z);
        
        if (Physics.Raycast(myPosition,Vector3.down, out RaycastHit hitInfo, Mathf.Infinity))
        {
            isRayHittingCollider = true;
            Debug.DrawRay(myPosition, Vector3.down * 100, Color.blue, 1);
            _MyY = hitInfo.point;
        }
        else
        {
            Debug.Log("Hit Nothing");
        }
    }

    private void TheAmountOfPrefabsThatWillSpawn()
    {
        foreach (var Prefab in _propBoxBuilderStats.ListOfPrefabs)
        {
            amount = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 100 * _propBoxBuilderStats.strengthValue;
        }
        Debug.Log("My Amount:" + amount);
    }
}
