using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringSize
{
    //public static float Width(string text, Font? font = null, FontStyle fontStyle = FontStyle.Normal)
    public static float Width(string text, Font font, FontStyle fontStyle)
    {
        float totalLength = 0;

        //Font effectiveFont = font != null ? font : Stats.Font.defaultFont;
        //Font effectiveFont = font ?? Stats.Font.normal;
        
        char[] arr = text.ToCharArray();

        foreach(char c in arr)
        {
            CharacterInfo characterInfo = new();

            //effectiveFont.GetCharacterInfo(c, out characterInfo, effectiveFont.fontSize, fontStyle);
            font.GetCharacterInfo(c, out characterInfo, font.fontSize, fontStyle);
            totalLength += characterInfo.advance;
        }

        return totalLength;
    }
}
