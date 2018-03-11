using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using CardLib;
using System.Reflection;
using System.IO;

namespace IllogicalCards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
        CardGame cg;
        float HandSlide = 0.0f;
        SKRect CardsRegion;

		public GamePage (string nick)
		{
			InitializeComponent ();
            CardSet cs1 = new CardSet();
            var assembly = typeof(CardSet).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("CardLib.SampleCards.json");
            cs1.Load(new StreamReader(stream));
            List<CardSet> sets = new List<CardSet>();
            sets.Add(cs1);
            cg = new CardGame(nick, sets);
            HandSlide = 4.0f;
        }

        delegate void del(int i);

        public void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs pse)
        {
            SKSurface surf = pse.Surface;
            SKCanvas cv = surf.Canvas;
            SKImageInfo ii = pse.Info;
            cv.Clear(SKColors.White);
            float w = ii.Width;
            float h = ii.Height;
            float sc = Math.Min(w, h) / 440.0f;
            if (cg.Phase == GamePhase.Choosing)
            {
                // draw hand
                int midcard = (int) Math.Round(HandSlide);
                float midscr = w / 2;
                del fn = (i) => {
                    cv.ResetMatrix();
                    float relidx = i - HandSlide;
                    relidx /= cg.Me.Hand.Count;
                    relidx *= 2; // [-1; 1] range
                    float csc = h / 400.0f * 1.3f * (1.0f - Math.Abs(relidx));
                    csc = Math.Max(csc, 0.01f);
                    cv.Translate(midscr + sc * ((w<h)?(midscr / 1.2f):(midscr * 0.75f)) * (float)Math.Pow(Math.Abs(relidx)*0.8f, 0.5) * Math.Sign(relidx), h - sc * 150.0f);
                    cv.Scale(csc);
                    cv.Translate(-51.5f, -51.5f);
                    Rendering.DrawCard(cv, cg.Me.Hand[i]);
                };
                for (int i = 0; i < midcard; i++)
                {
                    fn(i);
                }
                for (int i = cg.Me.Hand.Count-1; i > midcard; i--)
                {
                    fn(i);
                }
                fn(midcard);
                // Question card
                cv.ResetMatrix();
                sc *= 1.5f;
                cv.Scale(sc);
                cv.Translate(midscr/sc - 51.5f, 20.0f/sc);
                Rendering.DrawCard(cv, cg.AllBlackCards[0]);
            }
        }

        static float lastpan = 0.0f;

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch(e.StatusType)
            {
                case GestureStatus.Started:
                    lastpan = 0.0f;
                    break;
                case GestureStatus.Running:
                    float dx = (float)(e.TotalX - lastpan) / 440.0f;
                    dx = Math.Sign(dx) * (float) Math.Pow(Math.Abs(dx), Math.Max(0.3f, 1.0f-Math.Abs(e.TotalX)/600.0f));
                    HandSlide -= dx;
                    lastpan = (float)e.TotalX;
                    HandSlide = Math.Min(Math.Max(HandSlide, -0.4f), cg.Me.Hand.Count - 0.6f);
                    CanvasView.InvalidateSurface();
                    break;
                case GestureStatus.Canceled:
                case GestureStatus.Completed:
                    Animation zeroOut = new Animation(x => { HandSlide = (float)x; CanvasView.InvalidateSurface(); }, HandSlide, Math.Round(HandSlide), Easing.CubicOut);
                    zeroOut.Commit(this, "cardZeroOut");
                    break;
            }
            
        }
    }
}
