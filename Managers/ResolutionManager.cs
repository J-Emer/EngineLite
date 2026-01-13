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
        private bool IsFullScreen = false;
        private readonly List<Resolution> _availableResolutions = new();
        public event Action<Resolution> OnResolutionChanged;


        public ResolutionManager(GraphicsDeviceManager graphicsDevice)
        {
            _graphicsDeviceManager = graphicsDevice;

            _normalResolution = SettingsManager.Instance.Settings.DefaultResolution;
            ActiveResolution = _normalResolution;    

            foreach (var item in _graphicsDeviceManager.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                _availableResolutions.Add(new Resolution(item.Width, item.Height, false));
            }

            int _fullWidth = _graphicsDeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            int _fullHeight = _graphicsDeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _fullScreenResolution = new Resolution(_fullWidth, _fullHeight, true);

            Instance = this;

            HandleResolution();

            Console.WriteLine($"Resolution Manager: {ActiveResolution}");
        }

        public void ToggleFullScreen()
        {
            IsFullScreen = !IsFullScreen;

            if(IsFullScreen)
            {
                ActiveResolution = _fullScreenResolution;
            }
            else
            {
                ActiveResolution = _normalResolution;
            }

            HandleResolution();
        }

        public void SetResolution(Resolution _resolution)
        {
            ActiveResolution = _resolution;
            HandleResolution();
        }
        private void HandleResolution()
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = ActiveResolution.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = ActiveResolution.Height;
            _graphicsDeviceManager.IsFullScreen = ActiveResolution.IsFullScreen;
            _graphicsDeviceManager.ApplyChanges();
            OnResolutionChanged?.Invoke(ActiveResolution);
        }

        public string GetActiveResolution_String() => ActiveResolution.ToString();

    }

}
