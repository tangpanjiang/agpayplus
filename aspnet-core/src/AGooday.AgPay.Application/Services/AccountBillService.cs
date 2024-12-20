﻿using AGooday.AgPay.Application.DataTransfer;
using AGooday.AgPay.Application.Interfaces;
using AGooday.AgPay.Common.Enumerator;
using AGooday.AgPay.Common.Models;
using AGooday.AgPay.Common.Utils;
using AGooday.AgPay.Domain.Core.Bus;
using AGooday.AgPay.Domain.Interfaces;
using AGooday.AgPay.Domain.Models;
using AutoMapper;

namespace AGooday.AgPay.Application.Services
{
    /// <summary>
    /// 账户帐单表 服务实现类
    /// </summary>
    public class AccountBillService : AgPayService<AccountBillDto, AccountBill>, IAccountBillService
    {
        // 注意这里是要IoC依赖注入的，还没有实现
        private readonly IPayOrderProfitRepository _payOrderProfitRepository;

        public AccountBillService(IMapper mapper, IMediatorHandler bus,
            IAccountBillRepository accountBillRepository,
            IPayOrderProfitRepository payOrderProfitRepository)
            : base(mapper, bus, accountBillRepository)
        {
            _payOrderProfitRepository = payOrderProfitRepository;
        }

        public async Task GenAccountBillAsync(string payOrderId)
        {
            var payOrderProfits = _payOrderProfitRepository.GetByPayOrderIdAsNoTracking(payOrderId).OrderBy(o => o.Id);
            var isSaveChanges = false;
            foreach (var payOrderProfit in payOrderProfits)
            {
                if (payOrderProfit.ProfitAmount > 0)
                {
                    var accountBill = new AccountBill();
                    accountBill.BillId = SeqUtil.GenBillId();
                    accountBill.InfoId = payOrderProfit.InfoId;
                    accountBill.InfoName = payOrderProfit.InfoName;
                    accountBill.InfoType = payOrderProfit.InfoType;
                    accountBill.BeforeBalance = 0;
                    accountBill.ChangeAmount = payOrderProfit.ProfitAmount;
                    accountBill.AfterBalance = payOrderProfit.ProfitAmount;
                    accountBill.BizType = (byte)AccountBillBizType.ORDER_PROFIT_CALCULATE;
                    accountBill.AccountType = (byte)AccountBillAccountType.IN_TRANSIT_ACCOUNT;
                    accountBill.RelaBizOrderType = (byte)AccountBillRelaBizOrderType.PAY_ORDER;
                    accountBill.RelaBizOrderId = payOrderProfit.PayOrderId;
                    accountBill.CreatedAt = DateTime.Now;
                    accountBill.UpdatedAt = DateTime.Now;
                    await _agPayRepository.AddAsync(accountBill);
                    isSaveChanges = true;
                }
            }
            if (isSaveChanges)
            {
                await _agPayRepository.SaveChangesAsync();
            }
        }

        public override async Task<bool> AddAsync(AccountBillDto dto)
        {
            var entity = _mapper.Map<AccountBill>(dto);
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            await _agPayRepository.AddAsync(entity);
            var result = await _agPayRepository.SaveChangesAsync() > 0;
            return result;
        }

        public override async Task<bool> UpdateAsync(AccountBillDto dto)
        {
            var entity = _mapper.Map<AccountBill>(dto);
            entity.UpdatedAt = DateTime.Now;
            _agPayRepository.Update(entity);
            return await _agPayRepository.SaveChangesAsync() > 0;
        }

        public Task<PaginatedList<AccountBillDto>> GetPaginatedDataAsync(AccountBillQueryDto dto)
        {
            var query = _agPayRepository.GetAllAsNoTracking()
                .Where(w => (dto.Id.Equals(null) || w.Id.Equals(dto.Id))
                && (string.IsNullOrWhiteSpace(dto.BillId) || w.BillId.Equals(dto.BillId))
                && (string.IsNullOrWhiteSpace(dto.InfoId) || w.InfoId.Equals(dto.InfoId))
                && (string.IsNullOrWhiteSpace(dto.InfoType) || w.InfoType.Equals(dto.InfoType))
                && (dto.BizType.Equals(null) || w.BizType.Equals(dto.BizType))
                && (dto.AccountType.Equals(null) || w.AccountType.Equals(dto.AccountType))
                && (string.IsNullOrWhiteSpace(dto.RelaBizOrderId) || w.RelaBizOrderId.Equals(dto.RelaBizOrderId))
                && (dto.CreatedStart.Equals(null) || w.CreatedAt >= dto.CreatedStart)
                && (dto.CreatedEnd.Equals(null) || w.CreatedAt <= dto.CreatedEnd))
                .OrderByDescending(o => o.CreatedAt);
            var records = PaginatedList<AccountBill>.CreateAsync<AccountBillDto>(query, _mapper, dto.PageNumber, dto.PageSize);
            return records;
        }
    }
}
