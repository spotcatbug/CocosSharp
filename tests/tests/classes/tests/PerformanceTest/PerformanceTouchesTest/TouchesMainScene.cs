using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TouchesMainScene : PerformBasicLayer
    {

        public TouchesMainScene(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            CCLayer pLayer = null;
            switch (m_nCurCase)
            {
                case 0:
                    pLayer = new TouchesPerformTest1(true, PerformanceTouchesTest.TEST_COUNT, m_nCurCase);
                    break;
                case 1:
                    pLayer = new TouchesPerformTest2(true, PerformanceTouchesTest.TEST_COUNT, m_nCurCase);
                    break;
            }
            PerformanceTouchesTest.s_nTouchCurCase = m_nCurCase;

            if (pLayer != null)
            {
                CCScene pScene = new CCScene(Scene);
                pScene.AddChild(pLayer);

                Scene.Director.ReplaceScene(pScene);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            // add title
            var label = new CCLabel(title(), "arial", 32, CCLabelFormat.SpriteFont);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

			Schedule ();

            m_plabel = new CCLabel("00.0", "fonts/arial16.fnt");
            m_plabel.Position = new CCPoint(s.Width / 2, s.Height / 2);
            AddChild(m_plabel);

            elapsedTime = 0;
            numberOfTouchesB = numberOfTouchesM = numberOfTouchesE = numberOfTouchesC = 0;
        }

        public virtual string title()
        {
            return "No title";
        }

        public override void Update(float dt)
        {
            elapsedTime += dt;

            if (elapsedTime > 1.0f)
            {
                float frameRateB = numberOfTouchesB / elapsedTime;
                float frameRateM = numberOfTouchesM / elapsedTime;
                float frameRateE = numberOfTouchesE / elapsedTime;
                float frameRateC = numberOfTouchesC / elapsedTime;
                elapsedTime = 0;
                numberOfTouchesB = numberOfTouchesM = numberOfTouchesE = numberOfTouchesC = 0;

                //char str[32] = {0};
                string str;
				str = string.Format("{0:f1},{1:f1},{2:f1},{3:f1}", frameRateB, frameRateM, frameRateE, frameRateC);
                m_plabel.Text = (str);
            }
        }

        protected CCLabel m_plabel;
        protected int numberOfTouchesB;
        protected int numberOfTouchesM;
        protected int numberOfTouchesE;
        protected int numberOfTouchesC;
        protected float elapsedTime;
    }
}
