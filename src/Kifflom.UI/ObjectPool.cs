using System;
using System.Collections.Generic;
using Kifflom.UI.Common;

namespace Kifflom.UI
{
    /// <summary>
    /// Manager for Scaleforms.
    /// </summary>
    public class ObjectPool
    {
        private readonly IList<IProcessable> _processables;

        /// <summary>
        /// Create a new ObjectPool which contains multiple items.
        /// </summary>
        public ObjectPool()
        {
            _processables = new List<IProcessable>();
        }

        /// <summary>
        /// Add a new processable to the object pool.
        /// </summary>
        /// <param name="processable">The processable to be added to the pool.</param>
        /// <exception cref="ArgumentNullException">Throws when the processable is null.</exception>
        public void Add(IProcessable processable)
        {
            if (processable == null)
            {
                throw new ArgumentNullException(nameof(processable));
            }

            _processables.Add(processable);
        }

        /// <summary>
        /// Remove a processable from the object pool.
        /// </summary>
        /// <param name="processable">The processable to be removed from the pool.</param>
        public void Remove(IProcessable processable)
        {
            _processables.Remove(processable);
        }

        /// <summary>
        /// Process all the items in the object pool.
        /// </summary>
        public void Process()
        {
            foreach (var processable in _processables)
            {
                processable.Process();
            }
        }
    }
}
