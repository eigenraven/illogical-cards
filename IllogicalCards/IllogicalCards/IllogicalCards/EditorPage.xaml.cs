using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CardLib;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.IO;

namespace IllogicalCards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditorPage : ContentPage
	{
        List<TextReader> setFiles;
        List<Card> sets;

        public EditorPage()
        {
            InitializeComponent();

            String[] setFiles = CardSet.ScanAllSets();
            sets.Capacity = setFiles.Length;

            //setFiles.Select(setFile =>
            foreach (String setFile in setFiles)
            {
                sets.Add(new Card()
                {
                    Type = CardType.Black,
                    Text = setFile.Remove(setFile.Length - 5)
                });
                // 5 is the length of ".json"
            }
            //);
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
            Rendering.DrawCard(cv, sets.First());
        }
    }
}