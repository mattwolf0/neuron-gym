using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using neuron_gym.Data;
using neuron_gym.Models;

namespace neuron_gym.Pages.Games
{
    public class ArithmeticModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly Random _random = new Random();

        public ArithmeticModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public int Number1 { get; set; }

        [BindProperty]
        public int Number2 { get; set; }

        [BindProperty]
        public string Operation { get; set; } = "+";

        [BindProperty]
        public int? UserAnswer { get; set; }

        [BindProperty]
        public int CurrentScore { get; set; }

        [BindProperty]
        public string PlayerName { get; set; } = string.Empty;

        public int CorrectAnswer { get; set; }

        public int BestScore { get; set; }

        public string? ResultMessage { get; set; }

        public bool GameOver { get; set; }

        public void OnGet()
        {
            CurrentScore = 0;
            BestScore = GetBestScore();
            GameOver = false;
            GenerateQuestion();
        }


        public void OnPostAnswer()
        {
            BestScore = GetBestScore();
            CorrectAnswer = Calculate(Number1, Number2, Operation);

            if (!UserAnswer.HasValue)
            {
                ResultMessage = "Please enter an answer.";
                GameOver = false;
                return;
            }

            if (UserAnswer.Value == CorrectAnswer)
            {
                CurrentScore++;
                ResultMessage = $"Correct! Current score: {CurrentScore}.";
                GameOver = false;

                ModelState.Clear();
                GenerateQuestion();
            }
            else
            {
                ResultMessage = $"Incorrect. The correct answer was {CorrectAnswer}. Your final score: {CurrentScore}.";
                GameOver = true;
            }
        }

        public IActionResult OnPostSaveScore()
        {
            if (CurrentScore <= 0)
            {
                return RedirectToPage();
            }

            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                BestScore = GetBestScore();
                GameOver = true;
                ResultMessage = "Please enter a name or cancel.";
                return Page();
            }

            var score = new Score
            {
                PlayerName = PlayerName,
                Game = "Arithmetic",
                Value = CurrentScore,
                CreatedAt = DateTime.UtcNow
            };

            _db.Scores.Add(score);
            _db.SaveChanges();

            return RedirectToPage("/Scoreboard", new { SelectedGame = "Arithmetic" });
        }

        void GenerateQuestion()
        {
            var operations = new[] { "+", "-", "*", "/" };
            Operation = operations[_random.Next(operations.Length)];

            if (Operation == "/")
            {
                Number2 = _random.Next(1, 10);
                var result = _random.Next(1, 11);
                Number1 = Number2 * result;
            }
            else
            {
                Number1 = _random.Next(1, 21);
                Number2 = _random.Next(1, 21);
            }

            UserAnswer = null;
        }

        int Calculate(int a, int b, string op)
        {
            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                _ => a + b
            };
        }

        int GetBestScore()
        {
            return _db.Scores
                .Where(s => s.Game == "Arithmetic")
                .OrderByDescending(s => s.Value)
                .Select(s => s.Value)
                .FirstOrDefault();
        }
    }
}
