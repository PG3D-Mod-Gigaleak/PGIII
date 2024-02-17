using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
	internal class TConnect
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass8
		{
			public MemoryStream opCollectionStream;

			public TConnect _003C_003E4__this;

			public void _003CRun_003Eb__3()
			{
				_003C_003E4__this.peer.ReceiveIncomingCommands(opCollectionStream.ToArray(), (int)opCollectionStream.Length);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClassb
		{
			public _003C_003Ec__DisplayClass8 CS_0024_003C_003E8__locals9;

			public byte[] inBuff;

			public void _003CRun_003Eb__2()
			{
				CS_0024_003C_003E8__locals9._003C_003E4__this.peer.ReceiveIncomingCommands(inBuff, inBuff.Length);
			}
		}

		internal const int TCP_HEADER_BYTES = 7;

		private const int MSG_HEADER_BYTES = 2;

		private const int ALL_HEADER_BYTES = 9;

		private EndPoint serverEndPoint;

		internal bool obsolete;

		internal bool isRunning;

		internal TPeer peer;

		private Socket socketConnection;

		internal TConnect(TPeer npeer, string ipPort)
		{
			if ((int)npeer.debugOut >= 5)
			{
				npeer.Listener.DebugReturn(DebugLevel.ALL, "new TConnect()");
			}
			peer = npeer;
		}

		internal bool StartConnection()
		{
			if (isRunning)
			{
				if ((int)peer.debugOut >= 1)
				{
					peer.Listener.DebugReturn(DebugLevel.ERROR, "startConnectionThread() failed: connection thread still running.");
				}
				return false;
			}
			socketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socketConnection.NoDelay = true;
			new Thread(Run).Start();
			return true;
		}

		internal void StopConnection()
		{
			if ((int)peer.debugOut >= 5)
			{
				peer.Listener.DebugReturn(DebugLevel.ALL, "StopConnection()");
			}
			obsolete = true;
			if (socketConnection != null)
			{
				socketConnection.Close();
			}
		}

		public void sendTcp(byte[] opData)
		{
			if (obsolete)
			{
				if ((int)peer.debugOut >= 3)
				{
					peer.Listener.DebugReturn(DebugLevel.INFO, "Sending was skipped because connection is obsolete. " + Environment.StackTrace);
				}
				return;
			}
			try
			{
				socketConnection.Send(opData);
			}
			catch (NullReferenceException ex)
			{
				if ((int)peer.debugOut >= 1)
				{
					peer.Listener.DebugReturn(DebugLevel.ERROR, ex.Message);
				}
			}
			catch (SocketException ex2)
			{
				if ((int)peer.debugOut >= 1)
				{
					peer.Listener.DebugReturn(DebugLevel.ERROR, ex2.Message);
				}
			}
		}

		public void Run()
		{
			PeerBase.MyAction myAction = null;
			PeerBase.MyAction myAction2 = null;
			try
			{
				serverEndPoint = PeerBase.GetEndpoint(peer.ServerAddress);
				if (serverEndPoint == null)
				{
					if ((int)peer.debugOut >= 1)
					{
						peer.Listener.DebugReturn(DebugLevel.ERROR, "StartConnection() failed. Address must be 'address:port'. Is: " + peer.ServerAddress);
					}
					return;
				}
				socketConnection.Connect(serverEndPoint);
			}
			catch (SecurityException ex)
			{
				if ((int)peer.debugOut >= 3)
				{
					peer.Listener.DebugReturn(DebugLevel.INFO, "Connect() failed: " + ex.ToString());
				}
				if (socketConnection != null)
				{
					socketConnection.Close();
				}
				isRunning = false;
				obsolete = true;
				peer.EnqueueStatusCallback(StatusCode.ExceptionOnConnect);
				TPeer tPeer = peer;
				if (myAction == null)
				{
					myAction = _003CRun_003Eb__0;
				}
				tPeer.EnqueueActionForDispatch(myAction);
				return;
			}
			catch (SocketException ex2)
			{
				if ((int)peer.debugOut >= 3)
				{
					peer.Listener.DebugReturn(DebugLevel.INFO, "Connect() failed: " + ex2.ToString());
				}
				if (socketConnection != null)
				{
					socketConnection.Close();
				}
				isRunning = false;
				obsolete = true;
				peer.EnqueueStatusCallback(StatusCode.ExceptionOnConnect);
				TPeer tPeer2 = peer;
				if (myAction2 == null)
				{
					myAction2 = _003CRun_003Eb__1;
				}
				tPeer2.EnqueueActionForDispatch(myAction2);
				return;
			}
			obsolete = false;
			isRunning = true;
			if (peer.TcpConnectionPrefix != null)
			{
				sendTcp(peer.TcpConnectionPrefix);
			}
			while (!obsolete)
			{
				PeerBase.MyAction myAction3 = null;
				_003C_003Ec__DisplayClass8 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass8();
				_003C_003Ec__DisplayClass._003C_003E4__this = this;
				_003C_003Ec__DisplayClass.opCollectionStream = new MemoryStream(256);
				try
				{
					PeerBase.MyAction myAction4 = null;
					_003C_003Ec__DisplayClassb _003C_003Ec__DisplayClassb = new _003C_003Ec__DisplayClassb();
					_003C_003Ec__DisplayClassb.CS_0024_003C_003E8__locals9 = _003C_003Ec__DisplayClass;
					int num = 0;
					_003C_003Ec__DisplayClassb.inBuff = new byte[9];
					while (num < 9)
					{
						num += socketConnection.Receive(_003C_003Ec__DisplayClassb.inBuff, num, 9 - num, SocketFlags.None);
						if (num == 0)
						{
							peer.SendPing();
							Thread.Sleep(100);
						}
					}
					if (_003C_003Ec__DisplayClassb.inBuff[0] == 240)
					{
						if (peer.TrafficStatsEnabled)
						{
							peer.TrafficStatsIncoming.CountControlCommand(_003C_003Ec__DisplayClassb.inBuff.Length);
						}
						if (peer.NetworkSimulationSettings.IsSimulationEnabled)
						{
							TPeer tPeer3 = peer;
							if (myAction4 == null)
							{
								myAction4 = _003C_003Ec__DisplayClassb._003CRun_003Eb__2;
							}
							tPeer3.ReceiveNetworkSimulated(myAction4);
						}
						else
						{
							peer.ReceiveIncomingCommands(_003C_003Ec__DisplayClassb.inBuff, _003C_003Ec__DisplayClassb.inBuff.Length);
						}
						continue;
					}
					int num2 = (_003C_003Ec__DisplayClassb.inBuff[1] << 24) | (_003C_003Ec__DisplayClassb.inBuff[2] << 16) | (_003C_003Ec__DisplayClassb.inBuff[3] << 8) | _003C_003Ec__DisplayClassb.inBuff[4];
					if (peer.TrafficStatsEnabled)
					{
						if (_003C_003Ec__DisplayClassb.inBuff[5] == 0)
						{
							peer.TrafficStatsIncoming.CountReliableOpCommand(num2);
						}
						else
						{
							peer.TrafficStatsIncoming.CountUnreliableOpCommand(num2);
						}
					}
					if ((int)peer.debugOut >= 5)
					{
						peer.EnqueueDebugReturn(DebugLevel.ALL, "message length: " + num2);
					}
					_003C_003Ec__DisplayClass.opCollectionStream.Write(_003C_003Ec__DisplayClassb.inBuff, 7, num - 7);
					num = 0;
					num2 -= 9;
					_003C_003Ec__DisplayClassb.inBuff = new byte[num2];
					for (; num < num2; num += socketConnection.Receive(_003C_003Ec__DisplayClassb.inBuff, num, num2 - num, SocketFlags.None))
					{
					}
					_003C_003Ec__DisplayClass.opCollectionStream.Write(_003C_003Ec__DisplayClassb.inBuff, 0, num);
					if (_003C_003Ec__DisplayClass.opCollectionStream.Length > 0)
					{
						if (peer.NetworkSimulationSettings.IsSimulationEnabled)
						{
							TPeer tPeer4 = peer;
							if (myAction3 == null)
							{
								myAction3 = _003C_003Ec__DisplayClass._003CRun_003Eb__3;
							}
							tPeer4.ReceiveNetworkSimulated(myAction3);
						}
						else
						{
							peer.ReceiveIncomingCommands(_003C_003Ec__DisplayClass.opCollectionStream.ToArray(), (int)_003C_003Ec__DisplayClass.opCollectionStream.Length);
						}
					}
					if ((int)peer.debugOut >= 5)
					{
						peer.EnqueueDebugReturn(DebugLevel.ALL, "TCP < " + _003C_003Ec__DisplayClass.opCollectionStream.Length);
					}
				}
				catch (SocketException ex3)
				{
					if (!obsolete)
					{
						obsolete = true;
						if ((int)peer.debugOut >= 1)
						{
							peer.EnqueueDebugReturn(DebugLevel.ERROR, "Receiving failed. SocketException: " + ex3.SocketErrorCode);
						}
						switch (ex3.SocketErrorCode)
						{
						case SocketError.ConnectionAborted:
						case SocketError.ConnectionReset:
							peer.EnqueueStatusCallback(StatusCode.DisconnectByServer);
							break;
						default:
							peer.EnqueueStatusCallback(StatusCode.Exception);
							break;
						}
					}
				}
				catch (Exception ex4)
				{
					if (!obsolete && (int)peer.debugOut >= 1)
					{
						peer.EnqueueDebugReturn(DebugLevel.ERROR, "Receiving failed. Exception: " + ex4.ToString());
					}
				}
			}
			if (socketConnection != null)
			{
				socketConnection.Close();
			}
			isRunning = false;
			obsolete = true;
			peer.EnqueueActionForDispatch(_003CRun_003Eb__4);
		}

		[CompilerGenerated]
		private void _003CRun_003Eb__0()
		{
			peer.Disconnected();
		}

		[CompilerGenerated]
		private void _003CRun_003Eb__1()
		{
			peer.Disconnected();
		}

		[CompilerGenerated]
		private void _003CRun_003Eb__4()
		{
			peer.Disconnected();
		}
	}
}
