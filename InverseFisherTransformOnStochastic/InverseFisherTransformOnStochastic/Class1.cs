using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;

namespace InverseFisherTransformOnStochastic
{
    public class IftsHesapla
    {
        public static List<double> Hesapla(List<IOhlcv> barListesi, int stochUzunlugu, int smoothingUzunlugu)
        {
            ////@version=3
            //// author: KIVANC @fr3762 on twitter
            //// creator John EHLERS
            ////
            //study("Inverse Fisher Transform on STOCHASTIC", shorttitle = "IFTSTOCH")
            //stochlength = input(5, "STOCH Length")
            //wmalength = input(9, title = "Smoothing length")
            //v1 = 0.1 * (stoch(close, high, low, stochlength) - 50)
            //v2 = wma(v1, wmalength)
            //INV = (exp(2 * v2) - 1) / (exp(2 * v2) + 1)
            //plot(INV, color = blue, linewidth = 2)
            //hline(0.5, color = red)
            //hline(-0.5, color = green)

            if (barListesi.Count > 360)
            {
                //stoch(close, high, low, stochlength=5)
                var calcStoch = new Stochastics.Fast(barListesi, stochUzunlugu, stochUzunlugu).Compute();

                //v1 = 0.1 * (stoch(close, high, low, stochlength=5) - 50)
                List<decimal> list = (from x in calcStoch where x.Tick.K != null select Convert.ToDecimal(0.1 * ((double)x.Tick.K - 50))).ToList();
                List<decimal?> v1 = list.ConvertAll<decimal?>(x => x);

                //v2 = wma(v1, wmalength=9)
                var v2 = new WeightedMovingAverageByTuple(v1, smoothingUzunlugu).Compute();

                //INV = (exp(2 * v2) - 1) / (exp(2 * v2) + 1)
                List<double> inv = (from x in v2 select (Math.Exp((double)(2 * x ?? 1)) - 1) / (Math.Exp((double)(2 * x ?? 1)) + 1)).ToList();
                
                inv.RemoveRange(0, 140);
                return inv;
            }

            return null;
        }
    }
}
