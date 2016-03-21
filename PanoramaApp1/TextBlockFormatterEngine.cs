using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PanoramaApp1
{
    public class TextBlockFormatterEngine
    {
        private TextBlock measureBlock;
        private const double maxHeight = 2048;

        public FontFamily FontFamily { get; set; }

        private TextBlockFormatterEngine()
        {
            measureBlock = GenerateTextBlock();
        }

        private static TextBlockFormatterEngine instance;
        public static TextBlockFormatterEngine Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextBlockFormatterEngine();
                return instance;
            }
        }

        private TextBlock GenerateTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Margin = new Thickness(10);
            return textBlock;
        }

        public IList<string> Split(string value, double fontSize, FontWeight fontWeight, double screenWidth)
        {

            value = CleanUpText(value);

            List<string> parsedText = new List<string>();
            StringReader reader = new StringReader(value);
            measureBlock.FontSize = fontSize;
            measureBlock.FontWeight = fontWeight;
            measureBlock.Width = screenWidth;

            int maxTextCount = this.GetMaxTextSize();

            if (value.Length < maxTextCount)
            {
                parsedText.Add(value);
            }
            else
            {
                while (reader.Peek() > 0)
                {
                    string line = reader.ReadLine();
                    parsedText.AddRange(ParseLine(line, maxTextCount));
                }
            }
            return parsedText;
        }

        private string CleanUpText(string value)
        {
            value = value.Replace("<BR>", "\n");
            value = value.Replace("&nbsp;", " ");

            value = value.Replace(@"<font class=""EnglishComment"">", "(");
            value = value.Replace(@"<font class=""CopticComment"">", "(");
            value = value.Replace(@"<font class=""ArabicComment"">", "(");
            value = value.Replace("</font>",")");
            value = value.Replace(@"<comment=", "");

            value = value.Replace(@">", ": "); 

            return value;
        }

        private IList<string> ParseLine(string line, int maxTextCount)
        {
            int maxLineCount = GetMaxLineCount();
            string tempLine = line;
            var parsedText = new List<string>();

            try
            {
                while (tempLine.Length > 0)
                {
                    int charactersFitted = GetCharactersThatFit(tempLine, maxTextCount);
                    parsedText.Add(tempLine.Substring(0, charactersFitted));
                    tempLine = tempLine.Substring(charactersFitted, tempLine.Length - (charactersFitted));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return parsedText;
        }

        private int GetCharactersThatFit(string text, int maxTextCount)
        {
            int maxLineLength = maxTextCount > text.Length ? text.Length : maxTextCount;
            for (int i = maxLineLength - 1; i > 1; i--)
            {
                if (text[i] == ' ')
                {
                    var nHeight = MeasureString(text.Substring(0, i - 1)).Height;
                    if (nHeight <= maxHeight)
                        return i;
                }
            }
            return maxLineLength;
        }

        private Size MeasureString(string text)
        {
            this.measureBlock.Text = text;
            return new Size(measureBlock.ActualWidth, measureBlock.ActualHeight);
        }

        private int GetMaxTextSize()
        {
            // Get average char size
            Size size = this.MeasureText(" ");
            // Get number of char that fit in the line
            int charLineCount = (int)(measureBlock.Width / size.Width);
            // Get line count
            int lineCount = (int)(maxHeight / size.Height);

            return charLineCount * lineCount / 2;
        }

        private int GetMaxLineCount()
        {
            Size size = this.MeasureText(" ");
            // Get number of char that fit in the line
            int charLineCount = (int)(measureBlock.Width / size.Width);
            // Get line count
            int lineCount = (int)(maxHeight / size.Height) - 5;

            return lineCount;
        }

        private Size MeasureText(string value)
        {
            measureBlock.Text = value;
            return new Size(measureBlock.ActualWidth, measureBlock.ActualHeight);
        }
    }
}