
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SapperJiangWFControlsLib
{
    public partial class WFMyTreeView : TreeView
    {
        public WFMyTreeView()
        {
            InitializeComponent();
        }
        public bool IsShowHidden = false;

        //internal class PropertyMgr
        //{
            //private static Font _font = new Font("宋体", 15.97f);
        internal class TreeDepthMgr
        {
            //private SortedList<string, int> ok = new SortedList<string, int>();
            //private SortedDictionary<string, int> nodeLength = new SortedDictionary<string, int>();
            private Dictionary<string, int> nodeLength = new Dictionary<string, int>();
            private List<int> depthList = new List<int>();
            internal TreeDepthMgr()
            {
                depthList.Add(200);
            }
            internal int GetTreeDepth()
            {
                int index = depthList.Count - 1;
                return depthList[index];
            }
            public void BuildNewTree()
            {
                this.nodeLength.Clear();
                this.depthList.Clear();
                this.depthList.Add(200);
            }
            public void ExpandTree(string strFullPath, int length)
            {
                if (!this.nodeLength.ContainsKey(strFullPath))
                {
                    this.nodeLength.Add(strFullPath, length);
                    depthList.Add(length);
                    depthList.Sort();
                }
            }
            //parent directory path
            public void CollapseTree(string strFullPath)
            {
                foreach (KeyValuePair<string, int> item in new Dictionary<string, int>(nodeLength))
                {
                    if (item.Key.Contains(strFullPath))
                    {
                        int length = nodeLength[item.Key];
                        nodeLength.Remove(item.Key);
                        depthList.Remove(length);
                    }
                }
            }
            //使用递归删除打开的标签
            //public void CollapseTree(TreeNode node)
            //{
            //    //foreach (TreeNode item in node.Nodes)
            //    //{
            //    //    if (nodeLength.ContainsKey(node.FullPath))
            //    //    {
            //    //        int length = nodeLength[node.FullPath];
            //    //        nodeLength.Remove(node.FullPath);
            //    //        depthList.Remove(length);
            //    //    }
            //    //    item.Collapse();
            //    //    CollapseTree(item);
            //    //}
            //}
        }
        private TreeDepthMgr treeDepthMgr = new TreeDepthMgr();


        private string treeRoot;

        public string TreeRoot
        {
            get { return treeRoot; }
            set { treeRoot = value;
            GetRootTreeView(value);
            }
        }

       private void GetRootTreeView(string strFolderName)
            {
                this.Nodes.Clear();
                //更新索引
                this.treeDepthMgr.BuildNewTree();

                TreeNode rootNode = new TreeNode(strFolderName);
                rootNode.Name = strFolderName;//nam是索引
                rootNode.Tag = strFolderName;//全路径
                this.Nodes.Add(rootNode);
 
                DirectoryInfo[] folders = null;// = di.GetDirectories();

                try
                {
                    folders = (new DirectoryInfo(strFolderName)).GetDirectories();
                    foreach (DirectoryInfo folder in folders)
                    {
                        if (this.IsShowHidden == false&&folder.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            continue;
                        }
                        TreeNode node = new TreeNode(folder.Name);
                        node.ImageKey = "0.ico";
                        node.SelectedImageKey = "0.ico";
                        node.Tag = folder.FullName;
                        node.Name = folder.FullName;//name索引 是节点的

                        rootNode.Nodes.Add(node);
                    }
                    rootNode.Expand();//借点展开
                }
                catch (Exception)
                {
                }
            }

        protected override void  OnAfterCollapse(TreeViewEventArgs e)
        {
            CollapseTreeNode(e.Node.Name);
        }
        //节点折叠
        public void CollapseTreeNode(string nodeName)
        {
            treeDepthMgr.CollapseTree(nodeName);
            TreeNode[] tn = this.Nodes.Find(nodeName,true);
            if (tn.GetLength(0)>0)
            {
                tn[0].Collapse();
            }
            else
            {
                this.GetRootTreeView(nodeName);
            }

            this.Width = this.treeDepthMgr.GetTreeDepth();
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            System.Drawing.Font ft = this.Font;
            Graphics g = this.CreateGraphics();
            int iTreeLevelLength = (e.Node.Level + 1) * this.Indent + 30;

            int iFileNameLength = 0;

            DirectoryInfo folder = null;
            foreach (TreeNode node in e.Node.Nodes)// 下一层
            {
                if (node.Tag == null)
                {
                    continue;
                }
                folder = new DirectoryInfo(node.Tag.ToString());

                //计算目录的长度，并且找到最长的长度
                SizeF sizeF = g.MeasureString(folder.Name, ft);
                if (sizeF.Width > iFileNameLength)
                {
                    iFileNameLength = (int)sizeF.Width;
                }
                //尝试添加第二层
                DirectoryInfo[] subFolders = null;
                try
                {
                    subFolders = folder.GetDirectories();
                }
                catch (System.Exception)
                {
                }

                if (subFolders != null && node.Nodes.Count != subFolders.GetLength(0))//第一次添加
                {
                    foreach (DirectoryInfo subFolder in subFolders)
                    {
                        //是否显示隐藏文件夹
                        if (this.IsShowHidden == true&&subFolder.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            continue;
                        }
                        TreeNode subNode = new TreeNode(subFolder.Name);
                        subNode.Tag = subFolder.FullName;
                        //名字作为节点索引。
                        subNode.Name = subFolder.FullName;
                        subNode.ImageKey = subNode.Level.ToString() + ".ico";
                        subNode.SelectedImageKey = subNode.Level.ToString() + ".ico";

                        node.Nodes.Add(subNode);

                    }
                }
            }
            g.Dispose();
            treeDepthMgr.ExpandTree(e.Node.Name, iFileNameLength + iTreeLevelLength);

            base.OnAfterExpand(e);

            this.Width = this.treeDepthMgr.GetTreeDepth();
             
            this.SelectedNode = e.Node;
        }

        public void OnTreeNodeDelete(string nodePath)
        {
            this.treeDepthMgr.CollapseTree(nodePath);
            this.Width = this.treeDepthMgr.GetTreeDepth();
        }
        
        //public override on

        ////选中目录树则更改地址栏和旁边的DirView
        //public override void TreeViewDir_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
        //    {
        //        if (e.Node.Tag == null)
        //        {
        //            this.UpdateUIAddressPath(e.Node.Text);
        //        }
        //        else
        //        {
        //            this.UpdateUIAddressPath(e.Node.Tag.ToString());
        //        }
        //    }
        //}

    }
}
//=======
//﻿using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using System.IO;

//namespace SapperJiangWFControlsLib
//{
//    public partial class WFMyTreeView : TreeView
//    {
//        public WFMyTreeView()
//        {
//            InitializeComponent();
//        }

//        //internal class PropertyMgr
//        //{
//            //private static Font _font = new Font("宋体", 15.97f);
//        internal class TreeDepthMgr
//        {
//            //private SortedList<string, int> ok = new SortedList<string, int>();
//            //private SortedDictionary<string, int> nodeLength = new SortedDictionary<string, int>();
//            private Dictionary<string, int> nodeLength = new Dictionary<string, int>();
//            private List<int> depthList = new List<int>();
//            internal TreeDepthMgr()
//            {
//                depthList.Add(200);
//            }
//            internal int GetTreeDepth()
//            {
//                int index = depthList.Count - 1;
//                return depthList[index];

//            }
//            public void BuildNewTree()
//            {
//                this.nodeLength.Clear();
//                this.depthList.Clear();
//                this.depthList.Add(200);
//            }
//            public void ExpandTree(string strFullPath, int length)
//            {
//                if (!this.nodeLength.ContainsKey(strFullPath))
//                {
//                    this.nodeLength.Add(strFullPath, length);
//                    depthList.Add(length);
//                    depthList.Sort();
//                }
//            }
//            public void CollapseTree(string strFullPath)
//            {
//                foreach (KeyValuePair<string, int> item in new Dictionary<string, int>(nodeLength))
//                {
//                    if (item.Key.Contains(strFullPath))
//                    {
//                        int length = nodeLength[item.Key];
//                        nodeLength.Remove(item.Key);
//                        depthList.Remove(length);
//                    }
//                }
//            }
//            //使用递归删除打开的标签
//            //public void CollapseTree(TreeNode node)
//            //{
//            //    //foreach (TreeNode item in node.Nodes)
//            //    //{
//            //    //    if (nodeLength.ContainsKey(node.FullPath))
//            //    //    {
//            //    //        int length = nodeLength[node.FullPath];
//            //    //        nodeLength.Remove(node.FullPath);
//            //    //        depthList.Remove(length);
//            //    //    }
//            //    //    item.Collapse();
//            //    //    CollapseTree(item);
//            //    //}
//            //}
//        }
//        private TreeDepthMgr treeDepthMgr = new TreeDepthMgr();


//        private string treeRoot;

//        public string TreeRoot
//        {
//            get { return treeRoot; }
//            set { treeRoot = value;
//            GetRootTreeView(value);
//            }
//        }

//       private void GetRootTreeView(string strFolderName)
//            {
//                this.Nodes.Clear();
//                //更新索引
//                this.treeDepthMgr.BuildNewTree();

//                TreeNode rootNode = new TreeNode(strFolderName);
//                rootNode.Name = strFolderName;//nam是索引
//                rootNode.Tag = strFolderName;//全路径
//                this.Nodes.Add(rootNode);


//                DirectoryInfo[] folders = null;// = di.GetDirectories();

//                try
//                {
//                    folders = (new DirectoryInfo(strFolderName)).GetDirectories();
//                    foreach (DirectoryInfo folder in folders)
//                    {
//                        TreeNode node = new TreeNode(folder.Name);
//                        node.ImageKey = "0.ico";
//                        node.SelectedImageKey = "0.ico";
//                        node.Tag = folder.FullName;
//                        node.Name = folder.FullName;//name索引 是节点的

//                        rootNode.Nodes.Add(node);
//                    }
//                    rootNode.Expand();//借点展开
//                }
//                catch (Exception)
//                {
//                }
//            }

//        protected override void  OnAfterCollapse(TreeViewEventArgs e)
//        {
//            CollapseTreeNode(e.Node.Name);
//        }
//        //节点折叠
//        public void CollapseTreeNode(string nodeName)
//        {
//            treeDepthMgr.CollapseTree(nodeName);
//            TreeNode[] tn = this.Nodes.Find(nodeName,true);
//            if (tn.GetLength(0)>0)
//            {
//                tn[0].Collapse();
//            }
//            else
//            {
//                this.GetRootTreeView(nodeName);
//            }

//            this.Width = this.treeDepthMgr.GetTreeDepth();
//        }

//        protected override void OnAfterExpand(TreeViewEventArgs e)
//        {
//            System.Drawing.Font ft = this.Font;
//            Graphics g = this.CreateGraphics();
//            int iTreeLevelLength = (e.Node.Level + 1) * this.Indent + 30;

//            int iFileNameLength = 0;

//            DirectoryInfo folder = null;
//            foreach (TreeNode node in e.Node.Nodes)// 下一层
//            {
//                if (node.Tag == null)
//                {
//                    continue;
//                }
//                folder = new DirectoryInfo(node.Tag.ToString());

//                //计算目录的长度，并且找到最长的长度
//                SizeF sizeF = g.MeasureString(folder.Name, ft);
//                if (sizeF.Width > iFileNameLength)
//                {
//                    iFileNameLength = (int)sizeF.Width;
//                }


//                //尝试添加第二层
//                DirectoryInfo[] subFolders = null;
//                try
//                {
//                    subFolders = folder.GetDirectories();
//                }
//                catch (System.Exception)
//                {
//                }

//                if (subFolders != null && node.Nodes.Count != subFolders.GetLength(0))//第一次添加
//                {
//                    foreach (DirectoryInfo subFolder in subFolders)
//                    {
//                        //是否显示隐藏文件夹
//                        if (subFolder.Attributes == FileAttributes.Hidden)
//                        {
//                            continue;
//                        }
//                        TreeNode subNode = new TreeNode(subFolder.Name);
//                        subNode.Tag = subFolder.FullName;
//                        //名字作为节点索引。
//                        subNode.Name = subFolder.FullName;
//                        subNode.ImageKey = subNode.Level.ToString() + ".ico";
//                        subNode.SelectedImageKey = subNode.Level.ToString() + ".ico";

//                        node.Nodes.Add(subNode);

//                    }
//                }
//            }

//            g.Dispose();
//            treeDepthMgr.ExpandTree(e.Node.Name, iFileNameLength + iTreeLevelLength);

//            base.OnAfterExpand(e);

//            //added by sapperjiang
//            this.Width = this.treeDepthMgr.GetTreeDepth();

//            this.SelectedNode = e.Node;

//        }
//        ////选中目录树则更改地址栏和旁边的DirView
//        //public override void TreeViewDir_AfterSelect(object sender, TreeViewEventArgs e)
//        //{
//        //    if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
//        //    {
//        //        if (e.Node.Tag == null)
//        //        {
//        //            this.UpdateUIAddressPath(e.Node.Text);
//        //        }
//        //        else
//        //        {
//        //            this.UpdateUIAddressPath(e.Node.Tag.ToString());
//        //        }
//        //    }
//        //}

//    }
//}
//>>>>>>> .r3
