namespace CRT
{
    public struct Lightsource
    {
        public Vec3 position;
        public Color color;
        public double intensity;
        public Lightsource(Vec3 position_, Color color_, double intensity_ = 100.0)
        {
            this.position = position_;
            this.color = color_;
            this.intensity = intensity_;
        }
    }
}