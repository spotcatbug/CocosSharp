using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class Bug624Layer : BugsTestBaseLayer
    {
        public Bug624Layer() 
        {
            InitBug624Layer();
        }

        private void InitBug624Layer()
        {
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabel("Layer1", "MarkerFelt", 22, CCLabelFormat.SpriteFont);

            label.Position = new CCPoint(size.Width / 2, size.Height / 2);
            AddChild(label);
            Schedule(switchLayer, 5.0f);

        }

        public void switchLayer(float dt)
        {
            //unschedule(Bug624Layer.switchLayer);

            CCScene scene = new CCScene(Scene);
            scene.AddChild(new Bug624Layer2(), 0);
            Scene.Director.ReplaceScene(new CCTransitionFade(2.0f, scene, new CCColor3B { R = 255, G = 255, B = 255 }));
        }

        public virtual void didAccelerate(CCAcceleration pAccelerationValue)
        {
            CCLog.Log("Layer1 accel");
        }

        //LAYER_NODE_FUNC(Bug624Layer);
    }

    public class Bug624Layer2 : BugsTestBaseLayer
    {

        public Bug624Layer2()
        {
            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabel("Layer2", "MarkerFelt", 36, CCLabelFormat.SpriteFont);

            label.Position = new CCPoint(size.Width / 2, size.Height / 2);
            AddChild(label);
            Schedule(switchLayer, 5.0f);

        }

        public void switchLayer(float dt)
        {
            //unschedule(schedule_selector(Bug624Layer::switchLayer));

            CCScene scene = new CCScene(Scene);
            scene.AddChild(new Bug624Layer(), 0);
            Scene.Director.ReplaceScene(new CCTransitionFade(2.0f, scene, new CCColor3B { R = 255, G = 0, B = 0 }));
        }

        public virtual void didAccelerate(CCAcceleration pAccelerationValue)
        {
            CCLog.Log("Layer2 accel");
        }

        //LAYER_NODE_FUNC(Bug624Layer2);
    }
}
