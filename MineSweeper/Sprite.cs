using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MineSweeper
{
    class Sprite
    {
        public Texture2D texture;
        
        public Vector2 position;
        public Vector2 scale;


        public Vector2 rawSize { get => new Vector2(texture.Width, texture.Height); }
        public Vector2 size { get => new Vector2(texture.Width * scale.X, texture.Height * scale.Y); }

        public Rectangle rect
        {
            get
            {
                return new Rectangle(position.ToPoint(), new Vector2(texture.Width, texture.Height).ToPoint() * scale.ToPoint());
            }
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            position = new Vector2(0, 0);
            scale = new Vector2(1, 1);
        }
        public Sprite(Texture2D texture, Vector2 scale)
        {
            this.texture = texture;
            position = new Vector2(0, 0);
            this.scale = scale;
        }


    }
}
