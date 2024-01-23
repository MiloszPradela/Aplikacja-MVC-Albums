using AlbumApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AlbumApp.MVC.Models;
using System.Collections.Generic; // Add this namespace for List<T>

namespace AlbumApp.MVC.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumService _albumService;
        private readonly RatingService _ratingService;

        public AlbumController(AlbumService albumService, RatingService ratingService)
        {
            _albumService = albumService;
            _ratingService = ratingService;
        }

        public IActionResult Index(int page = 1, int size = 5)
        {
            List<AlbumViewModel> albums = GetAlbums();

            int totalItems = albums.Count;

            var pagingList = PagingList<AlbumViewModel>.Create((p, s) => albums.Skip((p - 1) * s).Take(s), page, size, totalItems);

            return View(pagingList);
        }

        public IActionResult ShowSongs(int id)
        {
            var albumViewModel = _albumService.GetAlbumById(id);

            if (albumViewModel == null)
            {
                return RedirectToAction("Index");
            }

            return View(albumViewModel);
        }

        [HttpPost]
        public IActionResult RateAlbum(int albumId, int rating)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Handle unauthenticated user
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Convert the user ID to Guid

            // Check if the user has already rated the album
            if (_ratingService.HasUserRatedAlbum(albumId, userId))
            {
                // Handle error or display a message
                return RedirectToAction("Index");
            }

            // Add the new rating
            _ratingService.AddRating(albumId, userId, rating);

            return RedirectToAction("Index");
        }

        private List<AlbumViewModel> GetAlbums()
        {
            // Implement your logic to get albums from the service or database
            return _albumService.GetAllAlbums();
        }
    }
}
