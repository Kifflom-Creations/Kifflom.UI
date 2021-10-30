using System;
#if FIVEM
using CitizenFX.Core.Native;
#endif

namespace Kifflom.UI.Common
{
    /// <summary>
    /// Base class that implements the Scaleform interface.
    /// </summary>
    public abstract class BaseScaleform : IScaleform
    {
        /// <summary>
        /// Create a scaleform with a given name.
        /// </summary>
        /// <param name="scaleform">The name of the scaleform.</param>
        /// <param name="load">Whether the scaleform will be loaded in the constructor.</param>
        protected BaseScaleform(string scaleform, bool load = true)
        {
            Name = scaleform;

            if (load) LoadScaleform();
        }

        /// <inheritdoc />
        public int Handle { get; private set; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public bool Visible { get; set; } = true;

        /// <inheritdoc />
        public bool IsLoaded
        {
            get
            {
#if FIVEM
                return API.HasScaleformMovieLoaded(Handle);
#endif
            }
        }

        /// <summary>
        /// Load the scaleform based on the name.
        /// </summary>
        protected void LoadScaleform()
        {
#if FIVEM
            Handle = API.RequestScaleformMovie(Name);
#endif
        }

        private void CallFunctionBase(string function, params object[] parameters)
        {
#if FIVEM
            API.BeginScaleformMovieMethod(Handle, function);
#endif

            foreach (var param in parameters)
            {
                if (param is int paramInt)
                {
#if FIVEM
                    API.ScaleformMovieMethodAddParamInt(paramInt);
#endif
                }
                else if (param is double paramDouble)
                {
#if FIVEM
                    API.ScaleformMovieMethodAddParamFloat((float)paramDouble);
#endif
                }
                else if (param is float paramFloat)
                {
#if FIVEM
                    API.ScaleformMovieMethodAddParamFloat(paramFloat);
#endif
                }
                else if (param is bool paramBool)
                {
#if FIVEM
                    API.ScaleformMovieMethodAddParamBool(paramBool);
#endif
                }
                else if (param is string paramString)
                {
#if FIVEM
                    API.BeginTextCommandScaleformString("STRING");
                    API.AddTextComponentSubstringPlayerName(paramString);
                    API.EndTextCommandScaleformString();
#endif
                }
                else
                {
                    throw new ArgumentException($"Unexpected argument type {param.GetType().Name}.", nameof(parameters));
                }
            }
        }

        /// <summary>
        /// Checks whether the value of a id is ready.
        /// </summary>
        /// <param name="id">The id of the value.</param>
        /// <returns></returns>
        public bool IsValueReady(int id)
        {
#if FIVEM
            return API.IsScaleformMovieMethodReturnValueReady(id);
#endif
        }

        /// <summary>
        /// Get the value by its id.
        /// </summary>
        /// <param name="id">The id of the value.</param>
        /// <param name="value">The string value.</param>
        public void GetValue(int id, out string value)
        {
#if FIVEM
            value = API.GetScaleformMovieMethodReturnValueString(id);
#endif
        }

        /// <summary>
        /// Get the value by its id.
        /// </summary>
        /// <param name="id">The id of the value.</param>
        /// <param name="value">The int value.</param>
        public void GetValue(int id, out int value)
        {
#if FIVEM
            value = API.GetScaleformMovieMethodReturnValueInt(id);
#endif
        }

        /// <summary>
        /// Get the value by its id.
        /// </summary>
        /// <param name="id">The id of the value.</param>
        /// <param name="value">The boolean value.</param>
        public void GetValue(int id, out bool value)
        {
#if FIVEM
            value = API.GetScaleformMovieMethodReturnValueBool(id);
#endif
        }

        /// <summary>
        /// Call a function on the Scaleform.
        /// </summary>
        /// <param name="function">The name of the function.</param>
        /// <param name="parameters">The parameter to send in the function.</param>
        public void CallFunction(string function, params object[] parameters)
        {
            CallFunctionBase(function, parameters);

#if FIVEM
            API.EndScaleformMovieMethod();
#endif
        }

        /// <summary>
        /// Call a function on the Scaleform and get the id of the return value.
        /// </summary>
        /// <param name="function">The name of the function.</param>
        /// <param name="parameters">The parameter to send in the function.</param>
        /// <returns>The id of the return value.</returns>
        public int CallFunctionReturn(string function, params object[] parameters)
        {
            CallFunctionBase(function, parameters);

#if FIVEM
            return API.EndScaleformMovieMethodReturnValue();
#endif
        }

        /// <summary>
        /// Update the scaleform.
        /// </summary>
        protected abstract void Update();

        /// <inheritdoc />
        public void Process()
        {
            if (!Visible) return;

            Update();

            Draw();
        }

        /// <inheritdoc />
        public virtual void Draw()
        {
#if FIVEM
            API.DrawScaleformMovieFullscreen(Handle, 255, 255, 255, 255, 0);
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            var handle = Handle;

#if FIVEM
            API.SetScaleformMovieAsNoLongerNeeded(ref handle);
#endif
        }
    }
}
