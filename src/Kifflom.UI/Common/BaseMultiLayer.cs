using System.Collections.Generic;
using System.Linq;

namespace Kifflom.UI.Common
{
    /// <summary>
    /// Base class for a processable item that contains multiple layers.
    /// </summary>
    /// <typeparam name="T">The type of layers.</typeparam>
    public abstract class BaseMultiLayer<T> : IProcessable where T : IScaleformLayer
    {
        private bool _visible;

        /// <summary>
        /// All the layers.
        /// </summary>
        protected readonly List<T> Layers;

        /// <summary>
        /// Construct a new class that supports multiple layers.
        /// </summary>
        protected BaseMultiLayer()
        {
            Layers = new List<T>();
        }

        /// <inheritdoc />
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (!value)
                {
                    foreach (var layer in Layers)
                    {
                        layer.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// Add a new layer to the collection of layers.
        /// </summary>
        /// <param name="layer">The layer to add.</param>
        protected void Add(T layer)
        {
            Layers.Add(layer);

            if (Layers.Count == 1)
            {
                Layers[0].LayerHidden += (sender, args) => _visible = false;
            }
        }

        /// <summary>
        /// Whether the first layer is active now.
        /// </summary>
        public bool Active => Layers[0].Visible;

        /// <summary>
        /// Whether any of the layers is active now.
        /// </summary>
        public bool AnyActive => Layers.Any(layer => layer.Visible);

        /// <inheritdoc />
        public void Process()
        {
            if (!Visible) return;

            if (Layers.Any(layer => layer.NeedsLoading))
            {
                foreach (var layer in Layers.Where(layer => layer.NeedsLoading))
                {
                    layer.Load();
                }
            }
            else if (Layers.All(layer => layer.IsReady))
            {
                foreach (var layer in Layers)
                {
                    layer.Show();
                }
            }

            foreach (var layer in Layers)
            {
                layer.Process();
            }
        }
    }
}
