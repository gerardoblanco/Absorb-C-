using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Absorb
{
    public partial class frmRules : Form
    {
        public frmRules()
        {
            InitializeComponent();
            frmRules_Load();
        }

        private void frmRules_Load()
        {
            StreamReader s = File.OpenText(@"rules.txt"); // Open file.

            string rules = null;
            while ((rules = s.ReadLine()) != null)  // Read from file until done   
            {
                txtBoxRules.AppendText(rules + Environment.NewLine); // Displays to textbox.
            }
            s.Close(); // Closes stream.
        }
    }
}
