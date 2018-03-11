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
        float handSlide = 0.0f;

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
		}

        public void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs pse)
        {
            SKSurface surf = pse.Surface;
            SKCanvas cv = surf.Canvas;
            SKImageInfo ii = pse.Info;
            float w = ii.Width;
            float h = ii.Height;
            cv.Scale(h / 400.0f);
            cv.Translate(20.0f, 20.0f);
            Rendering.DrawCard(cv, cg.AllBlackCards[0]);
            cv.ResetMatrix();
            cv.Scale(h / 400.0f);
            cv.Translate(20.0f, 220.0f);
            Rendering.DrawCard(cv, cg.AllWhiteCards[0]);
        }
    }
}
