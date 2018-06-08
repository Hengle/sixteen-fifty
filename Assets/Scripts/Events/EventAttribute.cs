using System;

/**
 * \brief
 * Applied to classes that derive from BasicScript and can be used in
 * the event system.
 */
[AttributeUsage(AttributeTargets.Class)]
public class EventAttribute : Attribute {
  public string friendlyName;
}
