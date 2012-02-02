using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;

namespace Util
{
    /// <summary>
    /// Summary description for Imagem
    /// </summary>
    public class Imagem
    {
        public class Size
        {
            public enum tipoDimensao
            {
                Width,
                Height
            }

            public double Width { get; set; }
            public double Height { get; set; }

            public Size() { }

            public Size(double width, double height)
            {
                this.Width = width;
                this.Height = height;
            }

            public Size(double valor, tipoDimensao tipo)
            {
                if (tipo == tipoDimensao.Width)
                    this.Width = valor;
                else if (tipo == tipoDimensao.Height)
                    this.Height = valor;
            }

        }

        public enum EnumFormato
        {
            GIF,
            JPEG,
            PNG
        }

        public static void Redimensionar(System.IO.Stream fileStream, String caminhoFinal, Size tamanho, EnumFormato formato)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromStream(fileStream);

            Redimensionar(original, caminhoFinal, tamanho, formato);
        }

        public static void Redimensionar(System.IO.Stream fileStream, String caminhoFinal, Size tamanho, EnumFormato formato, bool manterProporcao)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromStream(fileStream);

            Redimensionar(original, caminhoFinal, tamanho, formato, manterProporcao);
        }

        public static void Redimensionar(System.IO.Stream fileStream, String caminhoFinal, Size tamanho)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromStream(fileStream);

            Redimensionar(original, caminhoFinal, tamanho, EnumFormato.JPEG);
        }

        public static void Redimensionar(System.IO.Stream fileStream, String caminhoFinal, Size tamanho, bool manterProporcao)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromStream(fileStream);

            Redimensionar(original, caminhoFinal, tamanho, EnumFormato.JPEG, manterProporcao);
        }

        public static void Redimensionar(String caminhoOriginal, String caminhoFinal, Size tamanho, EnumFormato formato)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);

            Redimensionar(original, caminhoFinal, tamanho, formato);
        }

        public static void Redimensionar(String caminhoOriginal, String caminhoFinal, Size tamanho, EnumFormato formato, bool manterProporcao)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);

            Redimensionar(original, caminhoFinal, tamanho, formato, manterProporcao);
        }

        public static void Redimensionar(String caminhoOriginal, Size tamanho, EnumFormato formato)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);
            string caminhoFinal = caminhoOriginal;

            Redimensionar(original, caminhoFinal, tamanho, formato);
        }

        public static void Redimensionar(String caminhoOriginal, Size tamanho, EnumFormato formato, bool manterProporcao)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);
            string caminhoFinal = caminhoOriginal;

            Redimensionar(original, caminhoFinal, tamanho, formato, manterProporcao);
        }

        public static void Redimensionar(String caminhoOriginal, String caminhoFinal, Size tamanho)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);

            Redimensionar(original, caminhoFinal, tamanho, EnumFormato.JPEG);
        }

        public static void Redimensionar(String caminhoOriginal, String caminhoFinal, Size tamanho, bool manterProporcao)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);

            Redimensionar(original, caminhoFinal, tamanho, EnumFormato.JPEG, manterProporcao);
        }

        public static void Redimensionar(String caminhoOriginal, Size tamanho)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);
            string caminhoFinal = caminhoOriginal;

            Redimensionar(original, caminhoFinal, tamanho, EnumFormato.JPEG);
        }

        public static void Redimensionar(String caminhoOriginal, Size tamanho, bool manterProporcao)
        {
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(caminhoOriginal);
            string caminhoFinal = caminhoOriginal;

            Redimensionar(original, caminhoFinal, tamanho, EnumFormato.JPEG, manterProporcao);
        }

        private static void Redimensionar(Bitmap original, String caminhoFinal, Size tamanho, EnumFormato formato)
        {
            Redimensionar(original, caminhoFinal, tamanho, formato, true);
        }

        private static void Redimensionar(Bitmap original, String caminhoFinal, Size tamanho, EnumFormato formato, bool manterProporcao)
        {
            double maxWidth = (tamanho.Width != 0) ? tamanho.Width : original.Width;
            double maxHeight = (tamanho.Height != 0) ? tamanho.Height : original.Height;

            caminhoFinal = Util.Url.GetCaminhoFisico(caminhoFinal);

            if (manterProporcao)
            {
                Double widthAtual, heightAtual;
                Double diferenca, diferencaW, diferencaH;

                widthAtual = original.Width;
                heightAtual = original.Height;

                diferencaW = maxWidth / widthAtual;
                diferencaH = maxHeight / heightAtual;

                if (diferencaH < diferencaW)
                    diferenca = diferencaH;
                else
                    diferenca = diferencaW;

                maxHeight = heightAtual * diferenca;
                maxWidth = widthAtual * diferenca;
            }

            Bitmap imgFinal = new Bitmap((int)(maxWidth), (int)(maxHeight));

            // Redimensiona imagem
            Graphics g = Graphics.FromImage(imgFinal);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(original, new Rectangle(0, 0, imgFinal.Width, imgFinal.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //qualidade 
            ImageCodecInfo encoderFinal = null;
            foreach (ImageCodecInfo encoder in ImageCodecInfo.GetImageEncoders())
            {
                if (encoder.FormatDescription == formato.ToString())
                {
                    encoderFinal = encoder;
                    break;
                }
            }
            // Setup to give the encoder the quality parameter
            EncoderParameters parms = new EncoderParameters(1);
            EncoderParameter qparam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (Int64)90);
            parms.Param[0] = qparam;

            original.Dispose();

            if (System.IO.File.Exists(caminhoFinal))
            {
                System.IO.File.Delete(caminhoFinal);
            }

            // Grava imagem redimensionada como Jpeg
            imgFinal.Save(caminhoFinal, encoderFinal, parms);

            imgFinal.Dispose();
        }
    }
}
