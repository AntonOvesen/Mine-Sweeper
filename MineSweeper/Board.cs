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
        
        //Simple class used to hold the loaded content for tidyness
        LoadedContent lC;

        /// <summary>
        /// The basic data structure used to represent the cells
        /// </summary>
        public struct Cell
        {
            public int amountOfBombsAround;
            public Sprite sprite;
            public bool isBomb;
            public bool isOpen;
        }
        /// <summary>
        /// The playarea
        /// </summary>
        public Cell[] cells;

        /// <summary>
        /// Returns the size of the sides. Play area will always be square.
        /// </summary>
        public int boardSize { get => (int)MathF.Sqrt(cells.Length); }
        
        /// <summary>
        /// Use this to rescale the game window
        /// </summary>
        public float cellScale 
        {
            get 
            { 
                return _cellScale; 
            }
            set
            {
                _cellScale = value;
                UpdateWindowSize();
            }
        }
        private float _cellScale;

        /// <summary>
        /// Predetermined difficulty levels
        /// </summary>
        public enum Difficulty
        {
            easy = 1,
            medium = 2,
            hard = 3
        }

        public Board(int size, Difficulty difficulty, float scale)
        {
            

            //Preload content so it doesnt have to do it during gameplay. Also put in to seperate class for tidyness.
            lC = new LoadedContent();

            //Determin how many bombs it should try and spawn depending on preset difficulty
            int amountOfBombsToPlant = size * (int)difficulty;

            //Initialize cells
            cells = new Cell[size * size];

            //Spawn bombs
            for (int i = 0; i < amountOfBombsToPlant; i++)
            {
                Random rand = new Random();
                int indexToPlant = rand.Next(0, cells.Length);

                //If the spot it tries to spawn a bomb in already has a bomb it will just continue instead.
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

                        //If the index is for a cell outside play area (doesnt exist) it'll just not do anything
                        if (index >= 0 && index < cells.Length)
                            cells[index].amountOfBombsAround++;
                    }
                }
            }

            //Initialize the cells. They all contain a sprite that holds draw informaiton.
            for (int i = 0; i < cells.Length; i++)
            {
                Sprite copy = cells[i].sprite;

                copy = new Sprite(lC.baseTexture, new Vector2(scale, scale));

                int x = i % size * (int)MathF.Round(copy.size.X);
                int y = ((int)MathF.Floor(i / size) % size) * (int)MathF.Round(copy.size.Y);

                copy.position = new Vector2(x, y);

                cells[i].sprite = copy;
            }

            //saves the scale for future use
            cellScale = scale;
        }
        /// <summary>
        /// Update window size to fit game
        /// </summary>
        private void UpdateWindowSize()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                Sprite copy = cells[i].sprite;
                copy.scale = new Vector2(cellScale, cellScale);
                int x = i % boardSize * (int)MathF.Round(copy.size.X);
                int y = ((int)MathF.Floor(i / boardSize) % boardSize) * (int)MathF.Round(copy.size.Y);

                copy.position = new Vector2(x, y);

                cells[i].sprite = copy;
            }

            Game1.Graphics.PreferredBackBufferWidth = (int)MathF.Round(lC.baseTexture.Width * cellScale) * boardSize;
            Game1.Graphics.PreferredBackBufferHeight = (int)MathF.Round(lC.baseTexture.Height * cellScale) * boardSize;
            Game1.Graphics.ApplyChanges();
        }
    }

    /// <summary>
    /// Class that holds all the textures and stuff needed for the board
    /// </summary>
    class LoadedContent
    {
        /// <summary>
        /// Goes from 0-8. index = number on texture
        /// </summary>
        public Texture2D[] numberTextures { get; private set; }

        public Texture2D flag;
        public Texture2D bomb;
        public Texture2D baseTexture;

        public LoadedContent()
        {
            ContentManager Content = Game1.ContentManager;

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
        }
    }

}
