using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

/**
 * \brief
 * Formats stat values falling into fixed ranges.
 */
[CreateAssetMenu(menuName = "1650/Ranged Stat Value Formatter")]
public class RangedStatValueFormatter : StatValueFormatter {
  /**
   * \brief
   * The string to use when all checks fail.
   */
  public string whenBelowAll;

  /**
   * \brief
   * The strings to use paired with the thresholds to use them.
   */
  public StringAbove[] ranges;

  public override string FormatStatValue(float value) {
    Debug.Assert(null != ranges, "ranges to format with is not null");
    Debug.Assert(null != whenBelowAll, "string to use when all tests fail is not null");

    // find the highest range that `value` is above.
    int i = ranges.Length; 
    while(i --> 0) {
      if(value >= ranges[i].threshold)
        return ranges[i].text;
    }
    return whenBelowAll;
  }
}
