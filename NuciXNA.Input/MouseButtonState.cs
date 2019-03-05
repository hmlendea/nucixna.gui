using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciXNA.Input
{
    /// <summary>
    /// Mouse button state.
    /// </summary>
    public sealed class MouseButtonState : IEquatable<MouseButtonState>
    {
        static IDictionary<int, MouseButtonState> entries =
            new Dictionary<int, MouseButtonState>
            {
                { Pressed.Id, Pressed },
                { Released.Id, Released },
                { Down.Id, Down },
                { Up.Id, Up }
            };

        /// <summary>
        /// The mouse button was just pressed.
        /// </summary>
        public static MouseButtonState Pressed => new MouseButtonState(1, nameof(Pressed), true);

        /// <summary>
        /// They mouse button was just released.
        /// </summary>
        public static MouseButtonState Released => new MouseButtonState(1, nameof(Pressed), false);

        /// <summary>
        /// The mouse button is down.
        /// </summary>
        public static MouseButtonState Down => new MouseButtonState(3, nameof(Up), true);

        /// <summary>
        /// The mouse button is up.
        /// </summary>
        public static MouseButtonState Up => new MouseButtonState(3, nameof(Up), false);

        public int Id { get; }

        public string Name { get; }

        public bool IsDown { get; }

        private MouseButtonState(int id, string name, bool isDown)
        {
            Id = id;
            Name = name;
            IsDown = isDown;
        }

        public MouseButtonState FromId(int id)
            => entries[id];

        public MouseButtonState FromName(string name)
            => entries.Values.First(x => x.Name == name);

        public bool Equals(MouseButtonState other)
        {
            if (other is null)
            {
                return false;
            }

            if (!other.Id.Equals(Id))
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            
            return Equals(obj as MouseButtonState);
        }

        public override string ToString()
            => Name;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static IEnumerable<MouseButtonState> GetValues()
            => entries.Values.ToList();

        public static implicit operator int(MouseButtonState me)
            => me.Id;

        public static implicit operator string(MouseButtonState me)
            => me.ToString();

        public static bool operator ==(MouseButtonState me, MouseButtonState other)
        {
            if (me is null)
            {
                return other is null;
            }

            return me.Equals(other);
        }

        public static bool operator !=(MouseButtonState me, MouseButtonState other)
            => !(me == other);
    }
}
