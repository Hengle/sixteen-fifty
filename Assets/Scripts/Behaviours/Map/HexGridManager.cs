using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SixteenFifty.Behaviours {
  using TileMap;
  using UI;
  
  /**
  * \brief
  * Loads maps of any kind.
  */
  public class HexGridManager : MonoBehaviour {
    /**
    * \brief
    * The current managed map.
    */
    public IMap CurrentMap {
      get;
      private set;
    }

    public BasicMap initialMap;

    /**
     * \brief
     * Raised when a map finishes loading.
     *
     * This proxies the IMap::Ready event.
     */
    public event Action<IMap> MapReady;

    /**
     * \brief
     * The gameobject that represents the current map.
     */
    private GameObject currentMapObject;

    /**
     * \brief
     * The event manager.
     *
     * Used to play scripted events.
     */
    public EventManager eventManager;

    public InteractionMenu interactionMenu;

    /**
     * \brief
     * Proxies the loaded event from the current grid.
     */
    void OnMapReady(IMap map) => MapReady?.Invoke(map);

    /**
     * \brief
     * Gets the player controller in the current map.
     *
     * \returns
     * Null if there is no map currently loaded.
     */
    public PlayerController Player =>
      CurrentMap?.Player;

    public void PresentInteractionsMenu(Interaction[] interactions) {
      Debug.Log("Presenting: interactions menu!");
      interactionMenu.Show(interactions, OnMenuInteracted);
    }

    void OnMenuInteracted(Interaction interaction) {
      Debug.Log("hi");
      interactionMenu.Interacted -= OnMenuInteracted;
      if(interaction == null)
        return;
      eventManager.BeginScript(this, interaction.script.Compile());
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
      if(CurrentMap == null)
        return;
      
      CurrentMap.Ready -= OnMapReady;
      currentMapObject.SetActive(false); // calls OnDisable
      if(immediate)
        DestroyImmediate(currentMapObject);
      else
        Destroy(currentMapObject);
      CurrentMap = null;
      currentMapObject = null;
    }

    /**
    * \brief
    * Loads another map.
    * First destroys the current map, if any.
    */
    public IMap LoadMap(BasicMap map) {
      // decide what kind of map is next to load
      DestroyMap();

      currentMapObject = Instantiate(map.prefab, transform);

      CurrentMap = currentMapObject.GetComponent(typeof(IMap)) as IMap;
      Debug.Assert(
        CurrentMap != null,
        "instantiated map prefab contains an IMap component.");
      CurrentMap.Ready += OnMapReady;

      Camera.main.backgroundColor = map.backgroundColor;

      // do early map initialization
      CurrentMap.Load(this, map);

      return CurrentMap;
    }

    /**
     * \brief
     * Instantiates the player prefab under the current grid.
     * Sets #Player.
     */
    public PlayerController SpawnPlayer() =>
      CurrentMap.SpawnPlayer();

    void Start() {
      if(null != initialMap)
        LoadMap(initialMap);

      if(null == CurrentMap)
        return;

      SpawnPlayer();
    }
  }
}
