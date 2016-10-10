/* This file is generated by tdr. */
/* No manual modification is permitted. */

/* metalib version: 1 */
/* metalib md5sum: a7349dc383b180cd8dbdd1231b363184 */

/* creation time: Mon May 11 20:54:49 2015 */
/* tdr version: 2.6.7, build at 20131230 */


using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using tsf4g_tdr_csharp;

namespace wl_res
{


/* BOSS战开放限制 */
public class BossBattleOpen : tsf4g_csharp_interface
{
    /* public members */
    public Int16 Difficult; // 难度
    public Int32 MinLevel; // 开放等级

    /* construct methods */
    public BossBattleOpen()
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
        if (0 == cutVer || BossBattleOpen.CURRVERSION < cutVer)
        {
            cutVer = BossBattleOpen.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleOpen.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.Difficult */
        {
            ret = destBuf.writeInt16(this.Difficult);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.MinLevel */
        {
            ret = destBuf.writeInt32(this.MinLevel);
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
        if (0 == cutVer || BossBattleOpen.CURRVERSION < cutVer)
        {
            cutVer = BossBattleOpen.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleOpen.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.Difficult */
        {
            ret = srcBuf.readInt16(ref this.Difficult);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.MinLevel */
        {
            ret = srcBuf.readInt32(ref this.MinLevel);
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
        if (0 == cutVer || BossBattleOpen.CURRVERSION < cutVer)
        {
            cutVer = BossBattleOpen.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleOpen.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.Difficult */
        {
            ret = srcBuf.readInt16(ref this.Difficult);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.MinLevel */
        {
            ret = srcBuf.readInt32(ref this.MinLevel);
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

        /* visualize member: this.Difficult */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[Difficult]", "{0:d}", this.Difficult);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.MinLevel */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[MinLevel]", "{0:d}", this.MinLevel);
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


/* BOSS战挑战限制 */
public class BossBattleLimit : tsf4g_csharp_interface
{
    /* public members */
    public Int16 BossId; // BOSS编号
    public byte[] Model; // 模型
    public byte[] BossDesc; // BOSS描述
    public Int32 DailyChallengeMax; // 每日可挑战次数

    /* construct methods */
    public BossBattleLimit()
    {
        Model = new byte[128];
        BossDesc = new byte[128];
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
        if (0 == cutVer || BossBattleLimit.CURRVERSION < cutVer)
        {
            cutVer = BossBattleLimit.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleLimit.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.BossId */
        {
            ret = destBuf.writeInt16(this.BossId);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.Model */
        {
            /* record sizeinfo position */
            Int32 sizePos4Model = destBuf.getUsedSize();

            /* reserve space for sizeinfo */
            ret = destBuf.reserve(sizeof(UInt32));
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* record real-data's begin postion in buf */
            Int32 beginPos4Model = destBuf.getUsedSize();

            Int32 realSize4Model = TdrTypeUtil.cstrlen(this.Model);

            if (realSize4Model >= 128)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* pack */
            ret = destBuf.writeCString(this.Model, realSize4Model);
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

            /* set sizeinfo for this.Model */
            Int32 size4Model = destBuf.getUsedSize() - beginPos4Model;
            ret = destBuf.writeUInt32((UInt32)(size4Model), sizePos4Model);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.BossDesc */
        {
            /* record sizeinfo position */
            Int32 sizePos4BossDesc = destBuf.getUsedSize();

            /* reserve space for sizeinfo */
            ret = destBuf.reserve(sizeof(UInt32));
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* record real-data's begin postion in buf */
            Int32 beginPos4BossDesc = destBuf.getUsedSize();

            Int32 realSize4BossDesc = TdrTypeUtil.cstrlen(this.BossDesc);

            if (realSize4BossDesc >= 128)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* pack */
            ret = destBuf.writeCString(this.BossDesc, realSize4BossDesc);
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

            /* set sizeinfo for this.BossDesc */
            Int32 size4BossDesc = destBuf.getUsedSize() - beginPos4BossDesc;
            ret = destBuf.writeUInt32((UInt32)(size4BossDesc), sizePos4BossDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.DailyChallengeMax */
        {
            ret = destBuf.writeInt32(this.DailyChallengeMax);
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
        if (0 == cutVer || BossBattleLimit.CURRVERSION < cutVer)
        {
            cutVer = BossBattleLimit.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleLimit.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.BossId */
        {
            ret = srcBuf.readInt16(ref this.BossId);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.Model */
        {
            /* get sizeinfo for this.Model */
            UInt32 size4Model = 0;
            ret = srcBuf.readUInt32(ref size4Model);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether data in buffer are enough */
            if ((int)size4Model > srcBuf.getLeftSize())
            {
                return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
            }

            /* check whether sizeinfo is valid */
            if ((int)size4Model > this.Model.GetLength(0))
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* string or wstring must contains a null character */
            if (1 > size4Model)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
            }

            /* unpack */
            ret = srcBuf.readCString(ref this.Model, (int)size4Model);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether string-content is consistent with sizeinfo */
            if (0 != this.Model[(int)size4Model - 1])
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
            Int32 realSize4Model = TdrTypeUtil.cstrlen(this.Model) + 1;
            if (size4Model != realSize4Model)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
        }

        /* unpack member: this.BossDesc */
        {
            /* get sizeinfo for this.BossDesc */
            UInt32 size4BossDesc = 0;
            ret = srcBuf.readUInt32(ref size4BossDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether data in buffer are enough */
            if ((int)size4BossDesc > srcBuf.getLeftSize())
            {
                return TdrError.ErrorType.TDR_ERR_SHORT_BUF_FOR_READ;
            }

            /* check whether sizeinfo is valid */
            if ((int)size4BossDesc > this.BossDesc.GetLength(0))
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_BIG;
            }

            /* string or wstring must contains a null character */
            if (1 > size4BossDesc)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_TOO_SMALL;
            }

            /* unpack */
            ret = srcBuf.readCString(ref this.BossDesc, (int)size4BossDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* check whether string-content is consistent with sizeinfo */
            if (0 != this.BossDesc[(int)size4BossDesc - 1])
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
            Int32 realSize4BossDesc = TdrTypeUtil.cstrlen(this.BossDesc) + 1;
            if (size4BossDesc != realSize4BossDesc)
            {
                return TdrError.ErrorType.TDR_ERR_STR_LEN_CONFLICT;
            }
        }

        /* unpack member: this.DailyChallengeMax */
        {
            ret = srcBuf.readInt32(ref this.DailyChallengeMax);
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
        if (0 == cutVer || BossBattleLimit.CURRVERSION < cutVer)
        {
            cutVer = BossBattleLimit.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleLimit.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.BossId */
        {
            ret = srcBuf.readInt16(ref this.BossId);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.Model */
        {
            Int32 size4Model = 128;

            /* load */
            ret = srcBuf.readCString(ref this.Model, (int)size4Model);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

        }

        /* load member: this.BossDesc */
        {
            Int32 size4BossDesc = 128;

            /* load */
            ret = srcBuf.readCString(ref this.BossDesc, (int)size4BossDesc);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

        }

        /* load member: this.DailyChallengeMax */
        {
            ret = srcBuf.readInt32(ref this.DailyChallengeMax);
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

        /* visualize member: this.BossId */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[BossId]", "{0:d}", this.BossId);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.Model */
        ret = TdrBufUtil.printString(ref destBuf, indent, separator, "[Model]", this.Model);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.BossDesc */
        ret = TdrBufUtil.printString(ref destBuf, indent, separator, "[BossDesc]", this.BossDesc);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.DailyChallengeMax */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[DailyChallengeMax]", "{0:d}", this.DailyChallengeMax);
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


/* BOSS战奖励 */
public class BossBattleAward : tsf4g_csharp_interface
{
    /* public members */
    public Int16 BossId; // BOSS编号
    public Int16 Difficult; // 难度
    public UInt32 GiftBagID; // 礼包ID
    public UInt32 TimeLimit; // 限制时间

    /* construct methods */
    public BossBattleAward()
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
        if (0 == cutVer || BossBattleAward.CURRVERSION < cutVer)
        {
            cutVer = BossBattleAward.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleAward.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.BossId */
        {
            ret = destBuf.writeInt16(this.BossId);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.Difficult */
        {
            ret = destBuf.writeInt16(this.Difficult);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.GiftBagID */
        {
            ret = destBuf.writeUInt32(this.GiftBagID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.TimeLimit */
        {
            ret = destBuf.writeUInt32(this.TimeLimit);
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
        if (0 == cutVer || BossBattleAward.CURRVERSION < cutVer)
        {
            cutVer = BossBattleAward.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleAward.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.BossId */
        {
            ret = srcBuf.readInt16(ref this.BossId);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.Difficult */
        {
            ret = srcBuf.readInt16(ref this.Difficult);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.GiftBagID */
        {
            ret = srcBuf.readUInt32(ref this.GiftBagID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.TimeLimit */
        {
            ret = srcBuf.readUInt32(ref this.TimeLimit);
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
        if (0 == cutVer || BossBattleAward.CURRVERSION < cutVer)
        {
            cutVer = BossBattleAward.CURRVERSION;
        }

        /* check cutversion */
        if (BossBattleAward.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.BossId */
        {
            ret = srcBuf.readInt16(ref this.BossId);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.Difficult */
        {
            ret = srcBuf.readInt16(ref this.Difficult);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.GiftBagID */
        {
            ret = srcBuf.readUInt32(ref this.GiftBagID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.TimeLimit */
        {
            ret = srcBuf.readUInt32(ref this.TimeLimit);
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

        /* visualize member: this.BossId */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[BossId]", "{0:d}", this.BossId);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.Difficult */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[Difficult]", "{0:d}", this.Difficult);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.GiftBagID */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[GiftBagID]", "{0:d}", this.GiftBagID);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.TimeLimit */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[TimeLimit]", "{0:d}", this.TimeLimit);
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
