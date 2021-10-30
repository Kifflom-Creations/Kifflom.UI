using System;

namespace Kifflom.UI.Common
{
    /// <summary>
    /// Interface to handle Scaleforms.
    /// </summary>
    public interface IScaleform : IProcessable, IDisposable, IHandle
    {
        /// <summary>
        /// The name of the Scaleform.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether the Scaleform is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Draws the Scaleform in full screen.
        /// </summary>
        void Draw();
    }
}
