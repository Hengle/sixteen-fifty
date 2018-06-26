using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexGrid : MonoBehaviour, IPointerClickHandler {
  /**
   * \brief
   * The metrics of the map represented by this hex grid.
   */
  public HexMetrics hexMetrics =>
    map.metrics;
  
  private HexMap map;

  /**
   * \brief
   * The HexMap represented by this grid.
   */
  public HexMap Map {
    get {
      return map;
    }
    set {
      if(null != map)
        map.TileChanged -= OnTileChanged;
      map = value;
      if(null != map) {
        map.TileChanged += OnTileChanged;
      }
    }
  }

  void OnTileChanged(int i, HexTile tile) {
    cells[i].Tile = tile;
  }
  
  /**
   * \brief
   * The prefab for HexCells.
   */
  public GameObject cellPrefab;

  /**
   * \brief
   * Used to instantiate the NPCs listed in map model.
   */
  public GameObject npcPrefab;

  /**
   * \brief
   * Used to handle clicks on the grid.
   */
  new public BoxCollider collider;
  
  /**
   * \brief
   * The actual cells inside the map.
   */
  HexCell[] cells;

  /**
   * \brief
   * Raised when a HexCell is tapped by the user.
   */
  public event Action<HexCell> CellDown;

  void Start() {
    Debug.Assert(map == null, "Map has been set up only once.");
    Setup();
  }

  public void Setup() {
    SetupGrid(map);
    SetupNPCs(map.npcs);
  }

  /**
   * \brief
   * Intialize each BasicNPC from map::npcs.
   */
  void SetupNPCs(IEnumerable<NPCSettings> npcs) {
    foreach(var npc in npcs) {
      npc.Construct(npcPrefab, this, transform);
    }
  }

  void SetupGrid(HexMap map) {
    cells =
      map.tiles.Numbering()
      .Select(
        nt => {
          var x = nt.number % map.width;
          var y = nt.number / map.width;
          var coordinates = HexCoordinates.FromOffsetCoordinates(x, y, map.metrics);
          var cell = CreateCell(coordinates, nt.value);
          cell.SortingOrder = (map.height - y - 1) * 4 + (x % 2) * 2;
          // we set the sorting order to 0 for the top row,
          // 2 for the offset row, 4 for the next row, and so on.
          // The idea is to put the player on the odd-numbered orders in
          // between, so that the player can appear *behind* parts of the
          // map.
          // This is easy, when a player enters a cell, we set the
          // player's sorting order to be one greater than the cell
          // they're on.
          return cell;
        })
      .ToArray();

    var bounds = hexMetrics.Bounds(map.width, map.height);

    // compute the bounding box of our hex-map
    collider.size = bounds;
    // and shift it over so it actually contains our hex whose center is at the origin.
    collider.center = bounds * (1/2f) - new Vector2(hexMetrics.INNER_HALF_WIDTH, hexMetrics.INNER_HEIGHT);
  }

  HexCell CreateCell (HexCoordinates coordinates, HexTile tile) {
    var obj = Instantiate(cellPrefab, transform);
    var cell = obj.GetComponent<HexCell>();
    Debug.Assert(null != cell, "HexCell component of newly instantiated cell prefab exists");
    cell.Tile = tile;

    // define the position for our tile
    cell.coordinates = coordinates;
    var position = coordinates.Box.BottomLeft;
    cell.transform.localPosition = position.Upgrade();

    return cell;
  }

  /**
   * \brief
   * Gets the cell in the grid at the given coordinates.
   *
   * Returns null if the coordinates are bogus (do not refer to a real
   * cell / are out of bounds.)
   */
  public HexCell this[HexCoordinates p] {
    get {
      var oc = p.ToOffsetCoordinates();
      var i = oc.Item1 + oc.Item2 * Map.width;
      return i >= 0 && i < cells.Length ? cells[i] : null;
    }
  }

  /**
   * \brief
   * Gets the cell in the grid at the given coordinates.
   *
   * An exception-throwing variant of `this[p]`.
   * If the identified cell does not exist, throws a NullReferenceException.
   */
  public HexCell at(HexCoordinates p) {
    var cell = this[p];
    if(null == cell)
      throw new NullReferenceException("No such cell at " + p.ToString());
    return cell;
  }

  public void OnPointerClick(PointerEventData data) {
    // if we're doing a drag, then click-to-move shouldn't work
    if(data.dragging)
      return;
    
    Debug.Log("Clicked on grid!");
    if(data.button == 0) {
      var cell = GetCellAt(data.pointerPressRaycast.worldPosition);
      if(null == cell)
        Debug.Log("Touched bogus position.");
      else
        TouchCell(cell);
    }
  }

  /**
   * \brief
   * Gets the cell in the map at the given world position.
   */
  public HexCell GetCellAt(Vector3 worldPosition) {
    // the input position is in world-space, so we inverse transform
    // it to obtain coordinates relative to our hexgrid.
    var position = transform.InverseTransformPoint(worldPosition);
    // then we convert from grid-origin cartesian coordinates to hex
    // coordinates.
    var coordinates = HexCoordinates.FromPosition(position, hexMetrics);
    // Get the cell at those hex coordinates.
    return this[coordinates];
  }

  void TouchCell (HexCell cell) {
    // raise the CellDown event passing in the cell that was clicked.
    if(CellDown != null)
        CellDown(cell);
  }

  /**
   * \brief
   * A pathfinding algorithm from a source HexCoordinates to a
   * destination HexCoordinates within this HexGrid.
   *
   * This is an implementation of Dijkstra's algorithm.
   *
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
        // Debug.Log(cell.ToString() + " -> " + d.ToString());
        cameFrom.Add(d, cell);
        // we add these new cells to our frontier.
        q.Enqueue(d);
      }
    }

    if(reachedDestination) {
      // then cell refers to the destination.
      Debug.Assert(null != cell, "destination cell is not null when moving");
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
}
