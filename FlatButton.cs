using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public partial class FlatButton : UserControl
    {
        public event EventHandler ButtonClick;

        public FlatButton()
        {
            InitializeComponent();
        }

        [DisplayName(@"Text"), Description("Null"), Category("Appearance")]
        public string Label_text
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
                ButtonClick(sender, e);
        }

        private void Label1_MouseEnter(object sender, EventArgs e) => this.BackColor = Colors.BUTTON_HOVER;

        private void Label1_MouseLeave(object sender, EventArgs e) => this.BackColor = Colors.BUTTON_NORMAL;
    }
}
