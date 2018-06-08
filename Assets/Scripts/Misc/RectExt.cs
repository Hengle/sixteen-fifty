using System;

using UnityEngine;

public static class RectExt {
  /**
   * \brief
   * Splits the rectangle along the x axis into `count` equal-width
   * subrectangles.
   */
  public static Rect[] SplitX(this Rect r, int count) {
    var res = new Rect[count];
    var w = r.width / count;
    for(var i = 0; i < count; i++) {
      res[i] = r;
      res[i].width = w;
      res[i].x = w * i + r.x;
    }
    return res;
  }

  public static Rect Transforming(
    this Rect self,
    Func<float, float> X,
    Func<float, float> Y,
    Func<float, float> W,
    Func<float, float> H) {
    var s = self;
    s.x = X(s.x);
    s.y = Y(s.y);
    s.width = W(s.width);
    s.height = H(s.height);
    return s;
  }

  public static Rect WithWidth(
    this Rect self,
    float width) {
    return self.WithSize(width, self.height);
  }

  public static Rect WithHeight(
    this Rect self,
    float height) {
    return self.WithSize(self.width, height);
  }

  public static Rect WithSize(
    this Rect self,
    float width,
    float height) {

    var s = self;
    s.width = width;
    s.height = height;
    return s;
  }
}
