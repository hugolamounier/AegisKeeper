using System.IO.Compression;

namespace AegisKeeper.Core.Misc;

public static class StreamCompressor
{
    public static async Task<Stream> CompressAsync(Stream inputContent)
    {
        ArgumentNullException.ThrowIfNull(inputContent);
        
        if(inputContent.Length == 0L)
            throw new InvalidOperationException("Cannot compress an empty stream.");
        
        var compressedStream = new MemoryStream();
        await using (var zipStream = new GZipStream(compressedStream, CompressionLevel.Optimal))
        {
            await inputContent.CopyToAsync(zipStream);
        }
        
        if(compressedStream.CanSeek)
            compressedStream.Seek(0, SeekOrigin.Begin);

        return compressedStream;
    }
    
    public static async Task<Stream> DecompressAsync(Stream compressedContent)
    {
        ArgumentNullException.ThrowIfNull(compressedContent);
        
        if(compressedContent.Length == 0L)
            throw new InvalidOperationException("Cannot decompress an empty stream.");
        
        var decompressedStream = new MemoryStream();
        await using (var zipStream = new GZipStream(compressedContent, CompressionMode.Decompress))
        {
            await zipStream.CopyToAsync(decompressedStream);
        }
        
        if(decompressedStream.CanSeek)
            decompressedStream.Seek(0, SeekOrigin.Begin);

        return decompressedStream;
    }
}