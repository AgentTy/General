using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace General.Drawing
{
    public class ImageHandler : IDisposable
    {
        public Graphics Stage;
        public Bitmap Image;
        public Rectangle Bounds;

        #region SaveJPEG
        public void SaveJPEG(string strFileName)
        {
            Rendering.SaveJPEG(this, strFileName);
        }

        public void SaveJPEG(string strFileName, int intSizePercent)
        {
            Rendering.SaveJPEG(this, strFileName, intSizePercent);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            if(Image != null)
                Image.Dispose();

            if(Stage != null)
                Stage.Dispose();

            Image = null;
            Stage = null;
            System.GC.Collect();
        }

        #endregion
    }
}
