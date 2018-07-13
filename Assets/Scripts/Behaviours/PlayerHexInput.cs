using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Polls for inputs and fires C# events in response to them.
   *
   * The fired events are specific to the hex grid; e.g. this module
   * translates the joystick vector into a HexDirection.
   * Furthermore, this behaviour will stop firing if a modal UI has
   * appeared, thus centralizing the behaviour for stopping
   * interactions when dialogue / menus are open.
   */
  public class PlayerHexInput : MonoBehaviour, IHexInput {
    /**
     * \brief
     * Fires on every frame in which a change in the stick direction
     * is made.
     *
     * The given value is null when the stick is neutral.
     */
    public event Action<Maybe<HexDirection>> DirectionChanged;

    /**
     * \brief
     * Fires on the frame the submit button is released.
     */
    public event Action SubmitPressed;

    [SerializeField] [HideInInspector]
    HexGridManager manager;

    [SerializeField] [HideInInspector]
    bool active = false;

    [SerializeField] [HideInInspector]
    HexDirection? lastDirection = null;

    bool firedOnce = false;

    void Awake() {
      manager = GetComponentInParent<HexGridManager>();
      Debug.Assert(
        null != manager,
        "PlayerHexInput is under a HexGridManager.");
    }

    void StickNeutral() =>
      DirectionChanged?.Invoke(Maybe<HexDirection>.Nothing());

    void Update() {
      if(!firedOnce) {
        StickNeutral();
        firedOnce = true;
      }

      if(!manager.IsMapInput) {
        StickNeutral();
        return;
      }
      
      if(Input.GetButtonUp("Jump"))
        SubmitPressed?.Invoke();

      var v = InputUtility.PrimaryAxis;
      if(v.sqrMagnitude < 0.01) {
        // stopped holding the stick
        DirectionChanged
          ?.Invoke(Maybe<HexDirection>.Nothing());
        return;
      }

      var theta = Mathf.Atan2(v.y, v.x);
      if(theta < 0)
        theta += 2 * Mathf.PI;
      var d = TileMap.HexMetrics.DirectionFromAngle(theta);
      DirectionChanged
        ?.Invoke(Maybe<HexDirection>.Just(d));
    }
  }
}
