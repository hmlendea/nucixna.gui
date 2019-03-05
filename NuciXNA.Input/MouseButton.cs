using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciXNA.Input
{
    /// <summary>
    /// Button state.
    /// </summary>
    public sealed class MouseButton : IEquatable<MouseButton>
    {
        static IDictionary<int, MouseButton> entries =
            new Dictionary<int, MouseButton>
            {
                { Left.Id, Left },
                { Right.Id, Right },
                { Middle.Id, Middle },
                { Back.Id, Back },
                { Forward.Id, Forward }
            };

        /// <summary>
        /// The left mouse button.
        /// </summary>
        public static MouseButton Left => new MouseButton(1, nameof(Left), true);

        /// <summary>
        /// The right mouse button.
        /// </summary>
        public static MouseButton Right => new MouseButton(1, nameof(Right), false);

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        public static MouseButton Middle => new MouseButton(3, nameof(Middle), true);

        /// <summary>
        /// The back mouse button.
        /// </summary>
        public static MouseButton Back => new MouseButton(4, nameof(Back), true);

        /// <summary>
        /// The forward mouse button.
        /// </summary>
        public static MouseButton Forward => new MouseButton(5, nameof(Forward), true);

        public int Id { get; }

        public string Name { get; }

        public bool IsDown { get; }

        private MouseButton(int id, string name, bool isDown)
        {
            Id = id;
            Name = name;
            IsDown = isDown;
        }

        public static MouseButton FromId(int id)
            => entries[id];

        public static MouseButton FromName(string name)
            => entries.Values.First(x => x.Name == name);

        public bool Equals(MouseButton other)
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
            
            return Equals(obj as MouseButton);
        }

        public override string ToString()
            => Name;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static IEnumerable<MouseButton> GetValues()
            => entries.Values.ToList();

        public static implicit operator int(MouseButton me)
            => me.Id;

        public static implicit operator string(MouseButton me)
            => me.ToString();

        public static bool operator ==(MouseButton me, MouseButton other)
        {
            if (me is null)
            {
                return other is null;
            }

            return me.Equals(other);
        }

        public static bool operator !=(MouseButton me, MouseButton other)
            => !(me == other);
    }
}
