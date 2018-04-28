using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    class ClientConnection
    {
        private bool _isConnected;
        private string _connectionStatus;
        private string _serverResponse;
        private Stream _stream;
        private TcpClient _client;
        private TimeStamp timeStamp = new TimeStamp();

        /// <summary>
        /// Get connection status.
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Get server reposnse message.
        /// </summary>
        public string GetServerResponse => _serverResponse;

        /// <summary>
        /// Request Connection Status to update current status
        /// </summary>
        public string RequestConnectionStatus => _connectionStatus;

        /// <summary>
        /// Return current TCP client object
        /// </summary>
        public TcpClient GetClient => _client;

        /// <summary>
        /// Return stream object.
        /// </summary>
        public Stream GetStream => _stream;

        // Constructor
        public ClientConnection()
        {
            // this.connectionStatus = false;

            // Setup the local endpoint with any IP
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 54817);
            this._client = new TcpClient(localEndPoint);

        }

        /// <summary>
        /// Connect to Server
        /// </summary>
        /// <param name="ip">IP Address of server</param>
        /// <param name="port">Server Port</param>
        public void ConnectToServer(string ip, int port)
        {
            // Store parsed IP adress for server
            IPAddress ipAddress = IPAddress.Parse(ip);

            try
            {
                // Connect to remote server via given IP and port
                this._client.Client.Connect(ip, port);

                // If successful, update status
                this._connectionStatus = "Connected to Server! " +
                    "\n" + timeStamp.GetCurrentTimeStamp +
                    "Please write a message to send or open a file into the window to edit and send.";

                // Update status of connection
                this._isConnected = true;

                // Open stream to send and recieve data from server
                this._stream = _client.GetStream();
            }
            catch (Exception e)
            {
                // OnError: Update status and reason
                this._connectionStatus = "Connection falied: " + e.Message.ToString();
                this._isConnected = false;
            }

        }

        /// <summary>
        /// Transmit text to server
        /// </summary>
        /// <param name="message">Message to transmit to server.</param>
        /// <param name="stream">Steaming object used to transmit data.</param>
        public void TransmitText(string message, Stream stream)
        {
            // Declare and initialize encoder
            ASCIIEncoding encode = new ASCIIEncoding();

            // Cypher message with ROT13 and encode
            byte[] encodedMessage = encode.GetBytes(ROT13.Transform(message));

            // Update Status
            this._connectionStatus = "Transmitting text...";

            try
            {
                // Send message to server
                stream.Write(encodedMessage, 0, encodedMessage.Length);

                // Get Response from server
                byte[] byteB = new byte[4096];
                int k = _stream.Read(byteB, 0, 4096);

                // Declare variable to store incomming message
                string msg = "";

                // Iterate through stream received, convert, and store in variable
                for (int i = 0; i < k; i++)
                {
                    msg += Convert.ToChar(byteB[i]);
                }

                // Update private member and also the status
                this._serverResponse = ROT13.Transform(msg);
                this._connectionStatus += "\n\n" + timeStamp.GetCurrentTimeStamp + "Recieved Response: " + this._serverResponse;

                // Open dialog and ask user if he or she wants to save reponse to file
                DialogResult dialogResult = MessageBox.Show("Save server response to a file?", "NOTICE", MessageBoxButtons.YesNo);

                // Check user's answer
                if (dialogResult == DialogResult.Yes)
                {
                    // Store filename
                    string filename = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("MM_dd_yy_hh_mm") + ".txt";

                    // Update status
                    this._connectionStatus += "\n\nCreating File: " + filename;

                    // Create the file
                    CreateFile(this._serverResponse, filename);

                    // If the user chooses, he or she may view the file
                    dialogResult = MessageBox.Show("Saved! Open file?", "Saved File", MessageBoxButtons.YesNo);

                    // Check if the usr wishes to see the saved file
                    if (dialogResult == DialogResult.Yes)
                    {
                        // Open the file
                        Process.Start("explorer.exe", filename);
                    }
                }
            }
            catch (Exception e)
            {
                // OnError: Report status and reason
                this._connectionStatus = "Stream error: " + e.StackTrace;
            }
        }

        /// <summary>
        /// Method to create file.
        /// </summary>
        /// <param name="input">What to store in file.</param>
        /// <param name="fileName">Name and location of file.</param>
        public void CreateFile(string input, string fileName)
        {
            // Create file.
            File.WriteAllText(fileName, input);
        }

    }
}