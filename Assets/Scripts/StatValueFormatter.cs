using System;
using System.Collections.Generic;

using UnityEngine;

/**
 * \brief
 * Essentially `Func<float, string>`.
 */
public abstract class StatValueFormatter : ScriptableObject {
  /**
   * \brief Formats the given stat value.
   *
   * Basically, maps intervals of floats to string representations.
   * For example, a warmth stat value in `[0, 1)` might be "neutral".
   */
  abstract public string FormatStatValue(float value);
}

[Serializable]
public class StringAbove {
  public string text;
  public float threshold;
}
