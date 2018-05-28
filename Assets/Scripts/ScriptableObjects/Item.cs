using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1650/Item")]
public class Item : ScriptableObject {
  public Sprite icon;
  new public string name;
  public int stackingSize;
  public EquipmentItem equipment;
}

[Serializable]
public struct EquipmentItem {
  public EquipmentType type;
  public EquipmentStats stats;

  /**
   * \brief The item's warmth, multiplied by its type's effectiveness.
   */
  public float AdjustedWarmth =>
    stats.warmth * type.GetEffectMultiplier();

  /**
   * \brief The item's protection, multiplied by its type's effectiveness.
   */
  public float AdjustedProtection =>
    stats.protection * type.GetEffectMultiplier();

  /**
   * \brief The item's weight, multiplied by its type's effectiveness.
   */
  public float AdjustedWeight =>
    stats.weight * type.GetEffectMultiplier();

  /**
   * \brief The effectiveness of a hat/helmet.
   */
  public const float HEAD_EFFECTIVENESS = 0.6f;

  /**
   * \brief The effectiveness of an outfit.
   */
  public const float BODY_EFFECTIVENESS = 1.0f;

  /**
   * \brief The effectiveness of shoes/boots.
   */
  public const float FEET_EFFECTIVENESS = 0.4f;

  /**
   * \brief The effectiveness of accessories.
   */
  public const float ACCESSORY_EFFECTIVENESS = 0.2f;
}

/**
 * \brief
 * The stats of a piece of equipment.
 */
[Serializable]
public struct EquipmentStats {
  public float warmth;
  public float protection;
  public float weight;

  public EquipmentStats(float warmth, float protection, float weight) {
    this.warmth = warmth;
    this.protection = protection;
    this.weight = weight;
  }

  /**
   * \brief
   * Adds two EquipmentStats objects pointwise.
   */
  public static EquipmentStats operator+(EquipmentStats self, EquipmentStats that) =>
    new EquipmentStats {
      warmth = self.warmth + that.warmth,
      protection = self.protection + that.protection,
      weight = self.weight + that.weight };
}

public static class EquipmentStatsIEnumerableExt {
  /**
   * \brief
   * Sums a sequence of EquipmentStats objects using their `operator+`.
   */
  public static EquipmentStats Sum(this IEnumerable<EquipmentStats> source) {
    var r = new EquipmentStats { warmth = 0, protection = 0, weight = 0 };
    foreach(var s in source) {
      r += s;
    }
    return r;
  }
}

/**
 * \brief
 * The type of a piece of equipment. Determines what slot the equipment can be equipped into.
 */
[Serializable]
public enum EquipmentType {
  NONE,
  HEAD,
  BODY,
  FEET,
  ACCESSORY,
}

public static class EquipmentTypeExt {
  /**
   * \brief Gets the multiplier that moderates the effectiveness of different equipment types.
   *
   * For example, a "very warm" outfit provides more warmth than a
   * "very warm" hat.
   * The multiplier for an outfit is the baseline: `1.0f`. Other
   * equipment types will generally have fractional multipliers.
   */
  public static float GetEffectMultiplier(this EquipmentType self) {
    switch(self) {
    case EquipmentType.HEAD:
      return EquipmentItem.HEAD_EFFECTIVENESS;
    case EquipmentType.BODY:
      return EquipmentItem.BODY_EFFECTIVENESS;
    case EquipmentType.FEET:
      return EquipmentItem.FEET_EFFECTIVENESS;
    case EquipmentType.ACCESSORY:
      return EquipmentItem.ACCESSORY_EFFECTIVENESS;
    case EquipmentType.NONE:
      throw new EquipmentTypeException(
        "Can't get equipment effect multiplier for NONE equipment type.");
    default:
      throw new AnalysisExhaustedException();
    }
  }
}
