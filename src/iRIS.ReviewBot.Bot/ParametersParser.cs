using System;
using System.Linq;

namespace iRIS.ReviewBot.Bot
{
    using iRIS.ReviewBot.Bot.Entities;

    public class ParametersParser : IParametersParser
    {
        private const string TeamTag = "-t";
        private const int DefaultCount = 1;

        public NameAndTeamParameters ParseNameAndTeam(string text)
        {
            text = text?.Trim() ?? string.Empty;

            var name = string.Empty;
            var team = string.Empty;
            var words = text.Split(' ');
            var hasTeamTag = words.Any(w => string.Equals(w, TeamTag, StringComparison.OrdinalIgnoreCase));

            if (hasTeamTag)
            {
                (name, team) = ReadNameAndTeam(words);
            }
            else
            {
                name = text;
            }

            return new NameAndTeamParameters(name, team);
        }

        public CountAndTeamParameters ParseCountAndTeam(string text)
        {
            text = text?.Trim() ?? string.Empty;

            var words = text.Split(' ');

            (var count, var team) = ReadCountAndTeam(words);

            return new CountAndTeamParameters(count, team);
        }

        private (string name, string team) ReadNameAndTeam(string[] words)
        {
            var countOfTeamTag = words.Count(w => string.Equals(w, TeamTag, StringComparison.OrdinalIgnoreCase));

            if(countOfTeamTag > 1)
            {
                throw new Exception($"Кол-во параметров '{TeamTag}' больше одного");
            }

            var indexOfTeamTag = GetIndexOfTeamTag(words);
            var name = string.Join(" ", words.Take(indexOfTeamTag)).Trim();
            var team = string.Join(" ", words.Skip(indexOfTeamTag + 1)).Trim();

            return (name, team);
        }

        private (int count, string team) ReadCountAndTeam(string[] words)
        {
            var countOfTeamTag = words.Count(w => string.Equals(w, TeamTag, StringComparison.OrdinalIgnoreCase));
            var countStr = string.Empty;
            var team = string.Empty;

            if (countOfTeamTag > 1)
            {
                throw new Exception($"Кол-во параметров '{TeamTag}' больше одного");
            }

            if (countOfTeamTag != 0)
            {
                var indexOfTeamTag = GetIndexOfTeamTag(words);

                team = string.Join(" ", words.Skip(indexOfTeamTag + 1)).Trim();
                countStr = string.Join(" ", words.Take(indexOfTeamTag)).Trim();
            }
            else
            {
                countStr = string.Join(string.Empty, words);
            }

            if (!int.TryParse(countStr, out var count) || count < 1)
            {
                count = DefaultCount;
            }

            return (count, team);
        }

        private int GetIndexOfTeamTag(string[] words)
        {
            var index = 0;

            while(!string.Equals(words[index], TeamTag, StringComparison.OrdinalIgnoreCase))
            {
                index++;
            }

            return index;
        }
    }
}
