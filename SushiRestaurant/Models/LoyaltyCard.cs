using System;
using System.Collections.Generic; // Added for List (Extent)

namespace SushiRestaurant.Models
{
    public enum LoyaltyType
    {
        standard,
        gold,
        vip
    }

    public class LoyaltyCard
    {
        private static readonly List<LoyaltyCard> _extent = new();
        public static IReadOnlyList<LoyaltyCard> Extent => _extent.AsReadOnly();

        public static void ClearExtent() => _extent.Clear();

        internal static void SetExtent(List<LoyaltyCard>? items)
        {
            _extent.Clear();
            if (items is { Count: > 0 })
                _extent.AddRange(items);
        }

        private Guest? _owner;
        public Guest Owner => _owner ?? throw new InvalidOperationException("Association no longer valid (Guest removed)");

        public string EmailAddress { get; private set; }
        public LoyaltyType LoyaltyType { get; private set; }
        public int NumberOfPoints { get; private set; }

        public LoyaltyCard(Guest owner, string email, LoyaltyType type, int points)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (points < 0)
                throw new ArgumentOutOfRangeException(nameof(points));

            _owner = owner;
            EmailAddress = email;
            LoyaltyType = type;
            NumberOfPoints = points;

            owner.InternalSetLoyaltyCard(this);
            _extent.Add(this);
        }

        public void ChangeNumberOfPoints(int delta)
        {
            if (NumberOfPoints + delta < 0)
                throw new InvalidOperationException("Points cannot be negative.");

            NumberOfPoints += delta;
        }

        public void UpgradeTier()
        {
            if (LoyaltyType == LoyaltyType.vip)
                throw new InvalidOperationException("Already at highest tier.");

            LoyaltyType++;
        }

        public void RemoveCompletely()
        {
            if (_owner != null)
            {
                _owner.InternalRemoveLoyaltyCard(this);
                _owner = null;
            }
            
            _extent.Remove(this);
        }
    }
}