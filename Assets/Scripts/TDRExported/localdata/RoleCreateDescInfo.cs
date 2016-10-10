/* This file is generated by tdr. */
/* No manual modification is permitted. */

/* metalib version: 1 */
/* metalib md5sum: d27f0e7ec25d16514e3b8839bcc6ab7e */

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


public class RoleCreateDescInfo : tsf4g_csharp_interface
{
    /* public members */
    public UInt32 CareerID;
    public byte[] CareerName;
    public byte[] CareerDesc;
    public UInt32 ControlPower;
    public UInt32 SurvivePower;
    public UInt32 AttackPower;
    public byte[][] AppearancePic;

    /* construct methods */
    public RoleCreateDescInfo()
    {
        CareerName = new byte[24];
        CareerDesc = new byte[256];
        AppearancePic = new byte[6][];
        for (int AppearancePic_i = 0;AppearancePic_i < 6;AppearancePic_i++)
        {
            AppearancePic[AppearancePic_i] = new byte[128];
        }
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
        if (0 == cutVer || RoleCreateDescInfo.CURRVERSION < cutVer)
        {
            cutVer = RoleCreateDescInfo.CURRVERSION;
        }

        /* check cutversion */
        if (RoleCreateDescInfo.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.CareerID */
        {
            ret = destBuf.writeUInt32(this.CareerID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.CareerName */
        {
            /* record sizeinfo position */
            Int32 sizePos4CareerName = destBuf.getUsedSize();

            /* reserve space for sizeinfo */
            ret = destBuf.reserve(sizeof(UInt32));
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* record real-data's begin postion in buf */
            Int32 beginPos4CareerName = destBuf.getUsedSize();

            Int32 realSize4CareerName = TdrTypeUtil.cstrlen(this.CareerName);

            if (realSize4CareerName >= 24)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* pack */
            ret = destBuf.writeCString(this.CareerName, realSize4CareerName);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* gurantee string or wstring terminated with null character */
            ret = destBuf.writeUInt8(0x00);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* set sizeinfo for this.CareerName */
            Int32 size4CareerName = destBuf.getUsedSize() - beginPos4CareerName;
            ret = destBuf.writeUInt32((UInt32)(size4CareerName), sizePos4CareerName);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.CareerDesc */
        {
            /* record sizeinfo position */
            Int32 sizePos4CareerDesc = destBuf.getUsedSize();

            /* reserve space for sizeinfo */
            ret = destBuf.reserve(sizeof(UInt32));
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* record real-data's begin postion in buf */
            Int32 beginPos4CareerDesc = destBuf.getUsedSize();

            Int32 realSize4CareerDesc = TdrTypeUtil.cstrlen(this.CareerDesc);

            if (realSize4CareerDesc >= 256)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* pack */
            ret = destBuf.writeCString(this.CareerDesc, realSize4CareerDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* gurantee string or wstring terminated with null character */
            ret = destBuf.writeUInt8(0x00);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* set sizeinfo for this.CareerDesc */
            Int32 size4CareerDesc = destBuf.getUsedSize() - beginPos4CareerDesc;
            ret = destBuf.writeUInt32((UInt32)(size4CareerDesc), sizePos4CareerDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.ControlPower */
        {
            ret = destBuf.writeUInt32(this.ControlPower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.SurvivePower */
        {
            ret = destBuf.writeUInt32(this.SurvivePower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.AttackPower */
        {
            ret = destBuf.writeUInt32(this.AttackPower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.AppearancePic */
        {
            for (Int32 AppearancePic_i = 0; AppearancePic_i < 6; AppearancePic_i++)
            {
                /* record sizeinfo position */
                Int32 sizePos4AppearancePic = destBuf.getUsedSize();

                /* reserve space for sizeinfo */
                ret = destBuf.reserve(sizeof(UInt32));
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }

                /* record real-data's begin postion in buf */
                Int32 beginPos4AppearancePic = destBuf.getUsedSize();

                Int32 realSize4AppearancePic = TdrTypeUtil.cstrlen(this.AppearancePic[AppearancePic_i]);

                if (realSize4AppearancePic >= 128)
                {
                    return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
                }

                /* pack */
                ret = destBuf.writeCString(this.AppearancePic[AppearancePic_i], realSize4AppearancePic);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }

                /* gurantee string or wstring terminated with null character */
                ret = destBuf.writeUInt8(0x00);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }

                /* set sizeinfo for this.AppearancePic[AppearancePic_i] */
                Int32 size4AppearancePic = destBuf.getUsedSize() - beginPos4AppearancePic;
                ret = destBuf.writeUInt32((UInt32)(size4AppearancePic), sizePos4AppearancePic);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }
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
        if (0 == cutVer || RoleCreateDescInfo.CURRVERSION < cutVer)
        {
            cutVer = RoleCreateDescInfo.CURRVERSION;
        }

        /* check cutversion */
        if (RoleCreateDescInfo.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.CareerID */
        {
            ret = srcBuf.readUInt32(ref this.CareerID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.CareerName */
        {
            /* get sizeinfo for this.CareerName */
            UInt32 size4CareerName = 0;
            ret = srcBuf.readUInt32(ref size4CareerName);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether data in buffer are enough */
            if ((int)size4CareerName > srcBuf.getLeftSize())
            {
                return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
            }

            /* check whether sizeinfo is valid */
            if ((int)size4CareerName > this.CareerName.GetLength(0))
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* string or wstring must contains a null character */
            if (1 > size4CareerName)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
            }

            /* unpack */
            ret = srcBuf.readCString(ref this.CareerName, (int)size4CareerName);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether string-content is consistent with sizeinfo */
            if (0 != this.CareerName[(int)size4CareerName - 1])
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
            Int32 realSize4CareerName = TdrTypeUtil.cstrlen(this.CareerName) + 1;
            if (size4CareerName != realSize4CareerName)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
        }

        /* unpack member: this.CareerDesc */
        {
            /* get sizeinfo for this.CareerDesc */
            UInt32 size4CareerDesc = 0;
            ret = srcBuf.readUInt32(ref size4CareerDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether data in buffer are enough */
            if ((int)size4CareerDesc > srcBuf.getLeftSize())
            {
                return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
            }

            /* check whether sizeinfo is valid */
            if ((int)size4CareerDesc > this.CareerDesc.GetLength(0))
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* string or wstring must contains a null character */
            if (1 > size4CareerDesc)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
            }

            /* unpack */
            ret = srcBuf.readCString(ref this.CareerDesc, (int)size4CareerDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether string-content is consistent with sizeinfo */
            if (0 != this.CareerDesc[(int)size4CareerDesc - 1])
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
            Int32 realSize4CareerDesc = TdrTypeUtil.cstrlen(this.CareerDesc) + 1;
            if (size4CareerDesc != realSize4CareerDesc)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
        }

        /* unpack member: this.ControlPower */
        {
            ret = srcBuf.readUInt32(ref this.ControlPower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.SurvivePower */
        {
            ret = srcBuf.readUInt32(ref this.SurvivePower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.AttackPower */
        {
            ret = srcBuf.readUInt32(ref this.AttackPower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.AppearancePic */
        {
            for (Int32 AppearancePic_i = 0; AppearancePic_i < 6; AppearancePic_i++)
            {
                /* get sizeinfo for this.AppearancePic[AppearancePic_i] */
                UInt32 size4AppearancePic = 0;
                ret = srcBuf.readUInt32(ref size4AppearancePic);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }

                /* check whether data in buffer are enough */
                if ((int)size4AppearancePic > srcBuf.getLeftSize())
                {
                    return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
                }

                /* check whether sizeinfo is valid */
                if ((int)size4AppearancePic > this.AppearancePic[AppearancePic_i].GetLength(0))
                {
                    return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
                }

                /* string or wstring must contains a null character */
                if (1 > size4AppearancePic)
                {
                    return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
                }

                /* unpack */
                ret = srcBuf.readCString(ref this.AppearancePic[AppearancePic_i], (int)size4AppearancePic);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }

                /* check whether string-content is consistent with sizeinfo */
                if (0 != this.AppearancePic[AppearancePic_i][(int)size4AppearancePic - 1])
                {
                    return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
                }
                Int32 realSize4AppearancePic = TdrTypeUtil.cstrlen(this.AppearancePic[AppearancePic_i]) + 1;
                if (size4AppearancePic != realSize4AppearancePic)
                {
                    return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
                }
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
        if (0 == cutVer || RoleCreateDescInfo.CURRVERSION < cutVer)
        {
            cutVer = RoleCreateDescInfo.CURRVERSION;
        }

        /* check cutversion */
        if (RoleCreateDescInfo.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.CareerID */
        {
            ret = srcBuf.readUInt32(ref this.CareerID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.CareerName */
        {
            Int32 size4CareerName = 24;

            /* load */
            ret = srcBuf.readCString(ref this.CareerName, (int)size4CareerName);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

        }

        /* load member: this.CareerDesc */
        {
            Int32 size4CareerDesc = 256;

            /* load */
            ret = srcBuf.readCString(ref this.CareerDesc, (int)size4CareerDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

        }

        /* load member: this.ControlPower */
        {
            ret = srcBuf.readUInt32(ref this.ControlPower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.SurvivePower */
        {
            ret = srcBuf.readUInt32(ref this.SurvivePower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.AttackPower */
        {
            ret = srcBuf.readUInt32(ref this.AttackPower);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.AppearancePic */
        {
            for (Int32 AppearancePic_i = 0; AppearancePic_i < 6; AppearancePic_i++)
            {
                Int32 size4AppearancePic = 128;

                /* load */
                ret = srcBuf.readCString(ref this.AppearancePic[AppearancePic_i], (int)size4AppearancePic);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
                }

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

        /* visualize member: this.CareerID */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[CareerID]", "{0:d}", this.CareerID);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.CareerName */
        ret = TdrBufUtil.printString(ref destBuf, indent, separator, "[CareerName]", this.CareerName);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.CareerDesc */
        ret = TdrBufUtil.printString(ref destBuf, indent, separator, "[CareerDesc]", this.CareerDesc);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.ControlPower */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[ControlPower]", "{0:d}", this.ControlPower);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.SurvivePower */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[SurvivePower]", "{0:d}", this.SurvivePower);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.AttackPower */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[AttackPower]", "{0:d}", this.AttackPower);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.AppearancePic */
        for (Int32 AppearancePic_i = 0; AppearancePic_i < 6; AppearancePic_i++)
        {
            ret = TdrBufUtil.printString(ref destBuf, indent, separator, "[AppearancePic]", AppearancePic_i, this.AppearancePic[AppearancePic_i]);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
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
