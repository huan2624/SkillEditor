/* This file is generated by tdr. */
/* No manual modification is permitted. */

/* metalib version: 1 */
/* metalib md5sum: adf97275311689ab9641e9bb5eef5a22 */

/* creation time: Wed Jul 16 17:37:33 2014 */
/* tdr version: 2.6.7, build at 20131230 */


using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using tsf4g_tdr_csharp;

namespace wl_res
{


public class ObjectInfo : tsf4g_csharp_interface
{
    /* public members */
    public UInt32 ObjectID;
    public UInt32 ResID;
    public UInt32 HitNum;
    public float DelayDestoryTime;
    public UInt32 DeathGold;
    public UInt32 ObjType;
    public byte ShowHitPreEffect;

    /* construct methods */
    public ObjectInfo()
    {
    }

    /* public methods */
    public TdrError.ErrorType construct()
    {
        TdrError.ErrorType ret = TdrError.ErrorType.TDR_NO_ERROR;

        /*no need to  set defaultvalue */

        return ret;
    }

    public TdrError.ErrorType pack(ref byte[] buffer, int size,ref int usedSize , uint cutVer)
    {
        if (null == buffer || 0 == buffer.GetLength(0) || (size > buffer.GetLength(0)))
        {
            return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
        }

        TdrWriteBuf destBuf = new TdrWriteBuf(ref buffer, size);
        TdrError.ErrorType ret = pack(ref destBuf, cutVer);
        if (ret == TdrError.ErrorType.TDR_NO_ERROR)
        {
            buffer = destBuf.getBeginPtr();

            usedSize = destBuf.getUsedSize();
        }

        return ret;
    }

    public TdrError.ErrorType pack(ref TdrWriteBuf destBuf,  uint cutVer)
    {
        TdrError.ErrorType ret = TdrError.ErrorType.TDR_NO_ERROR;

        /* adjust cutversion */
        if (0 == cutVer || ObjectInfo.CURRVERSION < cutVer)
        {
            cutVer = ObjectInfo.CURRVERSION;
        }

        /* check cutversion */
        if (ObjectInfo.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.ObjectID */
        {
            ret = destBuf.writeUInt32(this.ObjectID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.ResID */
        {
            ret = destBuf.writeUInt32(this.ResID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.HitNum */
        {
            ret = destBuf.writeUInt32(this.HitNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.DelayDestoryTime */
        {
            ret = destBuf.writeFloat(this.DelayDestoryTime);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.DeathGold */
        {
            ret = destBuf.writeUInt32(this.DeathGold);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.ObjType */
        {
            ret = destBuf.writeUInt32(this.ObjType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.ShowHitPreEffect */
        {
            ret = destBuf.writeUInt8(this.ShowHitPreEffect);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        return ret;
    }

    public TdrError.ErrorType unpack(ref byte [] buffer, int size, ref int usedSize , uint cutVer)
    {
        if (null == buffer || 0 == buffer.GetLength(0) || size > buffer.GetLength(0))
        {
            return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
        }

        TdrReadBuf srcBuf = new TdrReadBuf(ref buffer, size);
        TdrError.ErrorType ret = unpack(ref srcBuf, cutVer);

        usedSize = srcBuf.getUsedSize();

        return ret;
    }

    public TdrError.ErrorType unpack(ref TdrReadBuf srcBuf, uint cutVer)
    {
        TdrError.ErrorType ret = TdrError.ErrorType.TDR_NO_ERROR;

        /* adjust cutversion */
        if (0 == cutVer || ObjectInfo.CURRVERSION < cutVer)
        {
            cutVer = ObjectInfo.CURRVERSION;
        }

        /* check cutversion */
        if (ObjectInfo.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.ObjectID */
        {
            ret = srcBuf.readUInt32(ref this.ObjectID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.ResID */
        {
            ret = srcBuf.readUInt32(ref this.ResID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.HitNum */
        {
            ret = srcBuf.readUInt32(ref this.HitNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.DelayDestoryTime */
        {
            ret = srcBuf.readFloat(ref this.DelayDestoryTime);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.DeathGold */
        {
            ret = srcBuf.readUInt32(ref this.DeathGold);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.ObjType */
        {
            ret = srcBuf.readUInt32(ref this.ObjType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.ShowHitPreEffect */
        {
            ret = srcBuf.readUInt8(ref this.ShowHitPreEffect);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        return ret;
    }

    public TdrError.ErrorType load(ref byte [] buffer, int size, ref int usedSize , uint cutVer)
    {
        if (null == buffer || 0 == buffer.GetLength(0) || size > buffer.GetLength(0))
        {
            return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
        }

        TdrReadBuf srcBuf = new TdrReadBuf(ref buffer, size);
        TdrError.ErrorType ret = load(ref srcBuf, cutVer);

        usedSize = srcBuf.getUsedSize();

        return ret;
    }

    public TdrError.ErrorType load(ref TdrReadBuf srcBuf, uint cutVer)
    {
        srcBuf.disableEndian();
        TdrError.ErrorType ret = TdrError.ErrorType.TDR_NO_ERROR;

        /* adjust cutversion */
        if (0 == cutVer || ObjectInfo.CURRVERSION < cutVer)
        {
            cutVer = ObjectInfo.CURRVERSION;
        }

        /* check cutversion */
        if (ObjectInfo.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.ObjectID */
        {
            ret = srcBuf.readUInt32(ref this.ObjectID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.ResID */
        {
            ret = srcBuf.readUInt32(ref this.ResID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.HitNum */
        {
            ret = srcBuf.readUInt32(ref this.HitNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.DelayDestoryTime */
        {
            ret = srcBuf.readFloat(ref this.DelayDestoryTime);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.DeathGold */
        {
            ret = srcBuf.readUInt32(ref this.DeathGold);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.ObjType */
        {
            ret = srcBuf.readUInt32(ref this.ObjType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.ShowHitPreEffect */
        {
            ret = srcBuf.readUInt8(ref this.ShowHitPreEffect);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        return ret;
    }

    /* set indent = -1 to disable indent , default: separator = '\n' */
    public TdrError.ErrorType visualize(ref string buffer,int indent , char separator)
    {
        TdrVisualBuf destBuf = new TdrVisualBuf();
        TdrError.ErrorType ret = visualize(ref destBuf, indent, separator);

        buffer = destBuf.getVisualBuf();

        return ret;
    }

    /* set indent = -1 to disable indent , default: separator = '\n' */
    public TdrError.ErrorType visualize(ref TdrVisualBuf destBuf, int indent, char separator)
    {
        TdrError.ErrorType ret = TdrError.ErrorType.TDR_NO_ERROR;

        /* visualize member: this.ObjectID */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[ObjectID]", "{0:d}", this.ObjectID);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.ResID */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[ResID]", "{0:d}", this.ResID);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.HitNum */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[HitNum]", "{0:d}", this.HitNum);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.DelayDestoryTime */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[DelayDestoryTime]", "{0:g}", this.DelayDestoryTime);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.DeathGold */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[DeathGold]", "{0:d}", this.DeathGold);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.ObjType */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[ObjType]", "{0:d}", this.ObjType);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.ShowHitPreEffect */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[ShowHitPreEffect]", "0x{0:x2}", this.ShowHitPreEffect);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        return ret;
    }

    public TdrError.ErrorType getSizeInfo(byte[] buffer ,int size , ref uint sizeInfo)
    {
        if (0 == buffer.GetLength(0) || size > buffer.GetLength(0))
        {
            return TdrError.ErrorType.TDR_ERR_INVALID_BUFFER_PARAMETER;
        }

        TdrReadBuf srcBuf = new TdrReadBuf(ref buffer, size);
        TdrError.ErrorType ret = getSizeInfo(srcBuf, ref sizeInfo);

        return ret;
    }

    public TdrError.ErrorType getSizeInfo(TdrReadBuf srcBuf, ref uint sizeInfo)
    {
        return TdrError.ErrorType.TDR_ERR_HAVE_NOT_SET_SIZEINFO;
    }

    /*  pstMeta version info */
    public static readonly uint BASEVERSION = 1;
    public static readonly uint CURRVERSION = 1;
    /*  entry version info */
}


}
