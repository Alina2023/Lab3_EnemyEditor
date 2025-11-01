using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lab2_EnemyEditor
{
    public class CIcon
    {
        public string Name { get; private set; }
        public Image ImageControl { get; private set; }

        public CIcon(string imagePath, int width = 64, int height = 64)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(imagePath);
            ImageControl = new Image();
            ImageControl.Width = width;
            ImageControl.Height = height;

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new System.Uri(imagePath, System.UriKind.RelativeOrAbsolute);
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();
            ImageControl.Source = bmp;
            ImageControl.Tag = Name;
        }
    }
}
