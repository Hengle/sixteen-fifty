using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCSettings {
  public string name;
  public BasicNPC npc;
  public int x, y;

  public GameObject Construct(GameObject prefab, HexGrid grid, Transform transform) {
    var ip = prefab.GetComponent<InitialPosition>();
    ip.x = x;
    ip.y = y;

    var me = prefab.GetComponent<MapEntity>();
    me.grid = grid;

    var mo = prefab.GetComponent<MapOrientation>();
    mo.hexMapEntity = npc.mapSprite;
    mo.Orientation = npc.orientation;

    var interactable = prefab.GetComponent<Interactable>();
    interactable.npcData = npc;

    var instance = GameObject.Instantiate(prefab);
    instance.transform.parent = transform;

    ip.x = default(int);
    ip.y = default(int);
    interactable.npcData = default(BasicNPC);
    me.grid = default(HexGrid);
    mo.Orientation = default(HexDirection);
    mo.hexMapEntity = default(HexMapEntity);

    return instance;
  }
}
