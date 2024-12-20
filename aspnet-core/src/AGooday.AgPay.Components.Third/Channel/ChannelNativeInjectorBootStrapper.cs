﻿using System.Reflection;
using AGooday.AgPay.Components.Third.Models;
using AGooday.AgPay.Components.Third.Utils;

namespace AGooday.AgPay.Components.Third.Channel
{
    public class ChannelNativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region ChannelUserService
            ServiceRegister<IChannelUserService>(services);
            #endregion
            #region DivisionService
            ServiceRegister<IDivisionService>(services);
            #endregion
            #region DivisionRecordChannelNotifyService
            ServiceRegister<IDivisionRecordChannelNotifyService, AbstractDivisionRecordChannelNotifyService>(services);
            #endregion
            #region PaymentService
            ServiceRegister<IPaymentService, AbstractPaymentService>(services, (targetType) =>
            {
                PayWayUtil.PayWayServiceRegister(services, targetType);
                PayWayUtil.PayWayV3ServiceRegister(services, targetType);
            });
            #endregion
            #region RefundService
            ServiceRegister<IRefundService, AbstractRefundService>(services);
            #endregion
            #region ChannelNoticeService
            ServiceRegister<IChannelNoticeService, AbstractChannelNoticeService>(services);
            #endregion
            #region ChannelRefundNoticeService
            ServiceRegister<IChannelRefundNoticeService, AbstractChannelRefundNoticeService>(services);
            #endregion
            #region CloseService
            ServiceRegister<IPayOrderCloseService>(services);
            #endregion
            #region QueryService
            ServiceRegister<IPayOrderQueryService>(services);
            #endregion
            #region TransferService
            ServiceRegister<ITransferService>(services);
            #endregion
            #region TransferNoticeService
            ServiceRegister<ITransferNoticeService, AbstractTransferNoticeService>(services);
            #endregion

            // 初始化静态 ServiceResolver
            var serviceProvider = services.BuildServiceProvider();
            ServiceResolver.Initialize(serviceProvider);
        }

        private static void ServiceRegister<TService>(IServiceCollection services)
            where TService : class
        {
            ServiceRegister(services, typeof(TService), null, null);
            services.AddScoped<IChannelServiceFactory<TService>, ChannelServiceFactory<TService>>();
        }

        private static void ServiceRegister<TService, TAbstractService>(IServiceCollection services, Action<Type> action = null)
            where TService : class
            where TAbstractService : class, TService
        {
            ServiceRegister(services, typeof(TService), typeof(TAbstractService), action);
            services.AddScoped<IChannelServiceFactory<TService>, ChannelServiceFactory<TService>>();
        }

        private static void ServiceRegister(IServiceCollection services, Type serviceType, Type baseType, Action<Type> action)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 获取指定命名空间下的所有类
            Type[] targetTypes = assembly.GetTypes()
                .Where(type => ((baseType == null && type.BaseType != null) || (baseType != null && type.BaseType == baseType)) && serviceType.IsAssignableFrom(type))
                .ToArray();

            // 注册所有类
            foreach (Type implementationType in targetTypes)
            {
                string serviceKey;
                // 查找无参构造函数
                // ConstructorInfo constructorInfo = implementationType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                ConstructorInfo constructor = implementationType.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    var instance = Activator.CreateInstance(implementationType);
                    var getIfCodeMethod = implementationType.GetMethod("GetIfCode");
                    serviceKey = getIfCodeMethod?.Invoke(instance, null)?.ToString();
                }
                else
                {
                    // 获取静态属性
                    PropertyInfo ifCodePropertyInfo = implementationType.GetProperty("IfCode", BindingFlags.Public | BindingFlags.Static);
                    serviceKey = ifCodePropertyInfo?.GetValue(null)?.ToString();
                }
                if (!string.IsNullOrEmpty(serviceKey))
                {
                    services.AddKeyedScoped(serviceType, serviceKey, implementationType);
                    action?.Invoke(implementationType);
                }
            }
        }
    }

    public interface IChannelServiceFactory<T>
    {
        T GetService(object serviceKey);
    }

    public class ChannelServiceFactory<T> : IChannelServiceFactory<T>
    {
        private readonly IServiceProvider _serviceProvider;

        public ChannelServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService(object serviceKey)
        {
            try
            {
                return _serviceProvider.GetRequiredKeyedService<T>(serviceKey);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }
    }
}
