﻿/****************************************************************************
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
using System.Diagnostics;

namespace CocosSharp
{
    public static class CCLog
    { 

        public delegate void LogDelegate (string value, params object[] args);
        public static LogDelegate Logger;

        static CCLog ()
        {

#if DEBUG
            // We wrap this in a DEBUG conditional as there is no sense to attach a lambda to
            // the logger if it will just be an empty invoke.  The Debug.WriteLine will conditionally
            // be compiled out when building for Release anyway.
            Logger = (format, args) =>
                {
                    Debug.WriteLine(format, args);
                };
#endif
        }

        public static void Log(string format, params object[] args)
        {
            var localLogAction = Logger;
            if (localLogAction != null)
                localLogAction.Invoke(format, args);
        }
    }
}
