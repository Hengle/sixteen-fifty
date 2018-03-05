using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour {
  public LineRenderer line;

	// Use this for initialization
	void Start () {
    line.positionCount = HexMetrics.corners.Length;
    line.SetPositions(
      Array.ConvertAll<Vector2, Vector3>(
        HexMetrics.corners,
        v => new Vector3(v.x, v.y, 0)));
    line.useWorldSpace = false;
    line.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
