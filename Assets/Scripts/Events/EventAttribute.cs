using System;

/**
 * \brief
 * Used for populating SubtypeSelector popups.
 */
[AttributeUsage(AttributeTargets.Class)]
public class SelectableSubtype : Attribute {
  public string friendlyName;
}
