/**
 * \brief
 * Defines a #Refresh method for data that needs to be synchronized
 * periodically.
 */
public interface IRefreshable {
  /**
   * \brief
   * Refreshes the data associated with this object.
   */
  void Refresh();
}
