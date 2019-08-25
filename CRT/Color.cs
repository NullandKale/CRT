namespace CRT
{
    public struct Color
    {
        public static Color red = new Color(255, 0, 0);
        public static Color green = new Color(0, 255, 0);
        public static Color blue = new Color(0, 0, 255);

        public double r, g, b;

        public Color(double red, double green, double blue)
        {
            r = red;
            g = green;
            b = blue;
        }

        public Color scale_by(double scalar)
        {
            return (scalar > 0) ? Color_trunc(scalar * r, scalar * g, scalar * b) : new Color(0, 0, 0);
        }

        public Color scale_by2(double scalar)
        {
            if (scalar < 0)
            {
                return new Color(0, 0, 0);
            }

            scalar *= scalar;

            return Color_trunc(scalar * r, scalar * g, scalar * b);
        }

        public Color mix_with(Color rhs)
        {
            return new Color(r * rhs.r, g * rhs.g, b * rhs.b);
        }

        public static Color operator *(Color c, double scalar)
        {
            return new Color(scalar * c.r, scalar * c.g, scalar * c.b);
        }

        public static Color operator +(Color a, Color rhs)
        {
            return Color_trunc(a.r + rhs.r, a.g + rhs.g, a.b + rhs.b);
        }

        public static double trunc(double val)
        {
            return (val > 255.0) ? 255.0 : val;
        }

        public Color GreyScale()
        {
            double average = (r + b + g) / 3.0;

            return new Color(average, average, average);
        }

        public static Color Color_trunc(double red, double green, double blue)
        {
            return new Color(trunc(red), trunc(green), trunc(blue));
        }
    }
}