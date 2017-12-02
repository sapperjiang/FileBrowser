namespace ComponentFactory.Krypton.Toolkit
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;

    [Designer("ComponentFactory.Krypton.Toolkit.KryptonBreadCrumbItemDesigner, ComponentFactory.Krypton.Design, Version=4.3.1.0, Culture=neutral, PublicKeyToken=a87e673e9ecb6e8e"), ToolboxBitmap(typeof(MyKryptonBreadCrumb), "ToolboxBitmaps.KryptonBreadCrumbItem.bmp"), DesignTimeVisible(false), ToolboxItem(false)]
    public class KryptonBreadCrumbItem : KryptonListItem
    {
        private BreadCrumbItems _items;
        private KryptonBreadCrumbItem _parent;

        public KryptonBreadCrumbItem() : this("ListItem", null, null, Color.Empty)
        {
        }

        public KryptonBreadCrumbItem(string shortText) : this(shortText, null, null, Color.Empty)
        {
        }

        public KryptonBreadCrumbItem(string shortText, string longText) : this(shortText, longText, null, Color.Empty)
        {
        }

        public KryptonBreadCrumbItem(string shortText, string longText, Image image) : this(shortText, longText, image, Color.Empty)
        {
        }

        public KryptonBreadCrumbItem(string shortText, string longText, Image image, Color imageTransparentColor) : base(shortText, longText, image, imageTransparentColor)
        {
            this._items = new BreadCrumbItems(this);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            KryptonBreadCrumbItem parent = this.Parent;
            if (parent != null)
            {
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                }
                parent.OnPropertyChanged(e);
            }
        }

        public override string ToString()
        {
            return ("(" + this._items.Count.ToString() + ") " + base.ShortText);
        }

        [EditorBrowsable(EditorBrowsableState.Always), Category("Data"), Description("Collection of child items."), RefreshProperties(RefreshProperties.All), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BreadCrumbItems Items
        {
            get
            {
                return this._items;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Advanced)]
        public KryptonBreadCrumbItem Parent
        {
            get
            {
                return this._parent;
            }
            internal set
            {
                this._parent = value;
            }
        }

        [Editor("ComponentFactory.Krypton.Toolkit.KryptonBreadCrumbItemsEditor, ComponentFactory.Krypton.Design, Version=4.3.1.0, Culture=neutral, PublicKeyToken=a87e673e9ecb6e8e", typeof(UITypeEditor))]
        public class BreadCrumbItems : TypedCollection<KryptonBreadCrumbItem>
        {
            private KryptonBreadCrumbItem _owner;

            internal BreadCrumbItems(KryptonBreadCrumbItem owner)
            {
                this._owner = owner;
            }

            protected override void OnCleared(EventArgs e)
            {
                base.OnCleared(e);
                this._owner.OnPropertyChanged(new PropertyChangedEventArgs("Items"));
            }

            protected override void OnClearing(EventArgs e)
            {
                foreach (KryptonBreadCrumbItem item in this)
                {
                    item.Parent = null;
                }
                base.OnClearing(e);
            }

            protected override void OnInserted(TypedCollectionEventArgs<KryptonBreadCrumbItem> e)
            {
                base.OnInserted(e);
                this._owner.OnPropertyChanged(new PropertyChangedEventArgs("Items"));
            }

            protected override void OnInserting(TypedCollectionEventArgs<KryptonBreadCrumbItem> e)
            {
                e.Item.Parent = this._owner;
                base.OnInserting(e);
            }

            protected override void OnRemoved(TypedCollectionEventArgs<KryptonBreadCrumbItem> e)
            {
                base.OnRemoved(e);
                e.Item.Parent = null;
                this._owner.OnPropertyChanged(new PropertyChangedEventArgs("Items"));
            }

            public override KryptonBreadCrumbItem this[string name]
            {
                get
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        foreach (KryptonBreadCrumbItem item in this)
                        {
                            string shortText = item.ShortText;
                            if (!(string.IsNullOrEmpty(shortText) || !(shortText == name)))
                            {
                                return item;
                            }
                            shortText = item.LongText;
                            if (!(string.IsNullOrEmpty(shortText) || !(shortText == name)))
                            {
                                return item;
                            }
                        }
                    }
                    return null;
                }
            }
        }
    }
}

