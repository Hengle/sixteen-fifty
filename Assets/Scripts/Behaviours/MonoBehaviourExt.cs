using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExt {
  public static T GetComponentInParentNotNull<T>(this MonoBehaviour self) {
    var o = self.GetComponentInParent<T>();
    Debug.Assert(null != o);
    return o;
  }

  public static T GetComponentInChildrenNotNull<T>(this MonoBehaviour self) {
    var o = self.GetComponentInChildren<T>();
    Debug.Assert(null != o);
    return o;
  }

  public static T GetComponentNotNull<T>(this MonoBehaviour self) {
    var o = self.GetComponent<T>();
    Debug.Assert(null != o);
    return o;
  }
	
}
