using System;

using UnityEngine;

using Commands;

/**
 * A wrapper for an EventScript asset with custom editor support.
 */
[CreateAssetMenu(menuName = "1650/Basic Event")]
public class BasicEvent : ScriptableObject {
  public EventScript script;
}
