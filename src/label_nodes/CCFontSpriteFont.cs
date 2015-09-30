﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCFontSpriteFont : CCFont
    {
        CCVector2 imageOffset;
        public bool IsFontConfigValid { get; private set; }
        string fontName;
        float fontSize;
        float fontScale;

        public CCFontSpriteFont (string fntFilePath, float fontSize, CCVector2? imageOffset = null)
        { 
            fontName = fntFilePath;
            this.fontSize = fontSize;

            this.imageOffset = CCVector2.Zero;

            if (imageOffset.HasValue)
                this.imageOffset = imageOffset.Value;

            fontScale = 1.0f;
        }

        /// <summary>
        /// Purges the cached data.
        /// Removes from memory the cached configurations and the atlas name dictionary.
        /// </summary>
        public void PurgeCachedData()
        {
            //            if (Configuration != null)
            //                Configuration.Clear();
        }

        internal override CCFontAtlas CreateFontAtlas()
        {
            float loadedSize = fontSize;
            SpriteFont font = CCSpriteFontCache.SharedInstance.TryLoadFont(fontName, fontSize, out loadedSize);
            if (font == null)
            {
                return null;
            }
#if XNA
            CCTexture2D atlasTexture = new CCTexture2D();
#else
            CCTexture2D atlasTexture = new CCTexture2D(font.Texture);
#endif

            if (loadedSize != 0)
            {
                fontScale = fontSize / loadedSize * CCSpriteFontCache.FontScale;
            }

            var atlas = new CCFontAtlas(this);
            // add the texture (only one for now)
            atlas.AddTexture(atlasTexture, 0);

            // Set the atlas's common height
            atlas.CommonHeight = font.LineSpacing;
            // Set the default character to us if a character does not exist in the font
            atlas.DefaultCharacter = font.DefaultCharacter;

#if !XNA
            
            var glyphs = font.GetGlyphs();
            var reusedRect = Rectangle.Empty;

            foreach ( var character in font.Characters)
            {
                var glyphDefintion = new CCFontLetterDefinition();

                glyphDefintion.LetterChar = character;

                var glyphDef = glyphs[character];

                glyphDefintion.XOffset = glyphDef.LeftSideBearing + glyphDef.Cropping.X;

                glyphDefintion.YOffset = glyphDef.Cropping.Y;

                reusedRect = glyphDef.BoundsInTexture;
                glyphDefintion.Subrect = new CCRect(reusedRect.X, reusedRect.Y, reusedRect.Width, reusedRect.Height);

                reusedRect = glyphDef.Cropping;
                glyphDefintion.Cropping = new CCRect(reusedRect.X, reusedRect.Y, reusedRect.Width, reusedRect.Height);

                glyphDefintion.TextureID = 0;

                glyphDefintion.IsValidDefinition = true;
                //glyphDefintion.XAdvance = (int)(font.Spacing + glyphDef.Width + glyphDef.RightSideBearing);
                glyphDefintion.XAdvance = (int)glyphDef.WidthIncludingBearings;

                atlas.AddLetterDefinition(glyphDefintion);

            }
#endif
            return atlas;
        }

        public override int[] HorizontalKerningForText(string text, out int numLetters)
        {
            numLetters = text.Length;

            if (numLetters == 0)
                return null;

            var sizes = new int[numLetters];
            if (sizes.Length == 0)
                return null;

            for (int c = 0; c < numLetters; ++c)
            {
                if (c < (numLetters-1))
                    sizes[c] = GetHorizontalKerningForChars(text[c], text[c+1]);
                else
                    sizes[c] = 0;
            }

            return sizes;

        }

        private int GetHorizontalKerningForChars(char firstChar, char secondChar)
        {
            int ret = 0;
            // TODO: Look at this as it seems to not do anything
            //int key = (firstChar << 16) | (secondChar & 0xffff);

            return ret;
        }

        public override float FontScale
        {
            get
            {
                return fontScale;
            }
        }
    }
}

