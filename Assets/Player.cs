using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class Player {

    private string playerID;
    private Controller controller;
    private GameObject context;
    private GameObject prefab;

    private const float MOVEMENT_FORCE = 10.0F;
    
    public Player(string playerID, GameObject context)
    {
        this.playerID = playerID;
        this.controller = new Controller(playerID);
        this.controller.Add2DVector("Movement", "MoveX", "MoveY");
        this.controller.On2DVector("Movement", this.Move);
        this.context = context;
        GameObject loadedPrefab = (GameObject)Resources.Load("Prefabs/PrefabPlayer");
        this.prefab = MonoBehaviour.Instantiate(loadedPrefab) as GameObject;
    }

    private void Move(Vector2 movementVector)
    {
        Rigidbody rigidBody = this.prefab.GetComponent<Rigidbody>();
        float xForce = movementVector.x * MOVEMENT_FORCE;
        float zForce = movementVector.y * MOVEMENT_FORCE;
        rigidBody.AddForce(new Vector3(xForce, 0, zForce));
    }

    /// <summary>
    /// Keep the update going
    /// </summary>
    public void Update() {
        this.controller.Update();
    }



}
