/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org


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
    public static class CCMacros
    {
        /// <summary>
        /// simple macro that swaps 2 variables
        /// </summary>
        public static void CCSwap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }

        private static readonly System.Random rand = new System.Random();

        /// <summary>
        /// returns a random float between -1 and 1
        /// </summary>
        public static float CCRandomBetweenNegative1And1()
        {
            return (2.0f * ((float) rand.Next() / int.MaxValue)) - 1.0f;
        }

        /** @def CCRANDOM_0_1
            returns a random float between 0 and 1
         */

        public static float CCRandomBetween0And1()
        {
            return (float) rand.Next() / int.MaxValue;
        }

        /** @def CC_DEGREES_TO_RADIANS
            converts degrees to radians
        */

        public static float CCDegreesToRadians(float angle)
        {
            return angle * 0.01745329252f; // PI / 180
        }

        /** @def CC_RADIANS_TO_DEGREES
            converts radians to degrees
        */

        public static float CCRadiansToDegrees(float angle)
        {
            return angle * 57.29577951f; // PI * 180
        }

        internal static Microsoft.Xna.Framework.Color ToColor (this CCColor4B color)
        {
            return new Microsoft.Xna.Framework.Color (color.R, color.G, color.B, color.A);
        }
        internal static Microsoft.Xna.Framework.Color ToColor (this CCColor4F color)
        {
            return new Microsoft.Xna.Framework.Color (color.R, color.G, color.B, color.A);
        }

        internal static Microsoft.Xna.Framework.Vector3 ToVector3 (this CCPoint point)
        {
            return new Microsoft.Xna.Framework.Vector3(point.X, point.Y, 0);
        }

        internal static Microsoft.Xna.Framework.Vector2 ToVector2 (this CCVector2 point)
        {
            return new Microsoft.Xna.Framework.Vector2(point.X, point.Y);
        }

        internal static CCVector2 ToCCVector2 (this Microsoft.Xna.Framework.Vector2 point)
        {
            return new CCVector2(point.X, point.Y);
        }



        /*
         * Macros defined in ccConfig.h
         */
        internal static readonly float CCDirectorStatsUpdateIntervalInSeconds = 0.5f;

        /*
         * Macros defined in CCSprite.h
         */
        internal static readonly int CCSpriteIndexNotInitialized = -1; // 0xffffffff; // CCSprite invalid index on the CCSpriteBatchode
    }
}