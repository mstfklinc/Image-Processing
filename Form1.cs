using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace WindowsFormsApplication2
{
    public partial class YSA : Form
    {
        int counter1 = 0, counter2 = 0, counter3 = 0;
        public static int[,] class1Member = new int[50, 2];
        public static int[,] class2Member = new int[50, 2];
        public static int[,] class3Member = new int[50, 2];

        public YSA()
        {
            InitializeComponent();
        }
        private void YSALoad(object sender, EventArgs e)   //burada _ nin olması olumuz.
        {

        }

        private void koordinatSistemi() {

            Graphics koordinat = CreateGraphics();
            Brush eksen = new SolidBrush(Color.Black);
            Pen ksistemi = new Pen(eksen, 3);
            Pen grid = new Pen(Color.Gray, 1);
            koordinat.DrawLine(ksistemi, 0, 300, 800, 300);
            koordinat.DrawLine(ksistemi, 400, 0, 400, 600);
            for (int i = 0; i < 600; i = i + 10)         //Yazılım ödevi için bu for döngleri kaldırılabilir.
            {
                for (int j = 0; j < 800; j = j + 10)
                {
                    koordinat.DrawLine(grid, 0, i, 800, i);
                    koordinat.DrawLine(grid, j, 0, j, 600);
                }
            }

        }

        /// <summary>
        /// SDPTA ///
        /// </summary>
        //// SDPTA değişkenleri 
        int w1 = 1, w2 = 1, w3 = 1, net = 0, output = 0, epsilon=1, cyclecountSDPTA = 0; //Başlagıç değerleri atanmak zorunda.
        //sabit değerler defineda tanımlanacak
        private void dogruCizSDPTA()
        {
            Graphics netgraph = CreateGraphics();
            Brush netgraphsbrush = new SolidBrush(Color.Navy);
            Pen netpen = new Pen(netgraphsbrush, 5);

            if (w2 != 0)
            {
                netgraph.DrawLine(netpen, 0, (((10 * w3) + (300 * w2) - (400 * w1)) / w2), 800, (((10 * w3) + (400 * w1) + (300 * w2)) / w2));
            }
            else netgraph.DrawLine(netpen, ((-10 * w3) + 400 * w1) / w1, 0, ((-10 * w3) + 400 * w1) / w1, 600);
        }
        private void dogruHesaplaSDPTA(int a, int b, int d)
        {
            net = w1 * a + w2 * b + w3;

            if (net > 0) { output = 1; }
            else { output = -1; }

            if (output != d)
            {
                ////if (output == -1)
                ////{
                ////    w1 = w1 + a;
                ////    w2 = w2 + b;
                ////    w3 = w3 + 1;
                ////}
                ////else
                ////{
                ////    w1 = w1 - a;
                ////    w2 = w2 - b;
                ////    w3 = w3 - 1;
                ////}
                w1 = w1 + (d - output) * a / 2;
                w2 = w2 + (d - output) * b / 2;
                w3 = w3 + (d - output) / 2;

                epsilon = 1;
            }
        }
        private void trainingSDPTA()
        {
            while (epsilon != 0)
            {
                epsilon = 0;
                
                if (counter1 > counter2)                             ////iki sınıftan sırayla alabilmek için
                {
                    for (int i = 0; i < counter1; i++)
                    {
                        dogruHesaplaSDPTA(class1Member[i, 0], class1Member[i, 1], 1);
                        if (i < counter2)
                        {
                            dogruHesaplaSDPTA(class2Member[i, 0], class2Member[i, 1], -1);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < counter2; i++)
                    {
                        if (i < counter1)
                        {
                            dogruHesaplaSDPTA(class1Member[i, 0], class1Member[i, 1], 1);
                        }
                       dogruHesaplaSDPTA(class2Member[i, 0], class2Member[i, 1], -1);
                    }
                }

               cyclecountSDPTA++;

                ////if (epsilon != 0)
                ////    

            }
           MessageBox.Show(cyclecountSDPTA.ToString() + ". cycle", "cycle sayısı");
           dogruCizSDPTA();
        }


        /// <summary>
        /// SCPTA ///
        /// ayırtaç fonksiyonların doğruları yanıltıcı olabiliyor. 
        /// muhtemelen int double dönüşümlerinden dolayı.
        /// </summary>
        //// SCPTA değişkenleri           
        double m1 = 1.0, m2 = 1.0, m3 = 1.0, cepsilon = 1.0, coutput = 0.0, cnet = 0.0, dxfnet = 0.0;
        double emax = 1.0;
        int cyclecountSCPTA = 0;

        private void dogruCizSCPTA()
        {
            Graphics netgraph1 = CreateGraphics();
            Brush netgraphsbrush1 = new SolidBrush(Color.DarkOrange);
            Pen netpen1 = new Pen(netgraphsbrush1, 5);

            if (m2 != 0)
            {
                netgraph1.DrawLine(netpen1, 0, (Convert.ToInt32(((10 * m3) + (300 * m2) - (400 * m1)) / m2)), 800, (Convert.ToInt32(((10 * m3) + (400 * m1) + (300 * m2)) / m2)));
            }
            else netgraph1.DrawLine(netpen1, (Convert.ToInt32(((-10 * m3) + 400 * m1) / m1)), 0, (Convert.ToInt32(((-10 * m3) + 400 * m1) / m1)), 600);
        }
        private void dogruHesaplaSCPTA(int a, int b, double d)
        {   
            cnet =  m1 * a + m2 * b + m3; ////net
            coutput = (2.0 / (1.0 + Math.Exp(-Convert.ToInt32(cnet))))-1.0; ////fnet
            dxfnet = (0.5) * (1.0 - (coutput * coutput)); ////f'net
            
                m1 = m1 + (0.5) * (d - coutput) * (dxfnet) * Convert.ToDouble(a);
                m2 = m2 + (0.5) * (d - coutput) * (dxfnet) * Convert.ToDouble(b);
                m3 = m3 + (0.5) * (d - coutput) * (dxfnet);

                cepsilon = cepsilon + (0.5) * (d - coutput) * (d - coutput); ////E=1/2(d-o)^2
            
        }
        private void trainingSCPTA()
        {
            do
            {
                cepsilon = 0.0;

                if (counter1 > counter2)                             ////iki sınıftan sırayla alabilmek için
                {
                    for (int i = 0; i < counter1; i++)
                    {
                        dogruHesaplaSCPTA(class1Member[i, 0], class1Member[i, 1], 1.0);
                        if (i < counter2)
                        {
                            dogruHesaplaSCPTA(class2Member[i, 0], class2Member[i, 1], -1.0);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < counter2; i++)
                    {
                        if (i < counter1)
                        {
                            dogruHesaplaSCPTA(class1Member[i, 0], class1Member[i, 1], 1.0);
                        }
                        dogruHesaplaSCPTA(class2Member[i, 0], class2Member[i, 1], -1.0);
                    }
                }

                cyclecountSCPTA++;


            } while (cepsilon >= emax);

            dogruCizSCPTA();
            MessageBox.Show(cyclecountSCPTA.ToString() + ". cycle", "cycle sayısı");
           
        }




        /// <summary>
        /// RCPTA ///
        /// </summary>
        //// RCPTA değişkenleri
        int w11 = 1, w12 = -2, w13 = 0;
        int w21 = 0, w22 = -1, w23 = 2;
        int w31 = 1, w32 = 3, w33 = -1;
        public static int[] d1 = new int[3] { 1, -1, -1 };
        public static int[] d2 = new int[3] { -1, 1, -1 };
        public static int[] d3 = new int[3] { -1, -1, 1 };
        int Ro1 = 0, Ro2 = 0, Ro3 = 0;
        int net1 = 0;
        int net2 = 0;
        int net3 = 0;
        int repsilon = 1;
        int cyclecountRCPTA = 0;

        private void dogruCizRCPTA1()
        {
            Graphics netgraph = CreateGraphics();
            Brush netgraphsbrush = new SolidBrush(Color.DarkGreen);
            Pen netpen = new Pen(netgraphsbrush, 5);

            if(w12 != 0)
            {
                netgraph.DrawLine(netpen, 0, (((10 * w13) + (300 * w12) - (400 * w11)) / w12), 800, (((10 * w13) + (400 * w11) + (300 * w12)) / w12));
            }
            else netgraph.DrawLine(netpen, ((-10 * w13) + 400 * w11) / w11, 0, ((-10 * w13) + 400 * w11) / w11, 600);


        }
        private void dogruCizRCPTA2()
        {
            Graphics netgraph = CreateGraphics();
            Brush netgraphsbrush = new SolidBrush(Color.Red);
            Pen netpen = new Pen(netgraphsbrush, 5);

            if (w22 != 0)
            {
                netgraph.DrawLine(netpen, 0, (((10 * w23) + (300 * w22) - (400 * w21)) / w22), 800, (((10 * w23) + (400 * w21) + (300 * w22)) / w22));
            }
            else netgraph.DrawLine(netpen, ((-10 * w23) + 400 * w21) / w21, 0, ((-10 * w23) + 400 * w21) / w21, 600);
        }
        private void dogruCizRCPTA3()
        {
            Graphics netgraph = CreateGraphics();
            Brush netgraphsbrush = new SolidBrush(Color.Blue);
            Pen netpen = new Pen(netgraphsbrush, 5);

            if (w32 != 0)
            {
                netgraph.DrawLine(netpen, 0, (((10 * w33) + (300 * w32) - (400 * w31)) / w32), 800, (((10 * w33) + (400 * w31) + (300 * w32)) / w32));
            }
            else netgraph.DrawLine(netpen, ((-10 * w33) + 400 * w31) / w31, 0, ((-10 * w33) + 400 * w31) / w31, 600);
        }
        private void dogruHesaplaRCPTA(int a, int b, int etiket)
        {
            net1 = w11 * a + w12 * b + w13;
            net2 = w21 * a + w22 * b + w23;
            net3 = w31 * a + w32 * b + w33;

            if (net1 > 0) { Ro1 = 1; }
            else { Ro1 = -1; }

            if (net2 > 0) { Ro2 = 1; }
            else { Ro2 = -1; }

            if (net3 > 0) { Ro3 = 1; }
            else { Ro3 = -1; }

            if (etiket == -1) ////class1 örneği
            {
                if (Ro1 != d1[0])
                {
                    w11 = w11 + ((d1[0] - Ro1) * a) / 2;
                    w12 = w12 + ((d1[0] - Ro1) * b) / 2;
                    w13 = w13 + (d1[0] - Ro1) / 2;

                    repsilon = 1;
                }

                if (Ro2 != d1[1])
                {
                    w21 = w21 + ((d1[1] - Ro2) * a) / 2;
                    w22 = w22 + ((d1[1] - Ro2) * b) / 2;
                    w23 = w23 + (d1[1] - Ro2) / 2;

                    repsilon = 1;
                }

                if (Ro3 != d1[2])
                {
                    w31 = w31 + ((d1[2] - Ro3) * a) / 2;
                    w32 = w32 + ((d1[2] - Ro3) * b) / 2;
                    w33 = w33 + (d1[2] - Ro3) / 2;

                    repsilon = 1;
                }
            }

            if (etiket == 0) ////class2 örneği
            {
                if (Ro1 != d2[0])
                {
                    w11 = w11 + ((d2[0] - Ro1) * a) / 2;
                    w12 = w12 + ((d2[0] - Ro1) * b) / 2;
                    w13 = w13 + (d2[0] - Ro1) / 2;

                    repsilon = 1;
                }

                if (Ro2 != d2[1])
                {
                    w21 = w21 + ((d2[1] - Ro2) * a) / 2;
                    w22 = w22 + ((d2[1] - Ro2) * b) / 2;
                    w23 = w23 + (d2[1] - Ro2) / 2;

                    repsilon = 1;
                }

                if (Ro3 != d2[2])
                {
                    w31 = w31 + ((d2[2] - Ro3) * a) / 2;
                    w32 = w32 + ((d2[2] - Ro3) * b) / 2;
                    w33 = w33 + (d2[2] - Ro3) / 2;

                    repsilon = 1;
                }
            }

            if (etiket == 1) ////class3 örneği
            {
                if (Ro1 != d3[0])
                {
                    w11 = w11 + ((d3[0] - Ro1) * a) / 2;
                    w12 = w12 + ((d3[0] - Ro1) * b) / 2;
                    w13 = w13 + (d3[0] - Ro1) / 2;

                    repsilon = 1;
                }

                if (Ro2 != d3[1])
                {
                    w21 = w21 + ((d3[1] - Ro2) * a) / 2;
                    w22 = w22 + ((d3[1] - Ro2) * b) / 2;
                    w23 = w23 + (d3[1] - Ro2) / 2;

                    repsilon = 1;
                }

                if (Ro3 != d3[2])
                {
                    w31 = w31 + ((d3[2] - Ro3) * a) / 2;
                    w32 = w32 + ((d3[2] - Ro3) * b) / 2;
                    w33 = w33 + (d3[2] - Ro3) / 2;

                    repsilon = 1;
                }
            }

        }
        private void trainingRCPTA()
        {
            while (repsilon != 0)
            {
                repsilon = 0;
                if (counter1 >= counter2 && counter1 >= counter3)
                {
                    for (int i = 0; i < counter1; i++)
                    {
                        dogruHesaplaRCPTA(class1Member[i, 0], class1Member[i, 1], -1);
                        if (i < counter2)
                        {
                            dogruHesaplaRCPTA(class2Member[i, 0], class2Member[i, 1], 0);
                        }
                        if (i < counter3)
                        {
                            dogruHesaplaRCPTA(class3Member[i, 0], class3Member[i, 1], 1);
                        }
                    }
                }
                else if (counter2 >= counter1 && counter2 >= counter3)
                {
                    for (int i = 0; i < counter2; i++)
                    {
                        if (i < counter1)
                        {
                            dogruHesaplaRCPTA(class1Member[i, 0], class1Member[i, 1], -1);
                        }

                        dogruHesaplaRCPTA(class2Member[i, 0], class2Member[i, 1], 0);
                        
                        if (i < counter3)
                        {
                            dogruHesaplaRCPTA(class3Member[i, 0], class3Member[i, 1], 1);
                        }
                    }
                }
                else if (counter3 >= counter2 && counter3 >= counter1)
                {
                    for (int i = 0; i < counter2; i++)
                    {
                        if (i < counter1)
                        {
                            dogruHesaplaRCPTA(class1Member[i, 0], class1Member[i, 1], -1);
                        }

                        if (i < counter2)
                        {
                            dogruHesaplaRCPTA(class2Member[i, 0], class2Member[i, 1], 0);
                        }
                        
                        dogruHesaplaRCPTA(class3Member[i, 0], class3Member[i, 1], 1);
                        
                    }
                }
                cyclecountRCPTA++;
            }
            dogruCizRCPTA1();
            dogruCizRCPTA2();
            dogruCizRCPTA3();
        }


        ////class1 elemanlarını gösteren button
        private void button1_Click(object sender, EventArgs e)
        {
            Form cls1 = new cls1members();
            cls1.Show();
        }
        ////class2 elemanlarını gösteren button
        private void button2_Click(object sender, EventArgs e)
        {
            Form cls2 = new cls2members();
            cls2.Show();
        }
        ////class3 elemanlarını gösteren button
        private void button3_Click(object sender, EventArgs e)
        {
            Form cls3 = new cls3members();
            cls3.Show();
        }
        ////koordinat düzlemini çizdiren button
        private void button4_Click(object sender, EventArgs e)
        {
            koordinatSistemi();
        }
        ////trainingSDPTA
        private void button5_Click(object sender, EventArgs e)
        {
            trainingSDPTA();
        }        
        ////trainingSCPTA
        private void button6_Click(object sender, EventArgs e)
        {
            trainingSCPTA();
        }
        ////trainingRCPTA
        private void button7_Click(object sender, EventArgs e)
        {
            trainingRCPTA();
        }


        

        protected override void OnMouseClick(MouseEventArgs e)
        {
            

            ////
            ////If Class 1 selected
            ////
            if (class1.Checked)
            {
                if (counter1 < 50)
                {
                    base.OnMouseClick(e);
                    int x = 0, y = 0;
                    Graphics g = CreateGraphics();
                    Pen p = new Pen(Color.Green);
                    x = e.X; y = e.Y;                                   /////
                    int kalanx = x % 10;                                /////   feature koordinatları ötelemesi
                    if (kalanx < 5) { x = x - kalanx; }                 /////   X Y ötelemesi
                    if (kalanx >= 5) { x = x + 10 - kalanx; }           /////
                    int kalany = y % 10;                                /////
                    if (kalany < 5) { y = y - kalany; }                 /////
                    if (kalany >= 5) { y = y + 10 - kalany; }           /////
                    g.DrawLine(p, (x + 5), (y - 5), (x - 5), (y + 5));
                    g.DrawLine(p, (x - 5), (y - 5), (x + 5), (y + 5));

                    class1Member[counter1, 0] = (x-400)/10;
                    class1Member[counter1, 1] = (300-y)/10;

                    counter1++;
                }
                else
                    MessageBox.Show("class 1 elemanları doldu!", "Bilgilendirme Penceresi");
            }

            ////
            ////If Class 2 selected
            ////
            else if (class2.Checked)
            {
                if (counter2 < 50)
                {
                    base.OnMouseClick(e);
                    int x = 0, y = 0;
                    Graphics g = CreateGraphics();
                    Pen p = new Pen(Color.Red);
                    x = e.X; y = e.Y;                               /////
                    int kalanx = x % 10;                            /////
                    if (kalanx < 5) { x = x - kalanx; }             /////feature koordinatları öteleme
                    if (kalanx >= 5) { x = x + 10 - kalanx; }       /////
                    int kalany = y % 10;                            /////
                    if (kalany < 5) { y = y - kalany; }             /////
                    if (kalany >= 5) { y = y + 10 - kalany; }       /////
                    g.DrawRectangle(p,x-4,y-4,8,8);                 ////kare çizdiriyor!

                    class2Member[counter2, 0] = (x - 400) / 10;
                    class2Member[counter2, 1] = (300 - y) / 10;

                    counter2++;
                }
                else
                    MessageBox.Show("class 2 elemanları doldu!", "Bilgilendirme Penceresi");
            }

            ////
            ////If Class 3 selected
            ////
            else if (class3.Checked)
            {
                if (counter3 < 50)
                {
                    base.OnMouseClick(e);
                    int x = 0, y = 0;
                    Graphics g = CreateGraphics();
                    Pen p = new Pen(Color.Blue);
                    x = e.X; y = e.Y;                                /////                              
                    int kalanx = x % 10;                             /////
                    if (kalanx < 5) { x = x - kalanx - 4; }          /////feature koordinat öteleme  
                    if (kalanx >= 5) { x = x + 10 - kalanx - 4; }    /////                       
                    int kalany = y % 10;                             /////                           
                    if (kalany < 5) { y = y - kalany - 4; }          /////  
                     if (kalany >= 5) { y = y + 10 - kalany - 4; }    /////       
                    g.DrawEllipse(p, x, y, 8, 8);

                    class3Member[counter3, 0] = ((x+4) - 400) / 10;
                    class3Member[counter3, 1] = (300 - (y+4)) / 10;
                  
                    counter3++;
                }
                else
                    MessageBox.Show("class 3 elemanları doldu!", "Bilgilendirme Penceresi");

            }

            ////
            ////sınıf seçilmeme durumu/////
            ////
            else
            {
                MessageBox.Show("Lütfen Bir Class Seçiniz.", "Bilgilendirme Peceresi");
            }
        }
        
    }
}