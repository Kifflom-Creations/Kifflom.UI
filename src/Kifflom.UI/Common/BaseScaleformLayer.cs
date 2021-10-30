using System;
using CitizenFX.Core.Native;

namespace Kifflom.UI.Common
{
    /// <summary>
    /// Scaleform that is a layer that can be used in group with others
    /// </summary>
    public abstract class BaseScaleformLayer : BaseScaleform, IScaleformLayer
    {
        /// <inheritdoc />
        protected BaseScaleformLayer(string scaleform) : base(scaleform)
        {
            base.Visible = false;
        }

        /// <inheritdoc />
        public bool IsLoading { get; set; }

        /// <inheritdoc />
        public bool IsReady => IsLoading && IsLoaded;

        /// <inheritdoc />
        public bool NeedsLoading => !IsLoading && !IsLoaded;

        /// <summary>
        /// If the Scaleform should be visible or not.
        /// </summary>
        public new bool Visible
        {
            get => base.Visible;
            set
            {
                if (!value)
                {
                    Hide();
                }
                base.Visible = value;
            }
        }

        /// <inheritdoc />
        public void Load()
        {
            IsLoading = true;
            LoadScaleform();
        }

        /// <inheritdoc />
        public void Unload()
        {
            IsLoading = false;
            var handle = Handle;

#if FIVEM
            API.SetScaleformMovieAsNoLongerNeeded(ref handle);
#endif
        }

        /// <inheritdoc />
        public virtual void Show()
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException("The scaleform has not been loaded yet");
            }

            Visible = true;
            IsLoading = false;
        }

        /// <inheritdoc />
        public abstract void Hide();

        /// <inheritdoc />
        protected override void Update()
        {
            if (IsLoading && IsLoaded)
            {
                IsLoading = false;
            }
        }
    }
}
