namespace ComponentFactory.Krypton.Toolkit
{
    using System;
    using System.ComponentModel;
    using SapperJiangWFControlsLib;

    public class ButtonSpecHeaderGroup : ButtonSpecAny
    {
        private ComponentFactory.Krypton.Toolkit.HeaderLocation _location = ComponentFactory.Krypton.Toolkit.HeaderLocation.PrimaryHeader;

        public void CopyFrom(ButtonSpecHeaderGroup source)
        {
            this.HeaderLocation = source.HeaderLocation;
            base.CopyFrom(source);
        }

        public override ComponentFactory.Krypton.Toolkit.HeaderLocation GetLocation(IPalette palette)
        {
            return this.HeaderLocation;
        }

        public void ResetHeaderLocation()
        {
            this.HeaderLocation = ComponentFactory.Krypton.Toolkit.HeaderLocation.PrimaryHeader;
        }

        [DefaultValue(typeof(ComponentFactory.Krypton.Toolkit.HeaderLocation), "PrimaryHeader"), RefreshProperties(RefreshProperties.All), Localizable(true), Category("Visuals"), Description("Defines header location for the button.")]
        public ComponentFactory.Krypton.Toolkit.HeaderLocation HeaderLocation
        {
            get
            {
                return this._location;
            }
            set
            {
                if (this._location != value)
                {
                    this._location = value;
                    this.OnButtonSpecPropertyChanged("Location");
                }
            }
        }

        [Browsable(false)]
        public override bool IsDefault
        {
            get
            {
                return (base.IsDefault && (this.HeaderLocation == ComponentFactory.Krypton.Toolkit.HeaderLocation.PrimaryHeader));
            }
        }
    }
}

