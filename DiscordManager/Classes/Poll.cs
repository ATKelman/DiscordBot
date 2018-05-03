using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordManager.Classes
{
    public class Poll
    {
        public string name { get; internal set; }
        public string[] options { get; internal set; }

        public string[] numbers = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        public string[] numbersEmote = new string[] { "0⃣", "1⃣", "2⃣", "3⃣", "4⃣", "5⃣", "6⃣", "7⃣", "8⃣", "9⃣" };
        public int[] votes = new int[10];

        public Poll(string pollName, string[] pollOptions)
        {
            name = pollName;
            options = pollOptions;
        }

        public bool IsValidOptionsAmount()
        {
            return 0 < options.Length && options.Length < 11;
        }

        public string GetNumberAsWord(int value)
        {
            return numbers[value];
        }

        public void AdjustVote(string vote, int score)
        {
            for(int i = 0; i < numbers.Length; i++)
            {
                if(vote.Equals(numbersEmote[i]))
                {
                    votes[i] += score;
                    return;
                }
            }
        }
    }
}
