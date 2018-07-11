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

    Rigidbody2D playerBody;

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
      playerBody = player.GetComponent<Rigidbody2D>();
      Debug.Assert(
        null != playerBody,
        "Player has a Rigidbody2D.");
    }

    public void OnPointerDown(PointerEventData data) {
      down = true;
    }

    public void OnPointerUp(PointerEventData data) {
      down = false;
      playerBody.velocity = Vector2.zero;
    }

    void FixedUpdate() {
      if(player == null || !down)
        return;

      var delta =
        InputUtility.PointerPosition.Upgrade()
        - player.transform.position;
      playerBody.velocity = delta.normalized * moveSpeed;
    }
  }
}
