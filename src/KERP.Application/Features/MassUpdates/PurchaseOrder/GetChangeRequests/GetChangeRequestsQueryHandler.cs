using KERP.Application.Common.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.GetChangeRequests;

public sealed class GetChangeRequestsQueryHandler
: IQueryHandler<GetChangeRequestsQuery, List<ChangeRequestDto>>
{
    private readonly IAppDbContext _context;

    public GetChangeRequestsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChangeRequestDto>> Handle(GetChangeRequestsQuery query, CancellationToken cancellationToken)
    {
        // Reszta metody pozostaje bez najmniejszych zmian
        var requests = await _context.PurchaseOrderReceiptDateChangeRequests
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new ChangeRequestDto
            {
                Id = r.Id,
                PurchaseOrder = r.PurchaseOrder,
                LineNumber = r.LineNumber,
                NewReceiptDate = r.NewReceiptDate,
                DateType = r.DateType.ToString(),
                UserId = r.UserId,
                CreatedAtUtc = r.CreatedAtUtc,
                IsProcessed = r.IsProcessed
            })
            .ToListAsync(cancellationToken);

        return requests;
    }
}
