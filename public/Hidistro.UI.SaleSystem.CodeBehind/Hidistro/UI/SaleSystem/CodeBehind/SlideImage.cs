namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.Runtime.CompilerServices;

    public class SlideImage
    {
        public SlideImage(string imageUrl, string locationUrl)
        {
            this.ImageUrl = imageUrl;
            this.LoctionUrl = locationUrl;
        }

        public string ImageUrl { get; set; }

        public string LoctionUrl { get; set; }
    }
}

