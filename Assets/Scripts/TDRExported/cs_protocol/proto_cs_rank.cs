/* This file is generated by tdr. */
/* No manual modification is permitted. */

/* metalib version: 1 */
/* metalib md5sum: cb9c5ea3d7cc76d0ec4a17348c3a6864 */

/* creation time: Mon Jun 02 18:05:48 2014 */
/* tdr version: 2.6.7, build at 20131230 */


using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using tsf4g_tdr_csharp;

namespace net
{


public class CSGetFriendRankReq : tsf4g_csharp_interface
{
    /* public members */
    public Int32 RankType; // 排行榜类型
    public Int32 PageIndex; // 页下标，从0开始

    /* construct methods */
    public CSGetFriendRankReq()
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
        if (0 == cutVer || CSGetFriendRankReq.CURRVERSION < cutVer)
        {
            cutVer = CSGetFriendRankReq.CURRVERSION;
        }

        /* check cutversion */
        if (CSGetFriendRankReq.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.RankType */
        {
            ret = destBuf.writeInt32(this.RankType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.PageIndex */
        {
            ret = destBuf.writeInt32(this.PageIndex);
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
        if (0 == cutVer || CSGetFriendRankReq.CURRVERSION < cutVer)
        {
            cutVer = CSGetFriendRankReq.CURRVERSION;
        }

        /* check cutversion */
        if (CSGetFriendRankReq.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.RankType */
        {
            ret = srcBuf.readInt32(ref this.RankType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.PageIndex */
        {
            ret = srcBuf.readInt32(ref this.PageIndex);
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
        if (0 == cutVer || CSGetFriendRankReq.CURRVERSION < cutVer)
        {
            cutVer = CSGetFriendRankReq.CURRVERSION;
        }

        /* check cutversion */
        if (CSGetFriendRankReq.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.RankType */
        {
            ret = srcBuf.readInt32(ref this.RankType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.PageIndex */
        {
            ret = srcBuf.readInt32(ref this.PageIndex);
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

        /* visualize member: this.RankType */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[RankType]", "{0:d}", this.RankType);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.PageIndex */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[PageIndex]", "{0:d}", this.PageIndex);
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


/* 排行数据 */
public class RankData : tsf4g_csharp_interface
{
    /* public members */
    public UInt64 UID; // 好友唯一ID
    public UInt32 Score; // 分数

    /* construct methods */
    public RankData()
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
        if (0 == cutVer || RankData.CURRVERSION < cutVer)
        {
            cutVer = RankData.CURRVERSION;
        }

        /* check cutversion */
        if (RankData.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.UID */
        {
            ret = destBuf.writeUInt64(this.UID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.Score */
        {
            ret = destBuf.writeUInt32(this.Score);
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
        if (0 == cutVer || RankData.CURRVERSION < cutVer)
        {
            cutVer = RankData.CURRVERSION;
        }

        /* check cutversion */
        if (RankData.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.UID */
        {
            ret = srcBuf.readUInt64(ref this.UID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.Score */
        {
            ret = srcBuf.readUInt32(ref this.Score);
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
        if (0 == cutVer || RankData.CURRVERSION < cutVer)
        {
            cutVer = RankData.CURRVERSION;
        }

        /* check cutversion */
        if (RankData.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.UID */
        {
            ret = srcBuf.readUInt64(ref this.UID);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.Score */
        {
            ret = srcBuf.readUInt32(ref this.Score);
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

        /* visualize member: this.UID */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[UID]", "{0:d}", this.UID);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.Score */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[Score]", "{0:d}", this.Score);
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


/* 页面数据 */
public class RankPage : tsf4g_csharp_interface
{
    /* public members */
    public Int16 DataNum; // 页面行数，不是最后一面时，值会填DataNumOnePage
    public RankData[] RankData; // 每页排行榜数据

    /* construct methods */
    public RankPage()
    {
        RankData = new RankData[32];
        for(int RankData_i = 0; RankData_i < 32; RankData_i++)
        {
            RankData[RankData_i] = new RankData();
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
        if (0 == cutVer || RankPage.CURRVERSION < cutVer)
        {
            cutVer = RankPage.CURRVERSION;
        }

        /* check cutversion */
        if (RankPage.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.DataNum */
        {
            ret = destBuf.writeInt16(this.DataNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.RankData */
        {
            if (0 > this.DataNum)
            {
                return TdrError.ErrorType.TDR_ERR_MINUS_REFER_VALUE;
            }
            if (32 < this.DataNum)
            {
                return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
            }
            for (Int32 RankData_i = 0; RankData_i < this.DataNum; RankData_i++)
            {
                ret = this.RankData[RankData_i].pack(ref destBuf, cutVer);
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
        if (0 == cutVer || RankPage.CURRVERSION < cutVer)
        {
            cutVer = RankPage.CURRVERSION;
        }

        /* check cutversion */
        if (RankPage.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.DataNum */
        {
            ret = srcBuf.readInt16(ref this.DataNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.RankData */
        {
            if (0 > this.DataNum)
            {
                return TdrError.ErrorType.TDR_ERR_MINUS_REFER_VALUE;
            }
            if (32 < this.DataNum)
            {
                return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
            }
            for (Int32 RankData_i = 0; RankData_i < this.DataNum; RankData_i++)
            {
                ret = this.RankData[RankData_i].unpack(ref srcBuf, cutVer);
                if (TdrError.ErrorType.TDR_NO_ERROR != ret)
                {
                    return ret;
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
        if (0 == cutVer || RankPage.CURRVERSION < cutVer)
        {
            cutVer = RankPage.CURRVERSION;
        }

        /* check cutversion */
        if (RankPage.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.DataNum */
        {
            ret = srcBuf.readInt16(ref this.DataNum);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.RankData */
        {
            for (Int32 RankData_i = 0; RankData_i < 32; RankData_i++)
            {
                ret = this.RankData[RankData_i].load(ref srcBuf, cutVer);
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

        /* visualize member: this.DataNum */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[DataNum]", "{0:d}", this.DataNum);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.RankData */
        if (0 > this.DataNum)
        {
            return TdrError.ErrorType.TDR_ERR_MINUS_REFER_VALUE;
        }
        if (32 < this.DataNum)
        {
            return TdrError.ErrorType.TDR_ERR_REFER_SURPASS_COUNT;
        }
        for (Int32 RankData_i = 0; RankData_i < this.DataNum; RankData_i++)
        {
            if (null == this.RankData[RankData_i])
            {
                continue;
            }

            ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[RankData]", RankData_i, true);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }

            /* visualize children of this.RankData[RankData_i] */
            if (0 > indent)
            {
                ret = this.RankData[RankData_i].visualize(ref destBuf, indent, separator);
            } else
            {
                ret = this.RankData[RankData_i].visualize(ref destBuf, indent + 1, separator);
            }
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


public class CSGetFriendRankRes : tsf4g_csharp_interface
{
    /* public members */
    public Int32 RankType; // 排行榜类型
    public Int32 SelfRanking; // 自己在好友里的排名
    public Int32 DataNumOnePage; // 一页有多少数据，固定值
    public Int32 PageIndex; // 页序，从0开始，根据客户端请求确定
    public RankPage RankPage; // 排行页好友信息

    /* construct methods */
    public CSGetFriendRankRes()
    {
        RankPage = new RankPage();
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
        if (0 == cutVer || CSGetFriendRankRes.CURRVERSION < cutVer)
        {
            cutVer = CSGetFriendRankRes.CURRVERSION;
        }

        /* check cutversion */
        if (CSGetFriendRankRes.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* pack member: this.RankType */
        {
            ret = destBuf.writeInt32(this.RankType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.SelfRanking */
        {
            ret = destBuf.writeInt32(this.SelfRanking);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.DataNumOnePage */
        {
            ret = destBuf.writeInt32(this.DataNumOnePage);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.PageIndex */
        {
            ret = destBuf.writeInt32(this.PageIndex);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* pack member: this.RankPage */
        {
            ret = this.RankPage.pack(ref destBuf, cutVer);
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
        if (0 == cutVer || CSGetFriendRankRes.CURRVERSION < cutVer)
        {
            cutVer = CSGetFriendRankRes.CURRVERSION;
        }

        /* check cutversion */
        if (CSGetFriendRankRes.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* unpack member: this.RankType */
        {
            ret = srcBuf.readInt32(ref this.RankType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.SelfRanking */
        {
            ret = srcBuf.readInt32(ref this.SelfRanking);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.DataNumOnePage */
        {
            ret = srcBuf.readInt32(ref this.DataNumOnePage);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.PageIndex */
        {
            ret = srcBuf.readInt32(ref this.PageIndex);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* unpack member: this.RankPage */
        {
            ret = this.RankPage.unpack(ref srcBuf, cutVer);
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
        if (0 == cutVer || CSGetFriendRankRes.CURRVERSION < cutVer)
        {
            cutVer = CSGetFriendRankRes.CURRVERSION;
        }

        /* check cutversion */
        if (CSGetFriendRankRes.BASEVERSION > cutVer)
        {
            return TdrError.ErrorType.TDR_ERR_CUTVER_TOO_SMALL;
        }

        /* load member: this.RankType */
        {
            ret = srcBuf.readInt32(ref this.RankType);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.SelfRanking */
        {
            ret = srcBuf.readInt32(ref this.SelfRanking);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.DataNumOnePage */
        {
            ret = srcBuf.readInt32(ref this.DataNumOnePage);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.PageIndex */
        {
            ret = srcBuf.readInt32(ref this.PageIndex);
            if (TdrError.ErrorType.TDR_NO_ERROR != ret)
            {
                return ret;
            }
        }

        /* load member: this.RankPage */
        {
            ret = this.RankPage.load(ref srcBuf, cutVer);
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

        /* visualize member: this.RankType */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[RankType]", "{0:d}", this.RankType);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.SelfRanking */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[SelfRanking]", "{0:d}", this.SelfRanking);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.DataNumOnePage */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[DataNumOnePage]", "{0:d}", this.DataNumOnePage);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.PageIndex */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[PageIndex]", "{0:d}", this.PageIndex);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize member: this.RankPage */
        ret = TdrBufUtil.printVariable(ref destBuf, indent, separator, "[RankPage]", true);
        if (TdrError.ErrorType.TDR_NO_ERROR != ret)
        {
            return ret;
        }

        /* visualize children of this.RankPage */
        if (0 > indent)
        {
            ret = this.RankPage.visualize(ref destBuf, indent, separator);
        } else
        {
            ret = this.RankPage.visualize(ref destBuf, indent + 1, separator);
        }
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
