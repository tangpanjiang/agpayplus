﻿using AGooday.AgPay.Application.DataTransfer;
using AGooday.AgPay.Common.Models;
using Newtonsoft.Json.Linq;

namespace AGooday.AgPay.Application.Interfaces
{
    public interface IPayOrderService : IAgPayService<PayOrderDto>
    {
        Task<bool> IsExistOrderUseIfCodeAsync(string ifCode);
        Task<bool> IsExistOrderUseWayCodeAsync(string wayCode);
        Task<bool> IsExistOrderByMchOrderNoAsync(string mchNo, string mchOrderNo);
        Task<PayOrderDto> QueryMchOrderAsync(string mchNo, string payOrderId, string mchOrderNo);
        Task<PaginatedList<PayOrderDto>> GetPaginatedDataAsync(PayOrderQueryDto dto);
        Task<JObject> StatisticsAsync(PayOrderQueryDto dto);
        /// <summary>
        /// 更新订单状态 【订单生成】 --》 【支付中】
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <param name="payOrder"></param>
        /// <returns></returns>
        Task<bool> UpdateInit2IngAsync(string payOrderId, PayOrderDto payOrder);
        /// <summary>
        /// 更新订单状态 【支付中】 --》 【支付成功】
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <param name="channelOrderNo"></param>
        /// <param name="channelUserId"></param>
        /// <returns></returns>
        Task<bool> UpdateIng2SuccessAsync(string payOrderId, string channelMchNo, string channelIsvNo, string channelOrderNo, string channelUserId, string platformOrderId, string platformMchOrderId);
        /// <summary>
        /// 更新订单状态  【支付中】 --》 【订单关闭】
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <returns></returns>
        Task<bool> UpdateIng2CloseAsync(string payOrderId);
        /// <summary>
        /// 更新订单状态 【支付中】 --》 【支付失败】
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <param name="channelOrderNo"></param>
        /// <param name="channelUserId"></param>
        /// <param name="channelErrCode"></param>
        /// <param name="channelErrMsg"></param>
        /// <returns></returns>
        Task<bool> UpdateIng2FailAsync(string payOrderId, string channelMchNo, string channelIsvNo, string channelOrderNo, string channelUserId, string platformOrderNo, string platformMchOrderNo, string channelErrCode, string channelErrMsg);
        /// <summary>
        /// 更新订单状态 【支付中】 --》 【支付成功/支付失败】
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <param name="updateState"></param>
        /// <param name="channelMchNo"></param>
        /// <param name="channelIsvNo"></param>
        /// <param name="channelOrderNo"></param>
        /// <param name="channelUserId"></param>
        /// <param name="platformOrderNo"></param>
        /// <param name="platformMchOrderNo"></param>
        /// <param name="channelErrCode"></param>
        /// <param name="channelErrMsg"></param>
        /// <returns></returns>
        Task<bool> UpdateIng2SuccessOrFail(string payOrderId, byte updateState, string channelMchNo, string channelIsvNo, string channelOrderNo, string channelUserId, string platformOrderNo, string platformMchOrderNo, string channelErrCode, string channelErrMsg);
        /// <summary>
        /// 更新订单为 超时状态
        /// </summary>
        /// <returns></returns>
        Task<int> UpdateOrderExpiredAsync();
        /// <summary>
        /// 更新订单 通知状态 --> 已发送
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<bool> UpdateNotifySentAsync(string orderId);
        /// <summary>
        /// 更新订单表分账状态为： 等待分账任务处理
        /// </summary>
        /// <param name="payOrder"></param>
        /// <returns></returns>
        Task<bool> UpdateDivisionStateAsync(PayOrderDto payOrder);
        /// <summary>
        /// 计算支付订单商家入账金额
        /// 商家订单入账金额 （支付金额 - 手续费 - 退款金额 - 总分账金额）</summary>
        /// <param name="dbPayOrder"></param>
        /// <returns></returns>
        Task<long> CalMchIncomeAmountAsync(PayOrderDto payOrder);
        /// <summary>
        /// 服务商/代理商/商户统计
        /// </summary>
        /// <param name="mchNo"></param>
        /// <param name="agentNo"></param>
        /// <returns></returns>
        Task<JObject> MainPageIsvAndMchCountAsync(string mchNo, string agentNo);
        /// <summary>
        /// 今日/昨日交易统计
        /// </summary>
        /// <param name="mchNo"></param>
        /// <param name="agentNo"></param>
        /// <returns></returns>
        Task<JObject> MainPagePayDayCountAsync(string mchNo, string agentNo, DateTime? day);
        /// <summary>
        /// 近期交易统计
        /// </summary>
        /// <param name="mchNo"></param>
        /// <param name="agentNo"></param>
        /// <param name="recentDay"></param>
        /// <returns></returns>
        Task<JObject> MainPagePayTrendCountAsync(string mchNo, string agentNo, int recentDay);
        /// <summary>
        /// 首页支付统计
        /// </summary>
        /// <param name="mchNo"></param>
        /// <param name="agentNo"></param>
        /// <param name="createdStart"></param>
        /// <param name="createdEnd"></param>
        /// <returns></returns>
        Task<JObject> MainPagePayCountAsync(string mchNo, string agentNo, string createdStart, string createdEnd);
        /// <summary>
        /// 首页支付类型统计
        /// </summary>
        /// <param name="mchNo"></param>
        /// <param name="agentNo"></param>
        /// <param name="createdStart"></param>
        /// <param name="createdEnd"></param>
        /// <returns></returns>
        Task<IEnumerable<PayTypeCountDto>> MainPagePayTypeCountAsync(string mchNo, string agentNo, string createdStart, string createdEnd);
    }
}
