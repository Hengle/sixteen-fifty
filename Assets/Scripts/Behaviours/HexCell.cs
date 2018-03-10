using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexCell : MonoBehaviour {
  public HexCoordinates coordinates;
  public HexTile tile;

  public int SortingOrder {
    get {
      return GetComponent<SpriteRenderer>().sortingOrder;
    }
    set {
      GetComponent<SpriteRenderer>().sortingOrder = value;
    }
  }

  public static HexCell Construct(GameObject prefab, HexTile tile) {
    var self = prefab.GetComponent<HexCell>();
    self.tile = tile;
    var instance = Instantiate(prefab).GetComponent<HexCell>();
    self.tile = null;
    return instance;
  }

  Mesh mesh;

  List<Vector3> vertices;
  List<int> triangles;

  void Awake () {
    // add our mesh to the meshfilter in this object
    vertices = new List<Vector3>();
    triangles = new List<int>();

    Debug.Assert(null != tile);

    SetupRenderer();
  }

  void SetupRenderer() {
    var renderer = GetComponent<SpriteRenderer>();
    renderer.sprite = tile.sprite;
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

  public override string ToString() {
    return "(Cell " + coordinates.ToString() + ")";
  }
}
