using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Reflection;
using Spring.Context;
using Spring.Context.Support;
using System.Diagnostics;
using UsbEject.Library;
using SapperJiangWFControlsLib;
using ConfigOperatorLib;

namespace MyFileBrowser
{
    public partial class MainForm : Form
    {
        internal Stack<string> stDirStack;

        OptionMenu oM = new OptionMenu();
        private string strPrevDirName = null;

        public bool IsShowHidden = false;
        IConfigOperator iConfiger = null;        
        

        public MainForm()
        {
            InitializeComponent();
            stDirStack = new Stack<string>();
            
            //最大化
            this.WindowState = FormWindowState.Maximized;
            this.FileTreeView.BackColor = this.BackColor;
            this.FileListView.BackColor = this.BackColor;
            //iConfiger = new XMLConfigOperater();

            //IApplicationContext iac = ContextRegistry.GetContext();
            //oM = iac.GetObject("OptionMenu") as OptionMenu;
            //oM.Visible = false;


            //iConfiger = iac.GetObject("XMLConfigOperater") as IConfigOperator;
            //iConfiger.SetProperty("ok","ok");

            /////根据不同类型进行设置
            string strP = Path.Combine(Application.StartupPath, "icons\\");
            oM.AddMenuItem("delete", ImageFast.FromFile(strP + "Folder.png"), new DragEventHandler(OptionMenu_DragDrop));
            oM.AddMenuItem("Test", ImageFast.FromFile(strP + "Can.png"), new DragEventHandler(OptionMenu_DragDrop));
            oM.AddMenuItem("Next", ImageFast.FromFile(strP + "Behavior.png"), new DragEventHandler(OptionMenu_DragDrop));
            oM.AddMenuItem("NPre", ImageFast.FromFile(strP + "x_symbol.png"), new DragEventHandler(OptionMenu_DragDrop));
            oM.AddMenuItem("OK", ImageFast.FromFile(strP + "Setting.png"), new DragEventHandler(OptionMenu_DragDrop));



            this.BackColor = PropertyMgr.BGColor;
            foreach (Control item in this.Controls)
            {
                item.BackColor = this.BackColor;
                item.Font = PropertyMgr.Font;
            }
            //添加服务
            this.FileTreeView.AfterExpand += new TreeViewEventHandler(OnTreeViewExpand);
            this.FileTreeView.AfterCollapse += new TreeViewEventHandler(OnTreeViewCollapse);
            this.AddressBar.SelectedItemChanged += new EventHandler(AddressBar_SelectedItemChanged);

        }

        void AddressBar_SelectedItemChanged(object sender, EventArgs e)
        {
            MyKryptionItem sItem = (sender as WFMyCmBox).SelectedItem as MyKryptionItem;
            if (sItem != null)
            {
                this.FileTreeView.CollapseTreeNode(sItem.strInnerLongText);
                //this.UpdateAddressTreeView(sItem.strInnerLongText);
                this.UpdateUIAddressPath(sItem.strInnerLongText);
            }
            //throw new NotImplementedException();
        }
        internal class PropertyMgr 
        {
            private static Font _font= new Font("宋体",15.97f);
         
            public static Font Font
            {
                get { return PropertyMgr._font; }
                set { PropertyMgr._font = value; }
            }
            private static Color _bgColor= Color.SkyBlue;//.SeaGreen;

            public static Color BGColor
            {
                get { return PropertyMgr._bgColor; }
                set { PropertyMgr._bgColor = value; }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.FileListView.AfterLabelEdit += new LabelEditEventHandler(FileListView_AfterLabelEdit);

            ToolStripButton stb = new ToolStripButton("桌面");
            stb.Tag = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            stb.Click+= new EventHandler(QuickLaunchBarAction);
            TSQuickLaunchBar.Items.Add(stb);

            stb = new ToolStripButton("我的文档");
            stb.Tag = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            stb.Click += new EventHandler(QuickLaunchBarAction);
            TSQuickLaunchBar.Items.Add(stb);

            //添加 我的电脑 的子节点
            DriveInfo[] drivers = DriveInfo.GetDrives();
            foreach (DriveInfo  driver in drivers)
            {
                stb = new ToolStripButton(driver.Name);
                stb.Tag = driver.Name;
                stb.Click += new EventHandler(QuickLaunchBarAction);
        
                TSQuickLaunchBar.Items.Add(stb);
            }
            //直接打开c：盘
            this.QuickLaunchBarAction(TSQuickLaunchBar.Items[0], null);
        }

        private void QuickLaunchBarAction(object sender, EventArgs arg)
        {
            ToolStripItem tsi = sender as ToolStripItem;
            string strFolderName = tsi.Tag as string;
           
            if (strPrevDirName == strFolderName)//如果是相同的dir，就不动作
            {
                return;
            } strPrevDirName = strFolderName;
   
            this.FileTreeView.TreeRoot = strFolderName;

            UpdateUIAddressPath(strFolderName);
           
        }

        private void UpdateUIAddressPath(string strFolderName)
        {
            this.AddressBar.Text = strFolderName;
            this.Text = strFolderName;

            UpdateFolderListView(strFolderName);

        }
        private void OnTreeViewCollapse(object sender, TreeViewEventArgs e)
        {
            //PropertyMgr.treeDepth.CollapseTree(e.Node.Name);
            //this.TreeViewDir.Width = PropertyMgr.treeDepth.GetTreeDepth();
            this.UpdateUIAddressPath(e.Node.Name);
            //this.SPLITCTN_LEFTRIGHT.SplitterDistance = PropertyMgr.tdm.GetTreeDepth();
        }
        private void OnTreeViewExpand(object sender, TreeViewEventArgs e)
        {
            this.UpdateUIAddressPath(e.Node.Name);//更新视图
        }

        private void FileTreeView_SizeChanged(object sender, EventArgs e)
        {
            this.FileListView.Width = this.Width - this.FileTreeView.Width;
            Point p = this.FileListView.Location;
            Point pNew = new Point(this.FileTreeView.Location.X + this.FileTreeView.Width + 3, p.Y);
            this.FileListView.Location = pNew;
        }

        private void FileTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Tag == null)
                {
                    this.UpdateUIAddressPath(e.Node.Text);
                }
                else
                {
                    this.UpdateUIAddressPath(e.Node.Tag.ToString());
                }
            }
        }

        private void UpdateFolderListView(string folderPath)
        {
            this.FileListView.Items.Clear();
            this.FileListView.BackgroundImage = null;

            if (folderPath != null)
            {
                DirectoryInfo folder = new DirectoryInfo(folderPath);
                //添加子文件夹
                DirectoryInfo[] subFolders = null;
                try
                {
                    subFolders = folder.GetDirectories();
                }
                catch (Exception)
                { }
                //lisview视图中不要文件夹
                if (subFolders != null)
                {
                    foreach (DirectoryInfo subfolder in subFolders)
                    {
                        if ( this.IsShowHidden == false&&  subfolder.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            continue;
                        }

                        ListViewItem lvi = new ListViewItem(new string[] { subfolder.Name, "文件夹", "", "" }, "folder.ico");
                        lvi.Tag = subfolder.FullName;//directoryInfo
                        FileListView.Items.Add(lvi);
                    }
                }
                //填充文件
                FileInfo[] files = null;
                try
                {
                    files = folder.GetFiles();
                }
                catch (Exception)
                { }
                if (files != null)
                {
                    foreach (FileInfo fileInfo in files)
                    {
                        if (this.IsShowHidden == false && fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            continue;
                        }

                        string strExt = Path.GetExtension(fileInfo.Name);
                        if (!DirViewImageList.Images.ContainsKey(strExt))
                        {//没有该类型的图标则添加
                            DirViewImageList.Images.Add(strExt, FileAPI.GetFileIcon(fileInfo.Name, true));
                        }
                        ListViewItem lvi = new ListViewItem(new string[] { fileInfo.Name, "文件", FileAPI.GetFileSize(fileInfo), fileInfo.LastWriteTime.ToShortDateString() }, strExt);
                        lvi.Tag = fileInfo.FullName;//directoryInfo
                        FileListView.Items.Add(lvi);
                    }
                }
            }
        }
      
        private void tsbUp_Click(object sender, EventArgs e)
        {
            TreeNode node = FileTreeView.SelectedNode;
         
            TreeNode ParentNode = node.Parent;
            if (ParentNode!=null)
            {
                this.FileTreeView.SelectedNode = ParentNode;
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //this.TreeViewDir.Width = PropertyMgr.treeDepth.GetTreeDepth();
            this.FileListView.Width = this.Width - this.FileTreeView.Width;
            this.FileListView.Height = this.Height - 40;
            this.FileTreeView.Height = this.Height - 40;
            this.AddressBar.Width = this.Width - 10;// -this.pictureBox1.Width - this.pictureBox2.Width;
            this.TSQuickLaunchBar.Width = this.Width - 10;
            //this.SPLITCTN_LEFTRIGHT.SplitterDistance = PropertyMgr.tdm.GetTreeDepth();
        }

        /// <summary>
        /// 调整DIr各个栏目的宽度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListView_SizeChanged(object sender, EventArgs e)
        {
            this.columnHeader1.Width = (int)(0.45 * this.Width);
            this.columnHeader2.Width = (int)(0.1 * this.Width);
            this.columnHeader3.Width = (int)(0.1 * this.Width);
            this.columnHeader4.Width = (int)(0.25 * this.Width);
        }
        private void FileListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.FileListView.SelectedItems.Count==1)
            {
                string strFileName = this.FileListView.SelectedItems[0].SubItems[0].Text;
                if (strFileName.ToLower().EndsWith(".jpg"))
                {
                    string strJPGPath = Path.Combine(this.AddressBar.Text, strFileName);
                    Image old = this.FileListView.BackgroundImage;
                    Image img = ImageFast.FromFile(strJPGPath);
                    this.FileListView.BackgroundImage = new Bitmap(img,this.FileListView.Size);//strJPGPath,);
                    this.FileListView.BackgroundImageTiled = false;
                    img.Dispose();
                    if (old != null)
                    {
                        old.Dispose();
                    }
                }    
            }
            

        }
        private void FileListView_DoubleClick(object sender, EventArgs e)
        {
            if (FileListView.SelectedItems.Count > 0)
            {
                try
                {
                    //获取路径
                    string strFilePath = Path.Combine(this.AddressBar.Text, FileListView.SelectedItems[0].Text);
                    //如果路径是一个目录则调用打开
                    if (Directory.Exists(strFilePath))
                    {
                        TreeNode[] tnc = FileTreeView.Nodes.Find(strFilePath, true);
                        if (tnc.GetLength(0) != 0)
                        {
                            TreeNode tn = tnc[0];
                            FileTreeView.SelectedNode = tn;
                            tn.Expand();
                            this.UpdateUIAddressPath(strFilePath);
                        }
                    }
                    else
                    {
                        //双击打开
                        Process pc= Process.Start(strFilePath);
                        if (pc!=null)
                        {
                            pc.CloseMainWindow();
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(FileListView.SelectedItems[0].Text + "无法打开！");
                }
            }
        }
        private void FileListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //MessageBox.Show(oM.Location.ToString());
            if (e.Button==MouseButtons.Right)//鼠标右键
            {
                oM.Show();
                //Point p =  DirView.Location;
                //Rectangle rect = new Rectangle(new Point(p.X - 40, p.Y - 20), new Size(DirView.Width + 40, 20));
                this.Invalidate(oM.RectangleToScreen(oM.ClientRectangle));//重绘整个区域。防止出现没有
                //oM.Hide();
            }

            FileListView.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private void FileListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Cancel:
                    break;
                case Keys.Capital:
                    break;

                case Keys.Clear:
                    break;
                case Keys.Control:
                    break;
                case Keys.ControlKey:
                    break;
                case Keys.Decimal:
                    break;
                case Keys.Delete:// 删除键
                    if (FileListView.SelectedItems.Count>0)
                    {
                        ListViewItem lvi = FileListView.SelectedItems[0];
                        string strPath = lvi.Tag as string;// Path.Combine(this.AddressBar.Text, lvi.Text);
                        if (!string.IsNullOrEmpty(strPath))
                        {
                            DeleteItem(strPath);
                            ///
                            this.UpdateFolderListView(Path.GetDirectoryName(strPath));
                            //this.UpdateFolderListView(
                        }
                    }

                    break;
                case Keys.Divide:
                    break;
                case Keys.Down:
                    break;
                case Keys.E:
                    break;
                case Keys.End:
                    break;
                case Keys.Enter:
                   // MessageBox.Show(this.DirView.SelectedItems[0].Text);
                    break;
                case Keys.EraseEof:
                    break;
                case Keys.Escape:
                    break;
                case Keys.Execute:
                    break;
                case Keys.Exsel:
                    break;
                case Keys.F:
                    break;
                case Keys.F1:
                    break;
                case Keys.F10:
                    break;
                case Keys.F11:
                    break;
                case Keys.F12:
                    break;
                case Keys.F13:
                    break;
                case Keys.F14:
                    break;
                case Keys.F15:
                    break;
                case Keys.F16:
                    break;
                case Keys.F17:
                    break;
                case Keys.F18:
                    break;
                case Keys.F19:
                    break;
                case Keys.F2:
                    if (this.FileListView.SelectedItems.Count!=0)
                    {
                        //this.DirView.SelectedItems[0]
                        this.FileListView.LabelEdit = true;
                        ListViewItem lvi =FileListView.SelectedItems[0];
                        lvi.BeginEdit();
                        //DirView.it
                        //MessageBox.Show(this.DirView.SelectedItems[0].Text.ToString());
                    }
                    break;
                case Keys.F20:
                    break;
                case Keys.F21:
                    break;
                case Keys.F22:
                    break;
                case Keys.F23:
                    break;
                case Keys.F24:
                    break;
                case Keys.F3:
                    break;
                case Keys.F4:
                    break;
                case Keys.F5:
                    break;
                case Keys.F6:
                    break;
                case Keys.F7:
                    break;
                case Keys.F8:
                    break;
                case Keys.F9:
                    break;
                case Keys.FinalMode:
                    break;
                case Keys.G:
                    break;
                case Keys.H:
                    break;
                case Keys.HanguelMode:

                    break;
                case Keys.HanjaMode:
                    break;
                case Keys.Help:
                    break;
                case Keys.Home:
                    break;
                case Keys.I:
                    break;
                case Keys.IMEAccept:
                    break;

                case Keys.IMEConvert:
                    break;
                case Keys.IMEModeChange:
                    break;
                case Keys.IMENonconvert:
                    break;
                case Keys.Insert:
                    break;
                case Keys.J:
                    break;
                case Keys.JunjaMode:
                    break;
                case Keys.K:
                    break;

                case Keys.KeyCode:
                    break;
                case Keys.L:
                    break;
                case Keys.LButton:
                    break;
                case Keys.LControlKey:
                    break;
                case Keys.LMenu:
                    break;
                case Keys.LShiftKey:
                    break;
                case Keys.LWin:
                    break;
                case Keys.LaunchApplication1:
                    break;
                case Keys.LaunchApplication2:
                    break;
                case Keys.LaunchMail:
                    break;
                case Keys.Left:
                    break;
                case Keys.LineFeed:
                    break;
                case Keys.M:
                    break;
                case Keys.MButton:
                    break;
                case Keys.MediaNextTrack:
                    break;
                case Keys.MediaPlayPause:
                    break;
                case Keys.MediaPreviousTrack:
                    break;
                case Keys.MediaStop:
                    break;
                case Keys.Menu:
                    break;
                case Keys.Modifiers:
                    break;
                case Keys.Multiply:
                    break;
                case Keys.N:
                    break;
                case Keys.Next:
                    break;
                case Keys.NoName:
                    break;
                case Keys.None:
                    break;
                case Keys.NumLock:
                    break;
                case Keys.NumPad0:
                    break;
                case Keys.NumPad1:
                    break;
                case Keys.NumPad2:
                    break;
                case Keys.NumPad3:
                    break;
                case Keys.NumPad4:
                    break;
                case Keys.NumPad5:
                    break;
                case Keys.NumPad6:
                    break;
                case Keys.NumPad7:
                    break;
                case Keys.NumPad8:
                    break;
                case Keys.NumPad9:
                    break;
                case Keys.O:
                    break;
                case Keys.Oem1:
                    break;
                case Keys.Oem102:
                    break;
                case Keys.Oem2:
                    break;
                case Keys.Oem3:
                    break;
                case Keys.Oem4:
                    break;
                case Keys.Oem5:
                    break;
                case Keys.Oem6:
                    break;
                case Keys.Oem7:
                    break;
                case Keys.Oem8:
                    break;
                case Keys.P:
                    break;
                case Keys.Pa1:
                    break;
                case Keys.Packet:
                    break;
                case Keys.PageUp:
                    break;
                case Keys.Pause:
                    break;
                case Keys.Play:
                    break;
                case Keys.Print:
                    break;
                case Keys.PrintScreen:
                    break;

                case Keys.ProcessKey:
                    break;
                case Keys.Q:
                    break;
                case Keys.R:
                    break;
                case Keys.RButton:
                    break;
                case Keys.RControlKey:
                    break;
                case Keys.RMenu:
                    break;
                case Keys.RShiftKey:
                    break;
                case Keys.RWin:

                    break;
                case Keys.Right:
                    break;
                case Keys.S:
                    break;
                case Keys.Scroll:
                    break;
                case Keys.Select:
                    break;
                case Keys.SelectMedia:
                    break;
                case Keys.Separator:
                    break;
                case Keys.Shift:
                    break;
                case Keys.ShiftKey:
                    break;
                case Keys.Sleep:
                    break;
                case Keys.Space:
                    break;
                case Keys.Subtract:
                    break;
                case Keys.T:
                    break;
                case Keys.Tab:
                    break;
                case Keys.U:
                    break;
                case Keys.Up:
                    break;
                case Keys.V:
                    break;
                case Keys.VolumeDown:
                    break;
                case Keys.VolumeMute:
                    break;
                case Keys.VolumeUp:
                    break;
                case Keys.W:
                    break;
                case Keys.X:
                    break;
                case Keys.XButton1:
                    break;
                case Keys.XButton2:
                    break;
                case Keys.Y:
                    break;
                case Keys.Z:
                    break;
                case Keys.Zoom:
                    break;
                default:
                    break;
            }
        }

        private void DeleteItem(string strPath)
        {
            int index = strPath.LastIndexOf(Path.DirectorySeparatorChar);
            string text = strPath.Substring(index, strPath.Length -index );
            if (MessageBox.Show("删除 " + text.Trim(Path.DirectorySeparatorChar) + " ?", "删除确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {   string strFilePath = strPath;
                if (Directory.Exists(strPath))
                {
                    //删除树形目录
                    TreeNode[] tnc = FileTreeView.Nodes.Find(strFilePath, true);
                    if (tnc.GetLength(0) != 0)
                    {
                        TreeNode tn = tnc[0];
                        FileTreeView.Nodes.Remove(tn);
                    }//删除目录
                    Directory.Delete(strPath, true);
                    
                }else if (File.Exists(strPath))
                {
                    File.Delete(strPath);
                }
                //this.UpdateFolderListView(this.AddressBar.Text);
            }
        }
        //处理文件改名等
        void FileListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ListViewItem lvi = FileListView.Items[e.Item];
            if (e.Label != null)//名字已经修改了
            {
                //lvi.text旧名称。e.label新名称
                string strPath = lvi.Tag as string;// Path.Combine(this.AddressBar.Text, lvi.Text);
                if (Directory.Exists(strPath))
                {//目录改名
                    Directory.Move(strPath, Path.Combine(this.AddressBar.Text, e.Label));
                }
                else
                    if (File.Exists(strPath))
                    {//文件重命名
                        File.Move(strPath, Path.Combine(this.AddressBar.Text, e.Label));
                    }
                //File.Move(.n.Exists(this.CBAddress.Text.
            }
            //MessageBox.Show(lvi.Text+e.Label);
            //e.Item
            //throw new NotImplementedException();
        }

        const byte CtrlMask = 8;
     
        private void OptionMenu_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            // 如果 Ctrl 键没有被按下，移除源文字以便营造出移动文字的效果。
            if ((e.KeyState & CtrlMask) != CtrlMask)
            {
                MessageBox.Show(lvi.Text);//.Text = "";
            }
        }

        private void UsbInsert()
        {
            //_loading = true;
            //treeViewDisks.Nodes.Clear();

            //TreeNode root = treeViewDisks.Nodes.Add("Computer");
            //root.ImageIndex = (int)IconIndex.MyComputer;
            //root.SelectedImageIndex = root.ImageIndex;

            // display volumes
            VolumeDeviceClass volumeDeviceClass = new VolumeDeviceClass();
            //TreeNode volumesNode = new TreeNode("Volumes");
            //volumesNode.ImageIndex = (int)IconIndex.Volume;
            //volumesNode.SelectedImageIndex = volumesNode.ImageIndex;
            //root.Nodes.Add(volumesNode);

            foreach (Volume device in volumeDeviceClass.Devices)
            {
                if ((!device.IsUsb))// &&(usbOnlyToolStripMenuItem.Checked))
                    continue;

                //string text = null;
                //if ((device.LogicalDrive != null) && (device.LogicalDrive.Length > 0))
                //{
                //    text += device.LogicalDrive;
                //}

                //if (text != null)
                //{
                //    text += " ";
                //}
                //text += device.Description;
                //if (device.FriendlyName != null)
                //{
                //    if (text != null)
                //    {
                //        text += " - ";
                //    }
                //    text += device.FriendlyName;
                //}

                //TreeNode deviceNode = volumesNode.Nodes.Add(text);

                //if (device.IsUsb)
                //{
                //    deviceNode.ImageIndex = (int)IconIndex.Box;
                //    deviceNode.SelectedImageIndex = deviceNode.ImageIndex;
                //}
                //deviceNode.Tag = device;

                //foreach (Device disk in device.Disks)
                //{
                //    TreeNode diskNode = deviceNode.Nodes.Add(disk.Description + " - " + disk.FriendlyName);
                //    diskNode.ImageIndex = deviceNode.ImageIndex;
                //    diskNode.SelectedImageIndex = diskNode.ImageIndex;
                //    diskNode.Tag = device;
                //}
            }

            //root.ExpandAll();
            //_loading = false;
        }

        private void TSQuickLaunchBar_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            // 如果 Ctrl 键没有被按下，移除源文字以便营造出移动文字的效果。
            string text = lvi.Text;
            string dif = lvi.Tag as string;
            
            if (string.IsNullOrEmpty(dif)==false)
           {
               ToolStripItem sti = new ToolStripButton(text);
               sti.Tag = dif;
               sti.Click += new EventHandler(QuickLaunchBarAction);
               TSQuickLaunchBar.Items.Add(sti);//text, null, new EventHandler(QuickLaunchBarAction));
           }
        }

        private void TSQuickLaunchBar_DragEnter(object sender, DragEventArgs e)
        {
          // 检查被拖曳的数据的类型是否适用于目标控件。如果不适用，则拒绝置放。
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
        private void FileTreeView_KeyDown(object sender, KeyEventArgs e)
        {
               switch (e.KeyCode)
            {
                     case Keys.Delete:// 删除键
                    if (FileTreeView.SelectedNode!=null)
                    {
                        TreeNode tn = FileTreeView.SelectedNode;// FileListView.SelectedItems[0];
                        string strPath = tn.Tag as string;// lvi.Tag as string;// Path.Combine(this.AddressBar.Text, lvi.Text);
                        if (!string.IsNullOrEmpty(strPath))
                        {
                            DeleteItem(strPath);
                            this.FileTreeView.OnTreeNodeDelete(strPath);

                            this.UpdateUIAddressPath(FileTreeView.SelectedNode.Tag as string);
                        }
                    }

                    break;
            }
        }
    }
}