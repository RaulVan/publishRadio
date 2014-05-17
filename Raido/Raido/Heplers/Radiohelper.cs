using Microsoft.Phone.BackgroundAudio;
using Raido.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Raido
{
   public class Radiohelper
    {
       public List<AudioTrack> GetRadioList()
       {
           List<AudioTrack> audioList = new List<AudioTrack>();
           List<RadioContent> radios = DataService.GetRadios();
           foreach (var item in radios)
           {
               audioList.Add(new AudioTrack(new Uri(item.RadioURL, UriKind.Absolute), item.RadioName, item.Type, "", null, "", EnabledPlayerControls.Pause));
           }
           return audioList;
       }

       
       public  async Task<WriteableBitmap> Screen()
       {
           //截图
           await Task.Delay(200);

           using (System.IO.Stream stream1 = Application.GetResourceStream(new Uri(@"Assets/qcode.png", UriKind.Relative)).Stream)
           {
               double width = Application.Current.Host.Content.ActualWidth;
               double heigth = Application.Current.Host.Content.ActualHeight;
               WriteableBitmap wbmp = new WriteableBitmap((int)width, (int)heigth);
               wbmp.Render(App.Current.RootVisual, null);
               wbmp.Invalidate();


               WriteableBitmap wideBitmap = new WriteableBitmap((int)width, (int)(heigth + 280));

               BitmapImage image1 = new BitmapImage();
               image1.SetSource(stream1);
               stream1.Close();
               //stream1.Flush();

               wideBitmap.Blit(new Rect(0, 0, width, heigth), new WriteableBitmap(wbmp), new Rect(0, 0, wbmp.PixelWidth, wbmp.PixelHeight), WriteableBitmapExtensions.BlendMode.Additive);
               wideBitmap.Blit(new Rect((width - 280) / 2, heigth, 280, 280), new WriteableBitmap(image1), new Rect(0, 0, image1.PixelWidth, image1.PixelHeight), WriteableBitmapExtensions.BlendMode.Additive);

               return wideBitmap;
           }
       }
    }
}
