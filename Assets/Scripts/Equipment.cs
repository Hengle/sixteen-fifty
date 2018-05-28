using System;
using System.Linq;

/**
 * \brief
 * The items equipped by a Player.
 */
[Serializable]
public class Equipment {
  public Item head;
  public Item body;
  public Item feet;
  public Item accessory;

  /**
   * \brief
   * Gets all equipment items as an array.
   */
  public Item[] Items =>
    new [] { head, body, feet, accessory };

  public EquipmentStats Stats =>
    Items.Where(i => null != i).Select(item => item.equipment.stats).Sum();

  /**
   * \brief Equips the given item in the appropriate slot.
   */
  public void Equip(Item item) {
    this[item.equipment.type] = item;
  }

  /**
   * \brief Gets the Item in the given equipment slot.
   *
   * If the slot is EquipmentType#NONE, then this throws EquipmentTypeException.
   * If the slot is empty, then this returns `null`.
   */
  public Item this[EquipmentType type] {
    get {
      switch(type) {
      case EquipmentType.HEAD:
        return head;
      case EquipmentType.BODY:
        return body;
      case EquipmentType.FEET:
        return feet;
      case EquipmentType.ACCESSORY:
        return accessory;
      case EquipmentType.NONE:
        throw new EquipmentTypeException("Can't get equipment in slot NONE.");
      default:
        throw new AnalysisExhaustedException();
      }
    }

    set {
      if(value.equipment.type != type)
        throw new EquipmentTypeException("Can't set equipment of wrong type.");

      switch(type) {
      case EquipmentType.HEAD:
        head = value;
        break;
      case EquipmentType.BODY:
        body = value;
        break;
      case EquipmentType.FEET:
        feet = value;
        break;
      case EquipmentType.ACCESSORY:
        accessory = value;
        break;
      case EquipmentType.NONE:
        throw new EquipmentTypeException("Can't get equipment in slot NONE.");
      default:
        throw new AnalysisExhaustedException();
      }
    }
  }
}

public class EquipmentTypeException : SixteenFiftyException {
  public EquipmentTypeException() : base() {
  }

  public EquipmentTypeException(string message) : base(message) {
  }
}
