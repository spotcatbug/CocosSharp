﻿using System;
using CocosSharp;

namespace tests
{
    // Subclass this for test cases with a back/restart/next navigation bar as well as a title/subtitle labels
    public abstract class TestNavigationLayer : CCLayerColor
    {
        CCMenu navigationMenu;
        CCMenuItem backMenuItem;
        CCMenuItem restartMenuItem;
        CCMenuItem nextMenuItem;


        #region Properties

        protected CCLabel TitleLabel { get; private set; }
        protected CCLabel SubtitleLabel { get; private set; }

        public virtual string Title
        {
            get { return string.Empty; }
        }

        public virtual string Subtitle
        {
            get { return string.Empty; }
        }

        #endregion Properties


        #region Constructors

        public TestNavigationLayer()
        {
            TitleLabel = new CCLabel(Title, "arial", 32, CCLabelFormat.SpriteFont);
            AddChild(TitleLabel, TestScene.TITLE_LEVEL);

            string subtitleStr = Subtitle;
			if (!string.IsNullOrEmpty(subtitleStr))
            {
                SubtitleLabel = new CCLabel(subtitleStr, "arial", 16, CCLabelFormat.SpriteFont);
                SubtitleLabel.AnchorPoint = CCPoint.AnchorMiddleTop;
                SubtitleLabel.HorizontalAlignment = CCTextAlignment.Center;
                AddChild(SubtitleLabel, TestScene.TITLE_LEVEL);
            }

            backMenuItem = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, BackCallback);
            restartMenuItem = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, RestartCallback);
            nextMenuItem = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, NextCallback);

            navigationMenu = new CCMenu(backMenuItem, restartMenuItem, nextMenuItem);

            AddChild(navigationMenu, TestScene.MENU_LEVEL);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 

            var visibleRect = VisibleBoundsWorldspace;

			if (!string.IsNullOrEmpty(Title))
				TitleLabel.Text = Title;

            TitleLabel.Position = new CCPoint(visibleRect.Center.X, visibleRect.Top().Y - 30);

            string subtitleStr = Subtitle;

            if (!string.IsNullOrEmpty(subtitleStr) && SubtitleLabel == null)
            {
                SubtitleLabel = new CCLabel(subtitleStr, "arial", 16, CCLabelFormat.SpriteFont);
                SubtitleLabel.AnchorPoint = CCPoint.AnchorMiddleTop;
                SubtitleLabel.HorizontalAlignment = CCTextAlignment.Center;
                AddChild(SubtitleLabel, TestScene.TITLE_LEVEL);
            }
            else 
    			if (!string.IsNullOrEmpty(Subtitle))
    				SubtitleLabel.Text = Subtitle;

            if(SubtitleLabel != null)
                SubtitleLabel.Position = new CCPoint(visibleRect.Center.X, visibleRect.Top().Y - 60);


            float padding = 10.0f;
            float halfRestartHeight = restartMenuItem.ContentSize.Height / 2.0f;


            navigationMenu.Position = CCPoint.Zero;
            backMenuItem.Position = new CCPoint(visibleRect.Center.X - restartMenuItem.ContentSize.Width * 2.0f, 
                visibleRect.Bottom().Y + halfRestartHeight) ;  

            restartMenuItem.Position = new CCPoint(visibleRect.Center.X, visibleRect.Bottom().Y + halfRestartHeight);

            nextMenuItem.Position = new CCPoint(visibleRect.Center.X + restartMenuItem.ContentSize.Width * 2.0f, 
                visibleRect.Bottom().Y + halfRestartHeight);
        }

        #endregion Setup content


        #region Callbacks

        public virtual void RestartCallback(object sender)
        {
        }

        public virtual void NextCallback(object sender)
        {
        }

        public virtual void BackCallback(object sender)
        {
        }

        #endregion Callbacks
    }
}

