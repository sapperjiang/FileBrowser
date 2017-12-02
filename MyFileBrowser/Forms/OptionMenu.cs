using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyFileBrowser
{

    public partial class OptionMenu : Form
    {
        private  Dictionary<string,PictureBox> dicMenuItems;// = new Dictionary<string itemName,Point>();

        private int iItemWidth=20;
        public OptionMenu()
        {
            InitializeComponent();
            dicMenuItems = new Dictionary<string, PictureBox>();
        }
        public Point pCenterPostion;
        private string _menuLayout;// =MenuLayoutType.Cycle;
        public string MenuLayout
        {
          get { return _menuLayout; }
          set {
              _menuLayout = value;
          }
        }
        ShapeLayout shapeLayout = new CycleLayout();


        public void AddMenuItem(string name,Image iconImg,DragEventHandler callBackFunction )
        {
            PictureBox pb = new PictureBox();
           
            pb.AllowDrop = true;
            pb.Name =name;
            pb.Image = iconImg;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Size = new Size(iItemWidth*2, iItemWidth*2);
            pb.DragEnter += new DragEventHandler(Default_DragEnter);
            pb.DragDrop += callBackFunction;
            //pb.
            dicMenuItems.Add(name,pb);

            this.Controls.Add(pb);
            //this.components.Add((IComponent)pb);
        }

        private Point Center
        {
            get
            {
                return new Point(this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
            }
        }
        private void SetItemLayout()
        {
            int iCount = dicMenuItems.Count;
            double iAngle = Math.PI * 2 / iCount;
            List<Point> p = null;
            p = shapeLayout.GetScreenPosition(this.Center,iItemWidth, dicMenuItems.Count);
           
            int i = 0;
       
            foreach(var item in dicMenuItems.Keys)
            {
                PictureBox lvi;
                dicMenuItems.TryGetValue(item,out lvi);
                lvi.Location = this.PointToClient(p[i++]);//转换为工作区坐标
            }

        }


        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible == true)
            {
                this.Location = new Point(Cursor.Position.X-this.Width/2,Cursor.Position.Y-this.Height/2);
               // MessageBox.Show(Cursor.Position.ToString());
                this.SetItemLayout();
            }
            base.OnVisibleChanged(e);
        }

        //protected override void OnLocationChanged(EventArgs e)
        //{
        //    this.SetItemLayout();
        //    base.OnLocationChanged(e);
        //}
        const byte CtrlMask = 8;
        private void Default_DragEnter(object sender, DragEventArgs e)
        {// 检查被拖曳的数据的类型是否适用于目标控件。如果不适用，则拒绝置放。
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem")==true)
            {
                // 如果在拖曳期间按着 Ctrl 键，则执行复制操作；反之，则执行移动操作。
                if ((e.KeyState & CtrlMask) == CtrlMask)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }

    /// <summary>
    /// 接口
    /// </summary>
    public abstract class ShapeLayout
    {
        protected abstract List<Point> LayoutItemPosition(int space, int iItemCount);
        public virtual List<Point> GetScreenPosition(Point pCenter,int space, int iItemCount)
        {
            int iX; int iY;
            List<Point> p = this.LayoutItemPosition(space,iItemCount);
            for (int i = 0; i < p.Count; i++)
            {
                iX = pCenter.X+p[i].X-space;
                iY = pCenter.Y+p[i].Y-space;
                p[i] = new Point(iX, iY);
            }
            return p;
        }
    }

    /// <summary>
    /// 实现类
    /// </summary>
    public class CycleLayout : ShapeLayout
    {
        protected override List<Point> LayoutItemPosition(int iIconWidth, int iItemCount)
        {
            List<Point> pItem = new List<Point>();
            double iAngle = Math.PI * 2 / iItemCount;

            int space = (int)(iIconWidth / Math.Tan(iAngle / 2)) + 30;

            for (int i = 0; i < iItemCount; i++)
            {
                double dX = space * Math.Cos(iAngle * i + Math.PI /2);
                double dY = space * Math.Sin(iAngle * i + Math.PI/2);
                pItem.Add(new Point((int)dX, (int)dY));
            }
            return pItem;
        }
    }
}
