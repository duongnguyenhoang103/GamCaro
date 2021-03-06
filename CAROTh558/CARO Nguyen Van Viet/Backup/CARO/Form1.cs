using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace CARO
{
    public partial class Form1 : Form
    {  const int dong=20,cot=20,trai=200,tren=50,canh=22,phai=cot*canh+trai,duoi=tren+dong*canh;
       private int[,] a=new int[dong,cot],nhay=new int[5,2];
        private int old_x=-1, old_y=-1,luotdi=1,cd_luu=18,luotnhay,lannhay,countplayer=1,dung=1,skill=0;
        private int[, ,] comp_play;
        private int oldplayer2_d,oldplayer2_c,hei;
        private int[] cd_complay;
        private Timer timer1 = new Timer();
        private string lbtxt;
        public Form1()
        {
            comp_play = new int[cd_luu, 500, 2];
            cd_complay = new int[cd_luu];
            InitializeComponent();
            Width = 700;
            Height = 550;
            Paint+=new PaintEventHandler(giaodien);
            timer1.Interval = 100;
            timer1.Tick+=new EventHandler(nhapnhay);
            label1.Left = 10;
            label1.Top = 100;
            label1.Size = new Size(170, 320);
            label1.Paint += new PaintEventHandler(label1_Paint);
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (hei >-(label1.Height-30))
                hei--;
            else hei = label1.Height;
            label1.Invalidate();
        }
        void label1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            Brush b = new SolidBrush(Color.Green);
            gr.DrawString(lbtxt,label1.Font, b, 0, hei,StringFormat.GenericDefault);
            
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            lbtxt = label1.Text;
            label1.Text = "";
            hei = label1.Bottom-100;
            timer2.Start();
            starMarqueeToolStripMenuItem.Enabled = false;       
        }
        private void kiemtra(int d, int c)
        {  
            int i = d, j = c,dem=0,k=d,h=c,thang=0;
            int l=1,tang_d,tang_c;
            while (l < 5)
            { dem=0;
            i = d; j = c; k = d; h = c;
                xacdinhchieu(out tang_d, out tang_c, l);
                while (l < 5)
                {   i -= tang_d; j -= tang_c;
                if (i < 0 || j < 0 || i >= dong || j >= cot) break;
                if (a[i, j] == a[d, c])
                {
                    k = i; h = j;
                }
                else break;
                }
                while (l < 5)
                {
                    if (a[k, h]==a[d, c])
                    {
                        nhay[dem,0] = k;nhay[dem, 1] = h;dem++;
                    }
                    else break;
                    if (dem >=5)
                    { thang = 1; break; }
                    k += tang_d; h+= tang_c;
                    if (k < 0 || h < 0 || k >= dong ||h >= cot) break;
                }
                if(thang==1)break;
                l++;
            }
            if (thang == 1)
            { dung = 1; timer1.Start(); }
            else xoanhay();
        }
        protected virtual void nhapnhay(object o, EventArgs e)
        {   Graphics gr = CreateGraphics();
            for (int i = 0; i < 5; i++)
            {
                if (luotnhay == 0)
                    graphic.datquan(gr, nhay[i, 0], nhay[i, 1], 0);
                else graphic.datquan(gr, nhay[i, 0], nhay[i, 1], a[nhay[i, 0], nhay[i, 1]]);
            }
            int temp = luotnhay == 0 ? 1 : 0;
            luotnhay = temp;
            lannhay++;
            if (lannhay == 20) { timer1.Stop(); lannhay = 0; thongbaoketqua(a[nhay[0, 0], nhay[0, 1]]); }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (luotdi == 1&&dung==0)
            {
                Graphics gr = CreateGraphics();
                graphic.mousemove(gr, ref old_x, ref old_y, e.X, e.Y);
            }
            else { old_x = -1; old_y = -1; }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {   Graphics gr = CreateGraphics();
        int d = (e.Y - tren) / canh, c = (e.X - trai) / canh;
        if (d >= 0 && c >= 0 && c < cot && d < cot&&a[d,c]==0&&luotdi==1&&dung==0)
        {
            if (e.Button == MouseButtons.Left)
            {
                a[d, c] = 1;
                graphic.datquan(gr, d, c, 1);

              kiemtra(d, c);
             luotdi = 0;
              if (countplayer == 1)
            maydanh(gr);
         else { oldplayer2_d = d; oldplayer2_c = c; graphic.vien(gr, trai + canh * c, tren + canh * d,canh, 2); }
            }
           
        }
            
        }
        private void xoanhay()
        {
            for (int i = 0; i < 5; i++)
            { nhay[i, 0] = 0; nhay[i, 1] = 0; }
        }
        private void giaodien(object o,PaintEventArgs p)
        {  
            Graphics gr = p.Graphics;
            graphic.khung(gr);
            graphic.phuchoi(gr, a);
        }
        private void xacdinhchieu(out int tang_d, out int tang_c, int chieu)
        {
            tang_d = 0; tang_c = 0;
            if (chieu == 1)
                tang_d = 1;
            else if (chieu == 2)
                tang_c = 1;
            else if (chieu == 3)
            { tang_d = 1; tang_c = 1; }
            else { tang_d = 1; tang_c = -1; }
        }
        private void ganmang(int d, int c, int index)
        {
            comp_play[index, cd_complay[index], 0] = d;
            comp_play[index, cd_complay[index], 1] = c;
            cd_complay[index]++;
        }
        private void Count(int d, int c, ref int q, ref int dq, ref int dkt, ref int p, int chieu, int vitri)
        {
            int tang_d, tang_c, k = 0, t = 0, dq1 = 0, dq2 = 0, q1 = 0, q2 = 0;
            xacdinhchieu(out tang_d, out tang_c, chieu);
            while (t == 0)
            {
                if (vitri == 1) { d -= tang_d; c -= tang_c; }
                else { d += tang_d; c += tang_c; }
                if (d >= dong || c >= cot || d < 0 || c < 0) break;
                if (a[d, c] == 0)
                {
                    dkt++;
                    if (dkt >= 2) break;
                }
                else if (a[d, c] == 1)
                {
                    if (dkt > p || dq2 > 0) break;
                    if (dkt == 1) k = 1;
                    dq1++; q1 = 1;

                }
                else if (a[d, c] == 2)
                {
                    if (dkt > p || dq1 > 0) break;
                    if (dkt == 1) k = 1;
                    dq2++; q2 = 2;
                }
            }
            q = dq1 > dq2 ? q1 : q2;
            dq = dq1 > dq2 ? dq1 : dq2;
            if (k == 1) p = 0;

        }
        private void AddPosition(int d, int c)
        {

            int left_space, right_space, q1 = 0, q2 = 0, dq1 = 0, dq2 = 0, dkt1 = 0, dkt2 = 0;
            int[,] nhan = new int[4, 5];
            for (int i = 1; i < 5; i++)
            {
                q1 = 0; q2 = 0; dq1 = 0; dq2 = 0; dkt1 = 0; dkt2 = 0;
                left_space = 1;
                Count(d, c, ref q1, ref dq1, ref dkt1, ref left_space, i, 1);
                right_space = left_space;
                Count(d, c, ref q2, ref dq2, ref dkt2, ref right_space, i, 2);
                nhan[i - 1, 4] = right_space; nhan[i - 1, 2] = dkt1; nhan[i - 1, 3] = dkt2;
                if (q1 == q2)
                {
                    nhan[i - 1, 0] = q1; nhan[i - 1, 1] = dq1 + dq2; nhan[i - 1, 2] = dkt1; nhan[i - 1, 3] = dkt2;

                }
                else
                {
                    if (dq1 == dq2)
                    {
                        nhan[i - 1, 0] = 2; nhan[i - 1, 1] = dq1;
                        if (q1 == 2 && (left_space == 0 || right_space == 1))
                            nhan[i - 1, 3] = 0;
                        if (q1 == 2 && left_space == 1 && right_space == 0)
                            nhan[i - 1, 3] = 1;
                        if (q2 == 2 && left_space == 1) nhan[i - 1, 2] = 0;
                        if (q2 == 2 && left_space == 0) nhan[i - 1, 2] = 1;
                    }
                    else if (dq1 > dq2)
                    {
                        nhan[i - 1, 0] = q1; nhan[i - 1, 1] = dq1;
                        if (dq2 > 0 && (left_space == 0 || right_space == 1)) nhan[i - 1, 3] = 0;
                        if (dq2 > 0 && left_space == 1 && right_space == 0) nhan[i - 1, 3] = 1;
                    }
                    else if (dq2 > dq1)
                    {
                        nhan[i - 1, 0] = q2; nhan[i - 1, 1] = dq2;
                        if (dq1 > 0 && left_space == 1) nhan[i - 1, 2] = 0;
                        if (dq1 > 0 && left_space == 0) nhan[i - 1, 2] = 1;
                    }
                }
            }
            luachonmang(nhan, d, c);
        }
        private int ssdk(int[,] nhan, int q, int soq, bool chan,int space, ref int chieu)
        {   for (int i = chieu; i < 4; i++)
            {
                if (nhan[i, 0] == q && nhan[i, 1] >=4&& soq == 4 && nhan[i, 4] == 1)
                 { chieu = i; return 1; }
                if (nhan[i, 0] == q && nhan[i, 1] == soq)
                {
                    if (soq == 3 && nhan[i, 4] == space)
                    {
                        if (chan == false && space == 1 && nhan[i, 2] >= 1 && nhan[i, 3] >= 1)
                        { chieu = i; return 1;}
                        if (chan == true)
                        {
                            if (space == 1 && ((nhan[i, 2] == 0 && nhan[i, 3] >= 1) || (nhan[i, 2] >= 1 && nhan[i, 3] == 0)))
                            { chieu = i; return 1; }
                            if (space == 0) { chieu = i; return 1; }
                        }
                    }
                    if (soq == 2)
                    {
                        if (chan == false && nhan[i, 4] == space)
                        {
                            if (space == 1 && ((nhan[i, 2] >= 1 && nhan[i, 3] >= 2) || (nhan[i, 2] >= 2 && nhan[i, 3] >= 1)))
                            { chieu = i; return 1; }
                            if (space == 0 && nhan[i, 2] >= 2 && nhan[i, 3] >= 2) { chieu = i; return 1; }
                        }
                        if (chan == true)
                        { if(nhan[i,4]==1&&((nhan[i,2]==0&&nhan[i,3]>=2)||(nhan[i,2]>=2&&nhan[i,3]==0)))
                           { chieu = i; return 1; }
                          if(nhan[i,4]==0&&((nhan[i,2]==1&&nhan[i,3]>=1)||(nhan[i,2]>=1&&nhan[i,3]==1)))
                          { chieu = i; return 1; }
                        }
                    }
                    if (soq == 1 && nhan[i, 4] == space)
                    { if((space==1||space==0)&&nhan[i,2] >=2&&nhan[i,3]>=2)
                        { chieu = i; return 1; }
                    }
                }
            }
            
            chieu = 4;
            return 0;
        }
        private void luachonmang(int[,] nhan, int d, int c)
        {
            int chieu,i,j,k=0,l,space;
         // 0 -> 3 =-=-=-=-
            j = 4;
            while (j >= 3)
            {
                l = 2;
                while (l > 0)
                {
                    chieu = 0;
                    do
                    {
                        if (ssdk(nhan, l,j, false, 1, ref chieu) == 1)
                            ganmang(d, c, k);
                        chieu++;
                    } while (chieu < 4);
                    l--; k++;
                }
                j--;
            }
          // 4 -> 11
            bool chan = true;
            k = 4;
            j = 3;
            while (j >= 2)
            {
                i = 2;
                while (i > 0)
                {
                    space = 1;
                    while (space >= 0)
                    {
                        chieu = 0;
                        do
                        {
                            if (ssdk(nhan, i, j,chan, space, ref chieu) == 1)
                                ganmang(d, c, k);
                            chieu++;
                        } while (chieu < 4);
                        k++; space--;
                    }
                    i--;
                }
                j--;
                chan = false;
            }
        // 12 -> 13
            k = 12;
            i = 2;
            while (i > 0)
            {
                chieu = 0;
                do
                {
                    if (ssdk(nhan, i, 2,true, 0, ref chieu) == 1)
                        ganmang(d, c, k);
                    chieu++;
                } while (chieu < 4);
                k++;i--;
            }
        // 14->17
            k = 14;
            i = 2;
            while (i > 0)
            {
                space = 1;
                while (space >= 0)
                {
                    chieu = 0;
                    do
                    {
                        if (ssdk(nhan,i, 1, false, space, ref chieu) == 1)
                            ganmang(d, c, k);
                        chieu++;
                    } while (chieu < 4);
                    k++;
                    space--;
                }
                i--;
            }
        }
        private void chonvitri()
        {
            for (int i = 0; i < cd_luu; i++)
                cd_complay[i] = 0;

            for (int d = 0; d < dong; d++)
                for (int c = 0; c < cot; c++)
                    if (a[d, c] == 0)
                        AddPosition(d, c);

        }
        private void thongbaoketqua(int i)
        {
            string ch = "";
            if (i == 1)
            {
                if (countplayer == 2)
                    ch += "Co do thang !";
                else ch += "Ban da thang computer !";
            }
            else
            {
                if (countplayer == 1)
                    ch += "Ban da thua computer !";
                else ch += "Co xanh thang !";
            }
            MessageBox.Show(ch, "Co Ca Ro  +-+-+-+-",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            label2.Visible = false;
            Graphics gr=CreateGraphics();
         if (countplayer == 2 && luotdi == 0&&dung==0)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        if (oldplayer2_d > 0)
                        { graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 0); oldplayer2_d--; graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 2); }
                        break;
                    }
                case Keys.Down:
                    {
                        if (oldplayer2_d < dong - 1)
                        { graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh,0); oldplayer2_d++; graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 2); }
                        break;
                    }
                case Keys.Left:
                    {
                        if (oldplayer2_c > 0)
                        { graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 0); oldplayer2_c--; graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 2); }
                        break;
                    }
                case Keys.Right:
                    {
                        if (oldplayer2_c < cot - 1)
                        { graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 0); oldplayer2_c++; graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 2); }
                        break;
                    }
                case Keys.Enter:
                    {
                        if (a[oldplayer2_d, oldplayer2_c] == 0) 
                        { 
                            graphic.datquan(gr, oldplayer2_d, oldplayer2_c, 2); a[oldplayer2_d, oldplayer2_c] = 2; kiemtra(oldplayer2_d, oldplayer2_c); luotdi = 1;;
                        }
                        break;
                    }
            }

        } 
        }
        private void xoa(Graphics gr)
        { for(int i=0;i<dong;i++)
            for(int j=0;j<cot;j++)
            if(a[i,j]!=0)
            {
                a[i, j] = 0; graphic.datquan(gr, i, j, a[i, j]);
            }

        }
        private int chon(int i, int k, ref int d, ref int c)
        {  
            d = 0; c = 0;
            if (cd_complay[i] == 0 || cd_complay[k] == 0) return 0; 
            int t = (i == k)? cd_complay[i] - 1 : cd_complay[i];
            for(int m=0;m<t;m++)
                for (int n = (k == i) ? (m+1) : 0; n < cd_complay[k]; n++)
                {
                    if (comp_play[i, m, 0] == comp_play[k, n, 0] && comp_play[i, m, 1] == comp_play[k, n, 1])
                    {
                        d = i;
                        c = m;
                        return 1;
                    }
                }
            return 0;
        }
        private void chonvitridedanhhard(ref int d, ref int c)
        {
            int index = 0, t = 0;
            Random rand = new Random();
            int i,j;
             if (cd_complay[0] >0)
                { index = 0; t = rand.Next(cd_complay[0]); goto end; }
               if (cd_complay[1] > 0)
               {
                   for (i = 1;i < 18;i++)
                       if (chon(1, i, ref index, ref t) == 1) goto end;
                   index = 1; t = rand.Next(cd_complay[1]); goto end;
               }
            if (cd_complay[2]>0)
               {index = 2; t = rand.Next(cd_complay[2]); goto end; }
            for (i = 4; i <6; i++)
                if (chon(4, i, ref index, ref t) == 1) goto end;
            if (chon(5, 5, ref index, ref t) == 1) goto end;
            for (i = 4;i < 6; i++)
                if (chon(i, 3, ref index, ref t) == 1) goto end;
            for (i = 6; i <18; i++)
                for (j = 4; j < 6; j++)
                {
                    if (i!=13&&chon(j, i, ref index, ref t) == 1) goto end;
                }
            for (i = 3;i < 8; i++)
            {
                if (i != 4&& i != 5 && chon(3,i,ref index, ref t) == 1) goto end;
            }
            for (i = 10; i < 12; i++)
                if (chon(3, i, ref index, ref t) == 1) goto end;
          
            for (i = 8; i <10;i++)
                if (chon(3, i, ref index, ref t) == 1) goto end;
            for (i = 12; i < 18; i++)
                if (chon(3,i, ref index, ref t) == 1) goto end;
            if (cd_complay[3] > 0)
            { index = 3; t = rand.Next(cd_complay[3]); goto end; }
            for (j = 6; j <12;j++)
                for(i = 6; i < 8; i++)
                    if (chon(i, j, ref index, ref t) == 1) goto end;
            for(i=8;i<18;i++)
                if(chon(8,i,ref index,ref t )==1) goto end;
            if (chon(9, 9, ref index, ref t) == 1) goto end;
            for (j = 12; j < 14; j++)
                for (i = 6; i < 8; i++)
                    if (chon(i, j, ref index, ref t) == 1) goto end;
            for (i = 4; i < 6; i++)
                if (cd_complay[i] > 0)
                { index = i; t = rand.Next(cd_complay[i]); goto end; }
            for (j = 10; j < 12; j++)
                for (i= 10; i < 12; i++)
                    if (chon(i, j, ref index, ref t) == 1) goto end;
       
            for(i=8;i<10;i++)
           if (cd_complay[i] > 0)
           { index = i; t = rand.Next(cd_complay[i]); goto end; }

            for (i = 14; i < 16; i++)
                if (chon(14, i, ref index, ref t) == 1) goto end;
            if (chon(15, 15, ref index, ref t) == 1) goto end;
         for(i=14;i<18;i++)
             if (cd_complay[i] > 0)
             { index = i; t = rand.Next(cd_complay[i]); goto end; }
         d = 8; c = 9; return;
            end:
            d = comp_play[index, t, 0];
            c = comp_play[index, t, 1];
           
        }
        private void chonvitridedanhsimple(ref int d, ref int c)
        {
            int index = 0, t = 0;
            Random rand = new Random();

            if (cd_complay[0] > 0)
            { index = 0; t = rand.Next(cd_complay[0]); goto end; }
            if (cd_complay[1] > 0)
            {
                for (int i = 1; i < 18; i++)
                    if (chon(1, i, ref index, ref t) == 1) goto end;
                index = 1; t = rand.Next(cd_complay[1]); goto end;
            }
            if (cd_complay[2] > 0)
            { index = 2; t = rand.Next(cd_complay[2]); goto end; }


            int[] temp = new int[] { 4, 5, 3, 6, 7, 8, 9, 10, 11, 12, 14, 15 };
            for (int i = 0; i < temp.Length; i++)
            {
                for (int j = 4; j < 6; j++)
                    if (chon(j, temp[i], ref index, ref t) == 1) goto end;
            }
            if (cd_complay[3] > 0)
            {
                temp = new int[] { 3, 6, 7, 10, 11, 8, 9, 12, 13, 14, 15 };
                for (int i = 0; i < temp.Length; i++)
                {
                    if (chon(3, temp[i], ref index, ref t) == 1) goto end;
                }
                index = 3; t = rand.Next(cd_complay[3]); goto end;
            }
            for (int i = 6; i < 8; i++)
            {
                for (int j = i; j < 8; j++)
                    if (chon(i, j, ref index, ref t) == 1) goto end;
            }
            for (int i = 6; i < 8; i++)
                for (int j = 8; j < 12; j++)
                    if (chon(i, j, ref index, ref t) == 1) goto end;
            temp = new int[] {8,9,10,11,12,14,15};
            for (int i = 0; i < temp.Length; i++)
                for (int j = 8; j < 10; j++)
                    if (chon(j,temp[i], ref index, ref t) == 1) goto end;
            for(int i=8;i<10;i++)
                if (cd_complay[i] > 0)
                {
                    index = i; t = rand.Next(cd_complay[i]); goto end; 
                }
            temp = new int[] {10,11,14,15};
            for (int i = 0; i < temp.Length; i++)
                for (int j = 10; j < 12; j++)
                    if (chon(j,temp[i], ref index, ref t) == 1) goto end;
            for(int i=4;i<6;i++)
                if (cd_complay[i] > 0)
                {
                    index = i; t = rand.Next(cd_complay[i]); goto end; 
                }
            for (int i = 14; i < 16; i++)
                for (int j = i; j < 16; j++)
                    if (chon(i, j, ref index, ref t) == 1) goto end;
            for(int i=14;i<18;i++)
                if(cd_complay[i]>0)
                {
                    index = i; t = rand.Next(cd_complay[i]); goto end;
                }
            d = 8; c = 9; return;
            end:
            d = comp_play[index, t, 0];
            c = comp_play[index, t, 1];
        }
        private void newgame(Graphics gr)
        {
            dung = 0;
            Random rand = new Random();
            luotdi = rand.Next(2);
            xoa(gr);
            if (luotdi == 0 && countplayer == 2)
            {   oldplayer2_d = 8; oldplayer2_c = 8;
                graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 2);
            }

            if (luotdi == 1 && countplayer == 1)
            {
                MessageBox.Show("Quyền đánh trước thuộc về bạn !", "Chuc mừng !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            if (luotdi == 0 && countplayer == 1)
            {
                a[8, 8] = 1; maydanh(gr); a[8, 8] = 0;
            }


        }
        private void maydanh(Graphics gr)
        {
            if (countplayer == 1 && luotdi == 0 && dung == 0)
            {
                chonvitri();
                int d = 0, c = 0;
                if (skill == 1)
                    chonvitridedanhsimple(ref d, ref c);
                else 
                    chonvitridedanhhard(ref d, ref c);
                graphic.datquan(gr, d, c, 2);
                a[d, c] = 2;
                kiemtra(d, c); luotdi = 1;
           } 
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
          DialogResult  d = MessageBox.Show("Ban thuc su muon thoat khoi chuong trinh nay ?", "Co Ca Ro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(d==DialogResult.Yes) Application.Exit(); 
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics gr = CreateGraphics();
            newgame(gr);
            if (countplayer == 1)
            {
                onePlayerToolStripMenuItem.Checked = true;
                twoPlayerToolStripMenuItem.Checked = false;
                hardToolStripMenuItem.Enabled = true;
                simpleToolStripMenuItem.Enabled = true;
                if (skill == 1) { simpleToolStripMenuItem.Checked = true; hardToolStripMenuItem.Checked = false; }
                else { simpleToolStripMenuItem.Checked = false; hardToolStripMenuItem.Checked = true; }
            }
        
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void onePlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            countplayer = 1;
            hardToolStripMenuItem.Enabled = true;
            simpleToolStripMenuItem.Enabled = true;
            onePlayerToolStripMenuItem.Checked = true;
            twoPlayerToolStripMenuItem.Checked = false;
        }

        private void twoPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            countplayer = 2;
            hardToolStripMenuItem.Enabled = false;
            simpleToolStripMenuItem.Enabled = false;
            onePlayerToolStripMenuItem.Checked = false;
            twoPlayerToolStripMenuItem.Checked = true;
        }
        private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skill = 1;
            simpleToolStripMenuItem.Checked = true;
            hardToolStripMenuItem.Checked = false;


        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skill = 2;
            simpleToolStripMenuItem.Checked =false ;
            hardToolStripMenuItem.Checked = true;
        }
        private void savegame()
        {
            StreamWriter sw = new StreamWriter(@"savegame.viet");
            for (int i = 0; i < dong; i++)
                for (int j = 0; j < cot; j++)
                    sw.WriteLine(a[i, j]);
            sw.WriteLine(countplayer);
            sw.WriteLine(luotdi);
            sw.WriteLine(skill);
            sw.Close();
        }
        private void loadgame()
        {
            try
            {
                StreamReader sr = new StreamReader(@"savegame.viet");
                for (int i = 0; i < dong; i++)
                    for (int j = 0; j < cot; j++)
                        a[i, j] = Convert.ToInt32(sr.ReadLine());

                countplayer = Convert.ToInt32(sr.ReadLine());
                luotdi = Convert.ToInt32(sr.ReadLine());
                skill = Convert.ToInt32(sr.ReadLine());
                sr.Close();
            }
      catch {
             MessageBox.Show("Ban chua save game lan nao ma !", "CARO WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {  int dem = 0;
            for(int i=0;i<dong;i++)
                for(int j=0;j<cot;j++)
                    if (a[i, j] != 0)
                    { dem++; break; }

            if (dung == 0&&dem!=0)
                savegame();
            else MessageBox.Show("Save game khong co gia tri !", "Co va ro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Graphics gr = CreateGraphics();
            xoa(gr);
            loadgame();
            graphic.phuchoi(gr, a);
            if (luotdi == 0 && countplayer == 2)
            {
                oldplayer2_d = 10; oldplayer2_c = 9;
                graphic.vien(gr, trai + oldplayer2_c * canh, tren + oldplayer2_d * canh, canh, 2);
            }
            if (luotdi == 0 && countplayer == 1)
            {
                maydanh(gr);
            }
            if (countplayer == 1)
            {   
                onePlayerToolStripMenuItem.Checked = true;
                twoPlayerToolStripMenuItem.Checked = false;
                hardToolStripMenuItem.Enabled = true;
                simpleToolStripMenuItem.Enabled = true;
                if (skill == 1) { simpleToolStripMenuItem.Checked = true; hardToolStripMenuItem.Checked = false; }
                else { simpleToolStripMenuItem.Checked = false; hardToolStripMenuItem.Checked = true; }
            }
            else
            { 
                twoPlayerToolStripMenuItem.Checked = true;
                onePlayerToolStripMenuItem.Checked = false;
                hardToolStripMenuItem.Enabled = false;
                simpleToolStripMenuItem.Enabled = false;
            }
        }

        private void stopMarqueeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            label1.Hide();
            hei = label1.Height;
            stopMarqueeToolStripMenuItem.Enabled = false;
            starMarqueeToolStripMenuItem.Enabled = true;
        }

        private void starMarqueeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Start();
            label1.Show();
            stopMarqueeToolStripMenuItem.Enabled = true;
            starMarqueeToolStripMenuItem.Enabled = false;
           
           
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label2.Location = new Point(230, 100);
            label2.Size = new Size(350, 200);
            label2.ForeColor = Color.Red;
            label2.Visible = true;
         

        }
    }
}