# LNL Bridge Poker

A simple test application designed to test LNL Bridges are working correctly without using Resonite.

# Setup
1. Build the application 
2. Run it with the following cli arguments:
    - './LNLBridgePoker.exe <IP ADDRESS>:<PORT> <TOKEN> <LOCAL PORT>
    - For example: './LNLBridgePoker.exe 127.0.0.1:3001 C:cheese 3002
    - Token MUST be "C:<any string>" in order to get a response from a Resonite Bridge

# Running
Once ran the application will send a NAT Introduction Request to the target ip address and port every second. It will report back any messages received, NAT related or otherwise.

Resonite Bridges will respond with `SERVER_GONE:<your token without the C;>` in 90% of cases. This is good though as it still means a successful messaging flow.

## Pointing two instances at each other
It is possible to point two separate instances of this application at each other, to do this just ensure the specified IP address when you run the application is the IP address and port of another computer running the same application.

E.g.
- Machine A(192.168.1.10): './LNLBridgePoker.exe 192.168.1.12:3001 C:cheese 3001`
- Machine B(192.168.1.12): './LNLBridgePoker.exe 192.168.1.10:3001 C:cheese 3001`

  Would cause Machine A to endlessly try to NAT Punch Machine B, while machine B is also endlessly trying to Nat Punch A

# Building
Usually using these versions:
- `dotnet publish --framework net472 --runtime linux-x64 -c Debug`
- `dotnet publish --framework net8 --runtime win-x64 -c Debug`
- `dotnet publish --framework net472 --runtime win-x64 -c Debug`
- `dotnet publish --framework net8 --runtime linux-x64 -c Debug`