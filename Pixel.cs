using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

/// This code is provided as-is, without any warrenty, so use it at your own risk.
/// You can freely use and modify this code.
namespace NetMFNeoPixelSPI
{

    /// <summary>
    /// Class representing one pixel<br />
    /// Highly inspired by frank26080115's NeoPixel class on NeoPixel-on-NetduinoPlus2 @ github
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// Green, 0 to 255
        /// </summary>
        public byte Green
        {
            get;
            set;
        }

        /// <summary>
        /// Red, 0 to 255
        /// </summary>
        public byte Red
        {
            get;
            set;
        }

        /// <summary>
        /// Blue, 0 to 255
        /// </summary>
        public byte Blue
        {
            get;
            set;
        }

        /// <summary>
        /// Color (Microsoft.SPOT.Presentation.Media) of this pixel
        /// </summary>
        public Color Color
        {
            get
            {
                return ColorUtility.ColorFromRGB(this.Red, this.Green, this.Blue);
            }

            set
            {
                this.Green = ColorUtility.GetGValue(value);
                this.Red = ColorUtility.GetRValue(value);
                this.Blue = ColorUtility.GetBValue(value);
            }
        }

        /// <summary>
        /// Creates a new pixel, black
        /// </summary>
        public Pixel()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Creates a pixel with an initial color
        /// </summary>
        /// <param name="c">Initial color</param>
        public Pixel(Color c)
        {
            this.Green = ColorUtility.GetGValue(c);
            this.Red = ColorUtility.GetRValue(c);
            this.Blue = ColorUtility.GetBValue(c);
        }

        /// <summary>
        /// Creates a new pixel with given color
        /// </summary>
        /// <param name="r">Initial red, 0 to 255</param>
        /// <param name="g">Initial green, 0 to 255</param>
        /// <param name="b">Initial blue, 0 to 255</param>
        public Pixel(byte r, byte g, byte b)
        {
            this.Green = g;
            this.Red = r;
            this.Blue = b;
        }

        /// <summary>
        /// Creates a new pixel with given color
        /// </summary>
        /// <param name="r">Initial red, 0 to 255</param>
        /// <param name="g">Initial green, 0 to 255</param>
        /// <param name="b">Initial blue, 0 to 255</param>
        public Pixel(int r, int g, int b)
        {
            this.Green = (byte)g;
            this.Red = (byte)r;
            this.Blue = (byte)b;
        }

        /// <summary>
        /// Creates a new pixel with given ARGB color, where A is ignored
        /// </summary>
        /// <param name="argb">ARGB color value</param>
        public Pixel(int argb)
        {
            this.Green = (byte)(argb & 0x0000FF00);
            this.Red = (byte)(argb & 0x00FF0000);
            this.Blue = (byte)(argb & 0x000000FF);
        }

        /// <summary>
        /// Creates the bytes needed for transfer via SPI in GRB format<br />
        /// Make sure that zero and one bytes have the same length and are properly initialized
        /// </summary>
        /// <param name="zeroBytes">bytes for zero bit</param>
        /// <param name="oneBytes">bytes for one bit</param>
        /// <returns>transfer bytes</returns>
        public byte[] ToTransferBytes(byte[] zeroBytes, byte[] oneBytes)
        {
            if ((zeroBytes == null) || (zeroBytes.Length == 0) || (oneBytes == null) || (oneBytes.Length == 0))
            {
                return new byte[0];
            }
            int len = zeroBytes.Length;
            if (oneBytes.Length != len)
            {
                return new byte[0];
            }

            byte[] result = new byte[24 * len];

            int pos = 0;
            byte msk;

            msk = 128;
            for (int i = 7; i >= 0; i--)
            {
                byte v = (byte)(this.Green & msk);
                if (v > 0)
                {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else
                {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            msk = 128;
            for (int i = 7; i >= 0; i--)
            {
                byte v = (byte)(this.Red & msk);
                if (v > 0)
                {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else
                {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            msk = 128;
            for (int i = 7; i >= 0; i--)
            {
                byte v = (byte)(this.Blue & msk);
                if (v > 0)
                {
                    Array.Copy(oneBytes, 0, result, pos, len);
                }
                else
                {
                    Array.Copy(zeroBytes, 0, result, pos, len);
                }
                pos += len;
                msk = (byte)(msk >> 1);
            }

            return result;
        }

    }

}
