using System;
using System.Data;
using System.Runtime.CompilerServices;
using EngineLite.Util;
using EngineLite.GameObjects;
using Microsoft.Xna.Framework;
using Penumbra;

namespace EngineLite.GameObjects.Components
{
    public class PointLightComponent : Component
    {
        public PointLight Light{get; private set;} = null;
        
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                SetLightData();
            }
        }        
        private Vector2 _scale = new Vector2(600);

        public ShadowType ShadowType
        {
            get => _shadowType;
            set
            {
                _shadowType = value;
                SetLightData();
            }
        }        
        private ShadowType _shadowType = ShadowType.Solid;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                SetLightData();
            }
        }
        private Color _color = Color.White;

        public float Intensity
        {
            get => _intensity;
            set
            {
                _intensity = Math.Clamp(value, 0f, 1f);
                SetLightData();
            }
        }        
        private float _intensity = 1f;




        public override void Start()
        {
            Light = new PointLight
            {
                Position = Transform.Position,
                Scale = Scale,
                ShadowType = ShadowType,
                Color = Color,
                Intensity = Intensity,
                Rotation = MathHelper.ToRadians(Transform.Rotation)
            };

            Engine.Instance.LightSystem.Lights.Add(Light);

        }
        private void SetLightData()
        {
            if(Light == null){return;}

            Light.Scale = _scale;
            Light.ShadowType = _shadowType;
            Light.Color = _color;
            Light.Intensity = _intensity;
            Light.Rotation = MathHelper.ToRadians(Transform.Rotation);

        }
        public override void Update()
        {
            Light.Position = Transform.Position;
            Light.Rotation = MathHelper.ToRadians(Transform.Rotation);
        }
        protected override void OnEnabledChanged()
        {
            Light.Enabled = IsEnabled;
        }
        public override void OnDestroy()
        {
            Engine.Instance.LightSystem.Lights.Remove(Light);
            Light = null;
        }

    }
}