/* This file is generated by tdr. */
/* No manual modification is permitted. */

/* metalib version: 1 */
/* metalib md5sum: 3c8675bcfb3436d7ec6801ca2e1d3691 */

/* creation time: Wed Apr 15 10:10:32 2015 */
/* tdr version: 2.6.7, build at 20131230 */


using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using tsf4g_tdr_csharp;

namespace wl_res
{


/* 密室逃脱限制表 */
public class RoomEscapeLimit : tsf4g_csharp_interface
{
    /* public members */
    public Int32 VipLevel;
    public Int32 MaxChallengeNum;
    public Int32 MaxResetNum;

    /* construct methods */
    public RoomEscapeLimit()
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
        if (0 == cutVer || RoomEscapeLimit.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeLimit.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeLimit.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.VipLevel */
        {
            ret = destBuf.writeInt32(this.VipLevel);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.MaxChallengeNum */
        {
            ret = destBuf.writeInt32(this.MaxChallengeNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.MaxResetNum */
        {
            ret = destBuf.writeInt32(this.MaxResetNum);
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
        if (0 == cutVer || RoomEscapeLimit.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeLimit.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeLimit.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.VipLevel */
        {
            ret = srcBuf.readInt32(ref this.VipLevel);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.MaxChallengeNum */
        {
            ret = srcBuf.readInt32(ref this.MaxChallengeNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.MaxResetNum */
        {
            ret = srcBuf.readInt32(ref this.MaxResetNum);
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
        if (0 == cutVer || RoomEscapeLimit.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeLimit.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeLimit.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.VipLevel */
        {
            ret = srcBuf.readInt32(ref this.VipLevel);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.MaxChallengeNum */
        {
            ret = srcBuf.readInt32(ref this.MaxChallengeNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.MaxResetNum */
        {
            ret = srcBuf.readInt32(ref this.MaxResetNum);
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

        /* visualize member: this.VipLevel */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[VipLevel]", "{0:d}", this.VipLevel);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.MaxChallengeNum */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[MaxChallengeNum]", "{0:d}", this.MaxChallengeNum);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.MaxResetNum */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[MaxResetNum]", "{0:d}", this.MaxResetNum);
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


/* 重置消耗 */
public class RoomEscapeResetCost : tsf4g_csharp_interface
{
    /* public members */
    public Int32 ResetNum;
    public Int32 NeedDiamond;

    /* construct methods */
    public RoomEscapeResetCost()
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
        if (0 == cutVer || RoomEscapeResetCost.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeResetCost.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeResetCost.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.ResetNum */
        {
            ret = destBuf.writeInt32(this.ResetNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.NeedDiamond */
        {
            ret = destBuf.writeInt32(this.NeedDiamond);
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
        if (0 == cutVer || RoomEscapeResetCost.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeResetCost.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeResetCost.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.ResetNum */
        {
            ret = srcBuf.readInt32(ref this.ResetNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.NeedDiamond */
        {
            ret = srcBuf.readInt32(ref this.NeedDiamond);
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
        if (0 == cutVer || RoomEscapeResetCost.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeResetCost.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeResetCost.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.ResetNum */
        {
            ret = srcBuf.readInt32(ref this.ResetNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.NeedDiamond */
        {
            ret = srcBuf.readInt32(ref this.NeedDiamond);
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

        /* visualize member: this.ResetNum */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[ResetNum]", "{0:d}", this.ResetNum);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.NeedDiamond */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[NeedDiamond]", "{0:d}", this.NeedDiamond);
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


/* 击杀怪物积分 */
public class RoomEscapeKillMonsterScore : tsf4g_csharp_interface
{
    /* public members */
    public Int32 MonsterID; // 怪物id
    public Int32 score; // 所得积分

    /* construct methods */
    public RoomEscapeKillMonsterScore()
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
        if (0 == cutVer || RoomEscapeKillMonsterScore.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeKillMonsterScore.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeKillMonsterScore.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.MonsterID */
        {
            ret = destBuf.writeInt32(this.MonsterID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.score */
        {
            ret = destBuf.writeInt32(this.score);
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
        if (0 == cutVer || RoomEscapeKillMonsterScore.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeKillMonsterScore.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeKillMonsterScore.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.MonsterID */
        {
            ret = srcBuf.readInt32(ref this.MonsterID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.score */
        {
            ret = srcBuf.readInt32(ref this.score);
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
        if (0 == cutVer || RoomEscapeKillMonsterScore.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeKillMonsterScore.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeKillMonsterScore.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.MonsterID */
        {
            ret = srcBuf.readInt32(ref this.MonsterID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.score */
        {
            ret = srcBuf.readInt32(ref this.score);
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

        /* visualize member: this.MonsterID */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[MonsterID]", "{0:d}", this.MonsterID);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.score */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[score]", "{0:d}", this.score);
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


/* 密室逃脱礼包 */
public class RoomEscapeGiftBag : tsf4g_csharp_interface
{
    /* public members */
    public Int32 Num; // 行号
    public Int32 MinScore; // 最低积分
    public Int32 MaxScore; // 最高积分
    public Int32 GiftBagID; // 礼包id

    /* construct methods */
    public RoomEscapeGiftBag()
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
        if (0 == cutVer || RoomEscapeGiftBag.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeGiftBag.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeGiftBag.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.Num */
        {
            ret = destBuf.writeInt32(this.Num);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.MinScore */
        {
            ret = destBuf.writeInt32(this.MinScore);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.MaxScore */
        {
            ret = destBuf.writeInt32(this.MaxScore);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.GiftBagID */
        {
            ret = destBuf.writeInt32(this.GiftBagID);
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
        if (0 == cutVer || RoomEscapeGiftBag.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeGiftBag.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeGiftBag.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.Num */
        {
            ret = srcBuf.readInt32(ref this.Num);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.MinScore */
        {
            ret = srcBuf.readInt32(ref this.MinScore);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.MaxScore */
        {
            ret = srcBuf.readInt32(ref this.MaxScore);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.GiftBagID */
        {
            ret = srcBuf.readInt32(ref this.GiftBagID);
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
        if (0 == cutVer || RoomEscapeGiftBag.CURRVERSION < cutVer)
        {
            cutVer = RoomEscapeGiftBag.CURRVERSION;
        }

        /* check cutversion */
        if (RoomEscapeGiftBag.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.Num */
        {
            ret = srcBuf.readInt32(ref this.Num);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.MinScore */
        {
            ret = srcBuf.readInt32(ref this.MinScore);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.MaxScore */
        {
            ret = srcBuf.readInt32(ref this.MaxScore);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.GiftBagID */
        {
            ret = srcBuf.readInt32(ref this.GiftBagID);
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

        /* visualize member: this.Num */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[Num]", "{0:d}", this.Num);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.MinScore */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[MinScore]", "{0:d}", this.MinScore);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.MaxScore */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[MaxScore]", "{0:d}", this.MaxScore);
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
