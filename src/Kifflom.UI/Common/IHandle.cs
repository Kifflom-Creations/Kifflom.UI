namespace Kifflom.UI.Common
{
    /// <summary>
    /// Objects that have a Handle given by the game.
    /// </summary>
    public interface IHandle
    {
        /// <summary>
        /// The unique handle of a object.
        /// </summary>
        int Handle { get; }
    }
}
