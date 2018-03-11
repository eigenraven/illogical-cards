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

        public static void DrawCard(SKCanvas cv, Card c)
        {
            if(cardWhitePaint == null)
            {
                cardWhitePaint = new SKPaint();
                cardWhitePaint.Color = SKColors.WhiteSmoke;
                cardBlackPaint = new SKPaint();
                cardBlackPaint.Color = SKColors.Black;
            }
            SKPaint bgPaint, fgPaint;
            if(c.Type == CardType.Answer)
            {
                bgPaint = cardWhitePaint;
                fgPaint = cardBlackPaint;
            }
            else
            {
                bgPaint = cardBlackPaint;
                fgPaint = cardWhitePaint;
            }
            cv.DrawRoundRect(-3, -3, 106, 106, 10, 10, fgPaint);
            cv.DrawRoundRect(0, 0, 100, 100, 10, 10, bgPaint);
            cv.DrawText(c.Text, 8, 28, fgPaint);
        }
    }
}
