using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;


///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.


public class HelloRequester : RunAbleThread
{
    public bool pokemonSelected;
    public bool sending;
    public string pokemonPath;
    ///     Stop requesting when Running=false.
    protected override void Run()
    {
        ForceDotNet.Force();

        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");

            while (Running)
            {
                if (Send)
                {
                    if (pokemonSelected)
                    {
                        if (!sending)
                        {
                            sending = true;
                            //string message = client.ReceiveFrameString();
                            client.SendFrame(pokemonPath);

                            string message = null;
                            bool gotMessage = false;

                            while (Running)
                            {
                                gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
                                if (gotMessage) break;
                            }
                            if (gotMessage) Debug.Log("Received " + message);
                            sending = false;
                        }


                    }
                }
            }
        }

        NetMQConfig.Cleanup();
    }
}