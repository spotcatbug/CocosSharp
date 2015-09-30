using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerTest : TestNavigationLayer
    {

        public LayerTest()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

        }

		public override string Title
		{
			get
			{
				return "Layer Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return string.Empty;
			}
		}

		public override void RestartCallback(object sender)
		{
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.restartTestAction());
            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.nextTestAction());

            Director.ReplaceScene(s);
        }

		public override void BackCallback(object sender)
		{
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.backTestAction());

            Director.ReplaceScene(s);
        }

        protected static void SetEnableRecursiveCascading(CCNode node, bool enable)
        {
            node.IsColorCascaded = true;
            node.IsOpacityCascaded = enable;

            if (node.ChildrenCount > 0)
            {
                foreach (var children in node.Children)
                {
                    SetEnableRecursiveCascading(children, enable);
                }
            }
        }
    }

    public class LayerMultiplexTest : LayerTest
    {
        CCLayerMultiplex child;

        public LayerMultiplexTest()
        {
            List<CCLayer> layers = new List<CCLayer>();
            for (int i = 0; i < 3; i++)
            {
				CCLayer l = new CCLayerColor(new CCColor4B(0,255,0));
                CCSprite img = null;
                switch (i)
                {
                    case 0:
                        img = new CCSprite("Images/grossini");
                        break;
                    case 1:
                        img = new CCSprite("Images/grossinis_sister1");
                        break;
                    case 2:
                        img = new CCSprite("Images/grossinis_sister2");
                        break;
                }
                img.AnchorPoint = CCPoint.Zero;
                img.Position = CCPoint.Zero;
				l.ContentSize = img.ContentSize;
                l.AddChild(img);
                l.Position = new CCPoint(128f, 128f);
                layers.Add(l);
            }
            child = new CCLayerMultiplex(layers.ToArray());
            child.InAction = new CCFadeIn(1);
            AddChild(child);
			Schedule(new Action<float>(AutoMultiplex), 3f);
        }

		public override string Title
		{
			get
			{
				return "Layer Multiplex Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Three layers switch every 3 seconds";
			}
		}

        private Random rand = new Random();
		private int lastRandom = 0;

        private void AutoMultiplex(float dt)
        {
			//CCLog.Log ("Switched");
			int newRand = -1;
			// make sure we always change to a new one
			do {
				newRand = rand.Next (3);
			} while (lastRandom == newRand);
			lastRandom = newRand;

			child.SwitchTo(newRand);
        }
    }
}

