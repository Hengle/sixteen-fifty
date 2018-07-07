using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  using TileMap;
  using Variables;
  
  [RequireComponent(typeof(MapEntity))]
  public class InitialPosition : MonoBehaviour {
    public HexCoordinatesVariable destination;

    [SerializeField] [HideInInspector]
    private HexGridManager hexGridManager;

    [SerializeField] [HideInInspector]
    private MapEntity mapEntity;
    
    public void Awake() {
      hexGridManager = GetComponentInParent<HexGridManager>();
      mapEntity = GetComponent<MapEntity>();

      Debug.Assert(
        null != hexGridManager,
        "InitialPosition exists within a map context.");
      Debug.Assert(
        null != mapEntity,
        "InitialPosition is attached with a MapEntity.");
    }

    public void OnEnable() {
      hexGridManager.GridLoaded += OnGridLoad;
    }

    public void OnDisable() {
      hexGridManager.GridLoaded -= OnGridLoad;
    }

    void OnGridLoad(HexGrid grid) {
      if(null != destination)
        mapEntity.Warp(destination.Value);
    }
  }
}
