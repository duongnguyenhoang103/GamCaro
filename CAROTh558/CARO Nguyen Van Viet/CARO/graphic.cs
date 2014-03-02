using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace CARO
{
   public class graphic
    {
        const int dong = 20, cot = 20, trai = 200, tren = 50, canh = 22, phai = cot * canh + trai, duoi = tren + dong * canh;
       static Color bgcolor = Color.MediumSeaGreen;
        public static void vien(Graphics gr, int x, int y, int width, int heso)
        { // p3 xoa khung cua quan //p2 dat khung
            Pen p0, p1 = new Pen(Color.Red,2), p2 = new Pen(Color.LightCyan),p3=new Pen(Color.Green);
            if (heso == 1)//ve cho khung 
                p0 = p1;
            else if (heso == 2)
                p0 = p2;
            else   
                p0 = p3;
            gr.DrawRectangle(p0, x, y, width, width);
         
        } 
        public static void khung(Graphics gr) 
        { 
            Brush b = new SolidBrush(bgcolor);
            gr.FillRectangle(b, trai, tren, dong * canh, cot * canh);
            
            Pen p = new Pen(Color.Green);
            for (int i = trai, j = tren; i < phai; i += canh, j += canh)
            {   gr.DrawLine(p, trai, j, phai, j);
                gr.DrawLine(p, i, tren, i, duoi);
            }
            vien(gr, trai -1, tren - 1, dong * canh + 2, 1);  
        }
        public static void datquan(Graphics gr, int d, int c, int quan)
        {
           
           Rectangle rect = new Rectangle(c * canh + trai, d * canh + tren, canh, canh);
           if (quan == 0)
           {
               Brush b = new SolidBrush(bgcolor);
               gr.FillRectangle(b, rect);
           }
           else
           {
               GraphicsPath path = new GraphicsPath();
               path.AddEllipse(rect);
               PathGradientBrush b1 = new PathGradientBrush(path);
               b1.CenterColor = Color.Empty; 
               b1.CenterPoint = new PointF(rect.Left+canh/2+5,rect.Top+canh/2+5);
               if (quan == 1)
                   b1.SurroundColors = new Color[] { Color.DarkRed};
               if (quan == 2) b1.SurroundColors = new Color[] { Color.Black};
               gr.CompositingQuality = CompositingQuality.HighQuality;
               gr.FillRectangle(b1, rect);
           }
           vien(gr, trai + c * canh, tren + d * canh, canh,3);
       }
        public static void mousemove(Graphics gr, ref int old_x,ref int old_y,int mouse_x,int mouse_y)
       {   int c = (mouse_x - trai) / canh, d = (mouse_y - tren) / canh;
       if ((c < 0 || d < 0 || d >= dong || c >= cot) && (old_x >= 0 && old_y >= 0))
       { vien(gr, trai + old_x * canh, tren + old_y * canh, canh, 3); old_x = -1; old_y = -1; }
     if(c>=0&&d>=0&&d<dong&&c<cot)
       {   if (old_x != c || old_y != c)
           {    if(old_x>=0&&old_y>=0)
               vien(gr, trai + old_x * canh, tren + old_y * canh, canh,3);
               vien(gr, trai + c * canh, tren + d * canh, canh, 2);
               old_x = c; old_y = d;
           }
       } 
       }
        public static void phuchoi(Graphics gr, int[,] a)
       {
           for (int i = 0; i < dong; i++)
               for (int j = 0; j < cot; j++)
                   if (a[i, j] != 0)
                       datquan(gr, i, j, a[i, j]);
               
       }
   }
}
