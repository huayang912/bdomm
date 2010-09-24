using System.IO;
using System.IO.Compression;

public static class Compressor
{
    /// <summary>
    /// Compresses a byte array with GZip Compression
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Compress(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        GZipStream gzip = new GZipStream(output,
                          CompressionMode.Compress, true);
        gzip.Write(data, 0, data.Length);
        gzip.Close();
        return output.ToArray();
    }
    /// <summary>
    /// Decompresses a byte array using GZipCompression Stream.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream();
        input.Write(data, 0, data.Length);
        input.Position = 0;
        GZipStream gzip = new GZipStream(input,
                          CompressionMode.Decompress, true);
        MemoryStream output = new MemoryStream();
        byte[] buff = new byte[64];
        int read = -1;
        read = gzip.Read(buff, 0, buff.Length);
        while (read > 0)
        {
            output.Write(buff, 0, read);
            read = gzip.Read(buff, 0, buff.Length);
        }
        gzip.Close();
        return output.ToArray();
    }
}