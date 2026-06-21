using System;

namespace Modules.SharedModule
{
    [Serializable]
    public struct BorderRectangle
    {
        public readonly float LeftBorderPosition, RightBorderPosition, TopBorderPosition, BottomBorderPosition;

        public BorderRectangle(float leftBorderPosition, float rightBorderPosition, float topBorderPosition, float bottomBorderPosition)
        {
            LeftBorderPosition = leftBorderPosition;
            RightBorderPosition = rightBorderPosition;
            TopBorderPosition = topBorderPosition;
            BottomBorderPosition = bottomBorderPosition;
        }
    }
}