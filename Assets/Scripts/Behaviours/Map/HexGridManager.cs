using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SixteenFifty.TileMap {
  /**
  * \brief
  * A controller for the HexMap model.

  * The HexGrid represents a hexagonal grid map.
  */
  public class HexGridManager : MonoBehaviour {
    /**
    * \brief
    * The current managed map.
    */
    public HexGrid CurrentGrid {
      get;
      private set;
    }

    /**
    * \brief
    * The initial map to load.
    * THIS IS FOR TESTING!
    */
    public HexMap initialMap;

    /**
    * \brief
    * The prefab used to create the player.
    */
    public GameObject playerPrefab;

    /**
    * \brief
    * The prefab used to create maps.
    */
    public GameObject gridPrefab;

    /**
    * \brief
    * Gets the metrics of the current map.
    */
    public HexMetrics hexMetrics => CurrentGrid?.Map.metrics;

    /**
     * \brief
     * Raised when a HexGrid finishes loading.
     *
     * Internally, the way this works is that when HexGridManager
     * loads a map, it registers a callback on the ::HexGrid::Loaded
     * event, and then just proxies it through this #GridLoaded event.
     *
     * This way, parties that care about maps loading and unloading
     * can just register a callback on HexGridManager (which doesn't
     * go anywhere) rather than have to worry about the HexGrid
     * objects coming and going.
     */
    public event Action<HexGrid> GridLoaded;

    /**
     * \brief
     * Proxies the loaded event from the current grid.
     */
    void OnGridLoaded(HexGrid grid) => GridLoaded?.Invoke(grid);

    /**
     * \brief
     * Gets the current player.
     */
    public PlayerController Player {
      get;
      private set;
    }

    void Awake () {
      Debug.Assert(null != StateManager.Instance, "state manager exists");
      StateManager.Instance.hexGridManager = this;
    }

    /**
    * \brief
    * Destroys the currently loaded map.
    *
    * This is a no-op if there is no loaded map.
    *
    * \param immediate
    * Determines whether to use `DestroyImmediate`.
    */
    public void DestroyMap(bool immediate = false) {
      if(null == CurrentGrid) 
        return;

      CurrentGrid.Loaded -= OnGridLoaded;
      CurrentGrid.OnDisable();
      if(immediate)
        DestroyImmediate(CurrentGrid.gameObject);
      else
        Destroy(CurrentGrid.gameObject);
      CurrentGrid = null;
    }

    /**
    * \brief
    * Loads a new map.
    * First destroys the current map, if any.
    */
    public HexGrid LoadMap(HexMap map) {
      DestroyMap();

      var obj = GameObject.Instantiate(gridPrefab, transform);
      CurrentGrid = obj.GetComponent<HexGrid>();
      Debug.Assert(CurrentGrid != null, "gridPrefab GameObject contains a HexGrid component.");
      CurrentGrid.Map = map;
      CurrentGrid.Loaded += OnGridLoaded;
      return CurrentGrid;
    }

    /**
     * \brief
     * Instantiates the player prefab under the current grid.
     * Sets #Player.
     */
    public PlayerController SpawnPlayer() {
      var obj = Instantiate(playerPrefab, CurrentGrid.transform);
      return Player = obj.GetComponent<PlayerController>();
    }

    void Start() {
      if(null != initialMap)
        LoadMap(initialMap);

      if(CurrentGrid == null)
        return;

      SpawnPlayer();
    }
  }
}
