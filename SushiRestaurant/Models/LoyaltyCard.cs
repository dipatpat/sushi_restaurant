using System;

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
        public Guest Owner { get; }
        public string EmailAddress { get; private set; }
        public LoyaltyType LoyaltyType { get; private set; }
        public int NumberOfPoints { get; private set; }

        public LoyaltyCard(Guest owner, string email, LoyaltyType type, int points)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (points < 0)
                throw new ArgumentOutOfRangeException(nameof(points));

            EmailAddress = email;
            LoyaltyType = type;
            NumberOfPoints = points;
            owner.LoyaltyCard = this;
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
    }
}