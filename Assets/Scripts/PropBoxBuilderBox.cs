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

    private GameObject instantiatedObject; // make sure they work in both for loops

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
   
       if (_propBoxBuilderStats.toggleSpacing)
       {
           for (int i = 0; i < Mathf.FloorToInt(amount); i++)
           {
               Vector3 randomPosition = new Vector3(
                   Random.Range(boxCenter.x - boxSize.x / 2, boxCenter.x + boxSize.x / 2),
                   _MyY.y,
                   Random.Range(boxCenter.z - boxSize.z / 2, boxCenter.z + boxSize.z / 2)
               );
               
               int randomPrefabIndex = Random.Range(0, _propBoxBuilderStats.ListOfPrefabs.Count);
               instantiatedObject = Instantiate(_propBoxBuilderStats.ListOfPrefabs[randomPrefabIndex], randomPosition, quaternion.identity);
               SetObjectsToGround();
           }
       }
       else
       {
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
                   instantiatedObject = Instantiate(_propBoxBuilderStats.ListOfPrefabs[randomPrefabIndex], position, quaternion.identity);
                   SetObjectsToGround();
               }
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

    private void SetObjectsToGround()
    {
        Collider objectCollider = instantiatedObject.GetComponent<Collider>();
        
        Vector3 rayOrigin = instantiatedObject.transform.position + Vector3.up * 0.1f;
        
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (objectCollider == null || hitInfo.collider != objectCollider)
            {
                Vector3 newPosition = instantiatedObject.transform.position;
                newPosition.y = hitInfo.point.y;
                instantiatedObject.transform.position = newPosition;
            }
        }
    }
}
