using System;
using System.IO;
using System.Windows.Forms;

namespace Client
{
    public partial class frmClient : Form
    {
        // Setup Connection class
        ClientConnection clientConnection = new ClientConnection();
        TimeStamp timeStamp = new TimeStamp();

        public frmClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show connect box.
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "CONNECT")
            {
                // Show form
                connection.Visible = true;
            } else if (btnConnect.Text == "DISCONNECT")
            {
                // Close the connection
                clientConnection.GetClient.Close();

                // Update local status
                updateStatus(false);

                // Update button
                btnConnect.Text = "CONNECT";
            }
        }

        /// <summary>
        /// Cancel/Close connect box.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Hide connection form
            connection.Visible = false;
        }

        /// <summary>
        /// Connect to server based on given information in form
        /// </summary>
        private void btnGo_Click(object sender, EventArgs e)
        {
            int port = 0;
            Int32.TryParse(txtPort.Text, out port);

            // Report status of connection
            rtStatus.Text += "\n" + timeStamp.GetCurrentTimeStamp + "Connecting...";

            // Connect to specified server
            clientConnection.ConnectToServer(txtIP.Text, port);

            // Check the connection
            if (clientConnection.IsConnected)
            {
                // If connection successful, change the button
                btnConnect.Text = "DISCONNECT";
            }

            // Report Connection Status
            rtStatus.Text += "'\n\n" + timeStamp.GetCurrentTimeStamp + clientConnection.RequestConnectionStatus;

            // Receive stuff testing

            updateStatus(clientConnection.GetClient.Connected);

            // close connection form
            connection.Visible = false;
        }

        /// <summary>
        /// Even to stay at the end of the textbox for ease of readability and access
        /// </summary>
        private void rtStatus_TextChanged(Object sender, EventArgs e)
        {
            // Stay at the end of the text in status frame
            rtStatus.SelectionStart = rtStatus.Text.Length;
            rtStatus.ScrollToCaret();
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void bntExit_Click(object sender, EventArgs e)
        {
            // Check if connection open and close it before exiting
            if (clientConnection.GetClient.Connected)
            {
                // Clean up
                clientConnection.GetClient.Close();
            }

            // Exit
            Application.Exit();
        }

        /// <summary>
        /// Send contents of form to the server.
        /// </summary>
        private void btnSend_Click(object sender, EventArgs e)
        {
            // Tramit text to server
            clientConnection.TransmitText(rtTransmissionText.Text, clientConnection.GetStream);
            rtStatus.Text += "\n\n" + timeStamp.GetCurrentTimeStamp + clientConnection.RequestConnectionStatus;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the textbox
            rtTransmissionText.Clear();
        }

        /// <summary>
        /// Change status in bottom right corner and button name
        /// </summary>
        /// <param name="connectionStatus">Get conenction status.</param>
        private void updateStatus(Boolean connectionStatus)
        {
            if (connectionStatus)
            {
                status.BackColor = System.Drawing.Color.Yellow;
                status.ForeColor = System.Drawing.Color.Black;
                status.Text = "CONNECTED";
            } else
            {
                status.BackColor = System.Drawing.Color.Transparent;
                status.ForeColor = System.Drawing.Color.WhiteSmoke;
                status.Text = "DISCONNECTED";
            }
        }

        /// <summary>
        /// Open dialog to load a file to send to server
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Use streamReader to get file contents
                StreamReader streamReader = new StreamReader(openFileDialog.FileName);

                // Load contents for editing and then sending
                rtTransmissionText.Text = streamReader.ReadToEnd();

                // clean up
                streamReader.Close();
            }
        }

        /// <summary>
        /// Tooltip to inform user of correct input.
        /// </summary>
        private void txtIP_Enter(object sender, EventArgs e)
        {
            // Enabled tooltip to assist user
            TextBox txtBox = (TextBox)sender;
            int VisibleTime = 10000;

            ToolTip toolTip = new ToolTip();
            toolTip.Show("Please ensure valid IP format: ###.###.###.###", txtBox, -25, -25, VisibleTime);
        }
    }
}
