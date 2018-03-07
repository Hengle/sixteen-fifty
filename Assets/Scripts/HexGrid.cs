using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
  public int width = 10;
  public int height = 10;

  public HexCell cellPrefab;

  new public BoxCollider collider;
  
  public Text cellLabelPrefab;

  Canvas gridCanvas;

  HexCell[] cells;

  public event Action<HexCell> CellDown;

  /**
   * Generates a sequence of cells (excluding the cell at the source
   * position) that goes from the source coordinates to the
   * destination coordinates.
   */
  public IEnumerator<HexCell> FindPath(HexCoordinates source, HexCoordinates destination) {
    // the queue that stores the frontier to explore
    var q = new Queue<HexCell>();
    // we associate each cell we traverse with the cell we came to it from.
    var cameFrom = new Dictionary<HexCell, HexCell>();
    // the coordinates we've visited already
    var visited = new HashSet<HexCoordinates>();

    var self = this;
    // gets a cell and throws if we get nothing.
    Func<HexCoordinates, HexCell> at = coords => {
      var cell = self[coords];
      if(null == cell)
        throw new NullReferenceException();
      return cell;
    };

    var start = at(source);
    q.Enqueue(start);
    visited.Add(source);
    cameFrom.Add(start, null);

    while(q.Count > 0) {
      var cell = q.Dequeue();
      // once we hit our destination, we can construct the path back.
      if(cell.coordinates.Equals(destination))
        return ConstructPath(cell, cameFrom);

      visited.Add(cell.coordinates);

      // loop over all hex directions
      IEnumerable<HexCell> ds = Enum.GetValues(typeof(HexDirection))
        // get the coordinates of those neighbours
        .Select<HexDirection, HexCoordinates>(d => cell.coordinates[d])
        // remove the ones we've already visited
        .Where(c => !visited.Contains(c))
        // convert the coordinates to cells by looking up
        .Select(c => this[c])
        // remove the ones that are out of bounds
        .Where(t => null != t.Item2);
      // associate with each of these cells the current cell,
      // and enqueue them to be explored later.
      foreach(var d in ds) {
        cameFrom.Add(d.Item2, cell);
        q.Enqueue(d.Item2);
      }
    }
  }

  private IEnumerator<HexCell> ConstructPath(HexCell destination, Dictionary<HexCell, HexCell> breadcrumbs) {
    var current = destination;
    while(null != breadcrumbs[current]) {
      yield return breadcrumbs[current];
      current = breadcrumbs[current];
    }
  }

  void Awake () {
    gridCanvas = GetComponentInChildren<Canvas>();
    SetupGrid();

    // now that everything is set up, we can start some coroutines that manage events in the hexgrid
    StartCoroutine("CoroCellSelected");
  }

  void SetupGrid() {
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

  /**
   * Gets the cell in the grid at the given coordinates.
   * Returns null if the coordinates are bogus (do not refer to a real
   * cell / are out of bounds.)
   */
  public HexCell this[HexCoordinates p] {
    get {
      var oc = p.ToOffsetCoordinates();
      var i = oc.Item1 + oc.Item2 * width;
      return i >= 0 && i < cells.Length ? cells[i] : null;
    }
  }

  private IEnumerator CoroCellSelected() {
    bool up = true;
    Func<bool> IsPressed = () => Input.GetMouseButton(0);
    
    while(true) {
      // if the mouse was previously up and now it's pressed, then an
      // event might be happening!
      if(IsPressed() && up) {
        up = false;
        HandleCellClick();
      }
      // if the mouse was previously down and now it's lifted, then we
      // reset our boolean.
      else if(!IsPressed() && !up) {
        up = true;
      }

      yield return null;
    }
  }

  void HandleCellClick() {
    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if(Physics.Raycast(inputRay, out hit)) {
      TouchCell(hit.point);
    }
  }

  void TouchCell (Vector3 worldPosition) {
    // the input position is in world-space, so we inverse transform
    // it to obtain coordinates relative to our hexgrid.
    var position = transform.InverseTransformPoint(worldPosition);
    // then we convert from grid-origin cartesian coordinates to hex
    // coordinates.
    var coordinates = HexCoordinates.FromPosition(position);
    // Get the cell at those hex coordinates.
    var cell = this[coordinates];
    if(cell == null) {
      Debug.LogWarning("touched bogus position");
      return;
    }

    Debug.Log("Touched at " + coordinates.ToString() + "; raising CellDown");
    // raise the CellDown event passing in the cell that was clicked.

    if(CellDown != null)
        CellDown(cell);
  }
}
