namespace DattingApp.API.Helpers
{
     public class PaginationHeader
     {
          public  int TotalItems{get;set;}
          public  int TotalPages{get;set;}
          public  int CurrentPage{get;set;}
          public  int ItemsPerPage{get;set;}
          public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
          {
               ItemsPerPage = itemsPerPage;
               CurrentPage = currentPage;
               TotalItems = totalItems;
               TotalPages = totalPages;

          }
     }
}