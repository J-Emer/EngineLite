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

        public ResolutionManager(GraphicsDeviceManager graphicsDevice)
        {
            _graphicsDeviceManager = graphicsDevice;

            _normalResolution = SettingsManager.Instance.Settings.DefaultResolution;
            ActiveResolution = _normalResolution;    

            _graphicsDeviceManager.PreferredBackBufferWidth = ActiveResolution.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = ActiveResolution.Height;
            _graphicsDeviceManager.IsFullScreen = ActiveResolution.IsFullScreen;
            _graphicsDeviceManager.ApplyChanges();

            Instance = this;

            Console.WriteLine($"Resolution Manager: {ActiveResolution}");
        }

        public string GetActiveResolution_String() => ActiveResolution.ToString();

    }

}
