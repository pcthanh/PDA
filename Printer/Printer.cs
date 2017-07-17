using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Printer
{
   public class Printer
    {
       public PrintDocument printDocument;
       
        public object Tag { get; set; }
        public Printer()
        {
            printDocument = new PrintDocument();
            printDocument.PrintController = new StandardPrintController();
        }
        public void Print()
        {
            printDocument.Print();
        }
        public void SetPrinterName(string name)
        {
            printDocument.PrinterSettings.PrinterName = name;
        }

        public int GetHeightPrinterLine()
        {
            //if (readconfig.PrinterModel == "PRP-085")
            //{
            //    return 100;
            //}
            //else
            //{
            //    return 10;
            //}
            return 100;
        }


        public float DrawString(string s, System.Drawing.Printing.PrintPageEventArgs e, System.Drawing.Font font, float y, int textAlign)
        {
            float x;
            List<string> list = SplitStringLine(s, e, font);
            foreach (string item in list)
            {
                if (textAlign == 1)
                {
                    x = 0;
                }
                else if (textAlign == 2)
                {
                    x = (float)Math.Abs(((float)e.PageBounds.Width - e.Graphics.MeasureString(item, font).Width) / 2);
                }
                else
                {
                    x = e.PageBounds.Width - e.Graphics.MeasureString(item, font).Width;
                }
                e.Graphics.DrawString(item, font, System.Drawing.Brushes.Black, x, y);
                y += e.Graphics.MeasureString(item, font).Height;
            }

            return y;
        }
        private List<string> SplitStringLine(string str, System.Drawing.Printing.PrintPageEventArgs e, System.Drawing.Font font)
        {
            List<string> list = new List<string>();
            string[] s = str.Split(' ');
            string resuilt = "";
            //float width = e.Graphics.MeasureString("ADD",font).Width;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length > 0)
                {
                    if (e.Graphics.MeasureString(resuilt + s[i], font).Width > e.PageBounds.Width && resuilt.Length > 0)
                    {
                        list.Add(resuilt);
                        i--;
                        resuilt = "";
                    }
                    else if (e.Graphics.MeasureString(s[i], font).Width > e.PageBounds.Width)
                    {
                        list.Add(s[i]);
                        resuilt = "";
                    }
                    else
                    {
                        resuilt += s[i] + " ";
                    }
                }
            }
            if (resuilt.Length > 0)
            {
                list.Add(resuilt);
            }
            return list;
        }
        public void DrawCancelLine(System.Drawing.Printing.PrintPageEventArgs e, float y_start, float y_end)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Brushes.Black);
            e.Graphics.DrawLine(pen, 0, y_start, e.PageBounds.Width, y_end);
            e.Graphics.DrawLine(pen, 0, y_end, e.PageBounds.Width, y_start);
        }
        public float DrawLine(string s, System.Drawing.Font font, System.Drawing.Printing.PrintPageEventArgs e, System.Drawing.Drawing2D.DashStyle dashStyle, float y, int textAlign)
        {
            float x;
            float width;
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Brushes.Black);
            if (dashStyle != System.Drawing.Drawing2D.DashStyle.Custom)
            {
                pen.DashStyle = dashStyle;
            }
            if (s == "" || s == null)
            {
                width = e.PageBounds.Width;
            }
            else
            {
                width = e.Graphics.MeasureString(s, font).Width;
            }
            if (textAlign == 1)
            {
                x = 0;
            }
            else if (textAlign == 2)
            {
                x = (float)Math.Abs(((float)e.PageBounds.Width - e.Graphics.MeasureString(s, font).Width) / 2);
            }
            else
            {
                x = e.PageBounds.Width - e.Graphics.MeasureString(s, font).Width;
            }
            e.Graphics.DrawLine(pen, x, y, x + width, y);
            y += 2;
            return y;
        }

        //public float DrawBarcode(string dataBarcode, System.Drawing.Printing.PrintPageEventArgs e, float y,int align)
        //{            
        //    Linear barcode = new Linear();

        //    // EAN 13 Barcode Basic Settings
        //    barcode.Type = BarcodeType.EAN13;

        //    /*
        //       EAN 13 Valid data char set:
        //                0, 1, 2, 3, 4, 5, 6, 7, 8, 9 (Digits)

        //       EAN 13 Valid data length: 12 digits only, excluding the last checksum digit
        //    */
        //    barcode.Data = dataBarcode;
        //    barcode.AddCheckSum = true;            
        //    // for EAN13 with supplement data (2 or 5 digits)
        //    /*
        //    barcode.SupData = "12";
        //    // supplement bar height vs bar height ratio
        //    barcode.SupHeight = 0.8f;
        //    // space between barcode and supplement barcode (in pixel)
        //    barcode.SupSpace = 15;
        //    */            
        //    // Barcode Size Related Settings
        //    barcode.UOM = UnitOfMeasure.PIXEL;            
        //    barcode.X = 1;
        //    barcode.Y = 40;
        //    barcode.LeftMargin = 0;
        //    barcode.RightMargin = 0;
        //    barcode.TopMargin = 0;
        //    barcode.BottomMargin = 0;
        //    barcode.Resolution = 96;
        //    barcode.Rotate = Rotate.Rotate0;
        //    barcode.BarAlignment = AlignmentHori.Center;
        //    // Barcode Text Settings
        //    barcode.ShowText = true;
        //    barcode.TextFont = new Font("Arial", 9f, FontStyle.Regular);
        //    barcode.TextMargin = 6;            

        //    Image img= (Image)barcode.drawBarcode();
        //    float x = 0;

        //    if (align == 1)
        //    {
        //        x = 0;
        //    }
        //    else if (align == 2)
        //    {
        //        x = e.PageBounds.Width - img.Width;
        //        if (x < 0)
        //        {
        //            x = 0;
        //        }
        //        x = x / 2;
        //    }
        //    else
        //    {
        //        x = e.PageBounds.Width - img.Width;
        //        if (x<0)
        //        {
        //            x = 0;
        //        }
        //    }
        //    e.Graphics.DrawImage(img, x, y);
        //    y += img.Height + 2;
        //    return y;
        //}
        //public float DrawBarcode(string dataBarcode, System.Drawing.Printing.PrintPageEventArgs e, float y, int align)
        //{
        //    //if (dataBarcode.Length != 12)
        //    //{
        //    //    y = DrawString("", e, new Font("Arial", 11), y, 1);
        //    //    return y;
        //    //}
        //    //Barcode.Ean13 ean13 = new Barcode.Ean13(dataBarcode);
        //    ////ean13.Width = 80;
        //    ////ean13.Height = 40;
        //    //float x = 0;

        //    //if (align == 1)
        //    //{
        //    //    x = 0;
        //    //}
        //    //else if (align == 2)
        //    //{
        //    //    x = e.PageBounds.Width - ean13.Width;
        //    //    if (x < 0)
        //    //    {
        //    //        x = 0;
        //    //    }
        //    //    x = x / 2;
        //    //}
        //    //else
        //    //{
        //    //    x = e.PageBounds.Width - ean13.Width;
        //    //    if (x < 0)
        //    //    {
        //    //        x = 0;
        //    //    }
        //    //}
        //    //ean13.DrawEan13Barcode(e.Graphics, new Point((int)x, (int)y));
        //    ////Image img = ean13.CreateBitmap();
        //    ////e.Graphics.DrawImage(img, 0, 0);
        //    ////e.Graphics.DrawImage(img, x, y);
        //    //y += ean13.Height + 2;
        //    ////ean13.DrawEan13Barcode(e.Graphics, new Point(0,0));
        //    //return y;
        //}
        //public float DrawBarcode1(string dataBarcode, System.Drawing.Printing.PrintPageEventArgs e, float y, int align)
        //{
        //    //Linear ean128 = new Linear();

        //    //// Barcode data to encode
        //    //ean128.Data = "ONBARCODE";
        //    //// Barcode symbology type
        //    //ean128.Type = BarcodeType.EAN128;
        //    //// Apply checksum digit for EAN-128 / GS1-128
        //    //ean128.AddCheckSum = true;

        //    ///*
        //    // * Barcode Image Related Settings
        //    // */
        //    //// Unit of meature for all size related setting in the library. 
        //    //ean128.UOM = UnitOfMeasure.PIXEL;
        //    //// Bar module width (X), default is 1 pixel;
        //    //ean128.X = 1;
        //    //// Bar module height (Y), default is 60 pixel;
        //    //ean128.Y = 60;
        //    //// Barcode image left, right, top, bottom margins. Defaults are 0.
        //    //ean128.LeftMargin = 0;
        //    //ean128.RightMargin = 0;
        //    //ean128.TopMargin = 0;
        //    //ean128.BottomMargin = 0;
        //    //// Image resolution in dpi, default is 72 dpi.
        //    //ean128.Resolution = 72;
        //    //// Created barcode orientation. 4 options are: facing left, facing right, facing bottom, and facing top
        //    //ean128.Rotate = Rotate.Rotate0;

        //    ///*
        //    // * Linear barcodes human readable text styles
        //    // */
        //    //// Display human readable text under the barcode
        //    //ean128.ShowText = true;
        //    //// Display checksum digit at the end of barcode data.
        //    //ean128.ShowCheckSumChar = true;
        //    //// Human readable text font size, font family and style
        //    //ean128.TextFont = new Font("Arial", 9f, FontStyle.Regular);
        //    //// Space between barcode and text. Default is 6 pixel.
        //    //ean128.TextMargin = 6;
        //    //Image img = (Image)ean128.drawBarcode();
        //    //float x = 0;

        //    //if (align == 1)
        //    //{
        //    //    x = 0;
        //    //}
        //    //else if (align == 2)
        //    //{
        //    //    x = e.PageBounds.Width - img.Width;
        //    //    if (x < 0)
        //    //    {
        //    //        x = 0;
        //    //    }
        //    //    x = x / 2;
        //    //}
        //    //else
        //    //{
        //    //    x = e.PageBounds.Width - img.Width;
        //    //    if (x < 0)
        //    //    {
        //    //        x = 0;
        //    //    }
        //    //}
        //    //e.Graphics.DrawImage(img, x, y);
        //    //y += img.Height + 2;
        //    //return y;
        //}
        public float DrawMessenge(string s, System.Drawing.Printing.PrintPageEventArgs e, System.Drawing.Font font, float y)
        {
            while (s.Contains("  "))
            {
                s = s.Replace("  ", " ");
            }
            string[] list = s.Split(' ');
            string resuilt = "";
            for (int i = 0; i < list.Length; i++)
            //foreach (string data in list)
            {
                string data = list[i];
                //resuilt += data;
                if (e.Graphics.MeasureString(resuilt + " " + data, font).Width > e.PageBounds.Width)
                {
                    y = DrawString(resuilt, e, font, y, 1);
                    resuilt = data;
                }
                else
                {
                    resuilt += data + " ";
                    if (i == (list.Length - 1))
                    {
                        y = DrawString(resuilt, e, font, y, 1);
                    }
                }
            }
            SizeF size = e.Graphics.MeasureString(s, font);
            int x = (int)size.Width;
            return y;
        }
        public void CanCelPrint()
        {
        }
    }
}
