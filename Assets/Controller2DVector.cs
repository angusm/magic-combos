using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Controller2DVector
{
    
    private string xAxisName;
    private string yAxisName;
    private Vector2 vector;

    public Controller2DVector(string xAxisName, string yAxisName)
    {

        // Establish the vector names
        this.xAxisName = xAxisName;
        this.yAxisName = yAxisName;

        // Establish the vector
        this.vector = new Vector2();

    }

    /// <summary>
    /// Set the value of the X Axis
    /// </summary>
    /// <param name="value"></param>
    public void SetXAxis(float value)
    {
        this.vector.x = value;
    }

    /// <summary>
    /// Set the value of the Y Axis
    /// </summary>
    /// <param name="value"></param>
    public void SetYAxis(float value)
    {
        this.vector.y = value;
    }

    /// <summary>
    /// Return the vector value
    /// </summary>
    /// <returns></returns>
    public Vector2 GetVector()
    {
        Vector2 rawVector = this.vector;
        float scale = 1;
        if (rawVector.magnitude > 1)
        {
            scale = 1 / rawVector.magnitude;
        }
        return new Vector2(rawVector.x * scale, rawVector.y * scale);
    }

}
