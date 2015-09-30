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

using System;

namespace CocosSharp
{
    /// <summary>
    /// @brief CCFadeOutTRTiles action
    /// Fades out the tiles in a Top-Right direction
    /// </summary>
    public class CCFadeOutTRTiles : CCTiledGrid3DAction
    {
        #region Constructors

        public CCFadeOutTRTiles (float duration) : base (duration)
        {
        }

        public CCFadeOutTRTiles (float duration, CCGridSize gridSize) : base (duration, gridSize)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCFadeOutTRTilesState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCFadeOutTRTilesState : CCTiledGrid3DActionState
    {
        public CCFadeOutTRTilesState (CCFadeOutTRTiles action, CCNodeGrid target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            int i, j;
            CCGridSize newGrid;

            for (i = 0; i < GridSize.X; ++i)
            {
                newGrid.X = i;
                for (j = 0; j < GridSize.Y; ++j)
                {
                    newGrid.Y = j;
                    float distance = TestFunc (newGrid, time);
                    if (distance == 0)
                    {
                        TurnOffTile (newGrid);
                    }
                    else if (distance < 1)
                    {
                        TransformTile (newGrid, distance);
                    }
                    else
                    {
                        TurnOnTile (newGrid);
                    }
                }
            }
        }


        #region Tile transform

        public virtual float TestFunc (CCGridSize pos, float time)
        {
            float px = GridSize.X * time;
            float py = GridSize.Y * time;
            if ((px + py) == 0.0f)
            {
                return 1.0f;
            }

            return (float)Math.Pow ((pos.X + pos.Y) / (px + py), 6);
        }

        public void TurnOnTile (CCGridSize pos)
        {
            CCQuad3 orig = OriginalTile (pos);
            SetTile (pos, ref orig);
        }

        public void TurnOffTile (CCGridSize pos)
        {
            var coords = new CCQuad3 ();
            //memset(&coords, 0, sizeof(ccQuad3));
            SetTile (pos, ref coords);
        }

        public virtual void TransformTile (CCGridSize pos, float distance)
        {
            CCQuad3 coords = OriginalTile (pos);

            var step = ((CCNodeGrid)Target).Grid.Step;

            float dx = (step.X / 2) * (1.0f - distance);
            float dy = (step.Y / 2) * (1.0f - distance);

            coords.BottomLeft.X += dx;
            coords.BottomLeft.Y += dy;

            coords.BottomRight.X -= dx;
            coords.BottomRight.Y += dy;

            coords.TopLeft.X += dx;
            coords.TopLeft.Y -= dy;

            coords.TopRight.X -= dx;
            coords.TopRight.Y -= dy;

            SetTile (pos, ref coords);
        }

        #endregion Tile transform
    }

    #endregion Action state
}