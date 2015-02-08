using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StormMacro
{
    public class KeyPress 
    {
        private int _key;
        private bool _KeyDown;

        /// <summary>
        /// _KeyDown will be true if a key is down, false if it is a keyup
        /// </summary>
        public bool IsKeyDown
        {      
            get{return _KeyDown;}
            set
            {
                _KeyDown = value;
            }
        }
        public int ScanCode { get; set; }
        public int KeyVal
        {
            get { return _key; }
            set { _key = value; }
        }

        public string KeyDown
        {
            get { return (_KeyDown ? "down.png" : "up.png"); }
        }

        public string Key
        {
            get { return Enum.GetName(typeof(KeyHooks.KeysWindows), _key); }
        }

       


    }
}
