using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  [RequireComponent(typeof(MapOrientation), typeof(HexPositioner), typeof(Interactable))]
  public class NPC : MonoBehaviour {
    [SerializeField] [HideInInspector]
    Interactable interactable;

    [SerializeField] [HideInInspector]
    HexPositioner hexPositioner;

    [SerializeField] [HideInInspector]
    MapOrientation mapOrientation;

    public BasicNPC npcData;

    public BasicNPC NPCData {
      get {
        return npcData;
      }
      set {
        npcData = value;
        UpdateData();
      }
    }

    public void UpdateData() {
      Debug.Assert(
        null != hexPositioner,
        "HexPositioner component exists.");
      Debug.Assert(
        null != interactable,
        "Interactable component exists.");
      Debug.Assert(
        null != mapOrientation,
        "MapOrientation component exists.");

      hexPositioner.destination = npcData.destination;
      interactable.interactions = npcData.interactions;
      mapOrientation.HexMapEntity = npcData.mapSprite;
    }

    void Awake() {
      interactable = GetComponent<Interactable>();
      hexPositioner = GetComponent<HexPositioner>();
      mapOrientation = GetComponent<MapOrientation>();

      Debug.Log("NPC awake.");
    }

    void Start() {
    }
  }
}
