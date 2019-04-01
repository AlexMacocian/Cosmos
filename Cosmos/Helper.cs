using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos
{
    class Helper
    {
        public static Color HSVtoRGB(float hue, float saturation, float value, float alpha)
        {
            if (hue > 1 || saturation > 1 || value > 1) throw new Exception("values cannot be more than 1!");
            if (hue < 0 || saturation < 0 || value < 0) throw new Exception("values cannot be less than 0!");

            Color output = new Color();
            if (Math.Abs(saturation) < 0.001)
            {
                output.R = (byte)(value * byte.MaxValue);
                output.G = (byte)(value * byte.MaxValue);
                output.B = (byte)(value * byte.MaxValue);
            }
            else
            {
                hue = hue / 60f;
                float f = hue - (int)hue;
                float p = value * (1f - saturation);
                float q = value * (1f - saturation * f);
                float t = value * (1f - saturation * (1f - f));
                switch ((int)hue)
                {
                    case (0):
                        output = new Color(value * 255, t * 255, p * 255, alpha);
                        break;
                    case (1):
                        output = new Color(q * 255, value * 255, p * 255, alpha);
                        break;
                    case (2):
                        output = new Color(p * 255, value * 255, t * 255, alpha);
                        break;
                    case (3):
                        output = new Color(p * 255, q * 255, value * 255, alpha);
                        break;
                    case (4):
                        output = new Color(t * 255, p * 255, value * 255, alpha);
                        break;
                    case (5):
                        output = new Color(value * 255, p * 255, q * 255, alpha);
                        break;
                    default:
                        throw new Exception("RGB color unknown!");
                }

            }
            return output;
        }

        public static Color HSLtoRGB(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    double temp2;
                    if (l < 0.5)
                        temp2 = l * (1.0 + s);
                    else
                        temp2 = l + s - (l * s);

                    double temp1 = 2.0 * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
                }
            }
            return new Color((int)(255 * r), (int)(255 * g), (int)(255 * b));

        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }

        public static Color GetColorByDepth(double depth)
        {
            return Color.Lerp(Color.Red, Color.Blue, (float)(depth / 66));
        }

        private static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
        {
            int stepA = ((end.A - start.A) / (steps - 1));
            int stepR = ((end.R - start.R) / (steps - 1));
            int stepG = ((end.G - start.G) / (steps - 1));
            int stepB = ((end.B - start.B) / (steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return new Color(start.A + (stepA * i),
                                            start.R + (stepR * i),
                                            start.G + (stepG * i),
                                            start.B + (stepB * i));
            }
        }
    }
}
