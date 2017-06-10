using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace VizualAlgoGeom
{
    public class KeyboardAdapter
    {

        readonly GLControl _control;

        private GLControl Canvas;
        internal event KeyEventHandler KeyEnter;

        public KeyboardAdapter(GLControl control)
        {
            _control = control;
            _control.KeyUp += control_KeyPressed;
        }

        private void control_KeyPressed(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == Keys.Enter)
            {
                FireEnterPressed(sender,e);
            }

        }

        private void FireEnterPressed(object sender, KeyEventArgs e)
        {
            if (KeyEnter != null)
                KeyEnter(sender, e);
        }


    }
}
