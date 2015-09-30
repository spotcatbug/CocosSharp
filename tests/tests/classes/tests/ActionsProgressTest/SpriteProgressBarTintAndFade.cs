using CocosSharp;

namespace tests
{
    internal class SpriteProgressBarTintAndFade : SpriteDemo
    {
        private string s_pPathSister1 = "Images/grossinis_sister1";
        private string s_pPathSister2 = "Images/grossinis_sister2";

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

			var progressTo = new CCProgressTo(6, 100);
			var tint = new CCSequence(new CCTintTo (1, 255, 0, 0),
                                              new CCTintTo (1, 0, 255, 0),
                                              new CCTintTo (1, 0, 0, 255));
			var fade = new CCSequence(new CCFadeTo (1.0f, 0),
                                              new CCFadeTo (1.0f, 255));

            CCProgressTimer left = new CCProgressTimer(new CCSprite(s_pPathSister1));
            left.Type = CCProgressTimerType.Bar;

            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            left.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            left.BarChangeRate = new CCPoint(1, 0);
            AddChild(left);
            left.Position = new CCPoint(100, s.Height / 2);
			left.RepeatForever(progressTo);
			left.RepeatForever(tint);

            left.AddChild(new CCLabel("Tint", "arial", 20.0f, CCLabelFormat.SpriteFont));

            CCProgressTimer middle = new CCProgressTimer(new CCSprite(s_pPathSister2));
            middle.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            middle.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            middle.BarChangeRate = new CCPoint(1, 1);
            AddChild(middle);
            middle.Position = new CCPoint(s.Width / 2, s.Height / 2);
			middle.RepeatForever(progressTo);
			middle.RepeatForever(fade);

            middle.AddChild(new CCLabel("Fade", "arial", 20.0f, CCLabelFormat.SpriteFont));

            CCProgressTimer right = new CCProgressTimer(new CCSprite(s_pPathSister2));
            right.Type = CCProgressTimerType.Bar;
            //    Setup for a bar starting from the bottom since the midpoint is 0 for the y
            right.Midpoint = new CCPoint(0.5f, 0.5f);
            //    Setup for a vertical bar since the bar change rate is 0 for x meaning no horizontal change
            right.BarChangeRate = new CCPoint(0, 1);
            AddChild(right);
            right.Position = new CCPoint(s.Width - 100, s.Height / 2);
			right.RepeatForever(progressTo);
			right.RepeatForever(tint);
			right.RepeatForever(fade);

            right.AddChild(new CCLabel("Tint and Fade", "arial", 20.0f, CCLabelFormat.SpriteFont));
        }

		public override string Subtitle
		{
			get
			{
				return "ProgressTo Tint And Fade";
			}
		}
    }
}