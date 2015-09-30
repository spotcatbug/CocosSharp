/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
Copyright (c) 2011-2012 openxlive.com
 
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

namespace CocosSharp
{
    public class CCSplitCols : CCTiledGrid3DAction
    {
        protected internal int Columns { get; private set; }


        #region Constructors

        /// <summary>
        /// creates the action with the number of columns to split and the duration
        /// </summary>
        public CCSplitCols (float duration, int nCols) : base (duration, new CCGridSize (nCols, 1))
        {
            Columns = nCols;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCSplitColsState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCSplitColsState : CCTiledGrid3DActionState
    {
        protected CCSize VisibleSize { get; private set; }

        // We only need the height
        private float height = 0;

        public CCSplitColsState (CCSplitCols action, CCNodeGrid target) : base (action, target)
        {
            VisibleSize = Target.VisibleBoundsWorldspace.Size;
            height = VisibleSize.Height;
        }

        public override void Update (float time)
        {

            // We may have started the action before the Visible Size was able
            // to be set from the Target so we will try it again
            if (height == 0)
            {
                VisibleSize = Target.VisibleBoundsWorldspace.Size;
                height = VisibleSize.Height;
            }

            int i;

            for (i = 0; i < GridSize.X; ++i)
            {
                CCQuad3 coords = OriginalTile (i, 0);
                float direction = 1;

                if ((i % 2) == 0)
                {
                    direction = -1;
                }

                coords.BottomLeft.Y += direction * height * time;
                coords.BottomRight.Y += direction * height * time;
                coords.TopLeft.Y += direction * height * time;
                coords.TopRight.Y += direction * height * time;

                SetTile (i, 0, ref coords);
            }
        }

    }

    #endregion Action state
}