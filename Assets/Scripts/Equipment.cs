using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using SixteenFifty.Serialization;

public class EquipmentDictionary : SerializableDictionary<EquipmentType, EquipmentItem> {
  public EquipmentDictionary() : base() {
  }
}

/**
 * \brief
 * The items equipped by a Character.
 */
[Serializable]
public class Equipment {
  public EquipmentDictionary equipment;
  
  /**
   * \brief
   * Gets all equipped items.
   *
   * Each `Item` in the collection is guaranteed to be not null.
   */
  public ICollection<EquipmentItem> Items => equipment.Values;

  /**
   * \brief
   * Collects all stat effects of the equipped items.
   */
  public EquipmentStats Stats =>
    Items.Select(equipItem => equipItem.stats).Sum();

  /**
   * \brief Equips the given item in the appropriate slot.
   */
  public void Equip(EquipmentItem item) {
    this[item.type] = item;
  }

  /**
   * \brief Gets the Item in the given equipment slot.
   *
   * If the slot is EquipmentType#NONE, then this throws EquipmentTypeException.
   * If the slot is empty, then this returns `null`.
   */
  public EquipmentItem this[EquipmentType type] {
    get {
      return equipment[type];
    }
    set {
      equipment[type] = value;
    }
  }
}

public class EquipmentTypeException : SixteenFiftyException {
  public EquipmentTypeException() : base() {
  }

  public EquipmentTypeException(string message) : base(message) {
  }
}
