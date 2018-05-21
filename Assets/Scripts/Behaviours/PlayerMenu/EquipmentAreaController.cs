using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAreaController : MonoBehaviour {
  public ItemSlot head;
  public ItemSlot body;
  public ItemSlot feet;

  public ItemSlot[] Slots =>
    new [] {head, body, feet};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
