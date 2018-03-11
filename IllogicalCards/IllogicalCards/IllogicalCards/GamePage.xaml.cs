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

namespace IllogicalCards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
        Card testCard;

		public GamePage ()
		{
			InitializeComponent ();
            testCard = new Card()
            {
                Type = CardType.Question,
                Text = "____ + ____"
            };
		}

        public void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs pse)
        {
            SKSurface surf = pse.Surface;
            SKCanvas cv = surf.Canvas;
            SKImageInfo ii = pse.Info;
            float w = ii.Width;
            float h = ii.Height;
            cv.Scale(h / 400.0f);
            cv.Translate(50.0f, 50.0f);
            Rendering.DrawCard(cv, testCard);
        }
    }
}
