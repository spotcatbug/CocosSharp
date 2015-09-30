# CocosSharp v1.6.0.0 release notes 
## Key new features
 ---
### Windows 8.1 and Visual Studio 2015 support

With the release of Windows 10 and Visual Studio 2015 we have added support for Windows 8.1 CocosSharp projects.  Visual Studio 2015 no longer opens Windows 8.0 projects giving the opportunity to migrate to Windows 8.1.

### New project templates for Windows 8.1 that will load in Visual Studio 2015

Upgrade the templates to the latest release.

### Text Field input

A new class was added to support input for the supported platforms.  You can now use `CCTextField` to enable keyboard input.

To enable this you will create an instance of `CCTextField` and add it as a child to your Scene Graph.

                var textField = new CCTextField("[click here for input]",
                    "fonts/MarkerFelt",
                    22,
                    CCLabelFormat.SpriteFont);
    
                AddChild(textField);
    

There are different delegates you can subscribe to be notified when specific actions have happened:

            textField.BeginEditing += OnBeginEditing;
            textField.EndEditing += OnEndEditing;

For instance to scroll the text field up the screen when editing begins and scroll it back into position when editing ends:

            public override void OnEnter()
            {
                base.OnEnter();
    
                // define our scrolling actions
                scrollUp = new CCMoveTo(0.5f, VisibleBoundsWorldspace.Top() - new CCPoint(0, s.Height / 4));
                scrollDown = new CCMoveTo(0.5f, textField.Position);
    
            }
    
            private void OnEndEditing(object sender, ref string text, ref bool canceled)
            {
                ((CCNode)sender).RunAction(scrollDown);
            }
    
            private void OnBeginEditing(object sender, ref string text, ref bool canceled)
            {
                ((CCNode)sender).RunAction(scrollUp);
            }

There are also delegates that can be subscribed to that will notify the developer of keyboard show and hide events:

            void AttachListeners()
            {
                // Attach our listeners.
                var imeImplementation = trackNode.TextFieldIMEImplementation;
                imeImplementation.KeyboardDidHide += OnKeyboardDidHide;
                imeImplementation.KeyboardDidShow += OnKeyboardDidShow;
                imeImplementation.KeyboardWillHide += OnKeyboardWillHide;
                imeImplementation.KeyboardWillShow += OnKeyboardWillShow;
    
            }

`CCTextField` Samples include:

* [TextField](https://github.com/mono/cocos-sharp-samples/tree/master/TextField) in CocosSharp Samples
* [TextFieldTest](https://github.com/mono/CocosSharp/tree/master/tests/tests/classes/tests/TextInputTest) in our tests.  

The tests also show an example of customizing how the text field works by implementing a [custom IMEImplementation](https://github.com/mono/CocosSharp/blob/master/tests/tests/classes/tests/TextInputTest/TextInputTest.cs#L556).



### CCStats Enhancements

There were a few problems with CocosSharp's statistics not working correctly across all platforms. New `CCStats` uses a new font with more characters, which allows us to display even short descriptions of the values.

![](images/ccstats-2015-09-16.png)

### Windows Phone 8.1 `CCLabel`

CCLabel is now supported on Windows Phone 8.1.  There were multiple problems across devices were it did not work at all and are now fixed in this release.  

### Accelerometer

Support for accelerometer was added in this release for Windows Phone 8.1 and Windows 8.1.  

Also, just as a side note there was also a fix for iOS accelerometer [305](https://github.com/mono/CocosSharp/issues/305) [IOS] Accelerometer not responding.  It has popped up from time to time as a problem but the source of the problem was never tracked down until recently.  Read commit note for more information.

### GUI Extensions

Major overhaul of the [GUI extensions](https://github.com/mono/CocosSharp/tree/master/Extensions/GUI).  These have not been high priority so have been lagging behind.

Notable improvements
* All GUI elements now sport a new C# events interface.
* Major overhaul of CCScale9Sprite to bring it up to current functionality and render correctly.
* Removal of obsolete CCSpriteBatch where appropriate
* All other elements are now rendering correctly based on [our tests](https://github.com/mono/CocosSharp/tree/master/tests/tests/classes/tests/ExtensionsTest).


### Release Contributors

* Alex Sorokoletov - Fixed Windows Phone 8.1 orientation, also provided valuable feedback on Windows Phone 8.1 CCLabel support.
* Vincent Dondain - Upgrade of CocosSharp Xamarin Studio templates to use the new project wizard pages.
* Marius Ungureanu - Memory leaks and performance changes. 

## Breaking changes
 ---

### Windows 8.0 is no longer supported in our NuGet packages.

Due to the way NuGet packages it's support for netcore targets we can only provide one so Windows 8.0 support was replaced with Windows 8.1 assemblies.  For those relying on this support you can still compile from source and use the generated NuGets for Windows 8.0.

### Legacy support for iOS .xnb content assets generated by the pipeline.

Previously, MonoGame would try to load the generated .xnb even though it was not generated for iOS platform specifically.  Now you may have to rebuild these if they were not specifically targeting iOS platform.  We have seen this mostly with .spritefonts that are used.

So if you have a project that used to work with .xnb formatted assets and after upgrading there are loading problems try regenerating them  specifically targeting iOS platform.  

View the [CocosSharp Content Pipeline introduction](https://developer.xamarin.com/guides/cross-platform/game_development/cocossharp/content_pipeline/)

### Unified PCL NuGet changes

*  iOS
*  Android
*  Windows DX
*  Windows 8.1
*  Windows Phone 8.1
*  Windows Phone 8.0

This also includes changes to the PCL profiles that are supported.  If you have problems updating your PCL projects you will need to update the profile to include Windows 8.1 or the NuGet package will not install.

* Mac - .NETPortable Version=v4.5 Profile=Profile111
* Windows - .NETPortable Version=v4.5 Profile=Profile138

### SharpDX updates

This will only be a possible problem if you are referencing SharpDX in your own projects.  SharpDX was updated to the latest release across all platforms.

### Note on Windows 10 and Visual Studio 2015

There have been reports of difficulties getting MonoGame to compile on Visual Studio 2015 and Windows 10.  

We had a few problems getting MonoGame and by proxy CocosSharp to compile with Windows 10 and VS2015.

Ran into this obscure problem with only Visual Studio 2015 installed on Windows 10 - https://social.msdn.microsoft.com/Forums/sqlserver/en-US/8cb70b42-c45c-4d2c-989c-6affa2a88343/w81netcore-error-message-when-building-win81-app-on-win10?forum=wpdevelop which prevented any of the MonoGame projects to build on Visual Studio 2015. Even deleting the lock file it still showed the same problems. Thus ensued more uninstalls of VS and finally an install of VS 2013 community edition which still did not help. Turns out that you can not just do a repair or anything you have to un-install VS 2015 completely AND REBOOT the windows 10 then install VS 2015 from scratch WITH VS 2013 community edition ALREADY installed. Make sure the project.lock.json file is deleted and once all this dancing is done it looks like MG projects are able to be compiled and run.



## Fixes and enhancements 
 ---
* [309](https://github.com/mono/CocosSharp/issues/309) CCGeometryNode NRE if Packet Texture is null
* [308](https://github.com/mono/CocosSharp/issues/308) CCGeometryNode only has support for TriangleList primitive types
* [305](https://github.com/mono/CocosSharp/issues/305) [IOS] Accelerometer not responding
* [304](https://github.com/mono/CocosSharp/issues/304) Application.CurrentOrientation is always CCDisplayOrientation.Default
* [303](https://github.com/mono/CocosSharp/issues/303) Accelerometer support for WP8.1 and Windows8.1
* [301](https://github.com/mono/CocosSharp/pull/301) Add coverity badge
* [300](https://github.com/mono/CocosSharp/pull/300) Some coverity fixes
* [299](https://github.com/mono/CocosSharp/pull/299) Fix memory leak on Win-based systems when loading Tiffs.
* [298](https://github.com/mono/CocosSharp/issues/298) Windows 8.1/WinRT/XAML platforms do not use the search paths to load .TTF fonts.
* [297](https://github.com/mono/CocosSharp/pull/297) Add nowarn on CS1591 for cleaner output.
* [296](https://github.com/mono/CocosSharp/pull/296) Fix memory overhead - Thanks Marius Ungureanu
* [295](https://github.com/mono/CocosSharp/issues/295) CCContentManager.SharedContentManager.GetAssetStreamAsBytes(fontName, out fontName); cause too much memory overhead
* [294](https://github.com/mono/CocosSharp/issues/294) Cross-platform PCL project errors out when referencing MonoGame.
* [293](https://github.com/mono/CocosSharp/issues/293) Way to copy CCDrawNode or duplicate the line lists and triangle lists
* [292](https://github.com/mono/CocosSharp/issues/292) CCSprite: Add HalfTexelOffset property
* [291](https://github.com/mono/CocosSharp/issues/291) [Win8.1 and VS 2015 support] Windows 8.1 and Windows 8.1 XAML projects to templates to replace the Windows 8 templates
* [289](https://github.com/mono/CocosSharp/issues/289) Xamarin Studio templates don't work
* [287](https://github.com/mono/CocosSharp/issues/287) CCDrawNode.DrawPolygon shows seams between polygons when they are close together.
* [286](https://github.com/mono/CocosSharp/issues/286) CCEaseSineIn Odd Behavior
* [285](https://github.com/mono/CocosSharp/issues/285) Windows 10 - "Unable to load mfplat.dll"
* [284](https://github.com/mono/CocosSharp/issues/284) CCNode: ZOrder property not correctly updated after set
* [283](https://github.com/mono/CocosSharp/issues/283) Crash on iOS with CCTransitionFade 
* [282](https://github.com/mono/CocosSharp/issues/282) CCRandom throwing Overflow exception on 4S devices.
* [280](https://github.com/mono/CocosSharp/pull/280) CCTextField implemenation
* [279](https://github.com/mono/CocosSharp/issues/279) WP8.1: CCRenderTexture.SaveToStream results in black image
* [278](https://github.com/mono/CocosSharp/issues/278) WP8.1: Bad performance when using CCLabel SystemFont
* [277](https://github.com/mono/CocosSharp/issues/277) CCSprite UntrimmedSizeInPixels not working correctly.
* [276](https://github.com/mono/CocosSharp/issues/276) Error using Schedule from CCScene.
* [275](https://github.com/mono/CocosSharp/issues/275) CCNode.Visit() using CCRenderTexture results in all black texture
