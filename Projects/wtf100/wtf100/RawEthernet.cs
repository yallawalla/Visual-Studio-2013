/*  RawEthernet Application
 *  Written by: Jeremiah Clark, Oct 2003
 *  This code is opensouce and free for you to use.  All I ask is 
 *  that you give credit in your source comments.
 * 
 *  This application will interface with the NDIS Protocol Driver to enable
 *  the sending of raw ethernet packets out of a network device.
 */

using System;
using System.Threading;
using System.Runtime.InteropServices;	// For DllImport
using System.Text;						// For Encoding
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApplication1
{
	// Class to perform the sending of raw ethernet packets
    class RawEthernet : EventArgs
	{
        #region ATTRIBUTES
			
			// Path to the NDIS Protocol Driver so we can open it like a file
			private string m_sNdisProtDriver = "\\\\.\\\\NdisProt";

			// IntegerPointer to hold the handle of the driver
			private IntPtr m_iHandle = IntPtr.Zero;

			// Bool to hold whether we have a connection to the driver
			private bool m_bDriverOpen = false;

			// Bool to hold whether we are bound to an adapter
			private bool m_bAdapterBound = false;
            private volatile bool _stopped = false;
            private byte[] rxpack = new byte[2000];

		#endregion ATTIBUTES

		#region PROPERTIES

			// public properties for the class
			public IntPtr Handle { get { return this.m_iHandle; } }
			public bool IsDriverOpen { get { return this.m_bDriverOpen; } }
            public bool IsAdapterBound { get { return this.m_bAdapterBound; } }
            public bool IsStopped { get { return this._stopped; } }
            public byte[] NdisData { get { return this.rxpack; } }
            public AdaptersInfo[] aiAdapters;

		#endregion PROPERTIES

		#region CONSTRUCTOR

			public RawEthernet()
			{
				// Open a handle to the NDIS Device driver
				this.m_bDriverOpen = this.OpenDriver();
                if (this.m_bDriverOpen)
                {
                    aiAdapters = this.EnumerateAdapters();	
                }
			}

		#endregion CONSTRUCTOR

		#region METHODS

			// method to open a handle to the driver so we can access it
			// returns true if we get a valid handle, false if otherwise
			private bool OpenDriver()
			{
				// User the CreateFile API to open a handle to the file
				this.m_iHandle = CreateFile(this.m_sNdisProtDriver, 
					GENERIC_WRITE|GENERIC_READ, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);

				// Check to see if we got a valid handle
				if ((int)this.m_iHandle <= 0)
				{
					// If not, then return false and reset the handle to 0
					this.m_iHandle = IntPtr.Zero;
					return false;
				}

				// Otherwise we have a valid handle
				return true;
			}


			// method to return an array of the active Network adapters on your system
			private AdaptersInfo[] EnumerateAdapters()
			{
				int adapterIndex = 0;		// the current adapter index
				bool validAdapter = true;	// are we still getting a valid adapter

				// we are going to look for up to 10 adapters
				// temp array to hold the adapters that we find
				AdaptersInfo[] aiTemp = new AdaptersInfo[10]; 

				//start a loop while we look for adapters one by one starting at index 0
				do
				{
					// buffer to hold the adapter information that we get
					byte[] buf = new byte[1024];
					// uint to hold the number of bytes that we read
					uint iBytesRead = 0;
					// NDISPROT_QUERY_BINDING structure containing the index
					// that we want to query for
					NDISPROT_QUERY_BINDING ndisprot = new NDISPROT_QUERY_BINDING();
					ndisprot.BindingIndex = (ulong)adapterIndex;
					// uint to hold the length of the ndisprot
					uint bufsize = (uint)Marshal.SizeOf(ndisprot);	
					// perform the following in and unsafe context
					unsafe
					{
						// create a void pointer to buf
						fixed (void* vpBuf = buf)
						{
							// use the DeviceIoControl API to query the adapters
							validAdapter = DeviceIoControl(this.m_iHandle,
								IOCTL_NDISPROT_QUERY_BINDING, (void*)&ndisprot, 
								bufsize, vpBuf, (uint)buf.Length, 
								&iBytesRead, 0);
						}
					}
					// if DeviceIoControl returns false, then there are no
					// more valid adapters, so break the loop
					if (!validAdapter) break;

					// add the adapter information to the temp AdaptersInfo struct array
					// first, get a string containing the info from buf
					string tmpinfo = Encoding.Unicode.GetString(buf).Trim((char)0x00);
					tmpinfo = tmpinfo.Substring(tmpinfo.IndexOf("\\"));
					// add the info to aiTemp
					aiTemp[adapterIndex].Index = adapterIndex;
					aiTemp[adapterIndex].AdapterID = tmpinfo.Substring(0,
						tmpinfo.IndexOf("}")+1);
					aiTemp[adapterIndex].AdapterName = tmpinfo.Substring(
						tmpinfo.IndexOf("}")+1).Trim((char)0x00);
					
					// Increment the adapterIndex count
					adapterIndex++;

				// loop while we have a valid adapter
				}while (validAdapter || adapterIndex < 10);	
				
				// Copy the temp adapter struct to the return struct
				AdaptersInfo[] aiReturn = new AdaptersInfo[adapterIndex];
				for (int i=0;i<aiReturn.Length;i++)
					aiReturn[i] = aiTemp[i];

				// return aiReturn struct
				return aiReturn;
			}


			// method to bind an adapter to the a the handle that we have open
			public bool BindAdapter(string adapterID)
			{
				// char array to hold the adapterID string
				char[] ndisAdapter = new char[256];
				// convert the string to a unicode non-localized char array
				int iNameLength = 0, i = 0;
				for (i=0;i<adapterID.Length;i++)
				{
					ndisAdapter[i] = adapterID[i];
					iNameLength++;
				}
				ndisAdapter[i] = '\0';
				
				// uint to hold the number of bytes read from DeviceIoControl
				uint uiBytesReturned;
			
				// do the following in an unsafe context
				unsafe 
				{
					// create a void pointer to ndisAdapter
					fixed (void* vpNdisAdapter = &ndisAdapter[0])
					{
						// Call the DeviceIoControl API to bind the adapter to the handle
                        this.m_bAdapterBound = DeviceIoControl(this.m_iHandle, IOCTL_NDISPROT_OPEN_DEVICE,
							vpNdisAdapter, (uint)(iNameLength*sizeof(char)), null, 0, 
							&uiBytesReturned, 0);
                        return this.m_bAdapterBound;
					}
				}
			}


			// method to close the handle to the device driver
			private bool CloseDriver()
			{
				return CloseHandle(this.m_iHandle);
			}


            public bool  GetMAC(byte[] pSrcMacAddr)
            {
                uint BytesWritten;
                uint oid=OID_802_3_CURRENT_ADDRESS;
                Array.Copy(BitConverter.GetBytes(oid), 0, pSrcMacAddr, 0, 4);
                unsafe
                {
                    fixed (void* pvPacket = pSrcMacAddr)
                    {
                        if (DeviceIoControl(m_iHandle, IOCTL_NDISPROT_QUERY_OID_VALUE,
                                        pvPacket, 32,
                                        pvPacket, 32,
                                        &BytesWritten, 0))
                        {
                            Array.Copy(pSrcMacAddr,4,pSrcMacAddr,0,6);
                            return (true);
                        }
                        else
                            return (false);
                    }
                }
            }
				
            public bool NdisSend(byte[] pack, int len)
            {
                uint uiSentCount = 0;
                bool packetSent = false;
                unsafe
                {
                    fixed (void* pvPacket = pack)
                    {
                        packetSent = WriteFile(this.m_iHandle, pvPacket,  (uint)len, &uiSentCount, 0);
                    }
                }

                if (!packetSent)
                    return false;
                 else
                    return true;
            }
        


            public int NdisRecv(byte[] pack, int len)
            {
                uint uiReadCount = 0;
                bool packetRead;
                unsafe
                {
                    fixed (void* pvPacket = pack)
                    {
                        packetRead = ReadFile(this.m_iHandle, pvPacket, (uint)len, &uiReadCount, 0);
                    } 
                }
                if (packetRead == true)
                    return ((int)uiReadCount);
                else
                    return (-1);
            }






           public delegate void NdisDataReady(int n);

           public event NdisDataReady DataReady;

      
           private void DoWork()
           {
               int n;
               while( !_stopped )
               {
                   n = NdisRecv(rxpack, rxpack.Length);
                   if(n>0 && DataReady != null)
                       DataReady(n);
                   Thread.Sleep(1);
               }
           }

           public void Start()
           {
               _stopped = false;
               Thread t = new Thread(new ThreadStart(DoWork));
               t.IsBackground = true;
               t.Start();
           }

           public void Stop()
           {
               _stopped = true;
           }

		#endregion METHODS

		#region CONSTANTS
 
			// file access constants
			private const uint GENERIC_READ  = 0x80000000;
			private const uint GENERIC_WRITE = 0x40000000;
			
			// file creation disposition constant
			private const uint OPEN_EXISTING = 0x00000003;

			// file attributes constant
			private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

			// invalid handle constant
			private const int INVALID_HANDLE_VALUE = -1;

			// iocontrol code constants
			private const uint  IOCTL_NDISPROT_QUERY_BINDING    =0x12C80C;
			private const uint  IOCTL_NDISPROT_OPEN_DEVICE      =0x12C800;
        
            private const uint  IOCTL_NDISPROT_QUERY_OID_VALUE  =0x12C804;
            private const uint 	IOCTL_NDISPROT_SET_OID_VALUE    =0x12C814;
            private const uint 	IOCTL_NDISPROT_BIND_WAIT		=0x12C810;

            private const uint 	OID_802_3_PERMANENT_ADDRESS		=0x01010101;
            private const uint 	OID_802_3_CURRENT_ADDRESS		=0x01010102;


		#endregion CONSTANTS

		#region IMPORTS

			[DllImport("kernel32", SetLastError=true)]
			private static extern IntPtr CreateFile(
				string _lpFileName,				// filename to open
				uint _dwDesiredAccess,			// access permissions for the file
				uint _dwShareMode,				// sharing or locked
				uint _lpSecurityAttributes,		// security attributes
				uint _dwCreationDisposition,	// file creation method (new, existing)
				uint _dwFlagsAndAttributes,		// other flags and sttribs
				uint _hTemplateFile);			// template file for creating

            [DllImport("kernel32", SetLastError = true)]
            private static extern unsafe bool WriteFile(
                IntPtr _hFile,					// handle of the file to write to
                void* _lpBuffer,				// pointer to the buffer to write
                uint _nNumberOfBytesToWrite,	// number of bytes to write from the buffer
                uint* _lpNumberOfBytesWritten,	// [out] number of bytes written to the file
                uint _lpOverlapped);			// used for async reading and writing


            [DllImport("kernel32", SetLastError = true)]
            private static extern unsafe bool ReadFile(
                IntPtr _hFile,					// handle of the file to write to
                void* _lpBuffer,				// pointer to the buffer to write
                uint _nNumberOfBytesToRead,	    // number of bytes to write from the buffer
                uint* _lpNumberOfBytesRead,	    // [out] number of bytes written to the file
                uint _lpOverlapped);			// used for async reading and writing

			[DllImport("kernel32", SetLastError=true)]
			private static extern bool CloseHandle(
				IntPtr _hObject);				// handle for the object to close

			[DllImport("kernel32", SetLastError=true)]
			private static extern unsafe bool DeviceIoControl(
				IntPtr _hDevice,				// handle of the device
				uint _dwIoControlCode,			// IO control code to execute
				void* _lpInBuffer,				// Input buffer for the execution
				uint _nInBufferSize,			// size of the input buffer
				void* lpOutBuffer,				// [out] output buffer for the execution
				uint _nOutBufferSize,			// [size of the output buffer
				uint* _lpBytesReturned,			// [out] number of bytes returned
				uint _lpOverlapped);			// used for async reading and writing

		#endregion IMPORTS

		#region STRUCTS

		    [StructLayout(LayoutKind.Sequential)]
			private struct NDISPROT_QUERY_BINDING
			{
				public ulong BindingIndex;        // 0-based binding number
				public ulong DeviceNameOffset;    // from start of this struct
				public ulong DeviceNameLength;    // in bytes
				public ulong DeviceDescrOffset;    // from start of this struct
				public ulong DeviceDescrLength;    // in bytes
			}

		#endregion STRUCTS
	}
	
	// Structure to hold the information for a specific adapter
	public struct AdaptersInfo
	{
		#region ATTRIBUTES

			private int m_iIndex;			// The index of the adapter
			private string m_sAdapterID;	// The ID of the adapter
			private string m_sAdapterName;	// The name of the adapter

		#endregion ATTRIBUTES

		#region PROPERTIES

			public int Index {get{return m_iIndex;}set{m_iIndex = value;}}
			public string AdapterID {get{return m_sAdapterID;}set{m_sAdapterID = value;}}
			public string AdapterName {get{return m_sAdapterName;}set{m_sAdapterName = value;}}

		#endregion PROPERTIES

		#region CONSTRUCTOR

			// The constructor for this struct accepts three arguments
			//  index = the adapter index
			//  adapterID = the ID of the adapter
			//  adapterName = the name of the adapter
			public AdaptersInfo(int index, string adapterID, string adapterName)
			{
				// set the attributes according to the passed args
				this.m_iIndex = index;
				this.m_sAdapterID = adapterID;
				this.m_sAdapterName = adapterName;
			}

		#endregion CONSTRUCTOR

		#region METHODS

		public override string ToString()
		{
			return this.m_iIndex + ". " + this.m_sAdapterID + " - " + this.m_sAdapterName;
		}


		#endregion METHODS
	}
}
