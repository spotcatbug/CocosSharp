using System;

namespace CocosSharp
{
    public class CCFlipY3D : CCFlipX3D
    {
        #region Constructors

        public CCFlipY3D (float duration) : base (duration)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCFlipY3DState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCFlipY3DState : CCFlipX3DState
    {
        public CCFlipY3DState (CCFlipY3D action, CCNodeGrid target) 
            : base (action, target)
        {
        }

        public override void Update (float time)
        {

            if (Target == null)
                return; 
            
            float angle = (float)Math.PI * time; // 180 degrees
            var mz = (float)Math.Sin (angle);
            angle = angle / 2.0f; // x calculates degrees from 0 to 90
            var my = (float)Math.Cos (angle);

            CCVertex3F v0, v1, v;
            var diff = new CCVertex3F ();

            v0 = OriginalVertex (1, 1);
            v1 = OriginalVertex (0, 0);

            float y0 = v0.Y;
            float y1 = v1.Y;
            float y;
            CCGridSize a, b, c, d;

            if (y0 > y1)
            {
                // Normal Grid
                a = new CCGridSize (0, 0);
                b = new CCGridSize (0, 1);
                c = new CCGridSize (1, 0);
                d = new CCGridSize (1, 1);
                y = y0;
            }
            else
            {
                // Reversed Grid
                b = new CCGridSize (0, 0);
                a = new CCGridSize (0, 1);
                d = new CCGridSize (1, 0);
                c = new CCGridSize (1, 1);
                y = y1;
            }

            diff.Y = y - y * my;
            diff.Z = Math.Abs ((float)Math.Floor ((y * mz) / 4.0f));

            // bottom-left
            v = OriginalVertex (a);
            v.Y = diff.Y;
            v.Z += diff.Z;
            SetVertex (a, ref v);

            // upper-left
            v = OriginalVertex (b);
            v.Y -= diff.Y;
            v.Z -= diff.Z;
            SetVertex (b, ref v);

            // bottom-right
            v = OriginalVertex (c);
            v.Y = diff.Y;
            v.Z += diff.Z;
            SetVertex (c, ref v);

            // upper-right
            v = OriginalVertex (d);
            v.Y -= diff.Y;
            v.Z -= diff.Z;
            SetVertex (d, ref v);
        }
    }

    #endregion Action state
}