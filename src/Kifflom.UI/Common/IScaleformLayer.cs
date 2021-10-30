namespace Kifflom.UI.Common
{
    /// <summary>
    /// Interface for a Layer that contains a Scaleform.
    /// </summary>
    public interface IScaleformLayer
    {
        /// <summary>
        /// Whether the Scaleform is being loaded.
        /// </summary>
        bool IsLoading { get; }
        
        /// <summary>
        /// Whether the Scaleform is Ready to Show.
        /// </summary>
        bool IsReady { get; }
        
        /// <summary>
        /// Whether the Scaleform needs to load before it can be shown.
        /// </summary>
        bool NeedsLoading { get; }

        /// <summary>
        /// Load the Scaleform.
        /// </summary>
        void Load();

        /// <summary>
        /// Unload the Scaleform and mark it as no long needed.
        /// </summary>
        void Unload();

        /// <summary>
        /// Make the Scaleform visible and setup the properties.
        /// </summary>
        void Show();

        /// <summary>
        /// Hide the Scaleform and reset it to a clean state.
        /// </summary>
        void Hide();
    }
}
