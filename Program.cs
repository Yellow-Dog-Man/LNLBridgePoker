using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;

namespace LNLBridgePoker;

public class Program : INatPunchListener
{
   public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Program program = new(int.Parse(args[2]));

        if (IPEndPoint.TryParse(args[0], out IPEndPoint? result))
        {
            if (result is null)
            {
                Console.WriteLine("Invalid endpoint passed.");
                return;
            }

            while (true)
            {
                program.Go(result, args[1].Replace(":", ";"));
                program.Update();
                await Task.Delay(1000);
            }
        }
    }

    public void OnNatIntroductionRequest(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, string token)
    {
        Console.WriteLine($"Recieved NAT Introduction Request on {localEndPoint} from {remoteEndPoint} with {token}");
        var writer = new NetDataWriter();
        writer.Put($"Hello {remoteEndPoint}, you sent {token}");
        Client.SendUnconnectedMessage(writer, remoteEndPoint);
    }

    public void OnNatIntroductionSuccess(IPEndPoint targetEndPoint, NatAddressType type, string token)
    {
        Console.WriteLine($"NAT Introduction Success to {targetEndPoint} with nat type {type} with {token}");
    }

    private NetManager Client;

    public Program(int port)
    {
        EventBasedNetListener netListener = new EventBasedNetListener();
        netListener.NetworkReceiveUnconnectedEvent += NetListener_NetworkReceiveUnconnectedEvent;

        Client = new NetManager(netListener);

        Client.NatPunchEnabled = true;
        Client.UnsyncedEvents = true;
        Client.NatPunchModule.UnsyncedEvents = true;
        Client.UnconnectedMessagesEnabled = true;
        Client.NatPunchModule.Init(this);
        Client.Start(port);
    }

    private void NetListener_NetworkReceiveUnconnectedEvent(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        // Resonite servers will just respond with a single string
        var result = reader.GetString();
        Console.WriteLine($"Got a message from: {remoteEndPoint} {messageType}, {result}");
        
    }

    public void Go(IPEndPoint target, string token)
    {
        Console.WriteLine($"Sending NAT Introduction Request to: {target} with token: {token}");
        Client.NatPunchModule.SendNatIntroduceRequest(target, token);
    }

    public void Update()
    {
        Client.PollEvents();
        Client.NatPunchModule.PollEvents();
    }
}

