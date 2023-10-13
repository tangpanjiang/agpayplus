﻿using AGooday.AgPay.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace AGooday.AgPay.Payment.Api.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： WX_LITE
    /// </summary>
    public class WxLiteOrderRQ : UnifiedOrderRQ
    {
        /// <summary>
        /// 微信openid
        /// </summary>
        [Required(ErrorMessage = "openid不能为空")]
        public string Openid { get; set; }

        /// <summary>
        /// 标志是否为 subMchAppId的对应 openId， 0-否， 1-是， 默认否
        /// </summary>
        public byte IsSubOpenId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WxLiteOrderRQ()
        {
            this.WayCode = CS.PAY_WAY_CODE.WX_LITE;
        }

        public override string GetChannelUserId()
        {
            return this.Openid;
        }
    }
}