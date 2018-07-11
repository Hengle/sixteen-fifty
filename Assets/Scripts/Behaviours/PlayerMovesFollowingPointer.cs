using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  [RequireComponent(typeof(IsoGrid))]
  public class PlayerMovesFollowingPointer : MonoBehaviour, IPointerDownHandler {
    // in units per second
    public float moveSpeed;

    [SerializeField] [HideInInspector]
    PlayerController player;

    [SerializeField] [HideInInspector]
    IsoGrid map;

    void Awake() {
      map = GetComponent<IsoGrid>();
      Debug.Assert(
        null != map,
        "PlayerMovesFollowerPointer is attached to an IsoGrid.");
    }

    void OnEnable() {
      map.PlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable() {
      map.PlayerSpawned -= OnPlayerSpawned;
    }

    void OnPlayerSpawned(IMap _map) {
      Debug.Assert(
        _map == map,
        "Triggering map is the same as registered map.");

      player = map.Player;
    }

    public void OnPointerDown(PointerEventData data) {
      if(player == null)
        return;

      player.transform.position =
        Vector3.MoveTowards(
          player.transform.position,
          data.pointerPressRaycast.worldPosition,
          moveSpeed * Time.deltaTime);
    }
  }
}
