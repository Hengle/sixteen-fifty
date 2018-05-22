using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuController : MonoBehaviour {
  public HUDController hudController;
  public Button closeButton;

  void OnEnable () {
    hudController.gameObject.SetActive(false);
    closeButton.onClick.AddListener(OnCloseButtonClick);
  }

  void OnDisable () {
    closeButton.onClick.RemoveListener(OnCloseButtonClick);
    hudController.gameObject.SetActive(true);
  }

  void OnCloseButtonClick() {
    this.gameObject.SetActive(false);
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
