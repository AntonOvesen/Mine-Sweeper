using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper
{
    class Board
    {
        /// <summary>
        /// When board has been initialized. Content is available through the WHOLE damn solution.
        /// </summary>
        public static ContentManager Content { get; private set; }




        public float cellSize { get; private set; }




        /// <summary>
        /// Goes from 0-8. index = number on texture
        /// </summary>
        public Texture2D[] numberTextures { get; private set; }
        public Texture2D flag;
        public Texture2D bomb;
        public Texture2D baseTexture;


        /// <summary>
        /// The basic data structure for each cell
        /// </summary>
        public struct Cell
        {
            public int amountOfBombsAround;
            public Sprite sprite;
            public bool isBomb;
            public bool isOpen;
        }
        public Cell[] cells;
        public int size;
        public bool isDoneConstructor = false;

        public enum Difficulty
        {
            easy = 1,
            medium = 2,
            hard = 3
        }

        public static GraphicsDeviceManager GDMRef;

        public Board(int size, Difficulty difficulty, ContentManager content, float scale, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.size = size;
            //Preload content for smoother gameplay
            Content = content;

            baseTexture = Content.Load<Texture2D>("Base");
            bomb = Content.Load<Texture2D>("Bomb");
            flag = Content.Load<Texture2D>("Flagged");

            numberTextures = new Texture2D[9];
            numberTextures[0] = Content.Load<Texture2D>("Blank");
            numberTextures[1] = Content.Load<Texture2D>("1");
            numberTextures[2] = Content.Load<Texture2D>("2");
            numberTextures[3] = Content.Load<Texture2D>("3");
            numberTextures[4] = Content.Load<Texture2D>("4");
            numberTextures[5] = Content.Load<Texture2D>("5");
            numberTextures[6] = Content.Load<Texture2D>("6");
            numberTextures[7] = Content.Load<Texture2D>("7");
            numberTextures[8] = Content.Load<Texture2D>("8");

            //Update window size to fit game
            GDMRef = graphicsDeviceManager;
            GDMRef.PreferredBackBufferWidth = (int)MathF.Round(baseTexture.Width * scale * size);
            GDMRef.PreferredBackBufferHeight = (int)MathF.Round(baseTexture.Height * scale * size);
            GDMRef.ApplyChanges();

            int amountOfBombsToPlant = size * (int)difficulty; //NOT SURE THIS WORKS
            cells = new Cell[size * size];

            //Can try and plant bomb in spot where there already is one
            //Adds a bit of randomness to how many bombs there are.
            for (int i = 0; i < amountOfBombsToPlant; i++)
            {
                Random rand = new Random();
                int indexToPlant = rand.Next(0, cells.Length);

                if (cells[indexToPlant].isBomb)
                    continue;
                else
                {
                    cells[indexToPlant].isBomb = true;

                    for (int j = 0; j < 9; j++)
                    {
                        int x = (j % 3) - 1;
                        int y = ((int)MathF.Floor(j / 3) % 3 * size) - size;
                        int index = indexToPlant + x + y;

                        if (index >= 0 && index < cells.Length)
                            cells[index].amountOfBombsAround++;
                    }
                }

                
            }

            //Give every cell default image.
            for (int i = 0; i < cells.Length; i++)
            {
                Sprite copy = cells[i].sprite;

                //Testing if it works
                //if (cells[i].isBomb) { copy = new Sprite(bomb, new Vector2(scale, scale)); }
                //else
                //{
                //    copy = new Sprite(numberTextures[cells[i].amountOfBombsAround], new Vector2(scale, scale));
                //}
                copy = new Sprite(baseTexture, new Vector2(scale, scale));


                int x = i % size * (int)MathF.Round(copy.size.X);
                int y = ((int)MathF.Floor(i / size) % size) * (int)MathF.Round(copy.size.Y);

                copy.position = new Vector2(x, y);

                cells[i].sprite = copy;
            }

        }

    }
}
