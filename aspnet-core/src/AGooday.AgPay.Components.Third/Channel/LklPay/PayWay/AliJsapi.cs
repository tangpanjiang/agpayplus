﻿using AGooday.AgPay.Application.DataTransfer;
using AGooday.AgPay.Application.Interfaces;
using AGooday.AgPay.Common.Exceptions;
using AGooday.AgPay.Common.Utils;
using AGooday.AgPay.Components.Third.Models;
using AGooday.AgPay.Components.Third.RQRS;
using AGooday.AgPay.Components.Third.RQRS.Msg;
using AGooday.AgPay.Components.Third.RQRS.PayOrder;
using AGooday.AgPay.Components.Third.RQRS.PayOrder.PayWay;
using AGooday.AgPay.Components.Third.Services;
using AGooday.AgPay.Components.Third.Utils;
using Newtonsoft.Json.Linq;

namespace AGooday.AgPay.Components.Third.Channel.LklPay.PayWay
{
    /// <summary>
    /// 拉卡拉 支付宝 jsapi
    /// </summary>
    public class AliJsapi : LklPayPaymentService
    {
        public AliJsapi(ILogger<AliJsapi> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
        }

        public override async Task<AbstractRS> PayAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            string logPrefix = "【拉卡拉(alipayJs)jsapi支付】";
            AliJsapiOrderRQ bizRQ = (AliJsapiOrderRQ)rq;
            JObject reqParams = new JObject();
            AliJsapiOrderRS res = ApiResBuilder.BuildSuccess<AliJsapiOrderRS>();
            ChannelRetMsg channelRetMsg = new ChannelRetMsg();
            res.ChannelRetMsg = channelRetMsg;

            // 请求参数赋值
            UnifiedParamsSet(reqParams, payOrder, GetNotifyUrl(), GetReturnUrl());

            //拉卡拉扫一扫支付， 需要传入buyerUserId参数
            /*用户号（微信openid / 支付宝userid / 银联userid）
            payType == "WECHAT"或"ALIPAY"时必传*/
            reqParams.Add("acc_busi_fields", new JObject { { "user_id", bizRQ.GetChannelUserId() } });

            // 发送请求
            JObject resJSON = await PackageParamAndReqAsync("/api/v3/labs/trans/preorder", reqParams, logPrefix, mchAppConfigContext);
            //请求 & 响应成功， 判断业务逻辑
            string code = resJSON?.GetValue("code").ToString(); //请求响应码
            string msg = resJSON?.GetValue("msg").ToString(); //响应信息
            try
            {
                if ("BBS00000".Equals(code))
                {
                    var respData = resJSON.GetValue("resp_data")?.ToObject<JObject>();
                    respData.TryGetString("merchant_no", out string merchantNo);//全局流水号
                    respData.TryGetString("trade_no", out string tradeNo);//全局流水号
                    var accRespFields = respData.GetValue("acc_resp_fields")?.ToObject<JObject>();
                    var prepayId = accRespFields?.GetValue("prepayId").ToString();
                    channelRetMsg.ChannelMchNo = merchantNo;
                    res.AlipayTradeNo = prepayId;
                    channelRetMsg.ChannelOrderId = tradeNo;
                    channelRetMsg.ChannelState = ChannelState.WAITING;
                }
                else if ("BBS11112".Equals(code) || "BBS11105".Equals(code) || "BBS10000".Equals(code))
                {
                    channelRetMsg.ChannelState = ChannelState.WAITING;
                    channelRetMsg.IsNeedQuery = true; // 开启轮询查单
                }
                else
                {
                    channelRetMsg.ChannelState = ChannelState.CONFIRM_FAIL;
                    channelRetMsg.ChannelErrCode = code;
                    channelRetMsg.ChannelErrMsg = msg;
                }
            }
            catch (Exception)
            {
                channelRetMsg.ChannelErrCode = code;
                channelRetMsg.ChannelErrMsg = msg;
            }
            return res;
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ rq, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            AliJsapiOrderRQ bizRQ = (AliJsapiOrderRQ)rq;
            if (string.IsNullOrWhiteSpace(bizRQ.GetChannelUserId()))
            {
                throw new BizException("[buyerUserId]不可为空");
            }

            return Task.FromResult<string>(null);
        }
    }
}
