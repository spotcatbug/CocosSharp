using System;
using System.IO;
using System.Diagnostics;

namespace CocosSharp
{

    public enum CCSceneResolutionPolicy
    {
        // The Viewport is not automatically calculated and it is up to the developer to take care of setting
        // the values correctly.
        Custom,

        // The entire application is visible in the specified area without trying to preserve the original aspect ratio. 
        // Distortion can occur, and the application may appear stretched or compressed.
        ExactFit,
        // The entire application fills the specified area, without distortion but possibly with some cropping, 
        // while maintaining the original aspect ratio of the application.
        NoBorder,
        // The entire application is visible in the specified area without distortion while maintaining the original 
        // aspect ratio of the application. Borders can appear on two sides of the application.
        ShowAll,
        // The application takes the height of the design resolution size and modifies the width of the internal
        // canvas so that it fits the aspect ratio of the device
        // no distortion will occur however you must make sure your application works on different
        // aspect ratios
        FixedHeight,
        // The application takes the width of the design resolution size and modifies the height of the internal
        // canvas so that it fits the aspect ratio of the device
        // no distortion will occur however you must make sure your application works on different
        // aspect ratios
        FixedWidth
    }



    /// <summary>
    /// brief CCScene is a subclass of CCNode that is used only as an abstract concept.
    /// CCScene and CCNode are almost identical with the difference that CCScene has it's
    /// anchor point (by default) at the center of the screen. Scenes have state management
    /// where they can serialize their state and have it reconstructed upon resurrection.
    ///  It is a good practice to use and CCScene as the parent of all your nodes.
    /// </summary>
    public class CCScene : CCNode
    {
        static readonly CCRect exactFitRatio = new CCRect(0,0,1,1);

        CCSceneResolutionPolicy resolutionPolicy = CCSceneResolutionPolicy.ExactFit;
        CCViewport viewport;
        CCWindow window;

        // A delegate type for hooking up SceneViewport change notifications.
        internal delegate void SceneViewportChangedEventHandler(object sender, EventArgs e);
        internal event SceneViewportChangedEventHandler SceneViewportChanged;


        #region Properties

        // Static properties

        public static CCSceneResolutionPolicy DefaultDesignResolutionPolicy { get; set; }
        public static CCSize DefaultDesignResolutionSize { get; set; }


        // Instance properties

        public CCSceneResolutionPolicy SceneResolutionPolicy 
        { 
            get { return resolutionPolicy; }
            set 
            { 
                if (value != resolutionPolicy)
                {
                    resolutionPolicy = value;
                    UpdateResolutionRatios();
                    Viewport.UpdateViewport();
                }
            }
        }

        public virtual bool IsTransition
        {
            get { return false; }
        }

#if USE_PHYSICS
		private CCPhysicsWorld _physicsWorld;

		public CCPhysicsWorld PhysicsWorld
		{
			get { return _physicsWorld; }
			set { _physicsWorld = value; }
		}
#endif

        public CCSize DesignResolutionSize { get; private set; }

        public override CCSize ContentSize
        {
            get { return DesignResolutionSize; }
            set {}
        }

        public CCRect VisibleBoundsScreenspace
        {
            get { return Viewport.ViewportInPixels; }
        }

        public override CCScene Scene
        {
            get { return this; }
        }

        public override CCWindow Window 
        { 
            get { return window; }
            set 
            {
                if(window != value) 
                {
                    window = value;
                    viewport.LandscapeScreenSizeInPixels = Window.LandscapeWindowSizeInPixels;
                }
                if (window != null)
                {
                    InitializeLazySceneGraph(Children);
                }
            }
        }

        void InitializeLazySceneGraph(CCRawList<CCNode> children)
        {
            if (children == null)
                return;

            foreach (var child in children)
            {
                if (child != null)
                {
                    child.AttachEvents();
                    child.AttachActions();
                    child.AttachSchedules ();
                    InitializeLazySceneGraph(child.Children);
                }
            }
        }

        public override CCDirector Director { get; set; }

        public override CCLayer Layer
        {
            get
            {
                return null;
            }

            internal set
            {
            }
        }

        public override CCCamera Camera 
        { 
            get { return null; }
            set 
            {
            }
        }

        public override CCViewport Viewport 
        {
            get { return viewport; }
            set 
            {
                if (viewport != value) 
                {
                    // Stop listening to previous viewport's event
                    if(viewport != null)
                        viewport.OnViewportChanged -= OnViewportChanged;

                    viewport = value;

                    viewport.OnViewportChanged += OnViewportChanged;

                    OnViewportChanged(this, null);
                }
            }
        }

        internal override CCEventDispatcher EventDispatcher 
        { 
            get { return Window != null ? Window.EventDispatcher : null; }
        }

        public override CCAffineTransform AffineLocalTransform
        {
            get
            {
                return CCAffineTransform.Identity;
            }
        }

        #endregion Properties


        #region Constructors

        static CCScene()
        {
            DefaultDesignResolutionPolicy = CCSceneResolutionPolicy.ShowAll;
        }

#if USE_PHYSICS
		public CCScene(CCWindow window, CCViewport viewport, CCDirector director = null, bool physics = false)
#else
		public CCScene(CCWindow window, CCViewport viewport, CCDirector director = null)
#endif
        {
            IgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Viewport = viewport;
            Window = window;
            Director = (director == null) ? window.DefaultDirector : director;

            if (window != null && director != null)
                window.AddSceneDirector(director);

#if USE_PHYSICS
			_physicsWorld = physics ? new CCPhysicsWorld(this) : null;
#endif
            resolutionPolicy = DefaultDesignResolutionPolicy;
            DesignResolutionSize = DefaultDesignResolutionSize;

            UpdateResolutionRatios();
        }

#if USE_PHYSICS
		public CCScene(CCWindow window, CCDirector director, bool physics = false)
			: this(window, new CCViewport(new CCRect(0.0f, 0.0f, 1.0f, 1.0f)), director, physics)
#else
        public CCScene(CCWindow window, CCDirector director)
            : this(window, 
                new CCViewport(new CCRect(0.0f, 0.0f, 1.0f, 1.0f), window.SupportedDisplayOrientations, window.CurrentDisplayOrientation), 
                director)
#endif
        {
        }

#if USE_PHYSICS
		public CCScene(CCWindow window, bool physics = false)
			: this(window, window.DefaultDirector, physics)
#else
		public CCScene(CCWindow window)
			: this(window, window.DefaultDirector)
#endif
        {
        }

#if USE_PHYSICS
		public CCScene(CCScene scene, bool physics = false)
			: this(scene.Window, scene.Viewport, scene.Director, physics)
#else
		public CCScene(CCScene scene)
			: this(scene.Window, scene.Viewport, scene.Director)
#endif
        {
        }

        #endregion Constructors


        #region Viewport handling

        void OnViewportChanged(object sender, EventArgs e)
        {
            CCViewport viewport = sender as CCViewport;

            if(viewport != null && viewport == Viewport) 
            {
                UpdateResolutionRatios();
                if (SceneViewportChanged != null)
                    SceneViewportChanged(this, null);
            }
        }

        #endregion Viewport handling


        #region Resolution Policy

        public static void SetDefaultDesignResolution(float width, float height, CCSceneResolutionPolicy resPolicy)
        {
            CCScene.DefaultDesignResolutionSize = new CCSize (width, height);
            CCScene.DefaultDesignResolutionPolicy = resPolicy;
        }

        void UpdateResolutionRatios()
        {
            if (Window != null && SceneResolutionPolicy != CCSceneResolutionPolicy.Custom)
            {
                bool dirtyViewport = false;
                CCSize designSize = DesignResolutionSize;
                CCRect designBounds = new CCRect(0.0f, 0.0f, designSize.Width, designSize.Height);

                if (designBounds != CCRect.Zero)
                {
                    // This is specific to the current orientation because WindowSize will change depending
                    // on whether it's landscape or portrait
                    var viewportRect = CalculateResolutionRatio(designBounds, resolutionPolicy);
                    dirtyViewport = Viewport.exactFitLandscapeRatio != viewportRect;

                    // Will create the correct ratio for the given orientation
                    // So set both landscape and portrait ratios to be the same
                    // Once the orientation changes, the rect will be updated
                    Viewport.exactFitLandscapeRatio = viewportRect;
                    Viewport.exactFitPortraitRatio = viewportRect;
                }

                if (dirtyViewport)
                    Viewport.UpdateViewport(false);
            }
        }


        CCRect CalculateResolutionRatio(CCRect resolutionRect, CCSceneResolutionPolicy resolutionPolicy)
        {

            var width = resolutionRect.Size.Width;
            var height = resolutionRect.Size.Height;

            if (width == 0.0f || height == 0.0f)
            {
                return exactFitRatio;
            }

            var designResolutionSize = resolutionRect.Size;
            var viewPortRect = CCRect.Zero;
            float resolutionScaleX, resolutionScaleY;

            var screenSize = Scene.Window.WindowSizeInPixels;

            resolutionScaleX = screenSize.Width / designResolutionSize.Width;
            resolutionScaleY = screenSize.Height / designResolutionSize.Height;

            if (resolutionPolicy == CCSceneResolutionPolicy.NoBorder)
            {
                resolutionScaleX = resolutionScaleY = Math.Max(resolutionScaleX, resolutionScaleY);
            }

            else if (resolutionPolicy == CCSceneResolutionPolicy.ShowAll)
            {
                resolutionScaleX = resolutionScaleY = Math.Min(resolutionScaleX, resolutionScaleY);
            }

            else if (resolutionPolicy == CCSceneResolutionPolicy.FixedHeight)
            {
                resolutionScaleX = resolutionScaleY;
                designResolutionSize.Width = (float)Math.Ceiling(screenSize.Width / resolutionScaleX);
            }

            else if (resolutionPolicy == CCSceneResolutionPolicy.FixedWidth)
            {
                resolutionScaleY = resolutionScaleX;
                designResolutionSize.Height = (float)Math.Ceiling(screenSize.Height / resolutionScaleY);
            }

            // calculate the rect of viewport    
            float viewPortW = designResolutionSize.Width * resolutionScaleX;
            float viewPortH = designResolutionSize.Height * resolutionScaleY;

            viewPortRect 
                = new CCRect((screenSize.Width - viewPortW) / 2, (screenSize.Height - viewPortH) / 2, viewPortW, viewPortH);

            var viewportRatio = new CCRect(
                ((viewPortRect.Origin.X) / screenSize.Width),
                ((viewPortRect.Origin.Y) / screenSize.Height),
                ((viewPortRect.Size.Width) / screenSize.Width),
                ((viewPortRect.Size.Height) / screenSize.Height)
            );

            DesignResolutionSize = designResolutionSize;

            return viewportRatio;

        }

        #endregion

		#region Physics


#if USE_PHYSICS

		public override void AddChild(CCNode child, int zOrder, int tag)
		{
			base.AddChild(child, zOrder, tag);
			AddChildToPhysicsWorld(child);
		}

		public override void Update(float dt)
		{
			base.Update(dt);

			if (_physicsWorld != null)
				_physicsWorld.Update(dt);
		}


		protected internal virtual void AddChildToPhysicsWorld(CCNode child)
		{
			if (_physicsWorld != null)
			{
				Action<CCNode> addToPhysicsWorldFunc = null;

				addToPhysicsWorldFunc = new Action<CCNode>(node =>
				{
					if (node.PhysicsBody != null)
					{
						_physicsWorld.AddBody(node.PhysicsBody);
					}

					var children = node.Children;
					if (children != null)
						foreach (var n in children)
						{
							addToPhysicsWorldFunc(n);
						}

				});

				addToPhysicsWorldFunc(child);

			}
		}

#endif
		
		#endregion

        public override void Visit()
        {
            CCDrawManager drawManager = Window.DrawManager;

            if(drawManager.CurrentRenderTarget == null)
                drawManager.Viewport = Viewport.XnaViewport; 

            base.Visit();
        }
    }
}