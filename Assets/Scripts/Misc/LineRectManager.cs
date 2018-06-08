using System;

using UnityEngine;

/**
 * \brief
 * Manages a Rect statefully with methods for moing around by lines.
 */
public class LineRectManager {
  public Rect Position {
    get;
    private set;
  }

  public float LineHeight {
    get;
    private set;
  }

  /**
   * \brief
   * A margin added below each item.
   *
   * See #Advance.
   */
  public float MarginBottom {
    get;
    set;
  }

  public LineRectManager(Rect initialPosition, float lineHeight, float marginBottom = 2f) {
    Position = initialPosition;
    LineHeight = lineHeight;
    MarginBottom = marginBottom;
  }

  /**
   * \brief
   * Returns the current position and advances the internal position
   * to the next line.
   *
   * \sa #LineHeight.
   */
  public Rect NextLine() {
    var p = Position;
    Advance();
    return p;
  }

  /**
   * \brief
   * Gets the current position, and advances the internal position by
   * the given amount.
   */
  public Rect NextBy(float amount) {
    var p = Position;
    Advance(amount);
    return p;
  }

  /**
   * \brief
   * Advances the internal position by the current #LineHeight.
   */
  public void Advance() {
    Advance(LineHeight);
  }

  /**
   * \brief
   * Advances the internal position by the given amount.
   *
   * This isn't totally true. The internal position is advanced by the
   * given amount **plus #MarginBottom**.
   * If you don't want to add a margin, you need to set #MarginBottom
   * to zero (and ideally reset it after).
   */
  public void Advance(float amount) {
    var p = Position;
    p.y += amount;
    p.y += MarginBottom;
    Position = p;
  }

  /**
   * \brief
   * Runs the given Action with #MarginBottom set to zero.
   */
  public void SuspendingMargin(Action action) {
    var m = MarginBottom;
    MarginBottom = 0;
    action();
    MarginBottom = m;
  }
}
