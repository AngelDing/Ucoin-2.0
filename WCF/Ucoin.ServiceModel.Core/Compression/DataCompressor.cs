using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Ucoin.ServiceModel.Core
{
    public class DataCompressor
    {
        public CompressionAlgorithm Algorithm { get; private set; }
        public DataCompressor(CompressionAlgorithm algorithm)
        {
            this.Algorithm = algorithm;
        }

        public virtual byte[] Compress(byte[] decompressedData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (this.Algorithm == CompressionAlgorithm.GZip)
                {
                    GZipStream stream2 = new GZipStream(ms, CompressionMode.Compress, true);
                    stream2.Write(decompressedData, 0, decompressedData.Length);
                    stream2.Close();
                }
                else if (this.Algorithm == CompressionAlgorithm.Deflate)
                {
                    DeflateStream stream3 = new DeflateStream(ms, CompressionMode.Compress, true);
                    stream3.Write(decompressedData, 0, decompressedData.Length);
                    stream3.Close();
                }
                else
                {
                    var bzipStream = new BZip2OutputStream(ms);
                    bzipStream.Write(decompressedData, 0, decompressedData.Length);
                    bzipStream.Close();
                }
                return ms.ToArray();
            }
        }
        public virtual byte[] Decompress(byte[] compressedData, CompressionAlgorithm algorithm)
        {
            using (MemoryStream ms = new MemoryStream(compressedData))
            {
                if (algorithm == CompressionAlgorithm.GZip)
                {
                    using (GZipStream stream2 = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        return LoadToBuffer(stream2);
                    }
                }
                else if (this.Algorithm == CompressionAlgorithm.Deflate)
                {
                    using (DeflateStream stream3 = new DeflateStream(ms, CompressionMode.Decompress))
                    {
                        return LoadToBuffer(stream3);
                    }
                }
                else
                {
                    using (var bzipStream = new BZip2InputStream(ms))
                    {
                        return LoadToBuffer(bzipStream);
                        //这里要指明要读入的格式，要不就有乱码
                        //StreamReader reader = new StreamReader(bzipStream, Encoding.UTF8);
                        //reader.
                        //var commonString = reader.ReadToEnd();

                        //return commonString;
                    }
                }
            }
        }
        private static byte[] LoadToBuffer(Stream stream)
        {
            using (MemoryStream stream2 = new MemoryStream())
            {
                int num;
                byte[] buffer = new byte[0x400];
                while ((num = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream2.Write(buffer, 0, num);
                }
                return stream2.ToArray();
            }
        }
    }
}