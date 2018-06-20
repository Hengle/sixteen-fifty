using UnityEngine;

public class SnapToPixelGrid : MonoBehaviour {
  private const int PPU = HexMetrics.PIXELS_PER_UNIT;
  
  void Awake() {
  }

  void LateUpdate() {
    var position = transform.localPosition;
    var parentPosition = transform.parent.position;

    var x = Mathf.Round(parentPosition.x * PPU) / PPU - parentPosition.x;
    var y = Mathf.Round(parentPosition.y * PPU) / PPU - parentPosition.y;

    if (position.x != x)
      Debug.Log("adjusting X!");
    if (position.y != y)
      Debug.Log("adjusting Y!");

    position.x = x;
    position.y = y;

    transform.localPosition = position;
  }
}
