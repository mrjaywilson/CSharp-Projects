using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(String[] args)
        {
            // Declare Variables and objects
            int port = 0;
            TimeStamp timeStamp = new TimeStamp();

            // Get port number from commandline argument
            Int32.TryParse(args[0], out port);
            
            // Report status
            Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Server Starting...\n\n");

            // Setup listener for client
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try
            {
                // Start listening for client
                listener.Start();
                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Server is running on port: {0}", port);
                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Waiting for a connection...");

                // Accept connection request from client
                Socket socket = listener.AcceptSocket();
                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Connection Accepted From: {0}", socket.RemoteEndPoint);
                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Waiting for client activity...\n\n");

                // Setup variable for message from client
                byte[] msg = new byte[4096];

                // populate variable with encoded message
                int receive = socket.Receive(msg);

                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Received Message From Client: ");

                // Declare variable to store message
                string decodedMessage = "";

                // iterate through message
                for (int i = 0; i < receive; i++)
                {
                    // Store decoded message
                    decodedMessage += Convert.ToChar(msg[i]);
                }

                // Remove ROT13 cypher
                Console.WriteLine(ROT13.Transform(decodedMessage));

                // Send Message back to client to let know it was recieved.
                ASCIIEncoding encode = new ASCIIEncoding();
                socket.Send(encode.GetBytes(ROT13.Transform(timeStamp.GetCurrentTimeStamp +
                    "Message received.")));

                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Sent Response to client");

                // Clean up
                socket.Close();
                listener.Stop();
            }
            catch (Exception e)
            {
                // OnError: report status
                Console.WriteLine(timeStamp.GetCurrentTimeStamp + "Error: " + e.Message);
            } finally
            {
                // Exit the program
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
