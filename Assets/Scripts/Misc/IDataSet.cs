using System;
using System.Collections;
using System.Collections.Generic;

/**
 * \brief
 * A dataset is a refreshable, finite, enumerable object whose
 * resources must be released when we're done with it.
 */
public interface IDataSet<T> : IRefreshable, IEnumerable<T>, IDisposable, IFinite {
}
