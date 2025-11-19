using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace neuron_gym.Pages.Games
{
    public class ArithmeticModel : PageModel
    {
        private readonly Random _random = new Random();

        [BindProperty]
        public int Number1 { get; set; }

        [BindProperty]
        public int Number2 { get; set; }

        [BindProperty]
        public string Operation { get; set; } = "+";

        [BindProperty]
        public int? UserAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public string? ResultMessage { get; set; }

        public void OnGet()
        {
            GenerateQuestion();
        }

        public void OnPost()
        {
            CorrectAnswer = Calculate(Number1, Number2, Operation);

            if (!UserAnswer.HasValue)
            {
                ResultMessage = "Please enter an answer.";
                return;
            }

            if (UserAnswer.Value == CorrectAnswer)
            {
                ResultMessage = "Correct!";
            }
            else
            {
                ResultMessage = $"Incorrect. The correct answer was {CorrectAnswer}.";
            }
        }

        void GenerateQuestion()
        {
            var operations = new[] { "+", "-", "*", "/" };
            Operation = operations[_random.Next(operations.Length)];

            if (Operation == "/")
            {
                Number2 = _random.Next(1, 10);
                CorrectAnswer = _random.Next(1, 11);
                Number1 = Number2 * CorrectAnswer;
            }
            else
            {
                Number1 = _random.Next(1, 21);
                Number2 = _random.Next(1, 21);
                CorrectAnswer = Calculate(Number1, Number2, Operation);
            }
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
    }
}
