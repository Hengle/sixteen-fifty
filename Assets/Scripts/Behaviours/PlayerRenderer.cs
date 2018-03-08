using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour {
  public MeshFilter meshFilter;

  private Mesh mesh;

  public const float PLAYER_SIZE = 7f;
  
	void Awake () {
    meshFilter.sharedMesh = mesh = new Mesh();
    mesh.name = "Player Mesh";
    mesh.vertices = Enumerable.Range(0, 3)
      .Select(i => -2 * Mathf.PI / 3f * i + Mathf.PI / 2f)
      .Select(t => PLAYER_SIZE * new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0f))
      .ToArray();
    mesh.triangles = new [] { 0, 1, 2 };
    mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
