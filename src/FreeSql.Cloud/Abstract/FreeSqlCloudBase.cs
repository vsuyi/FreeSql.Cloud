﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FreeSql.Cloud.Abstract
{
    public abstract class FreeSqlCloudBase
    {
        internal abstract string GetDBKey();
        public abstract IFreeSql Use(DBKeyString dbkey);
        public abstract IFreeSql Change(DBKeyString dbkey);
    }

    public class DBKeyString
    {
        string _dbkey;
        public override string ToString() => _dbkey;

        public static implicit operator DBKeyString(string dbkey) => string.IsNullOrWhiteSpace(dbkey) ? null : new DBKeyString { _dbkey = dbkey };
        public static implicit operator string(DBKeyString dbkey) => dbkey?.ToString();
    }

    public class AsyncLocalAccessor<T>
    {
		Func<T> _defaultValue;
        public AsyncLocalAccessor(Func<T> defaultValue)
        {
            _defaultValue = defaultValue;
		}
		public T Value
        {
            get
            {
                if (_asyncLocal.Value != null) return _asyncLocal.Value.Value;
                if (_defaultValue != null) return _defaultValue();
                return default;
            }
            set
            {
                if (_asyncLocal.Value == null) _asyncLocal.Value = new ValueHolder();
                _asyncLocal.Value.Value = value;
			}

		}

        class ValueHolder
        {
            public T Value { get; set; }
        }
#if net40
        ThreadLocal<ValueHolder> _asyncLocal = new ThreadLocal<ValueHolder>();
#else
        AsyncLocal<ValueHolder> _asyncLocal = new AsyncLocal<ValueHolder>();
#endif
    }
}
