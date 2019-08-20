using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace General.Model
{
    [DataContract]
    public class JsonRectangle : JsonObject
    {
        public JsonRectangle()
        {

        }

        public JsonRectangle(Rectangle objRectangle)
        {
            this.X = objRectangle.X;
            this.Y = objRectangle.Y;
            this.Width = objRectangle.Width;
            this.Height = objRectangle.Height;
        }

        public JsonRectangle(Point point, Size size)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Width = size.Width;
            this.Height = size.Height;
        }

        [DataMember]
        public int X { get; set; }
        [DataMember]
        public int Y { get; set; }
        [DataMember]
        public int Width { get; set; }
        [DataMember]
        public int Height { get; set; }

        public Rectangle ToRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

    }
}
