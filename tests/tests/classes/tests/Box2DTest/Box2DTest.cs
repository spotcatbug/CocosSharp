using System;
using System.Collections.Generic;
using Box2D;
using Box2D.Collision;
using Box2D.Common;
using Box2D.Collision.Shapes;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    internal class CCPhysicsSprite : CCSprite
    {
        public CCPhysicsSprite(CCTexture2D f, CCRect r)
            : base(f, r)
        {
        }

        public b2Body PhysicsBody { get; set; }

        public void UpdateSprite()
        {
            if(PhysicsBody == null)
                return;

            b2Vec2 pos = PhysicsBody.Position;

            float x = pos.x * Box2DTestLayer.PTM_RATIO;
            float y = pos.y * Box2DTestLayer.PTM_RATIO;

            Position = new CCPoint(x, y);
            Rotation = -CCMacros.CCRadiansToDegrees(PhysicsBody.Angle);
        }
    }

    public class Box2DTestLayer : CCLayer
    {
        public class Myb2Listener : b2ContactListener
        {

            public override void PreSolve(b2Contact contact, b2Manifold oldManifold)
            {
            }

            public override void PostSolve(Box2D.Dynamics.Contacts.b2Contact contact, ref b2ContactImpulse impulse)
            {
            }
        }

        public const int PTM_RATIO = 32;

        private const int kTagParentNode = 1;
        private readonly CCTexture2D spriteTexture;
        private b2World _world;

        public Box2DTestLayer()
        {
            spriteTexture = new CCTexture2D("Images/blocks");
		}

		public override void OnEnter()
		{
			base.OnEnter();

			var listener = new CCEventListenerTouchAllAtOnce();
			listener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(listener);    

			CCSize s = Layer.VisibleBoundsWorldspace.Size;
			// init physics
			initPhysics();
			// create reset button
			createResetButton();

			addNewSpriteAtPosition(new CCPoint(s.Width / 2, s.Height / 2));

            var label = new CCLabel("Tap screen", "MarkerFelt", 32, CCLabelFormat.SpriteFont);
			AddChild(label, 0);
			label.Color = new CCColor3B(0, 0, 255);
			label.Position = new CCPoint(s.Width / 2, s.Height - 50);

			Schedule ();
		}

        private void initPhysics()
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            var gravity = new b2Vec2(0.0f, -10.0f);
            _world = new b2World(gravity);
            float debugWidth = s.Width / PTM_RATIO * 2f;
            float debugHeight = s.Height / PTM_RATIO * 2f;

            //CCBox2dDraw debugDraw = new CCBox2dDraw(new b2Vec2(debugWidth / 2f + 10, s.Height - debugHeight - 10), 2);
            //debugDraw.AppendFlags(b2DrawFlags.e_shapeBit);
            
            //_world.SetDebugDraw(debugDraw);
            _world.SetAllowSleeping(true);
            _world.SetContinuousPhysics(true);

            //m_debugDraw = new GLESDebugDraw( PTM_RATIO );
            //world->SetDebugDraw(m_debugDraw);

            //uint32 flags = 0;
            //flags += b2Draw::e_shapeBit;
            //        flags += b2Draw::e_jointBit;
            //        flags += b2Draw::e_aabbBit;
            //        flags += b2Draw::e_pairBit;
            //        flags += b2Draw::e_centerOfMassBit;
            //m_debugDraw->SetFlags(flags);


            // Call the body factory which allocates memory for the ground body
            // from a pool and creates the ground box shape (also from a pool).
            // The body is also added to the world.
            b2BodyDef def = new b2BodyDef();
            def.allowSleep = true;
            def.position = b2Vec2.Zero;
            def.type = b2BodyType.b2_staticBody;
            b2Body groundBody = _world.CreateBody(def);
            groundBody.SetActive(true);

            // Define the ground box shape.

            // bottom
            b2EdgeShape groundBox = new b2EdgeShape();
            groundBox.Set(b2Vec2.Zero, new b2Vec2(s.Width / PTM_RATIO, 0));
            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // top
            groundBox = new b2EdgeShape();
            groundBox.Set(new b2Vec2(0, s.Height / PTM_RATIO), new b2Vec2(s.Width / PTM_RATIO, s.Height / PTM_RATIO));
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // left
            groundBox = new b2EdgeShape();
            groundBox.Set(new b2Vec2(0, s.Height / PTM_RATIO), b2Vec2.Zero);
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // right
            groundBox = new b2EdgeShape();
            groundBox.Set(new b2Vec2(s.Width / PTM_RATIO, s.Height / PTM_RATIO), new b2Vec2(s.Width / PTM_RATIO, 0));
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // _world.Dump();
        }

        public void createResetButton()
        {
            CCMenuItemImage res = new CCMenuItemImage("Images/r1", "Images/r2", reset);

            CCMenu menu = new CCMenu(res);

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            menu.Position = new CCPoint(s.Width / 2, 30);
            AddChild(menu, -1);
        }

        public void reset(object sender)
        {
            CCScene s = new Box2DTestScene();
            var child = new Box2DTestLayer();
            s.AddChild(child);
            Director.ReplaceScene(s);
        }

        /*
        public override void Draw()
        {
            //
            // IMPORTANT:
            // This is only for debug purposes
            // It is recommend to disable it
            //
            base.Draw();

            //ccGLEnableVertexAttribs( kCCVertexAttribFlag_Position );

            //kmGLPushMatrix();

            CCDrawingPrimitives.Begin();
            _world.DrawDebugData();
            CCDrawingPrimitives.End();

            //world.DrawDebugData();

            //kmGLPopMatrix();
        }
        */

        private const int kTagForPhysicsSprite = 99999;

        public void addNewSpriteAtPosition(CCPoint p)
        {
            //CCLog.Log("Add sprite #{2} : {0} x {1}", p.X, p.Y, _batch.ChildrenCount + 1);

            //We have a 64x64 sprite sheet with 4 different 32x32 images.  The following code is
            //just randomly picking one of the images
            int idx = (CCRandom.Float_0_1() > .5 ? 0 : 1);
            int idy = (CCRandom.Float_0_1() > .5 ? 0 : 1);
            var sprite = new CCPhysicsSprite(spriteTexture, new CCRect(32 * idx, 32 * idy, 32, 32));

            AddChild(sprite, 0, kTagForPhysicsSprite);

            sprite.Position = new CCPoint(p.X, p.Y);

            // Define the dynamic body.
            //Set up a 1m squared box in the physics world
            b2BodyDef def = new b2BodyDef();
            def.position = new b2Vec2(p.X / PTM_RATIO, p.Y / PTM_RATIO);
            def.type = b2BodyType.b2_dynamicBody;
            b2Body body = _world.CreateBody(def);
            // Define another box shape for our dynamic body.
            var dynamicBox = new b2PolygonShape();
            dynamicBox.SetAsBox(.5f, .5f); //These are mid points for our 1m box

            // Define the dynamic body fixture.
            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = dynamicBox;
            fd.density = 1f;
            fd.friction = 0.3f;
            b2Fixture fixture = body.CreateFixture(fd);

            sprite.PhysicsBody = body;
            //_world.SetContactListener(new Myb2Listener());

            // _world.Dump();
        }

        public override void Update(float dt)
        {
            _world.Step(dt, 8, 1);

            foreach (CCNode node in Children)
            {
                var sprite = node as CCPhysicsSprite;

                if(sprite == null)
                    continue;

                if (sprite.Visible && sprite.PhysicsBody.Position.y < 0f) {
                    _world.DestroyBody (sprite.PhysicsBody);
                    sprite.Visible = false;
                } else
                    sprite.UpdateSprite();
            }

//#if WINDOWS || WINDOWSGL || LINUX || MACOS
//
			// This needs replacing with EventDispatcher
//			CCInputState.Instance.Update(dt);
//            PlayerIndex p;
//            if (CCInputState.Instance.IsKeyPress(Microsoft.Xna.Framework.Input.Keys.D, PlayerIndex.One, out p))
//            {
//                _world.Dump();
//#if PROFILING
//                b2Profile profile = _world.Profile;
//                CCLog.Log("]-----------[{0:F4}]-----------------------[", profile.step);
//                CCLog.Log("Solve Time = {0:F4}", profile.solve);
//                CCLog.Log("# bodies = {0}", profile.bodyCount);
//                CCLog.Log("# contacts = {0}", profile.contactCount);
//                CCLog.Log("# joints = {0}", profile.jointCount);
//                CCLog.Log("# toi iters = {0}", profile.toiSolverIterations);
//                if (profile.step > 0f)
//                {
//                    CCLog.Log("Solve TOI Time = {0:F4} {1:F2}%", profile.solveTOI, profile.solveTOI / profile.step * 100f);
//                    CCLog.Log("Solve TOI Advance Time = {0:F4} {1:F2}%", profile.solveTOIAdvance, profile.solveTOIAdvance / profile.step * 100f);
//                }
//
//                CCLog.Log("BroadPhase Time = {0:F4}", profile.broadphase);
//                CCLog.Log("Collision Time = {0:F4}", profile.collide);
//                CCLog.Log("Solve Velocity Time = {0:F4}", profile.solveVelocity);
//                CCLog.Log("Solve Position Time = {0:F4}", profile.solvePosition);
//                CCLog.Log("Step Time = {0:F4}", profile.step);
//#endif
//            }
//#endif
        }

		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            //Add a new body/atlas sprite at the touched location
            foreach (CCTouch touch in touches)
            {
                CCPoint location = Layer.ScreenToWorldspace(touch.LocationOnScreen);
                addNewSpriteAtPosition(location);
            }
        }
    }

    internal class Box2DTestScene : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
            CCLayer pLayer = new Box2DTestLayer();
            AddChild(pLayer);

            Director.ReplaceScene(this);
        }
    }
}