﻿using AGooday.AgPay.Application.DataTransfer;
using AutoMapper;

namespace AGooday.AgPay.Components.Third.RQRS.Division
{
    /// <summary>
    /// 绑定账户 响应参数
    /// </summary>
    public class DivisionReceiverBindRS : AbstractRS
    {

        /// <summary>
        /// 分账接收者ID
        /// </summary>
        public long ReceiverId { get; set; }

        /// <summary>
        /// 接收者账号别名
        /// </summary>
        public string ReceiverAlias { get; set; }

        /// <summary>
        /// 组ID（便于商户接口使用）
        /// </summary>
        public long ReceiverGroupId { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MchNo { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 支付接口代码
        /// </summary>
        public string IfCode { get; set; }

        /// <summary>
        /// 分账接收账号类型: 0-个人(对私) 1-商户(对公)
        /// </summary>
        public byte AccType { get; set; }

        /// <summary>
        /// 分账接收账号
        /// </summary>
        public string AccNo { get; set; }

        /// <summary>
        /// 分账接收账号名称
        /// </summary>
        public string AccName { get; set; }

        /// <summary>
        /// 分账关系类型（参考微信）， 如： SERVICE_PROVIDER 服务商等
        /// </summary>
        public string RelationType { get; set; }

        /// <summary>
        /// 当选择自定义时，需要录入该字段。 否则为对应的名称
        /// </summary>
        public string RelationTypeName { get; set; }


        /// <summary>
        /// 渠道特殊信息
        /// </summary>
        public string ChannelExtInfo { get; set; }

        /// <summary>
        /// 绑定成功时间
        /// </summary>
        public long? BindSuccessTime { get; set; }

        /// <summary>
        /// 分账比例
        /// </summary>
        public decimal divisionProfit { get; set; }

        /// <summary>
        /// 分账状态 1-绑定成功, 0-绑定异常
        /// </summary>
        public byte BindState { get; set; }

        /// <summary>
        /// 支付渠道错误码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 支付渠道错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        public static DivisionReceiverBindRS BuildByRecord(MchDivisionReceiverDto record)
        {

            if (record == null)
            {
                return null;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<MchDivisionReceiverDto, DivisionReceiverBindRS>()
                .ForMember(dest => dest.BindSuccessTime, opt => opt.Ignore())
            );
            var mapper = config.CreateMapper();
            var result = mapper.Map<MchDivisionReceiverDto, DivisionReceiverBindRS>(record);
            result.BindSuccessTime = record.BindSuccessTime != null ? new DateTimeOffset(record.BindSuccessTime.Value).ToUnixTimeSeconds() : null;

            return result;
        }
    }
}
