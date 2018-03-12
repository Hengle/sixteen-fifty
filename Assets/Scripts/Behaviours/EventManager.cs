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
  public Canvas Canvas {
    get;
    private set;
  }

  public InteractionMenu interactionMenu;

  public Image TextBox {
    get;
    private set;
  }

  public Text Text {
    get;
    private set;
  }

  public Image SecretPanel {
    get;
    private set;
  }

  public Image TitleScreen {
    get;
    private set;
  }

  public CanvasGroup CanvasGroup {
    get;
    private set;
  }

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
      return CanvasGroup.blocksRaycasts;
    }
    set {
      CanvasGroup.blocksRaycasts = value;
    }
  }

	// Use this for initialization
	void Awake () {
    // we become the current event manager upon initialization;
    StateManager.Instance.eventManager = this;
    Canvas = this.GetComponentNotNull<Canvas>();
    TextBox = this.GetComponentInChildrenNotNull<Image>();
    Text = this.GetComponentInChildrenNotNull<Text>();
    CanvasGroup = this.GetComponentInChildrenNotNull<CanvasGroup>();
    SecretPanel = transform.Find("CanvasGroup/Secret Panel").GetComponent<Image>();
    Debug.Assert(null != SecretPanel, "secret panel was found");
    TitleScreen = transform.Find("CanvasGroup/Title Screen").GetComponent<Image>();

    BlocksRaycasts = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

  public void BeginScript(HexGrid map, IScript e) {
    // eventrunners null themselves out of the manager when they
    // finish executing script, so if currentEvent is not null,
    // there's already an event running.
    Debug.Assert(null == currentEvent, "Only one event can be running at once.");
    currentEvent = new EventRunner(this, map, e);
    currentEvent.EventComplete += OnEventComplete;
    StartCoroutine(currentEvent.Coroutine);
  }

  void OnEventComplete(IScript e) {
    currentEvent.EventComplete -= OnEventComplete;
    currentEvent = null;
  }

  public void RaiseMainPanelClicked(PointerEventData data) {
    if(null != MainPanelClicked) {
      MainPanelClicked(data);
    }
  }
}
