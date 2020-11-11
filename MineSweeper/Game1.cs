using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MineSweeper
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics;
        public static ContentManager ContentManager { get; private set; }
        private SpriteBatch _spriteBatch;
        Board mainBoard;
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Adding comment to do init commit.
            ContentManager = Content;

            mainBoard = new Board(20, Board.Difficulty.medium, 0.75f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            

        }

        ButtonState lastLeftClick;
        ButtonState lastRightClick;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            if (mainBoard.isPlay && mainBoard.isInsideScreenWindow)
            {
                ButtonState leftClick = Mouse.GetState().LeftButton;
                if (leftClick == ButtonState.Pressed && lastLeftClick == ButtonState.Released)
                {
                    mainBoard.PressCell(mainBoard.CurrentCell);
                }
                ButtonState rightClick = Mouse.GetState().RightButton;
                if (rightClick == ButtonState.Pressed && lastRightClick == ButtonState.Released)
                {
                    mainBoard.FlagCell(mainBoard.CurrentCell);
                }


                lastLeftClick = leftClick;
                lastRightClick = rightClick;
            }
            
            base.Update(gameTime);
        }

        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            for (int i = 0; i < mainBoard.boardSize * mainBoard.boardSize; i++)
            {
                _spriteBatch.Draw(mainBoard.cells[i].sprite.texture, mainBoard.cells[i].sprite.rect, Color.White);
            }
            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
