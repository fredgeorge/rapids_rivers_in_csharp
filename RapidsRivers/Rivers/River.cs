/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using RapidsRivers.Packets;
using RapidsRivers.Rapids;
using RapidsRivers.Validation;
using static RapidsRivers.Packets.SystemConstants;

namespace RapidsRivers.Rivers;

// Understands a themed flow of messages
public class River : RapidsConnection.MessageListener {
    private readonly RapidsConnection _connection;
    private readonly Rules _rules;
    private readonly int _maxReadCount;

    private readonly List<PacketListener> _listeners = new();
    private readonly List<SystemListener> _systemListeners = new();

    public River(RapidsConnection connection, Rules rules, int maxReadCount) {
        _connection = connection;
        _rules = rules;
        _maxReadCount = maxReadCount;
    }

    public void Register(PacketListener listener) {
        _listeners.Add(listener);
        _connection.Publish(new StartUpPacket(listener));
    }

    public void Register(SystemListener listener) {
        _systemListeners.Add(listener);
        Register((PacketListener)listener);
    }

    public void Message(RapidsConnection connection, string message) {
        try {
            Packet packet = new(message);
            if (packet.HasInvalidReadCount(_maxReadCount)) {
                TriggerLoopDetection(connection, packet);
                return;
            }
            if (packet.IsHeartBeat()) TriggerHeartBeatResponse(connection, packet);
            var listeners = packet.IsSystem() ? _systemListeners.ToList<PacketListener>() : _listeners;
            var status = packet.Evaluate(_rules);
            if (status.HasErrors()) TriggerRejectedPacket(listeners, connection, packet, status);
            else TriggerAcceptedPacket(listeners, connection, packet, status);
        }
        catch (PacketException e) {
            var status = new Status(message);
            status.Error(e.Message);
            TriggerInvalidFormat(connection, message, status);
        }
    }

    private void TriggerAcceptedPacket(List<PacketListener> listeners, RapidsConnection connection, Packet packet,
        Status problems) {
        var breadCrumbs = packet.BreadCrumbs();
        listeners.ForEach((service) => {
            packet.Set(SystemBreadCrumbsKey, new List<string>(breadCrumbs) { service.Name });
            service.Packet(connection, packet, problems);
        });
    }

    private void TriggerRejectedPacket(
        List<PacketListener> listeners,
        RapidsConnection connection,
        Packet packet,
        Status problems) =>
        listeners.ForEach((service) => service.RejectedPacket(connection, packet, problems));

    private void TriggerInvalidFormat(RapidsConnection connection, string message, Status problems) =>
        _systemListeners.ForEach((service) => service.InvalidFormat(connection, message, problems));

    private void TriggerLoopDetection(RapidsConnection connection, Packet packet) =>
        _systemListeners.ForEach((service) => service.LoopDetected(connection, packet));

    private void TriggerHeartBeatResponse(RapidsConnection connection, Packet packet) =>
        _listeners.ForEach((service) => {
            if (service.IsStillAlive(connection)) connection.Publish(packet.ToHeartBeatResponse(service));
        });

    public interface PacketListener {
        string Name { get; }
        Rules Rules { get; }

        bool IsStillAlive(RapidsConnection connection);

        void Packet(RapidsConnection connection, Packet packet, Status information);

        void RejectedPacket(RapidsConnection connection, Packet packet, Status problems) { }
    }

    public interface SystemListener : PacketListener {
        void InvalidFormat(RapidsConnection connection, string invalidString, Status problems) { }

        void LoopDetected(RapidsConnection connection, Packet packet) { }
    }
}