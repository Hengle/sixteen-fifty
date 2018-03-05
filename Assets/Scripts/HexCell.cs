using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
  public HexCoordinates coordinates;

  /**
   * The grid that contains this cell.
   */
  public HexGrid grid;

  Mesh mesh;

  List<Vector3> vertices;
  List<int> triangles;

  [SerializeField]
  HexCell[] neighbours;

  void Awake () {
    // add our mesh to the meshfilter in this object
    GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    mesh.name = "Hex Mesh";
    vertices = new List<Vector3>();
    triangles = new List<int>();
    Triangulate();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void Triangulate() {
    mesh.Clear();
    vertices.Clear();
    triangles.Clear();
    
    var center = Vector3.zero;
    var l = HexMetrics.corners.Length;
    for(int i = 0; i < l; i++) {
      var c1 = HexMetrics.corners[i];
      var c2 = HexMetrics.corners[(i + 1) % l];
      var t2 = center + c1.Upgrade();
      var t3 = center + c2.Upgrade();
      AddTriangle(center, t2, t3);
    }

    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
  }

  void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
    var vertexIndex = vertices.Count;
    vertices.Add(v1);
    vertices.Add(v2);
    vertices.Add(v3);
    triangles.Add(vertexIndex);
    triangles.Add(vertexIndex + 1);
    triangles.Add(vertexIndex + 2);
  }
}
