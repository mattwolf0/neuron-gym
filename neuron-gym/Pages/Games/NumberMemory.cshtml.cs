using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using neuron_gym.Data;
using neuron_gym.Models;

namespace neuron_gym.Pages.Games
{
    public class NumberMemoryModel : PageModel
    {
        private readonly AppDbContext _db;

        public NumberMemoryModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public int ScoreValue { get; set; }

        [BindProperty]
        public string PlayerName { get; set; } = string.Empty;

        public int BestScore { get; set; }

        public void OnGet()
        {
            BestScore = GetBestScore();
        }

        public IActionResult OnPostSaveScore()
        {
            BestScore = GetBestScore();

            if (ScoreValue <= 0)
            {
                return RedirectToPage();
            }

            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                PlayerName = "Anonymous";
            }

            var score = new Score
            {
                PlayerName = PlayerName,
                Game = "Number Memory",
                Value = ScoreValue,
                CreatedAt = DateTime.UtcNow
            };

            _db.Scores.Add(score);
            _db.SaveChanges();

            return RedirectToPage("/Scoreboard", new { SelectedGame = "Number Memory" });
        }

        int GetBestScore()
        {
            return _db.Scores
                .Where(s => s.Game == "Number Memory")
                .OrderByDescending(s => s.Value)
                .Select(s => s.Value)
                .FirstOrDefault();
        }
    }
}
