﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MineSweeper
{
    /// <summary>
    /// The game board itself
    /// </summary>
    class Board
    {
        /// <summary>
        /// When board has been initialized. Content is available through the WHOLE damn solution.
        /// </summary>

        //Simple class used to hold the loaded content for tidyness
        public LoadedContent lC;

        public bool isPlay = true;
        /// <summary>
        /// The basic data structure used to represent the cells
        /// </summary>
        public struct Cell
        {
            public int amountOfBombsAround;
            public Sprite sprite;
            public bool isBomb;
            public bool isOpen;
            public bool isFlag;
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
        public float CellScale
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
        public Point CellSize { get => new Point((int)MathF.Round(lC.baseTexture.Width * CellScale), (int)MathF.Round(lC.baseTexture.Height * CellScale)); }
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


        public Point indexToPoint(int index)
        {
            int x = index % boardSize;
            int y = (index - x) / boardSize;

            return new Point(x, y);
        }
        public int xValueOfIndex(int index) { return indexToPoint(index).X; }
        public int yValueOfIndex(int index) { return indexToPoint(index).Y; }

        public bool isNeighbourByX(int x1, int x2)
        {
            int diff = x1 - x2;
            return (diff >= -1 && diff <= 1) ? true : false;
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
                        if (isNeighbourByX(xValueOfIndex(index), xValueOfIndex(indexToPlant)) && index >= 0 && index < cells.Length)
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
            CellScale = scale;
        }

        /// <summary>
        /// Update window size to fit game
        /// </summary>
        private void UpdateWindowSize()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                Sprite copy = cells[i].sprite;

                copy.scale = new Vector2(CellScale, CellScale);

                int x = i % boardSize * (int)MathF.Round(copy.size.X);
                int y = ((int)MathF.Floor(i / boardSize) % boardSize) * (int)MathF.Round(copy.size.Y);

                copy.position = new Vector2(x, y);

                cells[i].sprite = copy;
            }

            Game1.Graphics.PreferredBackBufferWidth = (int)MathF.Round(lC.baseTexture.Width * CellScale) * boardSize;
            Game1.Graphics.PreferredBackBufferHeight = (int)MathF.Round(lC.baseTexture.Height * CellScale) * boardSize;

            Game1.Graphics.ApplyChanges();
        }

        /// <summary>
        /// Returns true if the mouse is inside game window
        /// </summary>
        public bool isInsideScreenWindow
        {
            get
            {
                MouseState mouse = Mouse.GetState();

                bool vertical = (mouse.Y < Game1.Graphics.PreferredBackBufferHeight && mouse.Y > 0) ? true : false;
                bool horizontal = (mouse.X < Game1.Graphics.PreferredBackBufferWidth && mouse.X > 0) ? true : false;

                return (vertical && horizontal);
            }
        }

        int lastCell;
        /// <summary>
        /// Returns the index for the cell the mouse is hovering over
        /// </summary>
        public int CurrentCell
        {
            get
            {
                MouseState mouse = Mouse.GetState();

                int x = Math.Clamp(mouse.X, 0, Game1.Graphics.PreferredBackBufferWidth) / CellSize.X;
                int y = (Math.Clamp(mouse.Y, 0, Game1.Graphics.PreferredBackBufferHeight) / CellSize.Y) * boardSize;
                lastCell = x + y;
                return x + y;
            }
        }

        public void FlagCell(int index)
        {
            if (cells[index].isFlag)
            {
                cells[index].isFlag = false;
                cells[index].sprite.texture = lC.baseTexture;
            }
            else
            {
                cells[index].isFlag = true;
                cells[index].sprite.texture = lC.flag;
            }

        }

        public void PressCell(int index)
        {

            if (cells[index].isFlag) { return; }
            if (cells[index].isOpen) { return; }

            if (cells[index].isBomb)
            {
                /*TODO: Make loose condition*/
                isPlay = false;
                cells[index].sprite.texture = lC.bomb;
                return;
            }

            if (cells[index].amountOfBombsAround == 0)
            {
                cells[index].sprite.texture = lC.numberTextures[cells[index].amountOfBombsAround];
                cells[index].isOpen = true;
                //TODO: open this and all neighbours and tell them to open their neighbours unless the neighbour in focus has bombs nearby
                int lastIndex;
                for (int i = 0; i < 9; i++)
                {
                    int x = i % 3 - 1;
                    int y = (int)(MathF.Floor(i / 3) % 3 * boardSize) - boardSize;
                    int nextIndex = index + x + y;

                    bool isNeighbour = isNeighbourByX(xValueOfIndex(index), xValueOfIndex(nextIndex));

                    if (isNeighbour && nextIndex >= 0 && nextIndex < cells.Length && !cells[nextIndex].isOpen)
                    { PressCell(nextIndex); }

                }
            }
            else
            {
                cells[index].sprite.texture = lC.numberTextures[cells[index].amountOfBombsAround];
                cells[index].isOpen = true;
            }


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
