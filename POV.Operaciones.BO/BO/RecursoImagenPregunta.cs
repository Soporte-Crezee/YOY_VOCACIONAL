using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POV.Operaciones.BO
{
    public class RecursoImagenPregunta
    {
        public byte[] ImageData { get; set; }
        public int ImageLenght { get; set; }
        public string ImageType { get; set; }
        public string ImageName { get; set; }

        public RecursoImagenPregunta() { }
        public RecursoImagenPregunta(Stream img_strm, int img_len, string strtype, string name)
        {
            byte[] imgdata = new byte[img_len];
            int n = img_strm.Read(imgdata, 0, img_len);


            this.ImageData = imgdata;
            this.ImageLenght = img_len;
            this.ImageName = name;
            this.ImageType = strtype;

        }        
    }
}
