using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Absorb
{
    public partial class frmMenu : Form
    {
        
        
        // Track: Metal Gear Solid - Encounter (test.wav).
        private static System.IO.Stream menuTrack = Properties.Resources.encounter; // Gets the soundtrack.
        private System.Media.SoundPlayer playTrack = new System.Media.SoundPlayer(menuTrack); // Loads soundplayer to stream track.
        private bool playSound = true;


        frmRules showRulesForm = new frmRules();

        // Constructor
        public frmMenu()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.cardback1;
        }

        // Required when form loads.
        private void frmMenu_Load(object sender, EventArgs e)
        {
            // Plays soundtrack when menu form loads.
            playTrack.Play(); // Plays the track.
        }

        // When exit button is clicked.
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Closes program.
        }

        // When minimize button is clicked.
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // Minimizes form.
        }

        // When about button is clicked.
        private void btnAbout_Click(object sender, EventArgs e)
        {
            // Shows messagebox.
            MessageBox.Show("© Gerardo Blanco Bernal.\r\nModule: AC31009, 2020/21, " +
                "University of Dundee.\r\nSoundtracks: Metal Gear Sold - Encounter \r\n Halo - A walk in the woods." +
                "\r\nBackground: Menu - https://pixabay.com/illustrations/composing-ice-planet-3d-rendering-2288442/ \r\n", "About this program", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // When the rules button is clicked.
        private void btnRules_Click(object sender, EventArgs e)
        {
            // This block of code ensures only one rules form is open at any time.
            try
            {
                if (showRulesForm.Visible) // if rules form is already open.
                {
                    showRulesForm.BringToFront(); // brings form to front.
                }
                else
                {
                    showRulesForm.Show(); // opens rules form again.
                }
            }
            // showRulesForm.Visible() throws an exception if either frmRules has 
            // not been instantiated or if the user has closed the form.
            catch (Exception)
            {
                showRulesForm = new frmRules(); // creates new instance of rules form.
                showRulesForm.Show(); // displays form.
            }
        }

        // When the play button is clicked.
        private void btnPlay_Click(object sender, EventArgs e)
        {
            playTrack.Stop();
            playTrack.Dispose();

            frmAbsorb loadGame = new frmAbsorb();
            loadGame.Show();
            
            this.Hide();
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            if (playSound)
            {
                playTrack.Stop();
                playSound = false;
                btnSound.BackgroundImage = Properties.Resources.nosound;
            }
            else
            {
                playTrack.Play();
                playSound = true;
                btnSound.BackgroundImage = Properties.Resources.sound;
            }
        }
    }
}
