using AlbumApp.MVC.Models; 

namespace AlbumApp.MVC.Models
{
    // AlbumListViewModel.cs

public class AlbumListViewModel
{
        public PaginatedList<AlbumViewModel> PaginatedAlbums { get; set; }
}

}
