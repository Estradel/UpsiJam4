using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    private Shepherd selectedShepherd;

    // Not Serializable
    [System.NonSerialized]
    public List<Shepherd> Shepherds;
    [System.NonSerialized]
    public List<Sheep> Sheeps;

    private void Awake() 
    {
        Debug.Log("Awake2");
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
        } 
        else 
        { 
            Instance = this;
        }
        
        
    }

    private void Start()
    {
        Shepherds = new List<Shepherd>();
        GameObject[] shepherdObjects = GameObject.FindGameObjectsWithTag("Shepherd");
        foreach (GameObject shepherdObject in shepherdObjects)
        {
            Shepherds.Add(shepherdObject.GetComponent<Shepherd>());
        }
        
        Sheeps = new List<Sheep>();
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        Debug.Log(sheepObjects.Length);
        foreach (GameObject sheepObject in sheepObjects)
        {
            Sheeps.Add(sheepObject.GetComponent<Sheep>());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    if (raycastHit.transform.gameObject.CompareTag("Shepherd"))
                    {
                        selectedShepherd = raycastHit.transform.gameObject.GetComponent<Shepherd>();
                        selectedShepherd.Select();
                    }
                    if (raycastHit.transform.gameObject.CompareTag("Terrain"))
                    {
                        Debug.Log("Terrain");
                        if (selectedShepherd != null)
                        {
                            selectedShepherd.Move(raycastHit.point);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedShepherd = null;
        }
    }
}