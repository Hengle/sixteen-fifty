using System;

/**
 * \brief Base exception type for the game.
 */
public class SixteenFiftyException : Exception {
  public SixteenFiftyException() : base() {
  }

  public SixteenFiftyException(string message) : base(message) {
  }
}

/**
 * \brief Thrown when a case analysis covers all cases.
 */
public class AnalysisExhaustedException : Exception {
  public AnalysisExhaustedException() : base() {
  }
  public AnalysisExhaustedException(string message) : base(message) {
  }
}
