using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class Player : CustomGameObject {

    private string playerID;
    private int comboCode; // Sequence of numbers used to store spell chain combos, whenever a button is 
                            // pressed its value is multiplied by ten, the combo value for the pressed 
                            // button is added and then it is modulo'd by 100 and the remainder is stored
                            // as the final value
         
    private Controller controller;
    private GameObject context;
    private GameObject prefab;
    private float lastShotTime;

    private const int AIR_COMBO_VALUE = 1;
    private const int FIRE_COMBO_VALUE = 2;
    private const int EARTH_COMBO_VALUE = 3;
    private const int WATER_COMBO_VALUE = 4;

    private const string MOVE_VECTOR = "Movement";
    private const string MOVE_X_AXIS = "MoveX";
    private const string MOVE_Y_AXIS = "MoveY";

    private const string LOOK_VECTOR = "Look";
    private const string LOOK_X_AXIS = "LookX";
    private const string LOOK_Y_AXIS = "LookY";

    private const string AIR_BUTTON = "Air";
    private const string FIRE_BUTTON = "Fire";
    private const string EARTH_BUTTON = "Earth";
    private const string WATER_BUTTON = "Water";

    private const string SHOOT_BUTTON = "Shoot";

    private static float MOVEMENT_FORCE = 15.0F;
    private static float MAX_ROTATION_VELOCITY = 5F;

    private static float MOVEMENT_DRAG = 0.75F;
    private static float ANGULAR_DRAG = 0.05F;
    private static float PLAYER_MASS = 1.0F;
    private static float BREAK_FORCE = 0.2F;
    private static float STOP_DEAD_ZONE = 0.0001F;
    private static float MAXIMUM_MOVEMENT_VELOCITY = 100.0F;

    private static float TIME_BETWEEN_SHOTS = 0.5f;

    public Player(string playerID, GameObject context) : base(context, "Prefabs/PrefabPlayer")
    {
        this.playerID = playerID;
        this.SetupController();
        this.comboCode = 0;
        this.lastShotTime = 0;
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

        // Combos
        this.controller.OnButton(AIR_BUTTON, this.addAir);
        this.controller.OnButton(FIRE_BUTTON, this.addFire);
        this.controller.OnButton(EARTH_BUTTON, this.addEarth);
        this.controller.OnButton(WATER_BUTTON, this.addWater);


        // Fire
        this.controller.OnAxis(SHOOT_BUTTON, this.Shoot);
    }

    /// <summary>
    /// Handle the addition of the air to the player's current combo
    /// </summary>
    /// <param name="buttonHoldDuration"></param>
    private void addAir(float buttonHoldDuration)
    {
        this.processElementButton(buttonHoldDuration, AIR_COMBO_VALUE);
    }

    /// <summary>
    /// Handle the addition of fire to the player's current combo
    /// </summary>
    /// <param name="buttonHoldDuration"></param>
    private void addFire(float buttonHoldDuration)
    {
        this.processElementButton(buttonHoldDuration, FIRE_COMBO_VALUE);
    }

    /// <summary>
    /// Handle the addtion of earth to the player's current combo
    /// </summary>
    /// <param name="buttonHoldDuration"></param>
    private void addEarth(float buttonHoldDuration)
    {
        this.processElementButton(buttonHoldDuration, EARTH_COMBO_VALUE);
    }

    /// <summary>
    /// Handle the addtion of water to the player's current combo
    /// </summary>
    /// <param name="buttonHoldDuration"></param>
    private void addWater(float buttonHoldDuration)
    {
        this.processElementButton(buttonHoldDuration, WATER_COMBO_VALUE);
    }

    /// <summary>
    /// Process an element button press, ensuring that the we're not re-processing
    /// the same button twice
    /// </summary>
    /// <param name="buttonHoldDuration"></param>
    /// <param name="elementValue"></param>
    private void processElementButton(float buttonHoldDuration, int elementValue)
    {
        // TODO: Ensure we're properly handling the button hold duration
        this.AddElementValue(elementValue);
    }

    /// <summary>
    /// Add the element value to the current combo value
    /// </summary>
    /// <param name="elementValue"></param>
    private void AddElementValue(int elementValue)
    {
        int newComboValue = ((this.comboCode * 10) % 100) + elementValue;
        this.comboCode = newComboValue;
    }

    /// <summary>
    /// Function to shoot a given projectile out
    /// </summary>
    /// <param name="axisValue"></param>
    private void Shoot(float axisValue)
    {
        // Do nothing if they haven't pulled the trigger
        if (axisValue < 0.5)
        {
            return;
        }

        // Do nothing if they've shot too recently to shoot again
        if (Time.fixedTime - this.lastShotTime < Player.TIME_BETWEEN_SHOTS)
        {
            return;
        }

        // Mark the time this shot was fired
        this.lastShotTime = Time.fixedTime;

        // TODO: Actually do something with the combo code
        GameObject projectile = this.InstantiateProjectile();
        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
        Vector3 shootVector = this.GetTransform().rotation * new Vector3(0, 0, 5000f);
        rigidbody.AddForce(shootVector);
    }

    /// <summary>
    /// Instantiate a projectile based on the given combo code
    /// </summary>
    /// <returns></returns>
    private GameObject InstantiateProjectile()
    {
        // TODO: Make this use combo code
        GameObject projectilePrefab = (GameObject)Resources.Load("Prefabs/PrefabProjectile");
        GameObject projectile = MonoBehaviour.Instantiate(projectilePrefab) as GameObject;

        Transform projectileTransform = projectile.GetComponent<Transform>();
        Transform playerTransform = this.GetTransform();

        projectileTransform.position = playerTransform.position + (playerTransform.rotation * new Vector3(0, 0, 3));
        projectileTransform.rotation = playerTransform.rotation;

        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
        rigidbody.mass = PLAYER_MASS;
        rigidbody.angularDrag = 0;
        rigidbody.drag = 0;

        return projectile;

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
    /// Return a Vector3 that represents the player's current facing
    /// </summary>
    /// <returns></returns>
    private Vector3 GetAimVector()
    {
        return this.GetTransform().rotation.eulerAngles;
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
            this.ApplyGameBreak(movementVector);
        }

        // Clamp the maximum speed
        this.GetRigidBody().velocity = Vector3.ClampMagnitude(this.GetRigidBody().velocity, Player.MAXIMUM_MOVEMENT_VELOCITY);

    }

    /// <summary>
    /// Applies a very non-simulation (aka FUN!) break to movement
    /// </summary>
    private void ApplyGameBreak(Vector2 movementVector)
    {
        Rigidbody rigidBody = this.GetRigidBody();
        Vector3 velocity = rigidBody.velocity;
        Boolean breakX = Player.GetFloatSignValue(velocity.x) != Player.GetFloatSignValue(movementVector.x);
        Boolean breakY = Player.GetFloatSignValue(velocity.z) != Player.GetFloatSignValue(movementVector.y);
        Boolean breakAll = rigidBody.velocity.magnitude < Player.STOP_DEAD_ZONE;

        // Handle breaking along the X axis
        float xBreakPercent = 0.0F;
        if (breakX || breakAll)
        {
            xBreakPercent = velocity.x * -Player.BREAK_FORCE;
        }

        float yBreakPercent = 0.0F;
        if (breakY || breakAll)
        {
            yBreakPercent = velocity.z * -Player.BREAK_FORCE;
        }

        // Setup the break vector
        Vector3 breakVector = new Vector3(xBreakPercent, 0, yBreakPercent);

        // Apply the break force
        rigidBody.velocity += breakVector;

        // Stop in the dead zone
        if (rigidBody.velocity.magnitude < Player.STOP_DEAD_ZONE)
        {
            rigidBody.velocity = new Vector3();
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

    /// <summary>
    /// Returns 1 or -1 depending on the sign of the parameter
    /// </summary>
    /// <param name="value"></param>
    private static float GetFloatSignValue(float value)
    {
        if (value == 0)
        {
            return 0.0F;
        }
        return value / Math.Abs(value);
    }

}
