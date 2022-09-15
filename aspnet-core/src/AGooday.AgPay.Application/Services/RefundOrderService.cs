﻿using AGooday.AgPay.Application.Interfaces;
using AGooday.AgPay.Application.ViewModels;
using AGooday.AgPay.Domain.Commands.SysUsers;
using AGooday.AgPay.Domain.Core.Bus;
using AGooday.AgPay.Domain.Interfaces;
using AGooday.AgPay.Domain.Models;
using AGooday.AgPay.Infrastructure.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGooday.AgPay.Application.Services
{
    public class RefundOrderService : IRefundOrderService
    {
        // 注意这里是要IoC依赖注入的，还没有实现
        private readonly IRefundOrderRepository _refundOrderRepository;
        // 用来进行DTO
        private readonly IMapper _mapper;
        // 中介者 总线
        private readonly IMediatorHandler Bus;

        public RefundOrderService(IRefundOrderRepository refundOrderRepository, IMapper mapper, IMediatorHandler bus)
        {
            _refundOrderRepository = refundOrderRepository;
            _mapper = mapper;
            Bus = bus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Add(RefundOrderVM vm)
        {
            var m = _mapper.Map<RefundOrder>(vm);
            _refundOrderRepository.Add(m);
            _refundOrderRepository.SaveChanges();
        }

        public void Remove(string recordId)
        {
            _refundOrderRepository.Remove(recordId);
            _refundOrderRepository.SaveChanges();
        }

        public void Update(RefundOrderVM vm)
        {
            var m = _mapper.Map<RefundOrder>(vm);
            _refundOrderRepository.Update(m);
            _refundOrderRepository.SaveChanges();
        }

        public RefundOrderVM GetById(string recordId)
        {
            var entity = _refundOrderRepository.GetById(recordId);
            var vm = _mapper.Map<RefundOrderVM>(entity);
            return vm;
        }

        public IEnumerable<RefundOrderVM> GetAll()
        {
            var refundOrders = _refundOrderRepository.GetAll();
            return _mapper.Map<IEnumerable<RefundOrderVM>>(refundOrders);
        }
    }
}