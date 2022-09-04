/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RabbitMqBus;
using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Rivers;
using RapidsRivers.Validation;
using static RapidsRivers.Rivers.River;

namespace MonitorService;

public class Monitor : SystemListener {
    public string Name => $"Monitor [{GetHashCode()}]";

    // Sample rule possibilities in comments below:
    public Rules Rules => new(
        // new RequireValue("key_for_string", "expectedValue"),  // This requires Packet to have key-value pair
        // new RequireValue("key_for_number", 42.42),  // This requires Packet to have key-value pair
        // new RequireKeys("key1", "key2", "key3"),  // This requires these keys exist (doesn't care about values)
        // new ForbidKeys("key4", "key5")  // This forbids these keys existing. Note that 'null', empty string, and empty arrays are considered to not exist
    );

    public static void Main(string[] args) // Pass in <IP address> and <port> for RabbitMQ
    {
        if (args.Length != 2)
            throw new ArgumentException(
                "Missing IP and Port arguments! The IP address of the Rapids (as a string), and the Port number of the Rapids (also as a string).");
        var rapidsConnection = new RabbitMqRapidsConnection(args[0], args[1]);
        rapidsConnection.Register(new Monitor());
        Thread.Sleep(Timeout.Infinite);
    }

    public bool IsStillAlive(RapidsConnection connection) => true;

    public void Packet(RapidsConnection connection, Packet packet, Status information) {
/*
 *      Sample Packet manipulations:
 * 
 *      packet.String("key");  // return the String value in the Packet
 *      packet.Integer("key");  // return the int value in the Packet
 *      packet.Double("key");  // return the double value in the Packet
 *      packet.DateTime("key");  // return the DateTime value in the Packet after conversion from string
 *      packet.StringList("key");  // return a List of Strings in the Packet
 *      packet["key"];              // return a sub-Packet at the key; use this to get deeper access to packet's JSON
 *      packet.IsLacking("key");   // return true if the packet does not have the key, the value is null, the value is an empty string, or the value is an empty array
 *      packet.Set("key", value);  // Set the key/value pair into the Packet; you can override an existing value, or create a new one
 *       
*/
        Console.WriteLine(" [x] {0}", information);
    }

    public void RejectedPacket(RapidsConnection connection, Packet packet, Status problems) {
        Console.WriteLine(" [x] {0}", problems);
    }
}