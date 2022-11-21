using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace IngenicoOKC.Class
{
    class DisplayClass
    {
        public static IngenicoCase dispCls;

        public void useImagesFromResources(PictureBox m_pbControl, string Resourcename)
        {
            ResourceManager resManager = new ResourceManager("GmpSampleSim.Resource1", Assembly.GetExecutingAssembly());
            m_pbControl.Image = (Bitmap)resManager.GetObject(Resourcename);
            resManager.ReleaseAllResources();
        }
    }
}
