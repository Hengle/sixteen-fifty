using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1650/Hex Map Entity")]
public class HexMapEntity : ScriptableObject {
  public Sprite north;
  public Sprite northEast;
  public Sprite southEast;
  public Sprite south;
  public Sprite southWest;
  public Sprite northWest;

  public Sprite[] AsArray => new [] { north, northEast, southEast, south, southWest, northWest };
  public Sprite this[HexDirection p] => AsArray[(int)p];
}
