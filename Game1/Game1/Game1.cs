using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Game1
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	/// 
	/// 
	/// 
	/// 
	//

	public enum GameState
	{
		mainMenu,
		optionsMenu,
		playing,
		paused,
		gameOver,
		gameOverMenu
	}

	public enum Sound
	{
		off,
		on
	}

	public class Button
    {
        private Texture2D texture2D;
        private Rectangle rect;
        private Vector2 position;
        private static float largeSize = 1.0f;
		private static float smallSize = 0.9f;
		private bool isLarge = false;
		protected SpriteBatch spriteBatchRef;
        protected Game gameRef;
        protected String img;

        public Button(Game gameRef, SpriteBatch spriteBatchRef, String s, int x, int y)
        {
            
            this.spriteBatchRef = spriteBatchRef;
            this.gameRef = gameRef;
            img = s;
            position.X = x;
            position.Y = y;
            position.X = x;
            position.Y = y;
            Init();
        }

        public void Init()
        {
            texture2D = gameRef.Content.Load<Texture2D>(img);
            rect = new Rectangle((int)position.X, (int)position.Y, texture2D.Width, texture2D.Height);
			isLarge = false;
        }

        public void Draw()
        {
            spriteBatchRef.Draw(texture2D, rect, Color.White);
        }

        public void Update(ref GameState state, ref Sound sound, ref Sound music, MouseState mouseState, MouseState lastMouseState, Point mousePosition)
        {
            if (img == "OnButton1" && sound == Sound.off)
            {
                img = "OffButton1";
                Init();
            }
            else if (img == "OnButton2" && music == Sound.off)
            {
                img = "OffButton2";
                Init();
            }
            else if (img == "OffButton1" && sound == Sound.on)
            {
                img = "OnButton1";
                Init();
            }
            else if (img == "OffButton2" && music == Sound.on)
            {
                img = "OnButton2";
                Init();
            }
			if (mousePosition.X >= rect.X && mousePosition.Y >= rect.Y && mousePosition.X <= rect.X + rect.Width && mousePosition.Y <= rect.Y + rect.Height)
			{
				isLarge = true;
				int w = (int)(texture2D.Width * (largeSize - 1.0f) / 2f);
				int h = (int)(texture2D.Height * (largeSize - 1.0f) / 2f);
				rect = new Rectangle((int)position.X - w, (int)position.Y - h, texture2D.Width + 2 * w, texture2D.Height + 2 * h);
				if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed)
				{
					if (img == "Play")
						state = GameState.playing;
					if (img == "Options")
						state = GameState.optionsMenu;
					if (img == "Exit")
						gameRef.Exit();
					if (img == "OffButton1")
					{
						img = "OnButton1";
						sound = Sound.on;
					}
					else if (img == "OnButton1")
					{
						img = "OffButton1";
						sound = Sound.off;
					}
					if (img == "OffButton2")
					{
						img = "OnButton2";
						music = Sound.on;
					}
					else if (img == "OnButton2")
					{
						img = "OffButton2";
						music = Sound.off;
					}
					if (img == "Back")
						state = GameState.mainMenu;
					if (img == "continue_button")
						state = GameState.playing;
					if (img == "menu_button")
					{
						state = GameState.mainMenu;
						(gameRef as Game1).GameInitialize();
					}
                    if (img == "PlayAgain")
                    {
                        
                        (gameRef as Game1).GameInitialize();
                        state = GameState.playing;
                    }
					Init();
				}
			}
			else
			{
				isLarge = false;
				int w = (int)(texture2D.Width * (smallSize - 1.0f) / 2f);
				int h = (int)(texture2D.Height * (smallSize - 1.0f) / 2f);
				rect = new Rectangle((int)position.X - w, (int)position.Y - h, texture2D.Width + 2 * w, texture2D.Height + 2 * h);
			}
        }
    }

    public class MainMenu
    {
        private Texture2D texture2D;
        private Rectangle rect;
        protected SpriteBatch spriteBatchRef;
        protected Game gameRef;
        public Button playButton, optionsButton, closeButton;

        public MainMenu(Game gameRef, SpriteBatch spriteBatchRef)
        {
            this.spriteBatchRef = spriteBatchRef;
            this.gameRef = gameRef;
            Init();
            playButton = new Button(gameRef, spriteBatchRef, "Play", 50, Game1.HEIGHT - 250);
            optionsButton = new Button(gameRef, spriteBatchRef, "Options", 59, Game1.HEIGHT - 150);
            closeButton = new Button(gameRef, spriteBatchRef, "Exit", Game1.WIDTH - 120, Game1.HEIGHT - 80);
        }

        public void Init()
        {
            rect = new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT);
            texture2D = gameRef.Content.Load<Texture2D>("Main");
        }

        public void Draw()
        {
            spriteBatchRef.Draw(texture2D, rect, rect, Color.White);
            playButton.Draw();
            optionsButton.Draw();
            closeButton.Draw();
        }

        public void Update(ref GameState state, ref Sound sound, ref Sound music, MouseState mouseState, MouseState lastMouseState, Point mousePosition)
        {
            playButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
            optionsButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
            closeButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
        }
    }

	public class OptionsMenu
	{
		private Texture2D texture2D;
		private Rectangle rect;
		protected SpriteBatch spriteBatchRef;
		protected Game gameRef;
		public Button soundButton, musicButton, backButton;

		public OptionsMenu(Game gameRef, SpriteBatch spriteBatchRef)
		{
			this.spriteBatchRef = spriteBatchRef;
			this.gameRef = gameRef;
			Init();
			soundButton = new Button(gameRef, spriteBatchRef, "OnButton1", Game1.WIDTH - 175, 110);
			musicButton = new Button(gameRef, spriteBatchRef, "OnButton2", Game1.WIDTH - 175, 190);
			backButton = new Button(gameRef, spriteBatchRef, "Back", Game1.WIDTH - 120, Game1.HEIGHT - 80);
		}

		public void Init()
		{
			rect = new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT);
			texture2D = gameRef.Content.Load<Texture2D>("OptionsBackground");
		}

		public void Draw()
		{
			spriteBatchRef.Draw(texture2D, rect, rect, Color.White);
			soundButton.Draw();
			musicButton.Draw();
			backButton.Draw();
		}

		public void Update(ref GameState state, ref Sound sound, ref Sound music, MouseState mouseState, MouseState lastMouseState, Point mousePosition)
		{
			soundButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
			musicButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
			backButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
		}
	}

	public class PauseMenu
	{
		private Texture2D texture2D;
		private Rectangle rect;
		protected SpriteBatch spriteBatchRef;
		protected Game gameRef;
		private float largeSize = 1.3f;
		public Button continueButton, menuButton, againButton;

		public PauseMenu(Game gameRef, SpriteBatch spriteBatchRef)
		{
			this.spriteBatchRef = spriteBatchRef;
			this.gameRef = gameRef;
			Init();
			continueButton = new Button(gameRef, spriteBatchRef, "continue_button", Game1.WIDTH / 2 - 32, Game1.HEIGHT - 238);
			menuButton = new Button(gameRef, spriteBatchRef, "menu_button", Game1.WIDTH / 2 - 30, Game1.HEIGHT - 130);
			againButton = new Button(gameRef, spriteBatchRef, "PlayAgain", Game1.WIDTH / 2 - 75, Game1.HEIGHT - 55);
		}

		public void Init()
		{
			texture2D = gameRef.Content.Load<Texture2D>("pause_menu");
			int w = (int)(texture2D.Width * largeSize);
			int h = (int)(texture2D.Height * largeSize);
			rect = new Rectangle((Game1.WIDTH - w) / 2, Game1.HEIGHT - h, w, h);
		}

		public void Draw()
		{
			spriteBatchRef.Draw(texture2D, rect, Color.White);
			continueButton.Draw();
			menuButton.Draw();
			againButton.Draw();
		}

		public void Update(ref GameState state, ref Sound sound, ref Sound music, MouseState mouseState, MouseState lastMouseState, Point mousePosition)
		{
			continueButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
			menuButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
			againButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
		}
	}


	public class GameOverMenu
	{
		private Texture2D texture2D;
		private Rectangle rect;
		protected SpriteBatch spriteBatchRef;
		protected Game gameRef;
		private float resize = 0.5f;
		public Button againButton, menuButton;
		private SoundEffect soundEffect;
		public SoundEffectInstance soundEffectInstance;

		public GameOverMenu(Game gameRef, SpriteBatch spriteBatchRef)
		{
			this.spriteBatchRef = spriteBatchRef;
			this.gameRef = gameRef;
			Init();
			againButton = new Button(gameRef, spriteBatchRef, "PlayAgain", rect.X + 10, rect.Y + rect.Height - 80);
			menuButton = new Button(gameRef, spriteBatchRef, "menu_button", rect.X + rect.Width - 75, rect.Y + 15);
			soundEffect = gameRef.Content.Load<SoundEffect>("You Are So Fucked");
			soundEffectInstance = soundEffect.CreateInstance();
		}

		public void Init()
		{
			texture2D = gameRef.Content.Load<Texture2D>("GameOver");
			int w = (int)(texture2D.Width * resize);
			int h = (int)(texture2D.Height * resize);
			rect = new Rectangle((Game1.WIDTH - w) / 2, (Game1.HEIGHT - h) / 2, w, h);
		}

		public void Draw()
		{
			spriteBatchRef.Draw(texture2D, rect, Color.White);

			againButton.Draw();
			menuButton.Draw();
		}

		public void Update(ref GameState state, ref Sound sound, ref Sound music, MouseState mouseState, MouseState lastMouseState, Point mousePosition)
		{
			if (music == Sound.on && soundEffectInstance.State != SoundState.Playing && state == GameState.gameOverMenu)
				soundEffectInstance.Play();
			if (music == Sound.off || state != GameState.gameOverMenu)
				soundEffectInstance.Stop();
			if (state == GameState.gameOverMenu)
			{
				againButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
				menuButton.Update(ref state, ref sound, ref music, mouseState, lastMouseState, mousePosition);
			}
		}
	}


	public class Tile
    {
        private Texture2D texture2D;

        private Vector2 position;
        private Rectangle rect;

        public static float Yspeed = 300;

        private SpriteBatch spriteBatch;
        private Game gameRef;
        public Texture2D Texture2D
        {
            get { return texture2D; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Tile(Game game, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.gameRef = game;
            Init();
        }

        public Tile(Game game, SpriteBatch spriteBatch, Vector2 startPos)
        {
            this.spriteBatch = spriteBatch;
            this.gameRef = game;
            Init();
            position = new Vector2(startPos.X, startPos.Y);
            rect.X = (int)Position.X;
            rect.Y = (int)Position.Y;
			//Yspeed = 300;
        }
        void Init()
        {
            position = new Vector2(0, 0);
            texture2D = gameRef.Content.Load<Texture2D>("Wall");
            rect = new Rectangle(0, -texture2D.Height, texture2D.Width, texture2D.Height);
            // rect = new Rectangle(50, gameRef.GraphicsDevice.Viewport.Height - 100, 80, 80);
            position.X = rect.X;
            position.Y = rect.Y;
        }

        public void Draw()
        {
            spriteBatch.Draw(texture2D, position, Color.White);
            //spriteBatch.Draw();
        }

        public void Update()
        {
            position.Y += Yspeed * (float)gameRef.TargetElapsedTime.TotalSeconds;
            rect.Y = (int)position.Y;
        }
    }


    public class GameManager
    {

        private SpriteBatch spriteBatch;
        private Game gameRef;


        private Tile firstTile;
        private Tile secondTile;

        public List<GameObject> hazards;
        

        private float timer;
        private float HazardDeathTime = 30;
        public float SpawnHazardTimeMin = 1;
        public float SpawnHazardTimeMax = 4;
        public float currSpawnHazard;
        public GameObject.Side lastSpawnedSide;
        public void SpawnHazard(GameObject.Side side)
        {
            
            
            Random r = new Random();


            SpawnHazardTimeMax -= .1f;
            if(SpawnHazardTimeMax < SpawnHazardTimeMin + .5f)
                SpawnHazardTimeMax = SpawnHazardTimeMin + .5f;
            currSpawnHazard = (float)(r.Next((int)(1000 * SpawnHazardTimeMin), (int)(1000 * SpawnHazardTimeMax))) / 1000;

            GameObject hazard = null;
            GameObject laundry = null;
            if (r.Next(0, 2) == 0)
                side = GameObject.Side.Right;
            GameObject temp = null;
            
            switch (r.Next(0, 6))
            {
                case 0:
                    {
                        hazard = new BirdEnemy(gameRef, spriteBatch, side);
                        break;
                    }
                case 1:
                    {
                        hazard = new Climber(gameRef, spriteBatch, side);
                        break;
                    }
                case 2:
                    {
                        laundry = new Laundry(gameRef, spriteBatch);
                        hazard = new Squirl(gameRef, spriteBatch, side);
                        break;
                    }
                case 3:
                    {
                        laundry = new Laundry(gameRef, spriteBatch);
                        break;
                    }
                case 4:
                    {
                        hazard = new Shield(gameRef, spriteBatch);
                        break;
                    }
                case 5:
                    {
                        hazard = new Roof(gameRef, spriteBatch,side);
                        break;
                    }
            }
            if (hazard != null)
            hazards.Add(hazard);
            if (laundry != null) 
            hazards.Add(laundry);
        }

        public GameManager(Game game, SpriteBatch spriteBatch)
        {
            this.gameRef = game;
            this.spriteBatch = spriteBatch;
            Init();
        }

        public void Draw()
        {
            //if (firstTile != null)
            firstTile.Draw();
            //if (secondTile != null)
            secondTile.Draw();
            for (int i = 0; i < hazards.Count; i++)
            {
                if (hazards[i] != null)
                    hazards[i].Draw();        
            }

            
        }

        public void Init()
        {
            
            hazards = new List<GameObject>();
            lastSpawnedSide = GameObject.Side.Left;
            SpawnHazardTimeMin = .5f;
            SpawnHazardTimeMax = 4;
            firstTile = new Tile(gameRef, spriteBatch, new Vector2(0, 0));
            secondTile = new Tile(gameRef, spriteBatch);
            currSpawnHazard = SpawnHazardTimeMax;
        }

        public bool Collision(Rectangle rect1, Rectangle rect2, float c = 0.45f)
        {
            int w1 = (int)(rect1.Width * c);
            int w2 = (int)(rect2.Width * c);
            int h1 = (int)(rect1.Height * c);
            int h2 = (int)(rect2.Height * c);
            
            rect1 = new Rectangle(rect1.X + w1 / 2, rect1.Y + h1 / 2, rect1.Width - w1, rect1.Height - h1);
            rect2 = new Rectangle(rect2.X + w2 / 2, rect2.Y + h2 / 2, rect2.Width - w2, rect2.Height - h2);

            return (rect1.X + rect1.Width >= rect2.X && rect1.X <= rect2.X + rect2.Width)
                   && (rect1.Y + rect1.Height >= rect2.Y && rect1.Y <= rect2.Y + rect2.Height);
        }

        public void Update()
        {

            firstTile.Update();
            secondTile.Update();
            timer += (float)gameRef.TargetElapsedTime.TotalSeconds;
            if (firstTile.Position.Y >= Game1.HEIGHT)
            {
                firstTile = secondTile;
                secondTile = new Tile(gameRef, spriteBatch);
            }
            if (timer >= currSpawnHazard)
            {
                timer = 0;
                SpawnHazard(lastSpawnedSide);
                if (lastSpawnedSide == GameObject.Side.Left)
                    lastSpawnedSide = GameObject.Side.Right;
                else
                    lastSpawnedSide = GameObject.Side.Left;
                
            }
            for (int i = 0; i < hazards.Count; i++)
            {
                if (hazards[i] == null || (gameRef as Game1).ElapsedTime >= hazards[i].CreateTime + HazardDeathTime)
                {
                    
                    hazards.Remove(hazards[i]);
                    i--;
                    continue;
                }
                else
                {
                    if (Collision((gameRef as Game1).ninja.Rect,hazards[i].Rect))
                    {
                        if (hazards[i] is Shield)
                        {
                            (gameRef as Game1).hasShield = true;
                           // (gameRef as Game1).score.Killed(hazards[i].LeftTexture2D, hazards[i].frameCount);
                            hazards.Remove(hazards[i]);
                            i--;
                            continue;

                        }
                        else if (!(hazards[i] is Laundry))
                        {
                            if ((gameRef as Game1).ninja.currNinjaState == Ninja.NinjaState.Jump && !(hazards[i] is Climber) && !(hazards[i] is Roof))
                            {
                                (gameRef as Game1).score.Killed(hazards[i].Name, hazards[i].frameCount);
                                hazards.Remove(hazards[i]);
                                i--;
                                continue;
                            }
                            else
                            {
                                if ((gameRef as Game1).hasShield)
                                {
                                    (gameRef as Game1).hasShield = false;
                                    (gameRef as Game1).score.Killed(hazards[i].Name, hazards[i].frameCount);
                                    hazards.Remove(hazards[i]);
                                    i--;
                                    continue;
                                }
                                else
                                (gameRef as Game1).gameState = GameState.gameOver;
                            }
                        }
                    }
                    if(hazards[i] != null)
                        hazards[i].Update();
                }
            }
        }

    }

    public class GameObject
    {
        public enum Side
        {
            Left,
            Right
        }

        public string Name;
        public float CreateTime;
        protected Texture2D leftTexture2D;

        public Texture2D LeftTexture2D
        {
            get { return leftTexture2D; }
        }

        protected Texture2D righTexture2D;
        public int frameCount;

        private int currFrame = 0;
        private int currFrameCount = 0;
        protected int FrameDrawCount = 2;


        protected Side side;
        protected Vector2 position;
        protected Rectangle rect;
        protected SpriteBatch spriteBatchRef;
        protected Game gameRef;


        public GameObject(Game game, SpriteBatch spriteBatchRef)
        {
            this.spriteBatchRef = spriteBatchRef;
            this.gameRef = game;
            
            Init();
        }

        public GameObject(Game game, SpriteBatch spriteBatchRef, Side side)
        {
            this.side = side;
            this.spriteBatchRef = spriteBatchRef;
            this.gameRef = game;
            Init();
        }


        public Vector2 Position
        {
            get { return position; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public void UpdateAnimation(Texture2D texture2D, int frameCount)
        {
            rect.Height = texture2D.Height;
            rect.Width = texture2D.Width / frameCount;
            currFrameCount++;
            if (currFrameCount >= FrameDrawCount)
            {
                currFrameCount = 0;
                currFrame = (currFrame + 1) % frameCount;
            }
        }

		public void DrawAnimation(Texture2D texture2D, int frameCount, int resize = 0, int shift = 0)
		{
			spriteBatchRef.Draw(texture2D, rect, new Rectangle(currFrame * (texture2D.Width / frameCount - resize) - shift, 0, texture2D.Width / frameCount, texture2D.Height), Color.White);
		}

		public virtual void Init()
		{
		    CreateTime = (gameRef as Game1).ElapsedTime;

		}

        public virtual void Draw()
        {
            spriteBatchRef.Draw(leftTexture2D, position, Color.White);
        }

        public virtual void Update()
        {

        }


    }

    public class Roof : GameObject
    {
        public Roof(Game game, SpriteBatch spriteBatch, Side side) : base(game, spriteBatch, side)
        {
            
        }

        
        private static Vector2 LEFT_POSITION = new Vector2(36 - 30, -100);
        private static Vector2 RIGHT_POSITION = new Vector2(500 - 36 - 50, -100);

        public Vector2 climbingSpeed = new Vector2(0,Tile.Yspeed);


        public override void Init()
        {
            base.Init();
            Random r = new Random();
            frameCount = 1;
          
            if (side == Side.Left)
                position = LEFT_POSITION;
            else
                position = RIGHT_POSITION;
            leftTexture2D = gameRef.Content.Load<Texture2D>("overhang");
            righTexture2D = gameRef.Content.Load<Texture2D>("FlippedOverhang");
            rect = new Rectangle((int)position.X, (int)position.Y, leftTexture2D.Width / frameCount, leftTexture2D.Height);
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }

        public override void Update()
        {
            base.Update();
           
            position.X += climbingSpeed.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
            position.Y += climbingSpeed.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
			if (side == Side.Left)
				UpdateAnimation(leftTexture2D, frameCount);
			else
				UpdateAnimation(righTexture2D, frameCount);
			rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }

        public override void Draw()
        {
            if (side == Side.Left)
                DrawAnimation(leftTexture2D, frameCount);
            else
                DrawAnimation(righTexture2D, frameCount);
            
            //base.Draw();
        }
    
    }

    public class BirdEnemy : GameObject
    {
        private static Vector2 LEFT_POSITION = new Vector2(36, 0);
        private static Vector2 RIGHT_POSITION = new Vector2(500 - 36 - 75, 0);
        private float AttackDelay = 1;
        private static Vector2 AttackDirection = new Vector2(Ninja.RIGHT_POSITION - LEFT_POSITION.X, Ninja.HEIGHT);
        private float MoveSpeed = 200;
        private float timer = 0;
        //private bool canAttack = false;
        public BirdEnemy(Game game, SpriteBatch spriteBatchRef) : base(game, spriteBatchRef)
        {

        }

        public BirdEnemy(Game game, SpriteBatch spriteBatchRef, Side side) : base(game, spriteBatchRef, side)
        {
            
        }

        public override void Init()
        {
            base.Init();
            Name = "BirdAnimation";
            //leftTexture2D = 
            frameCount = 12;
            if (side == Side.Left)
                rect = new Rectangle((int)LEFT_POSITION.X, (int)LEFT_POSITION.Y, 80, 80);
            else
            {
                //AttackDirection.X *= -1;
                rect = new Rectangle((int)RIGHT_POSITION.X, (int)RIGHT_POSITION.Y, 80, 80);
            }
            position.X = rect.X;
            position.Y = rect.Y;
            // currNinjaState = NinjaState.Left;
            leftTexture2D = gameRef.Content.Load<Texture2D>("BirdAnimation");
            righTexture2D = gameRef.Content.Load<Texture2D>("FlippedBirdAnimation");
            AttackDirection.Normalize();


        }

        public override void Update()
        {
            base.Update();
            timer += (float)gameRef.TargetElapsedTime.TotalSeconds;
            if (timer >= AttackDelay)
            {
                if (side == Side.Left)
                    position.X += AttackDirection.X * MoveSpeed * (float)gameRef.TargetElapsedTime.TotalSeconds;
                else
                    position.X += AttackDirection.X * -MoveSpeed * (float)gameRef.TargetElapsedTime.TotalSeconds;
                position.Y += AttackDirection.Y * MoveSpeed * (float)gameRef.TargetElapsedTime.TotalSeconds;
                rect.X = (int)position.X;
                rect.Y = (int)position.Y;
            }
			if (side == Side.Left)
				UpdateAnimation(leftTexture2D, frameCount);
			else
				UpdateAnimation(righTexture2D, frameCount);
		}

        public override void Draw()
        {
            if (side == Side.Left)
                DrawAnimation(leftTexture2D, frameCount);
            else
                DrawAnimation(righTexture2D, frameCount);
        }


    }

    public class Shuriken : GameObject
    {

        public Vector2 MoveSpeed = new Vector2(100, 0);

        public Shuriken(Game game, SpriteBatch spriteBatch, Vector2 startPosition, Vector2 moveSpeed) : base(game, spriteBatch)
        {
            this.position = new Vector2(startPosition.X, startPosition.Y);
            MoveSpeed = new Vector2(moveSpeed.X, moveSpeed.Y);
        }

        public override void Init()
        {
            base.Init();
            Name = "Shuriken";
            leftTexture2D = gameRef.Content.Load<Texture2D>("Shuriken");
            rect = new Rectangle((int)position.X, (int)position.Y, leftTexture2D.Width, leftTexture2D.Height);
            frameCount = 4;
            FrameDrawCount = 1;
            

        }

        public override void Draw()
        {
            DrawAnimation(leftTexture2D, frameCount);
        }

        public override void Update()
        {
            base.Update();
            position.X += MoveSpeed.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
            position.Y += MoveSpeed.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
			UpdateAnimation(leftTexture2D, frameCount);
		}
    }

    public class Squirl : GameObject
    {
        private static Vector2 LEFT_POSITION = new Vector2(36, -63 + 27 -100);
        private static Vector2 RIGHT_POSITION = new Vector2(500 - 36 - 100, -63 + 27 -100);


        public Squirl(Game game, SpriteBatch spriteBatch, Side side) : base(game, spriteBatch, side)
        {
            
        }



        public Vector2 MoveSpeed = new Vector2(190, Tile.Yspeed);



        public override void Init()
        {
            base.Init();
            Name = "Squirrel";
            leftTexture2D = gameRef.Content.Load<Texture2D>("Squirrel");
            righTexture2D = gameRef.Content.Load<Texture2D>("SquirrelFlipped");
            if (side == Side.Left)
                position = LEFT_POSITION;
            else
            {
                position = RIGHT_POSITION;
                MoveSpeed.X *= -1;
            }
            frameCount = 9;
            FrameDrawCount = 2;
            rect = new Rectangle((int)position.X, (int)position.Y, leftTexture2D.Width / frameCount, leftTexture2D.Height);


        }

        public override void Draw()
        {
            if (side == Side.Left)
                DrawAnimation(leftTexture2D, frameCount, 1, 0);
            else
                DrawAnimation(righTexture2D, frameCount, 1, -8);
        }

        public override void Update()
        {
            base.Update();

            position.X += MoveSpeed.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
            position.Y += MoveSpeed.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
            if (position.X >= RIGHT_POSITION.X)
            {
                MoveSpeed.X *= -1;
                side = Side.Right;
            }
            if (position.X <= LEFT_POSITION.X)
            {
                MoveSpeed.X *= -1;
                side = Side.Left;
            }
			if (side == Side.Left)
				UpdateAnimation(leftTexture2D, frameCount);
			else
				UpdateAnimation(righTexture2D, frameCount);
			rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }
    }
    public class Climber : GameObject
    {

        private static Vector2 LEFT_POSITION = new Vector2(36 - 30, 0 -100);
        private static Vector2 RIGHT_POSITION = new Vector2(500 - 36 - 50, 0 -100);
        private int shurikenThrowDelayMin = 200; // millisecond
        private int shurikenThrowDelayMax = 1000; // millisecond
        private float currshurikenThrowDelay;
        private int ShurikenNum = 2;

        public Vector2 climbingSpeed = new Vector2(0,Tile.Yspeed);

        private float timer = 0;
        private bool threwShuriken;

        public Climber(Game game, SpriteBatch spriteBatch, Side side)
            : base(game, spriteBatch,side)
        {
            //this.climbingSpeed = new Vector2(MoveSpeed.X, MoveSpeed.Y);
            this.side = side;
        }
        public override void Init()
        {
            base.Init();
            Random r = new Random();
            frameCount = 1;
            currshurikenThrowDelay = r.Next(shurikenThrowDelayMin, shurikenThrowDelayMax) / 1000;
            if (side == Side.Left)
                position = LEFT_POSITION;
            else
                position = RIGHT_POSITION;
            leftTexture2D = gameRef.Content.Load<Texture2D>("Thrower");
            righTexture2D = gameRef.Content.Load<Texture2D>("ThrowerFlipped");
            rect = new Rectangle((int)position.X, (int)position.Y, leftTexture2D.Width / frameCount, leftTexture2D.Height);
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }


        private void ThrowShuriken(int shurikenNum)
        {
            threwShuriken = true;
            int i = 1;
            if (side == Side.Right)
                i = -1;
            (gameRef as Game1).gameManager.hazards.Add(new Shuriken(gameRef, spriteBatchRef, position, climbingSpeed + new Vector2(i * 210, 0)));
            if (shurikenNum > 1)
            { 
                (gameRef as Game1).gameManager.hazards.Add(new Shuriken(gameRef, spriteBatchRef, position, climbingSpeed + new Vector2(i * 210, -100)));
            }
        }

        public override void Update()
        {
            base.Update();
            timer += (float)gameRef.TargetElapsedTime.TotalSeconds;
            position.X += climbingSpeed.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
            position.Y += climbingSpeed.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
            if (timer >= currshurikenThrowDelay && threwShuriken == false)
            {
                Random r = new Random();
                ThrowShuriken(r.Next(1, 3));
            }
			if (side == Side.Left)
				UpdateAnimation(leftTexture2D, frameCount);
			else
				UpdateAnimation(righTexture2D, frameCount);
			rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }

        public override void Draw()
        {
            if (side == Side.Left)
                DrawAnimation(leftTexture2D, frameCount);
            else
                DrawAnimation(righTexture2D, frameCount);
            
            //base.Draw();
        }
    }
   
    public class Laundry : GameObject
    {

        public Vector2 MoveSpeed = new Vector2(0, Tile.Yspeed);
        private Side secondSide = Side.Left;
        public Laundry(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            //this.position = new Vector2(startPosition.X,startPosition.Y);
            //MoveSpeed = new Vector2(moveSpeed.X, moveSpeed.Y);
        }



        public override void Init()
        {
            base.Init();
            Random r = new Random();
			side = secondSide = Side.Left;
            if (r.Next(0, 2) >= 1)
                side = Side.Right;
            if (r.Next(0, 2) >= 1)
                secondSide = Side.Right;
            leftTexture2D = gameRef.Content.Load<Texture2D>("laundry");
            righTexture2D = gameRef.Content.Load<Texture2D>("laundryFlipped");
            position = new Vector2(-6, -100);
            rect = new Rectangle((int)position.X, (int)position.Y, 2 * leftTexture2D.Width, leftTexture2D.Height);
            //frameCount = 4;

        }

        public override void Draw()
        {
            if (side == Side.Left)
                spriteBatchRef.Draw(leftTexture2D, position, Color.White);
            else
                spriteBatchRef.Draw(righTexture2D, position, Color.White);

            if (secondSide == Side.Left)
                spriteBatchRef.Draw(leftTexture2D, new Vector2(position.X + leftTexture2D.Width, position.Y), Color.White);
            else
                spriteBatchRef.Draw(righTexture2D, new Vector2(position.X + leftTexture2D.Width, position.Y), Color.White);
        }

        public override void Update()
        {
            base.Update();
            position.X += MoveSpeed.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
            position.Y += MoveSpeed.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }
    }

    public class Shield : Laundry
    {
        public Shield(Game game, SpriteBatch spriteBatch): base(game,spriteBatch)
        {
           
        }
        public override void Draw()
        {
            spriteBatchRef.Draw(leftTexture2D, position, Color.White);
        }
		public override void Init()
        {
            Random r = new Random();
            leftTexture2D = gameRef.Content.Load<Texture2D>("ShieldSmall");
            position = new Vector2(0, -100);
            if (r.Next(0, 2) >= 1)
            {
                side = Side.Right;
                position.X = Game1.WIDTH - 36 - leftTexture2D.Width + 4;
            }
            else
            {
                side = Side.Left;
                position.X = 36 - 4;
            }
            // righTexture2D = gameRef.Content.Load<Texture2D>("laundryFlipped");
            rect = new Rectangle((int)position.X, (int)position.Y, 2 * leftTexture2D.Width, leftTexture2D.Height);
        }
    }
	
	public class PlayingForeground
	{
		private Texture2D texture2D;
		private Rectangle rect;
		protected SpriteBatch spriteBatchRef;
		protected Game gameRef;
		private static float velocity = 110f;
		public Vector2 position;

		public PlayingForeground(Game gameRef, SpriteBatch spriteBatchRef)
		{
			this.spriteBatchRef = spriteBatchRef;
			this.gameRef = gameRef;
			Init();
		}

		public void Init()
		{
			texture2D = gameRef.Content.Load<Texture2D>("Foreground");
			position = new Vector2(0, Game1.HEIGHT / 3);
			rect = new Rectangle((int)position.X, (int)position.Y, Game1.WIDTH, texture2D.Height * Game1.WIDTH / texture2D.Width);
		}

		public void Draw()
		{
			if (position.Y <= Game1.HEIGHT)
				spriteBatchRef.Draw(texture2D, rect, Color.White);
		}

		public void Update()
		{
			if (position.Y <= Game1.HEIGHT)
			{
				position.Y += velocity * (float)gameRef.TargetElapsedTime.TotalSeconds;
				rect.Y = (int)position.Y;
			}
		}
	}

    public class Ninja
    {
        private Texture2D WalkLeftTexture2D;
        private Texture2D WalkRightTexture2D;
        private Texture2D ShieldTexture2D;
        private static int WalkFrameCount = 16;

        private Texture2D JumpTexture2D;
        private static int JumpFrameCount = 10;

        private Texture2D FallTexture2D;
        private static int FallRows = 3;
        private static int FallColumns = 6;

        private Vector2 position;

        public static float LEFT_POSITION = 34;
        public static float RIGHT_POSITION = 500 - 34 - 80;
        public static float HEIGHT = Game1.HEIGHT - 150;

        private Rectangle rect;
        private SpriteBatch spriteBatchRef;
        private Microsoft.Xna.Framework.Game gameRef;

        public bool hasShield = false;

        private Vector2 velocity;
        private Vector2 jumpStartVelocity;
        private float Gravity;
        public static float MAX_HEIGHT = 100;
        public static float X_SPEED = 450;

        public const int jumpFreezeTime = 90;
        private int framesAfterLastJump;

        private int currFrame = 0;
        private int currFrameCount = 0;
        private int FrameDrawCount = 2;

		private SoundEffect walkSoundEffect, jumpSoundEffect, fallSoundEffect;
		public SoundEffectInstance soundEffectInstance;



        public enum NinjaState
        {
            Left,
            Right,
            Jump,
            Special,
            FallLeft,
            FallRight
        }

        public NinjaState currNinjaState;



        public Vector2 Position
        {
            get { return position; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Ninja(Game game, SpriteBatch spriteBatch)
        {
            this.spriteBatchRef = spriteBatch;
            this.gameRef = game;
            Init();
        }

        public void UpdateAnimation(Texture2D texture2D, int frameCount)
        {
            rect.Height = texture2D.Height;
            rect.Width = texture2D.Width / frameCount;
            currFrameCount++;
            if (currFrameCount >= FrameDrawCount || currFrame >= frameCount)
            {
                currFrameCount = 0;
                currFrame = (currFrame + 1) % frameCount;
            }
        }

        public void UpdateAnimation2(Texture2D texture2D, int columns, int rows)
        {
            rect.Height = texture2D.Height / rows;
            rect.Width = texture2D.Width / columns;
            currFrameCount++;
            if (currFrameCount >= FrameDrawCount)
            {
                currFrameCount = 0;
                currFrame++;
            }
            if (currFrame == columns * rows)
                currFrame = columns;
        }

        public void DrawAnimation2(Texture2D texture2D, int columns, int rows, bool flip)
        {
            if (flip)
                spriteBatchRef.Draw(texture2D, rect, new Rectangle((currFrame % columns) * texture2D.Width / columns, (currFrame / columns) * texture2D.Height / rows, texture2D.Width / columns, texture2D.Height / rows), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatchRef.Draw(texture2D, rect, new Rectangle((currFrame % columns) * texture2D.Width / columns, (currFrame / columns) * texture2D.Height / rows, texture2D.Width / columns, texture2D.Height / rows), Color.White);
        }

        public void DrawAnimation(Texture2D texture2D, int frameCount)
        {
            spriteBatchRef.Draw(texture2D, rect, new Rectangle(currFrame * texture2D.Width / frameCount, 0, texture2D.Width / frameCount, texture2D.Height), Color.White);
        }


        public void Init()
        {
            velocity = new Vector2(0, 0);
            jumpStartVelocity = new Vector2(X_SPEED, 0);
            float t = (RIGHT_POSITION - LEFT_POSITION) / jumpStartVelocity.X;
            jumpStartVelocity.Y = -4 * MAX_HEIGHT / t;
            //Gravity = jumpStartVelocity.X*jumpStartVelocity.Y/(RIGHT_POSITION - LEFT_POSITION);
            Gravity = -2 * jumpStartVelocity.Y / t;
            rect = new Rectangle((int)LEFT_POSITION, (int)HEIGHT, 80, 80);
            position.X = rect.X;
            position.Y = rect.Y;
            framesAfterLastJump = jumpFreezeTime + 1;
            currNinjaState = NinjaState.Left;
            WalkLeftTexture2D = gameRef.Content.Load<Texture2D>("ClimbingAnimation");
            WalkRightTexture2D = gameRef.Content.Load<Texture2D>("ClimbingAnimationFlip");
            JumpTexture2D = gameRef.Content.Load<Texture2D>("JumpingAnimation");
            FallTexture2D = gameRef.Content.Load<Texture2D>("FallingAnimation");
            ShieldTexture2D = gameRef.Content.Load<Texture2D>("ShieldSmall");
			walkSoundEffect = gameRef.Content.Load<SoundEffect>("footsteps");
			jumpSoundEffect = gameRef.Content.Load<SoundEffect>("jump_7");
			fallSoundEffect = gameRef.Content.Load<SoundEffect>("fall");
			soundEffectInstance = walkSoundEffect.CreateInstance();
			soundEffectInstance.Stop();
		}


        public void Draw(int t)
        {
			
            switch (currNinjaState)
            {
                case NinjaState.Left:
                    {
                        DrawAnimation(WalkLeftTexture2D, WalkFrameCount);
                        break;
                    }
                case NinjaState.Right:
                    {
                        DrawAnimation(WalkRightTexture2D, WalkFrameCount);
                        break;

                    }
                case NinjaState.Jump:
                    {
                        DrawAnimation(JumpTexture2D, JumpFrameCount);
                        break;
                    }
                case NinjaState.Special:
                    {
                        break;
                    }
                case NinjaState.FallLeft:
                    {
                        DrawAnimation2(FallTexture2D, FallColumns, FallRows, false);
                        break;
                    }
                case NinjaState.FallRight:
                    {
                        DrawAnimation2(FallTexture2D, FallColumns, FallRows, true);
                        break;
                    }
            }

			if ((gameRef as Game1).hasShield)
			{ 
				Rectangle tmpRect = new Rectangle(0, 0, ShieldTexture2D.Width, ShieldTexture2D.Height);
				Vector2 origin = new Vector2(ShieldTexture2D.Width / 2, ShieldTexture2D.Height / 2);
				if (currNinjaState == NinjaState.Left)
					spriteBatchRef.Draw(ShieldTexture2D, new Rectangle(rect.X + 21, rect.Y + 36, ShieldTexture2D.Width, ShieldTexture2D.Height), tmpRect,
						Color.White, (float)(-t * 3.14 / 20.0), origin, SpriteEffects.None, 0);
				else if (currNinjaState == NinjaState.Right)
					spriteBatchRef.Draw(ShieldTexture2D, new Rectangle(rect.X + 58, rect.Y + 35, ShieldTexture2D.Width, ShieldTexture2D.Height), tmpRect,
						Color.White, (float)(t * 3.14 / 20.0), origin, SpriteEffects.None, 0);
				else
					spriteBatchRef.Draw(ShieldTexture2D, new Rectangle(rect.X + 62, rect.Y + 40, ShieldTexture2D.Width, ShieldTexture2D.Height), tmpRect,
						Color.White, (float)(-t * 3.14 / 20.0), origin, SpriteEffects.None, 0);
			}

		}


        public void Update(ref GameState state, Sound sound)
        {
            framesAfterLastJump++;
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space) && (currNinjaState == NinjaState.Left || currNinjaState == NinjaState.Right) && framesAfterLastJump > jumpFreezeTime)
            {
                velocity.X = jumpStartVelocity.X;
                jumpStartVelocity.X *= -1;
                velocity.Y = jumpStartVelocity.Y;
                currNinjaState = NinjaState.Jump;
                framesAfterLastJump = 0;
				soundEffectInstance.Stop();
            }
            if (state == GameState.gameOver)
            {
				if (currNinjaState != NinjaState.FallLeft && currNinjaState != NinjaState.FallRight)
				{
					velocity.X = jumpStartVelocity.X / 2f;
					velocity.Y = jumpStartVelocity.Y * 1.2f;
					soundEffectInstance.Stop();
					//TODO :-?
				}
                if (currNinjaState == NinjaState.Left || (currNinjaState == NinjaState.Jump && rect.X + rect.Width / 2 < Game1.WIDTH / 2))
                    currNinjaState = NinjaState.FallLeft;
				if (currNinjaState == NinjaState.Right || (currNinjaState == NinjaState.Jump && rect.X + rect.Width / 2 >= Game1.WIDTH / 2))
					currNinjaState = NinjaState.FallRight;
            }
            switch (currNinjaState)
            {
                case NinjaState.Left:
                    {
                        UpdateAnimation(WalkLeftTexture2D, WalkFrameCount);
                        break;
                    }
                case NinjaState.Right:
                    {
                        UpdateAnimation(WalkRightTexture2D, WalkFrameCount);
                        break;
                    }
                case NinjaState.Jump:
                    {
                        position.X += velocity.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        position.Y += velocity.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        velocity.Y += Gravity * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        if (position.X >= RIGHT_POSITION)
                        {
                            velocity = new Vector2();
                            position = new Vector2(RIGHT_POSITION, HEIGHT);
                            rect.X = (int)RIGHT_POSITION;
                            rect.Y = (int)HEIGHT;
                            currNinjaState = NinjaState.Right;
                        }
                        if (position.X <= LEFT_POSITION)
                        {
                            velocity = new Vector2();
                            position = new Vector2(LEFT_POSITION, HEIGHT);
                            rect.X = (int)LEFT_POSITION;
                            rect.Y = (int)HEIGHT;
                            currNinjaState = NinjaState.Left;
                        }
                        UpdateAnimation(JumpTexture2D, JumpFrameCount);
                        break;
                    }
                case NinjaState.Special:
                    {
                        break;
                    }
                case NinjaState.FallLeft:
                    {
						if (velocity.X < 0)
							velocity.X = -velocity.X;
                        position.X += velocity.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        position.Y += velocity.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        velocity.Y += Gravity * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        UpdateAnimation2(FallTexture2D, FallColumns, FallRows);
                        break;
                    }
                case NinjaState.FallRight:
                    {
						if (velocity.X > 0)
							velocity.X = -velocity.X;
						position.X += velocity.X * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        position.Y += velocity.Y * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        velocity.Y += Gravity * (float)gameRef.TargetElapsedTime.TotalSeconds;
                        UpdateAnimation2(FallTexture2D, FallColumns, FallRows);
                        break;
                    }
            }
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
			if (rect.Y > Game1.HEIGHT)
			{
				state = GameState.gameOverMenu;
				currNinjaState = NinjaState.Special;
			}
			if (sound == Sound.on)
			{
				if ((currNinjaState == NinjaState.Left || currNinjaState == NinjaState.Right) && (soundEffectInstance.State != SoundState.Playing))
				{
					soundEffectInstance = walkSoundEffect.CreateInstance();
					soundEffectInstance.IsLooped = false;
					soundEffectInstance.Play();
				}
				if (currNinjaState == NinjaState.Jump && (soundEffectInstance.State != SoundState.Playing))
				{
					soundEffectInstance = jumpSoundEffect.CreateInstance();
					soundEffectInstance.IsLooped = false;
					soundEffectInstance.Play();
				}
				if ((currNinjaState == NinjaState.FallLeft || currNinjaState == NinjaState.FallRight) && (soundEffectInstance.State != SoundState.Playing))
				{
					soundEffectInstance = fallSoundEffect.CreateInstance();
					soundEffectInstance.IsLooped = false;
					soundEffectInstance.Play();
				}
			}
			else
				soundEffectInstance.Stop();
		}

    }
    public class Score
    {
        private Texture2D texture2D;
        private Rectangle rect;
        private Rectangle lastEnemyRect;
        public SpriteFont font;
		public SpriteFont font2;
		public SpriteFont font3;
		protected SpriteBatch spriteBatchRef;
        protected Game gameRef;
        public Texture2D lastEnemy;
        public string lastEnemyName;
        public int count;

        public int value;
        public int frames;

		public int highScore = 0;

        public Score(Game gameRef, SpriteBatch spriteBatchRef)
        {
            this.spriteBatchRef = spriteBatchRef;
            this.gameRef = gameRef;
            font = gameRef.Content.Load<SpriteFont>("SpriteFont1");
			font2 = gameRef.Content.Load<SpriteFont>("SpriteFont2");
			font3 = gameRef.Content.Load<SpriteFont>("SpriteFont3");
			Init();
        }

        public void Init()
        {
            texture2D = gameRef.Content.Load<Texture2D>("score_bar");
            rect = new Rectangle(2, Game1.HEIGHT - texture2D.Height, texture2D.Width, texture2D.Height);
            lastEnemyRect = new Rectangle(Game1.WIDTH / 2 - 80, Game1.HEIGHT - 90, 0, 0);
            value = 0;
            frames = 0;
            count = 0;
            lastEnemy = null;
        }

        public void Draw(GameState state)
        {
            spriteBatchRef.Draw(texture2D, rect, Color.White);
            spriteBatchRef.DrawString(font, value.ToString(), new Vector2(25, Game1.HEIGHT - 27), Color.White);
            for (int i = 0; i < count; i++)
            {
                Rectangle tmpRect = new Rectangle(lastEnemyRect.X + i * (lastEnemyRect.Width + 2), lastEnemyRect.Y, lastEnemyRect.Width, lastEnemyRect.Height);
                spriteBatchRef.Draw(lastEnemy, tmpRect, new Rectangle(0, 0, lastEnemyRect.Width, lastEnemyRect.Height), Color.White);
            }
			if (state == GameState.gameOverMenu)
			{
				LoadFromFile();
				SaveToFile();
				spriteBatchRef.DrawString(font3, value.ToString(), new Vector2(Game1.WIDTH / 2 - 65, Game1.HEIGHT / 2 - 60), Color.White);
				spriteBatchRef.DrawString(font2, highScore.ToString(), new Vector2(Game1.WIDTH / 2 + 60, Game1.HEIGHT / 2 - 42), Color.White);
			}
        }

        public void Update()
        {
            frames++;
            if (frames % 3 == 0)
                value++;
        }

        public void Killed(string name, int div)
        {
            if (name == null)
            return;
            Debug.Print(name);
            lastEnemy = gameRef.Content.Load<Texture2D>(name);
            lastEnemyRect.Y = Game1.HEIGHT - (lastEnemy.Height + 0);
            lastEnemyRect.Width = lastEnemy.Width / div;
            lastEnemyRect.Height = lastEnemy.Height;
                if (lastEnemyName == name)
                {
                    count++;
                    if (count >= 3)
                    {
                        value += 2000;
                        count = 0;
                        lastEnemyName = " ";
                    }
                }
                else
                {
                    lastEnemyName = name;
                    count = 1;
                }
            

        }

		public void LoadFromFile()
		{
			StreamReader sr = new StreamReader("High Score.txt");
			highScore = 0;
			if (sr != null && !sr.EndOfStream)
			{
				int tmp = int.Parse(sr.ReadLine());
				highScore = tmp;
			}
			sr.Close();
		}

		public void SaveToFile()
		{
			if (value > highScore)
			{
				StreamWriter sw = new StreamWriter("High Score.txt");
				sw.WriteLine(value);
				highScore = value;
				sw.Close();
			}
		}
	}

    public class PlayingBackground
    {
        private Texture2D texture2D;
        private Texture2D secondTexture2D;
        private Rectangle rect;
        private Rectangle secondRect;
        protected SpriteBatch spriteBatchRef;
        protected Game gameRef;
        private static float velocity = 5f;
        private float secondY;

        public PlayingBackground(Game gameRef, SpriteBatch spriteBatchRef)
        {
            this.spriteBatchRef = spriteBatchRef;
            this.gameRef = gameRef;
            Init();
        }

        public void Init()
        {
            texture2D = gameRef.Content.Load<Texture2D>("PlayingBackground");
            secondTexture2D = gameRef.Content.Load<Texture2D>("PlayingBackground2");
            rect = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
            secondY = Game1.HEIGHT / 3;
            secondRect = new Rectangle((Game1.WIDTH - secondTexture2D.Width) / 2, (int)secondY, secondTexture2D.Width, secondTexture2D.Height);
        }

        public void Draw()
        {
            spriteBatchRef.Draw(texture2D, rect, Color.White);
            if (rect.Y <= Game1.HEIGHT)
                spriteBatchRef.Draw(secondTexture2D, secondRect, Color.White);
        }

        public void Update()
        {
            if (rect.Y <= Game1.HEIGHT)
            {
                secondY += velocity * (float)gameRef.TargetElapsedTime.TotalSeconds;
                secondRect.Y = (int)secondY;
            }
        }
    }

	public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int HEIGHT = 600;
        public static int WIDTH = 500;

        public MouseState mouseState = Mouse.GetState();
        public MouseState lastMouseState;
        public Point mousePosition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ///if (mouseState.LeftButton == ButtonState.Pressed)

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public Ninja ninja;

	    public bool hasShield = false;

        //protected BirdEnemy bird;
        
        
        public GameManager gameManager;
		public MainMenu mainMenu;
		public OptionsMenu optionsMenu;
		public PauseMenu pauseMenu;
		public GameOverMenu gameOverMenu;
		public GameState gameState;
		public Sound sound, music;
		public Score score;
		public PlayingBackground playingBackground;
		public PlayingForeground playingForeground;




	    public void GameInitialize()
	    {
	        Initialize();
	    }
		protected override void Initialize()
        {

            base.Initialize();
			gameState = GameState.mainMenu;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            ninja = new Ninja(this, spriteBatch);
			mainMenu = new MainMenu(this, spriteBatch);
			pauseMenu = new PauseMenu(this, spriteBatch);
			gameOverMenu = new GameOverMenu(this, spriteBatch);
			optionsMenu = new OptionsMenu(this, spriteBatch);
			gameManager = new GameManager(this, spriteBatch);
			playingBackground = new PlayingBackground(this, spriteBatch);
			playingForeground = new PlayingForeground(this, spriteBatch);
			score = new Score(this, spriteBatch);
			this.IsMouseVisible = true;
			lastMouseState = mouseState;
			mouseState = Mouse.GetState();
			mousePosition = new Point(mouseState.X, mouseState.Y);
			ninja.hasShield = false;
		}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// pub
		///

		SoundEffect soundEffect;
	    public float ElapsedTime = 0;
        protected override void Update(GameTime gameTime)
        {
			lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;
            if (gameState == GameState.mainMenu)
                mainMenu.Update(ref gameState, ref sound, ref music, mouseState, lastMouseState, mousePosition);
            if (gameState == GameState.optionsMenu)
                optionsMenu.Update(ref gameState, ref sound, ref music, mouseState, lastMouseState, mousePosition);
            if (gameState == GameState.playing || gameState == GameState.gameOver)
            {
                ElapsedTime += (float)TargetElapsedTime.TotalSeconds;
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                    gameState = GameState.paused;
           
                if (gameState != GameState.gameOver)
                {
                    
                    gameManager.Update();
                    score.Update();
                    playingBackground.Update();
                    playingForeground.Update();
                }
				ninja.Update(ref gameState, sound);
			}
            if (gameState == GameState.paused)
            {
                pauseMenu.Update(ref gameState, ref sound, ref music, mouseState, lastMouseState, mousePosition);
            }
            //if (gameState == GameState.gameOverMenu)
                gameOverMenu.Update(ref gameState, ref sound, ref music, mouseState, lastMouseState, mousePosition);
			// TODO: Add your update logic here

			if (sound == Sound.off || (gameState != GameState.playing && gameState != GameState.gameOver && gameState != GameState.gameOverMenu))
				ninja.soundEffectInstance.Stop();

			base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.mainMenu)
                mainMenu.Draw();
            if (gameState == GameState.optionsMenu)
                optionsMenu.Draw();
            if (gameState == GameState.playing || gameState == GameState.paused || gameState == GameState.gameOverMenu || gameState == GameState.gameOver)
            {
                playingBackground.Draw();
                
                gameManager.Draw();
				ninja.Draw(score.frames);

				playingForeground.Draw();

				if (gameState == GameState.gameOverMenu)
					gameOverMenu.Draw();

				score.Draw(gameState);
            }
            if (gameState == GameState.paused)
                pauseMenu.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
