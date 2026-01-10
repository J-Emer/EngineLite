namespace EngineLite.Engine.Managers
{
    public readonly struct Resolution
    {
        public int Width { get; }
        public int Height { get; }
        public bool IsFullScreen { get; }

        public Resolution(int width, int height, bool isFullScreen)
        {
            Width = width;
            Height = height;
            IsFullScreen = isFullScreen;
        }

        public override string ToString() => $"Width: {Width} | Height: {Height} | IsFullScreen: {IsFullScreen}";
    }    
}