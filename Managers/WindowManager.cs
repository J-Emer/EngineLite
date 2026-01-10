using System;
using Microsoft.Xna.Framework;

namespace EngineLite.Engine.Managers
{
    public class WindowManager
    {
        public static WindowManager Instance { get; private set; }
        public GameWindow Window { get; private set;}
        public Action<object, FileDropEventArgs> OnFileDrop;
        public Action<object, EventArgs> OnClientSizeChanged;



        public WindowManager(GameWindow window)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));

            Window.Title = SettingsManager.Instance.Settings.GameTitle;
            window.ClientSizeChanged += ClientSizeChanged;
            window.FileDrop += FileDrop;

            Instance = this;
        }

        private void FileDrop(object sender, FileDropEventArgs e)
        {
           //todo: do Engine specific things when a file is dropped on screen
            OnFileDrop?.Invoke(sender, e);
        }

        private void ClientSizeChanged(object sender, EventArgs e)
        {
            //todo: do Engine specific things when the window size is changed
            OnClientSizeChanged?.Invoke(sender, e);
        }
    }
}
