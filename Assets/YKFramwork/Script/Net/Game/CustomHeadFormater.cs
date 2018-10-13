using System;
using Newtonsoft.Json;
using SprotoType;
using YKFramework;
using Sproto;

/// <summary>
/// 定制的头部结构解析
/// </summary>
public class CustomHeadFormater : IHeadFormater
{
    public bool TryParse(string data, NetworkType type, out PackageHead head, out object body)
    {
        body = null;
        head = null;
        try
        {
            ResponseBody result = JsonConvert.DeserializeObject<ResponseBody>(data);
            if (result == null) return false;

            head = new PackageHead();
            head.StatusCode = result.StateCode;
            head.Description = result.StateDescription;
            body = result.Data;
            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool TryParse(byte[] data, out PackageHead head, out byte[] bodyBytes)
    {
        bodyBytes = new byte[0];
        head = null;
        int pos = 0;
        if (data == null || data.Length == 0)
        {
            return false;
        }
        Sproto.SprotoPack pack = new Sproto.SprotoPack();
        var bin = pack.unpack(data);
        SprotoTypeDeserialize d = new SprotoTypeDeserialize(bin);
        SprotoTypeReader sReader = new SprotoTypeReader(bin,0,bin.Length);
        SprotoType.package headpack = new SprotoType.package();
        headpack.init(sReader);
        head = new PackageHead();
        head.ActionId = headpack.protoId;
        head.MsgId = headpack.session;
        head.StatusCode = headpack.errorcode;
        
        if (sReader.Length - sReader.Position > 0)
        {
            bodyBytes = new byte[sReader.Length - sReader.Position];
            sReader.Read(bodyBytes, 0, bodyBytes.Length);
        }
        return true;
    }

    public byte[] BuildHearbeatPackage()
    {
        var writer = NetWriter.Instance;
        SprotoType.package headPack = new SprotoType.package()
        {
            protoId = 2,
            session = NetWriter.MsgId,
            errorcode = 0,
        };
        Sproto.SprotoPack pack = new Sproto.SprotoPack();
        byte[] headBytes = headPack.encode();
        writer.SetHeadBuffer(headBytes);
        writer.SetBodyData(new byte[0]);
        var data = writer.PostData();
        NetWriter.resetData();
        return data;
    }

    private UInt16 GetUInt16(byte[] data, ref int pos)
    {
        UInt16 val = 0;
        if (BitConverter.IsLittleEndian)
        {
            val = BitConverter.ToUInt16(new byte[2] { data[1], data[0] }, pos);
        }
        else
        {
            val = BitConverter.ToUInt16(new byte[2] { data[0], data[1] }, pos);
        }
        pos += sizeof(UInt16);
        return val;
    }
}
