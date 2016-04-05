using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolbox.CLI
{
    internal class InputHistory
    {
        internal enum Direction
        {
            Up,
            Down
        }

        private readonly Dictionary<Mode, List<string>> _history = new Dictionary<Mode, List<string>>();
        private int _index;

        public void Push(Mode mode, string input)
        {
            if (!this._history.ContainsKey(mode))
                this._history.Add(mode, new List<string>());
            this._history[mode].Add(input);
        }

        public string Get(Mode mode, Direction direction)
        {
            if (!this._history.ContainsKey(mode))
                return string.Empty;
            switch (direction)
            {
                case Direction.Up:
                    if (this._index > 0)
                        this._index--;
                    break;
                case Direction.Down:
                    if (this._index < this._history.Count - 1)
                        this._index++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            return this._history[mode][this._index];
        }

        public string Last(Mode mode)
        {
            if (!this._history.ContainsKey(mode))
                return string.Empty;
            return this._history[mode].LastOrDefault();
        }
    }
}
