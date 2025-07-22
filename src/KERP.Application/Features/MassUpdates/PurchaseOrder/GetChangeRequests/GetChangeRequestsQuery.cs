using KERP.Application.Common.Abstractions;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.GetChangeRequests;

// Definiujemy, że to zapytanie będzie zwracać listę naszych DTO
public sealed record GetChangeRequestsQuery() : IQuery<List<ChangeRequestDto>>;
