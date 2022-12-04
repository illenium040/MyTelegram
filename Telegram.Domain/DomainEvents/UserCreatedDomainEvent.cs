﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Domain.Abstractions;
using Telegram.Domain.ValueObjects;

namespace Telegram.Domain.DomainEvents
{
    public record UserCreatedDomainEvent (Guid Id, Login Login) : IDomainEvent;
}
