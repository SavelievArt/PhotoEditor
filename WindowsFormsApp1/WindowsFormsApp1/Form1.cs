using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        Image img; Boolean imgOpen = false;

        void update()
        {
            if(imgOpen)
            {
                img = Image.FromFile(openImageDialog.FileName);
                pictureBox1.Image = img;
            }
        }

        void setColor()
        {
            float red = trackBar3.Value * 0.1f;
            float green = trackBar4.Value * 0.1f;
            float blue = trackBar5.Value * 0.1f;
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {1+red, 0, 0, 0, 0},
                 new float[] {0, 1+green, 0, 0, 0},
                 new float[] {0, 0, 1+blue, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });
            setFilter(colorMatrix);
        }

        void setBrightness(int brightness)
        {
            Bitmap temp = (Bitmap)img;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    int cR = c.R + brightness;
                    int cG = c.G + brightness;
                    int cB = c.B + brightness;

                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;

                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;

                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bmap.SetPixel(i, j,
                    Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                }
            }
            img = (Bitmap)bmap.Clone();
        }

        void setContrast(double contrast)
        {
            Bitmap temp = (Bitmap)img;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bmap.SetPixel(i, j,
        Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
            img = (Bitmap)bmap.Clone();
        }

        void updateTrack()
        {
            if(trackBar1.Value != 0) setBrightness(trackBar1.Value);
            progressBar1.Value = 10;
            if (trackBar2.Value != 0) setContrast(trackBar2.Value);
            progressBar1.Value = 20;
            if (trackBar3.Value != 0 || trackBar4.Value != 0 || trackBar5.Value != 0) setColor();
            progressBar1.Value = 30;
        }

        void setFilter(ColorMatrix colorMatrix)
        {
            update();
            if (!imgOpen)
            {
                MessageBox.Show("Error");
                return;
            }
            Image newImg = pictureBox1.Image;
            Bitmap newBitmap = new Bitmap(img.Width, img.Height);

            Graphics g = Graphics.FromImage(newBitmap);

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(newImg, new Rectangle(0, 0, newImg.Width, newImg.Height),
               0, 0, newImg.Width, newImg.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            pictureBox1.Image = newBitmap;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openImageDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                img = Image.FromFile(openImageDialog.FileName);
                pictureBox1.Image = img; imgOpen = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            update();
            if (!imgOpen)
            {
                MessageBox.Show("Error, open image pls!");
                return;
            }


            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "|*.bmp";
            ImageFormat imageFormat = ImageFormat.Bmp;
            if(saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFile.FileName, imageFormat);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            update();
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });
            setFilter(colorMatrix);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            update();
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                        new float[]{.393f, .349f, .272f, 0, 0},
                        new float[]{.769f, .686f, .534f, 0, 0},
                        new float[]{.189f, .168f, .131f, 0, 0},
                        new float[]{0, 0, 0, 1, 0},
                        new float[]{0, 0, 0, 0, 1}
                });
            setFilter(colorMatrix);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            update();
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                    {
                            new float[]{-1, 0, 0, 0, 0},
                            new float[]{0, -1, 0, 0, 0},
                            new float[]{0, 0, -1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{1, 1, 1, 1, 1}
                    });
            setFilter(colorMatrix);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            update();
            float degrees = 45.0f;
            double r = degrees * System.Math.PI / 180;
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {(float)System.Math.Cos(r),
                (float)System.Math.Sin(r),0, 0, 0},
                new float[] {(float)-System.Math.Sin(r),
                (float)-System.Math.Cos(r),
                0, 0, 0},
                new float[] { .50f, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 1 }
            });
            setFilter(colorMatrix);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            update();
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { .50f, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 1 }}
            );
            setFilter(colorMatrix);
                
        }

        private void button6_Click(object sender, EventArgs e)
        {
            update();
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                 new float[] { 1, 0, 0, 0, 0 },
                    new float[] { 0, 0.8f, 0, 0, 0 },
                    new float[] { 0, 0, 0.5f, 0, 0 },
                    new float[] { 0, 0, 0, 0.5f, 0 },
                    new float[] { 0, 0, 0, 0, 1 } }
            );
            setFilter(colorMatrix);
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { .90f, .0f, .0f, .0f, 1 } }
            );
            setFilter(colorMatrix);
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            update();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] { 1+0.3f, 0, 0, 0, 0 },
                new float[] { 0, 1+0f, 0, 0, 0 },
                new float[] { 0, 0, 1+5f, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 1 } }
            );
            setFilter(colorMatrix);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if(!imgOpen)
            {
                MessageBox.Show("OpenFile!");
                return;
            }
            label5.Text = trackBar1.Value.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            trackBar4.Value = 0;
            trackBar5.Value = 0;
            progressBar1.Value = 0;
            update();
        }

        private void trackBar2_SystemColorsChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            updateTrack();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (!imgOpen)
            {
                MessageBox.Show("OpenFile!");
                return;
            }
            label6.Text = trackBar2.Value.ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            if (!imgOpen)
            {
                MessageBox.Show("OpenFile!");
                return;
            }
            label8.Text = trackBar3.Value.ToString();
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            if (!imgOpen)
            {
                MessageBox.Show("OpenFile!");
                return;
            }
            label9.Text = trackBar4.Value.ToString();
        }

        private void trackBar5_ValueChanged(object sender, EventArgs e)
        {
            if (!imgOpen)
            {
                MessageBox.Show("OpenFile!");
                return;
            }
            label10.Text = trackBar5.Value.ToString();
        }
    }
}
