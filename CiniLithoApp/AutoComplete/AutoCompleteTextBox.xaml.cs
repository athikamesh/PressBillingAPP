using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace CiniLithoApp.AutoComplete
{
    /// <summary>
    /// Interaction logic for AutoCompleteTextBox.xaml
    /// </summary>    
    public partial class AutoCompleteTextBox : Canvas
    {
        

        #region Members
        private VisualCollection controls;
        private TextBox textBox;
        private ComboBox comboBox;
        private ObservableCollection<AutoCompleteEntry> autoCompletionList;
        private System.Timers.Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold;
        #endregion

        #region Constructor
        public AutoCompleteTextBox()
        {
            controls = new VisualCollection(this);
          //  searchThreshold = 2;
            InitializeComponent();
          
            autoCompletionList = new ObservableCollection<AutoCompleteEntry>();
                   // default threshold to 2 char

            // set up the key press timer
            keypressTimer = new System.Timers.Timer();
            keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            // set up the text box and the combo box
            comboBox = new ComboBox();
            comboBox.Focusable = true;
            comboBox.IsSynchronizedWithCurrentItem = true;
            comboBox.IsTabStop = false;
            //comboBox.Style = (Style)FindResource("MaterialDesignFloatingHintComboBox");
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
           
            comboBox.FontSize = 20;
           

            textBox = new TextBox();
            //textBox.Style = (Style)FindResource("MaterialDesignFloatingHintTextBox");
            textBox.Focusable = true;
            textBox.Background = Brushes.White;
            textBox.FontSize = 20;
            textBox.Height=40;
            textBox.GotMouseCapture += textBox_GotFocus;
            textBox.GotKeyboardFocus += textBox_GotFocus;
            textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            textBox.PreviewKeyDown += textBox_PreviewKeyDown;
            textBox.LostFocus += textBox_LostFocus;
            textBox.LostKeyboardFocus += TextBox_LostKeyboardFocus;   
            textBox.VerticalContentAlignment = VerticalAlignment.Center;


            controls.Add(comboBox);
            controls.Add(textBox);
        }

        

        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);

                    // Note: Based on your requirements you may also need to fire events for:
                    // RoutedEvent = Keyboard.PreviewKeyDownEvent
                    // RoutedEvent = Keyboard.KeyUpEvent
                    // RoutedEvent = Keyboard.PreviewKeyUpEvent
                }
            }
        }
        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.Tab))
            {
                comboBox.IsDropDownOpen = false;
            }
        }

        void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Threshold == 0)
            {
                TextChanged();
                comboBox.IsDropDownOpen = true;
                comboBox.Focusable = true;
                comboBox.Focus();
                textBox.Focusable = true;
                textBox.Focus();
            }
           
        }

        void comboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (comboBox.SelectedIndex != -1)
                {
                    ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
                    textBox.Text = cbItem.Content.ToString();
                    this.Focusable = true;
                    this.Focus();
                }
                else
                {
                    this.Focusable = true;
                    this.Focus();
                }
            }
        }

        void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                comboBox.Focusable = true;
                comboBox.Focus();
            }

        }
        void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

      
        #endregion

        #region Methods
        public string Text
        {
            get { return textBox.Text; }
            set 
            {
                insertText = true;
                textBox.Text = value; 
            }
        }

        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        public int Threshold
        {
            get { return searchThreshold; }
            set { searchThreshold = value; }
        }

        public void AddItem(AutoCompleteEntry entry)
        {
            autoCompletionList.Add(entry);
        }

        public void ClearItems() 
        {
            autoCompletionList.Clear();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != comboBox.SelectedItem)
            {
                insertText = true;
                ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
                textBox.Text = cbItem.Content.ToString();
                textBox.Focusable = true;
                textBox.Focus();
               
            }
        }

        private void TextChanged()
        {
            try
            {
                comboBox.Items.Clear();
                if (textBox.Text.Length >= Threshold)
                {
                    foreach (AutoCompleteEntry entry in autoCompletionList)
                    {
                        foreach (string word in entry.KeywordStrings)
                        {
                            if (word.StartsWith(textBox.Text, StringComparison.CurrentCultureIgnoreCase))
                            {
                                ComboBoxItem cbItem = new ComboBoxItem();
                                cbItem.Content = entry.ToString();
                                comboBox.Items.Add(cbItem);
                                break;
                            }
                        }
                    }
                    comboBox.IsDropDownOpen = true;
                }
                else
                {
                    comboBox.IsDropDownOpen = false;
                }
            }
            catch { }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(this.TextChanged));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // text was not typed, do nothing and consume the flag
            if (insertText == true) insertText = false;
            
            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            textBox.Arrange(new Rect(arrangeSize));
            comboBox.Arrange(new Rect(arrangeSize));
            return base.ArrangeOverride(arrangeSize);
        }
        protected override Visual GetVisualChild(int index)
        {
            return controls[index];
        }
        protected override int VisualChildrenCount
        {
            get { return controls.Count; }
        }
        #endregion
    }
}