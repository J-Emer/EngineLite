using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineLite.Engine.Managers
{
    public class ResolutionManager
    {
        public static ResolutionManager Instance { get; private set; }

        private readonly GraphicsDevice _graphicsDevice;

        public Resolution ActiveResolution { get; private set; }
        private Resolution _normalResolution;
        private Resolution _fullScreenResolution;
        private Resolution _borderedFullScreenResolution;
        private bool _isFullScreen = false;
        public event Action<Resolution> OnResolutionChanged;

        private readonly List<Resolution> _availableResolutions = new();

        public ResolutionManager(GraphicsDevice graphicsDevice, Resolution initialResolution)
        {
            _graphicsDevice = graphicsDevice;
            _normalResolution = initialResolution;
            ActiveResolution = initialResolution;

            var maxMode = graphicsDevice.Adapter.SupportedDisplayModes
                .OrderByDescending(m => m.Width * m.Height)
                .FirstOrDefault();

            _fullScreenResolution = new Resolution(maxMode.Width, maxMode.Height, true);
            _borderedFullScreenResolution = new Resolution(maxMode.Width, maxMode.Height, false);


            ApplyResolution(ActiveResolution);

            Instance = this;
        }

        public void ToggleFullScreen()
        {
            _isFullScreen = !_isFullScreen;
            if(_isFullScreen)
            {
                ActiveResolution = _fullScreenResolution;
            }
            else
            {
                ActiveResolution = _normalResolution;
            }

            // ActiveResolution = ActiveResolution.IsFullScreen ? _normalResolution : _fullScreenResolution;
            ApplyResolution(ActiveResolution);
        }

        public void BorderFullScreen()
        {
            ActiveResolution = _borderedFullScreenResolution;
            ApplyResolution(ActiveResolution);
        }

        private void ApplyResolution(Resolution res)
        {
            _graphicsDevice.PresentationParameters.BackBufferWidth = res.Width;
            _graphicsDevice.PresentationParameters.BackBufferHeight = res.Height;
            _graphicsDevice.PresentationParameters.IsFullScreen = res.IsFullScreen;
            // If using GraphicsDeviceManager in main game class, call ApplyChanges there
            OnResolutionChanged?.Invoke(res);
        }

        public string GetCurrentResolutionString() => ActiveResolution.ToString();

        public List<Resolution> AvailableResolutions()
        {
            if (_availableResolutions.Count == 0)
            {
                foreach (var mode in _graphicsDevice.Adapter.SupportedDisplayModes)
                {
                    _availableResolutions.Add(new Resolution(mode.Width, mode.Height, false));
                }
            }
            return _availableResolutions;
        }
    }

}
