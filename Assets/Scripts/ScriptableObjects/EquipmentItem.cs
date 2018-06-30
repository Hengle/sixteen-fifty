using System;

using UnityEngine;

namespace SixteenFifty {
  [Serializable]
  [CreateAssetMenu(menuName = "1650/Equipment Item")]
  public class EquipmentItem : Item {
    public EquipmentType type;
    public EquipmentStats stats;
  }
}
