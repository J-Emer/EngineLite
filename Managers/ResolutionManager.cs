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

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        public Resolution ActiveResolution { get; private set; }
        private Resolution _normalResolution;
        private Resolution _fullScreenResolution;
        private Resolution _borderedFullScreenResolution;
        private bool _isFullScreen = false;
        public event Action<Resolution> OnResolutionChanged;

        private readonly List<Resolution> _availableResolutions = new();

        public ResolutionManager(GraphicsDeviceManager graphicsDevice, Resolution initialResolution)
        {
            _graphicsDeviceManager = graphicsDevice;
            _normalResolution = initialResolution;
            ActiveResolution = initialResolution;

            var maxMode = graphicsDevice.GraphicsDevice.Adapter.SupportedDisplayModes
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
            Console.WriteLine($"Applying Res: {res}");
            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = res.Width;
            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = res.Height;
            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.IsFullScreen = res.IsFullScreen;
            _graphicsDeviceManager.ApplyChanges();
            // If using GraphicsDeviceManager in main game class, call ApplyChanges there
            OnResolutionChanged?.Invoke(res);
        }

        public string GetCurrentResolutionString() => ActiveResolution.ToString();

        public List<Resolution> AvailableResolutions()
        {
            if (_availableResolutions.Count == 0)
            {
                foreach (var mode in _graphicsDeviceManager.GraphicsDevice.Adapter.SupportedDisplayModes)
                {
                    _availableResolutions.Add(new Resolution(mode.Width, mode.Height, false));
                }
            }
            return _availableResolutions;
        }
    }

}
