using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * \brief
 * A bundle of six sprites, one for each hexagonal direction.
 *
 * \sa
 * MapOrientation
 */
[CreateAssetMenu(menuName = "1650/Hex Map Entity")]
public class HexMapEntity : ScriptableObject {
  public Sprite north;
  public Sprite northEast;
  public Sprite southEast;
  public Sprite south;
  public Sprite southWest;
  public Sprite northWest;

  /**
   * \brief
   * Obtains the directional sprites as an array, starting at north going clockwise.
   */
  public Sprite[] AsArray => new [] { north, northEast, southEast, south, southWest, northWest };

  /**
   * \brief
   * Gets the Sprite associated with the given HexDirection.
   */
  public Sprite this[HexDirection p] => AsArray[(int)p];
}
