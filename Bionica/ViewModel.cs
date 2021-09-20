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
        Schema Schema = null;
        public int Size { get; set; }

        public BitmapSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        public ViewModel()
        {
            Size = 400;
            Schema = new Schema(Size);
            ReDraw();
        }

        public void ReDraw()
        {
            Bitmap Img = new Bitmap(400, 400);

            for (int i = 0; i < 400; i++)
                for (int j = 0; j < 400; j++)
                    Img.SetPixel(i, j, GetColor(Schema.Sch[i,j]));

            Update(Img);
        }

        public Color GetColor(int code)
        {
            Color color = Color.White;

            if (code == 1)
                color = Color.Green;

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
