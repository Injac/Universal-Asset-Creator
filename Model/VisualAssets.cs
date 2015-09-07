using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum VisualAssets
    {
        [ImageData(284, 284, 127, 126, PlacmentType.Center)] Square71x71LogoScale400,
        [ImageData(142, 142, 62, 61, PlacmentType.Center)] Square71x71LogoScale200,
        [ImageData(71, 71, 31, 30, PlacmentType.Center)] Square71x71LogoScale100,
        [ImageData(107, 107, 47, 46, PlacmentType.Center)] Square71x71LogoScale150,
        [ImageData(89, 89, 39, 38, PlacmentType.Center)] Square71x71LogoScale125,

        [ImageData(600, 600, 384, 384, PlacmentType.Center)] Square150x150LogoScale400,
        [ImageData(300, 300, 84, 96, PlacmentType.Center)] Square150x150LogoScale200,
        [ImageData(150, 150, 44, 45, PlacmentType.Center)] Square150x150LogoScale100,
        [ImageData(225, 225, 80, 80, PlacmentType.Center)] Square150x150LogoScale150,
        [ImageData(188, 188, 55, 64, PlacmentType.Center)] Square150x150LogoScale125,

        [ImageData(1240, 600, 192, 192, PlacmentType.Center)] Wide310X150LogoScale400,
        [ImageData(620, 300, 90, 108, PlacmentType.Center)] Wide310X150LogoScale200,
        [ImageData(310, 150, 45, 47, PlacmentType.Center)] Wide310X150LogoScale100,
        [ImageData(465, 225, 80, 80, PlacmentType.Center)] Wide310X150LogoScale150,
        [ImageData(388, 188, 53, 60, PlacmentType.Center)] Wide310X150LogoScale125,

        [ImageData(1240, 1240, 416, 417, PlacmentType.Center)] Square310x310LogoScale400,
        [ImageData(620, 620, 192, 192, PlacmentType.Center)] Square310x310LogoScal200,
        [ImageData(310, 310, 96, 72, PlacmentType.Center)] Square310x310LogoScale100,
        [ImageData(465, 465, 127, 126, PlacmentType.Center)] Square310x310LogoScale150,
        [ImageData(388, 388, 113, 112, PlacmentType.Center)] Square310x310LogoScale125,

        [ImageData(176, 176, 113, 112, PlacmentType.Center)] Square44x44LogoScale400,
        [ImageData(88, 88, 56, 56, PlacmentType.Center)] Square44x44LogoScale200,
        [ImageData(44, 44, 28, 28, PlacmentType.Center)] Square44x44LogoScale100,
        [ImageData(66, 66, 43, 42, PlacmentType.Center)] Square44x44LogoScale150,
        [ImageData(55, 55, 36, 36, PlacmentType.Center)] Square44x44LogoScale125,


        [ImageData(176, 176, 113, 112, PlacmentType.Center)] Square44x44LogoScale400_altform,
        [ImageData(88, 88, 56, 56, PlacmentType.Center)] Square44x44LogoScale200_altform,
        [ImageData(44, 44, 28, 28, PlacmentType.Center)] Square44x44LogoScale100_altform,
        [ImageData(66, 66, 43, 42, PlacmentType.Center)] Square44x44LogoScale150_altform,
        [ImageData(55, 55, 36, 36, PlacmentType.Center)] Square44x44LogoScale125_altform,


        [ImageData(256, 256, 240, 240, PlacmentType.Center)] Square44x44LogoTargetSize256,
        [ImageData(48, 48, 42, 48, PlacmentType.Center)] Square44x44LogoTargetSize48,
        [ImageData(24, 24, 15, 16, PlacmentType.CenterHorizontally)] Square44x44LogoTargetSize24,
        [ImageData(16, 16, 15, 16, PlacmentType.Center)] Square44x44LogoTargetSize16,

        [ImageData(256, 256, 240, 240, PlacmentType.Center)] Square44x44LogoTargetSize256_altform,
        [ImageData(48, 48, 42, 48, PlacmentType.Center)] Square44x44LogoTargetSize48_altform,
        [ImageData(24, 24, 15, 16, PlacmentType.CenterHorizontally)] Square44x44LogoTargetSize24_altform,
        [ImageData(16, 16, 15, 16, PlacmentType.Center)] Square44x44LogoTargetSize16_altform,


        [ImageData(200, 200, 128, 128, PlacmentType.Center)] StoreLogoScale400,
        [ImageData(100, 100, 44, 44, PlacmentType.Center)] StoreLogoScale200,
        [ImageData(75, 75, 33, 33, PlacmentType.Center)] StoreLogoScale150,
        [ImageData(63, 63, 41, 41, PlacmentType.Center)] StoreLogoScale125,
        [ImageData(50, 50, 30, 30, PlacmentType.Center)] StoreLogoScale100,

        [ImageData(96, 96, 90, 72, PlacmentType.TopWidth1PxMargin)] BadgeLogoScale400,
        [ImageData(48, 48, 30, 30, PlacmentType.TopWidth1PxMargin)] BadgeLogoScale200,
        [ImageData(36, 36, 34, 34, PlacmentType.TopWidth1PxMargin)] BadgeLogoScale150,
        [ImageData(30, 30, 28, 28, PlacmentType.TopWidth1PxMargin)] BadgeLogoScale125,
        [ImageData(24, 24, 22, 23, PlacmentType.TopWidth1PxMargin)] BadgeLogoScale100,

        [ImageData(2480, 1200, 364, 364, PlacmentType.Center)] SplashScreenScale400,
        [ImageData(1240, 600, 169, 168, PlacmentType.Center)] SplashScreenScale200,
        [ImageData(930, 450, 127, 126, PlacmentType.Center)] SplashScreenScale150,
        [ImageData(775, 375, 113, 112, PlacmentType.Center)] SplashScreenScale125,
        [ImageData(620, 300, 85, 84, PlacmentType.Center)] SplashScreenScale100
    }


    public enum PlacmentType
    {
        Center,
        TopWidth1PxMargin,
        CenterHorizontally
    }


    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class ImageDataAttribute : Attribute
    {
        // This is a positional argument
        public ImageDataAttribute(int width, int height, int iconicWidth, int iconicHeight, PlacmentType placementType)
        {
            this.Height = height;
            this.Width = width;

            this.IconicWidth = iconicWidth;
            this.IconicHeight = iconicHeight;

            this.HowToPlace = placementType;
        }

        public int Width { get; }

        public int Height { get; }

        public int IconicWidth { get; }

        public int IconicHeight { get; }

        public PlacmentType HowToPlace { get; }
    }

    public static class EnumTools
    {
        public static TAttribute GetAttribute<TAttribute>(Enum value)
            where TAttribute : Attribute
        {
            return value
                .GetType()
                .GetMember(value.ToString())[0]
                .GetCustomAttribute<TAttribute>();
        }
    }
}