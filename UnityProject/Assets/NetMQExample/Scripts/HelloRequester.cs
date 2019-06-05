using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using UnityEngine.UI;


///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.


public class HelloRequester : RunAbleThread
{
    public bool pokemonSelected;
    public bool sending;
    public string pokemonPath;  

    public string foundpokemons;
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
                            if (gotMessage) {
                                // Debug.Log("Received " + message);
                                // string editedmessage =message.Substring(8);
                                foundpokemons = foundpokemons + '\n' + message; 
                                Debug.Log(foundpokemons);

                            }
                            sending = false;
                        }


                    }
                }
            }
        }

        NetMQConfig.Cleanup();
    }
}