namespace MyFileBrowser
{
    using SapperJiangWFControlsLib;
    using ComponentFactory.Krypton.Ribbon;
    using ComponentFactory.Krypton.Toolkit;
    
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("");
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.SPLITCTN_UPDOWN = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.AddressBar = new SapperJiangWFControlsLib.WFMyCmBox();
            this.TSQuickLaunchBar = new SapperJiangWFControlsLib.MyToolStrip();
            this.FileTreeView = new SapperJiangWFControlsLib.WFMyTreeView();
            this.TreeViewImgList = new System.Windows.Forms.ImageList(this.components);
            this.FileListView = new SapperJiangWFControlsLib.WFMyListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DirViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.OptionMenuImagList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SPLITCTN_UPDOWN)).BeginInit();
            this.SPLITCTN_UPDOWN.Panel1.SuspendLayout();
            this.SPLITCTN_UPDOWN.Panel2.SuspendLayout();
            this.SPLITCTN_UPDOWN.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // skinEngine1
            // 
            this.skinEngine1.DefaultButtonStyle = Sunisoft.IrisSkin.DefaultButtonStyle.Shadow;
            this.skinEngine1.SerialNumber = "K207rBdgqXu4ktyQef5mcEO8dpFOITuEQiqZdd78c1DfzXLxbHDjaQ==";
            this.skinEngine1.SkinFile = null;
            this.skinEngine1.SkinStreamMain = ((System.IO.Stream)(resources.GetObject("skinEngine1.SkinStreamMain")));
            // 
            // SPLITCTN_UPDOWN
            // 
            this.SPLITCTN_UPDOWN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SPLITCTN_UPDOWN.IsSplitterFixed = true;
            this.SPLITCTN_UPDOWN.Location = new System.Drawing.Point(0, 0);
            this.SPLITCTN_UPDOWN.Name = "SPLITCTN_UPDOWN";
            this.SPLITCTN_UPDOWN.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SPLITCTN_UPDOWN.Panel1
            // 
            this.SPLITCTN_UPDOWN.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // SPLITCTN_UPDOWN.Panel2
            // 
            this.SPLITCTN_UPDOWN.Panel2.Controls.Add(this.FileTreeView);
            this.SPLITCTN_UPDOWN.Panel2.Controls.Add(this.FileListView);
            this.SPLITCTN_UPDOWN.Size = new System.Drawing.Size(962, 621);
            this.SPLITCTN_UPDOWN.SplitterDistance = 51;
            this.SPLITCTN_UPDOWN.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.AddressBar);
            this.flowLayoutPanel1.Controls.Add(this.TSQuickLaunchBar);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(962, 51);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // AddressBar
            // 
            this.AddressBar.AutoSize = false;
            this.AddressBar.DropDownNavigation = false;
            this.AddressBar.Location = new System.Drawing.Point(3, 3);
            this.AddressBar.Name = "AddressBar";
            // 
            // 
            // 
            this.AddressBar.RootItem.ShortText = "Root";
            this.AddressBar.SelectedItem = this.AddressBar.RootItem;
            this.AddressBar.Size = new System.Drawing.Size(679, 27);
            this.AddressBar.TabIndex = 7;
            // 
            // TSQuickLaunchBar
            // 
            this.TSQuickLaunchBar.AllowDrop = true;
            this.TSQuickLaunchBar.AutoSize = false;
            this.TSQuickLaunchBar.Dock = System.Windows.Forms.DockStyle.None;
            this.TSQuickLaunchBar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TSQuickLaunchBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.TSQuickLaunchBar.Location = new System.Drawing.Point(2, 33);
            this.TSQuickLaunchBar.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.TSQuickLaunchBar.Name = "TSQuickLaunchBar";
            this.TSQuickLaunchBar.Size = new System.Drawing.Size(960, 26);
            this.TSQuickLaunchBar.Stretch = true;
            this.TSQuickLaunchBar.TabIndex = 6;
            this.TSQuickLaunchBar.DragDrop += new System.Windows.Forms.DragEventHandler(this.TSQuickLaunchBar_DragDrop);
            this.TSQuickLaunchBar.DragEnter += new System.Windows.Forms.DragEventHandler(this.TSQuickLaunchBar_DragEnter);
            // 
            // FileTreeView
            // 
            this.FileTreeView.BackColor = System.Drawing.SystemColors.Window;
            this.FileTreeView.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FileTreeView.FullRowSelect = true;
            this.FileTreeView.HideSelection = false;
            this.FileTreeView.ImageIndex = 0;
            this.FileTreeView.ImageList = this.TreeViewImgList;
            this.FileTreeView.Location = new System.Drawing.Point(6, 0);
            this.FileTreeView.Name = "FileTreeView";
            treeNode1.Name = "";
            treeNode1.Text = "";
            this.FileTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.FileTreeView.SelectedImageIndex = 0;
            this.FileTreeView.Size = new System.Drawing.Size(232, 590);
            this.FileTreeView.TabIndex = 1;
            this.FileTreeView.TreeRoot = null;
            this.FileTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTreeView_AfterSelect);
            this.FileTreeView.SizeChanged += new System.EventHandler(this.FileTreeView_SizeChanged);
            this.FileTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileTreeView_KeyDown);
            // 
            // TreeViewImgList
            // 
            this.TreeViewImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeViewImgList.ImageStream")));
            this.TreeViewImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeViewImgList.Images.SetKeyName(0, "1.ico");
            this.TreeViewImgList.Images.SetKeyName(1, "2.ico");
            this.TreeViewImgList.Images.SetKeyName(2, "3.ico");
            this.TreeViewImgList.Images.SetKeyName(3, "4.ico");
            this.TreeViewImgList.Images.SetKeyName(4, "5.ico");
            this.TreeViewImgList.Images.SetKeyName(5, "6.ico");
            this.TreeViewImgList.Images.SetKeyName(6, "7.ico");
            this.TreeViewImgList.Images.SetKeyName(7, "8.ico");
            this.TreeViewImgList.Images.SetKeyName(8, "9.ico");
            this.TreeViewImgList.Images.SetKeyName(9, "10.ico");
            this.TreeViewImgList.Images.SetKeyName(10, "11.ico");
            this.TreeViewImgList.Images.SetKeyName(11, "12.ico");
            this.TreeViewImgList.Images.SetKeyName(12, "0.ico");
            // 
            // FileListView
            // 
            this.FileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.FileListView.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FileListView.Location = new System.Drawing.Point(246, -1);
            this.FileListView.Name = "FileListView";
            this.FileListView.Size = new System.Drawing.Size(712, 582);
            this.FileListView.SmallImageList = this.DirViewImageList;
            this.FileListView.StateImageList = this.DirViewImageList;
            this.FileListView.TabIndex = 5;
            this.FileListView.UseCompatibleStateImageBehavior = false;
            this.FileListView.View = System.Windows.Forms.View.Details;
            this.FileListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.FileListView_ItemDrag);
            this.FileListView.SelectedIndexChanged += new System.EventHandler(this.FileListView_SelectedIndexChanged);
            this.FileListView.SizeChanged += new System.EventHandler(this.FileListView_SizeChanged);
            this.FileListView.DoubleClick += new System.EventHandler(this.FileListView_DoubleClick);
            this.FileListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileListView_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 220;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "类型";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "大小";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "修改时间";
            this.columnHeader4.Width = 150;
            // 
            // DirViewImageList
            // 
            this.DirViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DirViewImageList.ImageStream")));
            this.DirViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.DirViewImageList.Images.SetKeyName(0, "folder.ico");
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(-1, 9);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(26, 596);
            this.miniToolStrip.TabIndex = 6;
            this.miniToolStrip.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            // 
            // OptionMenuImagList
            // 
            this.OptionMenuImagList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("OptionMenuImagList.ImageStream")));
            this.OptionMenuImagList.TransparentColor = System.Drawing.Color.Transparent;
            this.OptionMenuImagList.Images.SetKeyName(0, "Yahoo Messenger.png");
            this.OptionMenuImagList.Images.SetKeyName(1, "Control Panel.png");
            this.OptionMenuImagList.Images.SetKeyName(2, "Garbage Can.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(962, 621);
            this.Controls.Add(this.SPLITCTN_UPDOWN);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "我的资源浏览器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.SPLITCTN_UPDOWN.Panel1.ResumeLayout(false);
            this.SPLITCTN_UPDOWN.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SPLITCTN_UPDOWN)).EndInit();
            this.SPLITCTN_UPDOWN.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AddressBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private SapperJiangWFControlsLib.WFMyCmBox AddressBar;
        private System.Windows.Forms.SplitContainer SPLITCTN_UPDOWN;
        private SapperJiangWFControlsLib.MyToolStrip TSQuickLaunchBar;
        private SapperJiangWFControlsLib.WFMyTreeView FileTreeView;
        private SapperJiangWFControlsLib.WFMyListView FileListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.ImageList TreeViewImgList;
        private System.Windows.Forms.ImageList DirViewImageList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ImageList OptionMenuImagList;
    }
}

