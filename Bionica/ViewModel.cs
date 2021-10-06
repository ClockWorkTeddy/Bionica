using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Drawing;
using System;
using System.Windows;

namespace Bionica
{
    class ViewModel : INotifyPropertyChanged
    {
        private BitmapSource image = new BitmapImage();
        private int epoche = 0;
        private int saturation = 0;

        Schema Schema = null;
        public int Size { get; set; } = 400;

        public BitmapSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        public int Epoche
        {
            get { return epoche; }
            set
            {
                epoche = value;
                OnPropertyChanged("Epoche");
            }
        }

        public int Saturation
        {
            get { return saturation; }
            set
            {
                saturation = value;
                OnPropertyChanged("Saturation");
            }
        }

        public ViewModel()
        {
            Schema = new Schema(Size);
            ReDraw();
        }

        public void ReDraw()
        {
            Bitmap Img = new Bitmap(Size, Size);

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Img.SetPixel(j, i, GetColor(Schema.Sch[i,j]));

            Update(Img);
            Epoche = Schema.Epoche;
            Saturation = Schema.Herbivores.Count > 0 ? (Schema.Herbivores[0] as MobileCreature).Saturation : 0;
        }

        public Color GetColor(int code)
        {
            Color color = Color.White;

            if (code == 1)
                color = Color.Green;
            else if (code == 2)
                color = Color.Brown;

            return color;
        }
        public void Start()
        {
            Schema.Start();
            ReDraw();
        }
        public void Next()
        {
            Schema.Move();
            ReDraw();
        }

        public void Update(Bitmap img)
        {
            Image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
