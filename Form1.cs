using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThisImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap loadImage, resultImage, imageB, imageA, colorgreen;

        public Form1()
        {
            InitializeComponent();
            button10.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Basic Copy
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < loadImage.Width; x++) 
                for(int y = 0; y < loadImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    resultImage.SetPixel(x, y, pixel);
                }
            pictureBox2.Image = resultImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Grayscale
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    // pixel.R * .40 + pixel.G * .30 + pixel.B * .30

                    //Color myColor = Color.FromArgb(grey,grey,grey);

                    resultImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            pictureBox2.Image = resultImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Color Inversion
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    resultImage.SetPixel(x, y, Color.FromArgb(255-pixel.R, 255-pixel.G, 255-pixel.B));
                }
            pictureBox2.Image = resultImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Histogram
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    Color pixel = loadImage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;

                    resultImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }

            Color sample;
            int[] histdata = new int[256];

            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    sample = resultImage.GetPixel(x, y);    // 0-255 either r,g or b
                    histdata[sample.R] += 1;
                }

            // background for the graph
            Bitmap mydata = new Bitmap(256, 800); // Height can be any size
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 800; y++)
                {
                    mydata.SetPixel(x, y, Color.White);
                }

            // plot histdata
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < Math.Min(histdata[x]/5, 800); y++)
                {
                    mydata.SetPixel(x, 799-y, Color.Black);
                }
            pictureBox2.Image = mydata;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            resultImage.Save(saveFileDialog1.FileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Sepia
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int y = 0; y < loadImage.Height; y++)
            {
                for (int x = 0; x < loadImage.Width; x++)
                {
                    // get pixel value
                    Color pixel = loadImage.GetPixel(x, y);

                    // extract pixel component ARGB
                    int a = pixel.A;
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    // calculate temp value
                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    // set new RGB value
                    r = (tr > 255) ? 255 : tr;
                    g = (tg > 255) ? 255 : tg;
                    b = (tb > 255) ? 255 : tb;

                    // set the new RGB value in image pixel
                    resultImage.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox4.Image = imageB;
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox5.Image = imageA;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Image Subtraction PROCESS
            colorgreen = new Bitmap(imageB.Width, imageB.Height);
            Color mygreen = Color.FromArgb(0, 255, 0);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < imageB.Width; x++)
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);

                    if (subtractvalue > threshold)
                        colorgreen.SetPixel(x, y, pixel);
                    else
                        colorgreen.SetPixel(x, y, backpixel);
                }

            pictureBox3.Image = colorgreen;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Image Subtraction button
            // if this is clicked, all the other buttons and picture boxes will be hidden
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;

            // only visible are Image Subtraction things
            button10.Visible = true;
            button11.Visible = true;
            button12.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;
            pictureBox5.Visible = true;



        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
