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
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private BitmapSource image = new BitmapImage();
        private int epoche = 0;
        private int herb_count = 0;
        private int plant_count = 0;

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

        public int HerbCount
        {
            get { return herb_count; }
            set
            {
                herb_count = value;
                OnPropertyChanged("HerbCount");
            }
        }

        public int PlantCount
        {
            get { return plant_count; }
            set
            {
                plant_count = value;
                OnPropertyChanged("PlantCount");
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
            ImageWrapper img_wpar = new ImageWrapper(Img);

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    img_wpar.SetPixel(new System.Drawing.Point(j, i), GetColor(Schema.Sch[i, j]).ToArgb());

            img_wpar.Dispose();

            Update(Img);

            Epoche = Schema.Epoche;
            HerbCount = Schema.Herbivores.Count;
            PlantCount = Schema.Plants.Count;
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
            for (int i = 0; i < 1; i++)
            {
                Schema.Move();
                ReDraw();
            }
        }

        public void Revert()
        {
            Schema.Revert();
            ReDraw();
        }

        public void Update(Bitmap img)
        {
            IntPtr img_ptr = img.GetHbitmap();

            try
            {
                Image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(img_ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(img_ptr);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
