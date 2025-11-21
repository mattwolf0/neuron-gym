using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using neuron_gym.Data;
using neuron_gym.Models;

namespace neuron_gym.Pages
{
    public class ScoreboardModel : PageModel
    {
        private readonly AppDbContext _db;

        public ScoreboardModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<Score> Scores { get; set; } = new List<Score>();

        [BindProperty(SupportsGet = true)]
        public string? SelectedGame { get; set; }

        public List<SelectListItem> GameOptions { get; set; } = new List<SelectListItem>();

        public async Task OnGetAsync()
        {
            GameOptions = BuildGameOptions();
            await LoadScoresAsync();
        }

        async Task LoadScoresAsync()
        {
            IQueryable<Score> query = _db.Scores;

            if (!string.IsNullOrEmpty(SelectedGame) && SelectedGame != "all")
            {
                query = query.Where(s => s.Game == SelectedGame);
            }

            Scores = await query
                .OrderByDescending(s => s.Value)
                .ThenByDescending(s => s.CreatedAt)
                .Take(50)
                .ToListAsync();
        }

        List<SelectListItem> BuildGameOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem("All games", "all", SelectedGame == null || SelectedGame == "all"),
                new SelectListItem("Chimp Test", "Chimp Test", SelectedGame == "Chimp Test"),
                new SelectListItem("Simon Says", "Simon Says", SelectedGame == "Simon Says"),
                new SelectListItem("Number Memory", "Number Memory", SelectedGame == "Number Memory"),
                new SelectListItem("Arithmetic", "Arithmetic", SelectedGame == "Arithmetic")
            };
        }
    }
}
