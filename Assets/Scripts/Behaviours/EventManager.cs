using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Handler for scripted events such as dialogue.
 */
public class EventManager : MonoBehaviour {
  private EventRunner currentEvent;
  private Canvas canvas;
  public Canvas Canvas => canvas;

  private Image textBox;
  public Image TextBox => textBox;

  private Text text;
  public Text Text => text;

  private CanvasGroup canvasGroup;
  public CanvasGroup CanvasGroup => canvasGroup;

  public GameObject speakerPrefab;

  /**
   * Used by the dialogue system.
   * The main blocking panel raises this event when it receives a
   * click event from the Unity eventsystem.
   * Event scripts can listen on this event to receive these click
   * events in turn.
   */
  public event Action<PointerEventData> MainPanelClicked;

  /**
   * Controls whether the invisible dialogue panel will intercept mouse clicks.
   * Dialogue needs to set this to true as pretty much the first thing
   * it does, and set it to false when the dialogue concludes.
   */
  public bool BlocksRaycasts {
    get {
      return canvasGroup.blocksRaycasts;
    }
    set {
      canvasGroup.blocksRaycasts = value;
    }
  }

	// Use this for initialization
	void Awake () {
    // we become the current event manager upon initialization;
    StateManager.Instance.eventManager = this;
    canvas = this.GetComponentNotNull<Canvas>();
    textBox = this.GetComponentInChildrenNotNull<Image>();
    text = this.GetComponentInChildrenNotNull<Text>();
    canvasGroup = this.GetComponentInChildrenNotNull<CanvasGroup>();

    textBox.gameObject.SetActive(false);

    BlocksRaycasts = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

  public void BeginScript(HexGrid map, EventScript e) {
    Debug.Assert(null == currentEvent);
    currentEvent = new EventRunner(this, map, e);
    StartCoroutine(currentEvent.Coroutine);
  }

  public void RaiseMainPanelClicked(PointerEventData data) {
    if(null != MainPanelClicked) {
      MainPanelClicked(data);
    }
  }
}
