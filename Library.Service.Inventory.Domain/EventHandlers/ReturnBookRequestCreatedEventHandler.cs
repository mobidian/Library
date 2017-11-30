﻿using Library.Domain.Core;
using Library.Domain.Core.DataAccessor;
using Library.Domain.Core.Messaging;
using Library.Service.Inventory.Domain.DataAccessors;
using Library.Service.Inventory.Domain.Events;
using System;

namespace Library.Service.Inventory.Domain.EventHandlers
{
    public class ReturnBookRequestCreatedEventHandler : BaseInventoryEventHandler<ReturnBookRequestCreatedEvent>
    {
        public ReturnBookRequestCreatedEventHandler(IInventoryReportDataAccessor reportDataAccessor, ICommandTracker commandTracker, ILogger logger, IDomainRepository domainRepository, IEventPublisher eventPublisher) : base(reportDataAccessor, commandTracker, logger, domainRepository, eventPublisher)
        {
        }

        public override void HandleCore(ReturnBookRequestCreatedEvent evt)
        {
            try
            {
                _reportDataAccessor.UpdateBookInventoryStatus(evt.BookInventoryId, BookInventoryStatus.InStore, $"Return by {evt.Name.FirstName} {evt.Name.LastName} at {evt.ReturnDate.ToString("yyyy-MM-dd HH:mm:ss")}", evt.OccurredOn);
                _reportDataAccessor.Commit();

                _eventPublisher.Publish(new ReturnBookRequestSucceedEvent
                {
                    BookInventoryId = evt.BookInventoryId,
                    CustomerId = evt.AggregateId,
                    CommandUniqueId = evt.CommandUniqueId
                });

                evt.Result("RETURNBOOKREQUEST_CREATED");
            }
            catch (Exception ex)
            {
                //send event ReturnBookRequestFailedEvent

                evt.Result("SERVER_ERROR", ex.ToString());
            }
        }
    }
}