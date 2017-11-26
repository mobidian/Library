﻿using Library.Domain.Core;
using Library.Domain.Core.DataAccessor;
using Library.Domain.Core.Messaging;
using Library.Infrastructure.Core;
using Library.Service.Inventory.Domain.DataAccessors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Inventory.Domain.EventHandlers
{
    public abstract class BaseInventoryEventHandler<T> : IEventHandler<T> where T : DomainEvent
    {
        protected IInventoryReportDataAccessor _reportDataAccessor = null;
        protected IDomainRepository _domainRepository = null;
        protected ICommandTracker _commandTracker = null;
        protected IEventPublisher _eventPublisher = null;
        protected ILogger _logger = null;

        public BaseInventoryEventHandler(IInventoryReportDataAccessor reportDataAccessor, ICommandTracker commandTracker, ILogger logger, IDomainRepository domainRepository, IEventPublisher eventPublisher)
        {
            _reportDataAccessor = reportDataAccessor;
            _commandTracker = commandTracker;
            _logger = logger;
            _domainRepository = domainRepository;
            _eventPublisher = eventPublisher;
        }

        public abstract void Handle(T evt);

        public virtual Task HandleAsync(T evt)
        {
            return Task.Factory.StartNew(() =>
            {
                Handle(evt);
            });
        }
    }
}