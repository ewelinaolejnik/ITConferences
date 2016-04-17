using System;
using System.IO;
using System.Text;

namespace ITConferences.WebUI.Extensions
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            stream.Position = 0;
            var buffer = new byte[stream.Length];
            for (var totalBytesCopied = 0; totalBytesCopied < stream.Length;)
                totalBytesCopied += stream.Read(buffer, totalBytesCopied,
                    Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }
    }
}