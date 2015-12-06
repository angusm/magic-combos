using UnityEngine;
using System.Collections;

public class ControllerHandler {

    private static Hashtable ControllersByID = new Hashtable();

    public ControllerHandler()
    {

    }

    /// <summary>
    /// Handles checks for new controllers
    /// </summary>
    public static void Update()
    {

        // Check each possible key code in the system for a pressed key/button
        foreach (KeyCode PressedKeyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            // We only care about pressed keys
            if (!Input.GetKeyDown(PressedKeyCode))
            {
                continue;
            }

            // Check if the input belongs to a controller
            KeyCodeHandler HandledKeyCode = new KeyCodeHandler(PressedKeyCode.ToString());
            if (!HandledKeyCode.IsJoystickInput())
            {
                continue;
            }

            // Grab the controller number
            string ControllerID = HandledKeyCode.GetJoystickNumber();

            // If the input is good then we need to register a controller if we have
            // not already
            if (!IsControllerRegistered(ControllerID))
            {
                RegisterController(ControllerID);
            }            
            
        }
    }

    /// <summary>
    /// Registers a new Controller with the handler under the given ID
    /// </summary>
    /// <param name="ControllerID"></param>
    private static void RegisterController(string ControllerID)
    {
        Debug.Log(ControllerID);
        ControllersByID.Add(ControllerID, new Controller(ControllerID));
    }

    /// <summary>
    /// Indicates whether or not there is currently a controller registered under the
    /// given ID
    /// </summary>
    /// <param name="ControllerID"></param>
    /// <returns></returns>
    public static bool IsControllerRegistered(string ControllerID)
    {
        return ControllersByID.ContainsKey(ControllerID);
    }

    /// <summary>
    /// Returns an array of the currently registered controllers
    /// </summary>
    /// <returns></returns>
    public static Controller[] GetRegisteredControllers()
    {
        Controller[] RegisteredControllers = new Controller[ControllersByID.Count];
        int CurrentIndex = 0;
        foreach(DictionaryEntry ControllerEntry in ControllersByID)
        {
            RegisteredControllers[CurrentIndex] = (Controller) ControllerEntry.Value;
            CurrentIndex++;
        }

        return RegisteredControllers;
    }
	
}
