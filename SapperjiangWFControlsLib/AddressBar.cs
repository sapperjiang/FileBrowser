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
    public class MyButton : Button
    {
        private string longText;
        private int order;

        public MyButton() 
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value+@"\";
            }
        }
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public string LongText
        {
            get { return longText; }
            set { longText = value; }
        }

    }
    public partial class AddressBar : FlowLayoutPanel
    {
        SortedDictionary<int, Button> dirButton;
        FlowLayoutPanel plAddressContainer;
        public Color skinBtColor = Color.SkyBlue;
        public int iBarWidth= 30;
        TextBox tbAddress = new TextBox();

        public AddressBar()
        {
            InitializeComponent();

            plAddressContainer = this.flowLayoutPanel1;// new Panel();
            dirButton = new SortedDictionary<int, Button>();
            plAddressContainer.Click += new EventHandler(AddressBar_Click);
            plAddressContainer.LostFocus += new EventHandler(plAddressContainer_LostFocus);
            plAddressContainer.SizeChanged += new EventHandler(plAddressContainer_SizeChanged);
            tbAddress.Visible = false;
        //plAddressContainer.wid
        }

        void plAddressContainer_SizeChanged(object sender, EventArgs e)
        {
            tbAddress.Width = plAddressContainer.Width;

            for (int i = 0; i < dirButton.Count; i++)
            {
                dirButton[i].Width = plAddressContainer.Width;
            }
            //throw new NotImplementedException();
        }

        void plAddressContainer_LostFocus(object sender, EventArgs e)
        {
            tbAddress.Visible = false;
            //for (int i = 0; i < dirButton.Count; i++)
            //{
            //    dirButton[i].Visible = true;
            //}
            foreach (var item in dirButton.Keys)
            {
                dirButton[item].Visible = true;
            }
            //throw new NotImplementedException();
        }

        void AddressBar_Click(object sender, EventArgs e)
        {

            foreach (var item in dirButton.Keys)
            {
                dirButton[item].Visible = false;                
            }
            //tbAddress.Text = (dirButton[iCount-1] as MyButton).LongText;
            tbAddress.Visible = true;
            //throw new NotImplementedException();
        }
        //public override int Width
        //{
        //    get { return base.Width ; }
        //    set { base.Width = value;
        //    tbAddress.Width = value;
        //    }
        //}
        //public overrid
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                string text = value;
                //set textbox Text value
                this.tbAddress.Text = value;

                base.Text = text;
                int iIndex = text.LastIndexOf('\\');
                int iOrder = 0;
                while (iIndex > -1)
	            {
                    //clear all mybuttons controls
                    dirButton.Clear();
     
                    //Create text
                    MyButton bt = new MyButton();
                    //text.su
                    bt.Text = text.Substring(iIndex,text.Length-iIndex);//
                    bt.LongText = text.Substring(0,iIndex);//长文本
                    bt.BackColor =skinBtColor;
                    bt.Width = iBarWidth;
                    bt.Order =iOrder++; 
                    dirButton.Add(bt.Order,bt);
                    bt.Click += new EventHandler(AddressButton_Click);

                    text = bt.LongText;
                    iIndex = text.LastIndexOf('\\');
	            } 

                //Clear Controls
                plAddressContainer.Controls.Clear();
                
                //add controls

                foreach (var item in dirButton.Keys)
                {
                    plAddressContainer.Controls.Add(dirButton[item]);
                }
                //for (int i = dirButton.Keys.Count; i >=0 ; i--)
                //{//Add button to Panel by reverse order;
                //    plAddressContainer.Controls.Add(dirButton[i]);
                //}
            }
        }

        void AddressButton_Click(object sender, EventArgs e)
        {
            MyButton mb = sender as MyButton;
            if (mb!=null)
	        {
		         this.Text = mb.LongText;//重新赋值
	        }
            //throw new NotImplementedException();
        }
    }
}
