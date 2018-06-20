using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexCell : MonoBehaviour {
  /**
   * \brief
   * The hexadecimal coordinates of this cell.
   */
  public HexCoordinates coordinates;

  /**
   * \brief
   * The tile represented by the HexCell.
   *
   * The tile type determines the properties of this cell such as
   * movement costs and the sprite to render.
   */
  public HexTile tile;

  /**
   * \brief
   * The HexGrid object in which the HexCell lives.
   *
   * This field is filled automatically by the cell by looking up in
   * the hierarchy.
   */
  public HexGrid grid;

  /**
   * \brief
   * The renderer to use to draw the tile sprite.
   *
   * This field should be set through the inspector.
   */
  new public SpriteRenderer renderer;

  private ISet<MapEntity> entitiesHere;

  /**
   * \brief
   * The set of MapEntity objects located at this cell.
   */
  public ISet<MapEntity> EntitiesHere => entitiesHere;

  /**
   * \brief
   * Are there any MapEntities here?
   */
  public bool IsNonEmpty => 0 < EntitiesHere.Count;

  /**
   * \brief
   * Is this cell empty of MapEntities?
   */
  public bool IsEmpty => 0 == EntitiesHere.Count;

  /**
   * \brief
   * Fires when a MapEntity enters the cell.
   */
  public event Action<MapEntity> EntityAdded;

  /**
   * \brief
   * Fires when a MapEntity leaves the cell.
   */
  public event Action<MapEntity> EntityRemoved;

  /**
   * \brief
   * Gets all non-null neighbouring HexCell objects.
   */
  public IEnumerable<HexCell> Neighbours =>
    coordinates
    .Neighbours
    .Select(i => grid[i])
    .Where(o => null != o);

  /**
   * \brief
   * Adds an entity to this cell.
   */
  public void AddEntity(MapEntity e) {
    entitiesHere.Add(e);
    if(null != EntityAdded)
      EntityAdded(e);
  }

  /**
   * \brief
   * Removes an entity from this cell.
   */
  public void RemoveEntity(MapEntity e) {
    entitiesHere.Remove(e);
    if(null != EntityRemoved)
      EntityRemoved(e);
  }

  /**
   * \brief
   * Proxies the SpriteRenderer's `sortingOrder` property.
   */
  public int SortingOrder {
    get {
      return renderer.sortingOrder;
    }
    set {
      renderer.sortingOrder = value;
    }
  }

  void Awake () {
    entitiesHere = new HashSet<MapEntity>();
  }

  void Start () {
    Debug.Assert(null != tile, "tile of HexCell is set before Start");
    renderer.sprite = tile.sprite;
    grid = GetComponentInParent<HexGrid>();
    Debug.Assert(null != grid, "owning grid of cell is not null");
  }

  public override string ToString() {
    return "(Cell " + coordinates.ToString() + ")";
  }
}
