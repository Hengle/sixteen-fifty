using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using TileMap;

  [System.Serializable]
  public struct NPCSettings {
    public string name;
    public BasicNPC npc;
    public int x, y;

    public GameObject Construct(GameObject prefab, HexGrid grid, Transform transform) {
      // var ip = prefab.GetComponent<InitialPosition>();
      // ip.x = x;
      // ip.y = y;

      var mo = prefab.GetComponent<MapOrientation>();
      mo.hexMapEntity = npc.mapSprite;
      mo.Orientation = npc.orientation;

      var interactable = prefab.GetComponent<Interactable>();
      interactable.npcData = npc;

      var instance = GameObject.Instantiate(prefab, transform);

      interactable.npcData = default(BasicNPC);
      mo.Orientation = default(HexDirection);
      mo.hexMapEntity = default(HexMapEntity);

      return instance;
    }
  }
}
