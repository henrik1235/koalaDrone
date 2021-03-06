﻿using DroneLibrary.Data;
using DroneLibrary.Diagnostics;
using DroneLibrary.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DroneLibrary
{
    /// <summary>
    /// Stellt die Verbindung zu einer Drohne dar.
    /// </summary>
    public class Drone : IDisposable
    {
        public bool IsDisposed { get; private set; }

        private int lastPing = Environment.TickCount;
        private int ping = -1;
        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Gibt die letzte Paketumlaufzeit an die von der Drohne erfasst zurück. 
        /// Der Wert ist -1 wenn noch kein Ping-Wert emfangen wurde.
        /// </summary>
        public int Ping
        {
            get
            {
                return ping;
            }
            private set
            {
                if (ping != value)
                {
                    ping = value;
                    OnPingChange?.Invoke(this, new PingChangedEventArgs(this));
                }
            }
        }

        public bool IsConnected
        {
            get { return Ping >= 0; }
        }

        public LogStorage DroneLog { get; private set; }

        /// <summary>
        /// Wird aufgerufen wenn die Drohne verbunden ist.
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// Wird aufgerufen wenn die Verbindung zur Drohne verloren gehen.
        /// </summary>
        public event EventHandler OnDisconnect;

        /// <summary>
        /// Wird aufgerufen wenn sich der Ping-Wert ändert.
        /// </summary>
        public event EventHandler<PingChangedEventArgs> OnPingChange;

        /// <summary>
        /// Gibt die aktuelle Revision der Daten an die zu der Drohne geschickt wurden.
        /// </summary>
        private int currentRevision = 1;

        /// <summary>
        /// Gibt die letzte Revision der Daten an die von der Drohne geschickt wurden.
        /// </summary>
        private int lastDataDroneRevision = 0;

        /// <summary>
        /// Gibt die letzte Revision der Daten an die von der Drohne mit Log Daten geschickt wurden.
        /// </summary>
        private int lastDataLogRevision = 0;

        private int lastDataOutputRevision = 0;
        private int lastDataProfilerRevision = 0;

        /// <summary>
        /// Gibt die IPAdress der Drohne zurück.
        /// </summary>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// Gibt die Einstellungen der Drohne zurück
        /// </summary>
        public Config Config
        {
            get;
            private set;
        }

        /// <summary>
        /// Wrid aufgerufen, wenn sich die aktuellen Daten der Drohne ändern.
        /// </summary>
        public event EventHandler<DataChangedEventArgs> OnDataChange;

        private int lastDataTime;

        private DroneData data;

        /// <summary>
        /// Gibt aktuelle Daten über das Verhalten der Drohne zurück.
        /// </summary>
        public DroneData Data
        {
            get
            {
                lock (dataLock)
                {
                    return data;
                }
            }
            set
            {
                bool changed;
                lock (dataLock)
                {
                    changed = !value.Equals(data);
                    if (changed)
                        data = value;
                }

                if (changed)
                    OnDataChange?.Invoke(this, new DataChangedEventArgs(this));
            }
        }

        /// <summary>
        /// Wird aufgerufen wenn sich der Info-Wert ändert.
        /// </summary>
        public event EventHandler<InfoChangedEventArgs> OnInfoChange;

        private bool firstInfo = true;
        private DroneInfo info;

        /// <summary>
        /// Gibt Informationen über die Drohne zurück. 
        /// </summary>
        public DroneInfo Info
        {
            get
            {
                lock (infoLock)
                {
                    return info;
                }
            }
            set
            {
                bool changed;
                lock (infoLock)
                {
                    changed = !value.Equals(info);
                    if (changed)
                        info = value;
                }

                if (changed)
                    OnInfoChange?.Invoke(this, new InfoChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn sich die Einstellungen der Drohne ändern.
        /// </summary>
        public event EventHandler<SettingsChangedEventArgs> OnSettingsChange;

        private DroneSettings settings;

        /// <summary>
        /// Gibt die letzten bekannten Einstellungen der Drohne zurück.
        /// </summary>
        public DroneSettings Settings
        {
            get
            {
                lock (settingsLock)
                {
                    return settings;
                }
            }
            set
            {
                bool changed;
                lock (settingsLock)
                {
                    changed = !value.Equals(settings);
                    if (changed)
                        settings = value;
                }

                if (changed)
                    OnSettingsChange?.Invoke(this, new SettingsChangedEventArgs(this));
            }
        }

        public event EventHandler OnDebugDataChanged;

        public OutputData DebugOutputData { get; private set; }
        public ProfilerData DebugProfilerData { get; private set; }

        /// <summary>
        /// Gibt den Socket an mit dem die Drohne mit der Hardware per UDP verbunden ist.
        /// </summary>
        private UdpClient controlSocket;

        /// <summary>
        /// Gibt den Socket an, mit dem die Drohne die Daten empfängt.
        /// </summary>
        private UdpClient dataSocket;

        /// <summary>
        /// Gibt den Paket-Buffer an, welcher benutzt wird um die Pakete zu generieren.
        /// </summary>
        private MemoryStream packetStream = new MemoryStream();

        /// <summary>
        /// BinaryWriter der für den Packet-Buffer zum Schreiben benutzt wird.
        /// </summary>
        private PacketBuffer packetBuffer;

        /// <summary>
        /// Gibt den Zeitpunkt an, als das Paket abgeschickt wurde.
        /// </summary>
        private Dictionary<int, long> packetSendTime = new Dictionary<int, long>();

        /// <summary>
        /// Pakete die noch von der Drohne bestätigt werden müssen.
        /// </summary>
        private Dictionary<int, IPacket> packetsToAcknowledge = new Dictionary<int, IPacket>();

        /// <summary>
        /// EventHandler der aufgerufen werden soll, wenn ein Paket bestätigt wird.
        /// </summary>
        private Dictionary<int, EventHandler<IPacket>> packetAcknowledgeEvents = new Dictionary<int, EventHandler<IPacket>>();

        /// <summary>
        /// Gibt zurück ob Pakete noch warten von der Drohne bestätigt zu werden.
        /// </summary>
        public bool AnyPacketsAcknowledgePending => packetsToAcknowledge.Count > 0;

        /// <summary>
        /// Gibt die Anzahl der Pakete zurück die noch von der Drohne bestätigt werden müssen.
        /// </summary>
        public int PendingAcknowledgePacketsCount => packetsToAcknowledge.Count;

        private object infoLock = new object();
        private object dataLock = new object();
        private object settingsLock = new object();
        private object debugDataLock = new object();

        public Drone(IPAddress address, Config config)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            this.Config = config;
            this.Address = address;
            this.DroneLog = new LogStorage();

            controlSocket = new UdpClient();
            controlSocket.Connect(address, Config.ProtocolControlPort);

            dataSocket = new UdpClient(Config.ProtocolDataPort);

            packetBuffer = new PacketBuffer(packetStream);

            controlSocket.BeginReceive(ReceivePacket, null);
            dataSocket.BeginReceive(ReceiveDataPacket, null);

            // Ping senden und ein ResetRevision Paket senden damit die Revision wieder zurück gesetzt wird
            SendPing();

            OnConnected += (sender, args) =>
            {
                Log.Info("Connected to {0}", Address);

                firstInfo = true;
                currentRevision = 1;
                lastDataDroneRevision = 0;
                lastDataLogRevision = 0;
                lastDataOutputRevision = 0;
                lastDataProfilerRevision = 0;

                lastPing = Environment.TickCount;
                lastDataTime = Environment.TickCount;

                // alle Pending Packets leeren, damit die Drohne nach Reconnect nicht überfordert wird
                lock (packetsToAcknowledge)
                {
                    packetsToAcknowledge.Clear();
                    packetSendTime.Clear();
                    packetAcknowledgeEvents.Clear();
                }

                SendGetInfo();
                SendPacket(new PacketResetRevision(), true);
                SendPacket(new PacketSubscribeDataFeed(), true);
            };
        }

        #region Dispose

        ~Drone()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (disposing)
            {
                controlSocket?.Close();
                dataSocket?.Close();
                packetStream?.Dispose();
            }
        }

        #endregion

        public void Disconnect()
        {
            Log.Info("Disconnecting drone {0}", Address);
            // wenn wir schon disposed sind, können wir keine weiteren Pakete mehr versenden
            if (!IsDisposed)
            {
                SendDisarm();
                SendPacket(new PacketUnsubscribeDataFeed(), false);
            }

            Ping = -1;
            Dispose();
        }

        /// <summary>
        /// Verschickt alle Pakete nochmal die noch von der Drohne bestätigt werden sollen.
        /// </summary>
        /// <returns>Gibt true zurück, wenn Pakete gesendet wurden.</returns>
        public bool ResendPendingPackets()
        {
            lock (controlSocket)
            {
                lock (packetsToAcknowledge)
                {
                    bool anyDataSent = false;
                    KeyValuePair<int, IPacket>[] packets = packetsToAcknowledge.ToArray();
                    foreach (KeyValuePair<int, IPacket> packet in packets)
                        if (stopwatch.ElapsedMilliseconds - packetSendTime[packet.Key] > Math.Max(Ping, Config.AcknowlegdeTime)) // ist das Paket alt genug zum neusenden?
                            anyDataSent |= SendPacket(packet.Value, true, packet.Key);
                    return anyDataSent;
                }
            }
        }

        public bool CheckConnection()
        {
            if (!IsConnected)
                return false;

            if (Environment.TickCount - lastPing > 3000)
            {
                Log.Warning("Lost connection, no ping received");
                Ping = -1;
            }
            else if (Environment.TickCount - lastDataTime > 10000)
            {
                Log.Warning("Lost connection, no data receied");
                Ping = -1;
            }
            if (!IsConnected)
                OnDisconnect?.Invoke(this, EventArgs.Empty);

            return IsConnected;
        }

        #region SendShortcuts

        /// <summary>
        /// Schickt einen Ping-Befehl an die Drohne.
        /// </summary>
        public void SendPing()
        {
            // Stopwatch zum Messen der Zeit für die Pings starten
            if (!stopwatch.IsRunning)
                stopwatch.Restart();

            CheckConnection();

            SendPacket(new PacketPing(stopwatch.ElapsedMilliseconds), false);
        }

        /// <summary>
        /// Schickt der Drohne den Befehl neuzustarten.
        /// </summary>
        public void SendReset()
        {
            if (Data.State.AreMotorsRunning())
                throw new InvalidOperationException("Drone in invalid state: " + Data.State);

            Log.Info("SendReset()");
            SendPacket(new PacketReset(), false);
        }

        /// <summary>
        /// Fordert die Statusinformationen der Drone an.
        /// </summary>
        public void SendGetInfo()
        {
            SendPacket(new PacketInfo(), false);
        }

        /// <summary>
        /// Schickt einen Arm-Befehl an die Drohne
        /// </summary>
        public void SendArm()
        {
            Log.Info("SendArm()");
            SendPacket(new PacketArm(true), true);
        }

        /// <summary>
        /// Schickt einen Disarm-Befehl an die Drohne
        /// </summary>
        public void SendDisarm()
        {
            Log.Info("SendDisarm()");
            SendPacket(new PacketArm(false), true);
        }

        public void SendMovementData(short roll, short pitch, short yaw, short thrust)
        {
            SendPacket(new PacketSetMovement(roll, pitch, yaw, thrust), false);
        }

        /// <summary>
        /// Schickt einen Befehl an die Drohne um die LED blinken zu lassen.
        /// </summary>
        public void SendBlink()
        {
            SendPacket(new PacketBlink(), true);
        }

        /// <summary>
        /// Schickt einen Settings-Befehl an die Drohne.
        /// </summary>
        public void SendConfig(DroneSettings config)
        {
            SendPacket(new PacketSetConfig(config), true);
            Settings = config;
        }

        /// <summary>
        /// Schickt einen Stop-Befehl an die Drohne.
        /// </summary>
        public void SendStop()
        {
            Log.Info("SendStop()");
            SendPacket(new PacketStop(), true);
        }

        /// <summary>
        /// Schickt den ClearStatus-Befehl an die Drohne.
        /// </summary>
        public void SendClearStatus()
        {
            Log.Info("SendClearStatus()");
            SendPacket(new PacketClearStatus(), true);
        }

        /// <summary>
        /// Schickt ein Packet an die Drohne.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="guaranteed">Ob von der Drohne eine Antwort gefordert wird.</param>
        public bool SendPacket(IPacket packet, bool guaranteed, EventHandler<IPacket> handler = null)
        {
            return SendPacket(packet, guaranteed, currentRevision++, handler);
        }

        #endregion SendShortcuts

        private bool CheckRevision(int oldRev, int newRev)
        {
            if (newRev > oldRev)
                return true;
            if (newRev < 0 && oldRev >= 0) // Overflow
                return true;
            return false;
        }

        #region ControlUdp

        private bool SendPacket(IPacket packet, bool guaranteed, int revision, EventHandler<IPacket> handler = null)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            // wenn die Drohne nicht erreichbar ist
            if (!IsConnected)
            {
                if (Config.IgnoreGuaranteedWhenOffline)
                    guaranteed = false;

                // alle Pakete (außer Ping) ignorieren wenn die Drohne offline ist
                if (packet.Type != PacketType.Ping && Config.IgnorePacketsWhenOffline)
                    return false;
            }

            try
            {
                lock (controlSocket)
                {
                    bool alreadySent;
                    lock (packetsToAcknowledge)
                    {
                        alreadySent = packetsToAcknowledge.ContainsKey(revision);
                        if (guaranteed)
                        {
                            if (!stopwatch.IsRunning)
                                stopwatch.Start();
                            packetsToAcknowledge[revision] = packet;
                            packetSendTime[revision] = stopwatch.ElapsedMilliseconds;

                            if (handler != null)
                                packetAcknowledgeEvents[revision] = handler;
                        }
                    }

                    packetBuffer.ResetPosition();

                    // Paket-Header schreiben
                    packetBuffer.Write((byte)'F');
                    packetBuffer.Write((byte)'L');
                    packetBuffer.Write((byte)'Y');

                    // Alle Daten werden nach dem Netzwerkstandard BIG-Endian übertragen
                    packetBuffer.Write(revision);

                    // wenn die Drohne eine Antwort schickt dann wird kein Ack-Paket angefordert, sonst kann es passieren, dass das Ack-Paket die eigentliche Antwort verdrängt
                    packetBuffer.Write(guaranteed && !packet.Type.DoesAnswer());
                    packetBuffer.Write((byte)packet.Type);

                    // Paket Inhalt schreiben
                    packet.Write(packetBuffer);

                    controlSocket.BeginSend(packetStream.GetBuffer(), (int)packetBuffer.Position, SendPacket, null);
                    if (Config.VerbosePacketSending
                        && (Config.LogPingPacket || packet.Type != PacketType.Ping)
                        && (Config.LogNoisyPackets || !packet.Type.IsNosiy()))
                        Log.Verbose("[{0}] Packet:      [{1}] {2}, size: {3} bytes {4} {5}", Address.ToString(), revision, packet.Type, packetBuffer.Position, guaranteed ? "(guaranteed)" : "", alreadySent ? "(resend)" : "");
                }
                return true;
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            return false;
        }

        private void SendPacket(IAsyncResult result)
        {
            if (IsDisposed)
                return;

            try
            {
                controlSocket.EndSend(result);
            }
            catch (SocketException)
            {
                // Drohne ist möglicherweiße nicht verfügbar
            }
        }

        private void ReceivePacket(IAsyncResult result)
        {
            if (IsDisposed)
                return;

            try
            {
                IPEndPoint endPoint = new IPEndPoint(Address, Config.ProtocolControlPort);
                byte[] packet = controlSocket.EndReceive(result, ref endPoint);

                // kein Packet empfangen
                if (packet == null || packet.Length == 0)
                {
                    controlSocket.BeginReceive(ReceivePacket, null);
                    return;
                }

                HandlePacket(packet);
            }
            catch (SocketException)
            {
                Ping = -1;
            }
            catch (ObjectDisposedException e)
            {
                Log.Error(e);
            }
            catch (Exception e)
            {
                Log.Error(e);
                if (!Data.State.AreMotorsRunning() && Debugger.IsAttached)
                    Debugger.Break();
            }
            finally
            {
                if (!IsDisposed)
                    controlSocket.BeginReceive(ReceivePacket, null);
            }
        }

        private const int HeaderSize = 9;

        private void HandlePacket(byte[] packet)
        {
            // jedes Drohnen Paket ist mindestens HeaderSize Bytes lang und fangen mit "FLY" an
            using (MemoryStream stream = new MemoryStream(packet))
            {
                PacketBuffer buffer = new PacketBuffer(stream);

                if (packet.Length < HeaderSize || buffer.ReadByte() != 'F' || buffer.ReadByte() != 'L' || buffer.ReadByte() != 'Y')
                    return;

                int revision = buffer.ReadInt();

                bool isGuaranteed = buffer.ReadByte() > 0;
                PacketType type = (PacketType)buffer.ReadByte();

                if (Config.VerbosePacketReceive
                    && type != PacketType.Ack
                    && (Config.LogPingPacket || type != PacketType.Ping)
                    && (Config.LogNoisyPackets || !type.IsNosiy()))
                    Log.Verbose("[{0}] Received:    [{1}] {2}, size: {3} bytes", Address.ToString(), revision, type, packet.Length);

                switch (type)
                {
                    case PacketType.Ping:
                        bool wasNotConnected = !CheckConnection();
                        lastPing = Environment.TickCount;

                        long time = 0;
                        if (packet.Length >= HeaderSize + sizeof(long))
                            time = buffer.ReadLong(); // time ist der Wert von stopwatch zum Zeitpunkt des Absenden des Pakets
                        else
                            Log.Error("Invalid ping packet received with length: {0}", packet.Length);

                        int ping = (int)(stopwatch.ElapsedMilliseconds - time);
                        if (ping < 0)
                        {
                            Log.Warning("Invalid ping value received: {0}", ping);
                            ping = 0;
                        }
                        Ping = ping;

                        if (wasNotConnected)
                            OnConnected?.Invoke(this, EventArgs.Empty);

                        RemovePacketToAcknowledge(revision);
                        break;
                    case PacketType.Ack:
                        IPacket acknowlegdedPacket;
                        if (!packetsToAcknowledge.TryGetValue(revision, out acknowlegdedPacket))
                        {
                            if (Config.VerbosePacketReceive)
                                Log.Verbose("[{0}] Unknown acknowledge: [{1}]", Address.ToString(), revision);
                            break;
                        }

                        if (Config.VerbosePacketReceive)
                            Log.Verbose("[{0}] Acknowledge: [{1}] {2}", Address.ToString(), revision, acknowlegdedPacket.Type);

                        RemovePacketToAcknowledge(revision);
                        break;

                    case PacketType.Info:
                        Info = new DroneInfo(buffer);
                        Settings = DroneSettings.Read(buffer);

                        if (firstInfo)
                        {
                            Log.Info("Received drone info for first time...");
                            Log.WriteProperties(LogLevel.Info, Info);

                            firstInfo = false;
                        }

                        RemovePacketToAcknowledge(revision);
                        break;
                    default:
                        throw new InvalidDataException("Invalid packet type received.");
                }
            }
        }

        #endregion

        #region DataFeedReceive

        private void ReceiveDataPacket(IAsyncResult result)
        {
            if (IsDisposed)
                return;

            try
            {
                IPEndPoint endPoint = new IPEndPoint(Address, Config.ProtocolControlPort);
                byte[] packet = dataSocket.EndReceive(result, ref endPoint);

                // kein Packet empfangen
                if (packet == null || packet.Length == 0)
                {
                    dataSocket.BeginReceive(ReceivePacket, null);
                    return;
                }

                HandleDataPacket(packet);
            }
            catch (ObjectDisposedException e)
            {
                Log.Error(e);
            }
            catch (Exception e)
            {
                Log.Error(e);
                if (!Data.State.AreMotorsRunning() && Debugger.IsAttached)
                    Debugger.Break();
            }
            finally
            {
                if (!IsDisposed)
                    dataSocket.BeginReceive(ReceiveDataPacket, null);
            }
        }

        private const int DataDronePacketSize = 25;

        private void HandleDataPacket(byte[] packet)
        {
            using (MemoryStream stream = new MemoryStream(packet))
            {
                PacketBuffer buffer = new PacketBuffer(stream);
                if (buffer.Size < 3 || buffer.ReadByte() != 'F' || buffer.ReadByte() != 'L' || buffer.ReadByte() != 'Y')
                    return;

                int revision = buffer.ReadInt();
                DataPacketType type = (DataPacketType)buffer.ReadByte();

                lastDataTime = Environment.TickCount;

                switch (type)
                {
                    case DataPacketType.Drone:
                        if (!CheckRevision(lastDataDroneRevision, revision))
                            return;

                        DroneState state = (DroneState)buffer.ReadByte();
                        QuadMotorValues motorValues = new QuadMotorValues(buffer);
                        SensorData sensor = new SensorData(buffer);

                        float batteryVoltage = buffer.ReadFloat();
                        int wifiRssi = buffer.ReadInt();

                        Data = new DroneData(state, motorValues, sensor, batteryVoltage, wifiRssi);

                        lastDataDroneRevision = revision;
                        break;
                    case DataPacketType.Log:
                        if (!CheckRevision(lastDataLogRevision, revision))
                            return;

                        int lines = buffer.ReadInt();

                        for (int i = 0; i < lines; i++)
                        {
                            string msg = buffer.ReadString();

                            DroneLog.AddLine(msg);
                        }

                        lastDataLogRevision = revision;
                        break;
                    case DataPacketType.DebugOutput:
                        if (!CheckRevision(lastDataOutputRevision, revision))
                            return;

                        DebugOutputData = new OutputData(buffer);
                        lastDataOutputRevision = revision;

                        NotifyDebugDataChanged();
                        break;
                    case DataPacketType.DebugProfiler:
                        if (!CheckRevision(lastDataProfilerRevision, revision))
                            return;

                        DebugProfilerData = new ProfilerData(buffer);
                        lastDataProfilerRevision = revision;

                        NotifyDebugDataChanged();
                        break;
                }
            }
        }

        private void NotifyDebugDataChanged()
        {
            OnDebugDataChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Entfernt ein Paket von der Liste der noch zu bestätigen Pakete.
        /// </summary>
        /// <param name="packetID"></param>
        private void RemovePacketToAcknowledge(int packetID)
        {
            EventHandler<IPacket> handler = null;
            IPacket packet = null;
            lock (controlSocket)
            {
                lock (packetsToAcknowledge)
                {
                    if (packetAcknowledgeEvents.TryGetValue(packetID, out handler))
                    {
                        packet = packetsToAcknowledge[packetID];
                        packetAcknowledgeEvents.Remove(packetID);
                    }
                    packetsToAcknowledge.Remove(packetID);
                    packetSendTime.Remove(packetID);
                }
            }
            if (handler != null)
                handler(this, packet);
        }
    }
}
