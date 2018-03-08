using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour {
  public static Outline Construct(GameObject prefab, HexGrid grid) {
    var self = prefab.GetComponent<Outline>();
    self.grid = grid;
    var instance = Instantiate(prefab).GetComponent<Outline>();
    self.grid = null;
    instance.transform.parent = grid.transform;
    return instance;
  }

  // set in inspector
  public LineRenderer line;

  // set by Construct
  public HexGrid grid;

  private HexCoordinates currentCoords;

	// Use this for initialization
	void Awake () {
    line.positionCount = HexMetrics.corners.Length;
    line.SetPositions(
      Array.ConvertAll<Vector2, Vector3>(
        HexMetrics.corners,
        v => new Vector3(v.x, v.y, 0)));
    line.useWorldSpace = false;
    line.loop = true;

    // disable our renderer to make ourselves invisible initially
    line.enabled = false;
	}

  void OnEnable() {
    grid.CellDown += CellDown;
  }

  void OnDisable() {
    grid.CellDown -= CellDown;
  }

  void CellDown(HexCell cell) {
    var newCoords = cell.coordinates;

    if(line.enabled && !newCoords.Equals(currentCoords)) {
      line.enabled = false;
      return;
    }

    currentCoords = newCoords;
    line.enabled = true;
    transform.localPosition = currentCoords.ToPosition();
  }
	
	// Update is called once per frame
	void Update () {
	}
}
