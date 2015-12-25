using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class CustomGameObject
{

    private GameObject prefab;
    private GameObject context;
    
    public CustomGameObject(
        GameObject context, 
        string resourcesLocation
        )
    {
        this.context = context;
        GameObject loadedPrefab = (GameObject)Resources.Load(resourcesLocation);
        this.prefab = MonoBehaviour.Instantiate(loadedPrefab) as GameObject;
    }

    protected void SetupPhysics()
    {
    }

    /// <summary>
    /// Returns the rigid body attached to the player
    /// </summary>
    /// <returns></returns>
    protected Rigidbody GetRigidBody()
    {
        return this.prefab.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Returns the transform attached to the player
    /// </summary>
    /// <returns></returns>
    protected Transform GetTransform()
    {
        return this.prefab.GetComponent<Transform>();
    }
    
}
