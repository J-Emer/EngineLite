using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Managers
{
    public class ResolutionManager
    {
        private GraphicsDeviceManager _graphics;
        private Resolution _defaultResolution;
        private Resolution _fullScreenResolution;
        private Resolution _activeResolution;
        private bool _isFullScreen = false;

        public GraphicsDeviceManager Graphics
        {
            get
            {
                return _graphics;
            }
        }
        public Resolution DefaultResolution
        {
            get 
            {
                return _defaultResolution;
            } 
        }
        public Resolution FullScreenResolution
        {
            get
            {
                return _fullScreenResolution;
            }
        }
        public Resolution ActiveResolution => _activeResolution;

        public Action<Resolution> OnResolutionChanged;

        public ResolutionManager(GraphicsDeviceManager graphics, Resolution defaultResolution)
        {
            _graphics = graphics;
            _defaultResolution = defaultResolution;
            
            //full screen resolution
            int width = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            int height = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _fullScreenResolution = new Resolution(width, height, true);

            SetResolution(_defaultResolution);
        }

        public void ToggleFullScreen()
        {
            _isFullScreen = !_isFullScreen;

            if(_isFullScreen)
            {
                SetResolution(_fullScreenResolution);
            }
            else
            {
                SetResolution(_defaultResolution);
            }
        }

        private void SetResolution(Resolution _resolution)
        {
            _graphics.PreferredBackBufferWidth = _resolution.Width;
            _graphics.PreferredBackBufferHeight = _resolution.Height;
            _graphics.IsFullScreen = _resolution.IsFullScreen;
            _graphics.ApplyChanges();

            _activeResolution = _resolution;

            OnResolutionChanged?.Invoke(_activeResolution);
        }

        public override string ToString()
        {
            return $"DefaultResolution: {_defaultResolution} | FullScreenResolution: {_fullScreenResolution} | ActiveResolution: {_activeResolution}";
        }

    }
}
