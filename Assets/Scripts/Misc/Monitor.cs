using System;

/**
 * \brief
 * A thin messaging layer to notify objects of a change to another.
 */
public class Monitor<T> {
  public T value;
  public event Action<T> Changed;

  public Monitor(T value) {
    this.value = value;
  }

  public void Raise() {
    Changed?.Invoke(value);
  }
}
