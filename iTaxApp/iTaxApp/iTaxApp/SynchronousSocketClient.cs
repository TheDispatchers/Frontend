using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;

namespace iTaxApp
{
    public class SynchronousSocketClient
    {
        public static object StartClient(string function, object o)
        {
            int bytesSent;
            int bytesRec;
            string json;
            string response;
            User user;
            NewUser newUser;

            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];
            Console.WriteLine("Start!");
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPAddress ipAddress = new IPAddress(new byte[] { 86, 52, 212, 76 });
                //   IPAddress ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8113);
                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connect the socket to the remote endpoint. Catch any errors.
                sender.ReceiveTimeout = 30000;
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                    // Encode the data string into a byte array.
                    switch (function)
                    {
                        case "login":
                            user = (User)o;
                            json = JsonConvert.SerializeObject(user);
                            Console.WriteLine(json);
                            byte[] login = Encoding.ASCII.GetBytes(json);
                            bytesSent = 0;
                            bytesRec = 0;
                            bytesSent = sender.Send(login);
                            bytesRec = sender.Receive(bytes);
                            string sessionKey = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            Console.WriteLine(sessionKey);
                            user.sessionKey = sessionKey;
                            o = user;
                            break;
                        case "logout":
                            user = (User)o;
                            json = JsonConvert.SerializeObject(user);
                            Console.WriteLine(json);
                            byte[] logout = Encoding.ASCII.GetBytes(json);
                            bytesSent = 0;
                            bytesRec = 0;
                            bytesSent = sender.Send(logout);
                            bytesRec = sender.Receive(bytes);
                            response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            Console.WriteLine(response);
                            user.sessionKey = null;
                            o = user;
                            break;
                        case "register":
                            newUser = (NewUser)o;
                            json = JsonConvert.SerializeObject(newUser);
                            Console.WriteLine(json);
                            byte[] register = Encoding.ASCII.GetBytes(json);
                            bytesSent = sender.Send(register);
                            bytesRec = sender.Receive(bytes);
                            response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            Console.WriteLine(response);
                            newUser.response = response;
                            o = newUser;
                            break;
                        case "confirmRegister":
                            newUser = (NewUser)o;
                            json = JsonConvert.SerializeObject(newUser);
                            Console.WriteLine(json);
                            byte[] confirm = Encoding.ASCII.GetBytes(json);
                            bytesSent = sender.Send(confirm);
                            bytesRec = sender.Receive(bytes);
                            response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            Console.WriteLine(response);
                            newUser.response = response;
                            o = newUser;
                            break;
                        case "orderRide":
                            Ride getRide = (Ride)o;
                            bytesRec = 0;
                            json = JsonConvert.SerializeObject(getRide);
                            Console.WriteLine(json);
                            byte[] getNewRide = Encoding.ASCII.GetBytes(json);
                            bytesSent = sender.Send(getNewRide);
                            bytesRec = sender.Receive(bytes);
                            response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            Console.WriteLine(response);
                            getRide.response = response;
                            o = getRide;
                            break;
                        case "getDistanceTimePrice":
                            Ride ride = (Ride)o;
                            json = JsonConvert.SerializeObject(ride);
                            Console.WriteLine(json);
                            byte[] newRide = Encoding.ASCII.GetBytes(json);
                            bytesSent = sender.Send(newRide);
                            bytesRec = sender.Receive(bytes);
                            response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            Console.WriteLine(response);
                            ride.response = response;
                            o = ride;
                            break; //
                    }
                    //sender.Shutdown(SocketShutdown.Both);
                    //sender.Close();
                    return o;
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    return false;
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ErrorCode);
                    Console.WriteLine("SocketException : {0}", se.SocketErrorCode);
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        /// <summary>
        /// Method to test the socket connection to the server.
        /// </summary>
        /// <returns></returns>
        public static bool TestConnection()
        {
            int bytesSent;
            int bytesRec;
            byte[] bytes = new byte[1024];
            Console.WriteLine("Start!");
            try
            {
                IPAddress ipAddress = new IPAddress(new byte[] { 86, 52, 212, 76 });
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8113);
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                    byte[] test = Encoding.ASCII.GetBytes("test");
                    bytesSent = sender.Send(test);
                    bytesRec = sender.Receive(bytes);
                    string recieved = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    Console.WriteLine(recieved);
                    //sender.Shutdown(SocketShutdown.Both);
                    //sender.Close();
                    return true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

    }
}