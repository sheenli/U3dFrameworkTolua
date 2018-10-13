using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class CompressHelper 
{
    public static void LZMAEncode(Stream inStream, Stream outStream)
    {
        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
        // Write the encoder properties
        coder.WriteCoderProperties(outStream);
        // Write the decompressed file size.
        outStream.Write(BitConverter.GetBytes(inStream.Length), 0, 8);
        // Encode the file.
        coder.Code(inStream, outStream, inStream.Length, -1, null);
        outStream.Flush();
    }

    public static void LZMADecode(Stream inStream, Stream outStream)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();

        byte[] properties = new byte[5];
        inStream.Read(properties, 0, 5);

        // Read in the decompress file size.
        byte[] fileLengthBytes = new byte[8];
        inStream.Read(fileLengthBytes, 0, 8);
        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

        // Decompress the file.
        coder.SetDecoderProperties(properties);
        coder.Code(inStream, outStream, inStream.Length, fileLength, null);
        outStream.Flush();
        inStream.Close();
    }

    public static void CompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        // Write the encoder properties
        coder.WriteCoderProperties(output);

        // Write the decompressed file size.
        output.Write(BitConverter.GetBytes(input.Length), 0, 8);

        // Encode the file.
        coder.Code(input, output, input.Length, -1, null);
        output.Flush();
        output.Close();
        input.Close();
    }

    public static void DecompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        // Read the decoder properties
        byte[] properties = new byte[5];
        input.Read(properties, 0, 5);

        // Read in the decompress file size.
        byte[] fileLengthBytes = new byte[8];
        input.Read(fileLengthBytes, 0, 8);
        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

        // Decompress the file.
        coder.SetDecoderProperties(properties);
        coder.Code(input, output, input.Length, fileLength, null);
        output.Flush();
        output.Close();
        input.Close();
    }

    public static void DecompressWWWLZMA(WWW www, string outFile)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        using (MemoryStream input = new MemoryStream(www.bytes, 0, www.bytes.Length))
        {
            using (FileStream output = new FileStream(outFile, FileMode.Create))
            {
                // Read the decoder properties
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);

                // Read in the decompress file size.
                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                // Decompress the file.
                coder.SetDecoderProperties(properties);
                coder.Code(input, output, input.Length, fileLength, null);
                output.Flush();
                //output.Close();
            }

            //input.Close();
        }
    }


    public static void test()
    {
    }

    public static void DecompressBytesLZMA(byte[] bytes, string outFile)
    {
        if (File.Exists(outFile))
        {
            File.Delete(outFile);
        }
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        using (MemoryStream input = new MemoryStream(bytes, 0, bytes.Length))
        {
            using (FileStream output = new FileStream(outFile, FileMode.Create))
            {
                // Read the decoder properties
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);

                // Read in the decompress file size.
                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                // Decompress the file.
                coder.SetDecoderProperties(properties);
                coder.Code(input, output, input.Length, fileLength, null);
                output.Flush();
                //output.Close();
            }

            //input.Close();
        }
    }

    public static byte[] DecompressBytesLZMA(byte[] bytes)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        byte[] bs = null;
        using (MemoryStream input = new MemoryStream(bytes, 0, bytes.Length))
        {
            using (MemoryStream output = new MemoryStream())
            {
                // Read the decoder properties
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);

                // Read in the decompress file size.
                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                // Decompress the file.
                coder.SetDecoderProperties(properties);
                coder.Code(input, output, input.Length, fileLength, null);
                output.Flush();
                bytes = output.GetBuffer();
                //output.Close();
            }

            //input.Close();
        }
        return bytes;
    }

    public static void CompressBytesLZMA(byte[] bytes, string outFile)
    {
        using (FileStream output = new FileStream(outFile, FileMode.Create))
        {
            LZMAEncode(new MemoryStream(bytes), output);
            output.Flush();
        }
    }
}
