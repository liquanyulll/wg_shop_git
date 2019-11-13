using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace wg_utils
{
    public class RedisMQHelper
    {
        private static object _locker = new Object();
        private static ISubscriber _subscriber = null;

        public static ISubscriber Subscriber
        {
            get
            {
                if (_subscriber == null)
                {
                    lock (_locker)
                    {
                        _subscriber = RedisCachedHelper.Instance.GetSubscriber();
                    }
                }
                return _subscriber;
            }
        }
    }
}
