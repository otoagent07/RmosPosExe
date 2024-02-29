using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Drawing;
using System.Reflection;

namespace RmosIngenicoGMP
{
    class DisplayClass
    {
        public static Main dispCls;

        public void useImagesFromResources(PictureBox m_pbControl, string Resourcename)
        {
            ResourceManager resManager = new ResourceManager("GmpSampleSim.Resource1", Assembly.GetExecutingAssembly());
            m_pbControl.Image = (Bitmap)resManager.GetObject(Resourcename);
            resManager.ReleaseAllResources();
        }
    }
}
