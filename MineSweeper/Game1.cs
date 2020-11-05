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
            ContentManager = Content;

            mainBoard = new Board(10, Board.Difficulty.easy, 1.2f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
