using System.Collections.Generic;
using UnityEngine;
using System;

public static class HexMetrics {
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners;

    static HexMetrics() {
        var c = new List<Vector3>();
        for(int i = 0; i < 6; i++) {
            double theta = -i * Math.PI / 3;
            c.Add(new Vector3((float)Math.Cos(theta) * outerRadius, (float)Math.Sin(theta) * outerRadius, 0f));
        }
        corners = c.ToArray();
        Debug.Log("Initialized hex metrics.");
    }
}
