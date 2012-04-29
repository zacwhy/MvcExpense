using System;

namespace MvcExpense
{
    public static class EnhancedMath
    {
        private delegate double RoundingFunction( double value );

        private enum RoundingDirection { Up, Down }

        public static double RoundUp( double value, int precision )
        {
            return Round( value, precision, RoundingDirection.Up );
        }

        public static double RoundDown( double value, int precision )
        {
            return Round( value, precision, RoundingDirection.Down );
        }

        private static double Round( double value, int precision, RoundingDirection roundingDirection )
        {
            RoundingFunction roundingFunction;
            if ( roundingDirection == RoundingDirection.Up )
                roundingFunction = Math.Ceiling;
            else
                roundingFunction = Math.Floor;
            value *= Math.Pow( 10, precision );
            value = roundingFunction( value );
            return value * Math.Pow( 10, -1 * precision );
        }
    }
}