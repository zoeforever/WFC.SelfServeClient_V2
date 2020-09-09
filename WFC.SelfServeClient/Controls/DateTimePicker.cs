﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace WFC.SelfServeClient.Controls
{

    public enum DateTimePickerFormat { Long, Short, Time, Custom }

    [DefaultBindingProperty("Value")]
    public class DateTimePicker : Control
    {
        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }
        private CheckBox _checkBox;
        internal TextBox _textBox;
        private TextBlock _textBlock;
        private Popup _popUp;
        private Calendar _calendar;
        private BlockManager _blockManager;
        private string _defaultFormat = "yyyy-MM-dd HH:mm:ss";
        [Category("DateTimePicker")]
        public bool ShowCheckBox
        {
            get { return this._checkBox.Visibility == Visibility.Visible ? true : false; }
            set
            {
                if (value)
                    this._checkBox.Visibility = Visibility.Visible;
                else
                {
                    this._checkBox.Visibility = Visibility.Collapsed;
                    this.Checked = true;
                }
            }
        }

        [Category("DateTimePicker")]
        public bool ShowDropDown
        {
            get { return this._textBlock.Visibility == Visibility.Visible ? true : false; }
            set
            {
                if (value)
                    this._textBlock.Visibility = Visibility.Visible;
                else
                    this._textBlock.Visibility = Visibility.Collapsed;
            }
        }

        [Category("DateTimePicker")]
        public bool Checked
        {
            get { return this._checkBox.IsChecked.HasValue ? this._checkBox.IsChecked.Value : false; }
            set
            {
                this._checkBox.IsChecked = value;
                this._textBox.IsEnabled = value;
            }
        }

        [Category("DateTimePicker")]
        private string FormatString
        {
            get
            {
                switch (this.Format)
                {
                    case DateTimePickerFormat.Long:
                        return "dddd, MMMM dd, yyyy";
                    case DateTimePickerFormat.Short:
                        return "M/d/yyyy";
                    case DateTimePickerFormat.Time:
                        return "h:mm:ss tt";
                    case DateTimePickerFormat.Custom:
                        if (string.IsNullOrEmpty(this.CustomFormat))
                            return this._defaultFormat;
                        else
                            return this.CustomFormat;
                    default:
                        return this._defaultFormat;
                }
            }
        }
        private string _customFormat;
        [Category("DateTimePicker")]
        public string CustomFormat
        {
            get { return this._customFormat; }
            set
            {
                this._customFormat = value;
                this._blockManager = new BlockManager(this, this.FormatString);
            }
        }
        private DateTimePickerFormat _format;
        [Category("DateTimePicker")]
        public DateTimePickerFormat Format
        {
            get { return this._format; }
            set
            {
                this._format = value;
                this._blockManager = new BlockManager(this, this.FormatString);
            }
        }
        [Category("DateTimePicker")]
        public DateTime? Value
        {
            get
            {
                if (!this.Checked) return null;
                return (DateTime?)GetValue(ValueProperty);
            }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TheDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimePicker.OnValueChanged), new CoerceValueCallback(DateTimePicker.CoerceValue), true, System.Windows.Data.UpdateSourceTrigger.PropertyChanged));


        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as DateTimePicker).ShowCheckBox)
            {
                (d as DateTimePicker).Checked = e.NewValue == null ? false : true;
            }

          
            if (e.NewValue == null || !(d as DateTimePicker).Checked)
            {
                (d as DateTimePicker)._textBox.Text = "不限";
                (d as DateTimePicker)._calendar.SelectedDatesChanged -= new EventHandler<SelectionChangedEventArgs>((d as DateTimePicker).calendar_SelectedDatesChanged);
                (d as DateTimePicker)._calendar.SelectedDate = null;
                (d as DateTimePicker)._calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>((d as DateTimePicker).calendar_SelectedDatesChanged);
                if ((d as DateTimePicker).ShowCheckBox)
                {
                    (d as DateTimePicker).Checked = false;
                }
            }
            else
            {
                (d as DateTimePicker)._blockManager.Render();
            }
            RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(e.OldValue, e.NewValue);
            args.RoutedEvent = ValueChangedEvent;
            (d as DateTimePicker).RaiseEvent(args);
        }

        static object CoerceValue(DependencyObject d, object value)
        {
            return value;
        }

        internal DateTime InternalValue
        {
            get
            {
                DateTime? value = this.Value;
                if (value.HasValue) return value.Value;
                return DateTime.MinValue;
            }
        }

        public DateTimePicker()
        {
            //Debug.WriteLine("DateTimePicker");
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.Initializ();
                this._blockManager = new BlockManager(this, this.FormatString);
            }
        }

        private void Initializ()
        {
            //Debug.WriteLine("Initializ");
            this.Template = Application.Current.FindResource("DateTimePickerTemplate") as ControlTemplate; //this.GetTemplate();
            this.ApplyTemplate();
            this._checkBox = (CheckBox)this.Template.FindName("checkBox", this);
            this._textBox = (TextBox)this.Template.FindName("textBox", this);
            this._textBlock = (TextBlock)this.Template.FindName("textBlock", this);
            this._calendar = new Calendar();
            this._popUp = new Popup();
            this._popUp.StaysOpen = false;
            this._popUp.PlacementTarget = this._textBox;
            this._popUp.Placement = PlacementMode.Bottom;
            this._popUp.Child = new Border() { Background=Brushes.Transparent, Margin = new Thickness(0, -3, 0, -3), CornerRadius = new CornerRadius(5), Child = this._calendar };
            //this._popUp.Child = this._calendar;
            //this._checkBox.Checked += new RoutedEventHandler(_checkBox_Checked);
            //this._checkBox.Unchecked += new RoutedEventHandler(_checkBox_Checked);
            this._checkBox.Click += _checkBox_Click;
            this.MouseWheel += new MouseWheelEventHandler(Dameer_MouseWheel);
            this.Focusable = false;
            this._textBox.Cursor = Cursors.Arrow;
            this._textBox.AllowDrop = false;
            this._textBox.GotFocus += new RoutedEventHandler(_textBox_GotFocus);
            this._textBox.PreviewMouseUp += new MouseButtonEventHandler(_textBox_PreviewMouseUp);
            this._textBox.PreviewKeyDown += new KeyEventHandler(_textBox_PreviewKeyDown);
            this._textBox.ContextMenu = null;
            this._textBox.IsEnabled = this.Checked;
            this._textBox.IsReadOnly = true;
            this._textBox.IsReadOnlyCaretVisible = false;
            this._textBlock.MouseLeftButtonUp += new MouseButtonEventHandler(_textBlock_MouseLeftButtonUp);
            this._calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(calendar_SelectedDatesChanged);
        }

        private void _checkBox_Click(object sender, RoutedEventArgs e)
        {
            this._textBox.IsEnabled = this.Checked;

            if (this.Checked)
            {
                this.Value = this._oldDateTime == null ? DateTime.Now : this._oldDateTime;
            }
            else
            {
                this._oldDateTime = this.Value;
                this.Value = null;
            }
        }

        void _textBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("_button_Click");
            this._popUp.IsOpen = !(this._popUp.IsOpen);
        }

        private DateTime? _oldDateTime;
        void _checkBox_Checked(object sender, RoutedEventArgs e)
        {
            this._textBox.IsEnabled = this.Checked;

            if (this.Checked)
            {
                this.Value = this._oldDateTime;
            }
            else
            {
                this._oldDateTime = (DateTime?)this.GetValue(ValueProperty);
                this.Value = null;
            }
        }

        void Dameer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Debug.WriteLine("Dameer_MouseWheel");
            this._blockManager.Change(((e.Delta < 0) ? -1 : 1), true);
        }

        void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("_textBox_GotFocus");
            this._blockManager.ReSelect();
        }

        void _textBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("_textBox_PreviewMouseUp");
            this._blockManager.ReSelect();
        }

        void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine("_textBox_PreviewKeyDown");
            byte b = (byte)e.Key;

            if (e.Key == Key.Left)
                this._blockManager.Left();
            else if (e.Key == Key.Right)
                this._blockManager.Right();
            else if (e.Key == Key.Up)
                this._blockManager.Change(1, true);
            else if (e.Key == Key.Down)
                this._blockManager.Change(-1, true);
            if (b >= 34 && b <= 43)
                this._blockManager.ChangeValue(b - 34);
            if (b >= 74 && b <= 83)
                this._blockManager.ChangeValue(b - 74);
            if (!(e.Key == Key.Tab))
                e.Handled = true;
        }

        void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Checked = true;
            (((sender as Calendar).Parent as Border).Parent as Popup).IsOpen = false;
            DateTime selectedDate = (DateTime)e.AddedItems[0];
            this.Value = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, this.InternalValue.Hour, this.InternalValue.Minute, this.InternalValue.Second);
            this._blockManager.Render();
        }

        private ControlTemplate GetTemplate()
        {
            //Debug.WriteLine("GetTemplate");
            return (ControlTemplate)XamlReader.Parse(@"
        <ControlTemplate  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
            <Border BorderBrush=""Black"" BorderThickness=""0"" CornerRadius=""1"">
                <StackPanel Orientation=""Horizontal"" VerticalAlignment=""Center"" HorizontalAlignment=""Right"">
                    <CheckBox Name=""checkBox"" VerticalAlignment=""Center""/>
                    <TextBox Name=""textBox"" BorderThickness=""0"" VerticalAlignment=""Center""/>
                    <TextBlock Foreground=""#FF2B96CD"" Name=""textBlock"" Text=""▼"" VerticalAlignment=""Center""/>
                </StackPanel>
            </Border>
        </ControlTemplate>");
        }

        public override string ToString()
        {
            return this.InternalValue.ToString();
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(DateTimePicker));
        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }
    }
    internal class BlockManager
    {
        internal DateTimePicker _dameer;
        private List<Block> _blocks;
        private string _format;
        private Block _selectedBlock;
        private int _selectedIndex;
        public event EventHandler NeglectProposed;
        private string[] _supportedFormats = new string[] {
                "yyyy", "MMMM", "dddd",
                "yyy", "MMM", "ddd",
                "yy", "MM", "dd",
                "y", "M", "d",
                "HH", "H", "hh", "h",
                "mm", "m",
                "ss", "s",
                "tt", "t",
                "fff", "ff", "f",
                "K", "g"};

        public BlockManager(DateTimePicker dameer, string format)
        {
            //Debug.WriteLine("BlockManager");
            this._dameer = dameer;
            this._format = format;
            this._dameer.LostFocus += new RoutedEventHandler(_dameer_LostFocus);
            this._blocks = new List<Block>();
            this.InitBlocks();
        }

        private void InitBlocks()
        {
            //Debug.WriteLine("InitBlocks");
            foreach (string f in this._supportedFormats)
                this._blocks.AddRange(this.GetBlocks(f));
            this._blocks = this._blocks.OrderBy((a) => a.Index).ToList();
            this._selectedBlock = this._blocks[0];
            this.Render();
        }

        internal void Render()
        {
            //Debug.WriteLine("BlockManager.Render");
            int accum = 0;
            StringBuilder sb = new StringBuilder(this._format);
            foreach (Block b in this._blocks)
                b.Render(ref accum, sb);
            this._dameer._textBox.Text = this._format = sb.ToString();
            this.Select(this._selectedBlock);
        }

        private List<Block> GetBlocks(string pattern)
        {
            //Debug.WriteLine("GetBlocks");
            List<Block> bCol = new List<Block>();

            int index = -1;
            while ((index = this._format.IndexOf(pattern, ++index)) > -1)
                bCol.Add(new Block(this, pattern, index));
            this._format = this._format.Replace(pattern, (0).ToString().PadRight(pattern.Length, '0'));
            return bCol;
        }

        internal void ChangeValue(int p)
        {
            //Debug.WriteLine("ChangeValue");
            this._selectedBlock.Proposed = p;
            this.Change(this._selectedBlock.Proposed, false);
        }

        internal void Change(int value, bool upDown)
        {
            //Debug.WriteLine("Change");
            this._dameer.Value = this._selectedBlock.Change(this._dameer.InternalValue, value, upDown);
            if (upDown)
                this.OnNeglectProposed();
            this.Render();
        }

        internal void Right()
        {
            //Debug.WriteLine("Right");
            if (this._selectedIndex + 1 < this._blocks.Count)
                this.Select(this._selectedIndex + 1);
        }

        internal void Left()
        {
            //Debug.WriteLine("Left");
            if (this._selectedIndex > 0)
                this.Select(this._selectedIndex - 1);
        }

        private void _dameer_LostFocus(object sender, RoutedEventArgs e)
        {
            this.OnNeglectProposed();
        }

        protected virtual void OnNeglectProposed()
        {
            //Debug.WriteLine("OnNeglectProposed");
            EventHandler temp = this.NeglectProposed;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        internal void ReSelect()
        {
            //Debug.WriteLine("ReSelect");
            foreach (Block b in this._blocks)
                if ((b.Index <= this._dameer._textBox.SelectionStart) && ((b.Index + b.Length) >= this._dameer._textBox.SelectionStart))
                { this.Select(b); return; }
            Block bb = this._blocks.Where((a) => a.Index < this._dameer._textBox.SelectionStart).LastOrDefault();
            if (bb == null) this.Select(0);
            else this.Select(bb);
        }

        private void Select(int blockIndex)
        {
            //Debug.WriteLine("Select(int blockIndex)");
            if (this._blocks.Count > blockIndex)
                this.Select(this._blocks[blockIndex]);
        }

        private void Select(Block block)
        {
            //Debug.WriteLine("Select(Block block)");
            if (!(this._selectedBlock == block))
                this.OnNeglectProposed();
            this._selectedIndex = this._blocks.IndexOf(block);
            this._selectedBlock = block;
            this._dameer._textBox.Select(block.Index, block.Length);
        }
    }

    internal class Block
    {
        private BlockManager _blockManager;
        internal string Pattern { get; set; }
        internal int Index { get; set; }
        private int _length;
        internal int Length
        {
            get
            {
                //Debug.WriteLine("Length Get");
                return this._length;
            }
            set
            {
                //Debug.WriteLine("Length Set");
                this._length = value;
            }
        }
        private int _maxLength;
        private string _proposed;
        internal int Proposed
        {
            get
            {
                //Debug.WriteLine(string.Format("Proposed Get, {0}, {1}", this._proposed, this.Length));
                string p = this._proposed;
                return int.Parse(p.PadLeft(this.Length, '0'));
            }
            set
            {
                //Debug.WriteLine(string.Format("Proposed Set, {0}, {1}", this._proposed, this.Length));
                if (!(this._proposed == null) && this._proposed.Length >= this._maxLength)
                    this._proposed = value.ToString();
                else
                    this._proposed = string.Format("{0}{1}", this._proposed, value);
            }
        }

        public Block(BlockManager blockManager, string pattern, int index)
        {
            //Debug.WriteLine("Block");
            this._blockManager = blockManager;
            this._blockManager.NeglectProposed += new EventHandler(_blockManager_NeglectProposed);
            this.Pattern = pattern;
            this.Index = index;
            this.Length = this.Pattern.Length;
            this._maxLength = this.GetMaxLength(this.Pattern);
        }

        private int GetMaxLength(string p)
        {
            switch (p)
            {
                case "y":
                case "M":
                case "d":
                case "h":
                case "m":
                case "s":
                case "H":
                    return 2;
                case "yyy":
                    return 4;
                default:
                    return p.Length;
            }
        }

        private void _blockManager_NeglectProposed(object sender, EventArgs e)
        {
            //Debug.WriteLine("_blockManager_NeglectProposed");
            this._proposed = null;
        }

        internal DateTime Change(DateTime dateTime, int value, bool upDown)
        {
            //Debug.WriteLine("Change(DateTime dateTime, int value, bool upDown)");
            if (!upDown && !this.CanChange()) return dateTime;
            int y, m, d, h, n, s;
            y = dateTime.Year;
            m = dateTime.Month;
            d = dateTime.Day;
            h = dateTime.Hour;
            n = dateTime.Minute;
            s = dateTime.Second;

            if (this.Pattern.Contains("y"))
                y = ((upDown) ? dateTime.Year + value : value);
            else if (this.Pattern.Contains("M"))
                m = ((upDown) ? dateTime.Month + value : value);
            else if (this.Pattern.Contains("d"))
                d = ((upDown) ? dateTime.Day + value : value);
            else if (this.Pattern.Contains("h") || this.Pattern.Contains("H"))
                h = ((upDown) ? dateTime.Hour + value : value);
            else if (this.Pattern.Contains("m"))
                n = ((upDown) ? dateTime.Minute + value : value);
            else if (this.Pattern.Contains("s"))
                s = ((upDown) ? dateTime.Second + value : value);
            else if (this.Pattern.Contains("t"))
                h = ((h < 12) ? (h + 12) : (h - 12));

            if (y > 9999) y = 1;
            if (y < 1) y = 9999;
            if (m > 12) m = 1;
            if (m < 1) m = 12;
            if (d > DateTime.DaysInMonth(y, m)) d = 1;
            if (d < 1) d = DateTime.DaysInMonth(y, m);
            if (h > 23) h = 0;
            if (h < 0) h = 23;
            if (n > 59) n = 0;
            if (n < 0) n = 59;
            if (s > 59) s = 0;
            if (s < 0) s = 59;

            return new DateTime(y, m, d, h, n, s);
        }

        private bool CanChange()
        {
            switch (this.Pattern)
            {
                case "MMMM":
                case "dddd":
                case "MMM":
                case "ddd":
                case "g":
                    return false;
                default:
                    return true;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Pattern, this.Index);
        }

        internal void Render(ref int accum, StringBuilder sb)
        {
            //Debug.WriteLine("Block.Render");
            this.Index += accum;

            string f = this._blockManager._dameer.InternalValue.ToString(this.Pattern + ",").TrimEnd(',');
            sb.Remove(this.Index, this.Length);
            sb.Insert(this.Index, f);
            accum += f.Length - this.Length;

            this.Length = f.Length;
        }
    }
}
