using System;
using System.Collections.Generic;
using System.Text;
using CardLib;
using SkiaSharp;

namespace IllogicalCards
{
    class Rendering
    {
        static SKPaint cardWhitePaint = null;
        static SKPaint cardBlackPaint = null;
        static SKPaint cardShadowPaint = null;

        public static void DrawCard(SKCanvas cv, Card c)
        {
            if (cardWhitePaint == null)
            {
                cardWhitePaint = new SKPaint
                {
                    Color = SKColors.WhiteSmoke,
                    TextSize = 10
                };
                cardBlackPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 10
                };
                cardShadowPaint = new SKPaint
                {
                    Color = SKColors.Gray,
                    BlendMode = SKBlendMode.Multiply
                };
            }
            SKPaint bgPaint, fgPaint;
            if (c.Type == CardType.Black)
            {
                bgPaint = cardWhitePaint;
                fgPaint = cardBlackPaint;
            }
            else
            {
                bgPaint = cardBlackPaint;
                fgPaint = cardWhitePaint;
            }
            cv.DrawRoundRect(2 - 3, 2 - 3, 2 + 106, 2 + 106, 10, 10, cardShadowPaint);
            cv.DrawRoundRect(-3, -3, 106, 106, 10, 10, fgPaint);
            cv.DrawRoundRect(0, 0, 100, 100, 7, 7, bgPaint);
            int curChar = 0;
            float curY = 8 + fgPaint.FontSpacing;
            while (curChar < c.Text.Length)
            {
                float w;
                int cnt = (int)fgPaint.BreakText(c.Text.Substring(curChar).Trim(), 90, out w);
                bool breaking = (curChar + cnt) < c.Text.Length;
                string text = c.Text.Substring(curChar, cnt).TrimStart();
                if (!text.EndsWith(" ") && breaking)
                    text += "-";
                cv.DrawText(text, 8, curY, fgPaint);
                curChar += cnt;
                curY += fgPaint.FontSpacing;
            }
        }
    }
}
