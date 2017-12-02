using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SapperJiangWFControlsLib
{
    using ComponentFactory.Krypton.Ribbon;//.Toolkit;
    using ComponentFactory.Krypton.Toolkit;

    public class MyKryptionItem : KryptonBreadCrumbItem
    {
        public string strInnerLongText;
    }

    public partial class WFMyCmBox : KryptonBreadCrumb
    {
        

        private Stack<string> stDir = new Stack<string>();
        public WFMyCmBox()
        {
            InitializeComponent();
            this.DropDownNavigation = false;
            this.RootItem.Items.Clear();
            
            
            //this.
        }
        private bool bIsItemClicked = false;
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value==base.Text)
                {
                    return;
                }

                this.RootItem.Items.Clear();
                string temp = value.TrimEnd('\\');

               ///处理目录，层级压栈
                MyKryptionItem bt = null;
                Stack<KryptonBreadCrumbItem> stPath = new Stack<KryptonBreadCrumbItem>();
                int iIndex = -1;
                do
	            {
                    bt = new MyKryptionItem();
                    iIndex = temp.LastIndexOf('\\');
                    if (iIndex==-1)
                    {
                        bt.ShortText =temp;//.Substring(iIndex, temp.Length - iIndex);//
                        bt.strInnerLongText = temp;//.Substring(0, iIndex);//长文本
                        stPath.Push(bt);
                        break;
                    }else{
                            bt.ShortText =temp.Substring(iIndex, temp.Length - iIndex);//
                            bt.strInnerLongText = temp;//.Substring(0, iIndex);//长文本
                            //bt.Items.Add(stPath.Peek());
                            temp = temp.Substring(0,iIndex);// bt.strInnerLongText; 
                        stPath.Push(bt);
	                }
                   
                }while (iIndex>-1);

                //根目录出栈
                this.RootItem.ShortText = stPath.Pop().ShortText;//第一个
                //this.RootItem.LongText = this.RootItem.ShortText;
                //this.RootItem.s
                KryptonBreadCrumbItem itemTemp = this.RootItem;

                while (stPath.Count>0)
                {
                    itemTemp.Items.Add(stPath.Pop());
                    itemTemp = itemTemp.Items[0];
                }
                this.SelectedItem = itemTemp;//最后一个item

                base.Text = value;
            }
            
        }
        protected override void OnClick(EventArgs e)
        {
            this.bIsItemClicked = true;//只有单击的事件才激发
            base.OnClick(e);
        }
        protected override void OnSelectedItemChanged(EventArgs e)
        {
            if (this.bIsItemClicked == true)//只有单击的事件才激发
            {
                base.OnSelectedItemChanged(e);
                this.bIsItemClicked = false;                 
            }
        }
    }

}
