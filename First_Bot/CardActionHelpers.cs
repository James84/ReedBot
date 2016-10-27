using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace First_Bot
{
    public static class CardActionHelpers
    {
        public static HeroCard BuildHeroCard(IList<CardAction> buttons = null, IList<CardImage> images = null, string title = "", string subTitle = "")
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subTitle
            };

            if (buttons != null)
            {
                heroCard.Buttons = new List<CardAction>(buttons);
            }

            if (images != null)
            {
                heroCard.Images = new List<CardImage>(images);
            }

            return heroCard;
        }

        public static ThumbnailCard BuildTumbnailCard(IList<CardAction> buttons = null, IList<CardImage> images = null, string title = "", string subTitle = "")
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subTitle
            };

            if (buttons != null)
            {
                thumbnailCard.Buttons = new List<CardAction>(buttons);
            }

            if (images != null)
            {
                thumbnailCard.Images = new List<CardImage>(images);
            }

            return thumbnailCard;
        }

        public static CardAction BuildCardAction(string type = null, string title = null, string value = null, string image = null)
        {
            return new CardAction
            {
                Type = type,
                Title = title,
                Value = value,
                Image = image
            };
        }

        public static CardImage BuildCardImage(string alt = null, string tap = null, string url = null)
        {
            return new CardImage
            {
                Alt = alt,
                Url = url
            };
        }
    }
}