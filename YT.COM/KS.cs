using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT.COM
{
    public class KS
    {
        #region 속성
        public double Size { get; set; }
        public double Diameter { get; set; }
        public double Radius { get; set; }
        #endregion

        #region 생성자
        public KS()
        {
        }

        public KS(double size, double diameter, double radius)
        {
            this.Size = size;
            this.Diameter = diameter;
            this.Radius = radius;
        }
        #endregion

        #region PUBLIC

        public static double GetDiameter(double item)
        {
            var a = new KS().KSMainbar().Where(x => x.Size.Equals(item)).FirstOrDefault();
            return a == null ? 9.53 : a.Diameter;
        }
        public static double GetRadius(double item)
        {
            var a = new KS().KSMainbar().Where(x => x.Size.Equals(item)).FirstOrDefault();
            return a == null ? 30 : a.Radius;
        }
        #endregion

        #region PRIVATE
        private List<KS> KSMainbar() 
        {
            var n = new List<KS>();
            n.Add(new KS(10, 9.53, 30.00));
            n.Add(new KS(13, 12.7, 40.00));
            n.Add(new KS(16, 15.9, 50.00));
            n.Add(new KS(19, 19.1, 60.00));
            n.Add(new KS(22, 22.2, 70.00));
            n.Add(new KS(25, 25.4, 80.00));
            n.Add(new KS(29, 28.6, 120.00));
            n.Add(new KS(32, 31.8, 130.00));
            n.Add(new KS(35, 34.9, 140.00));
            n.Add(new KS(38, 38.1, 200.00));
            n.Add(new KS(41, 41.3, 210.00));
            n.Add(new KS(51, 50.8, 260.00));
            n.Add(new KS(57, 57.3, 290.00));

            return n;
        } 
        #endregion

    }
}
