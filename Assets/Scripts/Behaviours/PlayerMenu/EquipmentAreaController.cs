using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  public class EquipmentAreaController : MonoBehaviour {
    public ItemSlotController head;
    public ItemSlotController body;
    public ItemSlotController feet;

    public ItemSlotController[] Slots =>
      new [] {head, body, feet};

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
  }
}
