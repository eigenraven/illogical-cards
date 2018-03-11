using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IllogicalCards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void HostButton_Clicked(object sender, EventArgs e)
        {
            string nick = this.NickEntry.Text;
            Navigation.PushModalAsync(new GamePage(nick), true);
        }

        public void EditButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new EditorPage(), true);
        }
    }
}