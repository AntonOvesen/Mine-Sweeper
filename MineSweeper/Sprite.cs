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

        /// <summary>
        /// Size without scale
        /// </summary>
        public Vector2 rawSize { get => new Vector2(texture.Width, texture.Height); }

        /// <summary>
        /// Size with scale
        /// </summary>
        public Vector2 size { get => new Vector2(texture.Width * scale.X, texture.Height * scale.Y); }

        /// <summary>
        /// Rectangle with info from position & scale
        /// </summary>
        public Rectangle rect
        {
            get
            {
                return new Rectangle((int)position.X,(int)position.Y, (int)MathF.Round(texture.Width * scale.X), (int)MathF.Round(texture.Height * scale.Y));
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
