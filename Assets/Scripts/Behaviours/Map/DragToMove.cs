using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * \brief
 * Moves the camera in response to dragging on the object with this
 * behaviour.
 *
 * Apply to the HexGrid.
 *
 * Idea:
 * - When we begin dragging, store the current camera position and
     current cursor position.
 * - When drag events occur, compute the difference between the
     current and old cursor position.
 * - Set the camera position to its old position plus the delta computed from the cursor positions.
 */ 
public class DragToMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
  public Vector2 initialWorldPosition;
  public IntVariable pixelsPerUnit;

  public Camera mainCamera;

  void Awake() {
    var obj = GameObject.FindWithTag("MainCamera");
    Debug.Assert(null != obj, "main camera is not null");
    mainCamera = obj.GetComponent<Camera>();
  }

  public void OnBeginDrag(PointerEventData data) {
    // this is the position that needs to be under the finger at all times.
    initialWorldPosition = data.position;
    Debug.Log("Beginning drag.");
  }

  public void OnEndDrag(PointerEventData data) {
    Debug.Log("Ending drag.");
  }

  public void OnDrag(PointerEventData data) {
    mainCamera.transform.position -= (1f / pixelsPerUnit.value) * data.delta.Upgrade();
  }
}
