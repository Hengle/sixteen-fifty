using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
  public int width = 10;
  public int height = 10;

  public HexCell cellPrefab;
  public GameObject linePrefab;

  private GameObject currentLine;

  new public BoxCollider collider;
  
  public Text cellLabelPrefab;

  Canvas gridCanvas;

  HexCell[] cells;

  void Awake () {
    gridCanvas = GetComponentInChildren<Canvas>();

    cells = new HexCell[height * width];

    for (int y = 0, i = 0; y < height; y++) {
      for(int x = 0; x < width; x++) {
        CreateCell(x, y, i++);
      }
    }

    var bounds = HexMetrics.Bounds(width, height);

    // compute the bounding box of our hex-map
    collider.size = bounds;
    // and shift it over so it actually contains our hex whose center is at the origin.
    collider.center = bounds * (1/2f) - new Vector2(HexMetrics.OUTER_WIDTH, HexMetrics.FULL_HEIGHT);
  }

  void CreateCell (int x , int y, int i) {
    // define the position for our tile
    HexCoordinates coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
    Vector2 position = coordinates.ToPosition();

    var cell = cells[i] = Instantiate<HexCell>(cellPrefab);
    // make the cell belong to the grid, by reparenting its transform
    cell.transform.SetParent(transform, false);
    cell.transform.localPosition = position.Upgrade();
    cell.coordinates = coordinates;
    cell.grid = this;

    Text label = Instantiate<Text>(cellLabelPrefab);
    label.rectTransform.SetParent(gridCanvas.transform, false);
    label.rectTransform.anchoredPosition = position;
    label.text = cell.coordinates.ToStringMultiline();
  }

  public HexCell this[HexCoordinates p] {
    get {
      var oc = p.ToOffsetCoordinates();
      return cells[oc.Item1 + oc.Item2 * width];
    }
  }

  void Update() {
    if(Input.GetMouseButton(0)) {
      HandleInput();
    }
  }

  void HandleInput() {
    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if(Physics.Raycast(inputRay, out hit)) {
      TouchCell(hit.point);
    }
  }

  /**
   * Creates an outline for the hex at the given coordinates, removing
   * the existing outline.
   */
  void MoveOrCreateOutline(HexCoordinates coords) {
    if(currentLine == null) {
      currentLine = Instantiate(linePrefab);
      currentLine.transform.parent = transform;
    }
    currentLine.transform.localPosition = coords.ToPosition();
  }

  void TouchCell (Vector3 position) {
    // the input position is in world-space, so we inverse transform
    // it to obtain coordinates relative to our hexgrid.
    position = transform.InverseTransformPoint(position);
    var coordinates = HexCoordinates.FromPosition(position);
    Debug.Log("touched at " + coordinates.ToString());

    MoveOrCreateOutline(coordinates);
  }
}
