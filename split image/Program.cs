using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace split_image
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap image = (Bitmap)Image.FromFile("Yo.bmp");
            split(image);
        }
        public static void split(Bitmap image)
        {
            int[] heights = new int[image.Height];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (image.GetPixel(x, y).R == 0)
                    {
                        heights[y] = 1;
                        break;
                    }
                }
            }
            int j = 0;
            while (heights.Contains(1))
            {
                cropY(image, ref heights).Save(j.ToString() + ".bmp");
                j++;
            }
            Console.WriteLine(j);

            for (int i = 0; i < j; i++)
            {
                Console.WriteLine(i);
                int[] widths = new int[image.Width];
                Bitmap img = (Bitmap)Image.FromFile(i.ToString() + ".bmp");
                Console.WriteLine("opening {0}", i.ToString() + ".bmp");
                for (int x = 0; x < img.Width; x++)
                {
                    for (int y = 0; y < img.Height; y++)
                    {
                        if (img.GetPixel(x, y).R == 0)
                        {
                            widths[x] = 1;
                            break;
                        }
                    }
                }
                int k = 0;
                while (widths.Contains(1))
                {
                    Console.WriteLine("creating {0}", k.ToString() + "last.bmp");
                    cropX(img, ref widths).Save(k.ToString() + " " + i.ToString() + " last.bmp");
                    k++;
                }
                img.Dispose();

            }
            for (int i = 0; i < j; i++)
            {
                
                File.Delete(i.ToString() + ".bmp");
            }
        }
        public static Bitmap cropY(Image img, ref int[] array)
        {
            int Y1 = 0, Y2 = 0;
            bool first = false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 1 && !first)
                {
                    Y1 = i;
                    first = true;
                }
                if (array[i] == 0 && first)
                {
                    Y2 = i - 1;
                    break;
                }
                array[i] = 0;
            }
            Rectangle crop = new Rectangle(0, Y1, img.Width, Y2 - Y1);
            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }
        public static Bitmap cropX(Image img, ref int[] array)
        {
            int X1 = 0, X2 = 0;
            bool first = false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 1 && !first)
                {
                    X1 = i;
                    first = true;
                }
                if (array[i] == 0 && first)
                {
                    X2 = i;
                    break;
                }
                array[i] = 0;
            }
            Rectangle crop = new Rectangle(X1, 0, X2 - X1, img.Height);
            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }
    }
}
