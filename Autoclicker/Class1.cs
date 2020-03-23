using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoclicker
{
    class Class1
    {

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

     
        Queue<Bitmap> bitmaps;
        Queue<string> text;

        private static void SendCtrlhotKey(char key)
        {
            keybd_event(0x11, 0, 0, 0);
            keybd_event((byte)key, 0, 0, 0);
            keybd_event((byte)key, 0, 0x2, 0);
            keybd_event(0x11, 0, 0x2, 0);
        }


        public Class1()
        {

            bitmaps = new Queue<Bitmap>();
            text = new Queue<string>();

            int i = 4;

            text.Enqueue(i + "00-" + i + "09");
            text.Enqueue(i + "10-" + i + "19");
            text.Enqueue(i + "20-" + i + "29");
            text.Enqueue(i + "30-" + i + "39");
            text.Enqueue(i + "40-" + i + "49");
            text.Enqueue(i + "50-" + i + "59");
            text.Enqueue(i + "60-" + i + "69");
            text.Enqueue(i + "70-" + i + "79");
            text.Enqueue(i + "80-" + i + "89");
            text.Enqueue(i + "90-" + i + "99");

            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\02.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));
            bitmaps.Enqueue(new Bitmap(@"D:\Прочее\Cards\Print\" + i + "00-" + i + @"99\01.jpg", true));

            //SendCtrlhotKey('C');
            //SendCtrlhotKey('V');
            //SendCtrlhotKey('A');


            Form1.eventClick.Add(GetBufferText);
            Form1.eventClick.Add(GetBufferImg);
            Form1.eventClick.Add(Save);
        }

        public void GetBufferImg(object sender, EventArgs eventArgs)
        {
            Clipboard.SetImage(bitmaps.Dequeue());
            Thread.Sleep(30);
            SendCtrlhotKey('V');
            //MessageBox.Show("AAA");
        }

        public void GetBufferText(object sender, EventArgs eventArgs)
        {
            Clipboard.SetText(text.Dequeue());
            Thread.Sleep(100);
            SendCtrlhotKey('V');
        }

        public void Save(object sender, EventArgs eventArgs)
        {
            Thread.Sleep(30);
            SendCtrlhotKey('S');
        }

    }
}
