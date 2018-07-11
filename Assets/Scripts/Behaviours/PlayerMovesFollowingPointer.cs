using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  [RequireComponent(typeof(IsoGrid))]
  public class PlayerMovesFollowingPointer :
    MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler {
    // in units per second
    public float moveSpeed;

    [SerializeField] [HideInInspector]
    PlayerController player;

    [SerializeField] [HideInInspector]
    IsoGrid map;

    bool down;

    void Awake() {
      down = false;
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
      down = true;
    }

    public void OnPointerUp(PointerEventData data) {
      down = false;
    }

    void Update() {
      if(player == null || !down)
        return;

      player.transform.position =
        Vector3.MoveTowards(
          player.transform.position,
          InputUtility.PointerPosition.Upgrade(),
          moveSpeed * Time.deltaTime);
    }
  }
}
