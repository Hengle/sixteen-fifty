using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

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
   * Returns null if no path could be found.
   */
  public IEnumerable<HexCell> FindPath(HexCoordinates source, HexCoordinates destination) {
    // the queue that stores the frontier to explore
    var q = new Queue<HexCell>();
    // we associate each cell we traverse with the cell we came to it from.
    var cameFrom = new Dictionary<HexCell, HexCell>();

    var start = at(source);

    q.Enqueue(start);
    cameFrom.Add(start, null);

    // the current cell we're working on.
    HexCell cell = null;
    var reachedDestination = false;

    while(q.Count > 0) {
      cell = q.Dequeue();
      // once we hit our destination, we can construct the path back.
      if(cell.coordinates.Equals(destination)) {
        reachedDestination = true;
        break;
      }

      IEnumerable<HexCell> ds =
        // get the coordinate neighbours of this cell
        cell.coordinates.Neighbours
        // convert the coordinates to cells by looking up, getting
        // nulls for the out-of-bounds coordinates
        .Select(c => this[c])
        // remove the ones that are out of bounds
        .Where(t => null != t)
        // remove the ones we've already visited
        .Where(c => !cameFrom.ContainsKey(c));

      // associate with each of these cells the current cell,
      // and enqueue them to be explored later.
      foreach(var d in ds) {
        // we say that we arrived at cell `d` from `cell`.
        Debug.Log(cell.ToString() + " -> " + d.ToString());
        cameFrom.Add(d, cell);
        // we add these new cells to our frontier.
        q.Enqueue(d);
      }
    }

    if(reachedDestination) {
      // then cell refers to the destination.
      Assert.IsNotNull(cell);
      // we construct the path from the destination to the source, but
      // this is in reverse order! So we reverse the list.
      var l = new TrivialEnumerable<HexCell>(Path.Construct<HexCell>(cell, cameFrom)).ToList();
      l.Reverse();
      return l;
    }
    else {
      return null;
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

  /**
   * An exception-throwing variant of this[p].
   * If the identified cell does not exist, throws a NullReferenceException.
   */
  public HexCell at(HexCoordinates p) {
    var cell = this[p];
    if(null == cell)
      throw new NullReferenceException("No such cell at " + p.ToString());
    return cell;
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
