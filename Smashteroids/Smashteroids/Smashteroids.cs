using EntityLibrary.Controllers;
using EntityLibrary.IOC;
using LogSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EntityLibrary.Message;

namespace Smashteroids
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public sealed class Instance : Microsoft.Xna.Framework.Game
	{
		#region Private Fields

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		// Controllers
		private IEntityController _entityController;
		private IRenderableController _renderableController;
		private IAiController _aiController;
		private ICollidableController _collidableController;

		// entity messaging system
		private IPriorityMessageQueue _entityMessagingSystem;

		#endregion
		
		public Instance()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Initialize the logger system
			Logger.Initialize(this);

			// Give the IOC container a refence to the game.
			IocContainer.SetGameReference(this);

			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: Add your initialization logic here

			// Resolve controllers
			_entityController = IocContainer.Resolve<IEntityController>();
			_renderableController = IocContainer.Resolve<IRenderableController>();
			_aiController = IocContainer.Resolve<IAiController>();
			_collidableController = IocContainer.Resolve<ICollidableController>();

			// resolve messaging system
			_entityMessagingSystem = IocContainer.Resolve<IPriorityMessageQueue>();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
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
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			/* for now, we're only processing 1 message per update.
			 * if this is moved to a worker-thread, it could become intersting */
			if (!_entityMessagingSystem.IsEmpty())
			{
				foreach (var t in _entityMessagingSystem.PendingMessages())
				{
					_entityMessagingSystem.DispatchMessage(t);
				}
				//_entityMessagingSystem.DispatchMessage();
			}

			// TODO: Add your update logic here
			//_aiController.Do();
			//_collidableController.Do();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_renderableController.DrawRenderables(_spriteBatch, gameTime);

			base.Draw(gameTime);
		}


		/// <summary>
		/// Event handler for the game exiting.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected override void OnExiting(object sender, System.EventArgs args)
		{
			Logger.Write(MessageType.Information, "Shutting engine down ....", 1);
			Logger.Close();
			base.OnExiting(sender, args);
		}
	}
}
