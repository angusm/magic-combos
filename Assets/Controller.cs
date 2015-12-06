using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Controller
{

    private string id;
    private Hashtable axisEventHandlers;
    private Hashtable vectorEventHandlers2d;
    private Hashtable controller2DVectors;

    public Controller(string controllerID)
    {
        this.id = controllerID;

        this.controller2DVectors = new Hashtable();

        this.axisEventHandlers = new Hashtable();
        this.vectorEventHandlers2d = new Hashtable();
    }

    /// <summary>
    /// Add a new 2D vector with the given name tied to the given axes
    /// </summary>
    /// <param name="vectorName"></param>
    /// <param name="xAxisName"></param>
    /// <param name="yAxisName"></param>
    public void Add2DVector(string vectorName, string xAxisName, string yAxisName)
    {
        Controller2DVector newVector = new Controller2DVector(xAxisName, yAxisName);

        // Add handlers to the axes that will keep this vector up to date
        this.OnAxis(xAxisName, newVector.SetXAxis);
        this.OnAxis(yAxisName, newVector.SetYAxis);

        // Add this vector
        this.controller2DVectors.Add(vectorName, newVector);
    }

    /// <summary>
    /// Register an event handler to be called with the vector during the update cycle
    /// </summary>
    /// <param name="vectorName"></param>
    /// <param name="eventHandler"></param>
    public void On2DVector(string vectorName, Action<Vector2> eventHandler)
    {
        // Start tracking event handlers on the current event
        if (!this.vectorEventHandlers2d.ContainsKey(vectorName))
        {
            this.vectorEventHandlers2d.Add(vectorName, new List<Action<Vector2>>());
        }

        // Add the new event handler to the list
        List<Action<Vector2>> EventHandlersForVector = (List<Action<Vector2>>)this.vectorEventHandlers2d[vectorName];
        EventHandlersForVector.Add(eventHandler);
    }

    /// <summary>
    /// Register an event handler to be called on the given event
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="eventHandler"></param>
    public void OnAxis(string axisName, Action<float> eventHandler)
    {
        // Start tracking event handlers on the current event
        if (!this.axisEventHandlers.ContainsKey(axisName))
        {
            this.axisEventHandlers.Add(axisName, new List<Action<float>>());            
        }

        // Add the new event handler to the list
        List<Action<float>> EventHandlersForVector = (List<Action<float>>) this.axisEventHandlers[axisName];
        EventHandlersForVector.Add(eventHandler);
    }

    /// <summary>
    /// Return the ID for this controller
    /// </summary>
    /// <returns></returns>
    public string GetControllerID() {
        return this.id;
    }

    /// <summary>
    /// Return the properly processed axis value
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    private float GetProcessedAxisValue(string axisName)
    {
        string fullAxisName = this.GetAxisPrefix() + axisName;
        return Input.GetAxis(fullAxisName);
    }

    /// <summary>
    /// Returns the processed vector value
    /// </summary>
    /// <param name="vectorName"></param>
    /// <returns></returns>
    private Vector2 GetProcessedVector(string vectorName)
    {
        Controller2DVector controllerVector = (Controller2DVector)this.controller2DVectors[vectorName];
        Vector2 processedVector = controllerVector.GetVector();
        return processedVector;
    }
    /// <summary>
    /// Process the given controller
    /// </summary>
    public void Update()
    {
        this.ProcessAxisEvents();
        this.Process2DVectorEvents();
    }

    /// <summary>
    /// Process all of the Vector events
    /// </summary>
    private void Process2DVectorEvents()
    {
        foreach (DictionaryEntry Vector2DEventHandlerEntry in this.vectorEventHandlers2d)
        {
            this.Process2DVectorEventHandlerEntry(Vector2DEventHandlerEntry);
        }
    }

    /// <summary>
    /// Process all of the Axis events
    /// </summary>
    private void ProcessAxisEvents()
    {
        foreach (DictionaryEntry AxisEventHandlerEntry in this.axisEventHandlers)
        {
            this.ProcessAxisEventHandlerEntry(AxisEventHandlerEntry);
        }
    }

    /// <summary>
    /// Return the prefix we will use to identify Axes belonging to this controller
    /// </summary>
    /// <returns></returns>
    private string GetAxisPrefix()
    {
        return this.id + "__";
    }

    /// <summary>
    /// Process the given EventHandlerEntry
    /// </summary>
    /// <param name="EventHandlerEntry"></param>
    private void ProcessAxisEventHandlerEntry(DictionaryEntry EventHandlerEntry)
    {

        float axisValue = this.GetProcessedAxisValue((string)EventHandlerEntry.Key);
        foreach (Action<float> EventHandlerFunction in (List<Action<float>>)EventHandlerEntry.Value)
        {
            EventHandlerFunction(axisValue);
        }
    }

    /// <summary>
    /// Process the given EventHandlerEntry
    /// </summary>
    /// <param name="EventHandlerEntry"></param>
    private void Process2DVectorEventHandlerEntry(DictionaryEntry EventHandlerEntry)
    {

        string vectorName = (string) EventHandlerEntry.Key;
        Vector2 controllerVector = this.GetProcessedVector(vectorName);
        foreach (Action<Vector2> EventHandlerFunction in (List<Action<Vector2>>)EventHandlerEntry.Value)
        {
            EventHandlerFunction(controllerVector);
        }
    }

}
