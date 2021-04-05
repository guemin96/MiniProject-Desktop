using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NaverMobieFinderApp.Model
{
    public class YoutubeItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string URL { get; set; }
        public BitmapImage Thumbnail { get; set; }
    }
}
