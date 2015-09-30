using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class LabelFNTMultiLine : AtlasDemoNew
    {

		CCLabel label1, label2, label3;

        public LabelFNTMultiLine()
        {
            CCSize s;

            // Left
            label1 = new CCLabel("Multi line\nLeft", "fonts/bitmapFontTest3.fnt");
            label1.AnchorPoint = new CCPoint(0, 0);
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            s = label1.ContentSize;

            CCLog.Log("content size label1: {0,0:f2} x {1,0:f2}", s.Width, s.Height);


            // Center
            label2 = new CCLabel("Multi line\nCenter", "fonts/bitmapFontTest3.fnt");
            label2.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            s = label2.ContentSize;

            CCLog.Log("content size label2: {0,0:f2} x {1,0:f2}", s.Width, s.Height);

            // right
            label3 = new CCLabel("Multi line\nRight\nThree lines Three", "fonts/bitmapFontTest3.fnt");
            label3.AnchorPoint = new CCPoint(1, 1);
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            s = label3.ContentSize;

            CCLog.Log("content size labe3: {0,0:f2} x {1,0:f2}", s.Width, s.Height);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            label1.Position = visibleRect.LeftBottom();
            label2.Position = visibleRect.Center();
            label3.Position = visibleRect.RightTop();

		}

        public override string Title
        {
            get {
                return "New Label + .FNT file";
            }
        }

        public override string Subtitle
        {
            get {
                return "Multiline + anchor point";
            }
        }
    }
}
