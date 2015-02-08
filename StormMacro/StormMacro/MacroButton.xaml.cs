using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StormMacro
{
	/// <summary>
	/// Interaction logic for MacroButton.xaml
	/// </summary>
	public partial class MacroButton : UserControl
	{

        /// <summary>
        /// this var is to be set to true when a mouse down event is fired by the left mouse button
        /// it will be checked by the mouse up event to ensure this control was properly clicked
        /// it will be set to false by mouse exit/leave event
        /// </summary>
        private bool _clicked = false;
        public bool Clicked { get { return _clicked; } }

        private bool _selected = false;
        public bool Selected { get { return _selected; } set { _selected = value; HoverRect.Visibility = System.Windows.Visibility.Hidden; } }

        /// <summary>
        /// list holding keys
        /// </summary>
        public List<KeyPress> Keys = new List<KeyPress>();
        /// <summary>
        /// temporary key list, will be used as binding when recording, and eventually 
        /// copied to Keys
        /// </summary>
        public System.ComponentModel.BindingList<KeyPress> TempKeys = new System.ComponentModel.BindingList<KeyPress>();


        /// <summary>
        /// event delegates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MacroButtonSelectedHandler(object sender, MacroButtonSelectedEventArgs e);
        public event MacroButtonSelectedHandler MacroButtonSelected;


		public MacroButton()
		{
			this.InitializeComponent();
            HoverRect.Visibility = System.Windows.Visibility.Hidden;
		}

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverRect.Visibility = System.Windows.Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_selected)
            {
                HoverRect.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _clicked = true;
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Clicked)
            {
                _selected = true;
                MacroButtonSelected(this, new MacroButtonSelectedEventArgs(this.Name));
            }
        }

        
	}


    #region events and errata


    public class MacroButtonSelectedEventArgs : EventArgs
    {
        public MacroButtonSelectedEventArgs(string BTN_NAME)
        {
            _btnname = BTN_NAME;
        }
        private string _btnname;
        public string Name
        {
            set
            {
                _btnname = value;
            }
            get
            {
                return _btnname;
            }
        }
    }

    #endregion
}