using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Drawing
{
    class ColorMap
    {
        private List<Tuple<float, Color>> colorValues;

        public ColorMap()
        {
            colorValues = new List<Tuple<float, Color>>();
        }

        public void AddColorValue(float value, Color color)
        {
            for(int i = 0; i < colorValues.Count; i++)
            {
                var colorVal = colorValues[i];
                if(value < colorVal.Item1)
                {
                    colorValues.Insert(i, new Tuple<float, Color>(value, color));
                    return;
                }
            }
            colorValues.Add(new Tuple<float, Color>(value, color));
        }

        public Color GetColor(float value)
        {
            if(colorValues.Count == 0)
            {
                return Color.Transparent;
            }
            foreach(var colorVal in colorValues)
            {
                if(value < colorVal.Item1)
                {
                    return colorVal.Item2;
                }
            }
            return colorValues[colorValues.Count - 1].Item2;
        }
    }
}
