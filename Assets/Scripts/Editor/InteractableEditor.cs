using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Behaviours;
  
  [CustomEditor(typeof(Interactable))]
  public class InteractableEditor : UnityEditor.Editor {
    public void OnSceneGUI() {
      var obj = Selection.activeTransform.gameObject;
      // if the object is not in the scene, then we don't do anything
      if(obj.scene == null)
        return;

      var interactable = obj.GetComponent<Interactable>();

      interactable.interactionRadius =
        Handles.RadiusHandle(
          Quaternion.identity,
          obj.transform.position,
          interactable.interactionRadius);
    }
  }
}
