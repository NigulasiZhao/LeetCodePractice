using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;


namespace GISWaterSupplyAndSewageServer.CommonTools
{
    public class CurveHeng
    {
        #region 私有字段
        private Graphics objGraphics;//Graphics 类提供将对象绘制到显示设备的方法
        private Bitmap objBitmap;//位图对象

        private int m_Width = 900;//图像宽度
        private int m_Height = 500;//图像高度
        private int m_BottomToXHeight = 200;//图像离X轴的高度
        private float m_XSlice = 50;//X轴刻度宽度
        private float m_YSlice = 50;//Y轴刻度宽度
        private float m_YSliceValue = 5;//Y轴刻度的数值宽度
        private float m_YSliceBegin = -20;//Y轴刻度
        private float m_Tension = 0.5f;
        private string m_Title = "剖面分析";
        private string m_Unit = "米";//单位
        private string m_XAxisText = "月份";//X轴说明文字
        private string m_YAxisText = "高程";//Y轴说明文字
        private string[] m_Keys = new string[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };//键（管线编号）
        private float[] m_Values = new float[] { 20.0f, 30.0f, 50.0f, 55.4f, 21.6f, 12.8f, 99.5f, 36.4f, 78.2f, 56.4f, 45.8f, 66.5f };//值（高程）
        //private float[] m_Values = new float[] { -2.0f, -1.5f, 2.0f, -3.5f, -1.5f, -1.8f, -2.0f, -1.5f, -2.5f, 2.0f, 3.0f, -1.0f };//值
        private Color m_BgColor = Color.Snow;//背景
        private Color m_TextColor = Color.Black;//文字颜色
        private Color m_BorderColor = Color.Black;//整体边框颜色
        private Color m_AxisColor = Color.Black;//轴线颜色
        private Color m_AxisTextColor = Color.Black;//轴说明文字颜色
        private Color m_SliceTextColor = Color.Black;//刻度文字颜色
        private Color m_SliceColor = Color.Black;//刻度颜色
        private Color m_CurveColor = Color.Red;//曲线颜色
        private string m_IconPath = "ChartImages/regular.png";//管线图标

        private DataSet m_ds;
        #endregion
        public DataSet DS
        {
            get { return m_ds; }
            set { m_ds = value; }
        }
        public int Width
        {
            set
            {
                if (value < 300)
                {
                    m_Width = 300;
                }
                else
                {
                    m_Width = value;
                }
            }
            get
            {
                return m_Width;
            }
        }
        public int Height
        {
            set
            {
                if (value < 300)
                {
                    m_Height = 300;
                }
                else
                {
                    m_Height = value;
                }
            }
            get
            {
                return m_Height;
            }
        }
        public int BottomToXHeight
        {
            set
            {
                if (value < 200)
                {
                    m_BottomToXHeight = 200;
                }
                else
                {
                    m_BottomToXHeight = value;
                }
            }
            get
            {
                return m_BottomToXHeight;
            }
        }
        public float XSlice
        {
            set { m_XSlice = value; }
            get { return m_XSlice; }
        }
        public float YSlice
        {
            set { m_YSlice = value; }
            get { return m_YSlice; }
        }
        public float YSliceValue
        {
            set { m_YSliceValue = value; }
            get { return m_YSliceValue; }
        }
        public float YSliceBegin
        {
            set { m_YSliceBegin = value; }
            get { return m_YSliceBegin; }
        }
        public float Tension
        {
            set { m_Tension = value; }
            get { return m_Tension; }
        }
        public string Title
        {
            set { m_Title = value; }
            get { return m_Title; }
        }
        public string Unit
        {
            set { m_Unit = value; }
            get { return m_Unit; }
        }
        public string XAxisText
        {
            set { m_XAxisText = value; }
            get { return m_XAxisText; }
        }
        public string YAxisText
        {
            set { m_YAxisText = value; }
            get { return m_YAxisText; }
        }
        public string[] Keys
        {
            set { m_Keys = value; }
            get { return m_Keys; }
        }
        public float[] Values
        {
            set { m_Values = value; }
            get { return m_Values; }
        }
        public Color BgColor
        {
            set { m_BgColor = value; }
            get { return m_BgColor; }
        }
        public Color TextColor
        {
            set { m_TextColor = value; }
            get { return m_TextColor; }
        }
        public Color BorderColor
        {
            set { m_BorderColor = value; }
            get { return m_BorderColor; }
        }
        public Color AxisColor
        {
            set { m_AxisColor = value; }
            get { return m_AxisColor; }
        }
        public Color AxisTextColor
        {
            set { m_AxisTextColor = value; }
            get { return m_AxisTextColor; }
        }
        public Color SliceTextColor
        {
            set { m_SliceTextColor = value; }
            get { return m_SliceTextColor; }
        }
        public Color SliceColor
        {
            set { m_SliceColor = value; }
            get { return m_SliceColor; }
        }
        public Color CurveColor
        {
            set { m_CurveColor = value; }
            get { return m_CurveColor; }
        }
        public string IconPath
        {
            set { m_IconPath = value; }
            get { return m_IconPath; }
        }
        //生成图像并返回bmp图像对象
        public Bitmap CreateImage()
        {
            InitializeGraph();
            DrawContent(ref objGraphics);
            return objBitmap;
        }
        //初始化和填充图像区域，画出边框，初始标题
        private void InitializeGraph()
        {
            //根据DS给Keys和Values赋值
            SetKeyValues();

            //根据给定的高度和宽度创建一个位图图像
            objBitmap = new Bitmap(Width, Height);

            //从指定的objBitmap对象创建objGraphics对象(即在objBitmap对象中画图)
            objGraphics = Graphics.FromImage(objBitmap);

            //根据给定颜色(LightGray)填充图像的矩形区域（背景）
            objGraphics.DrawRectangle(new Pen(BorderColor, 1), 0, 0, Width, Height);
            objGraphics.FillRectangle(new SolidBrush(BgColor), 1, 1, Width - 2, Height - 2);

            //画X轴，pen,x1,y1,x2,y2 注意图像的原始X轴和Y轴是以左上角为原点，向右和向下计算的
            //objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 100, Height - 200, Width - 75, Height - 200);
            objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 100, Height - BottomToXHeight, Width - 75, Height - BottomToXHeight);

            //画Y轴，pen,x1,y1,x2,y2
            //objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 100, Height - 100, 100, 75);
            objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor), 1), 100, Height - BottomToXHeight, 100, 75);

            //初始化轴线说明文字
            SetAxisText(ref objGraphics);

            //初始化X轴上的刻度和文字
            SetXAxis(ref objGraphics);

            //初始化Y轴上的刻度和文字
            SetYAxis(ref objGraphics);

            //初始化标题
            CreateTitle(ref objGraphics);

            //初始化属性表格
            CreatePropTable(ref objGraphics);
        }
        private void CreatePropTable(ref Graphics objGraphics)
        {
            int X = 10;
            int Y = Height - BottomToXHeight;
            int width = Width - 10 - 10;//左右10像素间隔
            int height = 40;
            int scale = 0;//依次增加Y值 每画一个矩形，增加40个像素的高度
            for (int i = 0; i < 5; i++)
            {
                scale = i * height;
                objGraphics.DrawRectangle(new Pen(BorderColor, 1), X, Y + scale, width, height);
            }
            //画隔离线
            objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), 100, Y, 100, Height);
            //画属性名称
            int Y1 = Y + 15;
            Point p0 = new Point(15, Y1);
            Point p1 = new Point(15, Y1 + 1 * 40);
            Point p2 = new Point(15, Y1 + 2 * 40);
            Point p3 = new Point(15, Y1 + 3 * 40);
            Point p4 = new Point(15, Y1 + 4 * 40);
            Font font = new Font("宋体", 10);
            objGraphics.DrawString("管线编号", font, new SolidBrush(Color.Black), p0);
            objGraphics.DrawString("管线长度(m)", font, new SolidBrush(Color.Black), p1);
            objGraphics.DrawString("口径(mm)", font, new SolidBrush(Color.Black), p2);
            objGraphics.DrawString("埋深(m)", font, new SolidBrush(Color.Black), p3);
            objGraphics.DrawString("高程(m)", font, new SolidBrush(Color.Black), p4);

        }
        //根据DS给Keys和Values赋值
        private void SetKeyValues()
        {
            if (DS != null)
            {
                DataTable dt = DS.Tables[0];
                DataRow dr;
                int rows = dt.Rows.Count;
                string[] keys = new string[rows];
                float[] values = new float[rows];
                for (int i = 0; i < rows; i++)
                {
                    dr = dt.Rows[i];
                    keys[i] = dr[0].ToString();
                    //values[i] = Convert.ToSingle(dr[1]) / 10000;
                    values[i] = Convert.ToSingle(dr[3].ToString() == "" ? 0 : -Math.Abs(float.Parse(dr[3].ToString())));//高程
                }
                Keys = keys;
                Values = values;
            }
            else
            {
                Keys = m_Keys;
                Values = m_Values;
            }
        }
        //设置X,Y轴说明文本字体
        private void SetAxisText(ref Graphics objGraphics)
        {
            //objGraphics.DrawString(XAxisText, new Font("宋体", 10), new SolidBrush(AxisTextColor), Width / 2 - 50, Height - 50);
            //objGraphics.DrawString(XAxisText, new Font("宋体", 10), new SolidBrush(AxisTextColor), Width / 2 - 50, Height - (BottomToXHeight - 50));

            int X = 30;
            //int Y = (Height / 2) - 50;
            int Y = (Height / 2) - 50 - 100;
            for (int i = 0; i < YAxisText.Length; i++)
            {
                objGraphics.DrawString(YAxisText[i].ToString(), new Font("宋体", 10), new SolidBrush(AxisTextColor), X, Y);
                Y += 15;
            }
        }
        private void SetXAxis(ref Graphics objGraphics)
        {
            int x1 = 100;
            //int y1 = Height - 110;
            int y1 = Height - BottomToXHeight - 10;
            int x2 = 100;
            //int y2 = Height - 90;
            //int y2 = Height - BottomToXHeight + 10;
            int y2 = Height - BottomToXHeight + 5;
            int iCount = 0;
            int iSliceCount = 1;
            float Scale = 0;
            int iWidth = (int)((Width - 200) * (50 / XSlice));
            //objGraphics.DrawString(Keys[0].ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), 85, Height - 90);
            //objGraphics.DrawString(Keys[0].ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), 85, Height - BottomToXHeight + 10);

            for (int i = 0; i <= iWidth; i += 10)
            {
                Scale = i * (XSlice / 50);
                if (iCount == 5)
                {
                    objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), x1 + Scale, y1, x2 + Scale, y2 - 5);
                    //The Point 这里显示X轴刻度
                    if (iSliceCount <= Keys.Length - 1)
                    {
                        //objGraphics.DrawString(Keys[iSliceCount].ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), x1 + Scale - 15, y2);
                    }
                    else
                    {
                        //超过范围,不画任何刻度文字
                    }
                    iCount = 0;
                    iSliceCount++;
                    if (x1 + Scale > Width - 100)
                    {
                        break;
                    }
                }
                else
                {
                    objGraphics.DrawLine(new Pen(new SolidBrush(SliceColor)), x1 + Scale, y1 + 5, x2 + Scale, y2 - 5);
                }
                iCount++;
            }
        }
        private void SetYAxis(ref Graphics objGraphics)
        {
            int x1 = 95;
            //int y1 = (int)(Height - 100 - 10 * (YSlice / 50));
            int y1 = (int)(Height - BottomToXHeight - 10 * (YSlice / 50));
            int x2 = 105;
            //int y2 = (int)(Height - 100 - 10 * (YSlice / 50));
            int y2 = (int)(Height - BottomToXHeight - 10 * (YSlice / 50));
            int iCount = 1;
            float Scale = 0;
            int iSliceCount = 1;
            int iHeight = (int)((Height - 200 - 100) * (50 / YSlice));
            //objGraphics.DrawString(YSliceBegin.ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), 60, Height - 110);
            objGraphics.DrawString(YSliceBegin.ToString(), new Font("宋体", 10), new SolidBrush(SliceTextColor), 60, Height - BottomToXHeight - 10);
            for (int i = 0; i < iHeight; i += 10)
            {
                Scale = i * (YSlice / 50);
                if (iCount == 5)
                {
                    objGraphics.DrawLine(new Pen(new SolidBrush(AxisColor)), x1 - 5, y1 - Scale, x2 + 5, y2 - Scale);
                    //The Point这里显示Y轴刻度
                    objGraphics.DrawString(Convert.ToString(YSliceValue * iSliceCount + YSliceBegin), new Font("宋体", 10), new SolidBrush(SliceTextColor), 60, y1 - Scale);
                    iCount = 0;
                    iSliceCount++;
                }
                else
                {
                    objGraphics.DrawLine(new Pen(new SolidBrush(SliceColor)), x1, y1 - Scale, x2, y2 - Scale);
                }
                iCount++;
            }
        }
        private void DrawContent(ref Graphics objGraphics)
        {
            if (Keys.Length == Values.Length)
            {
                Pen CurvePen = new Pen(CurveColor, 1);
                PointF[] CurvePointF = new PointF[Keys.Length];
                PointF[] XAxisPointF = new PointF[Keys.Length];//对象坐标点对应在x轴上的坐标，用于垂直链接两点用
                float keys = 0;
                float values = 0;
                //float Offset1 = (Height - 100) + YSliceBegin;
                float Offset1 = (Height - BottomToXHeight) + YSliceBegin * YSliceValue * 2;
                //float Offset2 = (YSlice / 50) * (50 / YSliceValue);
                float Offset2 = (YSlice / 50) * (50 / YSliceValue);
                for (int i = 0; i < Keys.Length; i++)
                {
                    keys = XSlice * (i + 1) + 100;
                    values = Offset1 + Values[i] * Offset2 + 40;
                    CurvePointF[i] = new PointF(keys, values);
                    //XAxisPointF[i] = new PointF(keys, 500);
                    XAxisPointF[i] = new PointF(keys, Height - BottomToXHeight);
                }

                //垂直连接两点
                Pen MyPen_Guan = new Pen(Color.Black, 1);
                MyPen_Guan.DashStyle = DashStyle.Dot;
                for (int j = 0; j < Keys.Length; j++)
                {
                    objGraphics.DrawLine(MyPen_Guan, CurvePointF[j], XAxisPointF[j]);
                }

                //坐标点上增加图标
                Image img1 = Image.FromFile(IconPath);
                int width = img1.Width;
                int height = img1.Height;
                objGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

                //获取传参
                DataTable dt = DS.Tables[0];
                DataRow dr;
                int rows = dt.Rows.Count;
                float[] KouJings = new float[rows];//口径
                for (int i = 0; i < rows; i++)
                {
                    dr = dt.Rows[i];
                    KouJings[i] = Convert.ToSingle(dr[2].ToString() == "" ? 0 : float.Parse(dr[2].ToString()));
                }
                for (int k = 0; k < Keys.Length; k++)
                {
                    int F = (int)(KouJings[k] / 20);
                    objGraphics.DrawImage(img1, new Rectangle((int)(CurvePointF[k].X - Math.Abs(F) / 2), (int)(CurvePointF[k].Y), F, F));
                }

                //for (int k = 0; k < Keys.Length; k++)
                //{
                //    objGraphics.DrawImage(img1, CurvePointF[k]);
                //}

                //objGraphics.DrawCurve(CurvePen, CurvePointF, Tension);
                DrawPropInfo(ref objGraphics);
            }
            else
            {
                objGraphics.DrawString("错误! 键值的长度必须一致!", new Font("宋体", 16), new SolidBrush(SliceTextColor), new Point(150, Height / 2));
            }
        }
        //画各个属性信息
        private void DrawPropInfo(ref Graphics objGraphics)
        {
            if (Keys.Length == Values.Length)
            {
                if (DS != null)
                {
                    DataTable dt = DS.Tables[0];
                    DataRow dr;
                    int rows = dt.Rows.Count;
                    string[] smIds = new string[rows];//唯一标识
                    float[] smLengths = new float[rows];//长度
                    float[] kouJings = new float[rows];//口径
                    float[] maiShens = new float[rows];//埋深
                    float[] gaoChengs = new float[rows];//高程
                    for (int i = 0; i < rows; i++)
                    {
                        dr = dt.Rows[i];
                        smIds[i] = dr[0].ToString();
                        smLengths[i] = Convert.ToSingle(dr[1]);
                        kouJings[i] = Convert.ToSingle(dr[2].ToString() == "" ? 0 : float.Parse(dr[2].ToString()));
                        maiShens[i] = Convert.ToSingle(dr[3].ToString() == "" ? 0 : float.Parse(dr[3].ToString()));
                        gaoChengs[i] = Convert.ToSingle(dr[4].ToString() == "" ? 0 : float.Parse(dr[4].ToString()));
                    }
                    //画出所有属性线
                    Pen CurvePen = new Pen(CurveColor, 1);
                    PointF[] XAxisPointF = new PointF[Keys.Length];
                    PointF[] BottomPointF = new PointF[Keys.Length];
                    float keys = 0;
                    float Offset1 = (Height - BottomToXHeight) + YSliceBegin;

                    for (int i = 0; i < Keys.Length; i++)
                    {
                        keys = XSlice * (i + 1) + 100;
                        XAxisPointF[i] = new PointF(keys, Height - BottomToXHeight);
                        BottomPointF[i] = new PointF(keys, Height);
                    }

                    Pen MyPen_Prop = new Pen(Color.Black, 1);
                    MyPen_Prop.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    for (int j = 0; j < Keys.Length; j++)
                    {
                        objGraphics.DrawLine(MyPen_Prop, XAxisPointF[j], BottomPointF[j]);
                    }

                    //画Table出所有属性信息
                    //唯一标识
                    int scale = 0;//X坐标延伸
                    StringFormat format = new StringFormat();// 绘制围绕点旋转的文本  
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    for (int i = 0; i < Keys.Length; i++)
                    {
                        scale = (i + 1) * 50;

                        DrawString(smIds[i], new Font("宋体", 8), new SolidBrush(SliceTextColor), new Point(100 + scale - 5, Height - BottomToXHeight + 20), format, -90f);//smId
                        DrawString(Math.Round(smLengths[i], 3).ToString(), new Font("宋体", 8), new SolidBrush(SliceTextColor), new Point(100 + scale - 5, Height - BottomToXHeight + 40 + 20), format, -90f);//管线长度
                        DrawString(Math.Round(kouJings[i], 3).ToString(), new Font("宋体", 8), new SolidBrush(SliceTextColor), new Point(100 + scale - 5, Height - BottomToXHeight + 80 + 20), format, -90f);//口径
                        DrawString(Math.Round(maiShens[i], 3).ToString(), new Font("宋体", 8), new SolidBrush(SliceTextColor), new Point(100 + scale - 5, Height - BottomToXHeight + 120 + 20), format, -90f);//埋深
                        DrawString(Math.Round(gaoChengs[i], 3).ToString(), new Font("宋体", 8), new SolidBrush(SliceTextColor), new Point(100 + scale - 5, Height - BottomToXHeight + 160 + 20), format, -90f);//高程
                    }

                }

            }
            else
            {
                objGraphics.DrawString("错误! 键值的长度必须一致!", new Font("宋体", 16), new SolidBrush(SliceTextColor), new Point(150, Height / 2));
            }
        }
        //初始化标题
        private void CreateTitle(ref Graphics objGraphics)
        {
            objGraphics.DrawString(Title, new Font("宋体", 16), new SolidBrush(TextColor), new Point(5, 5));
        }
        public static void CreateChartByCurve(CurveHeng curveImg, string imgPath, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            try
            {
                if (curveImg == null)
                {
                    curveImg.Title = "Curve 图统计表示例";
                    curveImg.Width = 900;
                    curveImg.Height = 500;
                    curveImg.TextColor = Color.Red;
                }
                Bitmap bmp = curveImg.CreateImage();
                //string savePath = HttpContext.Current.Server.MapPath(imgPath);
                //bmp.Save(savePath, imgFormat);
                bmp.Save(imgPath, imgFormat);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 绘制根据点旋转文本，一般旋转点给定位文本包围盒中心点
        /// </summary>
        /// <param name="s">文本</param>
        /// <param name="font">字体</param>
        /// <param name="brush">填充</param>
        /// <param name="point">旋转点</param>
        /// <param name="format">布局方式</param>
        /// <param name="angle">角度</param>
        private void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format, float angle)
        {
            // Save the matrix
            Matrix mtxSave = objGraphics.Transform;

            Matrix mtxRotate = objGraphics.Transform;
            mtxRotate.RotateAt(angle, point);
            objGraphics.Transform = mtxRotate;

            objGraphics.DrawString(s, font, brush, point, format);

            // Reset the matrix
            objGraphics.Transform = mtxSave;
        }

    }
}