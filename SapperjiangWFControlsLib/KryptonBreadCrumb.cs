namespace ComponentFactory.Krypton.Toolkit
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch), DefaultProperty("RootItem"), Designer("ComponentFactory.Krypton.Toolkit.KryptonBreadCrumbDesigner, ComponentFactory.Krypton.Design, Version=4.3.1.0, Culture=neutral, PublicKeyToken=a87e673e9ecb6e8e"), DesignerCategory("code"), ToolboxItem(true), ToolboxBitmap(typeof(MyKryptonBreadCrumb), "ToolboxBitmaps.KryptonBreadCrumb.bmp"), DefaultEvent("SelectedItemChanged"), Description("Flat navigation of hierarchical data.")]
    public class MyKryptonBreadCrumb : VisualSimpleBase, ISupportInitializeNotification, ISupportInitialize
    {
        private bool _allowButtonSpecToolTips;
        private ButtonSpecManagerDraw _buttonManager;
        private BreadCrumbButtonSpecCollection _buttonSpecs;
        private ButtonStyle _buttonStyle;
        private ViewDrawDocker _drawDocker;
        private bool _dropDownNavigaton;
        private bool _initialized;
        private bool _initializing;
        private ViewLayoutCrumbs _layoutCrumbs;
        public KryptonBreadCrumbItem _rootItem;
        private KryptonBreadCrumbItem _selectedItem;
        private PaletteBreadCrumbRedirect _stateCommon;
        private PaletteBreadCrumbDoubleState _stateDisabled;
        private PaletteBreadCrumbDoubleState _stateNormal;
        private PaletteBreadCrumbState _statePressed;
        private PaletteBreadCrumbState _stateTracking;
        private ComponentFactory.Krypton.Toolkit.ToolTipManager _toolTipManager;
        private VisualPopupToolTip _visualPopupToolTip;
        private EventHandler<BreadCrumbMenuArgs> CrumbDropDown;
        private EventHandler Initialized;
        private EventHandler<ContextPositionMenuArgs> _OverflowDropDown;
        private EventHandler _SelectedItemChanged;

        //[Description("Occurs when the drop down portion of a bread crumb is pressed."), Category("Action")]
        //public event EventHandler<BreadCrumbMenuArgs> CrumbDropDown
        //{
        //    add
        //    {
        //        EventHandler<BreadCrumbMenuArgs> handler2;
        //        EventHandler<BreadCrumbMenuArgs> crumbDropDown = this.CrumbDropDown;
        //        do
        //        {
        //            handler2 = crumbDropDown;
        //            EventHandler<BreadCrumbMenuArgs> handler3 = (EventHandler<BreadCrumbMenuArgs>) Delegate.Combine(handler2, value);
        //            crumbDropDown = Interlocked.CompareExchange<EventHandler<BreadCrumbMenuArgs>>(ref this.CrumbDropDown, handler3, handler2);
        //        }
        //        while (crumbDropDown != handler2);
        //    }
        //    remove
        //    {
        //        EventHandler<BreadCrumbMenuArgs> handler2;
        //        EventHandler<BreadCrumbMenuArgs> crumbDropDown = this.CrumbDropDown;
        //        do
        //        {
        //            handler2 = crumbDropDown;
        //            EventHandler<BreadCrumbMenuArgs> handler3 = (EventHandler<BreadCrumbMenuArgs>) Delegate.Remove(handler2, value);
        //            crumbDropDown = Interlocked.CompareExchange<EventHandler<BreadCrumbMenuArgs>>(ref this.CrumbDropDown, handler3, handler2);
        //        }
        //        while (crumbDropDown != handler2);
        //    }
        //}

        [Category("Behavior"), Description("Occurs when the control has been fully initialized.")]
         event EventHandler ISupportInitializeNotification.Initialized
        {
            add
            {
                EventHandler handler2;
                EventHandler initialized = this.Initialized;
                do
                {
                    handler2 = initialized;
                    EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
                    initialized = Interlocked.CompareExchange<EventHandler>(ref this.Initialized, handler3, handler2);
                }
                while (initialized != handler2);
            }
            remove
            {
                EventHandler handler2;
                EventHandler initialized = this.Initialized;
                do
                {
                    handler2 = initialized;
                    EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
                    initialized = Interlocked.CompareExchange<EventHandler>(ref this.Initialized, handler3, handler2);
                }
                while (initialized != handler2);
            }
        }

        [Description("Occurs when the drop down portion of the overflow button is pressed."), Category("Action")]
        public event EventHandler<ContextPositionMenuArgs> OverflowDropDown
        {
            add
            {
                EventHandler<ContextPositionMenuArgs> handler2;
                EventHandler<ContextPositionMenuArgs> overflowDropDown = this._OverflowDropDown;
                do
                {
                    handler2 = overflowDropDown;
                    EventHandler<ContextPositionMenuArgs> handler3 = (EventHandler<ContextPositionMenuArgs>)Delegate.Combine(handler2, value);
                    overflowDropDown = Interlocked.CompareExchange<EventHandler<ContextPositionMenuArgs>>(ref this._OverflowDropDown, handler3, handler2);
                }
                while (overflowDropDown != handler2);
            }
            remove
            {
                EventHandler<ContextPositionMenuArgs> handler2;
                EventHandler<ContextPositionMenuArgs> overflowDropDown = this._OverflowDropDown;
                do
                {
                    handler2 = overflowDropDown;
                    EventHandler<ContextPositionMenuArgs> handler3 = (EventHandler<ContextPositionMenuArgs>)Delegate.Remove(handler2, value);
                    overflowDropDown = Interlocked.CompareExchange<EventHandler<ContextPositionMenuArgs>>(ref this._OverflowDropDown, handler3, handler2);
                }
                while (overflowDropDown != handler2);
            }
        }

        [Category("Property Changed"), Description("Occurs when the value of the SelectedItem property changes.")]
        public event EventHandler SelectedItemChanged
        {
            add
            {
                EventHandler handler2;
                EventHandler selectedItemChanged = this._SelectedItemChanged;
                do
                {
                    handler2 = selectedItemChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
                    selectedItemChanged = Interlocked.CompareExchange<EventHandler>(ref this._SelectedItemChanged, handler3, handler2);
                }
                while (selectedItemChanged != handler2);
            }
            remove
            {
                EventHandler handler2;
                EventHandler selectedItemChanged = this._SelectedItemChanged;
                do
                {
                    handler2 = selectedItemChanged;
                    EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
                    selectedItemChanged = Interlocked.CompareExchange<EventHandler>(ref this._SelectedItemChanged, handler3, handler2);
                }
                while (selectedItemChanged != handler2);
            }
        }

        public MyKryptonBreadCrumb()
        {
            base.SetStyle(ControlStyles.Selectable, false);
            this._selectedItem = null;
            this._dropDownNavigaton = true;
            this._buttonStyle = ButtonStyle.BreadCrumb;
            this._rootItem = new KryptonBreadCrumbItem();
            this._rootItem.PropertyChanged += new PropertyChangedEventHandler(this.OnCrumbItemChanged);
            this._allowButtonSpecToolTips = false;
            this._buttonSpecs = new BreadCrumbButtonSpecCollection(this);
            this._stateCommon = new PaletteBreadCrumbRedirect(base.Redirector, base.NeedPaintDelegate);
            this._stateDisabled = new PaletteBreadCrumbDoubleState(this._stateCommon, base.NeedPaintDelegate);
            this._stateNormal = new PaletteBreadCrumbDoubleState(this._stateCommon, base.NeedPaintDelegate);
            this._stateTracking = new PaletteBreadCrumbState(this._stateCommon, base.NeedPaintDelegate);
            this._statePressed = new PaletteBreadCrumbState(this._stateCommon, base.NeedPaintDelegate);
            //this._layoutCrumbs =new ViewLayoutCrumbs( new ViewLayoutCrumbs((KryptonBreadCrumb)this, base.NeedPaintDelegate);// new ViewLayoutCrumbs(this, base.NeedPaintDelegate);
            this._drawDocker = new ViewDrawDocker(this._stateNormal.Back, this._stateNormal.Border, null);
            //this._drawDocker.Add(this._layoutCrumbs, ViewDockStyle.Fill);
            base.ViewManager = new ViewManager(this, this._drawDocker);
            PaletteMetricPadding[] viewMetricPaddings = new PaletteMetricPadding[1];
            this._buttonManager = new ButtonSpecManagerDraw(this, base.Redirector, this._buttonSpecs, null, new ViewDrawDocker[] { this._drawDocker }, new IPaletteMetric[] { this._stateCommon }, new PaletteMetricInt[] { PaletteMetricInt.HeaderButtonEdgeInsetPrimary }, viewMetricPaddings, new GetToolStripRenderer(this.CreateToolStripRenderer), base.NeedPaintDelegate);
            this._toolTipManager = new ComponentFactory.Krypton.Toolkit.ToolTipManager();
            this._toolTipManager.ShowToolTip += new EventHandler<ToolTipEventArgs>(this.OnShowToolTip);
            this._toolTipManager.CancelToolTip += new EventHandler(this.OnCancelToolTip);
            this._buttonManager.ToolTipManager = this._toolTipManager;
        }

        public virtual void BeginInit()
        {
            this._initializing = true;
        }

        protected override PaletteRedirect CreateRedirector()
        {
            return new PaletteRedirectBreadCrumb(base.CreateRedirector());
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Component DesignerComponentFromPoint(Point pt)
        {
            if (base.IsDisposed)
            {
                return null;
            }
            return base.ViewManager.ComponentFromPoint(pt);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool DesignerGetHitTest(Point pt)
        {
            if (base.IsDisposed)
            {
                return false;
            }
            return ((this._buttonManager != null) && this._buttonManager.DesignerGetHitTest(pt));
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void DesignerMouseLeave()
        {
            this.OnMouseLeave(EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.OnCancelToolTip(this, EventArgs.Empty);
                this._buttonManager.Destruct();
            }
            base.Dispose(disposing);
        }

        public virtual void EndInit()
        {
            this._initialized = true;
            this._initializing = false;
            if (this.SelectedItem == null)
            {
                this.SelectedItem = this.RootItem;
            }
            this.OnNeedPaint(this, new NeedLayoutEventArgs(true));
            this.OnInitialized(EventArgs.Empty);
        }

        internal PaletteRedirect GetRedirector()
        {
            return base.Redirector;
        }

        internal PaletteBreadCrumbRedirect GetStateCommon()
        {
            return this._stateCommon;
        }

        protected override void OnButtonSpecChanged(object sender, EventArgs e)
        {
            this._buttonManager.RecreateButtons();
            base.OnButtonSpecChanged(sender, e);
        }

        private void OnCancelToolTip(object sender, EventArgs e)
        {
            if (this._visualPopupToolTip != null)
            {
                this._visualPopupToolTip.Dispose();
            }
        }

        protected internal virtual void OnCrumbDropDown(BreadCrumbMenuArgs e)
        {
            if (this.CrumbDropDown != null)
            {
                this.CrumbDropDown(this, e);
            }
        }

        private void OnCrumbItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "Items") && (this.SelectedItem != null))
            {
                KryptonBreadCrumbItem selectedItem = this.SelectedItem;
                while ((selectedItem != null) && (selectedItem != this.RootItem))
                {
                    selectedItem = selectedItem.Parent;
                }
                if (selectedItem == null)
                {
                    this.SelectedItem = null;
                }
            }
            this.PerformNeedPaint(true);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (base.Enabled)
            {
                this._drawDocker.SetPalettes(this._stateNormal.Back, this._stateNormal.Border);
            }
            else
            {
                this._drawDocker.SetPalettes(this._stateDisabled.Back, this._stateDisabled.Border);
            }
            this._drawDocker.Enabled = base.Enabled;
            this._buttonManager.RefreshButtons();
            this.PerformNeedPaint(true);
            base.OnEnabledChanged(e);
        }

        protected virtual void OnInitialized(EventArgs e)
        {
            if (this.Initialized != null)
            {
                this.Initialized(this, EventArgs.Empty);
            }
        }

        protected internal virtual void OnOverflowDropDown(ContextPositionMenuArgs e)
        {
            if (this._OverflowDropDown != null)
            {
                this._OverflowDropDown(this, e);
            }
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            if (this._SelectedItemChanged != null)
            {
                this._SelectedItemChanged(this, e);
            }
        }

        private void OnShowToolTip(object sender, ToolTipEventArgs e)
        {
            if (!base.IsDisposed)
            {
                Form form = base.FindForm();
                if (((form == null) || form.ContainsFocus) && !base.DesignMode)
                {
                    IContentValues contentValues = null;
                    LabelStyle toolTip = LabelStyle.ToolTip;
                    ButtonSpec buttonSpec = this._buttonManager.ButtonSpecFromView(e.Target);
                    if ((buttonSpec != null) && this.AllowButtonSpecToolTips)
                    {
                        ButtonSpecToContent content = new ButtonSpecToContent(base.Redirector, buttonSpec);
                        if (content.HasContent)
                        {
                            contentValues = content;
                            toolTip = buttonSpec.ToolTipStyle;
                        }
                    }
                    if (contentValues != null)
                    {
                        if (this._visualPopupToolTip != null)
                        {
                            this._visualPopupToolTip.Dispose();
                        }
                        this._visualPopupToolTip = new VisualPopupToolTip(base.Redirector, contentValues, base.Renderer, PaletteBackStyle.ControlToolTip, PaletteBorderStyle.ControlToolTip, CommonHelper.ContentStyleFromLabelStyle(toolTip));
                        this._visualPopupToolTip.Disposed += new EventHandler(this.OnVisualPopupToolTipDisposed);
                        this._visualPopupToolTip.ShowCalculatingSize(base.RectangleToScreen(e.Target.ClientRectangle));
                    }
                }
            }
        }

        private void OnVisualPopupToolTipDisposed(object sender, EventArgs e)
        {
            VisualPopupToolTip tip = (VisualPopupToolTip) sender;
            tip.Disposed += new EventHandler(this.OnVisualPopupToolTipDisposed);
            this._visualPopupToolTip = null;
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            return (((this.UseMnemonic && base.CanProcessMnemonic()) && this._buttonManager.ProcessMnemonic(charCode)) || base.ProcessMnemonic(charCode));
        }

        private void ResetControlBackStyle()
        {
            this.ControlBackStyle = PaletteBackStyle.PanelAlternate;
        }

        private void ResetControlBorderStyle()
        {
            this.ControlBorderStyle = PaletteBorderStyle.ControlClient;
        }

        private void ResetCrumbButtonStyle()
        {
            this.CrumbButtonStyle = ButtonStyle.BreadCrumb;
        }

        public virtual void SetFixedState(PaletteState state)
        {
            this._drawDocker.FixedState = state;
        }

        private bool ShouldSerializeControlBackStyle()
        {
            return (this.ControlBackStyle != PaletteBackStyle.PanelAlternate);
        }

        private bool ShouldSerializeControlBorderStyle()
        {
            return (this.ControlBorderStyle != PaletteBorderStyle.ControlClient);
        }

        private bool ShouldSerializeCrumbButtonStyle()
        {
            return (this.CrumbButtonStyle != ButtonStyle.BreadCrumb);
        }

        private bool ShouldSerializeStateCommon()
        {
            return !this._stateCommon.IsDefault;
        }

        private bool ShouldSerializeStateDisabled()
        {
            return !this._stateDisabled.IsDefault;
        }

        private bool ShouldSerializeStateNormal()
        {
            return !this._stateNormal.IsDefault;
        }

        private bool ShouldSerializeStatePressed()
        {
            return !this._statePressed.IsDefault;
        }

        private bool ShouldSerializeStateTracking()
        {
            return !this._stateTracking.IsDefault;
        }

        [DefaultValue(false), Description("Should tooltips be displayed for button specs."), Category("Visuals")]
        public bool AllowButtonSpecToolTips
        {
            get
            {
                return this._allowButtonSpecToolTips;
            }
            set
            {
                this._allowButtonSpecToolTips = value;
            }
        }

        [Browsable(true), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true), EditorBrowsable(EditorBrowsableState.Always), Localizable(true)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        [Category("Visuals"), Description("Collection of button specifications."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BreadCrumbButtonSpecCollection ButtonSpecs
        {
            get
            {
                return this._buttonSpecs;
            }
        }

        [Category("Visuals"), Description("Background style for the control.")]
        public PaletteBackStyle ControlBackStyle
        {
            get
            {
                return this._stateCommon.BackStyle;
            }
            set
            {
                if (this._stateCommon.BackStyle != value)
                {
                    this._stateCommon.BackStyle = value;
                    this.PerformNeedPaint(true);
                }
            }
        }

        [DefaultValue(typeof(PaletteBorderStyle), "Control - Client"), Description("Border style for the control."), Category("Visuals")]
        public PaletteBorderStyle ControlBorderStyle
        {
            get
            {
                return this._stateCommon.BorderStyle;
            }
            set
            {
                if (this._stateCommon.BorderStyle != value)
                {
                    this._stateCommon.BorderStyle = value;
                    this.PerformNeedPaint(true);
                }
            }
        }

        [Description("Button style used for drawing each bread crumb."), Category("Visuals")]
        public ButtonStyle CrumbButtonStyle
        {
            get
            {
                return this._buttonStyle;
            }
            set
            {
                if (this._buttonStyle != value)
                {
                    this._buttonStyle = value;
                    this._stateCommon.BreadCrumb.SetStyles(value);
                    this.PerformNeedPaint(true);
                }
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 0x1c);
            }
        }

        [DefaultValue(true), Description("Should drop down buttons allow navigation to children."), Category("Visuals")]
        public bool DropDownNavigation
        {
            get
            {
                return this._dropDownNavigaton;
            }
            set
            {
                if (this._dropDownNavigaton != value)
                {
                    this._dropDownNavigaton = value;
                    this.PerformNeedPaint(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool IsInitialized
        {
            [DebuggerStepThrough]
            get
            {
                return this._initialized;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        public bool IsInitializing
        {
            [DebuggerStepThrough]
            get
            {
                return this._initializing;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Root bread crumb item."), Category("Data")]
        public KryptonBreadCrumbItem RootItem
        {
            get
            {
                return this._rootItem;
            }
        }

        [Category("Data"), Description("Currently selected bread crumb item."), DefaultValue((string) null)]
        public KryptonBreadCrumbItem SelectedItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                if (value != this._selectedItem)
                {
                    KryptonBreadCrumbItem parent = value;
                    while ((parent != null) && (parent != this.RootItem))
                    {
                        parent = parent.Parent;
                    }
                    if ((value != null) && (parent == null))
                    {
                        throw new ArgumentOutOfRangeException("value", "Item must be inside the RootItem hierarchy.");
                    }
                    this._selectedItem = value;
                    this.OnSelectedItemChanged(EventArgs.Empty);
                    this.PerformNeedPaint(true);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Visuals"), Description("Overrides for defining common bread crumb appearance that other states can override.")]
        public PaletteBreadCrumbRedirect StateCommon
        {
            get
            {
                return this._stateCommon;
            }
        }

        [Description("Overrides for defining disabled appearance."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Visuals")]
        public PaletteBreadCrumbDoubleState StateDisabled
        {
            get
            {
                return this._stateDisabled;
            }
        }

        [Category("Visuals"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Overrides for defining normal appearance.")]
        public PaletteBreadCrumbDoubleState StateNormal
        {
            get
            {
                return this._stateNormal;
            }
        }

        [Category("Visuals"), Description("Overrides for defining pressed bread crumb appearance."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteBreadCrumbState StatePressed
        {
            get
            {
                return this._statePressed;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Overrides for defining tracking bread crumb appearance."), Category("Visuals")]
        public PaletteBreadCrumbState StateTracking
        {
            get
            {
                return this._stateTracking;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ComponentFactory.Krypton.Toolkit.ToolTipManager ToolTipManager
        {
            get
            {
                return this._toolTipManager;
            }
        }

        [Category("Appearance"), DefaultValue(true), Description("Defines if mnemonic characters generate click events for button specs.")]
        public bool UseMnemonic
        {
            get
            {
                return this._buttonManager.UseMnemonic;
            }
            set
            {
                if (this._buttonManager.UseMnemonic != value)
                {
                    this._buttonManager.UseMnemonic = value;
                    this.PerformNeedPaint(true);
                }
            }
        }

        public class BreadCrumbButtonSpecCollection : ButtonSpecCollection<ButtonSpecAny>
        {
            public BreadCrumbButtonSpecCollection(MyKryptonBreadCrumb owner) : base(owner)
            {
            }
        }

        //event EventHandler ISupportInitializeNotification.Initialized
        //{
        //    add { throw new NotImplementedException(); }
        //    remove { throw new NotImplementedException(); }
        //}
    }
}

