namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Reflection;

    public class CustomChart
    {
        public class BarGraph : CustomChart.Chart
        {
            private Color _backColor;
            private float _barWidth;
            private float _bottomBuffer;
            private bool _displayBarData;
            private bool _displayLegend;
            private Color _fontColor;
            private string _fontFamily;
            private float _graphHeight;
            private const float _graphLegendSpacer = 15f;
            private float _graphWidth;
            private const int _labelFontSize = 7;
            private const int _legendFontSize = 9;
            private const float _legendRectangleSize = 10f;
            private float _legendWidth;
            private string _longestLabel;
            private string _longestTickValue;
            private float _maxLabelWidth;
            private float _maxTickValueWidth;
            private float _maxValue;
            private float _scaleFactor;
            private float _spaceBtwBars;
            private const float _spacer = 5f;
            private float _topBuffer;
            private float _totalHeight;
            private float _totalWidth;
            private float _xOrigin;
            private string _yLabel;
            private float _yOrigin;
            private int _yTickCount;
            private float _yTickValue;

            public BarGraph()
            {
                this._longestTickValue = string.Empty;
                this._maxValue = 0f;
                this._longestLabel = string.Empty;
                this._maxLabelWidth = 0f;
                this.AssignDefaultSettings();
            }

            public BarGraph(Color bgColor)
            {
                this._longestTickValue = string.Empty;
                this._maxValue = 0f;
                this._longestLabel = string.Empty;
                this._maxLabelWidth = 0f;
                this.AssignDefaultSettings();
                this.BackgroundColor = bgColor;
            }

            private void AssignDefaultSettings()
            {
                this._totalWidth = 800f;
                this._totalHeight = 350f;
                this._fontFamily = "Verdana";
                this._backColor = Color.White;
                this._fontColor = Color.Black;
                this._topBuffer = 30f;
                this._bottomBuffer = 30f;
                this._yTickCount = 2;
                this._displayLegend = false;
                this._displayBarData = false;
            }

            private void CalculateBarWidth(int dataCount, float barGraphWidth)
            {
                this._barWidth = barGraphWidth / ((float) (dataCount * 2));
                this._spaceBtwBars = barGraphWidth / ((float) (dataCount * 2));
            }

            private void CalculateGraphDimension()
            {
                this.FindLongestTickValue();
                this._longestTickValue = this._longestTickValue + "0";
                this._maxTickValueWidth = this.CalculateImgFontWidth(this._longestTickValue, 7, this.FontFamily);
                float num = 5f + this._maxTickValueWidth;
                float num2 = 0f;
                if (this._displayLegend)
                {
                    this._legendWidth = (20f + this._maxLabelWidth) + 5f;
                    num2 = (15f + this._legendWidth) + 5f;
                }
                else
                {
                    num2 = 5f;
                }
                this._graphHeight = (this._totalHeight - this._topBuffer) - this._bottomBuffer;
                this._graphWidth = (this._totalWidth - num) - num2;
                this._xOrigin = num;
                this._yOrigin = this._topBuffer;
                this._scaleFactor = this._maxValue / this._graphHeight;
            }

            private float CalculateImgFontWidth(string text, int size, string family)
            {
                Bitmap image = null;
                Graphics graphics = null;
                Font font = null;
                float width;
                try
                {
                    font = new Font(family, (float) size);
                    image = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
                    graphics = Graphics.FromImage(image);
                    width = graphics.MeasureString(text, font).Width;
                }
                finally
                {
                    if (graphics != null)
                    {
                        graphics.Dispose();
                    }
                    if (image != null)
                    {
                        image.Dispose();
                    }
                    if (font != null)
                    {
                        font.Dispose();
                    }
                }
                return width;
            }

            private void CalculateSweepValues()
            {
                int num = 0;
                foreach (CustomChart.ChartItem item in base.DataPoints)
                {
                    if (item.Value >= 0f)
                    {
                        item.SweepSize = item.Value / this._scaleFactor;
                    }
                    item.StartPos = (this._spaceBtwBars / 2f) + (num * (this._barWidth + this._spaceBtwBars));
                    num++;
                }
            }

            private void CalculateTickAndMax()
            {
                float num = 0f;
                this._maxValue *= 1.1f;
                if (this._maxValue != 0f)
                {
                    double num2 = Convert.ToDouble(Math.Floor(Math.Log10((double) this._maxValue)));
                    num = Convert.ToSingle((double) (Math.Ceiling((double) (((double) this._maxValue) / Math.Pow(10.0, num2))) * Math.Pow(10.0, num2)));
                }
                else
                {
                    num = 1f;
                }
                this._yTickValue = num / ((float) this._yTickCount);
                double y = Convert.ToDouble(Math.Floor(Math.Log10((double) this._yTickValue)));
                this._yTickValue = Convert.ToSingle((double) (Math.Ceiling((double) (((double) this._yTickValue) / Math.Pow(10.0, y))) * Math.Pow(10.0, y)));
                this._maxValue = this._yTickValue * this._yTickCount;
            }

            public void CollectDataPoints(string[] values)
            {
                string[] labels = values;
                this.CollectDataPoints(labels, values);
            }

            public void CollectDataPoints(string[] labels, string[] values)
            {
                if (labels.Length != values.Length)
                {
                    throw new Exception("X data count is different from Y data count");
                }
                for (int i = 0; i < labels.Length; i++)
                {
                    float data = Convert.ToSingle(values[i]);
                    string label = this.MakeShortLabel(labels[i]);
                    base.DataPoints.Add(new CustomChart.ChartItem(label, labels[i], data, 0f, 0f, base.GetColor(i)));
                    if (this._maxValue < data)
                    {
                        this._maxValue = data;
                    }
                    if (this._displayLegend)
                    {
                        string text = labels[i] + " (" + label + ")";
                        float num3 = this.CalculateImgFontWidth(text, 9, this.FontFamily);
                        if (this._maxLabelWidth < num3)
                        {
                            this._longestLabel = text;
                            this._maxLabelWidth = num3;
                        }
                    }
                }
                this.CalculateTickAndMax();
                this.CalculateGraphDimension();
                this.CalculateBarWidth(base.DataPoints.Count, this._graphWidth);
                this.CalculateSweepValues();
            }

            public override Bitmap Draw()
            {
                int height = Convert.ToInt32(this._totalHeight);
                Bitmap image = new Bitmap(Convert.ToInt32(this._totalWidth), height);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle rect = new Rectangle(-1, -1, image.Width + 1, image.Height + 1);
                    LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(0xf6, 0xf7, 0xfb), Color.FromArgb(0xdb, 0xe4, 0xeb), LinearGradientMode.Vertical);
                    graphics.FillRectangle(brush, rect);
                    this.DrawVerticalLabelArea(graphics);
                    this.DrawBars(graphics);
                    this.DrawXLabelArea(graphics);
                    if (this._displayLegend)
                    {
                        this.DrawLegend(graphics);
                    }
                }
                return image;
            }

            private void DrawBars(Graphics graph)
            {
                SolidBrush brush = null;
                Font font = null;
                StringFormat format = null;
                try
                {
                    brush = new SolidBrush(this._fontColor);
                    font = new Font(this._fontFamily, 7f);
                    format = new StringFormat {
                        Alignment = StringAlignment.Center
                    };
                    int num = 0;
                    foreach (CustomChart.ChartItem item in base.DataPoints)
                    {
                        using (SolidBrush brush2 = new SolidBrush(item.ItemColor))
                        {
                            float height = (item.SweepSize == 0f) ? 2f : item.SweepSize;
                            float y = (this._yOrigin + this._graphHeight) - height;
                            graph.FillRectangle(brush2, (this._xOrigin + item.StartPos) + ((this._barWidth / 2f) - 8f), y, 16f, height);
                            if (this._displayBarData)
                            {
                                float x = this._xOrigin + (num * (this._barWidth + this._spaceBtwBars));
                                float num5 = (y - 2f) - font.Height;
                                RectangleF layoutRectangle = new RectangleF(x, num5, this._barWidth + this._spaceBtwBars, (float) font.Height);
                                graph.DrawString((item.Value.ToString(CultureInfo.InvariantCulture) == "0.0") ? "0" : item.Value.ToString(CultureInfo.InvariantCulture), font, brush, layoutRectangle, format);
                            }
                            num++;
                        }
                    }
                }
                finally
                {
                    if (brush != null)
                    {
                        brush.Dispose();
                    }
                    if (font != null)
                    {
                        font.Dispose();
                    }
                    if (format != null)
                    {
                        format.Dispose();
                    }
                }
            }

            private void DrawLegend(Graphics graph)
            {
                Font font = null;
                SolidBrush brush = null;
                StringFormat format = null;
                Pen pen = null;
                try
                {
                    font = new Font(this._fontFamily, 9f);
                    brush = new SolidBrush(this._fontColor);
                    format = new StringFormat();
                    pen = new Pen(this._fontColor);
                    format.Alignment = StringAlignment.Near;
                    float x = (this._xOrigin + this._graphWidth) + 6f;
                    float y = this._yOrigin;
                    float num3 = x + 5f;
                    float num4 = (num3 + 10f) + 5f;
                    float num5 = 0f;
                    int num6 = 0;
                    for (int i = 0; i < base.DataPoints.Count; i++)
                    {
                        CustomChart.ChartItem item = base.DataPoints[i];
                        string s = item.Description + "(" + item.Label + ")";
                        num5 += font.Height + 5f;
                        float num8 = (y + 5f) + ((i - num6) * (font.Height + 5f));
                        graph.DrawString(s, font, brush, num4, num8, format);
                        graph.FillRectangle(new SolidBrush(base.DataPoints[i].ItemColor), num3, num8 + 3f, 10f, 10f);
                    }
                    graph.DrawRectangle(pen, x, y, this._legendWidth, num5 + 5f);
                }
                finally
                {
                    if (font != null)
                    {
                        font.Dispose();
                    }
                    if (brush != null)
                    {
                        brush.Dispose();
                    }
                    if (format != null)
                    {
                        format.Dispose();
                    }
                    if (pen != null)
                    {
                        pen.Dispose();
                    }
                }
            }

            private void DrawVerticalLabelArea(Graphics graph)
            {
                Font font = null;
                SolidBrush brush = null;
                StringFormat format = null;
                Pen pen = null;
                StringFormat format2 = null;
                try
                {
                    font = new Font(this._fontFamily, 7f);
                    brush = new SolidBrush(this._fontColor);
                    format = new StringFormat();
                    pen = new Pen(this._fontColor);
                    format2 = new StringFormat();
                    format.Alignment = StringAlignment.Near;
                    RectangleF layoutRectangle = new RectangleF(0f, (this._yOrigin - 10f) - font.Height, this._xOrigin * 2f, (float) font.Height);
                    format2.Alignment = StringAlignment.Center;
                    graph.DrawString(this._yLabel, font, brush, layoutRectangle, format2);
                    for (int i = 0; i < this._yTickCount; i++)
                    {
                        float num2 = this._topBuffer + ((i * this._yTickValue) / this._scaleFactor);
                        float y = num2 - (font.Height / 2);
                        RectangleF ef2 = new RectangleF(5f, y, this._maxTickValueWidth, (float) font.Height);
                        graph.DrawString((this._maxValue - (i * this._yTickValue)).ToString("#,###.##"), font, brush, ef2, format);
                        graph.DrawLine(pen, this._xOrigin, num2, this._xOrigin - 4f, num2);
                    }
                    graph.DrawLine(pen, this._xOrigin, this._yOrigin, this._xOrigin, this._yOrigin + this._graphHeight);
                }
                finally
                {
                    if (font != null)
                    {
                        font.Dispose();
                    }
                    if (brush != null)
                    {
                        brush.Dispose();
                    }
                    if (format != null)
                    {
                        format.Dispose();
                    }
                    if (pen != null)
                    {
                        pen.Dispose();
                    }
                    if (format2 != null)
                    {
                        format2.Dispose();
                    }
                }
            }

            private void DrawXLabelArea(Graphics graph)
            {
                Font font = null;
                SolidBrush brush = null;
                StringFormat format = null;
                Pen pen = null;
                try
                {
                    font = new Font(this._fontFamily, 7f);
                    brush = new SolidBrush(this._fontColor);
                    format = new StringFormat();
                    pen = new Pen(this._fontColor);
                    format.Alignment = StringAlignment.Center;
                    graph.DrawLine(pen, this._xOrigin, this._yOrigin + this._graphHeight, this._xOrigin + this._graphWidth, this._yOrigin + this._graphHeight);
                    float y = (this._yOrigin + this._graphHeight) + 2f;
                    float width = this._barWidth + this._spaceBtwBars;
                    int num3 = 0;
                    foreach (CustomChart.ChartItem item in base.DataPoints)
                    {
                        float x = this._xOrigin + (num3 * width);
                        RectangleF layoutRectangle = new RectangleF(x, y, width, (float) font.Height);
                        string s = this._displayLegend ? item.Label : item.Description;
                        graph.DrawString(s, font, brush, layoutRectangle, format);
                        num3++;
                    }
                }
                finally
                {
                    if (font != null)
                    {
                        font.Dispose();
                    }
                    if (brush != null)
                    {
                        brush.Dispose();
                    }
                    if (format != null)
                    {
                        format.Dispose();
                    }
                    if (pen != null)
                    {
                        pen.Dispose();
                    }
                }
            }

            private void FindLongestTickValue()
            {
                for (int i = 0; i < this._yTickCount; i++)
                {
                    string str = (this._maxValue - (i * this._yTickValue)).ToString("#,###.##");
                    if (this._longestTickValue.Length < str.Length)
                    {
                        this._longestTickValue = str;
                    }
                }
            }

            private string MakeShortLabel(string text)
            {
                string str = text;
                if (text.Length > 2)
                {
                    int startIndex = Convert.ToInt32(Math.Floor((double) (text.Length / 2)));
                    str = text.Substring(0, 1) + text.Substring(startIndex, 1) + text.Substring(text.Length - 1, 1);
                }
                return str;
            }

            public Color BackgroundColor
            {
                set
                {
                    this._backColor = value;
                }
            }

            public int BottomBuffer
            {
                set
                {
                    this._bottomBuffer = Convert.ToSingle(value);
                }
            }

            public Color FontColor
            {
                set
                {
                    this._fontColor = value;
                }
            }

            public string FontFamily
            {
                get
                {
                    return this._fontFamily;
                }
                set
                {
                    this._fontFamily = value;
                }
            }

            public int Height
            {
                get
                {
                    return Convert.ToInt32(this._totalHeight);
                }
                set
                {
                    this._totalHeight = Convert.ToSingle(value);
                }
            }

            public bool ShowData
            {
                get
                {
                    return this._displayBarData;
                }
                set
                {
                    this._displayBarData = value;
                }
            }

            public bool ShowLegend
            {
                get
                {
                    return this._displayLegend;
                }
                set
                {
                    this._displayLegend = value;
                }
            }

            public int TopBuffer
            {
                set
                {
                    this._topBuffer = Convert.ToSingle(value);
                }
            }

            public string VerticalLabel
            {
                get
                {
                    return this._yLabel;
                }
                set
                {
                    this._yLabel = value;
                }
            }

            public int VerticalTickCount
            {
                get
                {
                    return this._yTickCount;
                }
                set
                {
                    this._yTickCount = value;
                }
            }

            public int Width
            {
                get
                {
                    return Convert.ToInt32(this._totalWidth);
                }
                set
                {
                    this._totalWidth = Convert.ToSingle(value);
                }
            }
        }

        public abstract class Chart
        {
            private Color[] _color = new Color[] { Color.Chocolate, Color.YellowGreen, Color.Olive, Color.DarkKhaki, Color.Sienna, Color.PaleGoldenrod, Color.Peru, Color.Tan, Color.Khaki, Color.DarkGoldenrod, Color.Maroon, Color.OliveDrab };
            private CustomChart.ChartItemsCollection _dataPoints = new CustomChart.ChartItemsCollection();

            protected Chart()
            {
            }

            public abstract Bitmap Draw();
            public Color GetColor(int index)
            {
                if (index == 11)
                {
                    return this._color[index];
                }
                return this._color[index % 12];
            }

            public void SetColor(int index, Color NewColor)
            {
                if (index == 11)
                {
                    this._color[index] = NewColor;
                }
                else
                {
                    this._color[index % 12] = NewColor;
                }
            }

            public CustomChart.ChartItemsCollection DataPoints
            {
                get
                {
                    return this._dataPoints;
                }
                set
                {
                    this._dataPoints = value;
                }
            }
        }

        public class ChartItem
        {
            private Color _color;
            private string _description;
            private string _label;
            private float _startPos;
            private float _sweepSize;
            private float _value;

            private ChartItem()
            {
            }

            public ChartItem(string label, string desc, float data, float start, float sweep, Color clr)
            {
                this._label = label;
                this._description = desc;
                this._value = data;
                this._startPos = start;
                this._sweepSize = sweep;
                this._color = clr;
            }

            public string Description
            {
                get
                {
                    return this._description;
                }
                set
                {
                    this._description = value;
                }
            }

            public Color ItemColor
            {
                get
                {
                    return this._color;
                }
                set
                {
                    this._color = value;
                }
            }

            public string Label
            {
                get
                {
                    return this._label;
                }
                set
                {
                    this._label = value;
                }
            }

            public float StartPos
            {
                get
                {
                    return this._startPos;
                }
                set
                {
                    this._startPos = value;
                }
            }

            public float SweepSize
            {
                get
                {
                    return this._sweepSize;
                }
                set
                {
                    this._sweepSize = value;
                }
            }

            public float Value
            {
                get
                {
                    return this._value;
                }
                set
                {
                    this._value = value;
                }
            }
        }

        public class ChartItemsCollection : CollectionBase
        {
            public int Add(CustomChart.ChartItem value)
            {
                return base.List.Add(value);
            }

            public bool Contains(CustomChart.ChartItem value)
            {
                return base.List.Contains(value);
            }

            public int IndexOf(CustomChart.ChartItem value)
            {
                return base.List.IndexOf(value);
            }

            public void Remove(CustomChart.ChartItem value)
            {
                base.List.Remove(value);
            }

            public CustomChart.ChartItem this[int index]
            {
                get
                {
                    return (CustomChart.ChartItem) base.List[index];
                }
                set
                {
                    base.List[index] = value;
                }
            }
        }

        public class PieChart : CustomChart.Chart
        {
            private Color _backgroundColor;
            private Color _borderColor;
            private const int _bufferSpace = 0x7d;
            private ArrayList _chartItems;
            private int _legendFontHeight;
            private float _legendFontSize;
            private string _legendFontStyle;
            private int _legendHeight;
            private int _legendWidth;
            private int _perimeter;
            private float _total;

            public PieChart()
            {
                this._chartItems = new ArrayList();
                this._perimeter = 250;
                this._backgroundColor = Color.White;
                this._borderColor = Color.FromArgb(0x3f, 0x3f, 0x3f);
                this._legendFontSize = 8f;
                this._legendFontStyle = "Verdana";
            }

            public PieChart(Color bgColor)
            {
                this._chartItems = new ArrayList();
                this._perimeter = 250;
                this._backgroundColor = bgColor;
                this._borderColor = Color.FromArgb(0x3f, 0x3f, 0x3f);
                this._legendFontSize = 8f;
                this._legendFontStyle = "Verdana";
            }

            private void CalculateLegendWidthHeight()
            {
                Font font = new Font(this._legendFontStyle, this._legendFontSize);
                this._legendFontHeight = font.Height + 5;
                this._legendHeight = font.Height * (this._chartItems.Count + 1);
                if (this._legendHeight > this._perimeter)
                {
                    this._perimeter = this._legendHeight;
                }
                this._legendWidth = this._perimeter + 0x7d;
            }

            public void CollectDataPoints(string[] xValues, string[] yValues)
            {
                this._total = 0f;
                for (int i = 0; i < xValues.Length; i++)
                {
                    float data = Convert.ToSingle(yValues[i]);
                    this._chartItems.Add(new CustomChart.ChartItem(xValues[i], xValues.ToString(), data, 0f, 0f, Color.AliceBlue));
                    this._total += data;
                }
                float num3 = 0f;
                int num4 = 0;
                foreach (CustomChart.ChartItem item in this._chartItems)
                {
                    item.StartPos = num3;
                    item.SweepSize = (item.Value / this._total) * 360f;
                    num3 = item.StartPos + item.SweepSize;
                    item.ItemColor = base.GetColor(num4++);
                }
                this.CalculateLegendWidthHeight();
            }

            public override Bitmap Draw()
            {
                int width = this._perimeter;
                Rectangle rect = new Rectangle(0, 0, width, width - 1);
                Bitmap image = new Bitmap(width + this._legendWidth, width);
                Graphics graphics = null;
                StringFormat format = null;
                SolidBrush brush = null;
                try
                {
                    graphics = Graphics.FromImage(image);
                    format = new StringFormat();
                    graphics.FillRectangle(new SolidBrush(this._backgroundColor), 0, 0, width + this._legendWidth, width);
                    format.Alignment = StringAlignment.Far;
                    for (int i = 0; i < this._chartItems.Count; i++)
                    {
                        CustomChart.ChartItem item = (CustomChart.ChartItem) this._chartItems[i];
                        using ((SolidBrush) (brush = null))
                        {
                            brush = new SolidBrush(item.ItemColor);
                            graphics.FillPie(brush, rect, item.StartPos, item.SweepSize);
                            graphics.FillRectangle(brush, width + 0x7d, (i * this._legendFontHeight) + 15, 10, 10);
                            graphics.DrawString(item.Label, new Font(this._legendFontStyle, this._legendFontSize), new SolidBrush(Color.Black), (float) ((width + 0x7d) + 20), (float) ((i * this._legendFontHeight) + 13));
                            graphics.DrawString(item.Value.ToString("C"), new Font(this._legendFontStyle, this._legendFontSize), new SolidBrush(Color.Black), (float) ((width + 0x7d) + 200), (float) ((i * this._legendFontHeight) + 13), format);
                        }
                    }
                    graphics.DrawEllipse(new Pen(this._borderColor, 2f), rect);
                    graphics.DrawRectangle(new Pen(this._borderColor, 1f), (width + 0x7d) - 10, 10, 220, (this._chartItems.Count * this._legendFontHeight) + 0x19);
                    graphics.DrawString("Total", new Font(this._legendFontStyle, this._legendFontSize, FontStyle.Bold), new SolidBrush(Color.Black), (float) ((width + 0x7d) + 30), (float) ((this._chartItems.Count + 1) * this._legendFontHeight), format);
                    graphics.DrawString(this._total.ToString("C"), new Font(this._legendFontStyle, this._legendFontSize, FontStyle.Bold), new SolidBrush(Color.Black), (float) ((width + 0x7d) + 200), (float) ((this._chartItems.Count + 1) * this._legendFontHeight), format);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }
                finally
                {
                    if (format != null)
                    {
                        format.Dispose();
                    }
                    if (graphics != null)
                    {
                        graphics.Dispose();
                    }
                }
                return image;
            }
        }
    }
}

