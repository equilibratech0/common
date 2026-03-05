using System;
using System.Collections.Generic;

namespace Shared.Domain.Entities;

public class ClientContext
{
    public Guid ClientId { get; }
    public string ClientName { get; }
    public IReadOnlyList<string> UserIds { get; }

    public ClientContext(Guid clientId, string clientName, IReadOnlyList<string> userIds)
    {
        ClientId = clientId;
        ClientName = clientName ?? throw new ArgumentNullException(nameof(clientName));
        UserIds = userIds ?? Array.Empty<string>();
    }
}
