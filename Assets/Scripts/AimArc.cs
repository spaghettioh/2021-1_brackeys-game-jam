using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArc : MonoBehaviour
{
    LineRenderer lr;

    public float velocity = 5;
    public float angle = 45;
    public int resolution = 10;

    float gravity; // force of gravity on the y axis
    float radianAngle;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics.gravity.y);
        
    }
    void Start()
    {
        RenderArc();
    }

    void Update()
    {

    }

    /// <summary>
    /// Populates the line renderer with the appropriate settings
    /// </summary>
    void RenderArc()
    {
        lr.positionCount = resolution;
        lr.SetPositions(CalculateArcArray());
    }

    /// <summary>
    /// Creates an array of vector3 positions for the arc
    /// </summary>
    /// <returns>Vector3</returns>
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    /// <summary>
    /// Calculate height and distance of an arc point
    /// </summary>
    /// <returns>Vector3</returns>
    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(x, y, 0);

    }
}
