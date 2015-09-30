using System;


namespace CocosSharp
{
    public class CCLens3D : CCGrid3DAction
    {
        #region Properties

        public bool Concave { get; private set; }
        public float Radius { get; private set; }
        public float LensScale { get; private set; }
        public CCPoint Position { get; private set; }

        #endregion Properties


        #region Constructors

        public CCLens3D (float duration, CCGridSize gridSize)
            : this (duration, gridSize, CCPoint.Zero, 0)
        {
        }

        public CCLens3D (float duration, CCGridSize gridSize, CCPoint position, float radius) : base (duration, gridSize)
        {
            Position = position;
            Radius = radius;
            LensScale = 0.7f;
            Concave = false;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction (CCNode target)
        {
            return new CCLens3DState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCLens3DState : CCGrid3DActionState
    {
        public CCPoint Position { get; set; }

        public float Radius { get; set; }

        public float LensScale { get; set; }

        public bool Concave { get; set; }

        public CCLens3DState (CCLens3D action, CCNodeGrid target) : base (action, target)
        {
            Position = action.Position;
            Radius = action.Radius;
            LensScale = action.LensScale;
            Concave = action.Concave;
        }

        public override void Update (float time)
        {
            if (Target == null)
                return;
            
            int i, j;

            CCPoint vect = CCPoint.Zero;

            for (i = 0; i < GridSize.X + 1; ++i)
            {
                for (j = 0; j < GridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex (i, j);
                    vect = Position - new CCPoint (v.X, v.Y);

                    float r = vect.Length;
                    float radius = Radius;

                    if (r < radius)
                    {
                        r = radius - r;
                        float pre_log = r / radius;
                        if (pre_log == 0)
                        {
                            pre_log = 0.001f;
                        }

                        float lensEffect = LensScale;
                        float l = (float)Math.Log (pre_log) * lensEffect;
                        float new_r = (float)Math.Exp (l) * radius;

                        if (Math.Sqrt ((vect.X * vect.X + vect.Y * vect.Y)) > 0)
                        {
                            vect = CCPoint.Normalize (vect);

                            CCPoint new_vect = vect * new_r;
                            v.Z += (Concave ? -1.0f : 1.0f) * new_vect.Length * lensEffect;
                        }
                    }

                    SetVertex (i, j, ref v);
                }
            }
        }
    }

    #endregion Action state
}