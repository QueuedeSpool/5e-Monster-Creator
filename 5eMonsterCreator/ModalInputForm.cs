using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5eMonsterCreator
{
    public partial class ModalInputForm : Form
    {
        public ModalInputForm(String _variableName)
        {
            InitializeComponent();

            _variableName = _variableName.Replace('_',' ');

            valueLabel.Text = _variableName + " value:";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public decimal inputValue
        {
            get { return modalInputUpDown.Value; }
        }

        private void modalInputUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, 100);
        }

        private void modalInputUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void modalInputUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                okButton.PerformClick();
            }
        }
    }
}
