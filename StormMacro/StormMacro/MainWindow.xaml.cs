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
using System.Windows.Shapes;

namespace StormMacro
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        /// <summary>
        /// the currently selected macro button
        /// </summary>
        MacroButton SelectedButton;

        bool isRecording = false;
        KeyHooks keys;

		public MainWindow()
		{
			this.InitializeComponent();

            MB_blue.MacroButtonSelected += _MacroButtonSelected;
            MB_brown.MacroButtonSelected += _MacroButtonSelected;
            MB_red.MacroButtonSelected += _MacroButtonSelected;
            MB_green.MacroButtonSelected += _MacroButtonSelected;
            MB_white.MacroButtonSelected += _MacroButtonSelected;
            MB_black.MacroButtonSelected += _MacroButtonSelected;

            //ButtoNTESTVALS();
            keys = new KeyHooks();
            keys.KeyPress += keys_KeyPress;

            loadmacros();
		}

        void keys_KeyPress(object sender, KeyHooks.KeyPressEventArgs e)
        {
            SelectedButton.TempKeys.Add(new KeyPress() { KeyVal = e.Key, IsKeyDown = e.IsKeyDown, ScanCode=e.Scancode });
            System.Diagnostics.Debug.WriteLine(e.Scancode);
        }

        private void ButtoNTESTVALS()
        {
            MB_blue.Keys.Add(new KeyPress() { KeyVal = (int)System.Windows.Input.Key.A, IsKeyDown = false });
            MB_blue.Keys.Add(new KeyPress() { KeyVal = (int)System.Windows.Input.Key.D0, IsKeyDown = true });

            MB_brown.Keys.Add(new KeyPress() { KeyVal = (int)System.Windows.Input.Key.B, IsKeyDown = false });
            MB_brown.Keys.Add(new KeyPress() { KeyVal = (int)System.Windows.Input.Key.LeftShift, IsKeyDown = true });

            MB_red.Keys.Add(new KeyPress() { KeyVal = (int)System.Windows.Input.Key.OemPipe, IsKeyDown = false });
            MB_red.Keys.Add(new KeyPress() { KeyVal = (int)System.Windows.Input.Key.VolumeMute, IsKeyDown = true });


        }

        void _MacroButtonSelected(object sender, MacroButtonSelectedEventArgs e)
        {
            if (isRecording)
            {
                //ignore the event
                ((MacroButton)sender).Selected = false;
                return;
            }
            else
            {
                //deselect other buttons
                foreach (MacroButton mb in MacroGrid.Children)
                {
                    if (!mb.Name.Equals(e.Name))
                    {
                        mb.Selected = false;
                    }
                }
                LB_KeyData.ItemsSource = ((MacroButton)sender).Keys;
                SelectedButton = ((MacroButton)sender);
            }
        }

        private void btn_record_Click(object sender, RoutedEventArgs e)
        {
            if (isRecording)
            {
                //stop recording
                isRecording = false;
                keys.Stop(false);
                if (SelectedButton.TempKeys.Count > 0)
                {
                    SelectedButton.Keys.Clear();
                    SelectedButton.Keys = new List<KeyPress>(SelectedButton.TempKeys);
                    SelectedButton.TempKeys.Clear();
                    LB_KeyData.ItemsSource = SelectedButton.Keys;
                }
                tb_status.Text = "Idle";
            }
            else
            {
                //start recording
                if (SelectedButton != null)
                {
                    isRecording = true;
                    keys.Start();
                    LB_KeyData.ItemsSource = SelectedButton.TempKeys;
                    tb_status.Text = "Recording...";
                }
                else
                {
                    System.Windows.MessageBox.Show("Please select a key first","error",MessageBoxButton.OK);
                   
                }
            }
        }

        private void btn_generatecode_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(
                CodeGen.generateCode(
                cb_scancode.IsChecked==true,
                MB_blue.Keys, MB_brown.Keys,
                MB_red.Keys, MB_green.Keys,
                MB_white.Keys, MB_black.Keys
                )
            );
//            string x = @"public enum KeysScancode
//{
//";

//            foreach (KeyPress k in MB_blue.Keys)
//            {
//                if (k.IsKeyDown)
//                {
//                    x += string.Format("{0}\t= 0x{1:X2},", k.Key, k.ScanCode);
//                    x += Environment.NewLine;
//                }
//            }
//            x += "};";
//            Clipboard.SetText(
//                x
//            );
            tb_status.Text = "Code Copied to Clipboard";
        }

        private void savemacros()
        {
            try
            {
                string s1, s2, s3, s4, s5, s6;
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        x.Serialize(xw, MB_blue.Keys);
                        s1 = sw.ToString();
                    }
                }
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        x.Serialize(xw, MB_brown.Keys);
                        s2 = sw.ToString();
                    }
                }
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        x.Serialize(xw, MB_red.Keys);
                        s3 = sw.ToString();
                    }
                }
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        x.Serialize(xw, MB_green.Keys);
                        s4 = sw.ToString();
                    }
                }
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        x.Serialize(xw, MB_white.Keys);
                        s5 = sw.ToString();
                    }
                }
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        x.Serialize(xw, MB_black.Keys);
                        s6 = sw.ToString();
                    }
                }

                System.Diagnostics.Debug.WriteLine(s1);
                System.Diagnostics.Debug.WriteLine(s2);
                Properties.Settings.Default.blue = s1;
                Properties.Settings.Default.brown = s2;
                Properties.Settings.Default.red = s3;
                Properties.Settings.Default.green = s4;
                Properties.Settings.Default.white = s5;
                Properties.Settings.Default.black = s6;
                Properties.Settings.Default.saved = true;
                Properties.Settings.Default.Save();
            }
            catch (Exception e) { }
        }

        private void loadmacros()
        {
            if (true == Properties.Settings.Default.saved)
            {
                try
                {
                    using (System.IO.TextReader tr =
                        new System.IO.StringReader(Properties.Settings.Default.blue))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        MB_blue.Keys = (List<KeyPress>)x.Deserialize(tr);
                    }
                    using (System.IO.TextReader tr =
                        new System.IO.StringReader(Properties.Settings.Default.brown))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        MB_brown.Keys = (List<KeyPress>)x.Deserialize(tr);
                    }
                    using (System.IO.TextReader tr =
                        new System.IO.StringReader(Properties.Settings.Default.red))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        MB_red.Keys = (List<KeyPress>)x.Deserialize(tr);
                    }
                    using (System.IO.TextReader tr =
                        new System.IO.StringReader(Properties.Settings.Default.green))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        MB_green.Keys = (List<KeyPress>)x.Deserialize(tr);
                    }
                    using (System.IO.TextReader tr =
                        new System.IO.StringReader(Properties.Settings.Default.white))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        MB_white.Keys = (List<KeyPress>)x.Deserialize(tr);
                    }
                    using (System.IO.TextReader tr =
                        new System.IO.StringReader(Properties.Settings.Default.black))
                    {
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<KeyPress>));
                        MB_black.Keys = (List<KeyPress>)x.Deserialize(tr);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("failed to load saved macros");
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            savemacros();
        }
	}
}