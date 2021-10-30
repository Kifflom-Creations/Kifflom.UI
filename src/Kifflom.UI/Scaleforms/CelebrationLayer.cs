using System;
using System.Collections.Generic;
#if FIVEM
using CitizenFX.Core;
#endif
using Kifflom.UI.Common;

namespace Kifflom.UI.Scaleforms
{
    /// <summary>
    /// A layer for the Celebration item
    /// </summary>
    public class CelebrationLayer : BaseScaleformLayer
    {
        private readonly CelebrationLayerEnum _layer;

        private int _durationValue = 0;
        private int _duration = -1;
        private int _start;

        private readonly List<Action<CelebrationLayer>> _items;

        internal CelebrationLayer(CelebrationLayerEnum layer, List<Action<CelebrationLayer>> items) : base(layer.Scaleform())
        {
            _layer = layer;
            _items = items;
        }

        /// <summary>
        /// The duration of each item on the wall.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// The Id of the wall.
        /// </summary>
        public string WallId => "ending";

        /// <inheritdoc />
        protected override void Update()
        {
            base.Update();

            if (_duration == -1)
            {
                if (IsValueReady(_durationValue))
                {
                    GetValue(_durationValue, out _duration);
                    _duration += 700;
                }
            }
            else if (Game.GameTime - _start > _duration)
            {
                Visible = false;
            }
        }

        /// <inheritdoc />
        public override void Show()
        {
            base.Show();

            CallFunction("CREATE_STAT_WALL", WallId, _layer.WallColour(), 3);
            CallFunction("SET_PAUSE_DURATION", Duration);

            foreach (var action in _items)
            {
                action.Invoke(this);
            }

            CallFunction("ADD_BACKGROUND_TO_WALL", WallId, 75, 0);

            _durationValue = CallFunctionReturn("GET_TOTAL_WALL_DURATION");
            CallFunction("SHOW_STAT_WALL", WallId);
            _start = (int)Game.GameTime;
        }

        /// <inheritdoc />
        public override void Hide()
        {
            CallFunction("CLEANUP", WallId);

            _durationValue = 0;
            _duration = -1;

            Unload();
        }
    }
}
