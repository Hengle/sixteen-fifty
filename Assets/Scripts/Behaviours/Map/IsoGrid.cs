using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  using TileMap;
  
  public class IsoGrid : MonoBehaviour, IMap {
    /**
     * The prefab to use to spawn the player in this map type.
     */
    public GameObject playerPrefab;

    /**
     * \brief
     * Used to detect clicks on the grid.
     */
    new public BoxCollider collider;

    public event Action<IMap> Ready;

    public event Action<IMap> PlayerSpawned;

    /**
     * \brief
     * All interactable objects in this map.
     *
     * Interactables register themselves in this list when they awake
     * inside the IsoGrid.
     */
    [SerializeField] [HideInInspector]
    private List<Interactable> interactables = new List<Interactable>();

    public List<Interactable> Interactables => interactables;

    [SerializeField] [HideInInspector]
    InteractableFocus interactableFocus;

    public HexGridManager Manager {
      get;
      private set;
    }

    public BasicMap Map => IsoMap;

    public IsoMap IsoMap {
      get;
      private set;
    }

    public PlayerController Player {
      get;
      private set;
    }

    public PlayerController SpawnPlayer() {
      Player =
        Instantiate(playerPrefab, transform)
        .GetComponent<PlayerController>();
      PlayerSpawned?.Invoke(this);

      return Player;
    }

    public void Load(HexGridManager manager, BasicMap _map) {
      var map = _map as IsoMap;
      Debug.Assert(
        null != map,
        "IsoGrid loads an IsoMap.");

      Debug.Assert(
        Manager == manager,
        "Instantiating manager is the same as loading manager.");
    }

    void Awake() {
      interactables = new List<Interactable>();
      Manager = GetComponentInParent<HexGridManager>();
      Debug.Assert(
        null != Manager,
        "IsoGrid is instantiated under a HexGridManager.");

      interactableFocus = GetComponent<InteractableFocus>();
    }
  }
}
