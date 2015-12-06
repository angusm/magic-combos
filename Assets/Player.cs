using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Player {

    private string playerID;
    private Controller controller;
    private GameObject context;
    
    public Player(string playerID, GameObject context)
    {
        this.playerID = playerID;
        this.controller = new Controller(playerID);
        this.controller.Add2DVector("Movement", "MoveX", "MoveY");
        this.controller.On2DVector("Movement", this.Move);
        this.context = context;
        GameObject instance = MonoBehaviour.Instantiate(Resources.Load("GameObjects/Player", typeof(GameObject))) as GameObject;

    }

    private void Move(Vector2 movementVector)
    {
    }

    /// <summary>
    /// Keep the update going
    /// </summary>
    public void Update() {
        this.controller.Update();
    }



}
