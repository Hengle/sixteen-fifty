using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapEntity))]
public class InitialPosition : MonoBehaviour {
  public int x, y;

  void Start() {
    var me = this.GetComponentNotNull<MapEntity>();
    me.Warp(new HexCoordinates(x, y, me.Grid.hexMetrics));
  }
}
