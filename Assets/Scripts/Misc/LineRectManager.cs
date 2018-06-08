using UnityEngine;

/**
 * \brief
 * Manages a Rect statefull with methods for moing around by lines.
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

  public LineRectManager(Rect initialPosition, float lineHeight) {
    Position = initialPosition;
    LineHeight = lineHeight;
  }

  /**
   * \brief
   * Returns the current position and advances the internal position
   * to the next line;
   * 
   */
  public Rect NextLine {
    get {
      var p = Position;
      Advance();
      return p;
    }
  }

  public Rect NextBy(float amount) {
    var p = Position;
    Advance(amount);
    return p;
  }

  public void Advance() {
    Advance(LineHeight);
  }

  public void Advance(float amount) {
    var p = Position;
    p.y += amount;
    Position = p;
  }
}
