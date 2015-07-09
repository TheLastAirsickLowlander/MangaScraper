using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ScFix.Utility.Images
{
    public class BitmapConversions
    {
        public static Image BitmapSourceToImage(BitmapSource bs)
        {
            try
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bs));
                encoder.QualityLevel = 100;
                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Frames.Add(BitmapFrame.Create(bs));
                    encoder.Save(stream);
                    Image i = Image.FromStream(stream);
                    return i;
                }
            }
            catch (Exception e)
            {
                //Should do something maybe alert f
                Debug.WriteLine(e.ToString());
            }
            return null;
        }

        public static string SaveImageFile(Image image, string name = null ,string path = null)
        {
            try
            {
                if (name != null)
                {
                    if (path != null)
                    {
                        if (path[path.Length - 1] == '\\')
                        {
                            path = path + name;
                            image.Save(path);
                            return path;
                        }
                        else
                        {
                            path = path + "\\" + name;
                            image.Save(path);
                            return path;
                        }
                    }
                    else
                    {
                        path = System.IO.Path.GetTempPath() + name;
                        image.Save(path);
                        return path;
                    }
                }
                else
                {
                    //this might need to be modified to handle the extension of the file 
                    path = System.IO.Path.GetTempFileName();
                    image.Save(path);
                    return path;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
