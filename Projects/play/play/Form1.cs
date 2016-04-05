using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        SoundPlayer myPlayer;
        WaveMemoryStream w;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] si = new int[200];

            short[] s = new short[1000000];
            byte[] sb = new byte[s.Length*2];

            Random rnd = new Random();
            for (int i = 0; i < si.Length; ++i)
                si[i]=rnd.Next(-10000,10000);

            for (int i = 0; i < s.Length; ++i)
                si[i % si.Length] = s[i] = (short)((si[i % si.Length] + si[(i + 1) % si.Length]) / 2);           

            Buffer.BlockCopy(s, 0, sb, 0, sb.Length);
            w = new WaveMemoryStream(sb, 20000, 16, 1);
            myPlayer = new SoundPlayer(w);
            myPlayer.Play();
        }
    }
}

public class WaveMemoryStream : Stream
{
    public override bool CanSeek { get { return false; } }
    public override bool CanWrite { get { return false; } }
    public override bool CanRead { get { return true; } }
    public override long Length { get { return _waveStream.Length; } }
    public override long Position { get { return _waveStream.Position; } set { _waveStream.Position = value; } }

    private MemoryStream _waveStream;

    public WaveMemoryStream(byte[] sampleData, int audioSampleRate, ushort audioBitsPerSample, ushort audioChannels)
    {
        _waveStream = new MemoryStream();
        WriteHeader(_waveStream, sampleData.Length, audioSampleRate, audioBitsPerSample, audioChannels);
        WriteSamples(_waveStream, sampleData);
        _waveStream.Position = 0;
    }

    public void WriteHeader(Stream stream, int length, int audioSampleRate, ushort audioBitsPerSample, ushort audioChannels)
    {
        BinaryWriter bw = new BinaryWriter(stream);

        bw.Write(new char[4] { 'R', 'I', 'F', 'F' });
        int fileSize = 36 + length;
        bw.Write(fileSize);
        bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
        bw.Write((int)16);
        bw.Write((short)1);
        bw.Write((short)audioChannels);
        bw.Write(audioSampleRate);
        bw.Write((int)(audioSampleRate * ((audioBitsPerSample * audioChannels) / 8)));
        bw.Write((short)((audioBitsPerSample * audioChannels) / 8));
        bw.Write((short)audioBitsPerSample);

        bw.Write(new char[4] { 'd', 'a', 't', 'a' });
        bw.Write(length);
    }

    public void WriteSamples(Stream stream, byte[] sampleData)
    {
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(sampleData, 0, sampleData.Length);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _waveStream.Read(buffer, offset, count);
    }

    public virtual void WriteTo(Stream stream)
    {
        int bytesRead = 0;
        byte[] buffer = new byte[8192];

        do
        {
            bytesRead = Read(buffer, 0, buffer.Length);
            stream.Write(buffer, 0, bytesRead);
        } while (bytesRead > 0);

        stream.Flush();
    }

    public override void Flush()
    {
        _waveStream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _waveStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}
