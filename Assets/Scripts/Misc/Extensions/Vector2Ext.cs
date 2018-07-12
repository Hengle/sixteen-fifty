using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Ext {
  public static Vector3 Upgrade(this Vector2 self) => new Vector3(self.x, self.y, 0);
}
