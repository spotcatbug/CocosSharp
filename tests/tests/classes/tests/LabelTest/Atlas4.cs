using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Atlas4 : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

		CCLabelBMFont label, label2;

        public Atlas4()
        {
            m_time = 0;

            // Upper Label
			label = new CCLabelBMFont("Bitmap Font Atlas", "fonts/bitmapFontTest.fnt");
            AddChild(label);

			label.AnchorPoint = CCPoint.AnchorMiddle;


			var BChar = (CCSprite)label[0];
			var FChar = (CCSprite)label[7];
			var AChar = (CCSprite)label[12];


			var rotate = new CCRotateBy (2, 360);
			var rot_4ever = new CCRepeatForever (rotate);

			var scale = new CCScaleBy(2, 1.5f);
			var scale_back = scale.Reverse();
			var scale_seq = new CCSequence(scale, scale_back);
			var scale_4ever = new CCRepeatForever (scale_seq);

            var jump = new CCJumpBy (0.5f, new CCPoint(), 60, 1);
            var jump_4ever = new CCRepeatForever (jump);

			var fade_out = new CCFadeOut  (1);
			var fade_in = new CCFadeIn  (1);
			var seq = new CCSequence(fade_out, fade_in);
			var fade_4ever = new CCRepeatForever (seq);

            BChar.RunAction(rot_4ever);
            BChar.RunAction(scale_4ever);
            FChar.RunAction(jump_4ever);
			AChar.RunAction(fade_4ever);


            // Bottom Label
			label2 = new CCLabelBMFont("00.0", "fonts/bitmapFontTest.fnt");
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

			var lastChar = (CCSprite)label2[3];
            lastChar.RunAction(rot_4ever);

            //schedule( schedule_selector(Atlas4::step), 0.1f);
            base.Schedule(step, 0.1f);
        }

        public override void OnEnter ()
        {
            base.OnEnter ();

            var s = Layer.VisibleBoundsWorldspace.Size;

            var drawNode = new CCDrawNode();
            drawNode.DrawLine( new CCPoint(0, s.Height/2), new CCPoint(s.Width, s.Height/2), new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f) );
            drawNode.DrawLine( new CCPoint(s.Width/2, 0), new CCPoint(s.Width/2, s.Height), new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f) );
            AddChild(drawNode, -1);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			label.Position = s.Center;
			label2.Position = new CCPoint(s.Width / 2.0f, 80);
		}

        public virtual void step(float dt)
        {
            m_time += dt;

			var Stepstring = string.Format("{0,4:f1}", m_time);

			var label1 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label1.Text = (Stepstring);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Using fonts as CCSprite objects. Some characters should rotate.";
        }
    }
}
