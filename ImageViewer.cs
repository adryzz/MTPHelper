using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTPHelper
{
    public partial class ImageViewer : Form
    {
        Image Image;
        PictureBoxSizeMode Mode;

        /// <summary>
        /// A window that lets the user wiew an image.
        /// </summary>
        /// <param name="i">The image</param>
        /// <param name="m">The resize mode</param>
        public ImageViewer(Image i, PictureBoxSizeMode m)
        {
            InitializeComponent();
            Image = i;
            Mode = m;
        }

        /// <summary>
        /// A window that lets the user wiew an image.
        /// </summary>
        /// <param name="i">The image</param>
        public ImageViewer(Image i)
        {
            InitializeComponent();
            Image = i;
            Mode = PictureBoxSizeMode.Zoom;
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image;
            pictureBox1.SizeMode = Mode;
        }
    }
}
