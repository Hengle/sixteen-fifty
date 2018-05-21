using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapEntity))]
public class InitialPosition : MonoBehaviour {
  public int x, y;

  void Awake() {
    var me = this.GetComponentNotNull<MapEntity>();
    me.Warp(new HexCoordinates(x, y));
  }
}
