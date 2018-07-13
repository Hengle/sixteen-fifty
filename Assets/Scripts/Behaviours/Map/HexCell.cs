using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  using TileMap;
  
  public class HexCell : MonoBehaviour {
    /**
    * \brief
    * The hexadecimal coordinates of this cell.
    */
    [ReadonlyHexCoordinatesAttribute]
    public HexCoordinates coordinates;

    private HexTile tile;

    /**
    * \brief
    * The tile represented by the HexCell.
    *
    * The tile type determines the properties of this cell such as
    * movement costs and the sprite to render.
    */
    public HexTile Tile {
      get {
        return tile;
      }
      set {
        tile = value;
        UpdateTile();
      }
    }

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
     * Collects all interactions available in this cell.
     */
    public IEnumerable<Interaction> EntityInteractions =>
      EntitiesHere
      .SelectWhere(
        me =>
        me.GetComponent<Interactable>().FromNull<Interactable>())
      .SelectMany(
        interactable => interactable.interactions);

    public IEnumerable<Interaction> TileInteractions =>
      Tile.interactions ?? Enumerable.Empty<Interaction>();

    /**
    * \brief
    * Gets all non-null neighbouring HexCell objects.
    */
    public IEnumerable<HexCell> Neighbours =>
      coordinates
      .Neighbours
      .Select(i => grid[i])
      .NotNull();

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
      UpdateTile();
      grid = GetComponentInParent<HexGrid>();
      Debug.Assert(null != grid, "owning grid of cell is not null");
    }

    void UpdateTile() {
      renderer.sprite = tile?.sprite;
    }

    public override string ToString() {
      return "(Cell " + coordinates.ToString() + ")";
    }
  }
}
