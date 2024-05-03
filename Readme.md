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

