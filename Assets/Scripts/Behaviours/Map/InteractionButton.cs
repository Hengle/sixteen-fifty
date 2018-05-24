using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractionButton : MonoBehaviour {
  private Interaction interaction;
  public Interaction Interaction {
    get {
      return interaction;
    }
    set {
      interaction = value;
      if(null != InteractionChanged) {
        InteractionChanged();
      }
    }
  }
    
  public event Action<Interaction> Interacted;
  private event Action InteractionChanged;

  private Button button;
  private Text text;

  void Awake() {
    button = GetComponent<Button>();
    Debug.Assert(null != button, "Button component attached to InteractionButton is not null.");
    text = GetComponentInChildren<Text>();
    Debug.Assert(null != text, "The button component has a text component beneath it.");
    InteractionChanged += OnInteractionChanged;
  }

  void OnEnable() {
    button.onClick.AddListener(OnClick);
  }

  void OnDisable() {
    button.onClick.RemoveListener(OnClick);
  }

  void OnClick() {
    Debug.Log("Pointer clicked on button " + interaction.name);
    if(null != Interacted) {
      Debug.Assert(null != interaction.script, "Interaction script associated with button is not null.");
      Interacted(interaction);
    }
  }

  private void OnInteractionChanged() {
    text.text = interaction.name;
  }
}
