using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciXNA.Input
{
    /// <summary>
    /// Button state.
    /// </summary>
    public sealed class ButtonState : IEquatable<ButtonState>
    {
        static IDictionary<int, ButtonState> entries =
            new Dictionary<int, ButtonState>
            {
                { Pressed.Id, Pressed },
                { Released.Id, Released },
                { Down.Id, Down },
                { Up.Id, Up }
            };

        /// <summary>
        /// The mouse button was just pressed.
        /// </summary>
        public static ButtonState Pressed => new ButtonState(1, nameof(Pressed), true);

        /// <summary>
        /// They mouse button was just released.
        /// </summary>
        public static ButtonState Released => new ButtonState(1, nameof(Pressed), false);

        /// <summary>
        /// The mouse button is down.
        /// </summary>
        public static ButtonState Down => new ButtonState(3, nameof(Up), true);

        /// <summary>
        /// The mouse button is up.
        /// </summary>
        public static ButtonState Up => new ButtonState(3, nameof(Up), false);

        public int Id { get; }

        public string Name { get; }

        public bool IsDown { get; }

        private ButtonState(int id, string name, bool isDown)
        {
            Id = id;
            Name = name;
            IsDown = isDown;
        }

        public static ButtonState FromId(int id)
            => entries[id];

        public static ButtonState FromName(string name)
            => entries.Values.First(x => x.Name == name);

        public bool Equals(ButtonState other)
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
            
            return Equals(obj as ButtonState);
        }

        public override string ToString()
            => Name;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static IEnumerable<ButtonState> GetValues()
            => entries.Values.ToList();

        public static implicit operator int(ButtonState me)
            => me.Id;

        public static implicit operator string(ButtonState me)
            => me.ToString();

        public static bool operator ==(ButtonState me, ButtonState other)
        {
            if (me is null)
            {
                return other is null;
            }

            return me.Equals(other);
        }

        public static bool operator !=(ButtonState me, ButtonState other)
            => !(me == other);
    }
}
