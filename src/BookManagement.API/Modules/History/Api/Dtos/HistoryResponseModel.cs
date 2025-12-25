namespace BookManagement.API.Modules.History.Api.Dtos
{
    public sealed record HistoryResponseModel
 (
     Guid Id,
     Guid BookId,
     string Action,          
     string BookTitle,       
     string Authors,         
     string Description,     
     DateTime OccurredOn
 );
}
