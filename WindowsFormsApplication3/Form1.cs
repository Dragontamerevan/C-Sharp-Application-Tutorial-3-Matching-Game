using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        //Choose random icons for the squares
        Random random = new Random();

        //Each of these letters in an interesting icon in the Webdings font, and each icon appears twice in this list
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k", "b", "b", "v", "v", "w", "w", "z", "z"
        };

        //firstClicked points to the first label control that the player clicks (null if the player hasn't clicked on a label yet)
        Label firstClicked = null;

        //secondClicked points to the second label control that the player clicks
        Label secondClicked = null;

        public Form1()
        {
            InitializeComponent();

            //Randomise icons
            AssignIconsToSquares();
        }

        /// <summary>
        /// Assign each icon from the list of icons to a random square
        /// </summary>
        private void AssignIconsToSquares()
        {
            //An icon is pulled at random from the list and added to each label
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    //Set foreground color to match background color (
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            //Timer is only on after 2 non-matching icons have been shown to the player, ignore any clicks if the timer is running
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                //If clicked label is black, the player clicked an icon that has already been revealed, thus ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    //return tells the program to stop executing the method
                    return;

                //If firstClicked is null, this is the first pair that the player clicked, so set firstClicked to the label clicked, change its color to black & return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                //If the player gets this far, the timer isn't running and firstClicked isn't null, so this must be the second icon the player clicked, set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                //Check to see if player won
                CheckForWinner();

                //If the player clicks 2 matching icons, keep them black & reset firstClicked & second so the player can click another icon
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                //If the player gets this far, the player clicked two different icons, thus start the the timer to hide the icons
                timer1.Start();
            }
        }

        /// <summary>
        /// Timer starts when the player clicks two icon that don't match, so it counts 3/4 of a second and turns itself off & hides both icons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            //Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            //Reset firstClicked & secondClicked so next time a label is clicked, program knows it is first click
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Check every icon to see if it is matched, by comparing its foreground color to its background color if all of the icons are match, the player win
        /// </summary>
        private void CheckForWinner()
        {
            //Go through all of the labels in the TableLayoutPanel to check if its icon is matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel != null)
                    {
                        if (iconLabel.ForeColor == iconLabel.BackColor)
                            return;
                    }
                }
            }

            //If the loop din't return, it din't find any unmatched icons, which means the user won. Show a message & close the form
            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }
    }
}
