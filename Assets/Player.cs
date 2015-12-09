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

    private const string MOVE_VECTOR = "Movement";
    private const string MOVE_X_AXIS = "MoveX";
    private const string MOVE_Y_AXIS = "MoveY";

    private const string LOOK_VECTOR = "Look";
    private const string LOOK_X_AXIS = "LookX";
    private const string LOOK_Y_AXIS = "LookY";

    private static float MOVEMENT_FORCE = 15.0F;
    private static float MAX_ROTATION_VELOCITY = 5F;

    private static float MOVEMENT_DRAG = 0.75F;
    private static float ANGULAR_DRAG = 0.05F;
    private static float PLAYER_MASS = 1.0F;
    private static float GAME_BREAK = 0.9F;
    private static float STOP_DEAD_ZONE = 0.0001F;
    private static float MAXIMUM_MOVEMENT_VELOCITY = 100.0F;

    public Player(string playerID, GameObject context)
    {
        this.playerID = playerID;
        this.SetupController();
        this.context = context;
        GameObject loadedPrefab = (GameObject)Resources.Load("Prefabs/PrefabPlayer");
        this.prefab = MonoBehaviour.Instantiate(loadedPrefab) as GameObject;

        this.SetupPhysics();
    }

    private void SetupPhysics()
    {
        Rigidbody rigidbody = this.GetRigidBody();
        rigidbody.mass = PLAYER_MASS;
        rigidbody.angularDrag = ANGULAR_DRAG;
        rigidbody.drag = MOVEMENT_DRAG;
    }

    /// <summary>
    /// Setups up the controller for the given player
    /// </summary>
    private void SetupController()
    {
        this.controller = new Controller(this.playerID);
        this.SetupControllerVectors();
    }

    /// <summary>
    /// Adds the appropriate vectors to the controller and listens to them
    /// </summary>
    private void SetupControllerVectors()
    {
        // Movement
        this.controller.Add2DVector(MOVE_VECTOR, MOVE_X_AXIS, MOVE_Y_AXIS);
        this.controller.On2DVector(MOVE_VECTOR, this.Move);

        // Aiming
        this.controller.Add2DVector(LOOK_VECTOR, LOOK_X_AXIS, LOOK_Y_AXIS);
        this.controller.On2DVector(LOOK_VECTOR, this.Aim);
    }

    /// <summary>
    /// Point the player in the direction they're aiming
    /// </summary>
    /// <param name="aimVector"></param>
    private void Aim(Vector2 aimVector)
    {

        // Do nothing with a 0 vector
        if (aimVector.magnitude == 0)
        {
            return;
        }

        Transform transform = this.GetTransform();
        Quaternion currentRotation = transform.rotation;
        Vector3 shootDirection = new Vector3(aimVector.x, 0, aimVector.y);
        Quaternion desiredRotation = Quaternion.LookRotation(shootDirection);

        transform.rotation = Quaternion.RotateTowards(currentRotation, desiredRotation, MAX_ROTATION_VELOCITY);
    }

    /// <summary>
    /// Returns the rigid body attached to the player
    /// </summary>
    /// <returns></returns>
    private Rigidbody GetRigidBody()
    {
        return this.prefab.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Returns the transform attached to the player
    /// </summary>
    /// <returns></returns>
    private Transform GetTransform()
    {
        return this.prefab.GetComponent<Transform>();
    }

    /// <summary>
    /// Moves the character using the given input vector
    /// </summary>
    /// <param name="movementVector"></param>
    private void Move(Vector2 movementVector)
    {
        if (movementVector.magnitude != 0)
        {
            this.ApplyMovementVector(movementVector);
        }
        else
        {
            this.ApplyGameBreak();
        }

        // Clamp the maximum speed
        this.GetRigidBody().velocity = Vector3.ClampMagnitude(this.GetRigidBody().velocity, Player.MAXIMUM_MOVEMENT_VELOCITY);

    }

    /// <summary>
    /// Applies a very non-simulation (aka FUN!) break to movement
    /// </summary>
    private void ApplyGameBreak()
    {
        Rigidbody rigidBody = this.GetRigidBody();
        if (rigidBody.velocity.magnitude < Player.STOP_DEAD_ZONE)
        {
            rigidBody.velocity = new Vector3();
        }
        else
        {
            Vector3 velocity = rigidBody.velocity;
            rigidBody.velocity *= Player.GAME_BREAK;
        }
    }
    /// <summary>
    /// Apply the movement vector from the input
    /// </summary>
    /// <param name="movementVector"></param>
    private void ApplyMovementVector(Vector2 movementVector)
    {
        Rigidbody rigidBody = this.GetRigidBody();
        float xForce = movementVector.x * Player.MOVEMENT_FORCE;
        float zForce = movementVector.y * Player.MOVEMENT_FORCE;
        rigidBody.AddForce(new Vector3(xForce, 0, zForce));
    }

    /// <summary>
    /// Keep the update going
    /// </summary>
    public void Update() {
        this.controller.Update();
    }



}
